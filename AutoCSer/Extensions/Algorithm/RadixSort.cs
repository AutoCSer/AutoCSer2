using AutoCSer.Memory;
using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 基数排序
    /// </summary>
    public unsafe static class RadixSort
    {
        /// <summary>
        /// 32B 基数排序数据量
        /// </summary>
        internal const int SortSize32 = 80;
        /// <summary>
        /// 64B 基数排序数据量
        /// </summary>
        internal const int SortSize64 = 160;
        /// <summary>
        /// 64B 关键字基数排序数据量
        /// </summary>
        internal const int KeySortSize64 = 128;

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
                uint andValue = uint.MaxValue, orValue = 0;
                int* count0 = countPointer.Pointer.Int + 1;
                AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                uint value;
                for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    value = *start;
                    andValue &= value;
                    orValue |= value;
                    ++count0[(byte)value];
                    ++count8[(byte)(value >> 8)];
                    ++count16[(byte)(value >> 16)];
                    ++count24[(value + 0x1000000U) >> 24];
                }
                if ((andValue ^= orValue) != 0)
                {
                    sum(count0, andValue);
                    count0 = sort(count0 - 1, arrayFixed, newArrayFixed, swapFixed, andValue, length);
                    if(count0 != null)
                    {
                        for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                        {
                            newArrayFixed[count0[*((byte*)start + 3)]++] = *start;
                        }
                    }
                }
                else if (arrayFixed != newArrayFixed)
                {
                    AutoCSer.Memory.Common.Copy(arrayFixed, newArrayFixed, length * sizeof(uint));
                }
            }
            finally { countPointer.PushOnly(); }
        }
        /// <summary>
        /// 累加计数
        /// </summary>
        /// <param name="start">+1</param>
        /// <param name="markValue">高位位 0 表示相同</param>
        private static void sum(int* start, uint markValue)
        {
            int* end = start + 254;//-1
            do
            {
                *(start + 1) += *start;
            }
            while (++start != end);

            if ((markValue & 0xffffff00U) != 0)
            {
                *(start + 1) = 0;
                end += 256;
                start += 2;
                do
                {
                    *(start + 1) += *start;
                }
                while (++start != end);

                if ((markValue & 0xffff0000U) != 0)
                {
                    *(start + 1) = 0;
                    end += 256;
                    start += 2;
                    do
                    {
                        *(start + 1) += *start;
                    }
                    while (++start != end);

                    if ((markValue & 0xff000000U) != 0)
                    {
                        *(start + 1) = 0;
                        end += 256;
                        start += 2;
                        do
                        {
                            *(start + 1) += *start;
                        }
                        while (++start != end);
                    }
                }
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="count0"></param>
        /// <param name="arrayFixed"></param>
        /// <param name="newArrayFixed"></param>
        /// <param name="swapFixed"></param>
        /// <param name="markValue"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int* sort(int* count0, uint* arrayFixed, uint* newArrayFixed, uint* swapFixed, uint markValue, int length)
        {
            for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = *start;
            }
            if ((markValue & 0xffffff00U) != 0)
            {
                count0 += 256;
                for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count0[*((byte*)start + 1)]++] = *start;
                }
                if ((markValue & 0xffff0000U) != 0)
                {
                    count0 += 256;
                    for (uint* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                    {
                        swapFixed[count0[*((byte*)start + 2)]++] = *start;
                    }
                    if ((markValue & 0xff000000U) != 0) return count0 + 256;
                    AutoCSer.Memory.Common.Copy(swapFixed, newArrayFixed, length * sizeof(uint));
                }
            }
            else AutoCSer.Memory.Common.Copy(swapFixed, newArrayFixed, length * sizeof(uint));
            return null;
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
                uint andValue = uint.MaxValue, orValue = 0;
                int* count0 = countPointer.Pointer.Int + 1;
                AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                uint value;
                for (uint* start = (uint*)arrayFixed, end = (uint*)arrayFixed + length; start != end; ++start)
                {
                    value = *start;
                    andValue &= value;
                    orValue |= value;
                    ++count0[(byte)value];
                    ++count8[(byte)(value >> 8)];
                    ++count16[(byte)(value >> 16)];
                    ++count24[((value + 0x1000000U) >> 24) ^ 0x80];
                }
                if ((andValue ^= orValue) != 0)
                {
                    sum(count0, andValue);
                    count0 = sort(count0 - 1, (uint*)arrayFixed, (uint*)newArrayFixed, swapFixed, andValue, length);
                    if (count0 != null)
                    {
                        for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
                        {
                            newArrayFixed[count0[*((byte*)start + 3) ^ 0x80]++] = (int)*start;
                        }
                    }
                }
                else if (arrayFixed != newArrayFixed)
                {
                    AutoCSer.Memory.Common.Copy(arrayFixed, newArrayFixed, length * sizeof(uint));
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
                ulong andValue = ulong.MaxValue, orValue = 0, value;
                int* count0 = countPointer.Pointer.Int + 1;
                AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
                {
                    value = *start;
                    andValue &= value;
                    orValue |= value;
                    ++count0[(byte)value];
                    ++count8[(byte)(value >> 8)];
                    ++count16[(byte)(value >> 16)];
                    ++count24[(byte)(value >> 24)];
                    ++count32[(byte)(value >> 32)];
                    ++count40[(byte)(value >> 40)];
                    ++count48[(byte)(value >> 48)];
                    ++count56[(value + 0x100000000000000UL) >> 56];
                }
                if ((andValue ^= orValue) != 0)
                {
                    sum(count0, andValue);
                    count0 = sort(count0 - 1, arrayFixed, newArrayFixed, swapFixed, andValue, length);
                    if (count0 != null)
                    {
                        for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                        {
                            newArrayFixed[count0[*((byte*)start + 7)]++] = *start;
                        }
                    }
                }
                else if (arrayFixed != newArrayFixed)
                {
                    AutoCSer.Memory.Common.Copy(arrayFixed, newArrayFixed, length * sizeof(ulong));
                }
            }
            finally { countPointer.PushOnly(); }
        }
        /// <summary>
        /// 累加计数
        /// </summary>
        /// <param name="start">+1</param>
        /// <param name="markValue"></param>
        private static void sum(int* start, ulong markValue)
        {
            if ((markValue & 0xffffffff00000000UL) != 0)
            {
                sum(start, uint.MaxValue);
                start += 256 * 4 - 1;
                *start = 0;
                sum(start + 1, (uint)(markValue >> 32));
            }
            else sum(start, (uint)markValue);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="count0"></param>
        /// <param name="arrayFixed"></param>
        /// <param name="newArrayFixed"></param>
        /// <param name="swapFixed"></param>
        /// <param name="markValue"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int* sort(int* count0, ulong* arrayFixed, ulong* newArrayFixed, ulong* swapFixed, ulong markValue, int length)
        {
            for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = *start;
            }
            if ((markValue & 0xffffffffffffff00U) != 0)
            {
                count0 += 256;
                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                {
                    newArrayFixed[count0[*((byte*)start + 1)]++] = *start;
                }
                if ((markValue & 0xffffffffffff0000U) != 0)
                {
                    count0 += 256;
                    for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                    {
                        swapFixed[count0[*((byte*)start + 2)]++] = *start;
                    }
                    if ((markValue & 0xffffffffff000000U) != 0)
                    {
                        count0 += 256;
                        for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                        {
                            newArrayFixed[count0[*((byte*)start + 3)]++] = *start;
                        }
                        if ((markValue & 0xffffffff00000000U) != 0)
                        {
                            count0 += 256;
                            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                            {
                                swapFixed[count0[*((byte*)start + 4)]++] = *start;
                            }
                            if ((markValue & 0xffffff0000000000U) != 0)
                            {
                                count0 += 256;
                                for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                                {
                                    newArrayFixed[count0[*((byte*)start + 5)]++] = *start;
                                }
                                if ((markValue & 0xffff000000000000U) != 0)
                                {
                                    count0 += 256;
                                    for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
                                    {
                                        swapFixed[count0[*((byte*)start + 6)]++] = *start;
                                    }
                                    if ((markValue & 0xff00000000000000U) != 0) return count0 + 256;
                                    AutoCSer.Memory.Common.Copy(swapFixed, newArrayFixed, length * sizeof(ulong));
                                }
                            }
                            else AutoCSer.Memory.Common.Copy(swapFixed, newArrayFixed, length * sizeof(ulong));
                        }
                    }
                    else AutoCSer.Memory.Common.Copy(swapFixed, newArrayFixed, length * sizeof(ulong));
                }
            }
            else AutoCSer.Memory.Common.Copy(swapFixed, newArrayFixed, length * sizeof(ulong));
            return null;
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
                ulong andValue = ulong.MaxValue, orValue = 0, value;
                int* count0 = countPointer.Pointer.Int + 1;
                AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                for (ulong* start = (ulong*)arrayFixed, end = (ulong*)arrayFixed + length; start != end; ++start)
                {
                    value = *start;
                    andValue &= value;
                    orValue |= value;
                    ++count0[(byte)value];
                    ++count8[(byte)(value >> 8)];
                    ++count16[(byte)(value >> 16)];
                    ++count24[(byte)(value >> 24)];
                    ++count32[(byte)(value >> 32)];
                    ++count40[(byte)(value >> 40)];
                    ++count48[(byte)(value >> 48)];
                    ++count56[((value + 0x100000000000000UL) >> 56) ^ 0x80];
                }
                if ((andValue ^= orValue) != 0)
                {
                    sum(count0, andValue);
                    count0 = sort(count0 - 1, (ulong*)arrayFixed, (ulong*)newArrayFixed, swapFixed, andValue, length);
                    if (count0 != null)
                    {
                        for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
                        {
                            newArrayFixed[count0[*((byte*)start + 7) ^ 0x80]++] = (long)*start;
                        }
                    }
                }
                else if (arrayFixed != newArrayFixed)
                {
                    AutoCSer.Memory.Common.Copy(arrayFixed, newArrayFixed, length * sizeof(ulong));
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

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void Sort<T>(T[] array, int startIndex, int count, Func<T, uint> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(UIntSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.Default.GetPoolPointer();
                UIntSortIndex* sortIndex = (UIntSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    int* count0 = countPointer.Pointer.Int + 1;
                    UIntSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                    int * count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                    int index = 0;
                    uint andValue = uint.MaxValue, orValue = 0, value;
                    do
                    {
                        (*swapSortIndex).Set(value = getKey(array[index + startIndex]), index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(value + 0x1000000U) >> 24];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void SortDesc<T>(T[] array, int startIndex, int count, Func<T, uint> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(UIntSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.Default.GetPoolPointer();
                UIntSortIndex* sortIndex = (UIntSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    int* count0 = countPointer.Pointer.Int + 1;
                    UIntSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                    int index = 0;
                    uint andValue = uint.MaxValue, orValue = 0, value;
                    do
                    {
                        (*swapSortIndex).Set(value = getKey(array[index + startIndex]) ^ uint.MaxValue, index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(value + 0x1000000U) >> 24];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortIndex"></param>
        /// <param name="count0"></param>
        /// <param name="markValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static UIntSortIndex* sort(UIntSortIndex* sortIndex, int* count0, uint markValue, int count)
        {
            UIntSortIndex* swapSortIndex = sortIndex + count, writeSortIndex;
            sum(count0, markValue);
            int* returnIndex;
            if ((markValue & 0xffffff00U) != 0)
            {
                for (writeSortIndex = sortIndex, --count0; writeSortIndex != swapSortIndex; ++writeSortIndex)
                {
                    swapSortIndex[count0[*(byte*)writeSortIndex]++] = *writeSortIndex;
                }
                UIntSortIndex* endSortIndex = swapSortIndex + count;
                if ((markValue & 0xffff0000U) != 0)
                {
                    for (writeSortIndex = swapSortIndex, count0 += 256; writeSortIndex != endSortIndex; ++writeSortIndex)
                    {
                        sortIndex[count0[*((byte*)writeSortIndex + 1)]++] = *writeSortIndex;
                    }
                    if ((markValue & 0xff000000U) != 0)
                    {
                        for (writeSortIndex = sortIndex, count0 += 256; writeSortIndex != swapSortIndex; ++writeSortIndex)
                        {
                            swapSortIndex[count0[*((byte*)writeSortIndex + 2)]++] = *writeSortIndex;
                        }
                        for (writeSortIndex = swapSortIndex, count0 += 256, returnIndex = (int*)sortIndex; writeSortIndex != endSortIndex; ++writeSortIndex)
                        {
                            returnIndex[count0[*((byte*)writeSortIndex + 3)]++] = (*writeSortIndex).Index;
                        }
                    }
                    else
                    {
                        for (writeSortIndex = sortIndex, count0 += 256, returnIndex = (int*)swapSortIndex; writeSortIndex != swapSortIndex; ++writeSortIndex)
                        {
                            returnIndex[count0[*((byte*)writeSortIndex + 2)]++] = (*writeSortIndex).Index;
                        }
                    }
                }
                else
                {
                    for (writeSortIndex = swapSortIndex, count0 += 256, returnIndex = (int*)sortIndex; writeSortIndex != endSortIndex; ++writeSortIndex)
                    {
                        returnIndex[count0[*((byte*)writeSortIndex + 1)]++] = (*writeSortIndex).Index;
                    }
                }
            }
            else
            {
                for (writeSortIndex = sortIndex, --count0, returnIndex = (int*)swapSortIndex; writeSortIndex != swapSortIndex; ++writeSortIndex)
                {
                    returnIndex[count0[*(byte*)writeSortIndex]++] = (*writeSortIndex).Index;
                }
            }
            return (UIntSortIndex*)returnIndex;
        }
        /// <summary>
        /// 索引排序以后调整数组数据（数据量较大的情况下 new 一个新数组比原地调整数组效率更高）
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        private unsafe static void sort<T>(T[] array, int count, int* sortIndex)
        {
            int index = 0, readIndex;
            do
            {
                readIndex = sortIndex[index];
                if (readIndex != index)
                {
                    T value = array[index];
                    int writeIndex = index;
                    do
                    {
                        sortIndex[writeIndex] = writeIndex;
                        array[writeIndex] = array[readIndex];
                        writeIndex = readIndex;
                        readIndex = sortIndex[readIndex];
                    }
                    while (readIndex != index);
                    sortIndex[writeIndex] = writeIndex;
                    array[writeIndex] = value;
                }
            }
            while (++index != count);
        }
        /// <summary>
        /// 索引排序以后调整数组数据（数据量较大的情况下 new 一个新数组比原地调整数组效率更高）
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        private unsafe static void sort<T>(T[] array, int startIndex, int endIndex, int* sortIndex)
        {
            if (startIndex == 0)
            {
                sort(array, endIndex, sortIndex);
                return;
            }
            int index = startIndex, readIndex;
            do
            {
                readIndex = sortIndex[index - startIndex];
                if (readIndex != index)
                {
                    T value = array[index];
                    int writeIndex = index;
                    do
                    {
                        sortIndex[writeIndex - startIndex] = writeIndex;
                        array[writeIndex] = array[readIndex];
                        writeIndex = readIndex;
                        readIndex = sortIndex[readIndex - startIndex];
                    }
                    while (readIndex != index);
                    sortIndex[writeIndex - startIndex] = writeIndex;
                    array[writeIndex] = value;
                }
            }
            while (++index != endIndex);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void Sort<T>(T[] array, int startIndex, int count, Func<T, int> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(UIntSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.Default.GetPoolPointer();
                UIntSortIndex* sortIndex = (UIntSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    int* count0 = countPointer.Pointer.Int + 1;
                    UIntSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                    int index = 0;
                    uint andValue = uint.MaxValue, orValue = 0, value;
                    do
                    {
                        (*swapSortIndex).Set(value = (uint)getKey(array[index + startIndex]) ^ 0x80000000U, index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(value + 0x1000000U) >> 24];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void SortDesc<T>(T[] array, int startIndex, int count, Func<T, int> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(UIntSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.Default.GetPoolPointer();
                UIntSortIndex* sortIndex = (UIntSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    int* count0 = countPointer.Pointer.Int + 1;
                    UIntSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.DefaultSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
                    int index = 0;
                    uint andValue = uint.MaxValue, orValue = 0, value;
                    do
                    {
                        (*swapSortIndex).Set(value = (uint)getKey(array[index + startIndex]) ^ int.MaxValue, index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(value + 0x1000000U) >> 24];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void Sort<T>(T[] array, int startIndex, int count, Func<T, ulong> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(ULongSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.RadixSortCountBuffer.GetPoolPointer();
                ULongSortIndex* sortIndex = (ULongSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    ulong andValue = uint.MaxValue, orValue = 0, value;
                    int* count0 = countPointer.Pointer.Int + 1;
                    ULongSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                    int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                    int index = 0;
                    do
                    {
                        (*swapSortIndex).Set(value = getKey(array[index + startIndex]), index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(byte)(value >> 24)];
                        ++count32[(byte)(value >> 32)];
                        ++count40[(byte)(value >> 40)];
                        ++count48[(byte)(value >> 48)];
                        ++count56[(value + 0x100000000000000UL) >> 56];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void SortDesc<T>(T[] array, int startIndex, int count, Func<T, ulong> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(ULongSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.RadixSortCountBuffer.GetPoolPointer();
                ULongSortIndex* sortIndex = (ULongSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    ulong andValue = uint.MaxValue, orValue = 0, value;
                    int* count0 = countPointer.Pointer.Int + 1;
                    ULongSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                    int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                    int index = 0;
                    do
                    {
                        (*swapSortIndex).Set(value = getKey(array[index + startIndex]) ^ ulong.MaxValue, index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(byte)(value >> 24)];
                        ++count32[(byte)(value >> 32)];
                        ++count40[(byte)(value >> 40)];
                        ++count48[(byte)(value >> 48)];
                        ++count56[(value + 0x100000000000000UL) >> 56];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortIndex"></param>
        /// <param name="count0"></param>
        /// <param name="markValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static ULongSortIndex* sort(ULongSortIndex* sortIndex, int* count0, ulong markValue, int count)
        {
            ULongSortIndex* swapSortIndex = sortIndex + count, writeSortIndex;
            sum(count0, markValue);
            int* returnIndex;
            if ((markValue & 0xffffffffffffff00U) != 0)
            {
                for (writeSortIndex = sortIndex, --count0; writeSortIndex != swapSortIndex; ++writeSortIndex)
                {
                    swapSortIndex[count0[*(byte*)writeSortIndex]++] = *writeSortIndex;
                }
                ULongSortIndex* endSortIndex = swapSortIndex + count;
                if ((markValue & 0xffffffffffff0000U) != 0)
                {
                    for (writeSortIndex = swapSortIndex, count0 += 256; writeSortIndex != endSortIndex; ++writeSortIndex)
                    {
                        sortIndex[count0[*((byte*)writeSortIndex + 1)]++] = *writeSortIndex;
                    }
                    if ((markValue & 0xffffffffff000000U) != 0)
                    {
                        for (writeSortIndex = sortIndex, count0 += 256; writeSortIndex != swapSortIndex; ++writeSortIndex)
                        {
                            swapSortIndex[count0[*((byte*)writeSortIndex + 2)]++] = *writeSortIndex;
                        }
                        if ((markValue & 0xffffffff00000000U) != 0)
                        {
                            for (writeSortIndex = swapSortIndex, count0 += 256; writeSortIndex != endSortIndex; ++writeSortIndex)
                            {
                                sortIndex[count0[*((byte*)writeSortIndex + 3)]++] = *writeSortIndex;
                            }
                            if ((markValue & 0xffffff0000000000U) != 0)
                            {
                                for (writeSortIndex = sortIndex, count0 += 256; writeSortIndex != swapSortIndex; ++writeSortIndex)
                                {
                                    swapSortIndex[count0[*((byte*)writeSortIndex + 4)]++] = *writeSortIndex;
                                }
                                if ((markValue & 0xffff000000000000U) != 0)
                                {
                                    for (writeSortIndex = swapSortIndex, count0 += 256; writeSortIndex != endSortIndex; ++writeSortIndex)
                                    {
                                        sortIndex[count0[*((byte*)writeSortIndex + 5)]++] = *writeSortIndex;
                                    }
                                    if ((markValue & 0xff00000000000000U) != 0)
                                    {
                                        for (writeSortIndex = sortIndex, count0 += 256; writeSortIndex != swapSortIndex; ++writeSortIndex)
                                        {
                                            swapSortIndex[count0[*((byte*)writeSortIndex + 6)]++] = *writeSortIndex;
                                        }
                                        for (writeSortIndex = swapSortIndex, count0 += 256, returnIndex = (int*)sortIndex; writeSortIndex != endSortIndex; ++writeSortIndex)
                                        {
                                            returnIndex[count0[*((byte*)writeSortIndex + 7)]++] = (*writeSortIndex).Index;
                                        }
                                    }
                                    else
                                    {
                                        for (writeSortIndex = sortIndex, count0 += 256, returnIndex = (int*)swapSortIndex; writeSortIndex != swapSortIndex; ++writeSortIndex)
                                        {
                                            returnIndex[count0[*((byte*)writeSortIndex + 6)]++] = (*writeSortIndex).Index;
                                        }
                                    }
                                }
                                else
                                {
                                    for (writeSortIndex = swapSortIndex, count0 += 256, returnIndex = (int*)sortIndex; writeSortIndex != endSortIndex; ++writeSortIndex)
                                    {
                                        returnIndex[count0[*((byte*)writeSortIndex + 5)]++] = (*writeSortIndex).Index;
                                    }
                                }
                            }
                            else
                            {
                                for (writeSortIndex = sortIndex, count0 += 256, returnIndex = (int*)swapSortIndex; writeSortIndex != swapSortIndex; ++writeSortIndex)
                                {
                                    returnIndex[count0[*((byte*)writeSortIndex + 4)]++] = (*writeSortIndex).Index;
                                }
                            }
                        }
                        else
                        {
                            for (writeSortIndex = swapSortIndex, count0 += 256, returnIndex = (int*)sortIndex; writeSortIndex != endSortIndex; ++writeSortIndex)
                            {
                                returnIndex[count0[*((byte*)writeSortIndex + 3)]++] = (*writeSortIndex).Index;
                            }
                        }
                    }
                    else
                    {
                        for (writeSortIndex = sortIndex, count0 += 256, returnIndex = (int*)swapSortIndex; writeSortIndex != swapSortIndex; ++writeSortIndex)
                        {
                            returnIndex[count0[*((byte*)writeSortIndex + 2)]++] = (*writeSortIndex).Index;
                        }
                    }
                }
                else
                {
                    for (writeSortIndex = swapSortIndex, count0 += 256, returnIndex = (int*)sortIndex; writeSortIndex != endSortIndex; ++writeSortIndex)
                    {
                        returnIndex[count0[*((byte*)writeSortIndex + 1)]++] = (*writeSortIndex).Index;
                    }
                }
            }
            else
            {
                for (writeSortIndex = sortIndex, --count0, returnIndex = (int*)swapSortIndex; writeSortIndex != swapSortIndex; ++writeSortIndex)
                {
                    returnIndex[count0[*(byte*)writeSortIndex]++] = (*writeSortIndex).Index;
                }
            }
            return (ULongSortIndex*)returnIndex;
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void Sort<T>(T[] array, int startIndex, int count, Func<T, long> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(ULongSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.RadixSortCountBuffer.GetPoolPointer();
                ULongSortIndex* sortIndex = (ULongSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    ulong andValue = uint.MaxValue, orValue = 0, value;
                    int* count0 = countPointer.Pointer.Int + 1;
                    ULongSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                    int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                    int index = 0;
                    do
                    {
                        (*swapSortIndex).Set(value = (ulong)getKey(array[index + startIndex]) ^ 0x8000000000000000UL, index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(byte)(value >> 24)];
                        ++count32[(byte)(value >> 32)];
                        ++count40[(byte)(value >> 40)];
                        ++count48[(byte)(value >> 48)];
                        ++count56[(value + 0x100000000000000UL) >> 56];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="getKey"></param>
        internal static void SortDesc<T>(T[] array, int startIndex, int count, Func<T, long> getKey)
        {
            UnmanagedPoolPointer indexPointer = UnmanagedPool.GetPoolPointer(count * (sizeof(ULongSortIndex) * 2));
            try
            {
                UnmanagedPoolPointer countPointer = UnmanagedPool.RadixSortCountBuffer.GetPoolPointer();
                ULongSortIndex* sortIndex = (ULongSortIndex*)indexPointer.Pointer.Data;
                try
                {
                    ulong andValue = uint.MaxValue, orValue = 0, value;
                    int* count0 = countPointer.Pointer.Int + 1;
                    ULongSortIndex* swapSortIndex = sortIndex;
                    AutoCSer.Memory.Common.Clear(countPointer.Pointer.ULong, UnmanagedPool.RadixSortCountBufferSize >> 3);
                    int* count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
                    int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
                    int index = 0;
                    do
                    {
                        (*swapSortIndex).Set(value = (ulong)getKey(array[index + startIndex]) ^ long.MaxValue, index);
                        andValue &= value;
                        orValue |= value;
                        ++count0[(byte)value];
                        ++count8[(byte)(value >> 8)];
                        ++count16[(byte)(value >> 16)];
                        ++count24[(byte)(value >> 24)];
                        ++count32[(byte)(value >> 32)];
                        ++count40[(byte)(value >> 40)];
                        ++count48[(byte)(value >> 48)];
                        ++count56[(value + 0x100000000000000UL) >> 56];
                        ++swapSortIndex;
                    }
                    while (++index != count);
                    if ((andValue ^= orValue) != 0) sortIndex = sort(sortIndex, count0, andValue, count);
                    else return;
                }
                finally { countPointer.PushOnly(); }
                sort(array, startIndex, startIndex + count, (int*)sortIndex);
            }
            finally { indexPointer.PushOnly(); }
        }
    }
}
