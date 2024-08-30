using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 方法参数相关操作
    /// </summary>
    internal static partial class ParameterInfoExtension
    {
        /// <summary>
        /// 获取参数真实类型
        /// </summary>
        /// <param name="parameter">参数信息</param>
        /// <returns>参数真实类型</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Type elementType(this ParameterInfo parameter)
        {
            Type type = parameter.ParameterType;
            return type.IsByRef ? type.GetElementType() : type;
        }
    }
}
