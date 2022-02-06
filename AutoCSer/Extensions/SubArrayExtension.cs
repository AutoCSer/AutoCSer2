using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展操作
    /// </summary>
    public static class SubArrayExtension
    {
        /// <summary>
        /// 创建内存字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemoryStream createMemoryStream(this SubArray<byte> data)
        {
            return new MemoryStream(data.Array, data.Start, data.Length);
        }
    }
}
