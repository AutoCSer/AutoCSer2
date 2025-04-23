using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 对象对比代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class FieldEqualsAttribute : Attribute
    {
        /// <summary>
        /// 对象对比方法名称
        /// </summary>
        internal const string FieldEqualsMethodName = "FieldEquals";
        /// <summary>
        /// 对象对比方法名称
        /// </summary>
        internal const string MemberMapFieldEqualsMethodName = "MemberMapFieldEquals";
    }
}
