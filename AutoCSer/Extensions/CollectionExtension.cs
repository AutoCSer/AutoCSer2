using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// ICollection泛型转换
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>泛型数据集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ICollection<T> toGeneric<T>(this ICollection value)
        {
            return new ToGenericCollection<T>(value);
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">数据集合</param>
        /// <returns>null为0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int count<T>(this ICollection<T> value)
        {
            return value != null ? value.Count : 0;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <returns>数组</returns>
        public static T[] getArray<T>(this ICollection<T> values)
        {
            if (values.Count == 0) return EmptyArray<T>.Array;
            T[] array = new T[values.Count];
            int index = 0;
            foreach (T value in values) array[index++] = value;
            if (index != array.Length) System.Array.Resize(ref array, index);
            return array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public static LeftArray<VT> getLeftArray<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
        {
            if (values.Count == 0) return new LeftArray<VT>(0);
            LeftArray<VT> array = new LeftArray<VT>(values.Count);
            foreach (T value in values) array.Add(getValue(value));
            return array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static VT[] getArray<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
        {
            return getLeftArray<T, VT>(values, getValue).ToArray();
        }
        /// <summary>
        /// 根据集合内容返回单向动态数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>单向动态数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ListArray<VT> getListArray<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
        {
            return new ListArray<VT>(getLeftArray<T, VT>(values, getValue));
        }

        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>匹配数组</returns>
        public static VT[] getFindArrayNotNull<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
            where VT : class
        {
            if (values.Count != 0)
            {
                int index = 0;
                foreach (T value in values)
                {
                    if (getValue(value) != null) ++index;
                }
                if (index != 0)
                {
                    VT[] newValues = new VT[index];
                    index = 0;
                    foreach (T value in values)
                    {
                        VT arrayValue = getValue(value);
                        if (arrayValue != null) newValues[index++] = arrayValue;
                    }
                    return newValues;
                }
            }
            return EmptyArray<VT>.Array;
        }

        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="VT">枚举值类型</typeparam>
        /// <typeparam name="KT">哈希键值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> getDictionary<VT, KT>(this ICollection<VT> values, Func<VT, KT> getKey)
            where KT : IEquatable<KT>
        {
            Dictionary<KT, VT> dictionary = DictionaryCreator<KT>.Create<VT>(values.Count);
            foreach (VT value in values) dictionary[getKey(value)] = value;
            return dictionary;
        }
    }
}
