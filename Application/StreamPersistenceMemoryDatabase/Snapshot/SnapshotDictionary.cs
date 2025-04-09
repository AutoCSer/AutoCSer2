using AutoCSer.Algorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照字典
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public abstract class SnapshotDictionary<T> : ReusableDictionary
    {
        /// <summary>
        /// 快照字典
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        protected SnapshotDictionary(int capacity, ReusableDictionaryGroupTypeEnum groupType) : base(capacity, groupType) { }
        /// <summary>
        /// 交换节点位置
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="changeIndex"></param>
        /// <param name="nodeArray"></param>
        protected void change(int nodeIndex, int changeIndex, ReusableHashNode<T>[] nodeArray)
        {
            ReusableHashNode<T> node = nodeArray[nodeIndex], changeNode = nodeArray[changeIndex];
            if (node.HashCode != changeNode.HashCode)
            {
                if (CapacityDivision.GetMod(node.HashCode) != CapacityDivision.GetMod(changeNode.HashCode))
                {
                    if (node.SourceHigh == 0) nodeArray[(int)node.Source].HashIndex = changeIndex;
                    else nodeArray[node.SourceIndex].Next = changeIndex;
                    if (node.Next != int.MaxValue) nodeArray[node.Next].SetNextSource(changeIndex);

                    if (changeNode.SourceHigh == 0) nodeArray[(int)changeNode.Source].HashIndex = nodeIndex;
                    else nodeArray[changeNode.SourceIndex].Next = nodeIndex;
                    if (changeNode.Next != int.MaxValue) nodeArray[changeNode.Next].SetNextSource(nodeIndex);

                    node.HashIndex = nodeArray[changeIndex].HashIndex;
                    changeNode.HashIndex = nodeArray[nodeIndex].HashIndex;
                    nodeArray[changeIndex] = node;
                    nodeArray[nodeIndex] = changeNode;
                }
                else
                {
                    nodeArray[changeIndex].Set(node.HashCode, node.Value);
                    nodeArray[nodeIndex].Set(changeNode.HashCode, changeNode.Value);
                }
            }
            else
            {
                nodeArray[changeIndex].Value = node.Value;
                nodeArray[nodeIndex].Value = changeNode.Value;
            }
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeArray"></param>
        protected unsafe void resizeHashIndex(ReusableHashNode<T>[] nodes, ReusableHashNode<T>[] nodeArray)
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
                            nodeArray[hashIndex].SetHashIndex(hashIndex, ref nodes[*(int*)start]);
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
                    while (nodeArray[index].Next != 0) ++index;
                    if (lastHashIndex != hashIndex)
                    {
                        nodeArray[hashIndex].HashIndex = index;
                        nodeArray[index].Set((uint)hashIndex, ref nodes[*(int*)start]);
                        lastHashIndex = hashIndex;
                    }
                    else
                    {
                        nodeArray[lastIndex].Next = index;
                        nodeArray[index].SetNext((uint)lastIndex, ref nodes[*(int*)start]);
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
        /// <param name="nodeArray"></param>
        protected unsafe void resizeHashIndexSort(ReusableHashNode<T>[] nodes, ReusableHashNode<T>[] nodeArray)
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
                        nodeArray[hashIndex].HashIndex = index;
                        nodeArray[index].Set((uint)hashIndex, ref nodes[*(int*)start]);
                        lastHashIndex = hashIndex;
                    }
                    else
                    {
                        nodeArray[hashIndex = index - 1].Next = index;
                        nodeArray[index].SetNext((uint)hashIndex, ref nodes[*(int*)start]);
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
        /// <param name="nodeArray"></param>
        protected void resizeRoll(ReusableHashNode<T>[] nodes, int rollIndex, ReusableHashNode<T>[] nodeArray)
        {
            clearRemoveCount();
            for (int index = rollIndex; index != nodes.Length; ++index)
            {
#if NetStandard21
                ref ReusableHashNode<T> node = ref nodes[index];
#else
                ReusableHashNode<T> node = nodes[index];
#endif
                add(node.HashCode, node.Value, nodeArray);
            }
            for (int index = 0; index != rollIndex; ++index)
            {
#if NetStandard21
                ref ReusableHashNode<T> node = ref nodes[index];
#else
                ReusableHashNode<T> node = nodes[index];
#endif
                add(node.HashCode, node.Value, nodeArray);
            }
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="hashCode"></param>
        /// <param name="value"></param>
        /// <param name="nodeArray"></param>
        /// <returns></returns>
        protected void add(uint hashCode, T value, ReusableHashNode<T>[] nodeArray)
        {
            int hashIndex = (int)CapacityDivision.GetMod(hashCode), linkIndex = nodeArray[hashIndex].HashIndex;
            if (linkIndex < Count && nodeArray[linkIndex].Source == (uint)hashIndex)
            {
                nodeArray[linkIndex].SetNextSource(Count);
                nodeArray[Count].Set((uint)hashIndex, hashCode, value, linkIndex);
            }
            else nodeArray[Count].Set((uint)hashIndex, hashCode, value);
            nodeArray[hashIndex].HashIndex = Count++;
        }
    }
    /// <summary>
    /// 快照字典
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型，必须是只读类型（不允许存在成员变更操作）</typeparam>
    public sealed class SnapshotDictionary<KT, VT> : SnapshotDictionary<BinarySerializeKeyValue<KT, VT>>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 快照字典节点数组
        /// </summary>
        internal SnapshotDictionaryNodeArray<KT, VT> Nodes;
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<KeyValue<KT, VT>> KeyValueSnapshot { get { return Nodes; } }
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>> BinarySerializeKeyValueSnapshot { get { return Nodes; } }
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<VT> ValueSnapshot { get { return Nodes; } }
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<BinarySerializeKeyValue<KT, VT>> KeyValues
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node in Nodes.Nodes)
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
                    foreach (ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node in Nodes.Nodes)
                    {
                        yield return node.Value.Key;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<VT> Values
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node in Nodes.Nodes)
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
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public VT this[KT key]
        {
            get
            {
                var value = default(VT);
                if (TryGetValue(key, (uint)key.GetHashCode(), out value)) return value;
                throw new IndexOutOfRangeException();
            }
            set { Set(key, (uint)key.GetHashCode(), value); }
        }
        /// <summary>
        /// 快照字典
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        public SnapshotDictionary(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType)
        {
            Nodes = new SnapshotDictionaryNodeArray<KT, VT>(this, (int)CapacityDivision.Divisor);
        }
        /// <summary>
        /// 清除所有数据并重建容器
        /// </summary>
        /// <param name="capacity"></param>
        internal void Renew(int capacity)
        {
            capacity = GetResizeCapacity(capacity);
            if (capacity != CapacityDivision.Divisor)
            {
                Nodes = new SnapshotDictionaryNodeArray<KT, VT>(this, capacity);
                CapacityDivision.Set(capacity);
                clearRemoveCount();
            }
            else ClearArray();
        }
        /// <summary>
        /// 清理数组
        /// </summary>
        internal void ClearArray()
        {
            if (!Nodes.ClearArray()) Nodes = new SnapshotDictionaryNodeArray<KT, VT>(this, (int)CapacityDivision.Divisor);
            clearRemoveCount();
        }
        /// <summary>
        /// 尝试修改访问节点索引位置
        /// </summary>
        /// <param name="nodeIndex"></param>
        private void changeIndex(int nodeIndex)
        {
            int orderIndex = nodeIndex - rollIndex, rollEndIndex = (Count >> 1) - 1;
            if (orderIndex < 0) orderIndex += Count;
            if (orderIndex <= rollEndIndex + ((Count - rollEndIndex) >> 1))
            {
                int changeIndex = nodeIndex + ((Count - orderIndex) >> 1);
                if (nodeIndex != changeIndex)
                {
                    if (changeIndex >= Count) changeIndex -= Count;
                    Nodes.TrySetSnapshotKeyValue(changeIndex);
                    Nodes.TrySetSnapshotKeyValue(nodeIndex);
                    change(nodeIndex, changeIndex, Nodes.Nodes);
                }
            }
        }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否获取成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value, bool isRoll = false)
#else
        public bool TryGetValue(KT key, out VT value, bool isRoll = false)
#endif
        {
            return TryGetValue(key, (uint)key.GetHashCode(), out value, isRoll);
        }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="hashCode">哈希值</param>
        /// <param name="value">目标数据</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否获取成功</returns>
#if NetStandard21
        internal bool TryGetValue(KT key, uint hashCode, [MaybeNullWhen(false)] out VT value, bool isRoll = false)
#else
        internal bool TryGetValue(KT key, uint hashCode, out VT value, bool isRoll = false)
#endif
        {
            if (Count != 0)
            {
                ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] nodeArray = Nodes.Nodes;
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = ref nodeArray[nodeIndex];
#else
                    ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = nodeArray[nodeIndex];
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
                            node = ref nodeArray[nodeIndex];
#else
                            node = nodeArray[nodeIndex];
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
        /// 添加数据
        /// </summary>
        /// <param name="hashCode">哈希值</param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(uint hashCode, BinarySerializeKeyValue<KT, VT> value)
        {
            if (Count == CapacityDivision.Divisor) resize();
            add(hashCode, value, Nodes.Nodes);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Set(KT key, VT value, bool isRoll = false)
        {
            return set(key, (uint)key.GetHashCode(), value, isRoll, false);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="hashCode">哈希值</param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Set(KT key, uint hashCode, VT value, bool isRoll = false)
        {
            return set(key, hashCode, value, isRoll, false);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Set(ref BinarySerializeKeyValue<KT, VT> keyValue, bool isRoll = false)
        {
            return set(keyValue.Key, (uint)keyValue.Key.GetHashCode(), keyValue.Value, isRoll, false);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Set(ref KeyValue<KT, VT> keyValue, bool isRoll = false)
        {
            return set(keyValue.Key, (uint)keyValue.Key.GetHashCode(), keyValue.Value, isRoll, false);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryAdd(KT key, VT value, bool isRoll = false)
        {
            return set(key, (uint)key.GetHashCode(), value, isRoll, true);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value"></param>
        /// <param name="hashCode">哈希值</param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool TryAdd(KT key, uint hashCode, VT value, bool isRoll = false)
        {
            return set(key, hashCode, value, isRoll, true);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value"></param>
        /// <param name="hashCode">哈希值</param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <param name="isAdd"></param>
        /// <returns>是否新增数据</returns>
        private bool set(KT key, uint hashCode, VT value, bool isRoll, bool isAdd)
        {
            int hashIndex = (int)CapacityDivision.GetMod(hashCode);
            ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] nodeArray = Nodes.Nodes;
            if (Count != 0)
            {
                int nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count && nodeArray[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = ref nodeArray[nodeIndex];
#else
                        ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = nodeArray[nodeIndex];
#endif
                        if (hashCode == node.HashCode && node.Value.Key.Equals(key))
                        {
                            if (!isAdd) Nodes.SetValue(nodeIndex, value);
                            if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return false;
                            changeIndex(nodeIndex);
                            return false;
                        }
                        if (node.Next == int.MaxValue)
                        {
                            if (Count != CapacityDivision.Divisor)
                            {
                                Nodes.TrySetSnapshotKeyValue(Count);
                                nodeArray[Count].Set((uint)nodeIndex | 0x80000000U, hashCode, new BinarySerializeKeyValue<KT, VT>(key, value));
                                nodeArray[nodeIndex].Next = Count++;
                            }
                            else
                            {
                                resize();
                                add(hashCode, new BinarySerializeKeyValue<KT, VT>(key, value), Nodes.Nodes);
                            }
                            return true;
                        }
                        nodeIndex = node.Next;
                    }
                    while (true);
                }
                if (Count != CapacityDivision.Divisor)
                {
                    Nodes.TrySetSnapshotKeyValue(Count);
                    nodeArray[Count].Set((uint)hashIndex, hashCode, new BinarySerializeKeyValue<KT, VT>(key, value));
                    nodeArray[hashIndex].HashIndex = Count++;
                }
                else
                {
                    resize();
                    add(hashCode, new BinarySerializeKeyValue<KT, VT>(key, value), Nodes.Nodes);
                }
            }
            else
            {
                Nodes.TrySetSnapshotKeyValue(0);
                nodeArray[0].Set((uint)hashIndex, hashCode, new BinarySerializeKeyValue<KT, VT>(key, value));
                nodeArray[hashIndex].HashIndex = 0;
                Count = 1;
            }
            return true;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        private void resize()
        {
            ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] nodeArray = Nodes.Nodes;
            int capacity = GetResizeCapacity((int)CapacityDivision.Divisor), rollIndex = this.rollIndex;

            Nodes = new SnapshotDictionaryNodeArray<KT, VT>(this, capacity);
            CapacityDivision.Set(capacity);
            switch (groupType)
            {
                case ReusableDictionaryGroupTypeEnum.HashIndexSort: resizeHashIndexSort(nodeArray, Nodes.Nodes); return;
                case ReusableDictionaryGroupTypeEnum.Roll: resizeRoll(nodeArray, rollIndex, Nodes.Nodes); return;
                default: resizeHashIndex(nodeArray, Nodes.Nodes); return;
            }
        }

        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(KT key, bool isRoll = false)
        {
            var value = default(VT);
            return TryGetValue(key, (uint)key.GetHashCode(), out value, isRoll);
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="hashCode">哈希值</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool ContainsKey(KT key, uint hashCode, bool isRoll = false)
        {
            var value = default(VT);
            return TryGetValue(key, hashCode, out value, isRoll);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(KT key)
        {
            var value = default(VT);
            return Remove(key, (uint)key.GetHashCode(), out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashCode">哈希值</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Remove(KT key, uint hashCode)
        {
            var value = default(VT);
            return Remove(key, hashCode, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool Remove(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool Remove(KT key, out VT value)
#endif
        {
            return Remove(key, (uint)key.GetHashCode(), out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashCode">哈希值</param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
#if NetStandard21
        internal bool Remove(KT key, uint hashCode, [MaybeNullWhen(false)] out VT value)
#else
        internal bool Remove(KT key, uint hashCode, out VT value)
#endif
        {
            if (Count != 0)
            {
                ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] nodeArray = Nodes.Nodes;
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = ref nodeArray[nodeIndex];
#else
                    ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = nodeArray[nodeIndex];
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
                            nodeArray[node.Next].Source = (uint)hashIndex;
                            nodeArray[hashIndex].HashIndex = node.Next;
                            if (nodeIndex != removeCount()) remove(nodeIndex, nodeArray[nodeIndex].HashIndex);
                        }
                        return true;
                    }
                    if (node.Source == hashIndex)
                    {
                        for (int nextNodeIndex = node.Next; nextNodeIndex < Count; nodeIndex = nextNodeIndex, nextNodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref nodeArray[nextNodeIndex];
#else
                            node = nodeArray[nextNodeIndex];
#endif
                            if (node.HashCode == hashCode && node.Value.Key.Equals(key))
                            {
                                value = node.Value.Value;
                                if (node.Next != int.MaxValue) nodeArray[node.Next].SetNextSource(nodeIndex);
                                nodeArray[nodeIndex].Next = node.Next;
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
        /// 删除节点
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="hashIndex"></param>
        private void remove(int nodeIndex, int hashIndex)
        {
            ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] nodeArray = Nodes.Nodes;
            ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = nodeArray[Count];
            node.HashIndex = hashIndex;
            Nodes.TrySetSnapshotKeyValue(nodeIndex);
            nodeArray[nodeIndex] = node;
            if (node.SourceHigh == 0) nodeArray[(int)node.Source].HashIndex = nodeIndex;
            else nodeArray[node.SourceIndex].Next = nodeIndex;
            if (node.Next != int.MaxValue) nodeArray[node.Next].SetNextSource(nodeIndex);
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveRoll()
        {
            BinarySerializeKeyValue<KT, VT> value;
            RemoveRoll(out value);
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
        public bool RemoveRoll(out BinarySerializeKeyValue<KT, VT> value)
        {
            if (Count != 0 && groupType == ReusableDictionaryGroupTypeEnum.Roll)
            {
                ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] nodeArray = Nodes.Nodes;
                if (rollIndex >= Count) rollIndex = 0;
#if NetStandard21
                ref ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = ref nodeArray[rollIndex];
#else
                ReusableHashNode<BinarySerializeKeyValue<KT, VT>> node = nodeArray[rollIndex];
#endif
                value = node.Value;
                int hashIndex = (int)CapacityDivision.GetMod(node.HashCode);
                if (node.SourceHigh == 0)
                {
                    if (node.Next != int.MaxValue)
                    {
                        nodeArray[node.Next].Source = (uint)hashIndex;
                        nodeArray[hashIndex].HashIndex = node.Next;
                        if (rollIndex != removeCount())
                        {
                            remove(rollIndex, nodeArray[rollIndex].HashIndex);
                            if (++rollIndex == Count) rollIndex = 0;
                        }
                        else rollIndex = 0;
                        return true;
                    }
                }
                else
                {
                    nodeArray[node.SourceIndex].Next = node.Next;
                    if (node.Next != int.MaxValue) nodeArray[node.Next].SetNextSource(node.SourceIndex);
                }
                if (rollIndex != removeCount())
                {
                    remove(rollIndex, node.HashIndex);
                    if (++rollIndex == Count) rollIndex = 0;
                }
                else rollIndex = 0;
                return true;
            }
            value = default(BinarySerializeKeyValue<KT, VT>);
            return false;
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
#if NetStandard21
        public bool RemoveRoll([MaybeNullWhen(false)] out VT value)
#else
        public bool RemoveRoll(out VT value)
#endif
        {
            BinarySerializeKeyValue<KT, VT> keyValue;
            if (RemoveRoll(out keyValue))
            {
                value = keyValue.Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 根据关键字集合获取匹配数据数组
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
#if NetStandard21
        public VT?[] GetValueArray(KT[] keys)
#else
        public VT[] GetValueArray(KT[] keys)
#endif
        {
            if (keys != null && keys.Length != 0)
            {
                VT[] values = new VT[keys.Length];
                var value = default(VT);
                int index = 0;
                foreach (KT key in keys)
                {
                    if (key != null && TryGetValue(key, (uint)key.GetHashCode(), out value)) values[index] = value;
                    ++index;
                }
                return values;
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>删除关键字数量</returns>
        public int RemoveKeys(KT[] keys)
        {
            int count = 0;
            foreach (KT key in keys)
            {
                if (key != null && Remove(key)) ++count;
            }
            return count;
        }
    }
}
