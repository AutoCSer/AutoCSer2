using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁标识信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct DistributedLockIdentity<T>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        internal T Key;
        /// <summary>
        /// 当前超时时间
        /// </summary>
        internal DateTime Timeout;
        /// <summary>
        /// 锁操作标识
        /// </summary>
        internal long Identity;
        /// <summary>
        /// 设置锁操作标识
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(long identity)
        {
            Identity = identity;
        }
        /// <summary>
        /// 设置锁信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T key, DateTime timeout, long identity)
        {
            Key = key;
            Timeout = timeout;
            Identity = identity;
        }
        /// <summary>
        /// 设置锁信息
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(DateTime timeout, long identity)
        {
            Timeout = timeout;
            Identity = identity;
        }
    }
}
