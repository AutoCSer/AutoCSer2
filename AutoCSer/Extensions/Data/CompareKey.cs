using System;

namespace AutoCSer.Data
{
    /// <summary>
    /// 排序关键字
    /// </summary>
    /// <typeparam name="T1">关键字类型1</typeparam>
    /// <typeparam name="T2">关键字类型2</typeparam>
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CompareKey<T1, T2> : IComparable<CompareKey<T1, T2>>
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
        public readonly T1 Value1;
        /// <summary>
        /// 关键字2
        /// </summary>
        public readonly T2 Value2;
        /// <summary>
        /// 排序关键字
        /// </summary>
        /// <param name="value1">关键字1</param>
        /// <param name="value2">关键字2</param>
        public CompareKey(T1 value1, T2 value2)
        {
            this.Value1 = value1;
            this.Value2 = value2;
        }
        /// <summary>
        /// 比较大小
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CompareKey<T1, T2> other)
        {
            int value = Value1.CompareTo(other.Value1);
            return value != 0 ? value : Value2.CompareTo(other.Value2);
        }
    }
}
