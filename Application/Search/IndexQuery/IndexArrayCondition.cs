using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 索引集合条件
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public sealed class IndexArrayCondition<T> : IIndexCondition<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引条件集合
        /// </summary>
        private LeftArray<IIndexCondition<T>> conditions;
        /// <summary>
        /// 预估数据数量
        /// </summary>
        public int EstimatedCount { get; private set; }
        /// <summary>
        /// 已经加载数据数量
        /// </summary>
        private int loadedCount;
        /// <summary>
        /// 是否已经加载数据
        /// </summary>
        bool IIndexCondition<T>.IsLoaded { get { return loadedCount == conditions.Length; } }
        /// <summary>
        /// 索引合并操作类型
        /// </summary>
        private readonly IndexMergeTypeEnum type;
        /// <summary>
        /// 索引集合条件
        /// </summary>
        /// <param name="conditions">索引条件集合</param>
        /// <param name="type">索引合并操作类型</param>
        public IndexArrayCondition(LeftArray<IIndexCondition<T>> conditions, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
        {
            if (conditions.Length == 0) throw new ArgumentNullException();
            this.type = type;
            long unionCount = 0;
            IIndexCondition<T>[] conditionArray = conditions.Array;
            for (int conditionIndex = 0, estimatedCount; conditionIndex != conditions.Length; ++conditionIndex)
            {
                IIndexCondition<T> indexCondition = conditionArray[conditionIndex];
                estimatedCount = indexCondition.EstimatedCount;
                if (estimatedCount != 0)
                {
                    unionCount += estimatedCount;
                    if (indexCondition.IsLoaded) ++loadedCount;
                }
                else
                {
                    if (type == IndexMergeTypeEnum.Intersection)
                    {
                        loadedCount = 0;
                        return;
                    }
                    conditions.RemoveToEndOnly(conditionIndex--);
                }
            }
            if (conditions.Length != 0)
            {
                this.conditions = conditions;
                setEstimatedCount(unionCount);
            }
        }
        /// <summary>
        /// 索引集合条件
        /// </summary>
        /// <param name="conditions">索引条件集合</param>
        /// <param name="type">索引合并操作类型</param>
        public IndexArrayCondition(IIndexCondition<T>[] conditions, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection) : this(new LeftArray<IIndexCondition<T>>(conditions), type) { }
        /// <summary>
        /// 设置预估数据数量
        /// </summary>
        /// <param name="unionCount"></param>
        private void setEstimatedCount(long unionCount)
        {
            switch (type)
            {
                case IndexMergeTypeEnum.Intersection:
                case IndexMergeTypeEnum.IntersectionNotEmpty:
                    if (conditions.Length > 1) AutoCSer.Extensions.ArraySort.QuickSort(conditions.Array, conditions.Length, p => p.EstimatedCount);
                    EstimatedCount = conditions.Array[0].EstimatedCount;
                    break;
                case IndexMergeTypeEnum.Union: EstimatedCount = (int)Math.Min(unionCount, int.MaxValue); break;
            }
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <returns></returns>
        async Task<ResponseResult> IIndexCondition<T>.Load()
        {
            long unionCount = 0;
            IIndexCondition<T>[] conditionArray = conditions.Array;
            ResponseResult result = CallStateEnum.Success;
            int isLoaded = 0;
            for (int conditionIndex = 0, estimatedCount; conditionIndex != conditions.Length; ++conditionIndex)
            {
                IIndexCondition<T> indexCondition = conditionArray[conditionIndex];
                if (!indexCondition.IsLoaded)
                {
                    isLoaded = 1;
                    ResponseResult loadResult = await indexCondition.Load();
                    if (loadResult.IsSuccess)
                    {
                        estimatedCount = indexCondition.EstimatedCount;
                        if (estimatedCount == 0 && type == IndexMergeTypeEnum.Intersection)
                        {
                            conditions.Length = 0;
                            break;
                        }
                    }
                    else
                    {
                        result = loadResult;
                        estimatedCount = 0;
                    }
                    if(estimatedCount == 0) conditions.RemoveToEndOnly(conditionIndex--);
                    else unionCount += estimatedCount;
                }
            }
            if (isLoaded != 0)
            {
                if ((loadedCount = conditions.Length) == 0)
                {
                    EstimatedCount = 0;
                    return result;
                }
                setEstimatedCount(unionCount);
            }
            return CallStateEnum.Success;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IIndexCondition<T>.Contains(T value)
        {
            int count = conditions.Length;
            if (count != 0)
            {
                if (type != IndexMergeTypeEnum.Union)
                {
                    foreach (IIndexCondition<T> condition in conditions.Array)
                    {
                        if (!condition.Contains(value)) return false;
                        if (--count == 0) return true;
                    }
                }
                foreach (IIndexCondition<T> condition in conditions.Array)
                {
                    if (condition.Contains(value)) return true;
                    if (--count == 0) break;
                }
            }
            return false;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="hashSet"></param>
        void IIndexCondition<T>.GetLoaded(QueryCondition<T> condition, BufferHashSet<T> hashSet)
        {
            switch (conditions.Length)
            {
                case 0: return;
                case 1: conditions.Array[0].GetLoaded(condition, hashSet); return;
                default:
                    if (type != IndexMergeTypeEnum.Union)
                    {
                        ArrayBuffer<T> buffer = GetLoaded(condition);
                        int count = buffer.Count;
                        if (count != 0)
                        {
                            foreach (T value in buffer.Array)
                            {
                                hashSet.Add(value);
                                if (--count == 0) break;
                            }
                        }
                        buffer.Free();
                    }
                    else
                    {
                        int count = conditions.Length;
                        foreach (IIndexCondition<T> indexCondition in conditions.Array)
                        {
                            indexCondition.GetLoaded(condition, hashSet);
                            if (--count == 0) break;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<ArrayBuffer<T>> IIndexCondition<T>.Get(QueryCondition<T> condition)
        {
            switch (conditions.Length)
            {
                case 0: return condition.GetNullBuffer();
                case 1: return conditions.Array[0].Get(condition);
                default:
                    switch (type)
                    {
                        case IndexMergeTypeEnum.IntersectionNotEmpty: return intersectionNotEmpty(condition);
                        case IndexMergeTypeEnum.Union: return union(condition);
                        default: return intersection(condition);
                    }
            }
        }
        /// <summary>
        /// 交集 AND
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersection(QueryCondition<T> condition)
        {
            return await intersection(condition, await conditions.Array[0].Get(condition));
        }
        /// <summary>
        /// 交集 AND
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersection(QueryCondition<T> condition, ArrayBuffer<T> buffer)
        {
            int valueCount = buffer.GetCount();
            if (valueCount != 0)
            {
                int conditionCount = conditions.Length;
                IIndexCondition<T>[] conditionArray = conditions.Array;
                foreach (T value in buffer.Array)
                {
                    for (int conditionIndex = 1; conditionIndex != conditionCount; ++conditionIndex)
                    {
                        IIndexCondition<T> indexCondition = conditionArray[conditionIndex];
                        if (indexCondition.IsLoaded)
                        {
                            if (!indexCondition.Contains(value)) goto NEXTVALUE;
                        }
                        else
                        {
                            ResponseResult result = await indexCondition.Load();
                            if (result.IsSuccess)
                            {
                                if (indexCondition.EstimatedCount == 0)
                                {
                                    buffer.Free();
                                    return condition.GetNullBuffer().Result;
                                }
                                if (!indexCondition.Contains(value)) goto NEXTVALUE;
                            }
                            else
                            {
                                conditions.RemoveAtOnly(conditionIndex--);
                                conditionCount = conditions.Length;
                            }
                        }
                    }
                    buffer.UnsafeAdd(value);
                    NEXTVALUE:
                    if (--valueCount == 0) break;
                }
                if (buffer.Count != 0) return buffer;
            }
            buffer.Free();
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 交集 AND（忽略空集，必须存在一个非空集）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersectionNotEmpty(QueryCondition<T> condition)
        {
            IIndexCondition<T>[] conditionArray = conditions.Array;
            int index = 0, conditionCount = conditions.Length;
            do
            {
                ArrayBuffer<T> buffer = await conditionArray[index].Get(condition);
                if (buffer.Count != 0)
                {
                    if (++index == conditionCount) return buffer;
                    return await intersectionNotEmpty(condition, buffer, index);
                }
                buffer.Free();
            }
            while (++index != conditionCount);
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 交集 AND（忽略空集，必须存在一个非空集）
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersectionNotEmpty(QueryCondition<T> condition, ArrayBuffer<T> buffer, int index)
        {
            IIndexCondition<T>[] conditionArray = conditions.Array;
            int valueCount = buffer.GetCount(), conditionCount = conditions.Length;
            foreach (T value in buffer.Array)
            {
                for (int conditionIndex = index; conditionIndex != conditionCount; ++conditionIndex)
                {
                    IIndexCondition<T> indexCondition = conditionArray[conditionIndex];
                    if (indexCondition.IsLoaded)
                    {
                        if (!indexCondition.Contains(value)) goto NEXTVALUE;
                    }
                    else
                    {
                        ResponseResult result = await indexCondition.Load();
                        if (indexCondition.EstimatedCount != 0 && result.IsSuccess)
                        {
                            if (!indexCondition.Contains(value)) goto NEXTVALUE;
                        }
                        else
                        {
                            conditions.RemoveAtOnly(conditionIndex--);
                            conditionCount = conditions.Length;
                        }
                    }
                }
                buffer.UnsafeAdd(value);
            NEXTVALUE:
                if (--valueCount == 0) break;
            }
            if (buffer.Count != 0) return buffer;
            buffer.Free();
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> union(QueryCondition<T> condition)
        {
            IIndexCondition<T>[] conditionArray = conditions.Array;
            int capacity = 0;
            for (int conditionIndex = 0; conditionIndex != conditions.Length; ++conditionIndex)
            {
                IIndexCondition<T> indexCondition = conditionArray[conditionIndex];
                int estimatedCount = indexCondition.IsLoaded || (await indexCondition.Load()).IsSuccess ? indexCondition.EstimatedCount : 0;
                if (estimatedCount != 0) capacity = Math.Max(capacity, estimatedCount);
                else conditions.RemoveToEndOnly(conditionIndex--);
            }
            if(capacity != 0) return unionLoaded(condition, capacity);
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        private ArrayBuffer<T> unionLoaded(QueryCondition<T> condition, int capacity)
        {
            BufferHashSet<T> hashSet = HashSetPool<T>.GetHashSet(condition.GetHashSetPool(), capacity);
            int count = conditions.Length;
            foreach (IIndexCondition<T> indexCondition in conditions.Array)
            {
                indexCondition.GetLoaded(condition, hashSet);
                if (--count == 0) break;
            }
            ArrayBuffer<T> buffer = hashSet.GetArrayBuffer(condition);
            hashSet.Free();
            return buffer;
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> GetLoaded(QueryCondition<T> condition)
        {
            switch (conditions.Length)
            {
                case 0: return condition.GetNullBuffer().Result;
                case 1: return conditions.Array[0].GetLoaded(condition);
                default:
                    if (type != IndexMergeTypeEnum.Union)
                    {
                        IIndexCondition<T>[] conditionArray = conditions.Array;
                        ArrayBuffer<T> buffer = conditionArray[0].GetLoaded(condition);
                        int valueCount = buffer.GetCount();
                        if (valueCount != 0)
                        {
                            int conditionCount = conditions.Length, conditionIndex;
                            foreach (T value in buffer.Array)
                            {
                                conditionIndex = 1;
                                do
                                {
                                    if (!conditionArray[conditionIndex].Contains(value)) goto NEXTVALUE;
                                }
                                while (++conditionIndex != conditionCount);
                                buffer.UnsafeAdd(value);
                            NEXTVALUE:
                                if (--valueCount == 0) break;
                            }
                            if (buffer.Count != 0) return buffer;
                        }
                        buffer.Free();
                        return condition.GetNullBuffer().Result;
                    }
                    else
                    {
                        int count = conditions.Length, capacity = 0;
                        foreach (IIndexCondition<T> indexCondition in conditions.Array)
                        {
                            if (indexCondition.EstimatedCount > capacity) capacity = indexCondition.EstimatedCount;
                            if (--count == 0) break;
                        }
                        if (capacity != 0) return unionLoaded(condition, capacity);
                        return condition.GetNullBuffer().Result;
                    }
            }
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<ArrayBuffer<T>> IIndexCondition<T>.GetFilter(QueryCondition<T> condition)
        {
            switch (conditions.Length)
            {
                case 0: return condition.GetNullBuffer();
                case 1: return conditions.Array[0].GetFilter(condition);
                default:
                    switch (type)
                    {
                        case IndexMergeTypeEnum.IntersectionNotEmpty: return intersectionNotEmptyFilter(condition);
                        case IndexMergeTypeEnum.Union: return unionFilter(condition);
                        default: return intersectionFilter(condition);
                    }
            }
        }
        /// <summary>
        /// 交集 AND
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersectionFilter(QueryCondition<T> condition)
        {
            return await intersection(condition, await conditions.Array[0].GetFilter(condition));
        }
        /// <summary>
        /// 交集 AND（忽略空集，必须存在一个非空集）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersectionNotEmptyFilter(QueryCondition<T> condition)
        {
            IIndexCondition<T>[] conditionArray = conditions.Array;
            int index = 0, conditionCount = conditions.Length;
            do
            {
                ArrayBuffer<T> buffer = await conditionArray[index].GetFilter(condition);
                if (buffer.Count != 0)
                {
                    if (++index == conditionCount) return buffer;
                    return await intersectionNotEmpty(condition, buffer, index);
                }
                buffer.Free();
            }
            while (++index != conditionCount);
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> unionFilter(QueryCondition<T> condition)
        {
            ArrayBuffer<T> buffer = await union(condition);
            if (buffer.Count != 0) return await condition.Filter(buffer);
            buffer.Free();
            return condition.GetNullBuffer().Result;
        }
    }
}
