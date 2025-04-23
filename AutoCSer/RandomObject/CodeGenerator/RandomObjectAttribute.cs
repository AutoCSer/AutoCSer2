using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 随机对象生成代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class RandomObjectAttribute : Attribute
    {
        /// <summary>
        /// 随机对象生成方法名称
        /// </summary>
        internal const string CreateRandomObjectMethodName = "CreateRandomObject";
    }
}
