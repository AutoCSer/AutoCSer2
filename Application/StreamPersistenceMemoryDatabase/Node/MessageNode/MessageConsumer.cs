using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

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
        protected readonly ICommandClient commandClient;
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
        protected MessageConsumer(ICommandClient commandClient, int delayMilliseconds)
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
        /// 消息处理委托
        /// </summary>
        private readonly Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> onMessageHandle;
        /// <summary>
        /// 服务端单次最大回调消息数量
        /// </summary>
        private readonly int maxMessageCount;
        /// <summary>
        /// 接收消息的最后一次错误信息
        /// </summary>
        protected ResponseResult lastError;
        /// <summary>
        /// 保持回调输出
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? keepCallback;
#else
        private CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// 字符串消息消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">消息客户端节点</param>
        /// <param name="maxMessageCount">服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数</param>
        protected MessageConsumer(ICommandClient commandClient, IMessageNodeClientNode<T> node, int maxMessageCount, int delayMilliseconds) : base(commandClient, delayMilliseconds)
        {
            this.node = node;
            this.maxMessageCount = maxMessageCount;
            lastError.Set(CommandClientReturnTypeEnum.Success, CallStateEnum.Success, null);
            onMessageHandle = onMessage;
            start().NotWait();
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
        /// 开始接收并处理消息
        /// </summary>
        /// <returns></returns>
        private async ValueTask start() 
        {
            try
            {
                do
                {
                    keepCallback = await node.GetMessage(maxMessageCount, onMessageHandle);
                    if (keepCallback != null || isDisposed || commandClient.IsDisposed) return;
                    await Task.Delay(delayMilliseconds);
                }
                while (!isDisposed && !commandClient.IsDisposed);
            }
            catch(Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception);
            }
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <param name="command"></param>
        private void onMessage(ResponseResult<T> message, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (message.IsSuccess)
            {
                if (message.Value != null) checkOnMessage(message.Value).NotWait();
                return;
            }
            onError(message, command).NotWait();
        }
        /// <summary>
        /// 错误消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task onError(ResponseResult<T> message, AutoCSer.Net.KeepCallbackCommand command)
        {
            try
            {
                await onError(message);
            }
            finally
            {
                while (keepCallback == null) await Task.Delay(1);
                if (object.ReferenceEquals(keepCallback.Command, command))
                {
                    CommandKeepCallback keepCallback = this.keepCallback;
                    this.keepCallback = null;
                    keepCallback.Close();
                    if (!isDisposed && !commandClient.IsDisposed) await start();
                }
            }
        }
        /// <summary>
        /// 接收消息错误处理
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual Task onError(ResponseResult<T> error)
        {
            if (!isDisposed && !commandClient.IsDisposed)
            {
                if (error.ReturnType != lastError.ReturnType || error.CallState != lastError.CallState)
                {
                    lastError.Set(error.ReturnType, error.CallState, error.ErrorMessage);
                    return AutoCSer.LogHelper.Error($"消息节点 {((ClientNode)node).Key} 接收消息失败，RPC 通讯状态为 {error.ReturnType} {error.ErrorMessage}，服务端节点调用状态为 {error.CallState}");
                }
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task checkOnMessage(T message)
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
                if (isMessage) await node.Completed(message.MessageIdeneity);
                else await node.Failed(message.MessageIdeneity);
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
