using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 字典事件缓存
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="VT">缓存数据类型</typeparam>
    /// <typeparam name="KT">缓存关键字类型</typeparam>
    public class DictionaryEventCache<T, VT, KT> : EventCache<T, VT>, ICachePersistence<T, VT, KT>
        where T : class
        where VT : class, T
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 获取缓存关键字委托
        /// </summary>
        private readonly Func<T, KT> getKey;
        /// <summary>
        /// 缓存数据
        /// </summary>
        private readonly Dictionary<RandomKey<KT>, VT> cache;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public override int Count { get { return cache.Count; } }
        /// <summary>
        /// 获取所有缓存关键字（缓存操作必须在队列中调用）
        /// </summary>
        public IEnumerable<KT> Keys
        {
            get 
            {
                foreach (RandomKey<KT> key in cache.Keys) yield return key;
            }
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        internal override IEnumerable<VT> Values { get { return cache.Values; } }
        /// <summary>
        /// 字典事件缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isEventAvailable">缓存对象是否事件可用</param>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="capacity">字典初始容器大小</param>
        /// <param name="getKey">获取缓存关键字委托</param>
        internal DictionaryEventCache(TableWriter<T> tableWriter, bool isEventAvailable, Action<Func<Task>> appendQueue, int capacity, Func<T, KT> getKey) : base(tableWriter, isEventAvailable, appendQueue)
        {
            this.getKey = getKey;
            cache = DictionaryCreator<RandomKey<KT>>.Create<VT>(capacity);
        }
        /// <summary>
        /// 字典事件缓存
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isEventAvailable">缓存对象是否事件可用</param>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="capacity">字典初始容器大小</param>
        internal DictionaryEventCache(TableWriter<T, KT> tableWriter, bool isEventAvailable, Action<Func<Task>> appendQueue, int capacity) : this(tableWriter, isEventAvailable, appendQueue, capacity, tableWriter.GetPrimaryKey)
        {
        }
        /// <summary>
        /// 缓存数据初始化
        /// </summary>
        /// <returns></returns>
        internal async Task Initialize()
        {
            if (cache.Count != 0) cache.Clear();
#if NetStandard21
            await using (IAsyncEnumerator<VT> selectEnumerator = await tableWriter.CreateQuery(null, false).Select<VT>())
            {
                while (await selectEnumerator.MoveNextAsync()) insert(selectEnumerator.Current);
            }
#else
            IEnumeratorTask<VT> selectEnumerator = await tableWriter.CreateQuery(null, false).Select<VT>();
            try
            {
                while (await selectEnumerator.MoveNextAsync()) insert(selectEnumerator.Current);
            }
            finally { await selectEnumerator.DisposeAsync(); }
#endif
        }
        /// <summary>
        /// 根据关键字获取缓存数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <param name="key">缓存关键字</param>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <returns>没有找到缓存对象时返回 null</returns>
#if NetStandard21
        public VT? Get(KT key, bool isClone = true)
#else
        public VT Get(KT key, bool isClone = true)
#endif
        {
            var value = default(VT);
            if (cache.TryGetValue(key, out value))
            {
                return isClone ? (VT)DefaultConstructor.CallMemberwiseClone(value) : value;
            }
            return null;
        }
        /// <summary>
        /// 根据数据库操作数据获取缓存数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        protected override VT? getCacheValue(T value)
#else
        protected override VT getCacheValue(T value)
#endif
        {
            var cacheValue = default(VT);
            return cache.TryGetValue(getKey(value), out cacheValue) ? cacheValue : null;
        }
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value"></param>
        protected override void insert(VT value)
        {
            cache.Add(getKey(value), value);
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value"></param>
        protected override void delete(VT value)
        {
            cache.Remove(getKey(value));
        }
        /// <summary>
        /// 根据缓存更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="isClone"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        async Task<VT?> ICachePersistence<T, VT, KT>.Update(VT value, MemberMap<T>? memberMap, bool isClone, Transaction? transaction)
#else
        async Task<VT> ICachePersistence<T, VT, KT>.Update(VT value, MemberMap<T> memberMap, bool isClone, Transaction transaction)
#endif
        {
            tableWriter.CheckReadOnly(ref transaction);
            var cacheValue = default(VT);
            KT key = getKey(value);
            if(cache.TryGetValue(key, out cacheValue) && await tableWriter.Update(value, memberMap, cacheValue, transaction))
            {
                return isClone ? (VT)DefaultConstructor.CallMemberwiseClone(value) : value;
            }
            return null;
        }
        /// <summary>
        /// 根据缓存更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isClone"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        async Task<VT?> ICachePersistence<T, VT, KT>.Update(MemberMapValue<T, VT> value, bool isClone, Transaction? transaction)
#else
        async Task<VT> ICachePersistence<T, VT, KT>.Update(MemberMapValue<T, VT> value, bool isClone, Transaction transaction)
#endif
        {
            tableWriter.CheckReadOnly(ref transaction);
            return await ((ICachePersistence<T, VT, KT>)this).Update(value.Value.notNull(), value.MemberMap, isClone, transaction);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        async Task<VT?> ICachePersistence<T, VT, KT>.Delete(KT key, Transaction? transaction)
#else
        async Task<VT> ICachePersistence<T, VT, KT>.Delete(KT key, Transaction transaction)
#endif
        {
            tableWriter.CheckReadOnly(ref transaction);
            var value = default(VT);
            return cache.TryGetValue(key, out value) && await tableWriter.Delete(value, isEventAvailable, transaction) ? value : null;
        }
    }
}
