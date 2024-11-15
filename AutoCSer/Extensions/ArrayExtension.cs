using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static unsafe class ArrayExtension
    {
        /// <summary>
        /// 数组是否为空或者长度为0
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>数组是否为空或者长度为0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool isEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
        ///// <summary>
        ///// 空值转0长度数组
        ///// </summary>
        ///// <typeparam name="T">数据类型</typeparam>
        ///// <param name="array">数组数据</param>
        ///// <returns>非空数组</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static T[] notNull<T>(this T[] array)
        //{
        //    return array != null ? array : EmptyArray<T>.Array;
        //}
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <returns>复制后的新数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] copy<T>(this T[] array)
        {
            if (array.isEmpty()) return EmptyArray<T>.Array;
            return AutoCSer.Common.GetCopyArray(array, array.Length);
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static T[] getArray<T>(this T[][] array)
        {
            if (array.isEmpty()) return EmptyArray<T>.Array;
            if (array.Length != 1) return getConcatArray(array);
            return array[0].copy();
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        private static T[] getConcatArray<T>(T[][] array)
        {
            int length = 0;
            foreach (T[] value in array)
            {
                if (value != null) length += value.Length;
            }
            if (length != 0)
            {
                T[] newValues = new T[length];
                length = 0;
                foreach (T[] value in array)
                {
                    if (value != null)
                    {
                        value.CopyTo(newValues, length);
                        length += value.Length;
                    }
                }
                return newValues;
            }
            return EmptyArray<T>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <param name="addArray">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static T[] concat<T>(this T[] array, T[] addArray)
        {
            if (addArray.Length == 0) return array;
            if (array.Length == 0) return addArray;
            T[] newArray = AutoCSer.Common.GetCopyArray(array, array.Length + addArray.Length);
            addArray.CopyTo(newArray, array.Length);
            return newArray;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] concat<T>(params T[][] array)
        {
            return array.getArray();
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public static LeftArray<T> getFind<T>(this T[] array, Func<T, bool> isValue)
        {
            if (array != null)
            {
                int length = array.Length;
                if (length != 0)
                {
                    T[] newValues = new T[array.Length < sizeof(int) ? sizeof(int) : length];
                    length = 0;
                    foreach (T value in array)
                    {
                        if (isValue(value)) newValues[length++] = value;
                    }
                    return new LeftArray<T>(length, newValues);
                }
            }
            return new LeftArray<T>(0);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] sort<T>(this T[] array, Func<T, T, int> comparer)
        {
            AutoCSer.Algorithm.QuickSort.Sort(array, comparer);
            return array;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string joinString<T>(this T[] array, char join, Func<T, string> toString)
        {
            if (array.Length == 0) return string.Empty;
            return JoinString(array.getArray(toString), join);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合，长度必须大于 0</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        internal static string JoinString(string[] array, char join)
        {
            int length = 0;
            foreach (string nextString in array) length += nextString.Length;
            string value = AutoCSer.Common.AllocateString(length + array.Length - 1);
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                foreach (string nextString in array)
                {
                    if (write != valueFixed) *write++ = join;
                    int size = nextString.Length;
                    if (size != 0)
                    {
                        AutoCSer.Common.CopyTo(nextString, write);
                        write += size;
                    }
                }
            }
            return value;
        }

#if DEBUG
        /// <summary>
        /// 检查数组范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        internal static void debugCheckRange<T>(this T[] array, int startIndex, int count)
        {
            int length = array != null ? array.Length : 0;
            if (startIndex > length)
            {
                throw new IndexOutOfRangeException(startIndex.toString() + " >= " + length.toString());
            }
            if (count < 0) throw new IndexOutOfRangeException(count.toString() + " < 0");
            if (startIndex + count > length) throw new IndexOutOfRangeException(startIndex.toString() + " + " + count.toString() + " > " + length.toString());
        }
#endif
    }
}
