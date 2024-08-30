using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search.Memory
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
        /// 默认分词字符类型数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Memory.Pointer GetDefaultCharType()
        {
            return AutoCSerStatic.Slice(0, 1 << 16);
        }
        /// <summary>
        /// 简体字符集
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Memory.Pointer GetSimplified()
        {
            return AutoCSerStatic.Slice(1 << 16, (1 << 16) * sizeof(char));
        }
        static Unmanaged()
        {
            AutoCSerStatic = AutoCSer.Memory.Unmanaged.GetStaticPointer((1 << 16) + ((1 << 16) * sizeof(char)), false);
        }
    }
}
