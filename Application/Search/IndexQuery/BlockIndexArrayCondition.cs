using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 索引集合条件
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public sealed class BlockIndexArrayCondition<T> : IIndexCondition<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引节点数据集合
        /// </summary>
        private LeftArray<IndexNode<T>> nodes;
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
        bool IIndexCondition<T>.IsLoaded { get { return loadedCount == nodes.Length; } }
        /// <summary>
        /// 索引合并操作类型
        /// </summary>
        private readonly IndexMergeTypeEnum type;
        /// <summary>
        /// 索引集合条件
        /// </summary>
        /// <param name="nodes">索引节点数据集合</param>
        /// <param name="type">索引合并操作类型</param>
        public BlockIndexArrayCondition(LeftArray<IndexNode<T>> nodes, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
        {
            if (nodes.Length == 0) throw new ArgumentNullException();
            this.type = type;
            long unionCount = 0;
            IndexNode<T>[] nodeArray = nodes.Array;
            for (int nodeIndex = 0, estimatedCount; nodeIndex != nodes.Length; ++nodeIndex)
            {
                IndexNode<T> indexNode = nodeArray[nodeIndex];
                estimatedCount = indexNode.EstimatedCount;
                if (estimatedCount != 0)
                {
                    unionCount += estimatedCount;
                    if (indexNode.IsLoaded) ++loadedCount;
                }
                else
                {
                    if (type == IndexMergeTypeEnum.Intersection)
                    {
                        loadedCount = 0;
                        return;
                    }
                    nodes.RemoveToEndOnly(nodeIndex--);
                }
            }
            if (nodes.Length != 0)
            {
                this.nodes = nodes;
                setEstimatedCount(unionCount);
            }
        }
        /// <summary>
        /// 索引集合条件
        /// </summary>
        /// <param name="nodes">索引节点数据集合</param>
        /// <param name="type">索引合并操作类型</param>
        public BlockIndexArrayCondition(IndexNode<T>[] nodes, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection) : this(new LeftArray<IndexNode<T>>(nodes), type) { }
        /// <summary>
        /// 设置预估数据数量
        /// </summary>
        /// <param name="unionCount"></param>
        private unsafe void setEstimatedCount(long unionCount)
        {
            switch (type)
            {
                case IndexMergeTypeEnum.Intersection:
                case IndexMergeTypeEnum.IntersectionNotEmpty:
                    int count = nodes.Length;
                    if (count > 1)
                    {
                        int index = 0;
                        IndexNode<T>[] array = nodes.Array;
                        AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.IntSortIndex));
                        try
                        {
                            AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                            (*nextSortIndex).Set(array[index].EstimatedCount, index);
                            while (++index != count) (*++nextSortIndex).Set(array[index].EstimatedCount, index);
                            AutoCSer.Algorithm.IntSortIndex.SortDesc(sortIndex, nextSortIndex);
                            AutoCSer.Extensions.ArraySort.QuickSort(array, count, sortIndex);
                        }
                        finally { buffer.PushOnly(); }
                    }
                    EstimatedCount = nodes.Array[0].EstimatedCount;
                    break;
                case IndexMergeTypeEnum.Union: EstimatedCount = (int)Math.Min(unionCount, int.MaxValue); break;
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        async Task<ResponseResult> IIndexCondition<T>.Load()
        {
            long unionCount = 0;
            IndexNode<T>[] nodeArray = nodes.Array;
            ResponseResult result = CallStateEnum.Success;
            int isLoaded = 0;
            for (int nodeIndex = 0, estimatedCount; nodeIndex != nodes.Length; ++nodeIndex)
            {
                IndexNode<T> indexNode = nodeArray[nodeIndex];
                if (!indexNode.IsLoaded)
                {
                    isLoaded = 1;
                    ResponseResult<IIndex<T>> loadResult = await indexNode.Node.Load();
                    if (loadResult.IsSuccess)
                    {
                        nodeArray[nodeIndex].Index = indexNode.Index = loadResult.Value.notNull();
                        estimatedCount = indexNode.Index.Count;
                        if (estimatedCount == 0 && type == IndexMergeTypeEnum.Intersection)
                        {
                            nodes.Length = 0;
                            break;
                        }
                    }
                    else
                    {
                        result = loadResult;
                        estimatedCount = 0;
                    }
                    if (estimatedCount == 0) nodes.RemoveToEndOnly(nodeIndex--);
                    else unionCount += estimatedCount;
                }
            }
            if (isLoaded != 0)
            {
                if ((loadedCount = nodes.Length) == 0)
                {
                    EstimatedCount = 0;
                    return result;
                }
                setEstimatedCount(unionCount);
            }
            return CallStateEnum.Success;
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<ArrayBuffer<T>> IIndexCondition<T>.Get(QueryCondition<T> condition)
        {
            switch (nodes.Length)
            {
                case 0: return condition.GetNullBuffer();
                case 1:
                    IndexNode<T> node = nodes.Array[0];
                    if (node.IsLoaded) return Task.FromResult(node.Index.Get(condition));
                    return get(condition, node);
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
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static async Task<ArrayBuffer<T>> get(QueryCondition<T> condition, IndexNode<T> node)
        {
            ResponseResult<IIndex<T>> result = await node.Node.Load();
            if (result.IsSuccess) return result.Value.notNull().Get(condition);
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 交集 AND
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersection(QueryCondition<T> condition)
        {
            IndexNode<T> node = nodes.Array[0];
            if (node.IsLoaded) return await intersection(condition, node.Index.Get(condition));
            return await intersection(condition, await get(condition, node));
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
                int nodeCount = nodes.Length;
                IndexNode<T>[] nodeArray = nodes.Array;
                foreach (T value in buffer.Array)
                {
                    for (int nodeIndex = 1; nodeIndex != nodeCount; ++nodeIndex)
                    {
                        IndexNode<T> indexNode = nodeArray[nodeIndex];
                        if (indexNode.IsLoaded)
                        {
                            if (!indexNode.Index.Contains(value)) goto NEXTVALUE;
                        }
                        else
                        {
                            ResponseResult<IIndex<T>> result = await indexNode.Node.Load();
                            if (result.IsSuccess)
                            {
                                nodeArray[nodeIndex].Index = indexNode.Index = result.Value.notNull();
                                if (indexNode.Index.Count == 0)
                                {
                                    buffer.Free();
                                    return condition.GetNullBuffer().Result;
                                }
                                if (!indexNode.Index.Contains(value)) goto NEXTVALUE;
                            }
                            else
                            {
                                nodes.RemoveAtOnly(nodeIndex--);
                                nodeCount = nodes.Length;
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
            IndexNode<T>[] nodeArray = nodes.Array;
            int index = 0, nodeCount = nodes.Length;
            do
            {
                IndexNode<T> node = nodeArray[index];
                ArrayBuffer<T> buffer = node.IsLoaded ? node.Index.Get(condition) : await get(condition, node);
                if (buffer.Count != 0)
                {
                    if (++index == nodeCount) return buffer;
                    return await intersectionNotEmpty(condition, buffer, index);
                }
                buffer.Free();
            }
            while (++index != nodeCount);
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
            IndexNode<T>[] nodeArray = nodes.Array;
            int valueCount = buffer.GetCount(), nodeCount = nodes.Length;
            foreach (T value in buffer.Array)
            {
                for (int nodeIndex = index, estimatedCount; nodeIndex != nodeCount; ++nodeIndex)
                {
                    IndexNode<T> indexNode = nodeArray[nodeIndex];
                    if (indexNode.IsLoaded)
                    {
                        if (!indexNode.Index.Contains(value)) goto NEXTVALUE;
                    }
                    else
                    {
                        ResponseResult<IIndex<T>> result = await indexNode.Node.Load();
                        if (result.IsSuccess)
                        {
                            nodeArray[nodeIndex].Index = indexNode.Index = result.Value.notNull();
                            estimatedCount = indexNode.Index.Count;
                        }
                        else estimatedCount = 0;
                        if (estimatedCount != 0)
                        {
                            if (!indexNode.Index.Contains(value)) goto NEXTVALUE;
                        }
                        else
                        {
                            nodes.RemoveAtOnly(nodeIndex--);
                            nodeCount = nodes.Length;
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
            IndexNode<T>[] nodeArray = nodes.Array;
            int capacity = 0;
            for (int nodeIndex = 0, estimatedCount; nodeIndex != nodes.Length; ++nodeIndex)
            {
                IndexNode<T> indexNode = nodeArray[nodeIndex];
                if (indexNode.IsLoaded) estimatedCount = indexNode.Index.Count;
                else
                {
                    ResponseResult<IIndex<T>> result = await indexNode.Node.Load();
                    if (result.IsSuccess)
                    {
                        nodeArray[nodeIndex].Index = indexNode.Index = result.Value.notNull();
                        estimatedCount = indexNode.Index.Count;
                    }
                    else estimatedCount = 0;
                }
                if (estimatedCount != 0) capacity = Math.Max(capacity, estimatedCount);
                else nodes.RemoveToEndOnly(nodeIndex--);
            }
            if (capacity != 0) return unionLoaded(condition, capacity);
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<ArrayBuffer<T>> IIndexCondition<T>.GetFilter(QueryCondition<T> condition)
        {
            switch (nodes.Length)
            {
                case 0: return condition.GetNullBuffer();
                case 1:
                    IndexNode<T> node = nodes.Array[0];
                    if (node.IsLoaded) return condition.Filter(node.Index.Get(condition));
                    return getFilter(condition, node);
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
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static async Task<ArrayBuffer<T>> getFilter(QueryCondition<T> condition, IndexNode<T> node)
        {
            ResponseResult<IIndex<T>> result = await node.Node.Load();
            if (result.IsSuccess) return await condition.Filter(result.Value.notNull().Get(condition));
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 交集 AND
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersectionFilter(QueryCondition<T> condition)
        {
            IndexNode<T> node = nodes.Array[0];
            if (node.IsLoaded) return await intersection(condition, await condition.Filter(node.Index.Get(condition)));
            return await intersection(condition, await getFilter(condition, node));
        }
        /// <summary>
        /// 交集 AND（忽略空集，必须存在一个非空集）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> intersectionNotEmptyFilter(QueryCondition<T> condition)
        {
            IndexNode<T>[] nodeArray = nodes.Array;
            int index = 0, nodeCount = nodes.Length;
            do
            {
                IndexNode<T> node = nodes.Array[index];
                ArrayBuffer<T> buffer = node.IsLoaded ? await condition.Filter(node.Index.Get(condition)) : await getFilter(condition, node);
                if (buffer.Count != 0)
                {
                    if (++index == nodeCount) return buffer;
                    return await intersectionNotEmpty(condition, buffer, index);
                }
                buffer.Free();
            }
            while (++index != nodeCount);
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
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IIndexCondition<T>.Contains(T value)
        {
            int count = nodes.Length;
            if (count != 0)
            {
                if (type != IndexMergeTypeEnum.Union)
                {
                    foreach (IndexNode<T> node in nodes.Array)
                    {
                        if (!node.Index.Contains(value)) return false;
                        if (--count == 0) return true;
                    }
                }
                foreach (IndexNode<T> node in nodes.Array)
                {
                    if (node.Index.Contains(value)) return true;
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
        void IIndexCondition<T>.GetLoaded(QueryCondition<T> condition, BufferHashSet<T> hashSet)
        {
            switch (nodes.Length)
            {
                case 0: return;
                case 1: nodes.Array[0].Index.Get(hashSet); return;
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
                        int count = nodes.Length;
                        foreach (IndexNode<T> indexNode in nodes.Array)
                        {
                            indexNode.Index.Get(hashSet);
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
        public ArrayBuffer<T> GetLoaded(QueryCondition<T> condition)
        {
            switch (nodes.Length)
            {
                case 0: return condition.GetNullBuffer().Result;
                case 1: return nodes.Array[0].Index.Get(condition);
                default:
                    if (type != IndexMergeTypeEnum.Union)
                    {
                        IndexNode<T>[] nodeArray = nodes.Array;
                        ArrayBuffer<T> buffer = nodeArray[0].Index.Get(condition);
                        int valueCount = buffer.GetCount();
                        if (valueCount != 0)
                        {
                            int nodeCount = nodes.Length, nodeIndex;
                            foreach (T value in buffer.Array)
                            {
                                nodeIndex = 1;
                                do
                                {
                                    if (!nodeArray[nodeIndex].Index.Contains(value)) goto NEXTVALUE;
                                }
                                while (++nodeIndex != nodeCount);
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
                        int count = nodes.Length, capacity = 0;
                        foreach (IndexNode<T> indexNode in nodes.Array)
                        {
                            if (indexNode.EstimatedCount > capacity) capacity = indexNode.EstimatedCount;
                            if (--count == 0) break;
                        }
                        if (capacity != 0) return unionLoaded(condition, capacity);
                        return condition.GetNullBuffer().Result;
                    }
            }
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        private ArrayBuffer<T> unionLoaded(QueryCondition<T> condition, int capacity)
        {
            HashSetPool<T>[] pools = condition.GetHashSetPool();
            BufferHashSet<T> hashSet = HashSetPool<T>.GetHashSet(pools, capacity);
            int count = nodes.Length;
            foreach (IndexNode<T> indexNode in nodes.Array)
            {
                indexNode.Index.Get(hashSet);
                if (--count == 0) break;
            }
            if ((count = hashSet.Count) != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(count);
                foreach (ReusableHashNode<T> node in hashSet.Nodes)
                {
                    buffer.UnsafeAdd(node.Value);
                    if (--count == 0)
                    {
                        hashSet.Free();
                        return buffer;
                    }
                }
            }
            hashSet.Free();
            return condition.GetNullBuffer().Result;
        }
    }
}
