using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Array element, used for one-time operations on data element
    /// 数组元素，用于一次性操作数据元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ArrayValue<T> where T : class
    {
        /// <summary>
        /// Array element value
        /// 数组元素值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal T Value;
        /// <summary>
        /// Pop out the value of the array element
        /// 弹出数组元素值
        /// </summary>
        /// <returns>Array element value
        /// 数组元素值</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Pop()
        {
            T value = Value;
            Value = null;
            return value;
        }
    }
}
