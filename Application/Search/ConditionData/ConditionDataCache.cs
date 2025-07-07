using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 非索引条件查询数据缓存
    /// </summary>
    /// <typeparam name="T">查询条件数据</typeparam>
    public abstract class ConditionDataCache<T> : IConditionDataCache<uint, T>, IConditionDataCache<int, T>
    {
        /// <summary>
        /// 查询条件数据缓存
        /// </summary>
        protected readonly ReusableHashCodeKeyDictionary<T> cache;
        /// <summary>
        /// 缓存数据访问锁
        /// </summary>
        protected readonly object cacheLock;
        /// <summary>
        /// 最大缓存数据数量
        /// </summary>
        protected readonly int maxCount;
        /// <summary>
        /// 查询数据缓存
        /// </summary>
        /// <param name="maxCount">最大缓存数据数量</param>
        protected ConditionDataCache(int maxCount)
        {
            this.maxCount = Math.Max(maxCount, 1);
            cacheLock = new object();
            cache = new ReusableHashCodeKeyDictionary<T>(maxCount + 1, ReusableDictionaryGroupTypeEnum.Roll);
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="key">查询数据关键字</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(uint key)
        {
            Monitor.Enter(cacheLock);
            try
            {
                return cache.Remove(key);
            }
            finally { Monitor.Exit(cacheLock); }
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="key">查询数据关键字</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(int key)
        {
            return Remove((uint)key);
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(uint key, T value)
        {
            Monitor.Enter(cacheLock);
            try
            {
                if (cache.Set(key, value, true) && cache.Count > maxCount) cache.RemoveRoll();
            }
            finally { Monitor.Exit(cacheLock); }
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int key, T value)
        {
            Set((uint)key, value);
        }
        ///// <summary>
        ///// 设置数据
        ///// </summary>
        ///// <param name="value"></param>
        //public void Set(T value)
        //{
        //    Set(getKey(value), value);
        //}
        ///// <summary>
        ///// 获取关键字
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected abstract uint getKey(T value);
        ///// <summary>
        ///// 获取所有数据
        ///// </summary>
        ///// <param name="queryParameter">查询参数</param>
        ///// <returns></returns>
        //public abstract EnumeratorCommand<T> GetAllData(PageParameter queryParameter);
    }
    /// <summary>
    /// 非索引条件查询数据缓存
    /// </summary>
    /// <typeparam name="KT">查询数据关键字类型</typeparam>
    /// <typeparam name="VT">查询条件数据</typeparam>
    public abstract class ConditionDataCache<KT, VT> : IConditionDataCache<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 查询条件数据缓存
        /// </summary>
        private readonly ReusableDictionary<KT, VT> cache;
        /// <summary>
        /// 缓存数据访问锁
        /// </summary>
        private readonly object cacheLock;
        /// <summary>
        /// 最大缓存数据数量
        /// </summary>
        private readonly int maxCount;
        /// <summary>
        /// 查询数据缓存
        /// </summary>
        /// <param name="maxCount">最大缓存数据数量</param>
        protected ConditionDataCache(int maxCount)
        {
            this.maxCount = Math.Max(maxCount, 1);
            cacheLock = new object();
            cache = new ReusableDictionary<KT, VT>(maxCount + 1, ReusableDictionaryGroupTypeEnum.Roll);
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="key">查询数据关键字</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(KT key)
        {
            Monitor.Enter(cacheLock);
            try
            {
                return cache.Remove(key);
            }
            finally { Monitor.Exit(cacheLock); }
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(KT key, VT value)
        {
            Monitor.Enter(cacheLock);
            try
            {
                if (cache.Set(key, value, true) && cache.Count > maxCount) cache.RemoveRoll();
            }
            finally { Monitor.Exit(cacheLock); }
        }
        ///// <summary>
        ///// 设置数据
        ///// </summary>
        ///// <param name="value"></param>
        //public void Set(VT value)
        //{
        //    Set(getKey(value), value);
        //}
        ///// <summary>
        ///// 获取关键字
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected abstract KT getKey(VT value);
        ///// <summary>
        ///// 获取所有数据
        ///// </summary>
        ///// <param name="queryParameter">查询参数</param>
        ///// <returns></returns>
        //public abstract EnumeratorCommand<VT> GetAllData(PageParameter queryParameter);
    }
}
