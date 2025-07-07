using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// AOT Simple serialization code generation configuration
    /// AOT 简单序列化代码生成配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    internal sealed class SimpleSerializeAttribute : Attribute
    {
        /// <summary>
        /// Name of serialization method
        /// 序列化方法名称
        /// </summary>
        internal const string SimpleSerializeMethodName = "SimpleSerialize";
        /// <summary>
        /// Name of deserialization method
        /// 反序列化方法名称
        /// </summary>
        internal const string SimpleDeserializeMethodName = "SimpleDeserialize";

        /// <summary>
        /// The default is true, indicating the generation of serialization code
        /// 默认为 true 表示生成序列化代码
        /// </summary>
        public bool IsSerialize = true;
        /// <summary>
        /// The default is true, indicating the generation of deserialization code
        /// 默认为 true 表示生成反序列化代码
        /// </summary>
        public bool IsDeserialize = true;
    }
}
