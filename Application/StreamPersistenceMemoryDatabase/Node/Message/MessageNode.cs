using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
#if AOT
    /// <summary>
    /// 消息处理节点
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    /// <typeparam name="IT">消息处理节点接口类型</typeparam>
    public abstract class MessageNode<T, IT> : ContextNode<IT, T>, ISnapshot<T>
        where IT : class, IMessageNode<T>
#else
    /// <summary>
    /// 消息处理节点
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    public sealed class MessageNode<T> : ContextNode<IMessageNode<T>, T>, IMessageNode<T>, ISnapshot<T>
#endif
        where T : Message<T>
    {
        /// <summary>
        /// 消息超时时间戳
        /// </summary>
        private readonly long timeoutTimestamp;
        /// <summary>
        /// 超时检查定时
        /// </summary>
#if AOT
        private readonly MessageNodeCheckTimer<T, IT> checkTimer;
#else
        private readonly MessageNodeCheckTimer<T> checkTimer;
#endif
        /// <summary>
        /// 正在处理的消息集合
        /// </summary>
        internal readonly MessageArrayItem<T>[] MessageArray;
        /// <summary>
        /// 初始化加载节点
        /// </summary>
#if AOT
        private MessageNodeLoader<T, IT> loader;
#else
        private MessageNodeLoader<T> loader;
#endif
        /// <summary>
        /// 是否默认空初始化加载节点
        /// </summary>
        private bool isNullLoader
        {
            get
            {
#if AOT
                return object.ReferenceEquals(loader, MessageNodeLoader<T, IT>.Null);
#else
                return object.ReferenceEquals(loader, MessageNodeLoader<T>.Null);
#endif
            }
        }
        /// <summary>
        /// 消息处理客户端回调集合
        /// </summary>
        private LeftArray<MessageNodeCallbackCount<T>> callbacks;
        /// <summary>
        /// 当前预备处理客户端回调
        /// </summary>
#if NetStandard21
        private MethodKeepCallback<T?>? currentCallback;
#else
        private MethodKeepCallback<T> currentCallback;
#endif
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
        private readonly T failedHead;
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
#if NetStandard21
        private T? arrayHead;
#else
        private T arrayHead;
#endif
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
        public MessageNode(int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1)
        {
            timeoutTimestamp = AutoCSer.Date.GetTimestampBySeconds(Math.Max(timeoutSeconds, 1));
#if AOT
            loader = MessageNodeLoader<T, IT>.Null;
            checkTimer = new MessageNodeCheckTimer<T, IT>(Math.Max(checkTimeoutSeconds, 1));
#else
            loader = MessageNodeLoader<T>.Null;
            checkTimer = new MessageNodeCheckTimer<T>(Math.Max(checkTimeoutSeconds, 1));
#endif
            MessageArray = new MessageArrayItem<T>[Math.Max(arraySize, 1)];
            callbacks = new LeftArray<MessageNodeCallbackCount<T>>(sizeof(int));
            timeoutMessages = AutoCSer.DictionaryCreator.CreateLong<T>();
            for (int index = 0; index != MessageArray.Length; MessageArray[index].NextIndex = ++index) ;
            MessageArray[MessageArray.Length - 1].NextIndex = -1;
            linkEnd = linkHead = nextFailed = failedEnd = failedHead = AutoCSer.Metadata.DefaultConstructor<T>.Constructor().notNull();
            failedHead.MessageIdeneity.Flags = MessageFlagsEnum.SnapshotEnd;
            sendFailedIndex = -1;
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if AOT
        public override IT? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
#if NetStandard21
        public override IMessageNode<T>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IMessageNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
#endif
        {
            if (!isNullLoader)
            {
                foreach (T message in loader.Messages.Values) AppendLinkCount(message);
                foreach (T message in loader.FailedMessages.Values) AppendFailedCount(message);
#if AOT
                loader = MessageNodeLoader<T, IT>.Null;
#else
                loader = MessageNodeLoader<T>.Null;
#endif
            }
            checkTimer.Set(this);
#if AOT
            return this as IT;
#else
            return this;
#endif
        }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            checkTimer.Cancel();
            var callback = default(MessageNodeCallbackCount<T>);
            while (callbacks.TryPopOnly(out callback)) callback.Callback.notNull().CancelKeep();
        }
        /// <summary>
        /// 数据库服务关闭操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceDisposable()
        {
            checkTimer.Cancel();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return count + failedCount + 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            SnapshotResult<T> result = new SnapshotResult<T>(count + failedCount + 1, snapshotArray.Length);
            if (!object.ReferenceEquals(linkHead, failedHead))
            {
                for (var message = linkHead; message != null; message = message.LinkNext) result.Add(snapshotArray, message);
            }
            for (var message = failedHead.LinkNext; message != null; message = message.LinkNext)
            {
                if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Completed) != 0) result.Add(snapshotArray, message);
            }
            failedHead.MessageIdeneity.Identity = CurrentIdentity;
            result.Add(snapshotArray, failedHead);
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotAdd(T value)
        {
            if (getLoader().SnapshotAdd(value)) CurrentIdentity = value.MessageIdeneity.Identity;
        }
        /// <summary>
        /// 获取初始化加载节点
        /// </summary>
        /// <returns></returns>
