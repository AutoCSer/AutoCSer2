using System;
using System.Collections.Generic;

namespace AutoCSer
{
    /// <summary>
    /// 可重用哈希表（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    [RemoteType]
    public sealed class ReusableHashSet<KT> : ReusableDictionary<KT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
#if AOT
        /// <summary>
        /// 比较器
        /// </summary>
        private readonly IEqualityComparer<keyType> comparer;
#endif
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<KT> Values
        {
            get
            {
                if (count != 0)
                {
                    int index = count;
                    foreach (ReusableDictionaryNode<KT> node in Nodes)
                    {
                        yield return node.Value;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
#if AOT
        /// <param name="comparer">比较器</param>
#endif
        public ReusableHashSet(int capacity = 0
#if AOT
            , IEqualityComparer<keyType> comparer
#endif
) : base(capacity)
        {
#if AOT
            this.comparer = comparer;
#endif
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        public bool Add(KT value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int hashIndex = (int)CapacityDivision.GetMod(hashCode);
            if (count != 0)
            {
                int nodeIndex = Nodes[hashIndex].LinkIndex;
                if (nodeIndex < count && Nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableDictionaryNode<KT> node = ref Nodes[nodeIndex];
#else
                        ReusableDictionaryNode<KT> node = nodes[nodeIndex];
#endif
                        if (hashCode == node.HashCode && node.Value.Equals(value)) return false;
                        if (node.Next == int.MaxValue)
                        {
                            if (count != CapacityDivision.Divisor)
                            {
                                Nodes[count].Set((uint)nodeIndex | 0x80000000U, hashCode, value);
                                Nodes[nodeIndex].Next = count++;
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
                if (count != CapacityDivision.Divisor)
                {
                    Nodes[count].Set((uint)hashIndex, hashCode, value);
                    Nodes[hashIndex].LinkIndex = count++;
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
                Nodes[hashIndex].LinkIndex = 0;
                count = 1;
            }
            return true;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        public bool Contains(KT value)
        {
            if (count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = Nodes[hashIndex].LinkIndex;
                if (nodeIndex < count)
                {
#if NetStandard21
                    ref ReusableDictionaryNode<KT> node = ref Nodes[nodeIndex];
#else
                    ReusableDictionaryNode<KT>> node = nodes[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Equals(value)) return true;
                    if (node.Source == hashIndex)
                    {
                        for (int nextNodeIndex = node.Next; nextNodeIndex < count; nextNodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nextNodeIndex];
#else
                            node = nodes[nextNodeIndex];
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
        public bool Remove(KT value)
        {
            if (count != 0)
            {
                uint hashCode = (uint)value.GetHashCode();
                int hashIndex = (int)CapacityDivision.GetMod(hashCode), nodeIndex = Nodes[hashIndex].LinkIndex;
                if (nodeIndex < count)
                {
#if NetStandard21
                    ref ReusableDictionaryNode<KT> node = ref Nodes[nodeIndex];
#else
                    ReusableDictionaryNode<KT> node = nodes[nodeIndex];
#endif
                    if (node.HashCode == hashCode && node.Source == hashIndex && node.Value.Equals(value))
                    {
                        if (node.Next != int.MaxValue) Nodes[node.Next].Source = (uint)hashIndex;
                        Nodes[hashIndex].LinkIndex = node.Next;
                        remove(nodeIndex);
                        return true;
                    }
                    if (node.Source == hashIndex)
                    {
                        for (int nextNodeIndex = node.Next; nextNodeIndex < count; nodeIndex = nextNodeIndex, nextNodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nextNodeIndex];
#else
                            node = nodes[nextNodeIndex];
#endif
                            if (node.HashCode == hashCode && node.Value.Equals(value))
                            {
                                if (node.Next != int.MaxValue) Nodes[node.Next].Source = (uint)nodeIndex | 0x80000000U;
                                Nodes[nodeIndex].Next = node.Next;
                                remove(nextNodeIndex);
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
