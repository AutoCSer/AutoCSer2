using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 快照集合扩展
    /// </summary>
    public static class SnapshotEnumerableExtension
    {
        /// <summary>
        /// 快照数据类型转换
        /// </summary>
        /// <typeparam name="ST">数据类型</typeparam>
        /// <typeparam name="T">持久化类型</typeparam>
        /// <param name="snapshot">快照集合</param>
        /// <param name="getValue">持久化类型</param>
        /// <returns>快照集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static SnapshotEnumerableCast<ST, T> Cast<ST, T>(this ISnapshotEnumerable<ST> snapshot, Func<ST, T> getValue)
        {
            return new SnapshotEnumerableCast<ST, T>(snapshot, getValue);
        }
        /// <summary>
        /// 快照数据类型转换
        /// </summary>
        /// <typeparam name="ST">数据类型</typeparam>
        /// <typeparam name="T">持久化类型</typeparam>
        /// <param name="snapshot">快照集合</param>
        /// <param name="getValue">持久化类型</param>
        /// <param name="getIsSnapshot">持久化类型</param>
        /// <returns>快照集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static SnapshotEnumerableCastEmpty<ST, T> Cast<ST, T>(this ISnapshotEnumerable<ST> snapshot, Func<ST, T> getValue, Func<bool> getIsSnapshot)
        {
            return new SnapshotEnumerableCastEmpty<ST, T>(snapshot, getValue, getIsSnapshot);
        }

    }
}
