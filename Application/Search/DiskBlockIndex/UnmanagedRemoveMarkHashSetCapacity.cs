using AutoCSer.Memory;
using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的非托管节点可重用哈希表容器参数
    /// </summary>
    internal sealed class UnmanagedRemoveMarkHashSetCapacity : RemoveMarkHashSetCapacity<UnmanagedRemoveMarkHashSetCapacity>
    {
        /// <summary>
        /// 节点集合非托管内存池
        /// </summary>
        internal readonly UnmanagedPool UnmanagedPool;
        /// <summary>
        /// 带移除标记的非托管节点可重用哈希表容器参数
        /// </summary>
        /// <param name="prime">取余质数</param>
        /// <param name="nodeBitCount">非托管节点数组大小二进制位数</param>
        /// <param name="next">下一个参数</param>
#if NetStandard21
        private UnmanagedRemoveMarkHashSetCapacity(int prime, byte nodeBitCount, UnmanagedRemoveMarkHashSetCapacity? next) 
#else
        private UnmanagedRemoveMarkHashSetCapacity(int prime, byte nodeBitCount, UnmanagedRemoveMarkHashSetCapacity next)
#endif
            : base(prime, nodeBitCount, next)
        {
            switch (nodeBitCount >> 1)
            {
                case 1: UnmanagedPool = UnmanagedPool.CachePage; break;
                case 2: UnmanagedPool = UnmanagedPool.Tiny; break;
                case 3: UnmanagedPool = UnmanagedPool.Kilobyte; break;
                case 4: UnmanagedPool = UnmanagedPool.Default; break;
                default: UnmanagedPool = UnmanagedPool.RadixSortCountBuffer; break;
            }
        }

        /// <summary>
        /// 默认带移除标记的可重用哈希表容器参数链表
        /// </summary>
        internal static readonly UnmanagedRemoveMarkHashSetCapacity DefaultLink = new UnmanagedRemoveMarkHashSetCapacity(7, 3
            , new UnmanagedRemoveMarkHashSetCapacity(31, 5
            , new UnmanagedRemoveMarkHashSetCapacity(127, 7
            , new UnmanagedRemoveMarkHashSetCapacity(509, 9
            , new UnmanagedRemoveMarkHashSetCapacity(1021, 10, null)))));
        ///// <summary>
        ///// 获取带移除标记的非托管节点可重用哈希表容器参数链表
        ///// </summary>
        ///// <returns></returns>
        //internal static UnmanagedRemoveMarkHashSetCapacity GetLink()
        //{
        //    return new UnmanagedRemoveMarkHashSetCapacity(7, 3
        //        , new UnmanagedRemoveMarkHashSetCapacity(31, 5
        //        , new UnmanagedRemoveMarkHashSetCapacity(127, 7
        //        , new UnmanagedRemoveMarkHashSetCapacity(509, 9
        //        , new UnmanagedRemoveMarkHashSetCapacity(1021, 10, null)))));
        //}
    }
}
