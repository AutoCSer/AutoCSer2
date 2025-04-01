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
    public sealed class SnapshotHashSet<T> : ReusableDictionary
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
        /// 可重用哈希表
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public SnapshotHashSet(int capacity = 0) : base(capacity)
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
                if (nodeIndex != changeIndex) change(nodeIndex, changeIndex < Count ? changeIndex : changeIndex - Count);
            }
        }
        /// <summary>
        /// 交换节点位置
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="changeIndex"></param>
        private void change(int nodeIndex, int changeIndex)
        {
            ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
            Nodes.TrySetSnapshotValue(changeIndex);
            Nodes.TrySetSnapshotValue(nodeIndex);
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
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
#if NetStandard21
        public bool Contains(T key, bool isRoll = false)
#else
        public bool Contains(T key, bool isRoll = false)
#endif
        {
            if (Count != 0)
            {
                ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
                uint hashCode = (uint)key.GetHashCode();
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
                        if (!isRoll) return true;
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
                                if (!isRoll) return true;
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
            return add(key, isRoll);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        private bool add(T key, bool isRoll)
        {
            uint hashCode = (uint)key.GetHashCode();
            int hashIndex = (int)CapacityDivision.GetMod(hashCode);
            ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
            if (Count != 0)
            {
                int nodeIndex = nodeArray[hashIndex].HashIndex;
                if (nodeIndex < Count && nodeArray[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode<T> node = ref nodeArray[nodeIndex];
#else
                        ReusableHashNode<T> node = nodeArray[nodeIndex];
#endif
                        if (hashCode == node.HashCode && node.Value.Equals(key)) return false;
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
                                add(hashCode, key);
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
                    add(hashCode, key);
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
            resize(nodeArray, rollIndex);
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="rollIndex"></param>
        private void resize(ReusableHashNode<T>[] nodes, int rollIndex)
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
        /// 新增数据
        /// </summary>
        /// <param name="hashCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private void add(uint hashCode, T value)
        {
            ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
            int hashIndex = (int)CapacityDivision.GetMod(hashCode), linkIndex = nodeArray[hashIndex].HashIndex;
            if (linkIndex < Count && nodeArray[linkIndex].Source == (uint)hashIndex)
            {
                nodeArray[linkIndex].SetNextSource(Count);
                nodeArray[Count].Set((uint)hashIndex, hashCode, value, linkIndex);
            }
            else nodeArray[Count].Set((uint)hashIndex, hashCode, value);
            nodeArray[hashIndex].HashIndex = Count++;
        }

        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(T key)
        {
            if (Count != 0)
            {
                ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
                uint hashCode = (uint)key.GetHashCode();
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
        /// <returns>是否存在数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveRoll()
        {
            if (Count != 0)
            {
                ReusableHashNode<T>[] nodeArray = Nodes.Nodes;
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
                if (key != null && Remove(key)) ++count;
            }
            return count;
        }
    }
}
