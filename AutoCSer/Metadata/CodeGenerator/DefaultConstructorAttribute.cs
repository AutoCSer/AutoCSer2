using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 默认构造函数代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DefaultConstructorAttribute : Attribute
    {
        /// <summary>
        /// 默认构造函数方法名称
        /// </summary>
        internal const string DefaultConstructorMethodName = "DefaultConstructor";
    }
}
