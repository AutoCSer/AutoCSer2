﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息处理节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(MethodIndexEnumType = typeof(MessageNodeMethodEnum), IsAutoMethodIndex = false, IsLocalClient = true)]
    public interface IMessageNode<T>
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
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
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Clear();
        /// <summary>
        /// 清除所有失败消息
        /// </summary>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void ClearFailed();
        /// <summary>
        /// 消费客户端获取消息
        /// </summary>
        /// <param name="maxCount">当前客户端最大并发消息数量</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void GetMessage(int maxCount, MethodKeepCallback<T> callback);
        /// <summary>
        /// 生产者添加新消息 持久化参数检查
        /// </summary>
        /// <param name="message"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        bool AppendMessageBeforePersistence(T message);
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendMessage(T message);
        /// <summary>
        /// 消息完成处理 持久化参数检查
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        bool CompletedBeforePersistence(MessageIdeneity identity);
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void Completed(MessageIdeneity identity);
        /// <summary>
        /// 消息失败处理 持久化参数检查
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        bool FailedBeforePersistence(MessageIdeneity identity);
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void Failed(MessageIdeneity identity);
    }
}
