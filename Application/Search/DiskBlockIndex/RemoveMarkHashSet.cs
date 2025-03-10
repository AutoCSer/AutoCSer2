using AutoCSer.Algorithm;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希表
    /// </summary>
    internal unsafe sealed class RemoveMarkHashSet
    {
        /// <summary>
        /// 有效数据数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// 添加数据数量
        /// </summary>
        private int valueCount;
        /// <summary>
        /// 节点集合
        /// </summary>
        private Pointer nodes;
        /// <summary>
        /// 容器参数
        /// </summary>
        private UnmanagedRemoveMarkHashSetCapacity capacity;
        /// <summary>
        /// 容器大小
        /// </summary>
        internal int Capacity
        {
            get { return capacity.Capacity; }
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        internal RemoveMarkHashSet()
        {
            capacity = UnmanagedRemoveMarkHashSetCapacity.DefaultLink;
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="capacity">容器大小</param>
        internal RemoveMarkHashSet(int capacity)
        {
            this.capacity = UnmanagedRemoveMarkHashSetCapacity.DefaultLink.Get(capacity);
            nodes = this.capacity.UnmanagedPool.GetPointer();
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        internal RemoveMarkHashSet(uint[] values) : this(values.Length)
        {
            foreach (uint value in values) Add(value);
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        internal RemoveMarkHashSet(int[] values) : this(values.Length)
        {
            foreach (int value in values) Add(value);
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="hashSet"></param>
        internal RemoveMarkHashSet(ReusableHashCodeKeyHashSet hashSet) : this(hashSet.Count)
        {
            int count = hashSet.Count;
            if (count != 0)
            {
                foreach (ReusableHashNode value in hashSet.Nodes)
                {
                    Add(value.HashCode);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Clear()
        {
            Count = valueCount = 0;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="hashIndex"></param>
        private void remove(int nodeIndex, int hashIndex)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.nodes.Data;
            RemoveMarkHashNode node = nodes[Count];
            node.SetHashIndex(hashIndex);
            nodes[nodeIndex] = node;
            if (node.SourceHigh == 0) nodes[node.SourceIndex].SetHashIndex(nodeIndex);
            else nodes[node.SourceIndex].SetNext(nodeIndex);
            int nextIndex = node.Next;
            if (nextIndex != RemoveMarkHashSetCapacity.MaxCapacity) nodes[nextIndex].SetNextSource(nodeIndex);
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <returns></returns>
        private bool resize()
        {
            UnmanagedRemoveMarkHashSetCapacity lastCapacity = this.capacity;
            var capacity = lastCapacity.LinkNext;
            if (capacity != null)
            {
                Pointer nodes = this.nodes;
                this.nodes = capacity.UnmanagedPool.GetPointer();
                this.capacity = capacity;
                Count = valueCount = 0;
                try
                {
                    RemoveMarkHashNode* node = (RemoveMarkHashNode*)nodes.Data, nodeEnd = node + lastCapacity.Capacity;
                    do
                    {
                        add(*node);
                    }
                    while (++node != nodeEnd);
                }
                finally { lastCapacity.UnmanagedPool.Free(ref nodes); }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放节点集合
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            capacity.UnmanagedPool.Free(ref nodes);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Contains(int value)
        {
            return Contains((uint)value);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        internal bool Contains(uint value)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.nodes.Data;
            int hashIndex = capacity.GetHashIndex(value);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
                    RemoveMarkHashNode node = nodes[nodeIndex];
                    if (node.Source == (uint)hashIndex)
                    {
                        do
                        {
                            if (value == node.HashCode) return node.IsRemove == 0;
                            nodeIndex = node.Next;
                            if (nodeIndex != RemoveMarkHashSetCapacity.MaxCapacity) node = nodes[nodeIndex];
                            else return false;
                        }
                        while (true);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 新增重组数据
        /// </summary>
        /// <param name="node"></param>
        internal void add(RemoveMarkHashNode node)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.nodes.Data;
            int hashIndex = capacity.GetHashIndex(node.HashCode);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
                        int nextIndex = nodes[nodeIndex].Next;
                        if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (node.IsRemove == 0)
                            {
                                nodes[Count].SetNextNode(nodeIndex, node.HashCode);
                                ++valueCount;
                            }
                            else nodes[Count].SetRemoveNextNode(nodeIndex, node.HashCode);
                            nodes[nodeIndex].SetNext(Count++);
                            return;
                        }
                        nodeIndex = nextIndex;
                    }
                    while (true);
                }
                if (node.IsRemove == 0)
                {
                    nodes[Count].SetNode(hashIndex, node.HashCode);
                    ++valueCount;
                }
                else nodes[Count].SetRemoveNode(hashIndex, node.HashCode);
                nodes[hashIndex].SetHashIndex(Count++);
                return;
            }
            if (node.IsRemove == 0)
            {
                nodes[0].SetNode(hashIndex, node.HashCode);
                valueCount = 1;
            }
            else nodes[0].SetRemoveNode(hashIndex, node.HashCode);
            nodes[hashIndex].SetHashIndex();
            Count = 1;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Add(int value)
        {
            return Add((uint)value);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        internal bool Add(uint value)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.nodes.Data;
            int hashIndex = capacity.GetHashIndex(value);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode node = nodes[nodeIndex];
                        if (value == node.HashCode)
                        {
                            if (node.IsRemove != 0)
                            {
                                int nextIndex = node.Next;
                                if (node.SourceHigh == 0)
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                                    {
                                        if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                                    }
                                    else
                                    {
                                        nodes[nextIndex].SetSource(hashIndex);
                                        nodes[hashIndex].SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, nodes[nodeIndex].HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) nodes[lastIndex].SetNext();
                                    else
                                    {
                                        nodes[nextIndex].SetNextSource(lastIndex);
                                        nodes[lastIndex].SetNext(nextIndex);
                                    }
                                    if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                                }
                                return true;
                            }
                            return false;
                        }
                        lastIndex = nodeIndex;
                        nodeIndex = node.Next;
                        if (nodeIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (Count != capacity.Capacity)
                            {
                                nodes[Count].SetNextNode(lastIndex, value);
                                nodes[lastIndex].SetNext(Count++);
                                ++valueCount;
                                return true;
                            }
                            if (resize())
                            {
                                add(new RemoveMarkHashNode(value));
                                return true;
                            }
                            return false;
                        }
                    }
                    while (true);
                }
                if (Count != capacity.Capacity)
                {
                    nodes[Count].SetNode(hashIndex, value);
                    nodes[hashIndex].SetHashIndex(Count++);
                    ++valueCount;
                    return true;
                }
                if (resize())
                {
                    add(new RemoveMarkHashNode(value));
                    return true;
                }
                return false;
            }
            nodes[0].SetNode(hashIndex, value);
            nodes[hashIndex].SetHashIndex();
            valueCount = Count = 1;
            return true;
        }
        /// <summary>
        /// 新增删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool AddRemove(int value)
        {
            return AddRemove((uint)value);
        }
        /// <summary>
        /// 新增删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        internal bool AddRemove(uint value)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.nodes.Data;
            int hashIndex = capacity.GetHashIndex(value);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode node = nodes[nodeIndex];
                        if (value == node.HashCode)
                        {
                            if (node.IsRemove == 0)
                            {
                                int nextIndex = node.Next;
                                if (node.SourceHigh == 0)
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                                    {
                                        if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                                    }
                                    else
                                    {
                                        nodes[nextIndex].SetSource(hashIndex);
                                        nodes[hashIndex].SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, nodes[nodeIndex].HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) nodes[lastIndex].SetNext();
                                    else
                                    {
                                        nodes[nextIndex].SetNextSource(lastIndex);
                                        nodes[lastIndex].SetNext(nextIndex);
                                    }
                                    if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                                }
                                --valueCount;
                                return true;
                            }
                            return false;
                        }
                        lastIndex = nodeIndex;
                        nodeIndex = node.Next;
                        if (nodeIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (Count != capacity.Capacity)
                            {
                                nodes[Count].SetRemoveNextNode(lastIndex, value);
                                nodes[lastIndex].SetNext(Count++);
                                return true;
                            }
                            if (resize())
                            {
                                add(new RemoveMarkHashNode(value, 0x80000000U));
                                return true;
                            }
                            return false;
                        }
                    }
                    while (true);
                }
                if (Count != capacity.Capacity)
                {
                    nodes[Count].SetRemoveNode(hashIndex, value);
                    nodes[hashIndex].SetHashIndex(Count++);
                    return true;
                }
                if (resize())
                {
                    add(new RemoveMarkHashNode(value, 0x80000000U));
                    return true;
                }
                return false;
            }
            nodes[0].SetRemoveNode(hashIndex, value);
            nodes[hashIndex].SetHashIndex();
            Count = 1;
            return true;
        }
        /// <summary>
        /// 删除新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在删除数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Remove(int value)
        {
            return Remove((uint)value);
        }
        /// <summary>
        /// 删除新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在删除数据</returns>
        internal bool Remove(uint value)
        {
            if (Count != 0)
            {
                RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.nodes.Data;
                int hashIndex = capacity.GetHashIndex(value), nodeIndex = nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode node = nodes[nodeIndex];
                        if (value == node.HashCode)
                        {
                            if (node.IsRemove == 0)
                            {
                                int nextIndex = node.Next;
                                if (node.SourceHigh == 0)
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                                    {
                                        if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                                    }
                                    else
                                    {
                                        nodes[nextIndex].SetSource(hashIndex);
                                        nodes[hashIndex].SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, nodes[nodeIndex].HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) nodes[lastIndex].SetNext();
                                    else
                                    {
                                        nodes[nextIndex].SetNextSource(lastIndex);
                                        nodes[lastIndex].SetNext(nextIndex);
                                    }
                                    if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                                }
                                --valueCount;
                                return true;
                            }
                            return false;
                        }
                        lastIndex = nodeIndex;
                        nodeIndex = node.Next;
                    }
                    while (nodeIndex != RemoveMarkHashSetCapacity.MaxCapacity);
                }
            }
            return false;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="valueCount">添加数据数量</param>
        /// <returns></returns>
        internal uint[] GetArray(out int valueCount)
        {
            if (Count != 0)
            {
                uint[] array = AutoCSer.Common.GetUninitializedArray<uint>(Count);
                fixed (uint* arrayFixed = array) valueCount = getArray(arrayFixed);
                return array;
            }
            valueCount = 0;
            return EmptyArray<uint>.Array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="arrayFixed"></param>
        /// <returns>添加数据数量</returns>
        private int getArray(uint* arrayFixed)
        {
            int removeIndex = Count, index = 0;
            RemoveMarkHashNode* node = (RemoveMarkHashNode*)nodes.Data, end = node + removeIndex;
            do
            {
                if ((*node).IsRemove == 0) arrayFixed[index++] = (*node).HashCode;
                else arrayFixed[--removeIndex] = (*node).HashCode;
                if (index == removeIndex) return index;
                ++node;
            }
            while (true);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="valueCount">添加数据数量</param>
        /// <returns></returns>
        internal int[] GetIntArray(out int valueCount)
        {
            if (Count != 0)
            {
                int[] array = AutoCSer.Common.GetUninitializedArray<int>(Count);
                fixed (int* arrayFixed = array) valueCount = getArray((uint*)arrayFixed);
                return array;
            }
            valueCount = 0;
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Get(BufferHashSet<uint> hashSet)
        {
            if (Count != 0)
            {
                RemoveMarkHashNode* node = (RemoveMarkHashNode*)nodes.Data, end = node + Count;
                do
                {
                    hashSet.Add((*node).HashCode);
                }
                while (++node != end);
            }
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Get(BufferHashSet<int> hashSet)
        {
            if (Count != 0)
            {
                RemoveMarkHashNode* node = (RemoveMarkHashNode*)nodes.Data, end = node + Count;
                do
                {
                    hashSet.Add((int)(*node).HashCode);
                }
                while (++node != end);
            }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="buffer"></param>
        private void getBuffer(uint* buffer)
        {
            RemoveMarkHashNode* node = (RemoveMarkHashNode*)nodes.Data, end = node + Count;
            do
            {
                *buffer++ = (*node).HashCode;
            }
            while (++node != end);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        internal ArrayBuffer<uint> Get(QueryCondition<uint> condition)
        {
            if (Count != 0)
            {
                ArrayBuffer<uint> buffer = condition.GetBuffer(Count);
                fixed (uint* bufferFixed = buffer.Array) getBuffer(bufferFixed);
                buffer.SetCount(Count);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        internal ArrayBuffer<int> Get(QueryCondition<int> condition)
        {
            if (Count != 0)
            {
                ArrayBuffer<int> buffer = condition.GetBuffer(Count);
                fixed (int* bufferFixed = buffer.Array) getBuffer((uint*)bufferFixed);
                buffer.SetCount(Count);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
    /// <summary>
    /// 带移除标记的可重用哈希表
    /// </summary>
    /// <typeparam name="T">关键字数据类型</typeparam>
    internal sealed class RemoveMarkHashSet<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 有效数据数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// 添加数据数量
        /// </summary>
        private int valueCount;
        /// <summary>
        /// 节点集合
        /// </summary>
        private RemoveMarkHashNode<T>[] nodes;
        /// <summary>
        /// 容器大小
        /// </summary>
        internal int Capacity
        {
            get { return nodes.Length; }
        }
        /// <summary>
        /// 容器参数
        /// </summary>
        private RemoveMarkHashSetCapacity capacity;
        /// <summary>
        /// 空哈希表
        /// </summary>
        internal RemoveMarkHashSet()
        {
            nodes = EmptyArray<RemoveMarkHashNode<T>>.Array;
            capacity = RemoveMarkHashSetCapacity.DefaultLink;
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="capacity">容器大小</param>
        internal RemoveMarkHashSet(int capacity)
        {
            this.capacity = RemoveMarkHashSetCapacity.DefaultLink.Get(capacity);
            nodes = new RemoveMarkHashNode<T>[this.capacity.Capacity];
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        internal RemoveMarkHashSet(T[] values) : this(values.Length)
        {
            foreach (T value in values) Add(value);
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        internal RemoveMarkHashSet(ICollection<T> values) : this(values.Count)
        {
            foreach (T value in values) Add(value);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Clear()
        {
            Count = valueCount = 0;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="hashIndex"></param>
        private void remove(int nodeIndex, int hashIndex)
        {
            RemoveMarkHashNode<T> node = nodes[Count];
            node.HashIndex.SetHashIndex(hashIndex);
            nodes[nodeIndex] = node;
            if (node.HashIndex.SourceHigh == 0) nodes[node.HashIndex.SourceIndex].HashIndex.SetHashIndex(nodeIndex);
            else nodes[node.HashIndex.SourceIndex].HashIndex.SetNext(nodeIndex);
            int nextIndex = node.HashIndex.Next;
            if (nextIndex != RemoveMarkHashSetCapacity.MaxCapacity) nodes[nextIndex].HashIndex.SetNextSource(nodeIndex);
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <returns></returns>
        private bool resize()
        {
            var capacity = this.capacity.LinkNext;
            if (capacity != null)
            {
                RemoveMarkHashNode<T>[] nodes = this.nodes;
                this.nodes = new RemoveMarkHashNode<T>[capacity.Capacity];
                this.capacity = capacity;
                Count = valueCount = 0;
                foreach(RemoveMarkHashNode<T> node in nodes) add(node);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool Contains(T value)
        {
            if (Count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = capacity.GetHashIndex(hashCode), nodeIndex = nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count)
                {
                    RemoveMarkHashNode<T> node = nodes[nodeIndex];
                    if (node.HashIndex.Source == (uint)hashIndex)
                    {
                        do
                        {
                            if (hashCode == node.HashIndex.HashCode && node.Value.Equals(value)) return node.HashIndex.IsRemove == 0;
                            nodeIndex = node.HashIndex.Next;
                            if (nodeIndex != RemoveMarkHashSetCapacity.MaxCapacity) node = nodes[nodeIndex];
                            else return false;
                        }
                        while (true);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 新增重组数据
        /// </summary>
        /// <param name="node"></param>
        internal void add(RemoveMarkHashNode<T> node)
        {
            int hashIndex = capacity.GetHashIndex(node.HashIndex.HashCode);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    do
                    {
                        int nextIndex = nodes[nodeIndex].HashIndex.Next;
                        if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (node.HashIndex.IsRemove == 0)
                            {
                                nodes[Count].SetNextNode(node.Value, nodeIndex, node.HashIndex.HashCode);
                                ++valueCount;
                            }
                            else nodes[Count].SetRemoveNextNode(node.Value, nodeIndex, node.HashIndex.HashCode);
                            nodes[nodeIndex].HashIndex.SetNext(Count++);
                            return;
                        }
                        nodeIndex = nextIndex;
                    }
                    while (true);
                }
                if (node.HashIndex.IsRemove == 0)
                {
                    nodes[Count].SetNode(node.Value, hashIndex, node.HashIndex.HashCode);
                    ++valueCount;
                }
                else nodes[Count].SetRemoveNode(node.Value, hashIndex, node.HashIndex.HashCode);
                nodes[hashIndex].HashIndex.SetHashIndex(Count++);
                return;
            }
            if (node.HashIndex.IsRemove == 0)
            {
                nodes[0].SetNode(node.Value, hashIndex, node.HashIndex.HashCode);
                valueCount = 1;
            }
            else nodes[0].SetRemoveNode(node.Value, hashIndex, node.HashIndex.HashCode);
            nodes[hashIndex].HashIndex.SetHashIndex();
            Count = 1;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        internal bool Add(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int hashIndex = capacity.GetHashIndex(hashCode);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode<T> node = nodes[nodeIndex];
                        if (hashCode == node.HashIndex.HashCode && node.Value.Equals(value))
                        {
                            if (node.HashIndex.IsRemove != 0)
                            {
                                int nextIndex = node.HashIndex.Next;
                                if (node.HashIndex.SourceHigh == 0)
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                                    {
                                        if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex.HashIndex);
                                    }
                                    else
                                    {
                                        nodes[nextIndex].HashIndex.SetSource(hashIndex);
                                        nodes[hashIndex].HashIndex.SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, nodes[nodeIndex].HashIndex.HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) nodes[lastIndex].HashIndex.SetNext();
                                    else
                                    {
                                        nodes[nextIndex].HashIndex.SetNextSource(lastIndex);
                                        nodes[lastIndex].HashIndex.SetNext(nextIndex);
                                    }
                                    if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex.HashIndex);
                                }
                                return true;
                            }
                            return false;
                        }
                        lastIndex = nodeIndex;
                        nodeIndex = node.HashIndex.Next;
                        if (nodeIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (Count != nodes.Length)
                            {
                                nodes[Count].SetNextNode(value, lastIndex, hashCode);
                                nodes[lastIndex].HashIndex.SetNext(Count++);
                                ++valueCount;
                                return true;
                            }
                            if (resize())
                            {
                                add(new RemoveMarkHashNode<T>(value, hashCode));
                                return true;
                            }
                            return false;
                        }
                    }
                    while (true);
                }
                if (Count != nodes.Length)
                {
                    nodes[Count].SetNode(value, hashIndex, hashCode);
                    nodes[hashIndex].HashIndex.SetHashIndex(Count++);
                    ++valueCount;
                    return true;
                }
                if (resize())
                {
                    add(new RemoveMarkHashNode<T>(value, hashCode));
                    return true;
                }
                return false;
            }
            nodes[0].SetNode(value, hashIndex, hashCode);
            nodes[hashIndex].HashIndex.SetHashIndex();
            valueCount = Count = 1;
            return true;
        }
        /// <summary>
        /// 新增删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        internal bool AddRemove(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int hashIndex = capacity.GetHashIndex(hashCode);
            if (Count != 0)
            {
                int nodeIndex = nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode<T> node = nodes[nodeIndex];
                        if (hashCode == node.HashIndex.HashCode && node.Value.Equals(value))
                        {
                            if (node.HashIndex.IsRemove == 0)
                            {
                                int nextIndex = node.HashIndex.Next;
                                if (node.HashIndex.SourceHigh == 0)
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                                    {
                                        if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex.HashIndex);
                                    }
                                    else
                                    {
                                        nodes[nextIndex].HashIndex.SetSource(hashIndex);
                                        nodes[hashIndex].HashIndex.SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, nodes[nodeIndex].HashIndex.HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) nodes[lastIndex].HashIndex.SetNext();
                                    else
                                    {
                                        nodes[nextIndex].HashIndex.SetNextSource(lastIndex);
                                        nodes[lastIndex].HashIndex.SetNext(nextIndex);
                                    }
                                    if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex.HashIndex);
                                }
                                --valueCount;
                                return true;
                            }
                            return false;
                        }
                        lastIndex = nodeIndex;
                        nodeIndex = node.HashIndex.Next;
                        if (nodeIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (Count != nodes.Length)
                            {
                                nodes[Count].SetRemoveNextNode(value, lastIndex, hashCode);
                                nodes[lastIndex].HashIndex.SetNext(Count++);
                                return true;
                            }
                            if (resize())
                            {
                                add(new RemoveMarkHashNode<T>(value, hashCode, 0x80000000U));
                                return true;
                            }
                            return false;
                        }
                    }
                    while (true);
                }
                if (Count != nodes.Length)
                {
                    nodes[Count].SetRemoveNode(value, hashIndex, hashCode);
                    nodes[hashIndex].HashIndex.SetHashIndex(Count++);
                    return true;
                }
                if (resize())
                {
                    add(new RemoveMarkHashNode<T>(value, hashCode, 0x80000000U));
                    return true;
                }
                return false;
            }
            nodes[0].SetRemoveNode(value, hashIndex, hashCode);
            nodes[hashIndex].HashIndex.SetHashIndex();
            Count = 1;
            return true;
        }
        /// <summary>
        /// 删除新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在删除数据</returns>
        internal bool Remove(T value)
        {
            if (Count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = capacity.GetHashIndex(hashCode), nodeIndex = nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode<T> node = nodes[nodeIndex];
                        if (hashCode == node.HashIndex.HashCode && node.Value.Equals(value))
                        {
                            if (node.HashIndex.IsRemove == 0)
                            {
                                int nextIndex = node.HashIndex.Next;
                                if (node.HashIndex.SourceHigh == 0)
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                                    {
                                        if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex.HashIndex);
                                    }
                                    else
                                    {
                                        nodes[nextIndex].HashIndex.SetSource(hashIndex);
                                        nodes[hashIndex].HashIndex.SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, nodes[nodeIndex].HashIndex.HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) nodes[lastIndex].HashIndex.SetNext();
                                    else
                                    {
                                        nodes[nextIndex].HashIndex.SetNextSource(lastIndex);
                                        nodes[lastIndex].HashIndex.SetNext(nextIndex);
                                    }
                                    if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex.HashIndex);
                                }
                                --valueCount;
                                return true;
                            }
                            return false;
                        }
                        lastIndex = nodeIndex;
                        nodeIndex = node.HashIndex.Next;
                    }
                    while (nodeIndex != RemoveMarkHashSetCapacity.MaxCapacity);
                }
            }
            return false;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="valueCount">添加数据数量</param>
        /// <returns></returns>
        internal T[] GetArray(out int valueCount)
        {
            if (Count != 0)
            {
                int removeIndex = Count, index = 0;
                T[] array = new T[removeIndex];
                foreach (RemoveMarkHashNode<T> node in nodes)
                {
                    if (node.HashIndex.IsRemove == 0) array[index++] = node.Value;
                    else array[--removeIndex] = node.Value;
                    if (index == removeIndex)
                    {
                        valueCount = index;
                        return array;
                    }
                }
            }
            valueCount = 0;
            return EmptyArray<T>.Array;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Get(BufferHashSet<T> hashSet)
        {
            if (Count != 0)
            {
                int count = Count;
                foreach (RemoveMarkHashNode<T> node in nodes)
                {
                    hashSet.Add(node.Value);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        internal ArrayBuffer<T> Get(QueryCondition<T> condition)
        {
            int count = Count;
            if (count != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(count);
                foreach (RemoveMarkHashNode<T> node in nodes)
                {
                    buffer.UnsafeAdd(node.Value);
                    if (--count == 0) break;
                }
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
}
