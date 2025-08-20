using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// Member copy code generation configuration (for.NET NativeAOT only)
    /// 成员复制代码生成配置（仅用于 .NET NativeAOT）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class MemberCopyAttribute : Attribute
    {
#if AOT
        /// <summary>
        /// Member copy method name
        /// 成员复制方法名称
        /// </summary>
        internal const string MemberCopyMethodName = "MemberCopy";
        /// <summary>
        /// Member copy method name
        /// 成员复制方法名称
        /// </summary>
        internal const string MemberMapCopyMethodName = "MemberMapCopy";
#endif
    }
}
