using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端下线计数对象
    /// </summary>
    public sealed class OfflineCount
    {
        /// <summary>
        /// 计数
        /// </summary>
        private int count;
        /// <summary>
        /// 获取计数
        /// </summary>
        /// <returns>0 表示需要释放计数</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Get()
        {
            return count != 0 ? 1 : Interlocked.CompareExchange(ref count, 1, 0);
        }

        /// <summary>
        /// 服务端下线计数对象空值
        /// </summary>
        internal static readonly OfflineCount Null = new OfflineCount { count = 1 };
    }
}
