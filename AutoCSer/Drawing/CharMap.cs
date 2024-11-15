using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 字符位图
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
    public sealed class CharMap
    {
        /// <summary>
        /// 0-3 行
        /// </summary>
        private readonly ulong map0;
        /// <summary>
        /// 4-7 行
        /// </summary>
        private readonly ulong map1;
        /// <summary>
        /// 8-11 行
        /// </summary>
        private readonly ulong map2;
        /// <summary>
        /// 12-15 行
        /// </summary>
        private readonly ulong map3;
        /// <summary>
        /// 左侧起始位置
        /// </summary>
        public readonly short Left;
        /// <summary>
        /// 顶部起始位置
        /// </summary>
        public readonly short Top;
        /// <summary>
        /// 宽度
        /// </summary>
        public readonly short Width;
        /// <summary>
        /// 高度
        /// </summary>
        public readonly short Height;
        /// <summary>
        /// 字符位图
        /// </summary>
        private CharMap() { }
        /// <summary>
        /// 字符位图
        /// </summary>
        /// <param name="lockBitmap"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="color"></param>
        internal CharMap(LockBitmap lockBitmap, short left, short top, LockBitmapColor color)
        {
            Left = left;
            Top = top;
            int width0, width1, width2, width3;
            map0 = readColorMap(lockBitmap, left, top, color, out width0);
            map1 = readColorMap(lockBitmap, left, top + 4, color, out width1);
            map2 = readColorMap(lockBitmap, left, top + 8, color, out width2);
            map3 = readColorMap(lockBitmap, left, top + 12, color, out width3);
            Width = (short)Math.Max(Math.Max(width0, width1), Math.Max(width2, width3));
            short bitHeight = getHeight(map3);
            if (bitHeight != 0) Height = (short)(12 + bitHeight);
            else
            {
                bitHeight = getHeight(map2);
                if (bitHeight != 0) Height = (short)(8 + bitHeight);
                else
                {
                    bitHeight = getHeight(map1);
                    Height = bitHeight != 0 ? (short)(4 + bitHeight) : (short)getHeight(map0);
                }
            }
        }
        /// <summary>
        /// 写入字符
        /// </summary>
        /// <param name="lockBitmap">位图</param>
        /// <param name="left">位图位置</param>
        /// <param name="top">位图位置</param>
        /// <param name="color"></param>
        public void Write(LockBitmap lockBitmap, int left, int top, LockBitmapColor color)
        {
            if (left > -Width && left < lockBitmap.Width)
            {
                top += Top;
                writeColorMap(lockBitmap, left, top, map0, color);
                if (map1 != 0) writeColorMap(lockBitmap, left, top + 4, map1, color);
                if (map2 != 0) writeColorMap(lockBitmap, left, top + 8, map2, color);
                if (map3 != 0) writeColorMap(lockBitmap, left, top + 12, map3, color);
            }
        }

        /// <summary>
        /// 读取颜色位图
        /// </summary>
        /// <param name="lockBitmap"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private static ulong readColorMap(LockBitmap lockBitmap, int left, int top, LockBitmapColor color, out int width)
        {
            int width0, width1, width2, width3;
            ulong map = lockBitmap.ReadColorMap(left, top, color, out width0)
                | (lockBitmap.ReadColorMap(left, top + 1, color, out width1) << 16)
                | (lockBitmap.ReadColorMap(left, top + 2, color, out width2) << 32)
                | (lockBitmap.ReadColorMap(left, top + 3, color, out width3) << 48);
            width = Math.Max(Math.Max(width0, width1), Math.Max(width2, width3));
            return map;
        }
        /// <summary>
        /// 获取颜色位图有效行数
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static short getHeight(ulong map)
        {
            if (map == 0) return 0;
            if ((map & 0xffffffff00000000UL) == 0) return (map & 0xffff0000) == 0 ? (short)1 : (short)2;
            return (map & 0xffff000000000000UL) == 0 ? (short)3 : (short)4;
        }
        /// <summary>
        /// 写入颜色位图
        /// </summary>
        /// <param name="lockBitmap"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="map"></param>
        /// <param name="color"></param>
        private static void writeColorMap(LockBitmap lockBitmap, int left, int top, ulong map, LockBitmapColor color)
        {
            lockBitmap.WriteColorMap(left, top, map & 0xffff, color);
            lockBitmap.WriteColorMap(left, top + 1, (map >> 16) & 0xffff, color);
            lockBitmap.WriteColorMap(left, top + 2, (map >> 32) & 0xffff, color);
            lockBitmap.WriteColorMap(left, top + 3, map >> 48, color);
        }
        /// <summary>
        /// 空字符位图
        /// </summary>
        public static readonly CharMap Empty = new CharMap();
    }
}
