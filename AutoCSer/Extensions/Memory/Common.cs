using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Memory
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Common
    {
        /// <summary>
        /// 填充二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        internal static void FillBits(byte* data, int start, int count)
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
            AutoCSer.Common.Fill((ulong*)data, start = count >> 6, ulong.MaxValue);
            if ((count = -count & ((sizeof(ulong) << 3) - 1)) != 0) *(ulong*)(data + (start << 3)) |= ulong.MaxValue >> count;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static short GetShortBigEndian(byte* start)
        {
            short value;
            byte* data = (byte*)&value;
            data[0] = start[1];
            data[1] = start[0];
            return value;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetIntBigEndian(byte* start)
        {
            int value;
            byte* data = (byte*)&value;
            data[0] = start[3];
            data[1] = start[2];
            data[2] = start[1];
            data[3] = start[0];
            return value;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        internal static long GetLongBigEndian(byte* start)
        {
            long value;
            byte* data = (byte*)&value;
            data[0] = start[7];
            data[1] = start[6];
            data[2] = start[5];
            data[3] = start[4];
            data[4] = start[3];
            data[5] = start[2];
            data[6] = start[1];
            data[7] = start[0];
            return value;
        }
        /// <summary>
        /// 获取大端编码 16b 整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static uint GetUShortBigEndian(byte* start)
        {
            return ((uint)start[0] << 8) + (uint)start[1];
        }
        /// <summary>
        /// 字节流转32位无符号整数
        /// </summary>
        /// <param name="data">字节数组,不能为null</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <returns>无符号整数值</returns>
        internal unsafe static uint GetUIntBigEndian(this byte[] data, int startIndex)
        {
            fixed (byte* dataFixed = data) return GetUIntBigEndian(dataFixed + startIndex);
        }
        /// <summary>
        /// 获取大端编码 32b 整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static uint GetUIntBigEndian(byte* start)
        {
            uint value;
            byte* data = (byte*)&value;
            data[0] = start[3];
            data[1] = start[2];
            data[2] = start[1];
            data[3] = start[0];
            return value;
            //return ((uint)start[0] << 24) + ((uint)start[1] << 16) + ((uint)start[2] << 8) + (uint)start[3];
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ulong GetULongBigEndian(byte* start)
        {
            ulong value;
            byte* data = (byte*)&value;
            data[0] = start[7];
            data[1] = start[6];
            data[2] = start[5];
            data[3] = start[4];
            data[4] = start[3];
            data[5] = start[2];
            data[6] = start[1];
            data[7] = start[0];
            return value;
            //return ((ulong)start[0] << 56) + ((ulong)start[1] << 48) + ((ulong)start[2] << 40) + ((ulong)start[3] << 32)
            //    + ((ulong)start[4] << 24) + ((ulong)start[5] << 16) + ((ulong)start[6] << 8) + (ulong)start[7];
        }
        /// <summary>
        /// 计算 64 位稳定 HASH 值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static ulong getHashCode64(this byte[] data)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    fixed (byte* dataFixed = data) return AutoCSer.Memory.Common.GetHashCode64(dataFixed, data.Length);
                }
                return 0;
            }
            return ulong.MaxValue;
        }
    }
}
