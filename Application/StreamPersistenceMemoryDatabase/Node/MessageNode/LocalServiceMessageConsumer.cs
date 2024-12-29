using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息节点消费者
    /// </summary>
    public abstract class LocalServiceMessageConsumer
    {
        /// <summary>
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        protected readonly LocalService service;
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
        /// <param name="client">日志流持久化内存数据库本地服务客户端</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数</param>
        protected LocalServiceMessageConsumer(LocalClient client, int delayMilliseconds)
        {
            this.service = client.Service;
            this.delayMilliseconds = Math.Max(delayMilliseconds, 1);
        }
    }
    /// <summary>
    /// 消息节点消费者
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    public abstract class LocalServiceMessageConsumer<T> : LocalServiceMessageConsumer, IDisposable
        where T : Message<T>
    {
        /// <summary>
        /// 消息客户端节点
        /// </summary>
        protected readonly IMessageNodeLocalClientNode<T> node;
        /// <summary>
        /// 接收消息的最后一次错误信息
        /// </summary>
        protected ResponseResult lastError;
        /// <summary>
        /// 保持回调输出
        /// </summary>
#if NetStandard21
        private KeepCallbackResponse<T>? keepCallback;
#else
        private KeepCallbackResponse<T> keepCallback;
#endif
        /// <summary>
        /// 字符串消息消费者
        /// </summary>
        /// <param name="client">日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">消息客户端节点</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<T> node, int delayMilliseconds = MessageConsumer.DefaultDelayMilliseconds) : base(client, delayMilliseconds)
        {
            this.node = node;
            lastError.Set(CommandClientReturnTypeEnum.Success, CallStateEnum.Success, null);
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
        protected virtual Task onError(ResponseResult<T> error)
        {
            if (!isDisposed && !service.IsDisposed)
            {
                if (error.ReturnType != lastError.ReturnType || error.CallState != lastError.CallState)
                {
                    lastError.Set(error.ReturnType, error.CallState, error.ErrorMessage);
                    return AutoCSer.LogHelper.Error($"字符串消息节点 {((ClientNode)node).Key} 接收消息失败，RPC 通讯状态为 {error.ReturnType} {error.ErrorMessage}，服务端节点调用状态为 {error.CallState}");
                }
            }
            return AutoCSer.Common.CompletedTask;
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
#if NetStandard21
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
#else
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
#endif
                }
                if (isDisposed || service.IsDisposed) return;
                await Task.Delay(delayMilliseconds);
            }
            while (!isDisposed && !service.IsDisposed);
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
                isMessage = await onMessage(message);
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (isMessage) node.Completed(message.MessageIdeneity);
                else node.Failed(message.MessageIdeneity);
            }
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
    }
}
