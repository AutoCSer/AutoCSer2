using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 项目配置代码生成配置（仅用于 .NET NativeAOT）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationAttribute : Attribute
    {
#if AOT
        /// <summary>
        /// 项目配置方法名称
        /// </summary>
        internal const string ConfigurationMethodName = "__Configuration__";
#endif
    }
}
