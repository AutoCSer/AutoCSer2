using System;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Persistent file information
    /// 持久化文件信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PersistenceFile
    {
        /// <summary>
        /// The starting position of persistent flow rebuild
        /// 持久化流重建起始位置
        /// </summary>
        internal ulong RebuildPosition;
        /// <summary>
        /// Persistent file information
        /// 持久化文件信息
        /// </summary>
#if NetStandard21
        internal FileInfo? FileInfo;
#else
        internal FileInfo FileInfo;
#endif
        /// <summary>
        /// Persistent callback exception location file information
        /// 持久化回调异常位置文件信息
        /// </summary>
#if NetStandard21
        internal FileInfo? CallbackExceptionPositionFileInfo;
#else
        internal FileInfo CallbackExceptionPositionFileInfo;
#endif

        /// <summary>
        /// File sorting
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int comparer(PersistenceFile left, PersistenceFile right)
        {
            return left.RebuildPosition.CompareTo(right.RebuildPosition);
        }
        /// <summary>
        /// File sorting
        /// </summary>
        internal static readonly Func<PersistenceFile, PersistenceFile, int> Comparer = comparer;
    }
}
