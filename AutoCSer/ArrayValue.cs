using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组元素，用于一次性操作数据元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ArrayValue<T> where T : class
    {
        /// <summary>
        /// 数组元素
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal T Value;
        /// <summary>
        /// 弹出数组元素
        /// </summary>
        /// <returns>数组元素</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Pop()
        {
            T value = Value;
            Value = null;
            return value;
        }
    }
}
