using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = sizeof(ulong) + sizeof(int))]
    internal partial struct ULongSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal ulong Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(ulong))]
        internal int Index;
        /// <summary>
        /// 设置排序索引
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="index">位置索引</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ulong value, int index)
        {
            Value = value;
            Index = index;
        }

        /// <summary>
        /// 索引排序以后调整数组数据（数据量较大的情况下 new 一个新数组比原地调整数组效率更高）
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal unsafe static void Sort<T>(T[] array, int count, AutoCSer.Algorithm.ULongSortIndex* sortIndex)
        {
            int index = 0, readIndex;
            do
            {
                readIndex = sortIndex[index].Index;
                if (readIndex != index)
                {
                    T value = array[index];
                    int writeIndex = index;
                    do
                    {
                        sortIndex[writeIndex].Index = writeIndex;
                        array[writeIndex] = array[readIndex];
                        writeIndex = readIndex;
                        readIndex = sortIndex[readIndex].Index;
                    }
                    while (readIndex != index);
                    sortIndex[writeIndex].Index = writeIndex;
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
        internal unsafe static void Sort<T>(T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.ULongSortIndex* sortIndex)
        {
            if (startIndex == 0)
            {
                Sort(array, endIndex, sortIndex);
                return;
            }
            int index = startIndex, readIndex;
            do
            {
                readIndex = sortIndex[index - startIndex].Index;
                if (readIndex != index)
                {
                    T value = array[index];
                    int writeIndex = index;
                    do
                    {
                        sortIndex[writeIndex - startIndex].Index = writeIndex;
                        array[writeIndex] = array[readIndex];
                        writeIndex = readIndex;
                        readIndex = sortIndex[readIndex - startIndex].Index;
                    }
                    while (readIndex != index);
                    sortIndex[writeIndex - startIndex].Index = writeIndex;
                    array[writeIndex] = value;
                }
            }
            while (++index != endIndex);
        }
    }
}
