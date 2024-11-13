using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Net;
using AutoCSer.ORM.Cache.Synchronous;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 事件缓存
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public abstract class EventCache<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        protected readonly TableWriter<T> tableWriter;
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        public TableWriter<T> TableWriter { get { return tableWriter; } }
        /// <summary>
        /// 表格操作事件
        /// </summary>
        private readonly CacheTableEvent<T> tableEvent;
        /// <summary>
        /// 是否调用了释放资源操作
        /// </summary>
        internal bool IsDispose;
        /// <summary>
        /// 缓存对象是否事件可用
        /// </summary>
        protected readonly bool isEventAvailable;
        /// <summary>
        /// 事件缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isEventAvailable">缓存对象是否事件可用</param>
        protected EventCache(TableWriter<T> tableWriter, bool isEventAvailable)
        {
            this.tableWriter = tableWriter;
            this.isEventAvailable = isEventAvailable;
            tableWriter.AppendEvent(tableEvent = new CacheTableEvent<T>(this));
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            tableWriter.RemoveEvent(tableEvent);
        }
        /// <summary>
        /// 添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        internal abstract void OnInserted(T value);
        /// <summary>
        /// 更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        internal abstract void OnUpdated(T value, MemberMap<T> memberMap);
        /// <summary>
        /// 删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal abstract void OnDeleted(T value);
    }
    /// <summary>
    /// 事件缓存
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="VT">缓存数据类型</typeparam>
    public abstract class EventCache<T, VT> : EventCache<T>
        where T : class
        where VT : class, T
    {
        /// <summary>
        /// 添加队列任务
        /// </summary>
        internal readonly Action<Func<Task>> AppendQueue;
        /// <summary>
        /// 缓存事件集合
        /// </summary>
        private readonly LeftArray<ICacheEvent<T, VT>> events;
        /// <summary>
        /// 当前缓存数据同步回调流
        /// </summary>
#if NetStandard21
        private CallbackFlow<T, VT>? callbackFlow;
#else
        private CallbackFlow<T, VT> callbackFlow;
#endif
        /// <summary>
        /// 获取缓存数据数量
        /// </summary>
        public abstract int Count { get; }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        internal abstract IEnumerable<VT> Values { get; }
        /// <summary>
        /// 事件缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isEventAvailable">缓存对象是否事件可用</param>
        /// <param name="appendQueue">添加队列任务</param>
        protected EventCache(TableWriter<T> tableWriter, bool isEventAvailable, Action<Func<Task>> appendQueue) : base(tableWriter, isEventAvailable)
        {
            AppendQueue = appendQueue;
            events = new LeftArray<ICacheEvent<T, VT>>(0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            IsDispose = true;
            base.Dispose();
        }
        /// <summary>
        /// 添加缓存事件处理对象（缓存操作必须在队列中调用）
        /// </summary>
        /// <param name="cacheEvent"></param>
        public void AppendEvent(ICacheEvent<T, VT> cacheEvent)
        {
            if (cacheEvent == null) throw new ArgumentNullException();
        }
        /// <summary>
        /// 添加缓存事件处理对象
        /// </summary>
        /// <param name="cacheEvent"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void appendEvent(ICacheEvent<T, VT> cacheEvent)
        {
            if (getIndexOfEvent(cacheEvent) < 0) events.Add(cacheEvent);
        }
        /// <summary>
        /// 移除缓存事件处理对象（缓存操作必须在队列中调用）
        /// </summary>
        /// <param name="cacheEvent"></param>
        /// <returns></returns>
        public bool RemoveEvent(ICacheEvent<T, VT> cacheEvent)
        {
            int index = getIndexOfEvent(cacheEvent);
            if (index >= 0)
            {
                events.RemoveAt(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取缓存事件处理对象索引位置
        /// </summary>
        /// <param name="cacheEvent"></param>
        /// <returns></returns>
        private int getIndexOfEvent(ICacheEvent<T, VT> cacheEvent)
        {
            if (events.Count == 0) return -1;
            int count = events.Count;
            foreach (ICacheEvent<T, VT> matchEvent in events.Array)
            {
                if (object.ReferenceEquals(matchEvent, cacheEvent)) return events.Count - count;
                if (--count == 0) return -1;
            }
            return -1;
        }
        /// <summary>
        /// 根据数据库操作数据获取缓存数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        protected abstract VT? getCacheValue(T value);
#else
        protected abstract VT getCacheValue(T value);
#endif
        /// <summary>
        /// 添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        internal override void OnInserted(T value)
        {
            var cacheValue = value as VT;
            if (cacheValue == null)
            {
                cacheValue = DefaultConstructor<VT>.Constructor().notNull();
                tableWriter.CopyTo(value, cacheValue);
            }
            insert(cacheValue);
            foreach (ICacheEvent<T, VT> cacheEvent in events)
            {
                try
                {
                    cacheEvent.OnInserted(cacheValue);
                }
                catch (Exception exception)
                {
                    LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value"></param>
        protected abstract void insert(VT value);
        /// <summary>
        /// 更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        internal override void OnUpdated(T value, MemberMap<T> memberMap)
        {
            var cacheValue = getCacheValue(value);
            if (cacheValue == null)
            {
                LogHelper.ErrorIgnoreException($"没有找到待更新的缓存数据 {AutoCSer.JsonSerializer.Serialize(value)}", LogLevelEnum.Fatal);
                return;
            }
            foreach (ICacheEvent<T, VT> cacheEvent in events)
            {
                try
                {
                    cacheEvent.BeforeUpdate(cacheValue, memberMap);
                }
                catch (Exception exception)
                {
                    LogHelper.ExceptionIgnoreException(exception);
                }
            }
            tableWriter.CopyTo(value, cacheValue, memberMap);
            foreach (ICacheEvent<T, VT> cacheEvent in events)
            {
                try
                {
                    cacheEvent.OnUpdated(cacheValue, memberMap);
                }
                catch (Exception exception)
                {
                    LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override void OnDeleted(T value)
        {
            var cacheValue = getCacheValue(value);
            if (cacheValue == null)
            {
                LogHelper.ErrorIgnoreException($"没有找到待删除的缓存数据 {AutoCSer.JsonSerializer.Serialize(value)}", LogLevelEnum.Fatal);
                return;
            }
            foreach (ICacheEvent<T, VT> cacheEvent in events)
            {
                try
                {
                    cacheEvent.OnDeleted(cacheValue);
                }
                catch (Exception exception)
                {
                    LogHelper.ExceptionIgnoreException(exception);
                }
            }
            delete(cacheValue);
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value"></param>
        protected abstract void delete(VT value);
        /// <summary>
        /// 获取所有缓存数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <returns></returns>
        public IEnumerable<VT> GetValues(bool isClone = true)
        {
            if (isClone)
            {
                foreach (VT value in Values) yield return (VT)DefaultConstructor.CallMemberwiseClone(value);
            }
            else
            {
                foreach (VT value in Values) yield return value;
            }
        }
        /// <summary>
        /// 缓存数据转数组
        /// </summary>
        /// <returns></returns>
        internal T[] GetCacheArray()
        {
            T[] array = new T[Count];
            int arrayIndex = 0;
            foreach (VT value in Values) array[arrayIndex++] = value;
            return array;
        }
        /// <summary>
        /// 创建缓存数据同步回调流（缓存操作必须在队列中调用）
        /// </summary>
        /// <param name="callback">缓存数据回调委托</param>
        /// <returns></returns>
        public async Task CreateCallbackFlow(CommandServerKeepCallbackCount<CallbackValue<T>> callback)
        {
            if (callback == null) throw new ArgumentNullException();
            bool isCallback = true;
            var callbackFlow = default(CallbackFlow<T, VT>);
            try
            {
                if (IsDispose) return;
                callbackFlow = new CallbackFlow<T, VT>(this, callback);
                if (this.callbackFlow == null)
                {
                    appendEvent(callbackFlow);
                    this.callbackFlow = callbackFlow;
                }
                else
                {
                    this.callbackFlow.Next = callbackFlow;
                    callbackFlow = null;
                }
                isCallback = false;
            }
            finally
            {
                if (isCallback) callback.CancelKeep();
                if (callbackFlow != null) await callbackFlow.Start();
            }
        }
        /// <summary>
        /// 启动下一个缓存数据同步回调流
        /// </summary>
        /// <param name="isRemove">是否移除当前缓存数据同步回调流</param>
        /// <returns></returns>
        internal async Task NextCallbackFlow(bool isRemove)
        {
            bool isAppend = false;
            try
            {
                if (isRemove) RemoveEvent(callbackFlow.notNull());
                callbackFlow = callbackFlow.notNull().GetNext();
                if(callbackFlow != null && !IsDispose)
                {
                    appendEvent(callbackFlow);
                    isAppend = true;
                }
            }
            finally
            {
                if (callbackFlow != null)
                {
                    if (isAppend) await callbackFlow.Start();
                    else await callbackFlow.StartError();
                }
            }
        }
    }
}
