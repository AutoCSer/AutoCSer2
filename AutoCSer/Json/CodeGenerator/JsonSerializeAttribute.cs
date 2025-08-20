using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// JSON serialization code generation configuration (for .NET NativeAOT only)
    /// JSON 序列化代码生成配置（仅用于 .NET NativeAOT）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class JsonSerializeAttribute : Attribute
    {
#if AOT
        /// <summary>
        /// Name of JSON serialization method
        /// JSON 序列化方法名称
        /// </summary>
        internal const string JsonSerializeMethodName = "JsonSerialize";
        /// <summary>
        /// Name of JSON serialization method
        /// JSON 序列化方法名称
        /// </summary>
        internal const string JsonSerializeMemberMapMethodName = "JsonSerializeMemberMap";
        /// <summary>
        /// The method name of get JSON serialized member type collection
        /// 获取 JSON 序列化成员类型集合的方法名称
        /// </summary>
        internal const string JsonSerializeMemberTypeMethodName = "JsonSerializeMemberTypes";
        /// <summary>
        /// Name of JSON deserialization method
        /// JSON 反序列化方法名称
        /// </summary>
        internal const string JsonDeserializeMethodName = "JsonDeserialize";
        /// <summary>
        /// Name of JSON deserialization method
        /// JSON 反序列化方法名称
        /// </summary>
        internal const string JsonDeserializeMemberMapMethodName = "JsonDeserializeMemberMap";
        /// <summary>
        /// The method name of get collection of JSON deserialization member names
        /// 获取 JSON 反序列化成员名称集合的方法名称
        /// </summary>
        internal const string JsonDeserializeMemberNameMethodName = "JsonDeserializeMemberNames";
#endif
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
