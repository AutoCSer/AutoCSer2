using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Emit 类型创建器扩展
    /// </summary>
    internal unsafe static partial class TypeBuilderExtension
    {
        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Type CreateType(this TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo().AsType();
        }
    }
}
