using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 成员复制代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class MemberCopyAttribute : Attribute
    {
        /// <summary>
        /// 成员复制方法名称
        /// </summary>
        internal const string MemberCopyMethodName = "MemberCopy";
        /// <summary>
        /// 成员复制方法名称
        /// </summary>
        internal const string MemberMapCopyMethodName = "MemberMapCopy";
    }
}
