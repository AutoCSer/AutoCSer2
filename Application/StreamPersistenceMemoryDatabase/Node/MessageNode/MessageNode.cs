using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息处理节点
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    public class MessageNode<T> : ContextNode<IMessageNode<T>>, IMessageNode<T>, ISnapshot<T>
        where T : Message<T>
    {
        /// <summary>
        /// 消息超时时间戳
        /// </summary>
        private readonly long timeoutTimestamp;
        /// <summary>
        /// 超时检查定时
        /// </summary>
        private readonly MessageNodeCheckTimer<T> checkTimer;
        /// <summary>
        /// 正在处理的消息集合
        /// </summary>
        internal MessageArrayItem<T>[] MessageArray;
        /// <summary>
        /// 消息处理客户端回调集合
        /// </summary>
        private LeftArray<MessageNodeCallbackCount<T>> callbacks;
        /// <summary>
        /// 当前预备处理客户端回调
        /// </summary>
        private MethodKeepCallback<T> currentCallback;
        /// <summary>
        /// 超时消息集合
        /// </summary>
        private Dictionary<long, T> timeoutMessages;
        /// <summary>
        /// 等待处理的消息头节点
        /// </summary>
        private T linkHead;
        /// <summary>
        /// 等待处理的消息尾节点
        /// </summary>
        private T linkEnd;
        /// <summary>
        /// 失败消息头节点
        /// </summary>
        private T failedHead;
        /// <summary>
        /// 失败消息尾节点
        /// </summary>
        private T failedEnd;
        /// <summary>
        /// LinkNext 为下一个待重试处理失败消息
        /// </summary>
        private T nextFailed;
        /// <summary>
        /// 正在处理的消息头节点，NextIndex 表示上一个节点位置
        /// </summary>
        private T arrayHead;
        /// <summary>
        /// 当前消息分配唯一编号（节点内唯一编号）
        /// </summary>
        internal long CurrentIdentity;
        /// <summary>
        /// 未处理完成消息数量（不包括失败消息）
        /// </summary>
        private int count;
        /// <summary>
        /// 失败消息数量（包括处理超时消息）
        /// </summary>
        private int failedCount;
        /// <summary>
        /// 正在处理中的消息数量
        /// </summary>
        private int arrayMessageCount;
        /// <summary>
        /// 空闲的消息位置，NextIndex 表示下一个空闲位置
        /// </summary>
        private int messageArrayFreeIndex;
        /// <summary>
        /// 正在重试失败消息的数组位置
        /// </summary>
        private int sendFailedIndex;
        /// <summary>
        /// 满负载消费者回调起始位置
        /// </summary>
        private int fullCallbackIndex;
        /// <summary>
        /// 是否正在检查超时
        /// </summary>
        private int isCheckTimeout;
        /// <summary>
        /// 消息处理节点
        /// </summary>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        private MessageNode(int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1)
        {
            timeoutTimestamp = AutoCSer.Date.GetTimestampBySeconds(Math.Max(timeoutSeconds, 1));
            checkTimeoutSeconds = Math.Max(checkTimeoutSeconds, 1);
            checkTimer = new MessageNodeCheckTimer<T>(this, Math.Max(checkTimeoutSeconds, 1));
            MessageArray = new MessageArrayItem<T>[Math.Max(arraySize, 1)];
            callbacks = new LeftArray<MessageNodeCallbackCount<T>>(sizeof(int));
            timeoutMessages = AutoCSer.Extensions.DictionaryCreator.CreateLong<T>();
            for (int index = 0; index != MessageArray.Length; MessageArray[index].NextIndex = ++index) ;
            MessageArray[MessageArray.Length - 1].NextIndex = -1;
            nextFailed = failedHead = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            failedHead.MessageIdeneity.Flags = MessageFlagsEnum.SnapshotEnd;
            sendFailedIndex = -1;
        }
        /// <summary>
        /// 创建消息处理节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns></returns>
        public static IMessageNode<T> Create(StreamPersistenceMemoryDatabaseService service, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1)
        {
            MessageNode<T> messageNode = new MessageNode<T>(arraySize, timeoutSeconds, checkTimeoutSeconds);
            if (service.IsLoaded) return messageNode;
            return new MessageNodeLoader<T>(messageNode);
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
        public override IMessageNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
        {
            checkTimer.AppendTaskArray();
            return this;
        }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            checkTimer.Cancel();
            while (callbacks.Length != 0) callbacks.Array[--callbacks.Length].Callback.CancelKeep();
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            LeftArray<T> array = new LeftArray<T>(count + failedCount + 1);
            for (T message = linkHead; message != null; message = message.LinkNext) array.Add(message);
            for (T message = failedHead.LinkNext; message != null; message = message.LinkNext)
            {
                if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Completed) != 0) array.Add(message);
            }
            failedHead.MessageIdeneity.Identity = CurrentIdentity;
            array.Add(failedHead);
            return array;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotAdd(T value)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 添加等待处理消息
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendLinkCount(T message)
        {
            if (linkHead == null) linkHead = message;
            else linkEnd.LinkNext = message;
            linkEnd = message;
            ++count;
        }
        /// <summary>
        /// 添加失败消息
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendFailedCount(T message)
        {
            appendFailed(message);
            ++failedCount;
        }
        /// <summary>
        /// 添加失败消息
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void appendFailed(T message)
        {
            if (failedHead.LinkNext == null) failedHead.LinkNext = message;
            else failedEnd.LinkNext = message;
            failedEnd = message;
        }
        /// <summary>
        /// 获取未处理完成消息数量（包括失败消息）
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            return count + failedCount;
        }
        /// <summary>
        /// 获取未处理完成消息数量（不包括失败消息）
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return count;
        }
        /// <summary>
        /// 获取消费者回调数量
        /// </summary>
        /// <returns></returns>
        public int GetCallbackCount()
        {
            return callbacks.Length;
        }
        /// <summary>
        /// 获取未完成处理超时消息数量
        /// </summary>
        /// <returns></returns>
        public int GetTimeoutCount()
        {
            return timeoutMessages.Count;
        }
        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void Clear()
        {
            clearFailed();
            sendFailedIndex = -1;
            arrayHead = linkHead = null;
            messageArrayFreeIndex = arrayMessageCount = count = 0;
            timeoutMessages.Clear();
            for (int index = 0; index != MessageArray.Length; MessageArray[index].Clear(++index)) ;
            MessageArray[MessageArray.Length - 1].NextIndex = -1;
            for (int index = fullCallbackIndex = callbacks.Length; index != 0; callbacks.Array[--index].Count = 0) ;
            if (fullCallbackIndex != 0) currentCallback = callbacks[0].Callback;
        }
        /// <summary>
        /// 获取失败消息数量（包括处理超时消息）
        /// </summary>
        /// <returns></returns>
        public int GetFailedCount()
        {
            return failedCount;
        }
        /// <summary>
        /// 清除所有失败消息
        /// </summary>
        private void clearFailed()
        {
            streamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
            nextFailed = failedHead;
            failedCount = 0;
            failedHead.LinkNext = null;
        }
        /// <summary>
        /// 清除所有失败消息（包括处理超时消息）
        /// </summary>
        public void ClearFailed()
        {
            for (T message = failedHead.LinkNext; message != null; message = message.LinkNext)
            {
                if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Timeout) != 0) timeoutMessages.Remove(message.MessageIdeneity.Identity);
            }
            if (sendFailedIndex >= 0)
            {
                int nextIndex;
                T message = MessageArray[sendFailedIndex].GetMessage(out nextIndex);
                if (message != null && (message.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) != 0) freeArrayIndex(message, nextIndex);
#if DEBUG
                else
                {
                    if (message == null) throw new IndexOutOfRangeException($"MessageArray[{sendFailedIndex}] is null");
                    else if ((message.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) == 0) throw new IndexOutOfRangeException($"MessageArray[{sendFailedIndex}] is not FailedOrTimeout");
                }
#endif
                sendFailedIndex = -1;
            }
            clearFailed();
        }
        /// <summary>
        /// 移除无效客户端回调
        /// </summary>
        /// <param name="callback"></param>
        private void removeCallback(MethodKeepCallback<T> callback)
        {
            int index = callback.Reserve;
            MessageNodeCallbackCount<T>[] callbackArray = callbacks.Array;
            if (callbackArray[index].FreeCount == 0)
            {
                if (index != --callbacks.Length) callbackArray[index].SetRemove(ref callbackArray[callbacks.Length]);
            }
            else
            {
                if (index != --fullCallbackIndex) callbackArray[index].SetRemove(ref callbackArray[fullCallbackIndex]);
                if (fullCallbackIndex != --callbacks.Length) callbackArray[fullCallbackIndex].SetRemove(ref callbackArray[callbacks.Length]);
            }
            callbackArray[callbacks.Length].Callback = null;
            if (index == 0) currentCallback = fullCallbackIndex != 0 ? callbackArray[0].Callback : null;
        }
        /// <summary>
        /// 消费客户端获取消息
        /// </summary>
        /// <param name="maxCount">当前客户端最大并发消息数量</param>
        /// <param name="callback"></param>
        public void GetMessage(int maxCount, MethodKeepCallback<T> callback)
        {
            MessageNodeCallbackCount<T>[] callbackArray = callbacks.Array;
            for (int index = callbacks.Length; index != 0;)
            {
                MethodKeepCallback<T> checkCallback = callbackArray[--index].Callback;
                if (!checkCallback.Callback(streamPersistenceMemoryDatabaseService.CommandServerCallQueue, (T)null)) removeCallback(checkCallback);
            }
            callbacks.PrepLength(1);
            if (fullCallbackIndex != callbacks.Length)
            {
                callbackArray[callbacks.Length] = callbackArray[fullCallbackIndex];
                callbackArray[fullCallbackIndex].Callback.Reserve = callbacks.Length;
            }
            callback.Reserve = fullCallbackIndex;
            callbackArray[fullCallbackIndex++].Set(callback, maxCount);
            ++callbacks.Length;
            if (callback.Reserve == 0)
            {
                currentCallback = callback;
                if (maxCount > 1) sendMessageLink();
                else if (messageArrayFreeIndex >= 0) sendMessage();
            }
        }
        /// <summary>
        /// 发送一条消息
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void sendMessage()
        {
            if (linkHead != null) sendNewMessage();
            else sendFailedMessage();
        }
        /// <summary>
        /// 发送多条新消息
        /// </summary>
        private void sendMessageLink()
        {
            while (linkHead != null && messageArrayFreeIndex >= 0)
            {
                do
                {
                    int freeIndex = messageArrayFreeIndex, getCount = Math.Min(callbacks.Array[0].FreeCount, MessageArray.Length - arrayMessageCount);
#if DEBUG
                    if (getCount <= 0) throw new IndexOutOfRangeException($"getCount[{getCount}] <= 0");
#endif
                    T end = linkHead;
                    do
                    {
                        end.MessageIdeneity.ArrayIndex = freeIndex;
                        if ((end = end.LinkNext) == null || --getCount == 0) break;
                        freeIndex = MessageArray[freeIndex].NextIndex;
#if DEBUG
                        if (freeIndex < 0) throw new IndexOutOfRangeException($"freeIndex[{freeIndex}] < 0");
#endif
                    }
                    while (true);
                    if (currentCallback.Callback(streamPersistenceMemoryDatabaseService.CommandServerCallQueue, linkHead, end))
                    {
                        do
                        {
                            linkHead = appendArrayMessage(linkHead);
                        }
                        while (linkHead != end);
                        if (currentCallback == null) return;
                        break;
                    }
                    else
                    {
                        removeCallback(currentCallback);
                        if (currentCallback == null) return;
                    }
                }
                while (true);

                //T message = linkHead;
                //message.MessageIdeneity.ArrayIndex = messageArrayFreeIndex;
                //do
                //{
                //    if (currentCallback.Callback(message))
                //    {
                //        linkHead = message.LinkNext;
                //        appendArrayMessage(message);
                //        if (currentCallback == null) return;
                //        break;
                //    }
                //    removeCallback(currentCallback);
                //    if (currentCallback == null) return;
                //}
                //while (true);
            }
            if (messageArrayFreeIndex >= 0) sendFailedMessage();
        }
        /// <summary>
        /// 发送一条失败消息
        /// </summary>
        private void sendFailedMessage()
        {
            if (sendFailedIndex < 0)
            {
                while (nextFailed.LinkNext != null)
                {
                    T message = nextFailed.LinkNext;
                    if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Completed) == 0)
                    {
                        message.MessageIdeneity.ArrayIndex = messageArrayFreeIndex;
                        do
                        {
                            if (currentCallback.Callback(streamPersistenceMemoryDatabaseService.CommandServerCallQueue, message))
                            {
                                nextFailed.LinkNext = appendArrayMessage(message);
                                if (nextFailed.LinkNext == null) nextFailed = failedHead;
                                sendFailedIndex = messageArrayFreeIndex;
                                return;
                            }
                            removeCallback(currentCallback);
                        }
                        while (currentCallback != null);
                        return;
                    }
                    nextFailed.LinkNext = message.LinkNext;
                    if (nextFailed.LinkNext == null) nextFailed = failedHead;
                    --failedCount;
                    if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Timeout) != 0) timeoutMessages.Remove(message.MessageIdeneity.Identity);
                }
            }
        }
        /// <summary>
        /// 消息添加到处理中数组
        /// </summary>
        /// <param name="message"></param>
        /// <returns>message.LinkNext</returns>
        private T appendArrayMessage(T message)
        {
            T next = message.LinkNext;
            message.LinkNext = arrayHead;
            if (arrayHead != null) MessageArray[arrayHead.MessageIdeneity.ArrayIndex].NextIndex = messageArrayFreeIndex;
            messageArrayFreeIndex = MessageArray[messageArrayFreeIndex].Set(message, currentCallback, timeoutTimestamp);
            arrayHead = message;
            ++arrayMessageCount;
            MessageNodeCallbackCount<T>[] callbackArray = callbacks.Array;
            if (callbackArray[0].IncrementCount())
            {
                if (fullCallbackIndex == 1)
                {
                    fullCallbackIndex = 0;
                    currentCallback = null;
                }
                else
                {
                    MessageNodeCallbackCount<T> count = callbackArray[0];
                    callbackArray[0] = callbackArray[--fullCallbackIndex];
                    callbackArray[fullCallbackIndex] = count;
                    count.Callback.Reserve = fullCallbackIndex;
                    currentCallback = callbackArray[0].Callback;
                    currentCallback.Reserve = 0;
                }
            }
            return next;
        }
        /// <summary>
        /// 生产者添加新消息 持久化参数检查
        /// </summary>
        /// <param name="message"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool AppendMessageBeforePersistence(T message)
        {
            return message != null;
        }
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        public void AppendMessage(T message)
        {
            streamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
            message.MessageIdeneity.SetNew(++CurrentIdentity);
            AppendLinkCount(message);
            if (currentCallback != null && messageArrayFreeIndex >= 0) sendNewMessage();
        }
        /// <summary>
        /// 发送新消息
        /// </summary>
        private void sendNewMessage()
        {
            T message = linkHead;
            message.MessageIdeneity.ArrayIndex = messageArrayFreeIndex;
            do
            {
                if (currentCallback.Callback(streamPersistenceMemoryDatabaseService.CommandServerCallQueue, message))
                {
                    linkHead = appendArrayMessage(message);
                    return;
                }
                removeCallback(currentCallback);
            }
            while (currentCallback != null);
        }
        /// <summary>
        /// 消息完成处理 持久化参数检查
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool CompletedBeforePersistence(MessageIdeneity identity)
        {
            return (uint)identity.ArrayIndex < (uint)MessageArray.Length && (ulong)(identity.Identity - 1) < (ulong)CurrentIdentity;
        }
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        public void Completed(MessageIdeneity identity)
        {
            int nextIndex;
            T message = MessageArray[identity.ArrayIndex].GetMessage(out nextIndex);
            if (message != null && message.MessageIdeneity.Identity == identity.Identity)
            {
                streamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                if (identity.ArrayIndex != sendFailedIndex) --count;
                else
                {
                    --failedCount;
                    sendFailedIndex = -1;
                }
                freeArrayIndex(message, nextIndex);
                sendMessage();
            }
            else if (timeoutMessages.Remove(identity.Identity, out message)) message.MessageIdeneity.Flags |= MessageFlagsEnum.Completed;
        }
        /// <summary>
        /// 释放消息处理状态
        /// </summary>
        /// <param name="message"></param>
        /// <param name="nextIndex"></param>
        private void freeArrayIndex(T message, int nextIndex)
        {
            if (object.ReferenceEquals(message, arrayHead))
            {
                arrayHead = message.LinkNext;
            }
            else
            {
                T nextMessage = message.LinkNext;
                MessageArray[nextIndex].Message.LinkNext = nextMessage;
                if (nextMessage != null) MessageArray[nextMessage.MessageIdeneity.ArrayIndex].NextIndex = nextIndex;
            }
            int arrayIndex = message.MessageIdeneity.ArrayIndex;
            MethodKeepCallback<T> callbacdk = MessageArray[arrayIndex].Free(messageArrayFreeIndex);
            messageArrayFreeIndex = arrayIndex;
            --arrayMessageCount;
            if (object.ReferenceEquals(callbacdk, currentCallback)) --callbacks.Array[0].Count;
            else
            {
                int index = callbacdk.Reserve;
                if (callbacks.Array[index].DecrementCount())
                {
                    if (index != fullCallbackIndex)
                    {
                        MessageNodeCallbackCount<T>[] callbackArray = callbacks.Array;
                        MessageNodeCallbackCount<T> count = callbackArray[fullCallbackIndex];
                        callbackArray[fullCallbackIndex] = callbackArray[index];
                        callbackArray[index] = count;
                        count.Callback.Reserve = index;
                        callbacdk.Reserve = fullCallbackIndex;
                    }
                    if (fullCallbackIndex++ == 0) currentCallback = callbacdk;
                }
            }
        }
        /// <summary>
        /// 消息失败处理 持久化参数检查
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool FailedBeforePersistence(MessageIdeneity identity)
        {
            return (uint)identity.ArrayIndex < (uint)MessageArray.Length && (ulong)(identity.Identity - 1) < (ulong)CurrentIdentity;
        }
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        public void Failed(MessageIdeneity identity)
        {
            int nextIndex;
            T message = MessageArray[identity.ArrayIndex].GetMessage(out nextIndex);
            if (message != null && message.MessageIdeneity.Identity == identity.Identity)
            {
                streamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                if ((message.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) == 0)
                {
                    --count;
                    ++failedCount;
                }
                message.MessageIdeneity.Flags |= MessageFlagsEnum.Failed;
                failed(message, nextIndex);
                sendMessage();
            }
        }
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="message"></param>
        /// <param name="nextIndex"></param>
        private void failed(T message, int nextIndex)
        {
            if (message.MessageIdeneity.ArrayIndex == sendFailedIndex) sendFailedIndex = -1;
            freeArrayIndex(message, nextIndex);
            message.LinkNext = null;
            appendFailed(message);
        }
        /// <summary>
        /// 消息超时检查
        /// </summary>
        internal void CheckTimeout()
        {
            if (arrayHead != null && Interlocked.CompareExchange(ref isCheckTimeout, 1, 0) == 0)
            {
                bool isCallback = false;
                try
                {
                    streamPersistenceMemoryDatabaseService.CommandServerCallQueue.AddOnly(new MessageNodeCheckTimeoutCallback<T>(this));
                    isCallback = true;
                }
                finally
                {
                    if (!isCallback) Interlocked.Exchange(ref isCheckTimeout, 0);
                }
            }
        }
        /// <summary>
        /// 消息超时检查
        /// </summary>
        internal void CheckTimeoutCallback()
        {
            int timeoutCount = 0;
#if DEBUG
            int messageCount = MessageArray.Length;
#endif
            try
            {
                int nextIndex;
                for (T message = arrayHead; message != null; )
                {
                    T nextMessage = message.LinkNext;
                    if (object.ReferenceEquals(MessageArray[message.MessageIdeneity.ArrayIndex].CheckTimeout(out nextIndex), message))
                    {
                        if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Timeout) == 0)
                        {
                            timeoutMessages.TryAdd(message.MessageIdeneity.Identity, message);
                            message.MessageIdeneity.Flags |= MessageFlagsEnum.Timeout;
                            if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Failed) == 0)
                            {
                                --count;
                                ++failedCount;
                            }
                        }
                        failed(message, nextIndex);
                        ++timeoutCount;
                    }
                    message = nextMessage;
#if DEBUG
                    if (--messageCount < 0) throw new IndexOutOfRangeException($"messageCount[{messageCount}] < 0");
#endif
                }
            }
            finally
            {
                Interlocked.Exchange(ref isCheckTimeout, 0);
                switch (timeoutCount)
                {
                    case 0: break;
                    case 1: sendMessage(); break;
                    default: sendMessageLink(); break;
                }
            }
        }
    }
}
