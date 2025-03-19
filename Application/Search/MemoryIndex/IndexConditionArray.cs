using AutoCSer.CommandService.Search.IndexQuery;
using System;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 索引集合条件
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public sealed class IndexConditionArray<T> : IIndexCondition<T>
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
        /// 索引合并操作类型
        /// </summary>
        private readonly IndexMergeTypeEnum type;
        /// <summary>
        /// 索引集合条件
        /// </summary>
        /// <param name="conditions">索引条件集合</param>
        /// <param name="type">索引合并操作类型</param>
        internal IndexConditionArray(LeftArray<IIndexCondition<T>> conditions, IndexMergeTypeEnum type)
        {
            this.type = type != IndexMergeTypeEnum.IntersectionNotEmpty ? type : IndexMergeTypeEnum.Intersection;
            switch (this.type)
            {
                case IndexMergeTypeEnum.Intersection:
                    AutoCSer.Extensions.ArraySort.QuickSort(conditions.Array, conditions.Length, p => p.EstimatedCount);
                    EstimatedCount = conditions.Array[0].EstimatedCount;
                    break;
                case IndexMergeTypeEnum.Union:
                    IIndexCondition<T>[] conditionArray = conditions.Array;
                    long unionCount = 0;
                    int count = conditions.Length;
                    foreach (IIndexCondition<T> indexCondition in conditionArray)
                    {
                        unionCount += indexCondition.EstimatedCount;
                        if (--count == 0) break;
                    }
                    EstimatedCount = (int)Math.Min(unionCount, int.MaxValue);
                    break;
            }
            this.conditions = conditions;
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
        void IIndexCondition<T>.Get(QueryCondition<T> condition, BufferHashSet<T> hashSet)
        {
            switch (conditions.Length)
            {
                case 0: return;
                case 1: conditions.Array[0].Get(condition, hashSet); return;
                default:
                    if (type != IndexMergeTypeEnum.Union)
                    {
                        ArrayBuffer<T> buffer = intersection(condition, conditions.Array[0].Get(condition));
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
                            indexCondition.Get(condition, hashSet);
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
        public ArrayBuffer<T> Get(QueryCondition<T> condition)
        {
            if (type != IndexMergeTypeEnum.Union) return intersection(condition, conditions.Array[0].Get(condition));
            BufferHashSet<T> hashSet = union(condition);
            ArrayBuffer<T> buffer = hashSet.GetArrayBuffer(condition);
            hashSet.Free();
            return buffer;
        }
        /// <summary>
        /// 交集 AND
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private ArrayBuffer<T> intersection(QueryCondition<T> condition, ArrayBuffer<T> buffer)
        {
            int valueCount = buffer.GetCount();
            if (valueCount != 0)
            {
                int nodeCount = conditions.Length;
                IIndexCondition<T>[] nodeArray = conditions.Array;
                foreach (T value in buffer.Array)
                {
                    for (int nodeIndex = 1; nodeIndex != nodeCount; ++nodeIndex)
                    {
                        if (!nodeArray[nodeIndex].Contains(value)) goto NEXTVALUE;
                    }
                    buffer.UnsafeAdd(value);
                NEXTVALUE:
                    if (--valueCount == 0) break;
                }
                if (buffer.Count != 0) return buffer;
            }
            buffer.Free();
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private BufferHashSet<T> union(QueryCondition<T> condition)
        {
            BufferHashSet<T> hashSet = HashSetPool<T>.GetHashSet(condition.GetHashSetPool(), EstimatedCount);
            int count = conditions.Length;
            foreach (IIndexCondition<T> indexNode in conditions.Array)
            {
                indexNode.Get(condition, hashSet);
                if (--count == 0) break;
            }
            return hashSet;
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> GetFilter(QueryCondition<T> condition)
        {
            if (type != IndexMergeTypeEnum.Union) return intersection(condition, conditions.Array[0].GetFilter(condition));
            BufferHashSet<T> hashSet = union(condition);
            ArrayBuffer<T> buffer = condition.Filter(hashSet.Values, condition.GetBuffer(hashSet.Count));
            hashSet.Free();
            return buffer;
        }

        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="conditions">索引条件集合</param>
        /// <param name="type">索引合并操作类型</param>
        /// <returns></returns>
#if NetStandard21
        public static IIndexCondition<T>? GetIndexCondition(LeftArray<IIndexCondition<T>> conditions, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#else
        public static IIndexCondition<T> GetIndexCondition(LeftArray<IIndexCondition<T>> conditions, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#endif
        {
            switch (conditions.Length)
            {
                case 0: return null;
                case 1: return conditions.Array[0];
                default:
                    IIndexCondition<T>[] conditionArray = conditions.Array;
                    for (int conditionIndex = 0; conditionIndex != conditions.Length; ++conditionIndex)
                    {
                        IIndexCondition<T> indexCondition = conditionArray[conditionIndex];
                        if (indexCondition.EstimatedCount == 0)
                        {
                            if (type == IndexMergeTypeEnum.Intersection) return null;
                            conditions.RemoveToEndOnly(conditionIndex--);
                        }
                    }
                    switch (conditions.Length)
                    {
                        case 0: return null;
                        case 1: return conditions.Array[0];
                        default: return new IndexConditionArray<T>(conditions, type);
                    }
            }
        }
    }
}