#if AOT
        private MessageNodeLoader<T, IT> getLoader()
#else
        private MessageNodeLoader<T> getLoader()
#endif
        {
            if (!isNullLoader) return loader;
#if AOT
            return loader = new MessageNodeLoader<T, IT>(this);
#else
            return loader = new MessageNodeLoader<T>(this);
#endif
        }
        /// <summary>
        /// 获取初始化加载节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        private MessageNodeLoader<T, IT>? tryGetLoader()
#else
#if NetStandard21
        private MessageNodeLoader<T>? tryGetLoader()
#else
        private MessageNodeLoader<T> tryGetLoader()
#endif
#endif
        {
            return !isNullLoader ? loader : null;
        }
        /// <summary>
        /// 添加等待处理消息
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendLinkCount(T message)
        {
            if (object.ReferenceEquals(linkHead, failedHead)) linkHead = message;
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
        public void ClearLoadPersistence()
        {
            tryGetLoader()?.Clear();
        }
        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void Clear()
        {
            clearFailed();
            sendFailedIndex = -1;
            linkEnd = linkHead = failedEnd = failedHead;
            arrayHead = null;
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
            nextFailed = failedHead;
            failedCount = 0;
            failedHead.LinkNext = null;
        }
        /// <summary>
        /// 清除所有失败消息（包括处理超时消息）
        /// </summary>
        public void ClearFailedLoadPersistence()
        {
            tryGetLoader()?.ClearFailed();
        }
        /// <summary>
        /// 清除所有失败消息（包括处理超时消息）
        /// </summary>
        public void ClearFailed()
        {
            for (var message = failedHead.LinkNext; message != null; message = message.LinkNext)
            {
                if ((message.MessageIdeneity.Flags & MessageFlagsEnum.Timeout) != 0) timeoutMessages.Remove(message.MessageIdeneity.Identity);
            }
            if (sendFailedIndex >= 0)
            {
                int nextIndex;
                var message = MessageArray[sendFailedIndex].GetMessage(out nextIndex);
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
#if NetStandard21
        private void removeCallback(MethodKeepCallback<T?> callback)
#else
        private void removeCallback(MethodKeepCallback<T> callback)
#endif
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
#if NetStandard21
        public void GetMessage(int maxCount, MethodKeepCallback<T?> callback)
#else
        public void GetMessage(int maxCount, MethodKeepCallback<T> callback)
#endif
        {
            MessageNodeCallbackCount<T>[] callbackArray = callbacks.Array;
            for (int index = callbacks.Length; index != 0;)
            {
                var checkCallback = callbackArray[--index].Callback.notNull();
                if (!checkCallback.Callback(default(T))) removeCallback(checkCallback);
            }
            callbacks.PrepLength(1);
            if (fullCallbackIndex != callbacks.Length)
            {
                callbackArray[callbacks.Length] = callbackArray[fullCallbackIndex];
                callbackArray[fullCallbackIndex].Callback.notNull().Reserve = callbacks.Length;
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
            if (!object.ReferenceEquals(linkHead, failedHead)) sendNewMessage();
            else sendFailedMessage();
        }
        /// <summary>
        /// 发送多条新消息（currentCallback 不允许为 null）
        /// </summary>
        private void sendMessageLink()
        {
            while (!object.ReferenceEquals(linkHead, failedHead) && messageArrayFreeIndex >= 0)
            {
                do
                {
                    int freeIndex = messageArrayFreeIndex, getCount = Math.Min(callbacks.Array[0].FreeCount, MessageArray.Length - arrayMessageCount);
#if DEBUG
                    if (getCount <= 0) throw new IndexOutOfRangeException($"getCount[{getCount}] <= 0");
#endif
                    var end = linkHead;
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
                    var callback = currentCallback.notNull();
#pragma warning disable CS8631
                    if (callback.Callback(linkHead, end))
#pragma warning restore CS8631
                    {
                        if (end == null) end = failedHead;
                        do
                        {
                            linkHead = appendArrayMessage(linkHead) ?? failedHead;
                        }
                        while (!object.ReferenceEquals(linkHead, end));
                        if (currentCallback == null) return;
                        break;
                    }
                    else
                    {
                        removeCallback(callback);
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
                        while(currentCallback != null)
                        //do
                        {
                            if (currentCallback.Callback(message))
                            {
                                nextFailed.LinkNext = appendArrayMessage(message);
                                if (nextFailed.LinkNext == null) nextFailed = failedHead;
                                sendFailedIndex = messageArrayFreeIndex;
                                return;
                            }
                            removeCallback(currentCallback);
                        }
                        //while (currentCallback != null);
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
#if NetStandard21
        private T? appendArrayMessage(T message)
#else
        private T appendArrayMessage(T message)
#endif
        {
            var next = message.LinkNext;
            message.LinkNext = arrayHead;
            if (arrayHead != null) MessageArray[arrayHead.MessageIdeneity.ArrayIndex].NextIndex = messageArrayFreeIndex;
            messageArrayFreeIndex = MessageArray[messageArrayFreeIndex].Set(message, currentCallback.notNull(), timeoutTimestamp);
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
                    count.Callback.notNull().Reserve = fullCallbackIndex;
                    currentCallback = callbackArray[0].Callback.notNull();
                    currentCallback.Reserve = 0;
                }
            }
            return next;
        }
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        public void AppendMessageLoadPersistence(T message)
        {
            message.MessageIdeneity.SetNew(++CurrentIdentity);
            getLoader().AppendMessage(message);
        }
        /// <summary>
        /// 生产者添加新消息
        /// </summary>
        /// <param name="message"></param>
        public void AppendMessage(T message)
        {
            if (message != null)
            {
                message.MessageIdeneity.SetNew(++CurrentIdentity);
                StreamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                AppendLinkCount(message);
                if (currentCallback != null && messageArrayFreeIndex >= 0) sendNewMessage();
            }
        }
        /// <summary>
        /// 发送新消息
        /// </summary>
        private void sendNewMessage()
        {
            T message = linkHead;
            message.MessageIdeneity.ArrayIndex = messageArrayFreeIndex;
            while (currentCallback != null)
            //do
            {
                if (currentCallback.Callback(message))
                {
                    linkHead = appendArrayMessage(message) ?? failedHead;
                    return;
                }
                removeCallback(currentCallback);
            }
            //while (currentCallback != null);
        }
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        public void CompletedLoadPersistence(MessageIdeneity identity)
        {
            tryGetLoader()?.Completed(identity);
        }
        /// <summary>
        /// 消息完成处理
        /// </summary>
        /// <param name="identity"></param>
        public void Completed(MessageIdeneity identity)
        {
            if ((uint)identity.ArrayIndex < (uint)MessageArray.Length && (ulong)(identity.Identity - 1) < (ulong)CurrentIdentity)
            {
                int nextIndex;
                var message = MessageArray[identity.ArrayIndex].GetMessage(out nextIndex);
                if (message != null && message.MessageIdeneity.Identity == identity.Identity)
                {
                    if ((message.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) == 0) --count;
                    else
                    {
                        --failedCount;
                        if (identity.ArrayIndex == sendFailedIndex) sendFailedIndex = -1;
                    }
                    StreamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                    freeArrayIndex(message, nextIndex);
                    sendMessage();
                }
                else if (timeoutMessages.Remove(identity.Identity, out message)) message.MessageIdeneity.Flags |= MessageFlagsEnum.Completed;
            }
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
                var nextMessage = message.LinkNext;
                MessageArray[nextIndex].Message.notNull().LinkNext = nextMessage;
                if (nextMessage != null) MessageArray[nextMessage.MessageIdeneity.ArrayIndex].NextIndex = nextIndex;
            }
            int arrayIndex = message.MessageIdeneity.ArrayIndex;
            var callback = MessageArray[arrayIndex].Free(messageArrayFreeIndex);
            messageArrayFreeIndex = arrayIndex;
            --arrayMessageCount;
            if (object.ReferenceEquals(callback, currentCallback)) --callbacks.Array[0].Count;
            else
            {
                int index = callback.Reserve;
                if (callbacks.Array[index].DecrementCount())
                {
                    if (index != fullCallbackIndex)
                    {
                        MessageNodeCallbackCount<T>[] callbackArray = callbacks.Array;
                        MessageNodeCallbackCount<T> count = callbackArray[fullCallbackIndex];
                        callbackArray[fullCallbackIndex] = callbackArray[index];
                        callbackArray[index] = count;
                        count.Callback.notNull().Reserve = index;
                        callback.Reserve = fullCallbackIndex;
                    }
                    if (fullCallbackIndex++ == 0) currentCallback = callback;
                }
            }
        }
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        public void FailedLoadPersistence(MessageIdeneity identity)
        {
            tryGetLoader()?.Failed(identity);
        }
        /// <summary>
        /// 消息失败处理
        /// </summary>
        /// <param name="identity"></param>
        public void Failed(MessageIdeneity identity)
        {
            if ((uint)identity.ArrayIndex < (uint)MessageArray.Length && (ulong)(identity.Identity - 1) < (ulong)CurrentIdentity)
            {
                int nextIndex;
                var message = MessageArray[identity.ArrayIndex].GetMessage(out nextIndex);
                if (message != null && message.MessageIdeneity.Identity == identity.Identity)
                {
                    if ((message.MessageIdeneity.Flags & MessageFlagsEnum.FailedOrTimeout) == 0)
                    {
                        --count;
                        ++failedCount;
                    }
                    StreamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                    message.MessageIdeneity.Flags |= MessageFlagsEnum.Failed;
                    failed(message, nextIndex);
                    sendMessage();
                }
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
#if AOT
                    StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(new MessageNodeCheckTimeoutCallback<T, IT>(this));
#else
                    StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(new MessageNodeCheckTimeoutCallback<T>(this));
#endif
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
                for (var message = arrayHead; message != null; )
                {
                    var nextMessage = message.LinkNext;
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
                    default:
                        if (currentCallback != null) sendMessageLink();
                        break;
                }
            }
        }
    }
}
