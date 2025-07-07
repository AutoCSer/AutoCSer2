using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// Configuration for generating XML serialization code in the AOT environment
    /// AOT 环境 XML 序列化代码生成配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class XmlSerializeAttribute : Attribute
    {
        /// <summary>
        /// Name of XML serialization method
        /// XML 序列化方法名称
        /// </summary>
        internal const string XmlSerializeMethodName = "XmlSerialize";
        /// <summary>
        /// Name of XML serialization method
        /// XML 序列化方法名称
        /// </summary>
        internal const string XmlSerializeMemberMapMethodName = "XmlSerializeMemberMap";
        /// <summary>
        /// The method name of get XML serialized member type collection
        /// 获取 XML 序列化成员类型集合的方法名称
        /// </summary>
        internal const string XmlSerializeMemberTypeMethodName = "XmlSerializeMemberTypes";
        /// <summary>
        /// Name of XML deserialization method
        /// XML 反序列化方法名称
        /// </summary>
        internal const string XmlDeserializeMethodName = "XmlDeserialize";
        /// <summary>
        /// The method name of get collection of XML deserialization member names
        /// 获取 XML 反序列化成员名称集合的方法名称
        /// </summary>
        internal const string XmlDeserializeMemberNameMethodName = "XmlDeserializeMemberNames";
    }
}
