﻿using AutoCSer.Memory;
using System;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 基数排序
    /// </summary>
    internal unsafe static class RadixSort
    {
        /// <summary>
        /// 32B 基数排序数据量
        /// </summary>
        internal const int SortSize32 = 1 << 9;
        /// <summary>
        /// 64B 基数排序数据量
        /// </summary>
        internal const int SortSize64 = 4 << 9;

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static void sort(uint* arrayFixed, uint* newArrayFixed, uint* swapFixed, int length)
        {
            UnmanagedPoolPointer countPointer = UnmanagedPool.Default.GetPoolPointer();
            try
            {
                AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                int* count0 = countPointer.Pointer.Int + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    ++count0[*(byte*)start];
                    ++count8[*((byte*)start + 1)];
                    ++count16[*((byte*)start + 2)];
                    byte value = *((byte*)start + 3);
                    ++count24[++value];
                }
                int index = *count0;
                for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
                index = *count8;
                for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
                index = *count16;
                for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
                *count24 = 0;
                index = *(count24 + 1);
                for (int* start = count24 + 2, end = count24 + 256; start != end; *start++ = index) index += *start;
                *--count0 = 0;
                for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    swapFixed[count0[*(byte*)start]++] = *start;
                }
                *--count8 = 0;
                for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count8[*((byte*)start + 1)]++] = *start;
                }
                *--count16 = 0;
                for (uint* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count16[*((byte*)start + 2)]++] = *start;
                }
                for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count24[*((byte*)start + 3)]++] = *start;
                }
            }
            finally { countPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        internal static void Sort(uint[] array, int startIndex, int count)
        {
            UnmanagedPoolPointer swap = UnmanagedPool.GetPoolPointer(count * sizeof(uint));
            try
            {
                fixed (uint* arrayFixed = array)
                {
                    uint* start = arrayFixed + startIndex;
                    sort(start, start, swap.Pointer.UInt, count);
                }
            }
            finally { swap.PushOnly(); }
        }

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static void sort(int* arrayFixed, int* newArrayFixed, uint* swapFixed, int length)
        {
            UnmanagedPoolPointer countPointer = UnmanagedPool.Default.GetPoolPointer();
            try
            {
                AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                int* count0 = countPointer.Pointer.Int + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                for (int* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    ++count0[*(byte*)start];
                    ++count8[*((byte*)start + 1)];
                    ++count16[*((byte*)start + 2)];
                    byte value = *((byte*)start + 3);
                    value ^= 0x80;
                    ++count24[++value];
                }
                int index = *count0;
                for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
                index = *count8;
                for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
                index = *count16;
                for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
                *count24 = 0;
                index = *(count24 + 1);
                for (int* start = count24 + 2, end = count24 + 256; start != end; *start++ = index) index += *start;
                *--count0 = 0;
                for (int* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    swapFixed[count0[*(byte*)start]++] = (uint)*start ^ 0x80000000U;
                }
                *--count8 = 0;
                for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count8[*((byte*)start + 1)]++] = (int)*start;
                }
                *--count16 = 0;
                for (int* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count16[*((byte*)start + 2)]++] = (uint)*start;
                }
                for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count24[*((byte*)start + 3)]++] = (int)(*start ^ 0x80000000U);
                }
            }
            finally { countPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        internal static void Sort(int[] array, int startIndex, int count)
        {
            UnmanagedPoolPointer swap = UnmanagedPool.GetPoolPointer(count * sizeof(uint));
            try
            {
                fixed (int* arrayFixed = array)
                {
                    int* start = arrayFixed + startIndex;
                    sort(start, start, swap.Pointer.UInt, count);
                }
            }
            finally { swap.PushOnly(); }
        }

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static void sort(ulong* arrayFixed, ulong* newArrayFixed, ulong* swapFixed, int length)
        {
            UnmanagedPoolPointer countPointer = UnmanagedPool.RadixSortCountBuffer.GetPoolPointer();
            try
            {
                AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                int* count0 = countPointer.Pointer.Int + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    ++count0[*(byte*)start];
                    ++count8[*((byte*)start + 1)];
                    ++count16[*((byte*)start + 2)];
                    ++count24[*((byte*)start + 3)];
                    ++count32[*((byte*)start + 4)];
                    ++count40[*((byte*)start + 5)];
                    ++count48[*((byte*)start + 6)];
                    byte value = *((byte*)start + 7);
                    ++count56[++value];
                }
                int index = *count0;
                for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
                index = *count8;
                for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
                index = *count16;
                for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
                index = *count24;
                for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
                index = *count32;
                for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
                index = *count40;
                for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
                index = *count48;
                for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
                *count56 = 0;
                index = *(count56 + 1);
                for (int* start = count56 + 2, end = count56 + 256; start != end; *start++ = index) index += *start;
                *--count0 = 0;
                for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    swapFixed[count0[*(byte*)start]++] = *start;
                }
                *--count8 = 0;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count8[*((byte*)start + 1)]++] = *start;
                }
                *--count16 = 0;
                for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count16[*((byte*)start + 2)]++] = *start;
                }
                *--count24 = 0;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count24[*((byte*)start + 3)]++] = *start;
                }
                *--count32 = 0;
                for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count32[*((byte*)start + 4)]++] = *start;
                }
                *--count40 = 0;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count40[*((byte*)start + 5)]++] = *start;
                }
                *--count48 = 0;
                for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count48[*((byte*)start + 6)]++] = *start;
                }
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count56[*((byte*)start + 7)]++] = *start;
                }
            }
            finally { countPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        internal static void Sort(ulong[] array, int startIndex, int count)
        {
            UnmanagedPoolPointer swap = UnmanagedPool.GetPoolPointer(count * sizeof(ulong));
            try
            {
                fixed (ulong* arrayFixed = array)
                {
                    ulong* start = arrayFixed + startIndex;
                    sort(start, start, swap.Pointer.ULong, count);
                }
            }
            finally { swap.PushOnly(); }
        }

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sort(long* arrayFixed, long* newArrayFixed, ulong* swapFixed, int length)
        {
            UnmanagedPoolPointer countPointer = UnmanagedPool.RadixSortCountBuffer.GetPoolPointer();
            try
            {
                AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                int* count0 = countPointer.Pointer.Int + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                for (long* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    ++count0[*(byte*)start];
                    ++count8[*((byte*)start + 1)];
                    ++count16[*((byte*)start + 2)];
                    ++count24[*((byte*)start + 3)];
                    ++count32[*((byte*)start + 4)];
                    ++count40[*((byte*)start + 5)];
                    ++count48[*((byte*)start + 6)];
                    byte value = *((byte*)start + 7);
                    value ^= 0x80;
                    ++count56[++value];
                }
                int index = *count0;
                for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
                index = *count8;
                for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
                index = *count16;
                for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
                index = *count24;
                for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
                index = *count32;
                for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
                index = *count40;
                for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
                index = *count48;
                for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
                *count56 = 0;
                index = *(count56 + 1);
                for (int* start = count56 + 2, end = count56 + 256; start != end; *start++ = index) index += *start;
                *--count0 = 0;
                for (long* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    swapFixed[count0[*(byte*)start]++] = (ulong)*start ^ 0x8000000000000000UL;
                    //byte* low = (byte*)(swapFixed + count0[*(byte*)start]++);
                    //*(uint*)low = *(uint*)start;
                    //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
                }
                *--count8 = 0;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count8[*((byte*)start + 1)]++] = (long)*start;
                }
                *--count16 = 0;
                for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count16[*((byte*)start + 2)]++] = (ulong)*start;
                }
                *--count24 = 0;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count24[*((byte*)start + 3)]++] = (long)*start;
                }
                *--count32 = 0;
                for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count32[*((byte*)start + 4)]++] = (ulong)*start;
                }
                *--count40 = 0;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count40[*((byte*)start + 5)]++] = (long)*start;
                }
                *--count48 = 0;
                for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                {
                    swapFixed[count48[*((byte*)start + 6)]++] = (ulong)*start;
                }
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count56[*((byte*)start + 7)]++] = (long)(*start ^ 0x8000000000000000UL);
                    //byte* low = (byte*)(newArrayFixed + count56[*((byte*)start + 7)]++);
                    //*(uint*)low = *(uint*)start;
                    //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
                }
            }
            finally { countPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        internal static void Sort(long[] array, int startIndex, int count)
        {
            UnmanagedPoolPointer swap = UnmanagedPool.GetPoolPointer(count * sizeof(ulong));
            try
            {
                fixed (long* arrayFixed = array)
                {
                    long* start = arrayFixed + startIndex;
                    sort(start, start, swap.Pointer.ULong, count);
                }
            }
            finally { swap.PushOnly(); }
        }
    }
}
