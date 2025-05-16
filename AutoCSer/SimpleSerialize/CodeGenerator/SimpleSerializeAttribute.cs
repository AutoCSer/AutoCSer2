using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 简单序列化代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    internal sealed class SimpleSerializeAttribute : Attribute
    {
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        internal const string SimpleSerializeMethodName = "SimpleSerialize";
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        internal const string SimpleDeserializeMethodName = "SimpleDeserialize";

        /// <summary>
        /// 默认为 true 表示生成序列化代码
        /// </summary>
        public bool IsSerialize = true;
        /// <summary>
        /// 默认为 true 表示生成反序列化代码
        /// </summary>
        public bool IsDeserialize = true;
    }
}
