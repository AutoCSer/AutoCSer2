using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Array substring expansion operation
    /// 数组子串扩展操作
    /// </summary>
    public static class SubArrayExtension
    {
        /// <summary>
        /// Create a memory byte stream
        /// 创建内存字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static MemoryStream createMemoryStream(this SubArray<byte> data)
        {
            return new MemoryStream(data.Array, data.Start, data.Length);
        }
    }
}
