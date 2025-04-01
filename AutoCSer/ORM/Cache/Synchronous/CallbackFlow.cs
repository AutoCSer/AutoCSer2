using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.ORM.Cache.Synchronous
{
    /// <summary>
    /// 缓存数据同步回调流
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="VT">缓存数据类型</typeparam>
    internal sealed class CallbackFlow<T, VT> : ICacheEvent<T, VT>
        where T : class
        where VT : class, T
    {
        /// <summary>
        /// 缓存数据回调委托
        /// </summary>
        private readonly CommandServerKeepCallbackCount<CallbackValue<T>> callback;
        /// <summary>
        /// 事件缓存
        /// </summary>
        private readonly EventCache<T, VT> cache;
        /// <summary>
        /// 检查队列数据委托
        /// </summary>
        private readonly Func<Task> checkQueueHandle;
        /// <summary>
        /// 等待传输的新数据队列首节点
        /// </summary>
#if NetStandard21
        private CallbackValueLinkNode<T>? queueHead;
#else
        private CallbackValueLinkNode<T> queueHead;
#endif
        /// <summary>
        /// 等待传输的新数据队列尾节点
        /// </summary>
#if NetStandard21
        private CallbackValueLinkNode<T>? queueEnd;
#else
        private CallbackValueLinkNode<T> queueEnd;
#endif
        /// <summary>
        /// 下一个等待处理的缓存数据同步回调流
        /// </summary>
#if NetStandard21
        internal CallbackFlow<T, VT>? Next;
#else
        internal CallbackFlow<T, VT> Next;
#endif
        /// <summary>
        /// 是否已经启动同步任务
        /// </summary>
        private int synchronousTask;
        /// <summary>
        /// 缓存数据同步回调流
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="callback"></param>
        internal CallbackFlow(EventCache<T, VT> cache, CommandServerKeepCallbackCount<CallbackValue<T>> callback)
        {
            this.callback = callback;
            this.cache = cache;
            checkQueueHandle = checkQueue;
            synchronousTask = 1;
        }
        /// <summary>
        /// 获取下一个等待处理的缓存数据同步回调流
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal CallbackFlow<T, VT>? GetNext()
#else
        internal CallbackFlow<T, VT> GetNext()
#endif
        {
            var next = Next;
            Next = null;
            return next;
        }
        /// <summary>
        /// 启动缓存数据同步
        /// </summary>
        /// <returns></returns>
        internal async ValueTask Start()
        {
            bool isStart = false;
            try
            {
                synchronous(cache.GetCacheArray()).Catch();
                isStart = true;
            }
            finally
            {
                if (!isStart) await StartError();
            }
        }
        /// <summary>
        /// 启动错误处理
        /// </summary>
        /// <returns></returns>
        internal async Task StartError()
        {
            try
            {
                await cache.NextCallbackFlow(true);
            }
            finally { callback.CancelKeep(); }
        }
        /// <summary>
        /// 同步初始缓存数据
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private async Task synchronous(T[] array)
        {
            bool isError = true;
            try
            {
                CallbackValue<T> callbackValue = new CallbackValue<T>(OperationTypeEnum.Cache);
                foreach (T value in array)
                {
                    callbackValue.Value = value;
                    if (!await callback.CallbackAsync(callbackValue)) return;
                }
                if (!await callback.CallbackAsync(new CallbackValue<T>(OperationTypeEnum.Loaded))) return;
                isError = false;
            }
            finally { await cache.NextCallbackFlow(isError); }
            cache.AppendQueue(checkQueueHandle);
        }
        /// <summary>
        /// 检查队列数据委托
        /// </summary>
        /// <returns></returns>
        private Task checkQueue()
        {
            if (queueHead == null) synchronousTask = 0;
            else if (!cache.IsDispose)
            {
                CallbackValueLinkNode<T> node = queueHead;
                queueHead = queueEnd = null;
                synchronous(node).Catch();
            }
            else callback.CancelKeep();
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 同步新操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task synchronous(CallbackValueLinkNode<T> node)
        {
            bool isNext = false;
            var callbackNode = node;
            try
            {
                do
                {
                    if (!await callback.CallbackAsync(callbackNode.Value)) return;
                }
                while ((callbackNode = callbackNode.LinkNext) != null);
                cache.AppendQueue(checkQueueHandle);
                isNext = true;
            }
            finally
            {
                if (!isNext) cache.RemoveEvent(this);
            }
        }
        /// <summary>
        /// 添加新操作数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        private void append(VT value, OperationTypeEnum operationType)
        {
            var node = new CallbackValueLinkNode<T>(value, operationType);
            if (synchronousTask == 0)
            {
                synchronousTask = 1;
                if (queueEnd != null)
                {
                    queueEnd.LinkNext = node;
                    node = queueHead;
                    queueHead = queueEnd = null;
                }
               synchronous(node.notNull()).Catch();
            }
            else
            {
                if (queueEnd == null) queueHead = node;
                else queueEnd.LinkNext = node;
                queueEnd = node;
            }
        }
        /// <summary>
        /// 添加事件缓存数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        public void OnInserted(VT value)
        {
            append(value, OperationTypeEnum.Insert);
        }
        /// <summary>
        /// 更新事件缓存数据之前的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        public void BeforeUpdate(VT value, MemberMap<T> memberMap) { }
        /// <summary>
        /// 更新事件缓存数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        public void OnUpdated(VT value, MemberMap<T> memberMap)
        {
            append(value, OperationTypeEnum.Update);
        }
        /// <summary>
        /// 删除事件数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        public void OnDeleted(VT value)
        {
            append(value, OperationTypeEnum.Delete);
        }
    }
}
