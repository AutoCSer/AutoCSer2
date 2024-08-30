using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息节点消费者
    /// </summary>
    public abstract class MessageConsumer
    {
        /// <summary>
        /// 默认重试间隔毫秒数
        /// </summary>
        public const int DefaultDelayMilliseconds = 1000;

        /// <summary>
        /// 客户端
        /// </summary>
        protected readonly CommandClient commandClient;
        /// <summary>
        /// 重试间隔毫秒数
        /// </summary>
        protected readonly int delayMilliseconds;
        /// <summary>
        /// 是否已释放资源
        /// </summary>
        protected bool isDisposed;
        /// <summary>
        /// 消息节点消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数</param>
        protected MessageConsumer(CommandClient commandClient, int delayMilliseconds)
        {
            this.commandClient = commandClient;
            this.delayMilliseconds = Math.Max(delayMilliseconds, 1);
        }

    }
    /// <summary>
    /// 消息节点消费者
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    public abstract class MessageConsumer<T> : MessageConsumer, IDisposable
        where T : Message<T>
    {
        /// <summary>
        /// 消息客户端节点
        /// </summary>
        protected readonly IMessageNodeClientNode<T> node;
        /// <summary>
        /// 接收消息的最后一次错误信息
        /// </summary>
        protected ResponseResult<T> lastError;
        /// <summary>
        /// 保持回调输出
        /// </summary>
        private KeepCallbackResponse<T> keepCallback;
        /// <summary>
        /// 字符串消息消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">消息客户端节点</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数</param>
        protected MessageConsumer(CommandClient commandClient, IMessageNodeClientNode<T> node, int delayMilliseconds) : base(commandClient, delayMilliseconds)
        {
            this.node = node;
            lastError = null;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                keepCallback?.Dispose();
            }
        }
        /// <summary>
        /// 接收消息错误处理
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual async Task onError(ResponseResult<T> error)
        {
            if (!isDisposed && !commandClient.IsDisposed)
            {
                if (error.ReturnType != lastError.ReturnType || error.CallState != lastError.CallState)
                {
                    lastError = error;
                    await AutoCSer.LogHelper.Error($"字符串消息节点 {((ClientNode)node).Key} 接收消息失败，RPC 通讯状态为 {error.ReturnType} {error.ErrorMessage}，服务端节点调用状态为 {error.CallState}");
                }
            }
        }
        /// <summary>
        /// 开始接收并处理消息
        /// </summary>
        /// <param name="maxCallbackCount">单个连接最大并发数量</param>
        /// <returns></returns>
        public virtual async Task Start(int maxCallbackCount)
        {
            do
            {
                using (keepCallback = await node.GetMessage(maxCallbackCount))
                {
                    if (keepCallback.IsSuccess)
                    {
#if DotNet45 || NetStandard2
                        while (await keepCallback.MoveNextAsync())
                        {
                            ResponseResult<T> message = keepCallback.Current;
                            if (message.IsSuccess)
                            {
                                if (message.Value != null) checkOnMessage(message.Value).NotWait();
                            }
                            else
                            {
                                await onError(message);
                                break;
                            }
                        }
#else
                        await foreach (ResponseResult<T> message in keepCallback.GetAsyncEnumerable())
                        {
                            if (message.IsSuccess)
                            {
                                if (message.Value != null) checkOnMessage(message.Value).NotWait();
                            }
                            else
                            {
                                await onError(message);
                                break;
                            }
                        }
#endif
                    }
                }
                if (isDisposed || commandClient.IsDisposed) return;
                await Task.Delay(delayMilliseconds);
            }
            while (!isDisposed && !commandClient.IsDisposed);
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual async Task checkOnMessage(T message)
        {
            bool isMessage = false;
            try
            {
                await onMessage(message);
                isMessage = true;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (isMessage) await node.Completed(message.MessageIdeneity);
                else await node.Failed(message.MessageIdeneity);
            }
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task onMessage(T message);
    }
}
