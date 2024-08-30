using System;

namespace AutoCSer.Extensions.Memory
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal static class Common
    {
        /// <summary>
        /// 填充二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        internal unsafe static void FillBits(byte* data, int start, int count)
        {
#if DEBUG
            if (data == null) throw new Exception("data == null");
            if (start < 0) throw new Exception(start.toString() + " < 0");
            if (count <= 0) throw new Exception(count.toString() + " <= 0");
#endif
            data += (start >> 6) << 3;
            if ((start &= ((sizeof(ulong) << 3) - 1)) != 0)
            {
                int high = (sizeof(ulong) << 3) - start;
                if ((count -= high) >= 0)
                {
                    *(ulong*)data |= ulong.MaxValue << start;
                    data += sizeof(ulong);
                }
                else
                {
                    *(ulong*)data |= (ulong.MaxValue >> (start - count)) << start;
                    return;
                }
            }
            if ((start = count >> 6) != 0) AutoCSer.Common.Config.Fill((ulong*)data, start, ulong.MaxValue);
            if ((count = -count & ((sizeof(ulong) << 3) - 1)) != 0) *(ulong*)(data + (start << 3)) |= ulong.MaxValue >> count;
        }
    }
}
