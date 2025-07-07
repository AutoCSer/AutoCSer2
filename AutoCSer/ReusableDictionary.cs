using AutoCSer.Algorithm;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 可重用字典（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    public unsafe class ReusableDictionary
    {
        /// <summary>
        /// 有效数据数量
        /// </summary>
        public int Count { get; protected set; }
        /// <summary>
        /// 滚动索引位置（用于优先级淘汰策略）
        /// </summary>
        protected int rollIndex;
        /// <summary>
        /// 哈希取余
        /// </summary>
        internal IntegerDivision CapacityDivision;
        /// <summary>
        /// 最大删除数据位置
        /// </summary>
        protected int maxRemoveCount;
        /// <summary>
        /// 可重用字典重组操作类型
        /// </summary>
        protected readonly ReusableDictionaryGroupTypeEnum groupType;
        /// <summary>
        /// 需要清理的数组位置
        /// </summary>
        internal int NodeArrayClearCount { get { return Math.Max(Count, maxRemoveCount); } }
        /// <summary>
        /// 空字典
        /// </summary>
        protected ReusableDictionary() { }
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        protected ReusableDictionary(int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            this.groupType = groupType;
            CapacityDivision.Set(GetCapacity(capacity));
        }
        /// <summary>
        /// 清除计数位置信息
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void clear()
        {
            rollIndex = Count = 0;
        }
        /// <summary>
        /// 删除数据移动结束位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected int removeCount()
        {
            if (Count > maxRemoveCount) maxRemoveCount = Count;
            return --Count;
        }
        /// <summary>
        /// 清除计数位置信息
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearCount()
        {
            if (Count > maxRemoveCount) maxRemoveCount = Count;
            clear();
        }
        /// <summary>
        /// 清除计数位置信息
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void clearRemoveCount()
        {
            clear();
            maxRemoveCount = 0;
        }

        /// <summary>
        /// 获取重组数据数组大小
        /// </summary>
        /// <param name="capacity">当前数组大小</param>
        /// <returns>重组数据数组大小</returns>
        internal static int GetResizeCapacity(int capacity)
        {
            ulong nextCapacity = (((ulong)capacity << 31) / 0x409e0b86UL) + 1;//第 30 个值大于 MaxPrime
            if (nextCapacity < MaxPrime) return GetCapacity((int)nextCapacity);
            if (capacity < MaxPrime) return MaxPrime;
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// Get the container size
        /// 获取容器大小
        /// </summary>
        /// <param name="capacity">指定容器大小</param>
        /// <returns></returns>
        public static int GetCapacity(int capacity)
        {
            if (capacity <= 3) return 3;
            if (capacity <= MaxPrime)
            {
                capacity |= 1;
                do
                {
                    if (IsPrime(capacity)) return capacity;
                    capacity += 2;
                }
                while (true);
            }
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// 最大质数
        /// </summary>
        internal const int MaxPrime = 0x7fffffc3;
        /// <summary>
        /// 最大小质数
        /// </summary>
        private const int maxPrice = 46349;
        /// <summary>
        /// 小质数集合起始位置
        /// </summary>
        private static AutoCSer.Memory.Pointer primes;
        /// <summary>
        /// 判断是否质数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPrime(int value)
        {
            int maxPrime = (int)Math.Sqrt(value), prime = 0;
            byte* nextPrime = primes.Byte;
            do
            {
                if ((prime += *nextPrime) > maxPrime) return true;
                if ((value % prime) == 0) return false;
                ++nextPrime;
            }
            while (true);
        }
        static ReusableDictionary()
        {
            primes = Unmanaged.GetReusableDictionaryPrimes();
            byte* endPrime = primes.Byte;
            *endPrime++ = 3;
            int value = 5, lastPrime = 3;
            do
            {
                int maxValue = (int)Math.Sqrt(value), prime = 0;
                byte* nextPrime = primes.Byte;
                do
                {
                    if ((prime += *nextPrime) > maxValue)
                    {
                        *endPrime++ = (byte)(value - lastPrime);
                        lastPrime = value;
                        break;
                    }
                    if ((value % prime) == 0) break;
                    ++nextPrime;
                }
                while (true);
                value += 2;
            }
            while (value <= maxPrice);
        }
    }
    /// <summary>
    /// 可重用字典（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    public abstract class ReusableDictionary<T> : ReusableDictionary
    {
        /// <summary>
        /// 节点集合
        /// </summary>
        internal ReusableHashNode<T>[] Nodes;
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        protected ReusableDictionary(int capacity, ReusableDictionaryGroupTypeEnum groupType) : base(capacity, groupType)
        {
            Nodes = capacity >= 0 ? new ReusableHashNode<T>[CapacityDivision.Divisor] : EmptyArray<ReusableHashNode<T>>.Array;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="hashCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected void add(uint hashCode, T value)
        {
            int hashIndex = (int)CapacityDivision.GetMod(hashCode), linkIndex = Nodes[hashIndex].HashIndex;
            if (linkIndex < Count && Nodes[linkIndex].Source == (uint)hashIndex)
            {
                Nodes[linkIndex].SetNextSource(Count);
                Nodes[Count].Set((uint)hashIndex, hashCode, value, linkIndex);
            }
            else Nodes[Count].Set((uint)hashIndex, hashCode, value);
            Nodes[hashIndex].HashIndex = Count++;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        protected virtual void resize()
        {
            ReusableHashNode<T>[] nodes = Nodes;
            int capacity = GetResizeCapacity((int)CapacityDivision.Divisor), rollIndex = this.rollIndex;

            Nodes = new ReusableHashNode<T>[capacity];
            CapacityDivision.Set(capacity);
            resize(nodes, rollIndex);
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="rollIndex"></param>
        protected void resize(ReusableHashNode<T>[] nodes, int rollIndex)
        {
            switch (groupType)
            {
                case ReusableDictionaryGroupTypeEnum.HashIndexSort: resizeHashIndexSort(nodes); return;
                case ReusableDictionaryGroupTypeEnum.Roll: resizeRoll(nodes, rollIndex); return;
                default: resizeHashIndex(nodes); return;
            }
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        private unsafe void resizeHashIndex(ReusableHashNode<T>[] nodes)
        {
            IntegerDivision capacityDivision = CapacityDivision;
            int count = nodes.Length, hashIndex;
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((count + 1) * sizeof(long));
            try
            {
                byte* start = buffer.Pointer.Byte, current = start, write = start;
                for (hashIndex = 0; hashIndex != count; ++hashIndex)
                {
                    *(int*)current = hashIndex;
                    *(uint*)(current + sizeof(int)) = capacityDivision.GetMod(nodes[hashIndex].HashCode);
                    current += sizeof(long);
                }
                QuickSort.SortLong(start, current - sizeof(long));
                *(int*)(current + sizeof(int)) = int.MaxValue;
                do
                {
                    if ((hashIndex = *(int*)(start + sizeof(int))) < count)
                    {
                        if (hashIndex != *(int*)(start + (sizeof(int) + sizeof(long))))
                        {
                            Nodes[hashIndex].SetHashIndex(hashIndex, ref nodes[*(int*)start]);
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
                        Nodes[index].Set((uint)hashIndex, ref nodes[*(int*)start]);
                        lastHashIndex = hashIndex;
                    }
                    else
                    {
                        Nodes[lastIndex].Next = index;
                        Nodes[index].SetNext((uint)lastIndex, ref nodes[*(int*)start]);
                    }
                }
            }
            finally { buffer.PushOnly(); }
            maxRemoveCount = 0;
            Count = count;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        private unsafe void resizeHashIndexSort(ReusableHashNode<T>[] nodes)
        {
            IntegerDivision capacityDivision = CapacityDivision;
            int count = nodes.Length, hashIndex;
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(long));
            try
            {
                byte* start = buffer.Pointer.Byte, current = start;
                for (hashIndex = 0; hashIndex != count; ++hashIndex)
                {
                    *(int*)current = hashIndex;
                    *(uint*)(current + sizeof(int)) = capacityDivision.GetMod(nodes[hashIndex].HashCode);
                    current += sizeof(long);
                }
                QuickSort.SortLong(start, current - sizeof(long));
                int index = 0, lastHashIndex = int.MinValue;
                do
                {
                    hashIndex = *(int*)(start + sizeof(int));
                    if (lastHashIndex != hashIndex)
                    {
                        Nodes[hashIndex].HashIndex = index;
                        Nodes[index].Set((uint)hashIndex, ref nodes[*(int*)start]);
                        lastHashIndex = hashIndex;
                    }
                    else
                    {
                        Nodes[hashIndex = index - 1].Next = index;
                        Nodes[index].SetNext((uint)hashIndex, ref nodes[*(int*)start]);
                    }
                    start += sizeof(long);
                }
                while (++index != count);
            }
            finally { buffer.PushOnly(); }
            maxRemoveCount = 0;
            Count = count;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="rollIndex"></param>
        private void resizeRoll(ReusableHashNode<T>[] nodes, int rollIndex)
        {
            clearRemoveCount();
            for (int index = rollIndex; index != nodes.Length; ++index)
            {
#if NetStandard21
                ref ReusableHashNode<T> node = ref nodes[index];
#else
                ReusableHashNode<T> node = nodes[index];
#endif
                add(node.HashCode, node.Value);
            }
            for (int index = 0; index != rollIndex; ++index)
            {
#if NetStandard21
                ref ReusableHashNode<T> node = ref nodes[index];
#else
                ReusableHashNode<T> node = nodes[index];
#endif
                add(node.HashCode, node.Value);
            }
        }
        /// <summary>
        /// Clear the data
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
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="hashIndex"></param>
        protected void remove(int nodeIndex, int hashIndex)
        {
            ReusableHashNode<T> node = Nodes[Count];
            node.HashIndex = hashIndex;
            Nodes[nodeIndex] = node;
            if (node.SourceHigh == 0) Nodes[(int)node.Source].HashIndex = nodeIndex;
            else Nodes[node.SourceIndex].Next = nodeIndex;
            if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(nodeIndex);
        }
        /// <summary>
        /// 尝试修改访问节点索引位置
        /// </summary>
        /// <param name="nodeIndex"></param>
        protected void changeIndex(int nodeIndex)
        {
            int orderIndex = nodeIndex - rollIndex, rollEndIndex = (Count >> 1) - 1;
            if (orderIndex < 0) orderIndex += Count;
            if (orderIndex <= rollEndIndex + ((Count - rollEndIndex) >> 1))
            {
                int changeIndex = nodeIndex + ((Count - orderIndex) >> 1);
                if (nodeIndex != changeIndex) change(nodeIndex, changeIndex < Count ? changeIndex : changeIndex - Count);
            }
        }
        /// <summary>
        /// 交换节点位置
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="changeIndex"></param>
        protected void change(int nodeIndex, int changeIndex)
        {
            ReusableHashNode<T> node = Nodes[nodeIndex], changeNode = Nodes[changeIndex];
            if (node.HashCode != changeNode.HashCode)
            {
                if (CapacityDivision.GetMod(node.HashCode) != CapacityDivision.GetMod(changeNode.HashCode))
                {
                    if (node.SourceHigh == 0) Nodes[(int)node.Source].HashIndex = changeIndex;
                    else Nodes[node.SourceIndex].Next = changeIndex;
                    if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(changeIndex);

                    if (changeNode.SourceHigh == 0) Nodes[(int)changeNode.Source].HashIndex = nodeIndex;
                    else Nodes[changeNode.SourceIndex].Next = nodeIndex;
                    if (changeNode.Next != int.MaxValue) Nodes[changeNode.Next].SetNextSource(nodeIndex);

                    node.HashIndex = Nodes[changeIndex].HashIndex;
                    changeNode.HashIndex = Nodes[nodeIndex].HashIndex;
                    Nodes[changeIndex] = node;
                    Nodes[nodeIndex] = changeNode;
                }
                else
                {
                    Nodes[changeIndex].Set(node.HashCode, node.Value);
                    Nodes[nodeIndex].Set(changeNode.HashCode, changeNode.Value);
                }
            }
            else
            {
                Nodes[changeIndex].Value = node.Value;
                Nodes[nodeIndex].Value = changeNode.Value;
            }
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="values"></param>
        internal void CopyTo(ref LeftArray<T> values)
        {
            if (Count != 0)
            {
                values.PrepLength(Count);
                T[] array = values.Array;
                int arrayIndex = values.Length, endIndex = arrayIndex + Count;
                foreach (ReusableHashNode<T> node in Nodes)
                {
                    array[arrayIndex] = node.Value;
                    if (++arrayIndex == endIndex) break;
                }
                values.Length = endIndex;
            }
        }
    }
    /// <summary>
    /// 可重用字典（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">Keyword type
    /// 数据类型</typeparam>
    [RemoteType]
    public sealed class ReusableDictionary<KT, VT> : ReusableDictionary<KeyValue<KT, VT>>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<KeyValue<KT, VT>> KeyValues
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<KeyValue<KT, VT>> node in Nodes)
                    {
                        yield return node.Value;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<KT> Keys
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<KeyValue<KT, VT>> node in Nodes)
                    {
                        yield return node.Value.Key;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// The data collection
        /// 数据集合
        /// </summary>
        public IEnumerable<VT> Values
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<KeyValue<KT, VT>> node in Nodes)
                    {
                        yield return node.Value.Value;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取或者设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns></returns>
        public VT this[KT key]
        {
            get
            {
                var value = default(VT);
                if (TryGetValue(key, out value)) return value;
                throw new IndexOutOfRangeException();
            }
            set { Set(key, value); }
        }
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        public ReusableDictionary(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType) { }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">Target data</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否获取成功</returns>
#if NetStandard21
        public bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value, bool isRoll = false)
#else
        public bool TryGetValue(KT key, out VT value, bool isRoll = false)
#endif
        {
            if (Count != 0)
            {
                uint hashCode = (uint)key.GetHashCode();
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<KeyValue<KT, VT>> node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode<KeyValue<KT, VT>> node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Key.Equals(key))
                    {
                        value = node.Value.Value;
                        if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return true;
                        changeIndex(nodeIndex);
                        return true;
                    }
                    if (node.Source == hashIndex)
                    {
                        for (nodeIndex = node.Next; nodeIndex < Count; nodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nodeIndex];
#else
                            node = Nodes[nodeIndex];
#endif
                            if (node.HashCode == hashCode && node.Value.Key.Equals(key))
                            {
                                value = node.Value.Value;
                                if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return true;
                                changeIndex(nodeIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Set(KT key, VT value, bool isRoll = false)
        {
            return set(key, value, isRoll, false);
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryAdd(KT key, VT value, bool isRoll = false)
        {
            return set(key, value, isRoll, true);
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <param name="isAdd"></param>
        /// <returns>是否新增数据</returns>
        private bool set(KT key, VT value, bool isRoll, bool isAdd)
        {
            uint hashCode = (uint)key.GetHashCode();
            int hashIndex = (int)CapacityDivision.GetMod(hashCode);
            if (Count != 0)
            {
                int nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode<KeyValue<KT, VT>> node = ref Nodes[nodeIndex];
#else
                        ReusableHashNode<KeyValue<KT, VT>> node = Nodes[nodeIndex];
#endif
                        if (hashCode == node.HashCode && node.Value.Key.Equals(key))
                        {
                            if (!isAdd) Nodes[nodeIndex].Value.Value = value;
                            if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return false;
                            changeIndex(nodeIndex);
                            return false;
                        }
                        if (node.Next == int.MaxValue)
                        {
                            if (Count != CapacityDivision.Divisor)
                            {
                                Nodes[Count].Set((uint)nodeIndex | 0x80000000U, hashCode, new KeyValue<KT, VT>(key, value));
                                Nodes[nodeIndex].Next = Count++;
                            }
                            else
                            {
                                resize();
                                add(hashCode, new KeyValue<KT, VT>(key, value));
                            }
                            return true;
                        }
                        nodeIndex = node.Next;
                    }
                    while (true);
                }
                if (Count != CapacityDivision.Divisor)
                {
                    Nodes[Count].Set((uint)hashIndex, hashCode, new KeyValue<KT, VT>(key, value));
                    Nodes[hashIndex].HashIndex = Count++;
                }
                else
                {
                    resize();
                    add(hashCode, new KeyValue<KT, VT>(key, value));
                }
            }
            else
            {
                Nodes[0].Set((uint)hashIndex, hashCode, new KeyValue<KT, VT>(key, value));
                Nodes[hashIndex].HashIndex = 0;
                Count = 1;
            }
            return true;
        }

        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(KT key, bool isRoll = false)
        {
            var value = default(VT);
            return TryGetValue(key, out value, isRoll);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(KT key)
        {
            var value = default(VT);
            return Remove(key, out value);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">Deleted data
        /// 被删除数据</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
#if NetStandard21
        public bool Remove(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool Remove(KT key, out VT value)
#endif
        {
            if (Count != 0)
            {
                uint hashCode = (uint)key.GetHashCode();
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<KeyValue<KT, VT>> node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode<KeyValue<KT, VT>> node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Key.Equals(key))
                    {
                        value = node.Value.Value;
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
                            if (node.HashCode == hashCode && node.Value.Key.Equals(key))
                            {
                                value = node.Value.Value;
                                if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(nodeIndex);
                                Nodes[nodeIndex].Next = node.Next;
                                if (nextNodeIndex != removeCount()) remove(nextNodeIndex, node.HashIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveRoll()
        {
            KeyValue<KT, VT> value;
            RemoveRoll(out value);
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">Deleted data
        /// 被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
        public bool RemoveRoll(out KeyValue<KT, VT> value)
        {
            if (Count != 0 && groupType == ReusableDictionaryGroupTypeEnum.Roll)
            {
                if (rollIndex >= Count) rollIndex = 0;
#if NetStandard21
                ref ReusableHashNode<KeyValue<KT, VT>> node = ref Nodes[rollIndex];
#else
                ReusableHashNode<KeyValue<KT, VT>> node = Nodes[rollIndex];
#endif
                value = node.Value;
                int hashIndex = (int)CapacityDivision.GetMod(node.HashCode);
                if (node.SourceHigh == 0)
                {
                    if (node.Next != int.MaxValue)
                    {
                        Nodes[node.Next].Source = (uint)hashIndex;
                        Nodes[hashIndex].HashIndex = node.Next;
                        if (rollIndex != removeCount())
                        {
                            remove(rollIndex, Nodes[rollIndex].HashIndex);
                            //if (++rollIndex == Count) rollIndex = 0;
                            ++rollIndex;
                            rollIndex &= (rollIndex ^ Count).logicalInversion() - 1;
                        }
                        else rollIndex = 0;
                        return true;
                    }
                }
                else
                {
                    Nodes[node.SourceIndex].Next = node.Next;
                    if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(node.SourceIndex);
                }
                if (rollIndex != removeCount())
                {
                    remove(rollIndex, node.HashIndex);
                    //if (++rollIndex == Count) rollIndex = 0;
                    ++rollIndex;
                    rollIndex &= (rollIndex ^ Count).logicalInversion() - 1;
                }
                else rollIndex = 0;
                return true;
            }
            value = default(KeyValue<KT, VT>);
            return false;
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">Deleted data
        /// 被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
#if NetStandard21
        public bool RemoveRoll([MaybeNullWhen(false)] out VT value)
#else
        public bool RemoveRoll(out VT value)
#endif
        {
            KeyValue<KT, VT> keyValue;
            if (RemoveRoll(out keyValue))
            {
                value = keyValue.Value;
                return true;
            }
            value = default(VT);
            return false;
        }
    }
}
