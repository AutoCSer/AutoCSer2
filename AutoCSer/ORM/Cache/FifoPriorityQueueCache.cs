using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 先进先出队列缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="VT"></typeparam>
    /// <typeparam name="KT"></typeparam>
    public sealed class FifoPriorityQueueCache<T, VT, KT> : EventCache<T>
        where T : class
        where VT : class, T
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 从数据库获取数据委托
        /// </summary>
        private readonly Func<KT, Task<VT>> getValue;
        /// <summary>
        /// 获取缓存关键字委托
        /// </summary>
        private readonly Func<T, KT> getKey;
        /// <summary>
        /// 缓存数据
        /// </summary>
        private readonly FifoPriorityQueue<RandomKey<KT>, VT> cache;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get { return cache.Count; } }
        /// <summary>
        /// 获取所有缓存关键字
        /// </summary>
        public IEnumerable<KT> Keys
        {
            get
            {
                foreach (RandomKey<KT> key in cache.Keys) yield return key;
            }
        }
        /// <summary>
        /// 字典容器大小
        /// </summary>
        private readonly int capacity;
        /// <summary>
        /// 先进先出队列缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="capacity">字典容器大小</param>
        /// <param name="isClear">是否需要清除数据</param>
        /// <param name="getKey">获取缓存关键字委托</param>
        /// <param name="getValue">从数据库获取数据委托</param>
        internal FifoPriorityQueueCache(TableWriter<T> tableWriter, int capacity, bool isClear, Func<T, KT> getKey, Func<KT, Task<VT>> getValue) : base(tableWriter, false)
        {
            this.capacity = Math.Max(capacity, 1);
            this.getKey = getKey;
            this.getValue = getValue;
            cache = new FifoPriorityQueue<RandomKey<KT>, VT>(this.capacity, isClear);
        }
        /// <summary>
        /// 先进先出队列缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="capacity">字典容器大小</param>
        /// <param name="isClear">是否需要清除数据</param>
        internal FifoPriorityQueueCache(TableWriter<T, KT> tableWriter, int capacity, bool isClear) : this(tableWriter, capacity, isClear, tableWriter.GetPrimaryKey, tableWriter.GetByPrimaryKey<VT>)
        {
        }
        /// <summary>
        /// 获取所有缓存数据
        /// </summary>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <returns></returns>
        public IEnumerable<VT> GetValues(bool isClone = true)
        {
            if (isClone)
            {
                foreach (VT value in cache.Values) yield return (VT)DefaultConstructor.CallMemberwiseClone(value);
            }
            else
            {
                foreach (VT value in cache.Values) yield return value;
            }
        }
        /// <summary>
        /// 根据关键字获取缓存数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <param name="key">缓存关键字</param>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <returns></returns>
        public async Task<VT> Get(KT key, bool isClone = true)
        {
            VT value;
            RandomKey<KT> randomKey = key;
            if (cache.TryGetValue(ref randomKey, out value)) return isClone ? (VT)DefaultConstructor.CallMemberwiseClone(value) : value;
            value = await getValue(key);
            if (value != null)
            {
                cache.Set(ref randomKey, value);
                if (cache.Count > capacity) cache.Pop();
                return isClone ? (VT)DefaultConstructor.CallMemberwiseClone(value) : value;
            }
            return null;
        }
        /// <summary>
        /// 添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        internal override void OnInserted(T value)
        {
            VT cacheValue = value as VT;
            if (cacheValue == null)
            {
                cacheValue = DefaultConstructor<VT>.Constructor();
                tableWriter.CopyTo(value, cacheValue);
            }
            RandomKey<KT> key = getKey(cacheValue);
            cache.Set(ref key, cacheValue);
            if (cache.Count > capacity) cache.Pop();
        }
        /// <summary>
        /// 更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        internal override void OnUpdated(T value, MemberMap<T> memberMap)
        {
            VT cacheValue;
            RandomKey<KT> key = getKey(value);
            if (cache.TryGetValue(ref key, out cacheValue)) tableWriter.CopyTo(value, cacheValue, memberMap);
        }
        /// <summary>
        /// 删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override void OnDeleted(T value)
        {
            VT cacheValue;
            RandomKey<KT> key = getKey(value);
            cache.Remove(ref key, out cacheValue);
        }
    }
}
