using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Message node consumer
    /// 消息节点消费者
    /// </summary>
    public abstract class MessageConsumer
    {
        /// <summary>
        /// The default retry interval is in milliseconds
        /// 默认重试间隔毫秒数
        /// </summary>
        public const int DefaultDelayMilliseconds = 1000;

        /// <summary>
        /// Command client
        /// </summary>
        protected readonly CommandClient commandClient;
        /// <summary>
        /// Retry interval in milliseconds
        /// 重试间隔毫秒数
        /// </summary>
        protected readonly int delayMilliseconds;
        /// <summary>
        /// Whether resources have been released
        /// 是否已释放资源
        /// </summary>
        protected bool isDisposed;
        /// <summary>
        /// Message node consumer
        /// 消息节点消费者
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="delayMilliseconds">Retry interval in milliseconds
        /// 重试间隔毫秒数</param>
        protected MessageConsumer(CommandClient commandClient, int delayMilliseconds)
        {
            this.commandClient = commandClient;
            this.delayMilliseconds = Math.Max(delayMilliseconds, 1);
        }
    }
    /// <summary>
    /// Message node consumer
    /// 消息节点消费者
    /// </summary>
    /// <typeparam name="T">Message data type
    /// 消息数据类型</typeparam>
    public abstract class MessageConsumer<T> : MessageConsumer, IDisposable
        where T : Message<T>
    {
        /// <summary>
        /// Message client node
        /// 消息客户端节点
        /// </summary>
        protected readonly IMessageNodeClientNode<T> node;
        /// <summary>
        /// Message processing delegate
        /// 消息处理委托
        /// </summary>
        private readonly Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> onMessageHandle;
        /// <summary>
        /// The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量
        /// </summary>
        private readonly int maxMessageCount;
        /// <summary>
        /// Receive the last error status information of the message
        /// 接收消息的最后一次错误状态信息
        /// </summary>
        protected ResponseResult lastError;
        /// <summary>
        /// Keep callback object of the command
        /// 命令保持回调对象
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? keepCallback;
#else
        private CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// Message node consumer
        /// 消息节点消费者
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Message client node
        /// 消息客户端节点</param>
        /// <param name="maxMessageCount">The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">Retry interval in milliseconds
        /// 重试间隔毫秒数</param>
        protected MessageConsumer(CommandClient commandClient, IMessageNodeClientNode<T> node, int maxMessageCount, int delayMilliseconds) : base(commandClient, delayMilliseconds)
        {
            this.node = node;
            this.maxMessageCount = maxMessageCount;
            lastError.Set(CommandClientReturnTypeEnum.Success, CallStateEnum.Success, null);
            onMessageHandle = onMessage;
            start().AutoCSerNotWait();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                keepCallback?.Close();
            }
        }
        /// <summary>
        /// Start receiving and processing messages
        /// 开始接收并处理消息
        /// </summary>
        /// <returns></returns>
        private async Task start() 
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
        /// Message processing
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <param name="command"></param>
        private void onMessage(ResponseResult<T> message, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (message.IsSuccess)
            {
                if (message.Value != null) checkOnMessage(message.Value).AutoCSerNotWait();
                return;
            }
            onError(message, command).AutoCSerNotWait();
        }
        /// <summary>
        /// Handle error messages
        /// 处理错误消息
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
        /// Error handling of received messages
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
        /// Message processing
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
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
    }
}
