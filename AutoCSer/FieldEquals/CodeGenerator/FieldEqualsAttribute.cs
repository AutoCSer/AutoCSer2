using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// Object comparison code generation configuration (for .NET NativeAOT only)
    /// 对象对比代码生成配置（仅用于 .NET NativeAOT）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class FieldEqualsAttribute : Attribute
    {
#if AOT
        /// <summary>
        /// Object comparison method name
        /// 对象对比方法名称
        /// </summary>
        internal const string FieldEqualsMethodName = "FieldEquals";
        /// <summary>
        /// Object comparison method name
        /// 对象对比方法名称
        /// </summary>
        internal const string MemberMapFieldEqualsMethodName = "MemberMapFieldEquals";
#endif
    }
}
