using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 项目配置代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationAttribute : Attribute
    {
        /// <summary>
        /// 项目配置方法名称
        /// </summary>
        internal const string ConfigurationMethodName = "__Configuration__";
    }
}
