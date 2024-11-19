using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志收集反向服务客户端套接字事件
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public interface ILogCollectionReverseClientSocketEvent<T>
    {
        /// <summary>
        /// 日志收集反向服务客户端
        /// </summary>
        ILogCollectionReverseClient<T> LogCollectionReverseClient { get; }
    }
}
