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
#if AOT
        /// <summary>
        /// 默认重试间隔毫秒数
        /// </summary>
        public const int DefaultDelayMilliseconds = 1000;
#endif

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
#if AOT
    /// <summary>
    /// 消息节点消费者
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    public abstract class LocalServiceMessageConsumer<T> : LocalServiceMessageConsumer, IDisposable
        where T : Message<T>
    {
        /// <summary>
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode node;
        /// <summary>
        /// 服务端单次最大回调消息数量
        /// </summary>
        protected readonly int maxMessageCount;
        /// <summary>
        /// 接收消息的最后一次错误信息
        /// </summary>
        protected LocalResult lastError;
        /// <summary>
        /// 保持回调输出
        /// </summary>
        protected IDisposable? keepCallback;
        /// <summary>
        /// 字符串消息消费者
        /// </summary>
        /// <param name="client">日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">本地服务客户端节点</param>
        /// <param name="maxMessageCount">服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceMessageConsumer(LocalClient client, LocalClientNode node, int maxMessageCount, int delayMilliseconds = LocalServiceMessageConsumer.DefaultDelayMilliseconds) : base(client, delayMilliseconds)
        {
            this.node = node;
            this.maxMessageCount = maxMessageCount;
            lastError.Set(CallStateEnum.Success, null);
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
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task checkOnMessage(T message);
        /// <summary>
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
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
#else
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
        private IDisposable? keepCallback;
#else
        private IDisposable keepCallback;
#endif
        /// <summary>
        /// 字符串消息消费者
        /// </summary>
        /// <param name="client">日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">消息客户端节点</param>
        /// <param name="maxMessageCount">服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<T> node, int maxMessageCount, int delayMilliseconds = MessageConsumer.DefaultDelayMilliseconds) : base(client, delayMilliseconds)
        {
            this.node = node;
            this.maxMessageCount = maxMessageCount;
            lastError.Set(CommandClientReturnTypeEnum.Success, CallStateEnum.Success, null);
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
#endif
    }
}
