using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照节点
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SnapshotArrayNode<T>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        internal T Value;
        /// <summary>
        /// 是否存在快照数据
        /// </summary>
        internal bool IsSnapshot;
        /// <summary>
        /// 设置快照数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TrySet(T value)
        {
            if (!IsSnapshot)
            {
                this.Value = value;
                IsSnapshot = true;
            }
        }
    }
}
