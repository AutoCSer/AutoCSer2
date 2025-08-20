using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// Random object generation code generation configuration (for .NET NativeAOT only)
    /// 随机对象生成代码生成配置（仅用于 .NET NativeAOT）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class RandomObjectAttribute : Attribute
    {
#if AOT
        /// <summary>
        /// Name of the method for generating random objects
        /// 随机对象生成方法名称
        /// </summary>
        internal const string CreateRandomObjectMethodName = "CreateRandomObject";
#endif
    }
}
