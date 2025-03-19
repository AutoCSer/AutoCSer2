using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 带移除标记的可重用哈希表（最大支持 1023 个元素）
    /// </summary>
    public unsafe class RemoveMarkHashSet : IDisposable
    {
        /// <summary>
        /// 有效数据数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 添加数据数量
        /// </summary>
        private int valueCount;
        /// <summary>
        /// 节点集合
        /// </summary>
        internal Pointer Nodes;
        /// <summary>
        /// 获取数据集合
        /// </summary>
        internal IEnumerable<uint> OnlyValues
        {
            get
            {
                Pointer node = Nodes;
                for (int count = Count; count != 0; --count) yield return getHashCode(ref node);
            }
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        internal IEnumerable<int> OnlyIntValues
        {
            get
            {
                Pointer node = Nodes;
                for (int count = Count; count != 0; --count) yield return (int)getHashCode(ref node);
            }
        }
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
        /// 空哈希表
        /// </summary>
        /// <param name="capacity"></param>
        internal RemoveMarkHashSet(UnmanagedRemoveMarkHashSetCapacity capacity)
        {
            this.capacity = capacity;
        }
        /// <summary>
        /// 带移除标记的可重用哈希表（最大支持 1023 个元素）
        /// </summary>
        /// <param name="capacity">容器大小</param>
        public RemoveMarkHashSet(int capacity = 0)
        {
            this.capacity = UnmanagedRemoveMarkHashSetCapacity.DefaultLink.Get(capacity);
            Nodes = this.capacity.UnmanagedPool.GetPointer();
            Nodes.Clear();
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        public RemoveMarkHashSet(uint[] values) : this(values.Length)
        {
            foreach (uint value in values) add(new RemoveMarkHashNode(value));
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        public RemoveMarkHashSet(int[] values) : this(values.Length)
        {
            foreach (int value in values) add(new RemoveMarkHashNode((uint)value));
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="hashSet"></param>
        public RemoveMarkHashSet(ReusableHashCodeKeyHashSet hashSet) : this(hashSet.Count)
        {
            int count = hashSet.Count;
            if (count != 0)
            {
                foreach (ReusableHashNode value in hashSet.Nodes)
                {
                    add(new RemoveMarkHashNode(value.HashCode));
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 释放节点集合
        /// </summary>
        public void Dispose()
        {
            capacity.UnmanagedPool.Free(ref Nodes);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
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
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.Nodes.Data;
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
                Pointer nodes = this.Nodes;
                this.Nodes = capacity.UnmanagedPool.GetPointer();
                this.capacity = capacity;
                Count = valueCount = 0;
                try
                {
                    this.Nodes.Clear();
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
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(int value)
        {
            return Contains((uint)value);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        public bool Contains(uint value)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.Nodes.Data;
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
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.Nodes.Data;
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
        public bool Add(int value)
        {
            return Add((uint)value);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        public bool Add(uint value)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.Nodes.Data;
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
        public bool AddRemove(int value)
        {
            return AddRemove((uint)value);
        }
        /// <summary>
        /// 新增删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        public bool AddRemove(uint value)
        {
            RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.Nodes.Data;
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
        public bool Remove(int value)
        {
            return Remove((uint)value);
        }
        /// <summary>
        /// 删除新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在删除数据</returns>
        public bool Remove(uint value)
        {
            if (Count != 0)
            {
                RemoveMarkHashNode* nodes = (RemoveMarkHashNode*)this.Nodes.Data;
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
        public uint[] GetArray(out int valueCount)
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
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint[] GetArray()
        {
            int valueCount;
            return GetArray(out valueCount);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="arrayFixed"></param>
        /// <returns>添加数据数量</returns>
        private int getArray(uint* arrayFixed)
        {
            int removeIndex = Count, index = 0;
            RemoveMarkHashNode* node = (RemoveMarkHashNode*)Nodes.Data, end = node + removeIndex;
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
        public int[] GetIntArray(out int valueCount)
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
        internal void Get(ReusableHashSet<uint> hashSet)
        {
            if (Count != 0)
            {
                RemoveMarkHashNode* node = (RemoveMarkHashNode*)Nodes.Data, end = node + Count;
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
        internal void Get(ReusableHashSet<int> hashSet)
        {
            if (Count != 0)
            {
                RemoveMarkHashNode* node = (RemoveMarkHashNode*)Nodes.Data, end = node + Count;
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
        internal void GetBuffer(uint* buffer)
        {
            RemoveMarkHashNode* node = (RemoveMarkHashNode*)Nodes.Data, end = node + Count;
            do
            {
                *buffer++ = (*node).HashCode;
            }
            while (++node != end);
        }
        /// <summary>
        /// 获取关键字数据哈希值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static uint getHashCode(ref Pointer node)
        {
            return (*(RemoveMarkHashNode*)node.GetBeforeMove(sizeof(RemoveMarkHashNode))).HashCode;
        }

        /// <summary>
        /// 空索引数据
        /// </summary>
        internal static readonly RemoveMarkHashSet Empty = new RemoveMarkHashSet(UnmanagedRemoveMarkHashSetCapacity.DefaultLink);
    }
    /// <summary>
    /// 带移除标记的可重用哈希表（最大支持 1023 个元素）
    /// </summary>
    /// <typeparam name="T">关键字数据类型</typeparam>
    public class RemoveMarkHashSet<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 有效数据数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 添加数据数量
        /// </summary>
        private int valueCount;
        /// <summary>
        /// 节点集合
        /// </summary>
        internal RemoveMarkHashNode<T>[] Nodes;
        /// <summary>
        /// 获取数据集合（false 表示标记删除的数据）
        /// </summary>
        public IEnumerable<KeyValue<T, bool>> Values
        {
            get
            {
                int count = Count;
                if (count != 0)
                {
                    foreach (RemoveMarkHashNode<T> node in Nodes)
                    {
                        yield return new KeyValue<T, bool>(node.Value, node.HashIndex.IsRemove == 0);
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取数据集合（false 表示标记删除的数据）
        /// </summary>
        internal IEnumerable<T> OnlyValues
        {
            get
            {
                int count = Count;
                if (count != 0)
                {
                    foreach (RemoveMarkHashNode<T> node in Nodes)
                    {
                        yield return node.Value;
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 容器大小
        /// </summary>
        internal int Capacity
        {
            get { return Nodes.Length; }
        }
        /// <summary>
        /// 容器参数
        /// </summary>
        private RemoveMarkHashSetCapacity capacity;
        /// <summary>
        /// 空哈希表
        /// </summary>
        /// <param name="capacity"></param>
        internal RemoveMarkHashSet(RemoveMarkHashSetCapacity capacity)
        {
            Nodes = EmptyArray<RemoveMarkHashNode<T>>.Array;
            this.capacity = capacity;
        }
        /// <summary>
        /// 带移除标记的可重用哈希表（最大支持 1023 个元素）
        /// </summary>
        /// <param name="capacity">容器大小</param>
        public RemoveMarkHashSet(int capacity = 0)
        {
            this.capacity = RemoveMarkHashSetCapacity.DefaultLink.Get(capacity);
            Nodes = new RemoveMarkHashNode<T>[this.capacity.Capacity];
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        public RemoveMarkHashSet(T[] values) : this(values.Length)
        {
            foreach (T value in values) Add(value);
        }
        /// <summary>
        /// 带移除标记的可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        public RemoveMarkHashSet(ICollection<T> values) : this(values.Count)
        {
            foreach (T value in values) Add(value);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
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
            RemoveMarkHashNode<T> node = Nodes[Count];
            node.HashIndex.SetHashIndex(hashIndex);
            Nodes[nodeIndex] = node;
            if (node.HashIndex.SourceHigh == 0) Nodes[node.HashIndex.SourceIndex].HashIndex.SetHashIndex(nodeIndex);
            else Nodes[node.HashIndex.SourceIndex].HashIndex.SetNext(nodeIndex);
            int nextIndex = node.HashIndex.Next;
            if (nextIndex != RemoveMarkHashSetCapacity.MaxCapacity) Nodes[nextIndex].HashIndex.SetNextSource(nodeIndex);
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
                RemoveMarkHashNode<T>[] nodes = this.Nodes;
                this.Nodes = new RemoveMarkHashNode<T>[capacity.Capacity];
                this.capacity = capacity;
                Count = valueCount = 0;
                foreach (RemoveMarkHashNode<T> node in nodes) add(node);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            if (Count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = capacity.GetHashIndex(hashCode), nodeIndex = Nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count)
                {
                    RemoveMarkHashNode<T> node = Nodes[nodeIndex];
                    if (node.HashIndex.Source == (uint)hashIndex)
                    {
                        do
                        {
                            if (hashCode == node.HashIndex.HashCode && node.Value.Equals(value)) return node.HashIndex.IsRemove == 0;
                            nodeIndex = node.HashIndex.Next;
                            if (nodeIndex != RemoveMarkHashSetCapacity.MaxCapacity) node = Nodes[nodeIndex];
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
                int nodeIndex = Nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    do
                    {
                        int nextIndex = Nodes[nodeIndex].HashIndex.Next;
                        if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity)
                        {
                            if (node.HashIndex.IsRemove == 0)
                            {
                                Nodes[Count].SetNextNode(node.Value, nodeIndex, node.HashIndex.HashCode);
                                ++valueCount;
                            }
                            else Nodes[Count].SetRemoveNextNode(node.Value, nodeIndex, node.HashIndex.HashCode);
                            Nodes[nodeIndex].HashIndex.SetNext(Count++);
                            return;
                        }
                        nodeIndex = nextIndex;
                    }
                    while (true);
                }
                if (node.HashIndex.IsRemove == 0)
                {
                    Nodes[Count].SetNode(node.Value, hashIndex, node.HashIndex.HashCode);
                    ++valueCount;
                }
                else Nodes[Count].SetRemoveNode(node.Value, hashIndex, node.HashIndex.HashCode);
                Nodes[hashIndex].HashIndex.SetHashIndex(Count++);
                return;
            }
            if (node.HashIndex.IsRemove == 0)
            {
                Nodes[0].SetNode(node.Value, hashIndex, node.HashIndex.HashCode);
                valueCount = 1;
            }
            else Nodes[0].SetRemoveNode(node.Value, hashIndex, node.HashIndex.HashCode);
            Nodes[hashIndex].HashIndex.SetHashIndex();
            Count = 1;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        public bool Add(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int hashIndex = capacity.GetHashIndex(hashCode);
            if (Count != 0)
            {
                int nodeIndex = Nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode<T> node = Nodes[nodeIndex];
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
                                        Nodes[nextIndex].HashIndex.SetSource(hashIndex);
                                        Nodes[hashIndex].HashIndex.SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, Nodes[nodeIndex].HashIndex.HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) Nodes[lastIndex].HashIndex.SetNext();
                                    else
                                    {
                                        Nodes[nextIndex].HashIndex.SetNextSource(lastIndex);
                                        Nodes[lastIndex].HashIndex.SetNext(nextIndex);
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
                            if (Count != Nodes.Length)
                            {
                                Nodes[Count].SetNextNode(value, lastIndex, hashCode);
                                Nodes[lastIndex].HashIndex.SetNext(Count++);
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
                if (Count != Nodes.Length)
                {
                    Nodes[Count].SetNode(value, hashIndex, hashCode);
                    Nodes[hashIndex].HashIndex.SetHashIndex(Count++);
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
            Nodes[0].SetNode(value, hashIndex, hashCode);
            Nodes[hashIndex].HashIndex.SetHashIndex();
            valueCount = Count = 1;
            return true;
        }
        /// <summary>
        /// 新增删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据变更</returns>
        public bool AddRemove(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int hashIndex = capacity.GetHashIndex(hashCode);
            if (Count != 0)
            {
                int nodeIndex = Nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode<T> node = Nodes[nodeIndex];
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
                                        Nodes[nextIndex].HashIndex.SetSource(hashIndex);
                                        Nodes[hashIndex].HashIndex.SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, Nodes[nodeIndex].HashIndex.HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) Nodes[lastIndex].HashIndex.SetNext();
                                    else
                                    {
                                        Nodes[nextIndex].HashIndex.SetNextSource(lastIndex);
                                        Nodes[lastIndex].HashIndex.SetNext(nextIndex);
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
                            if (Count != Nodes.Length)
                            {
                                Nodes[Count].SetRemoveNextNode(value, lastIndex, hashCode);
                                Nodes[lastIndex].HashIndex.SetNext(Count++);
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
                if (Count != Nodes.Length)
                {
                    Nodes[Count].SetRemoveNode(value, hashIndex, hashCode);
                    Nodes[hashIndex].HashIndex.SetHashIndex(Count++);
                    return true;
                }
                if (resize())
                {
                    add(new RemoveMarkHashNode<T>(value, hashCode, 0x80000000U));
                    return true;
                }
                return false;
            }
            Nodes[0].SetRemoveNode(value, hashIndex, hashCode);
            Nodes[hashIndex].HashIndex.SetHashIndex();
            Count = 1;
            return true;
        }
        /// <summary>
        /// 删除新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在删除数据</returns>
        public bool Remove(T value)
        {
            if (Count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = capacity.GetHashIndex(hashCode), nodeIndex = Nodes[hashIndex].HashIndex.HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].HashIndex.Source == (uint)hashIndex)
                {
                    int lastIndex = 0;
                    do
                    {
                        RemoveMarkHashNode<T> node = Nodes[nodeIndex];
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
                                        Nodes[nextIndex].HashIndex.SetSource(hashIndex);
                                        Nodes[hashIndex].HashIndex.SetHashIndex(nextIndex);
                                        if (nodeIndex != --Count) remove(nodeIndex, Nodes[nodeIndex].HashIndex.HashIndex);
                                    }
                                }
                                else
                                {
                                    if (nextIndex == RemoveMarkHashSetCapacity.MaxCapacity) Nodes[lastIndex].HashIndex.SetNext();
                                    else
                                    {
                                        Nodes[nextIndex].HashIndex.SetNextSource(lastIndex);
                                        Nodes[lastIndex].HashIndex.SetNext(nextIndex);
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
        public T[] GetArray(out int valueCount)
        {
            if (Count != 0)
            {
                int removeIndex = Count, index = 0;
                T[] array = new T[removeIndex];
                foreach (RemoveMarkHashNode<T> node in Nodes)
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
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] GetArray()
        {
            int valueCount;
            return GetArray(out valueCount);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Get(ReusableHashSet<T> hashSet)
        {
            if (Count != 0)
            {
                int count = Count;
                foreach (RemoveMarkHashNode<T> node in Nodes)
                {
                    hashSet.Add(node.Value);
                    if (--count == 0) break;
                }
            }
        }
    }
}
