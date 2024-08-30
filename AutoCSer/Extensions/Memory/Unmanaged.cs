using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Memory
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    internal unsafe static class Unmanaged
    {
        /// <summary>
        /// AutoCSer 使用静态内存段，防止碎片化
        /// </summary>
        private static AutoCSer.Memory.Pointer AutoCSerStatic;
        /// <summary>
        /// XML 字符状态位
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Memory.Pointer GetXmlBits()
        {
            return AutoCSerStatic.Slice(0, 256);
        }
        /// <summary>
        /// HTML 字符状态位
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Memory.Pointer GetHtmlBits()
        {
            return AutoCSerStatic.Slice(256, 256);
        }
        /// <summary>
        /// 原始表达式字符状态位
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Memory.Pointer GetRawExpressionBits()
        {
            return AutoCSerStatic.Slice(256 + 256, 256);
        }
        static Unmanaged()
        {
            AutoCSerStatic = AutoCSer.Memory.Unmanaged.GetStaticPointer(256 + 256 + 256, false);
        }
    }
}
