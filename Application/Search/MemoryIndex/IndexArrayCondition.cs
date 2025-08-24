using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using Microsoft.VisualBasic;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.MemoryIndex
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
        /// 索引集合
        /// </summary>
        private LeftArray<IIndex<T>> indexs;
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
        /// <param name="indexs">索引节点数据集合</param>
        /// <param name="type">索引合并操作类型</param>
        internal unsafe IndexArrayCondition(LeftArray<IIndex<T>> indexs, IndexMergeTypeEnum type)
        {
            this.type = type != IndexMergeTypeEnum.IntersectionNotEmpty ? type : IndexMergeTypeEnum.Intersection;
            IIndex<T>[] array = indexs.Array;
            int count = indexs.Length;
            switch (this.type)
            {
                case IndexMergeTypeEnum.Intersection:
                    int index = 0;
                    AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.IntSortIndex));
                    try
                    {
                        AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                        (*nextSortIndex).Set(array[index].Count, index);
                        while (++index != count) (*++nextSortIndex).Set(array[index].Count, index);
                        AutoCSer.Algorithm.IntSortIndex.Sort(sortIndex, nextSortIndex);
                        AutoCSer.Algorithm.IntSortIndex.Sort(array, count, sortIndex);
                    }
                    finally { buffer.PushOnly(); }
                    EstimatedCount = indexs.Array[0].Count;
                    break;
                case IndexMergeTypeEnum.Union:
                    int maxCount = array[0].Count;
                    long unionCount = maxCount;
                    while (count > 1)
                    {
                        IIndex<T> indexNode = array[--count];
                        unionCount += indexNode.Count;
                        if (indexNode.Count > maxCount)
                        {
                            array[count] = array[0];
                            maxCount = indexNode.Count;
                            array[0] = indexNode;
                        }
                    }
                    EstimatedCount = (int)Math.Min(unionCount, int.MaxValue);
                    break;
            }
            this.indexs = indexs;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IIndexCondition<T>.Contains(T value)
        {
            int count = indexs.Length;
            if (count != 0)
            {
                if (type != IndexMergeTypeEnum.Union)
                {
                    foreach (IIndex<T> node in indexs.Array)
                    {
                        if (!node.Contains(value)) return false;
                        if (--count == 0) return true;
                    }
                }
                foreach (IIndex<T> node in indexs.Array)
                {
                    if (node.Contains(value)) return true;
                    if (--count == 0) break;
                }
            }
            return false;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="hashSet"></param>
        void IIndexCondition<T>.Get(QueryCondition<T> condition, BufferHashSet<T> hashSet)
        {
            switch (indexs.Length)
            {
                case 0: return;
                case 1: indexs.Array[0].Get(hashSet); return;
                default:
                    if (type != IndexMergeTypeEnum.Union)
                    {
                        ArrayBuffer<T> buffer = intersection(condition, indexs.Array[0].Get(condition));
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
                        int count = indexs.Length;
                        foreach (IIndex<T> indexNode in indexs.Array)
                        {
                            indexNode.Get(hashSet);
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
            if (type != IndexMergeTypeEnum.Union) return intersection(condition, indexs.Array[0].Get(condition));
            if (isUnion())
            {
                BufferHashSet<T> hashSet = union(condition);
                ArrayBuffer<T> buffer = hashSet.GetArrayBuffer(condition);
                hashSet.Free();
                return buffer;
            }
            return indexs.Array[0].Get(condition);
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
                int nodeCount = indexs.Length;
                IIndex<T>[] nodeArray = indexs.Array;
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
        /// 判断是否需要求并集 OR
        /// </summary>
        /// <returns></returns>
        private bool isUnion()
        {
            IIndex<T>[] array = indexs.Array;
            IIndex<T> index = array[0];
            int count = indexs.Length;
            if (!array[--count].AllIn(index)) return true;
            while (count > 1)
            {
                if (!array[--count].AllIn(index))
                {
                    ++count;
                    break;
                }
            }
            indexs.Length = count;
            if (count == 1) return false;
            long unionCount = 0;
            foreach (IIndex<T> indexNode in array)
            {
                unionCount += indexNode.Count;
                if (--count != 0) break;
            }
            EstimatedCount = (int)Math.Min(unionCount, int.MaxValue);
            return true;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private BufferHashSet<T> union(QueryCondition<T> condition)
        {
            BufferHashSet<T> hashSet = HashSetPool<T>.GetHashSet(condition.GetHashSetPool(), EstimatedCount);
            int count = indexs.Length;
            foreach (IIndex<T> indexNode in indexs.Array)
            {
                indexNode.Get(hashSet);
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
            if (type != IndexMergeTypeEnum.Union) return intersection(condition, indexs.Array[0].GetFilter(condition));
            if (isUnion())
            {
                BufferHashSet<T> hashSet = union(condition);
                ArrayBuffer<T> buffer = condition.Filter(hashSet.Values, condition.GetBuffer(hashSet.Count));
                hashSet.Free();
                return buffer;
            }
            return indexs.Array[0].GetFilter(condition);
        }
    }
}
