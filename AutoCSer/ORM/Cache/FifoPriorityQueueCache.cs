using AutoCSer.Extensions;
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
#if NetStandard21
        private readonly Func<KT, Task<VT?>> getValue;
#else
        private readonly Func<KT, Task<VT>> getValue;
#endif
        /// <summary>
        /// 获取缓存关键字委托
        /// </summary>
        private readonly Func<T, KT> getKey;
        /// <summary>
        /// 缓存数据
        /// </summary>
        private readonly ReusableDictionary<RandomKey<KT>, VT> cache;
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
        /// <param name="getKey">获取缓存关键字委托</param>
        /// <param name="getValue">从数据库获取数据委托</param>
#if NetStandard21
        internal FifoPriorityQueueCache(TableWriter<T> tableWriter, int capacity, Func<T, KT> getKey, Func<KT, Task<VT?>> getValue) : base(tableWriter, false)
#else
        internal FifoPriorityQueueCache(TableWriter<T> tableWriter, int capacity, Func<T, KT> getKey, Func<KT, Task<VT>> getValue) : base(tableWriter, false)
#endif
        {
            this.capacity = Math.Max(capacity, 1);
            this.getKey = getKey;
            this.getValue = getValue;
            cache = new ReusableDictionary<RandomKey<KT>, VT>(this.capacity, ReusableDictionaryGroupTypeEnum.Roll);
        }
        /// <summary>
        /// 先进先出队列缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="capacity">字典容器大小</param>
        internal FifoPriorityQueueCache(TableWriter<T, KT> tableWriter, int capacity) 
            : this(tableWriter, capacity, tableWriter.GetPrimaryKey, tableWriter.GetByPrimaryKey<VT>)
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
#if NetStandard21
        public async Task<VT?> Get(KT key, bool isClone = true)
#else
        public async Task<VT> Get(KT key, bool isClone = true)
#endif
        {
            var value = default(VT);
            RandomKey<KT> randomKey = key;
            if (cache.TryGetValue(randomKey, out value)) return isClone ? (VT)DefaultConstructor.CallMemberwiseClone(value) : value;
            value = await getValue(key);
            if (value != null)
            {
                cache.Set(randomKey, value, true);
                if (cache.Count > capacity) cache.RemoveRoll();
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
            var cacheValue = value as VT;
            if (cacheValue == null)
            {
                cacheValue = DefaultConstructor<VT>.Constructor().notNull();
                tableWriter.CopyTo(value, cacheValue);
            }
            cache.Set(getKey(cacheValue), cacheValue, true);
            if (cache.Count > capacity) cache.RemoveRoll();
        }
        /// <summary>
        /// 更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        internal override void OnUpdated(T value, MemberMap<T> memberMap)
        {
            var cacheValue = default(VT);
            if (cache.TryGetValue(getKey(value), out cacheValue, true)) tableWriter.CopyTo(value, cacheValue, memberMap);
        }
        /// <summary>
        /// 删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override void OnDeleted(T value)
        {
            var cacheValue = default(VT);
            cache.Remove(getKey(value), out cacheValue);
        }
    }
}
