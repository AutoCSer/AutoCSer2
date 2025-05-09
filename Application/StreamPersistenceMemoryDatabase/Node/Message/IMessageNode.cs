using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息处理节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if !AOT
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
#endif
    public partial interface IMessageNode<T>
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(T value);
        /// <summary>
        /// 获取未处理完成消息数量（包括失败消息）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetTotalCount();
        /// <summary>
        /// 获取未处理完成消息数量（不包括失败消息）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCount();
        /// <summary>
        /// 获取失败消息数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetFailedCount();
        /// <summary>
        /// 获取消费者回调数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCallbackCount();
        /// <summary>
        /// 获取未完成处理超时消息数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetTimeoutCount();
        /// <summary>
        /// 清除所有消息
        /// </summary>
        void ClearLoadPersistence();
        /// <summary>
        /// 清除所有消息
        /// </summary>
        void Clear();
        /// <summary>
        /// 清除所有失败消息（包括处理超时消息）
        /// </summary>
        void ClearFailedLoadPersistence();
        /// <summary>
        /// 清除所有失败消息（包括处理超时消息）
        /// </summary>
        void ClearFailed();
        /// <summary>
        /// 消费客户端获取消息
        /// </summary>
        /// <param name="maxCount">当前客户端最大并发消息数量</param>
        /// <param name="callback">null 表示心跳测试数据，客户端应该忽略该消息</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
#if NetStandard21
        void GetMessage(int maxCount, MethodKeepCallback<T?> callback);
#else
        void GetMessage(int maxCount, MethodKeepCallback<T> callback);
#endif
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        void AppendMessageLoadPersistence(T message);
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        void AppendMessage(T message);
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void CompletedLoadPersistence(MessageIdeneity identity);
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void Completed(MessageIdeneity identity);
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void FailedLoadPersistence(MessageIdeneity identity);
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void Failed(MessageIdeneity identity);
    }
}
