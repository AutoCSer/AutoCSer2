using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// XML 序列化代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class XmlSerializeAttribute : Attribute
    {
        /// <summary>
        /// XML 序列化方法名称
        /// </summary>
        internal const string XmlSerializeMethodName = "XmlSerialize";
        /// <summary>
        /// XML 序列化方法名称
        /// </summary>
        internal const string XmlSerializeMemberMapMethodName = "XmlSerializeMemberMap";
        /// <summary>
        /// 获取 XML 序列化成员类型方法名称
        /// </summary>
        internal const string XmlSerializeMemberTypeMethodName = "XmlSerializeMemberTypes";
        /// <summary>
        /// XML 反序列化方法名称
        /// </summary>
        internal const string XmlDeserializeMethodName = "XmlDeserialize";
        /// <summary>
        /// 获取 XML 反序列化成员名称
        /// </summary>
        internal const string XmlDeserializeMemberNameMethodName = "XmlDeserializeMemberNames";
    }
}
