using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Configuration;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存节点
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    public abstract class BlockIndexDataCacheNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 少量数据最小数量
        /// </summary>
        internal const int LittleMinCount = 8;

        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        protected readonly BlockIndexDataCache<T> cache;
        /// <summary>
        /// 索引数据磁盘块索引信息节点
        /// </summary>
        protected BlockIndexData<T> data;
        /// <summary>
        /// 获取数据关键字更新版本
        /// </summary>
        private int getKeyVersion;
        /// <summary>
        /// 获取更新关键字集合保持回调版本
        /// </summary>
        private int getChangeKeyVersion;
        /// <summary>
        /// 关键字更新版本
        /// </summary>
        internal int ChangeKeyVersion;
        /// <summary>
        /// 返回 true 表示索引数据版本有效
        /// </summary>
        private bool isVersion
        {
            get
            {
                return ((getKeyVersion ^ ChangeKeyVersion) | (getChangeKeyVersion ^ cache.GetChangeKeyVersion)) == 0;
            }
        }
        /// <summary>
        /// 哈希节点类型
        /// </summary>
        internal IndexDataTypeEnum Type;
        /// <summary>
        /// 预估数据数量
        /// </summary>
        internal int EstimatedCount
        {
            get
            {
                switch (Type)
                {
                    case IndexDataTypeEnum.BlockIndexData: return data.ValueCount;
                    case IndexDataTypeEnum.Little: return littleCount;
                    case IndexDataTypeEnum.Many: return manyCount;
                    default: return data.EstimatedCount;
                }
            }
        }
        /// <summary>
        /// 少量数据集合数据量
        /// </summary>
        protected abstract int littleCount { get; }
        /// <summary>
        /// 大量数据集合数据量
        /// </summary>
        protected abstract int manyCount { get; }
        /// <summary>
        /// 少量数据索引
        /// </summary>
        internal abstract IIndex<T> LittleIndex { get; }
        /// <summary>
        /// 大量数据索引
        /// </summary>
        internal abstract IIndex<T> ManyIndex { get; }
        /// <summary>
        /// 是否已删除节点
        /// </summary>
        internal bool IsRemoved;
        /// <summary>
        /// 索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="cache"></param>
        protected BlockIndexDataCacheNode(BlockIndexDataCache<T> cache)
        {
            this.cache = cache;
        }
        /// <summary>
        /// 获取节点缓存加载访问锁
        /// </summary>
        /// <returns></returns>
        internal abstract SemaphoreSlimCache GetSemaphoreSlimCache();
        /// <summary>
        /// 释放获取节点缓存加载访问锁
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        internal abstract void Release(SemaphoreSlimCache semaphoreSlim);
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <returns></returns>
        internal async Task<ResponseResult> GetBlockIndexData()
        {
            if (Type == IndexDataTypeEnum.None)
            {
                SemaphoreSlimCache semaphoreSlim = GetSemaphoreSlimCache();
                await semaphoreSlim.Lock.WaitAsync();
                try
                {
                    if (Type == IndexDataTypeEnum.None) return await getBlockIndexData();
                }
                finally { Release(semaphoreSlim); }
            }
            return CallStateEnum.Success;
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <returns></returns>
        protected abstract Task<ResponseResult> getBlockIndexData();
        /// <summary>
        /// 设置索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="getKeyVersion"></param>
        /// <param name="getChangeKeyVersion"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool Set(int getKeyVersion, int getChangeKeyVersion, BlockIndexData<T> data)
        {
            if (getKeyVersion == ChangeKeyVersion)
            {
                this.getChangeKeyVersion = getChangeKeyVersion;
                this.getKeyVersion = getKeyVersion;
                int cacheValueCount = 0, newValueCount = data.CacheValueCount;
                if (!IsRemoved)
                {
                    switch (Type)
                    {
                        case IndexDataTypeEnum.NotLoaded:
                        case IndexDataTypeEnum.BlockIndexData:
                            cacheValueCount = this.data.CacheValueCount;
                            break;
                        case IndexDataTypeEnum.Little: cacheValueCount = getRemoveLittleCount(); break;
                        case IndexDataTypeEnum.Many: cacheValueCount = getRemoveManyCount(); break;
                    }
                }
                this.data = data;
                if (data.BlockIndexTotalCount == 0)
                {
                    if (newValueCount >= LittleMinCount)
                    {
                        bool isSetValue = false;
                        try
                        {
                            if (newValueCount <= RemoveMarkHashSetCapacity.MaxCapacity)
                            {
                                setLittleValues();
                                Type = IndexDataTypeEnum.Little;
                            }
                            else
                            {
                                setManyValues();
                                Type = IndexDataTypeEnum.Many;
                            }
                            data.SetEmptyValues();
                            isSetValue = true;
                        }
                        finally
                        {
                            if (!isSetValue)
                            {
                                this.data = default(BlockIndexData<T>);
                                Type = IndexDataTypeEnum.None;
                                if (cacheValueCount != 0) cache.RemoveValueCount(this, -cacheValueCount);
                            }
                        }
                    }
                    else Type = IndexDataTypeEnum.BlockIndexData;
                }
                else Type = IndexDataTypeEnum.NotLoaded;
                if (!IsRemoved)
                {
                    if ((newValueCount -= cacheValueCount) > 0) cache.AddValueCount(this, newValueCount);
                    else cache.RemoveValueCount(this, newValueCount);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取少量数据集合数据量
        /// </summary>
        /// <returns></returns>
        protected abstract int getRemoveLittleCount();
        /// <summary>
        /// 获取大量数据集合数据量
        /// </summary>
        /// <returns></returns>
        protected abstract int getRemoveManyCount();
        /// <summary>
        /// 设置为少量数据索引
        /// </summary>
        protected abstract void setLittleValues();
        /// <summary>
        /// 设置为大量数据索引
        /// </summary>
        protected abstract void setManyValues();
        /// <summary>
        /// 获取删除节点的缓存数据数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetRemoveCacheValueCount()
        {
            IsRemoved = true;
            switch (Type)
            {
                case IndexDataTypeEnum.Little: return littleCount;
                case IndexDataTypeEnum.Many: return manyCount;
                default: return data.CacheValueCount;
            }
        }
        /// <summary>
        /// 获取索引
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal IIndex<T>? GetLoadedIndex()
#else
        internal IIndex<T> GetLoadedIndex()
#endif
        {
            return isVersion ? getIndex() : null;
        }
        /// <summary>
        /// 获取索引
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private IIndex<T>? getIndex()
#else
        private IIndex<T> getIndex()
#endif
        {
            switch (Type)
            {
                case IndexDataTypeEnum.BlockIndexData: return ArrayIndex<T>.Get(data.Values);
                case IndexDataTypeEnum.Little: return LittleIndex;
                case IndexDataTypeEnum.Many: return ManyIndex;
            }
            return null;
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <returns></returns>
        internal async Task<ResponseResult<IIndex<T>>> Load()
        {
            var manyIndex = default(IIndex<T>);
            BlockIndexData<T> manyData = default(BlockIndexData<T>);
            SemaphoreSlimCache semaphoreSlim = GetSemaphoreSlimCache();
            await semaphoreSlim.Lock.WaitAsync();
            try
            {
                switch (Type)
                {
                    case IndexDataTypeEnum.None:
                        ResponseResult result = await getBlockIndexData();
                        if (!result.IsSuccess) return result;
                        if (Type != IndexDataTypeEnum.NotLoaded) return new ResponseResult<IIndex<T>>(getIndex());
                        break;
                    case IndexDataTypeEnum.NotLoaded: break;
                    default:
                        if (isVersion) return new ResponseResult<IIndex<T>>(getIndex());
                        if (Type == IndexDataTypeEnum.Many)
                        {
                            manyData = data;
                            manyIndex = ManyIndex;
                        }
                        else manyData = new BlockIndexData<T>(BlockIndex.BinarySerializeNullValue);
                        result = await getBlockIndexData();
                        if (!result.IsSuccess) return result;
                        if (Type != IndexDataTypeEnum.NotLoaded) return new ResponseResult<IIndex<T>>(getIndex());
                        break;
                }
                try
                {
                    LeftArray<PersistenceNode<T>> nodes = new LeftArray<PersistenceNode<T>>(0);
                    PersistenceNode<T> node = data.GetPersistenceNode();
                    int totalCount = data.BlockIndexTotalCount;
                    nodes.Add(node);
                    do
                    {
                        if (manyData.BlockIndex.Equals(node.BlockIndex))
                        {
                            Load(nodes, manyIndex.notNull());
                            manyIndex = null;
                            return CallStateEnum.Success;
                        }
                        ReadResult<PersistenceNode<T>> result = await new ReadBinaryAwaiter<PersistenceNode<T>>(cache.GetDiskBlockClient(node.BlockIndex), node.BlockIndex);
                        if (result.ReturnType != Net.CommandClientReturnTypeEnum.Success) return new ResponseResult<IIndex<T>>(result.ReturnType, result.ErrorMessage);
                        if (result.BufferState != ReadBufferStateEnum.Success) return CallStateEnum.CustomStateError;
                        nodes.Add(node = result.Value);
                        totalCount -= node.Values.Length;
                    }
                    while (totalCount > 0);
                    Load(nodes, data.BlockIndexValueCount + data.ValueCount);
                    manyIndex = null;
                    return CallStateEnum.Success;
                }
                catch(Exception exception)
                {
                    await AutoCSer.LogHelper.Exception(exception);
                }
                finally
                {
                    if (manyIndex != null)
                    {
                        --getKeyVersion;
                        int newValueCount = manyIndex.Count - data.CacheValueCount;
                        data = manyData;
                        SetManyValues(manyIndex);
                        Type = IndexDataTypeEnum.Many;
                        if (!IsRemoved)
                        {
                            if (newValueCount > 0) cache.AddValueCount(this, newValueCount);
                            else cache.RemoveValueCount(this, newValueCount);
                        }
                    }
                }
                if (manyIndex != null) return new ResponseResult<IIndex<T>>(manyIndex);
                return CallStateEnum.CustomException;
            }
            finally { Release(semaphoreSlim); }
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="capacity"></param>
        internal abstract void Load(LeftArray<PersistenceNode<T>> nodes, int capacity);
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="manyIndex"></param>
        internal abstract void Load(LeftArray<PersistenceNode<T>> nodes, IIndex<T> manyIndex);
        /// <summary>
        /// 加载数据失败恢复数据
        /// </summary>
        /// <param name="manyIndex"></param>
        internal abstract void SetManyValues(IIndex<T> manyIndex);

        /// <summary>
        /// 空索引数据
        /// </summary>
        internal static readonly RemoveMarkHashSet<T> EmptyRemoveMarkHashSet = new RemoveMarkHashSet<T>(RemoveMarkHashSetCapacity.DefaultLink);
        /// <summary>
        /// 空索引数据
        /// </summary>
        internal static readonly HashSet<T> EmptyHashSet = new HashSet<T>();
        /// <summary>
        /// 空索引条件
        /// </summary>
        internal static readonly Task<ResponseResult<IIndexCondition<T>>> NullIndexCondition = Task.FromResult(new ResponseResult<IIndexCondition<T>>(default(IIndexCondition<T>)));
    }
    /// <summary>
    /// 索引数据磁盘块索引缓存节点
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public abstract class BlockIndexDataCacheNode<KT, VT> : BlockIndexDataCacheNode<VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 关键字
        /// </summary>
        internal readonly KT Key;
        /// <summary>
        /// 索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="cache">索引数据磁盘块索引缓存</param>
        /// <param name="key">keyword</param>
        protected BlockIndexDataCacheNode(BlockIndexDataCache<KT, VT> cache, KT key) : base(cache)
        {
            this.Key = key;
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <returns></returns>
        protected override Task<ResponseResult> getBlockIndexData()
        {
            return ((BlockIndexDataCache<KT, VT>)cache).GetBlockIndexData(this);
        }
        /// <summary>
        /// 获取节点缓存加载访问锁
        /// </summary>
        /// <returns></returns>
        internal override SemaphoreSlimCache GetSemaphoreSlimCache()
        {
            return ((BlockIndexDataCache<KT, VT>)cache).GetSemaphoreSlimCache(Key);
        }
        /// <summary>
        /// 释放获取节点缓存加载访问锁
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        internal override void Release(SemaphoreSlimCache semaphoreSlim)
        {
            ((BlockIndexDataCache<KT, VT>)cache).Release(semaphoreSlim, Key);
        }
    }
}
