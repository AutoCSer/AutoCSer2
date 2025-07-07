using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Local service message node consumer
    /// 本地服务消息节点消费者
    /// </summary>
    public abstract class LocalServiceMessageConsumer
    {
#if AOT
        /// <summary>
        /// The default retry interval is in milliseconds
        /// 默认重试间隔毫秒数
        /// </summary>
        public const int DefaultDelayMilliseconds = 1000;
#endif

        /// <summary>
        /// Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        protected readonly LocalService service;
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
        /// Local service message node consumer
        /// 本地服务消息节点消费者
        /// </summary>
        /// <param name="client">Log stream persistence in-memory database local service client
        /// 日志流持久化内存数据库本地服务客户端</param>
        /// <param name="delayMilliseconds">Retry interval in milliseconds
        /// 重试间隔毫秒数</param>
        protected LocalServiceMessageConsumer(LocalClient client, int delayMilliseconds)
        {
            this.service = client.Service;
            this.delayMilliseconds = Math.Max(delayMilliseconds, 1);
        }
    }
#if AOT
    /// <summary>
    /// Local service message node consumer
    /// 本地服务消息节点消费者
    /// </summary>
    /// <typeparam name="T">Message data type
    /// 消息数据类型</typeparam>
    public abstract class LocalServiceMessageConsumer<T> : LocalServiceMessageConsumer, IDisposable
        where T : Message<T>
    {
        /// <summary>
        /// Local service client node
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode node;
        /// <summary>
        /// The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量
        /// </summary>
        protected readonly int maxMessageCount;
        /// <summary>
        /// Receive the last error status information of the message
        /// 接收消息的最后一次错误信息
        /// </summary>
        protected LocalResult lastError;
        /// <summary>
        /// Keep callback object of the command
        /// 命令保持回调对象
        /// </summary>
        protected IDisposable? keepCallback;
        /// <summary>
        /// Local service message node consumer
        /// 本地服务消息节点消费者
        /// </summary>
        /// <param name="client">Log stream persistence in-memory database local service client
        /// 日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="maxMessageCount">The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">The retry interval is in milliseconds, with a default of 1000 and a minimum value of 1
        /// 重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceMessageConsumer(LocalClient client, LocalClientNode node, int maxMessageCount, int delayMilliseconds = LocalServiceMessageConsumer.DefaultDelayMilliseconds) : base(client, delayMilliseconds)
        {
            this.node = node;
            this.maxMessageCount = maxMessageCount;
            lastError.Set(CallStateEnum.Success, null);
        }
        /// <summary>
        /// Release resources
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
        /// Message processing
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        protected void onMessage(LocalResult<T> message)
        {
            if (message.IsSuccess)
            {
                if (message.Value != null) checkOnMessage(message.Value).NotWait();
                return;
            }
            onError(message).NotWait();
        }
        /// <summary>
        /// Message processing
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task checkOnMessage(T message);
        /// <summary>
        /// Error handling of received messages
        /// 接收消息错误处理
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual Task onError(LocalResult<T> error)
        {
            if (!isDisposed && !service.IsDisposed)
            {
                if (error.CallState != lastError.CallState)
                {
                    var errorMessage = error.Exception?.Message;
                    lastError.Set(error.CallState, errorMessage);
                    return AutoCSer.LogHelper.Error($"消息节点 {node.Key} 接收消息失败，节点调用状态为 {error.CallState} {errorMessage}");
                }
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
#else
    /// <summary>
    /// Local service message node consumer
    /// 本地服务消息节点消费者
    /// </summary>
    /// <typeparam name="T">Message data type
    /// 消息数据类型</typeparam>
    public abstract class LocalServiceMessageConsumer<T> : LocalServiceMessageConsumer, IDisposable
        where T : Message<T>
    {
        /// <summary>
        /// Message client node
        /// 消息客户端节点
        /// </summary>
        protected readonly IMessageNodeLocalClientNode<T> node;
        /// <summary>
        /// The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量
        /// </summary>
        private readonly int maxMessageCount;
        /// <summary>
        /// Receive the last error status information of the message
        /// 接收消息的最后一次错误信息
        /// </summary>
        protected ResponseResult lastError;
        /// <summary>
        /// Keep callback object of the command
        /// 命令保持回调对象
        /// </summary>
#if NetStandard21
        private IDisposable? keepCallback;
#else
        private IDisposable keepCallback;
#endif
        /// <summary>
        /// Local service message node consumer
        /// 本地服务消息节点消费者
        /// </summary>
        /// <param name="client">Log stream persistence in-memory database local service client
        /// 日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">Message client node
        /// 消息客户端节点</param>
        /// <param name="maxMessageCount">The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">The retry interval is in milliseconds, with a default of 1000 and a minimum value of 1
        /// 重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<T> node, int maxMessageCount, int delayMilliseconds = MessageConsumer.DefaultDelayMilliseconds) : base(client, delayMilliseconds)
        {
            this.node = node;
            this.maxMessageCount = maxMessageCount;
            lastError.Set(CommandClientReturnTypeEnum.Success, CallStateEnum.Success, null);
            start().NotWait();
        }
        /// <summary>
        /// Release resources
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
        /// Start receiving and processing messages
        /// 开始接收并处理消息
        /// </summary>
        /// <returns></returns>
        private async Task start()
        {
            try
            {
                keepCallback = await node.GetMessage(maxMessageCount, onMessage);
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception);
            }
        }
        /// <summary>
        /// Message processing
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        private void onMessage(LocalResult<T> message)
        {
            if (message.IsSuccess)
            {
                if (message.Value != null) checkOnMessage(message.Value).NotWait();
                return;
            }
            onError(message).NotWait();
        }
        /// <summary>
        /// Error handling of received messages
        /// 接收消息错误处理
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual Task onError(LocalResult<T> error)
        {
            if (!isDisposed && !service.IsDisposed)
            {
                if (error.CallState != lastError.CallState)
                {
                    var errorMessage = error.Exception?.Message;
                    lastError.Set(error.CallState, errorMessage);
                    return AutoCSer.LogHelper.Error($"消息节点 {((LocalClientNode)node).Key} 接收消息失败，节点调用状态为 {error.CallState} {errorMessage}");
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
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
#endif
    }
}
