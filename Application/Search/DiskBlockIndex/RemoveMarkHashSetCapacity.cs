using AutoCSer.Algorithm;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希表容器参数
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    internal abstract class RemoveMarkHashSetCapacity<T> : AutoCSer.Threading.Link<T>
        where T : RemoveMarkHashSetCapacity<T>
    {
        /// <summary>
        /// 容器大小
        /// </summary>
        internal readonly int Capacity;
        /// <summary>
        /// 计算哈希索引位置最低位位移数量
        /// </summary>
        private readonly int hashIndexShiftBit;
        /// <summary>
        /// 哈希取余
        /// </summary>
        private readonly IntegerDivision integerDivision;
        /// <summary>
        /// 带移除标记的可重用哈希表容器参数
        /// </summary>
        /// <param name="prime">取余质数</param>
        /// <param name="nodeBitCount">非托管节点数组大小二进制位数</param>
        /// <param name="next">下一个参数</param>
#if NetStandard21
        protected RemoveMarkHashSetCapacity(int prime, byte nodeBitCount, T? next)
#else
        protected RemoveMarkHashSetCapacity(int prime, byte nodeBitCount, T next)
#endif
        {
            Capacity = (1 << nodeBitCount) - 1;
            hashIndexShiftBit = (Capacity - prime) >> 1;
            if (hashIndexShiftBit == 0) hashIndexShiftBit = 32;
            integerDivision = new IntegerDivision(prime);
            LinkNext = next;
        }
        /// <summary>
        /// 根据指定容器大小获取容器参数
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        internal T Get(int capacity)
        {
            var nextCapacity = this;
            do
            {
                if (nextCapacity.Capacity >= capacity) return (T)nextCapacity;
                nextCapacity = nextCapacity.LinkNext;
            }
            while (nextCapacity != null);
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// 获取哈希索引位置
        /// </summary>
        /// <param name="hashCode">哈希值</param>
        /// <returns>哈希索引位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetHashIndex(uint hashCode)
        {
            return integerDivision.GetMod((int)(hashCode >> 1)) + (int)((hashCode & 1) << hashIndexShiftBit);
        }
    }
    /// <summary>
    /// 带移除标记的可重用哈希表容器参数
    /// </summary>
    internal sealed class RemoveMarkHashSetCapacity : RemoveMarkHashSetCapacity<RemoveMarkHashSetCapacity>
    {
        /// <summary>
        /// 最大容器大小
        /// </summary>
        internal const int MaxCapacity = (1 << 10) - 1;
        /// <summary>
        /// 带移除标记的可重用哈希表容器参数
        /// </summary>
        /// <param name="prime">取余质数</param>
        /// <param name="nodeBitCount">非托管节点数组大小二进制位数</param>
        /// <param name="next">下一个参数</param>
#if NetStandard21
        internal RemoveMarkHashSetCapacity(int prime, byte nodeBitCount, RemoveMarkHashSetCapacity? next) 
#else
        internal RemoveMarkHashSetCapacity(int prime, byte nodeBitCount, RemoveMarkHashSetCapacity next)
#endif
            : base(prime, nodeBitCount, next) { }

        /// <summary>
        /// 默认带移除标记的可重用哈希表容器参数链表
        /// </summary>
        internal static readonly RemoveMarkHashSetCapacity DefaultLink = new RemoveMarkHashSetCapacity(3, 2
                , new RemoveMarkHashSetCapacity(7, 3
                , new RemoveMarkHashSetCapacity(13, 4
                , new RemoveMarkHashSetCapacity(31, 5
                , new RemoveMarkHashSetCapacity(61, 6
                , new RemoveMarkHashSetCapacity(127, 7
                , new RemoveMarkHashSetCapacity(251, 8
                , new RemoveMarkHashSetCapacity(509, 9
                , new RemoveMarkHashSetCapacity(1021, 10, null)))))))));
    }
}
