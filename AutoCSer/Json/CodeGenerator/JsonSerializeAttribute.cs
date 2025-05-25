using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// JSON 序列化代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class JsonSerializeAttribute : Attribute
    {
        /// <summary>
        /// JSON 序列化方法名称
        /// </summary>
        internal const string JsonSerializeMethodName = "JsonSerialize";
        /// <summary>
        /// JSON 序列化方法名称
        /// </summary>
        internal const string JsonSerializeMemberMapMethodName = "JsonSerializeMemberMap";
        /// <summary>
        /// 获取 JSON 序列化成员类型方法名称
        /// </summary>
        internal const string JsonSerializeMemberTypeMethodName = "JsonSerializeMemberTypes";
        /// <summary>
        /// JSON 反序列化方法名称
        /// </summary>
        internal const string JsonDeserializeMethodName = "JsonDeserialize";
        /// <summary>
        /// JSON 反序列化方法名称
        /// </summary>
        internal const string JsonDeserializeMemberMapMethodName = "JsonDeserializeMemberMap";
        /// <summary>
        /// 获取 JSON 反序列化成员名称
        /// </summary>
        internal const string JsonDeserializeMemberNameMethodName = "JsonDeserializeMemberNames";

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
