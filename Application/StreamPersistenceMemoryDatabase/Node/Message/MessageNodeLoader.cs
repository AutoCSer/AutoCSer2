using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
#if AOT
    /// <summary>
    /// 消息处理节点初始化加载节点
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    /// <typeparam name="IT">消息处理节点接口类型</typeparam>
    internal sealed class MessageNodeLoader<T, IT>
        where IT : class, IMessageNode<T>
#else
    /// <summary>
    /// 消息处理节点初始化加载节点
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    internal sealed class MessageNodeLoader<T>
#endif
        where T : Message<T>
    {
        /// <summary>
        /// 未完成消息集合
        /// </summary>
        internal Dictionary<long, T> Messages;
        /// <summary>
        /// 处理失败消息集合
        /// </summary>
        internal Dictionary<long, T> FailedMessages;
        /// <summary>
        /// 消息处理节点初始化加载节点
        /// </summary>
        private MessageNodeLoader()
        {
            Messages = FailedMessages = AutoCSer.DictionaryCreator.CreateLong<T>();
        }
        /// <summary>
        /// 消息处理节点初始化加载节点
        /// </summary>
        /// <param name="messageNode">消息处理节点</param>
#if AOT
        internal MessageNodeLoader(MessageNode<T, IT> messageNode)
#else
        internal MessageNodeLoader(MessageNode<T> messageNode)
#endif
        {
            Messages = AutoCSer.DictionaryCreator.CreateLong<T>(messageNode.MessageArray.Length);
            FailedMessages = AutoCSer.DictionaryCreator.CreateLong<T>();
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否快照结束数据</returns>
        internal bool SnapshotAdd(T value)
        {
            if ((value.MessageIdeneity.Flags & MessageFlagsEnum.SnapshotEnd) == 0)
            {
                ((value.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) == 0 ? Messages : FailedMessages).Add(value.MessageIdeneity.Identity, value);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 清除所有消息
        /// </summary>
        internal void Clear()
        {
            ClearFailed();
            if (Messages.Count != 0) Messages.Clear();
        }
        /// <summary>
        /// 清除所有失败消息
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClearFailed()
        {
            if (FailedMessages.Count != 0) FailedMessages.Clear();
        }
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        internal void AppendMessage(T message)
        {
            Messages.Add(message.MessageIdeneity.Identity, message);
        }
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        internal void Completed(MessageIdeneity identity)
        {
            if (!Messages.Remove(identity.Identity)) FailedMessages.Remove(identity.Identity);
        }
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        internal void Failed(MessageIdeneity identity)
        {
            var message = default(T);
            if (Messages.Remove(identity.Identity, out message))
            {
                message.MessageIdeneity.Flags |= MessageFlagsEnum.Failed;
                FailedMessages.Add(identity.Identity, message);
            }
        }

        /// <summary>
        /// 消息处理节点初始化加载节点
        /// </summary>
#if AOT
        internal static readonly MessageNodeLoader<T, IT> Null = new MessageNodeLoader<T, IT>();
#else
        internal static readonly MessageNodeLoader<T> Null = new MessageNodeLoader<T>();
#endif
    }
}
