using System;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针位图
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct BitMap
    {
        /// <summary>
        /// 位图指针
        /// </summary>
        internal byte* Map;
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        internal BitMap(void* map)
        {
            Map = (byte*)map;
        }
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        /// <param name="count">整数数量,大于0</param>
        internal BitMap(ulong* map, int count)
        {
            Map = (byte*)map;
            AutoCSer.Common.Clear(map, count);
        }
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        /// <param name="count">整数数量,大于0</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ulong* map, int count)
        {
            Map = (byte*)map;
            AutoCSer.Memory.Common.Clear(map, count);
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Get(int bit)
        {
            return Map[bit >> 3] & (1 << (bit & 7));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int bit)
        {
            Map[bit >> 3] |= (byte)(1 << (bit & 7));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns></returns>
        internal bool IsSet(int bit)
        {
            int index = bit >> 3, value = (1 << (bit & 7));
            if ((Map[index] & value) == 0)
            {
                Map[index] |= (byte)value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置占位段
        /// </summary>
        /// <param name="start">位值</param>
        /// <param name="count">段长</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int start, int count)
        {
            if (start < 0)
            {
                count += start;
                start = 0;
            }
            if (count > 0) AutoCSer.Extensions.Memory.Common.FillBits(Map, start, count);
        }
    }
}
