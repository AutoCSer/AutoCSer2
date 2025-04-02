using AutoCSer.Algorithm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 可重用哈希表（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    [RemoteType]
    public sealed class ReusableHashCodeKeyHashSet : ReusableDictionary
    {
        /// <summary>
        /// 节点集合
        /// </summary>
        internal ReusableHashNode[] Nodes;
        /// <summary>
        /// 容器大小
        /// </summary>
        internal int Capacity
        {
            get { return Nodes.Length; }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<uint> Values
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode node in Nodes)
                    {
                        yield return node.HashCode;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<int> IntValues
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode node in Nodes)
                    {
                        yield return (int)node.HashCode;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 空哈希表
        /// </summary>
        /// <param name="nodes"></param>
        internal ReusableHashCodeKeyHashSet(ReusableHashNode[] nodes) : base()
        {
            Nodes = nodes;
        }
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public ReusableHashCodeKeyHashSet(int capacity = 0) : base(capacity, ReusableDictionaryGroupTypeEnum.HashIndex)
        {
            Nodes = new ReusableHashNode[(int)CapacityDivision.Divisor];
        }
        /// <summary>
        /// 可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        internal ReusableHashCodeKeyHashSet(uint[] values) : this(values.Length)
        {
            int count = 0;
            foreach (uint value in values) Nodes[count++].HashCode = value;
            resizeHashIndex(Nodes, count);
        }
        /// <summary>
        /// 可重用哈希表
        /// </summary>
        /// <param name="values">初始化数据</param>
        internal ReusableHashCodeKeyHashSet(int[] values) : this(values.Length)
        {
            int count = 0;
            foreach (int value in values) Nodes[count++].HashCode = (uint)value;
            resizeHashIndex(Nodes, count);
        }
        /// <summary>
        /// 可重用哈希表
        /// </summary>
        /// <param name="hashSet">初始化数据</param>
        public ReusableHashCodeKeyHashSet(ReusableHashCodeKeyHashSet hashSet) : this(hashSet.Count)
        {
            int count = hashSet.Count;
            if (count != 0) resizeHashIndex(hashSet.Nodes, count);
        }
        /// <summary>
        /// 可重用哈希表
        /// </summary>
        /// <param name="hashSet">初始化数据</param>
        public unsafe ReusableHashCodeKeyHashSet(RemoveMarkHashSet hashSet) : this(hashSet.Count)
        {
            int count = hashSet.Count;
            if (count != 0)
            {
                RemoveMarkHashNode* node = (RemoveMarkHashNode*)hashSet.Nodes.Data;
                for (int index = 0; index != count; ++index) Nodes[index].HashCode = node[index].HashCode;
                resizeHashIndex(Nodes, count);
            }
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void add(uint value)
        {
            int hashIndex = (int)CapacityDivision.GetMod(value), linkIndex = Nodes[hashIndex].HashIndex;
            if (linkIndex < Count && Nodes[linkIndex].Source == (uint)hashIndex)
            {
                Nodes[linkIndex].SetNextSource(Count);
                Nodes[Count].Set((uint)hashIndex, value, linkIndex);
            }
            else Nodes[Count].Set((uint)hashIndex, value);
            Nodes[hashIndex].HashIndex = Count++;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        private void resize()
        {
            int capacity = GetResizeCapacity((int)CapacityDivision.Divisor);
            ReusableHashNode[] nodes = this.Nodes;

            this.Nodes = new ReusableHashNode[capacity];
            CapacityDivision.Set(capacity);
            resizeHashIndex(nodes, nodes.Length);
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="count"></param>
        private unsafe void resizeHashIndex(ReusableHashNode[] nodes, int count)
        {
            int hashIndex;
            IntegerDivision capacityDivision = CapacityDivision;
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((count + 1) * sizeof(long));
            try
            {
                byte* start = buffer.Pointer.Byte, current = start, write = start;
                for (hashIndex = 0; hashIndex != count; ++hashIndex)
                {
                    *(uint*)(current + sizeof(int)) = capacityDivision.GetMod(*(uint*)current = nodes[hashIndex].HashCode);
                    current += sizeof(long);
                }
                QuickSort.SortLong(start, current - sizeof(long));
                *(int*)current = int.MaxValue;
                do
                {
                    if ((hashIndex = *(int*)(start + sizeof(int))) < count)
                    {
                        if (hashIndex != *(int*)(start + (sizeof(int) + sizeof(long))))
                        {
                            Nodes[hashIndex].SetHashIndex(hashIndex, *(uint*)start);
                            start += sizeof(long);
                        }
                        else
                        {
                            *(long*)write = *(long*)start;
                            *(long*)(write + sizeof(long)) = *(long*)(start + sizeof(long));
                            for (start += sizeof(long) * 2, write += sizeof(long) * 2; hashIndex == *(int*)(start + sizeof(int)); start += sizeof(long), write += sizeof(long)) *(long*)write = *(long*)start;
                        }
                    }
                    else
                    {
                        long size = current - start;
                        AutoCSer.Common.CopyTo(start, write, (int)size);
                        write += size;
                        break;
                    }
                }
                while (start != current);
                start = buffer.Pointer.Byte;
                for (int index = 0, lastHashIndex = int.MinValue, lastIndex = 0; start != write; start += sizeof(long), lastIndex = index++)
                {
                    hashIndex = *(int*)(start + sizeof(int));
                    while (Nodes[index].Next != 0) ++index;
                    if (lastHashIndex != hashIndex)
                    {
                        Nodes[hashIndex].HashIndex = index;
                        Nodes[index].Set((uint)hashIndex, *(uint*)start);
                        lastHashIndex = hashIndex;
                    }
                    else
                    {
                        Nodes[lastIndex].Next = index;
                        Nodes[index].SetNext((uint)lastIndex, *(uint*)start);
                    }
                }
            }
            finally { buffer.PushOnly(); }
            maxRemoveCount = 0;
            Count = count;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Add(int value)
        {
            return Add((uint)value);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        public bool Add(uint value)
        {
            int hashIndex = (int)CapacityDivision.GetMod(value);
            if (Count != 0)
            {
                int nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode node = ref Nodes[nodeIndex];
#else
                        ReusableHashNode node = Nodes[nodeIndex];
#endif
                        if (value == node.HashCode) return false;
                        if (node.Next == int.MaxValue)
                        {
                            if (Count != CapacityDivision.Divisor)
                            {
                                Nodes[Count].Set((uint)nodeIndex | 0x80000000U, value);
                                Nodes[nodeIndex].Next = Count++;
                            }
                            else
                            {
                                resize();
                                add(value);
                            }
                            return true;
                        }
                        nodeIndex = node.Next;
                    }
                    while (true);
                }
                if (Count != CapacityDivision.Divisor)
                {
                    Nodes[Count].Set((uint)hashIndex, value);
                    Nodes[hashIndex].HashIndex = Count++;
                }
                else
                {
                    resize();
                    add(value);
                }
            }
            else
            {
                Nodes[0].Set((uint)hashIndex, value);
                Nodes[hashIndex].HashIndex = 0;
                Count = 1;
            }
            return true;
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
            if (Count != 0)
            {
                int hashIndex = (int)CapacityDivision.GetMod(value), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode node = Nodes[nodeIndex];
#endif
                    if (node.Source == hashIndex)
                    {
                        do
                        {
                            if (node.HashCode == value) return true;
                            nodeIndex = node.Next;
                            if (nodeIndex < Count)
                            {
#if NetStandard21
                                node = ref Nodes[nodeIndex];
#else
                                node = Nodes[nodeIndex];
#endif
                            }
                            else return false;
                        }
                        while (true);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(int value)
        {
            return Remove((uint)value);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        public bool Remove(uint value)
        {
            if (Count != 0)
            {
                int hashIndex = (int)CapacityDivision.GetMod(value), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == value && node.Source == hashIndex)
                    {
                        if (node.Next == int.MaxValue)
                        {
                            if (nodeIndex != removeCount()) remove(nodeIndex, node.HashIndex);
                        }
                        else
                        {
                            Nodes[node.Next].Source = (uint)hashIndex;
                            Nodes[hashIndex].HashIndex = node.Next;
                            if (nodeIndex != removeCount()) remove(nodeIndex, Nodes[nodeIndex].HashIndex);
                        }
                        return true;
                    }
                    if (node.Source == hashIndex)
                    {
                        for (int nextNodeIndex = node.Next; nextNodeIndex < Count; nodeIndex = nextNodeIndex, nextNodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nextNodeIndex];
#else
                            node = Nodes[nextNodeIndex];
#endif
                            if (node.HashCode == value)
                            {
                                if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(nodeIndex);
                                Nodes[nodeIndex].Next = node.Next;
                                if (nextNodeIndex != removeCount()) remove(nextNodeIndex, node.HashIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="hashIndex"></param>
        private void remove(int nodeIndex, int hashIndex)
        {
            ReusableHashNode node = Nodes[Count];
            node.HashIndex = hashIndex;
            Nodes[nodeIndex] = node;
            if (node.SourceHigh == 0) Nodes[(int)node.Source].HashIndex = nodeIndex;
            else Nodes[node.SourceIndex].Next = nodeIndex;
            if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(nodeIndex);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        internal void ClearArray()
        {
            int count = NodeArrayClearCount;
            if (count != 0)
            {
                Array.Clear(Nodes, 0, count);
                clearRemoveCount();
            }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        internal unsafe uint[] GetArray()
        {
            if (Count != 0)
            {
                uint[] array = AutoCSer.Common.GetUninitializedArray<uint>(Count);
                fixed (uint* arrayFixed = array) getArray(arrayFixed);
                return array;
            }
            return EmptyArray<uint>.Array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="write"></param>
        private unsafe void getArray(uint* write)
        {
            foreach(ReusableHashNode node in Nodes) *write++ = node.HashCode;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        internal unsafe int[] GetIntArray()
        {
            if (Count != 0)
            {
                int[] array = AutoCSer.Common.GetUninitializedArray<int>(Count);
                fixed (int* arrayFixed = array) getArray((uint*)arrayFixed);
                return array;
            }
            return EmptyArray<int>.Array;
        }

        /// <summary>
        /// 空索引数据
        /// </summary>
        internal static readonly ReusableHashCodeKeyHashSet Empty = new ReusableHashCodeKeyHashSet(EmptyArray<ReusableHashNode>.Array);
    }
}
