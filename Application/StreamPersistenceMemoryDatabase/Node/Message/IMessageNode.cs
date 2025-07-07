using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Message processing node interface
    /// 消息处理节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if !AOT
    [ServerNode(IsLocalClient = true)]
#endif
    public partial interface IMessageNode<T>
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(T value);
        /// <summary>
        /// Get the number of uncompleted messages (including failed messages)
        /// 获取未完成消息数量（包括失败消息）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetTotalCount();
        /// <summary>
        /// Get the number of uncompleted messages (excluding failed messages)
        /// 获取未完成消息数量（不包括失败消息）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCount();
        /// <summary>
        /// Get the number of failed messages (Including handling timeout messages)
        /// 获取失败消息数量（包括处理超时消息）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetFailedCount();
        /// <summary>
        /// Get the number of consumer callbacks
        /// 获取消费者回调数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCallbackCount();
        /// <summary>
        /// Get the number of unfinished timeout messages
        /// 获取未完成的超时消息数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetTimeoutCount();
        /// <summary>
        /// Clear all messages (Initialize and load the persistent data)
        /// 清除所有消息（初始化加载持久化数据）
        /// </summary>
        void ClearLoadPersistence();
        /// <summary>
        /// Clear all messages
        /// 清除所有消息
        /// </summary>
        void Clear();
        /// <summary>
        /// Clear all failure messages (including handling timeout messages) (Initialize and load the persistent data)
        /// 清除所有失败消息（包括处理超时消息）（初始化加载持久化数据）
        /// </summary>
        void ClearFailedLoadPersistence();
        /// <summary>
        /// Clear all failure messages (including handling timeout messages)
        /// 清除所有失败消息（包括处理超时消息）
        /// </summary>
        void ClearFailed();
        /// <summary>
        /// The consumer client gets the message
        /// 消费客户端获取消息
        /// </summary>
        /// <param name="maxCount">The current maximum number of concurrent messages on the client side
        /// 当前客户端最大并发消息数量</param>
        /// <param name="callback">Returning null indicates the heart rate test data. The client should ignore the null message
        /// 返回 null 表示心跳测试数据，客户端应该忽略 null 消息</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
#if NetStandard21
        void GetMessage(int maxCount, MethodKeepCallback<T?> callback);
#else
        void GetMessage(int maxCount, MethodKeepCallback<T> callback);
#endif
        /// <summary>
        /// Producers add new message (Initialize and load the persistent data)
        /// 生产者添加新消息（初始化加载持久化数据）
        /// </summary>
        /// <param name="message"></param>
        void AppendMessageLoadPersistence(T message);
        /// <summary>
        /// Producers add new message
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        void AppendMessage(T message);
        /// <summary>
        /// The message has been processed (Initialize and load the persistent data)
        /// 消息完成处理（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void CompletedLoadPersistence(MessageIdeneity identity);
        /// <summary>
        /// The message has been processed
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void Completed(MessageIdeneity identity);
        /// <summary>
        /// Message failed processing (Initialize and load the persistent data)
        /// 消息失败处理（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void FailedLoadPersistence(MessageIdeneity identity);
        /// <summary>
        /// Message failed processing
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void Failed(MessageIdeneity identity);
    }
}
