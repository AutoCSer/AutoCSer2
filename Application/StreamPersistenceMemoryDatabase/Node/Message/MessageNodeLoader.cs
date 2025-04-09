using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息处理节点初始化加载节点
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    internal sealed class MessageNodeLoader<T> : ContextNode<IMessageNode<T>, T>, IMessageNode<T>, ISnapshot<T>
        where T : Message<T>
    {
        /// <summary>
        /// 消息处理节点
        /// </summary>
        private MessageNode<T> messageNode;
        /// <summary>
        /// 未完成消息集合
        /// </summary>
        private Dictionary<long, T> messages;
        /// <summary>
        /// 处理失败消息集合
        /// </summary>
        private Dictionary<long, T> failedMessages;
        /// <summary>
        /// 消息处理节点初始化加载节点
        /// </summary>
        /// <param name="messageNode">消息处理节点</param>
        internal MessageNodeLoader(MessageNode<T> messageNode)
        {
            this.messageNode = messageNode;
            messages = AutoCSer.DictionaryCreator.CreateLong<T>(messageNode.MessageArray.Length);
            failedMessages = AutoCSer.DictionaryCreator.CreateLong<T>();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IMessageNode<T>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IMessageNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (T message in messages.Values) messageNode.AppendLinkCount(message);
            foreach (T message in failedMessages.Values) messageNode.AppendFailedCount(message);
            messageNode.SetContext(StreamPersistenceMemoryDatabaseNode);
            return messageNode.StreamPersistenceMemoryDatabaseServiceLoaded();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotAdd(T value)
        {
            if ((value.MessageIdeneity.Flags & MessageFlagsEnum.SnapshotEnd) == 0)
            {
                ((value.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) == 0 ? messages : failedMessages).Add(value.MessageIdeneity.Identity, value);
            }
            else messageNode.CurrentIdentity = value.MessageIdeneity.Identity;
        }
        /// <summary>
        /// 获取未处理完成消息数量（包括失败消息）
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 获取未处理完成消息数量（不包括失败消息）
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 获取失败消息数量
        /// </summary>
        /// <returns></returns>
        public int GetFailedCount()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 获取消费者回调数量
        /// </summary>
        /// <returns></returns>
        public int GetCallbackCount()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 获取未完成处理超时消息数量
        /// </summary>
        /// <returns></returns>
        public int GetTimeoutCount()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void Clear()
        {
            ClearFailed();
            messages.Clear();
        }
        /// <summary>
        /// 清除所有失败消息
        /// </summary>
        public void ClearFailed()
        {
            failedMessages.Clear();
        }
        /// <summary>
        /// 消费客户端获取消息
        /// </summary>
        /// <param name="maxCount">当前客户端最大并发消息数量</param>
        /// <param name="callback"></param>
#if NetStandard21
        public void GetMessage(int maxCount, MethodKeepCallback<T?> callback)
#else
        public void GetMessage(int maxCount, MethodKeepCallback<T> callback)
#endif
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        public void AppendMessage(T message)
        {
            message.MessageIdeneity.SetNew(++messageNode.CurrentIdentity);
            messages.Add(messageNode.CurrentIdentity, message);
        }
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        public void Completed(MessageIdeneity identity)
        {
            if (!messages.Remove(identity.Identity)) failedMessages.Remove(identity.Identity);
        }
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        public void Failed(MessageIdeneity identity)
        {
            var message = default(T);
            if (messages.Remove(identity.Identity, out message))
            {
                message.MessageIdeneity.Flags |= MessageFlagsEnum.Failed;
                failedMessages.Add(identity.Identity, message);
            }
        }
    }
}
