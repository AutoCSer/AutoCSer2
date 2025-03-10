using System;
using System.Collections.Generic;

namespace AutoCSer
{
    /// <summary>
    /// 可重用哈希表（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [RemoteType]
    public class ReusableHashSet<T> : ReusableDictionary<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
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
                    foreach (ReusableHashNode<T> node in Nodes)
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
        public ReusableHashSet(int capacity = 0) : base(capacity) { }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        public bool Add(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int hashIndex = (int)CapacityDivision.GetMod(hashCode);
            if (Count != 0)
            {
                int nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode<T> node = ref Nodes[nodeIndex];
#else
                        ReusableHashNode<T> node = Nodes[nodeIndex];
#endif
                        if (hashCode == node.HashCode && node.Value.Equals(value)) return false;
                        if (node.Next == int.MaxValue)
                        {
                            if (Count != CapacityDivision.Divisor)
                            {
                                Nodes[Count].Set((uint)nodeIndex | 0x80000000U, hashCode, value);
                                Nodes[nodeIndex].Next = Count++;
                            }
                            else
                            {
                                resize();
                                add(hashCode, value);
                            }
                            return true;
                        }
                        nodeIndex = node.Next;
                    }
                    while (true);
                }
                if (Count != CapacityDivision.Divisor)
                {
                    Nodes[Count].Set((uint)hashIndex, hashCode, value);
                    Nodes[hashIndex].HashIndex = Count++;
                }
                else
                {
                    resize();
                    add(hashCode, value);
                }
            }
            else
            {
                Nodes[0].Set((uint)hashIndex, hashCode, value);
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
        public bool Contains(T value)
        {
            if (Count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<T> node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode<T> node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Equals(value)) return true;
                    if (node.Source == hashIndex)
                    {
                        for (int nextNodeIndex = node.Next; nextNodeIndex < Count; nextNodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nextNodeIndex];
#else
                            node = Nodes[nextNodeIndex];
#endif
                            if (node.HashCode == hashCode && node.Value.Equals(value)) return true;
                        }
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
        public bool Remove(T value)
        {
            if (Count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<T> node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode<T> node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Equals(value))
                    {
                        if (node.Next == int.MaxValue)
                        {
                            if (nodeIndex != --Count) remove(nodeIndex, node.HashIndex);
                        }
                        else
                        {
                            Nodes[node.Next].Source = (uint)hashIndex;
                            Nodes[hashIndex].HashIndex = node.Next;
                            if (nodeIndex != --Count) remove(nodeIndex, Nodes[nodeIndex].HashIndex);
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
                            if (node.HashCode == hashCode && node.Value.Equals(value))
                            {
                                if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(nodeIndex);
                                Nodes[nodeIndex].Next = node.Next;
                                if (nextNodeIndex != --Count) remove(nextNodeIndex, node.HashIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
