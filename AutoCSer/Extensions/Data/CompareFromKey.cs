using System;

namespace AutoCSer.Data
{
    /// <summary>
    /// 逆序排序关键字
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CompareFromKey<T> : IComparable<CompareFromKey<T>>
#if NetStandard21
        where T : notnull, IComparable<T>
#else
        where T : IComparable<T>
#endif
    {
        /// <summary>
        /// 关键字
        /// </summary>
        private readonly T value;
        /// <summary>
        /// 逆序排序关键字
        /// </summary>
        /// <param name="value">keyword</param>
        public CompareFromKey(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CompareFromKey<T>(T value) { return new CompareFromKey<T>(value); }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator T(CompareFromKey<T> value) { return value.value; }
        /// <summary>
        /// 比较大小
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CompareFromKey<T> other)
        {
            return other.value.CompareTo(value);
        }
    }
    /// <summary>
    /// 逆序排序关键字
    /// </summary>
    /// <typeparam name="T1">关键字类型1</typeparam>
    /// <typeparam name="T2">关键字类型2</typeparam>
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CompareFromKey<T1, T2> : IComparable<CompareFromKey<T1, T2>>
#if NetStandard21
        where T1 : notnull, IComparable<T1>
        where T2 : notnull, IComparable<T2>
#else
        where T1 : IComparable<T1>
        where T2 : IComparable<T2>
#endif
    {
        /// <summary>
        /// 关键字1
        /// </summary>
        private readonly T1 value1;
        /// <summary>
        /// 关键字2
        /// </summary>
        private readonly T2 value2;
        /// <summary>
        /// 逆序排序关键字
        /// </summary>
        /// <param name="value1">关键字1</param>
        /// <param name="value2">关键字2</param>
        public CompareFromKey(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }
        /// <summary>
        /// 比较大小
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CompareFromKey<T1, T2> other)
        {
            int value = other.value1.CompareTo(value1);
            return value != 0 ? value : other.value2.CompareTo(value2);
        }
    }
}
