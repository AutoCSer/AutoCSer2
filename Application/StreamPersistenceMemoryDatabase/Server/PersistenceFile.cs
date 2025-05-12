using System;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 持久化文件信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PersistenceFile
    {
        /// <summary>
        /// 持久化流重建起始位置
        /// </summary>
        internal ulong RebuildPosition;
        /// <summary>
        /// 持久化文件信息
        /// </summary>
#if NetStandard21
        internal FileInfo? FileInfo;
#else
        internal FileInfo FileInfo;
#endif
        /// <summary>
        /// 持久化回调异常位置文件信息
        /// </summary>
#if NetStandard21
        internal FileInfo? CallbackExceptionPositionFileInfo;
#else
        internal FileInfo CallbackExceptionPositionFileInfo;
#endif

        /// <summary>
        /// 文件排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int comparer(PersistenceFile left, PersistenceFile right)
        {
            return left.RebuildPosition.CompareTo(right.RebuildPosition);
        }
        /// <summary>
        /// 文件排序
        /// </summary>
        internal static readonly Func<PersistenceFile, PersistenceFile, int> Comparer = comparer;
    }
}
