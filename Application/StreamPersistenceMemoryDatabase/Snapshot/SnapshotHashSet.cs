using AutoCSer.Algorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照哈希表
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public sealed class SnapshotHashSet<T> : SnapshotDictionary<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 快照哈希表节点数组
        /// </summary>
        internal SnapshotHashSetNodeArray<T> Nodes;
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<T> ValueSnapshot { get { return Nodes; } }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<T> node in Nodes.Nodes)
                    {
                        yield return node.Value;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 快照哈希表
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        public SnapshotHashSet(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType)
        {
            Nodes = new SnapshotHashSetNodeArray<T>(this, (int)CapacityDivision.Divisor);
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
                Nodes = new SnapshotHashSetNodeArray<T>(this, capacity);
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
            if (!Nodes.ClearArray()) Nodes = new SnapshotHashSetNodeArray<T>(this, (int)CapacityDivision.Divisor);
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
                    Nodes.TrySetSnapshotValue(changeIndex);
                    Nodes.TrySetSnapshotValue(nodeIndex);
                    change(nodeIndex, changeIndex, Nodes.Nodes);
                }
            }
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool Contains(T key, bool isRoll = false)
#else
        public bool Contains(T key, bool isRoll = false)
#endif
        {
            return Contains(key, (uint)key.GetHashCode(), isRoll);
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="hashCode"></param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
#if NetStandard21
        internal bool Contains(T key, uint hashCode, bool isRoll = false)
#else
        internal bool Contains(T key, uint hashCode, bool isRoll = false)
#endif
        {
            if (Count != 0)
            {
                ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<T> node = ref nodeArray[nodeIndex];
#else
                    ReusableHashNode<T> node = nodeArray[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Equals(key))
                    {
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
                            if (node.HashCode == hashCode && node.Value.Equals(key))
                            {
                                if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return true;
                                changeIndex(nodeIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Add(T key, bool isRoll = false)
        {
            return Add(key, (uint)key.GetHashCode(), isRoll);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="hashCode"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        internal bool Add(T key, uint hashCode, bool isRoll = false)
        {
            int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex;
            ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
            if (Count != 0)
            {
                nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count && nodeArray[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode<T> node = ref nodeArray[nodeIndex];
#else
                        ReusableHashNode<T> node = nodeArray[nodeIndex];
#endif
                        if (hashCode == node.HashCode && node.Value.Equals(key))
                        {
                            if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return false;
                            changeIndex(nodeIndex);
                            return false;
                        }
                        if (node.Next == int.MaxValue)
                        {
                            if (Count != CapacityDivision.Divisor)
                            {
                                Nodes.TrySetSnapshotValue(Count);
                                nodeArray[Count].Set((uint)nodeIndex | 0x80000000U, hashCode, key);
                                nodeArray[nodeIndex].Next = Count++;
                            }
                            else
                            {
                                resize();
                                add(hashCode, key, Nodes.Nodes);
                            }
                            return true;
                        }
                        nodeIndex = node.Next;
                    }
                    while (true);
                }
                if (Count != CapacityDivision.Divisor)
                {
                    Nodes.TrySetSnapshotValue(Count);
                    nodeArray[Count].Set((uint)hashIndex, hashCode, key);
                    nodeArray[hashIndex].HashIndex = Count++;
                }
                else
                {
                    resize();
                    add(hashCode, key, Nodes.Nodes);
                }
            }
            else
            {
                Nodes.TrySetSnapshotValue(0);
                nodeArray[0].Set((uint)hashIndex, hashCode, key);
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
            ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
            int capacity = GetResizeCapacity((int)CapacityDivision.Divisor), rollIndex = this.rollIndex;

            Nodes = new SnapshotHashSetNodeArray<T>(this, capacity);
            CapacityDivision.Set(capacity);
            switch (groupType)
            {
                case ReusableDictionaryGroupTypeEnum.HashIndexSort: resizeHashIndexSort(nodeArray, Nodes.Nodes); return;
                case ReusableDictionaryGroupTypeEnum.Roll: resizeRoll(nodeArray, rollIndex, Nodes.Nodes); return;
                default: resizeHashIndex(nodeArray, Nodes.Nodes); return;
            }
        }

        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(T key)
        {
            return Remove(key, (uint)key.GetHashCode());
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashCode"></param>
        /// <returns>是否存在关键字</returns>
        internal bool Remove(T key, uint hashCode)
        {
            if (Count != 0)
            {
                ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<T> node = ref nodeArray[nodeIndex];
#else
                    ReusableHashNode<T> node = nodeArray[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Equals(key))
                    {
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
                            if (node.HashCode == hashCode && node.Value.Equals(key))
                            {
                                if (node.Next != int.MaxValue) nodeArray[node.Next].SetNextSource(nodeIndex);
                                nodeArray[nodeIndex].Next = node.Next;
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
            ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
            ReusableHashNode<T> node = nodeArray[Count];
            node.HashIndex = hashIndex;
            Nodes.TrySetSnapshotValue(nodeIndex);
            nodeArray[nodeIndex] = node;
            if (node.SourceHigh == 0) nodeArray[(int)node.Source].HashIndex = nodeIndex;
            else nodeArray[node.SourceIndex].Next = nodeIndex;
            if (node.Next != int.MaxValue) nodeArray[node.Next].SetNextSource(nodeIndex);
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveRoll()
        {
            if (Count != 0 && groupType == ReusableDictionaryGroupTypeEnum.Roll)
            {
                ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
                if (rollIndex >= Count) rollIndex = 0;
#if NetStandard21
                ref ReusableHashNode<T> node = ref nodeArray[rollIndex];
#else
                ReusableHashNode<T> node = nodeArray[rollIndex];
#endif
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
            return false;
        }
        /// <summary>
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>删除关键字数量</returns>
        public int RemoveKeys(T[] keys)
        {
            int count = 0;
            foreach (T key in keys)
            {
                if (key != null && Remove(key, (uint)key.GetHashCode())) ++count;
            }
            return count;
        }
    }
}
