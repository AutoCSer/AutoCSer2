using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// AOT binary serialization code generation configuration
    /// AOT 二进制序列化代码生成配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class BinarySerializeAttribute : Attribute
    {
        /// <summary>
        /// Name of binary serialization method
        /// 二进制序列化方法名称
        /// </summary>
        internal const string BinarySerializeMethodName = "BinarySerialize";
        /// <summary>
        /// Name of binary serialization method
        /// 二进制序列化方法名称
        /// </summary>
        internal const string BinarySerializeMemberMapMethodName = "BinarySerializeMemberMap";
        /// <summary>
        /// The method name of get binary serialized member type collection
        /// 获取二进制序列化成员类型集合方法名称
        /// </summary>
        internal const string BinarySerializeMemberTypeMethodName = "BinarySerializeMemberTypes";
        /// <summary>
        /// Name of binary anti-data serialization method
        /// 二进制反数据序列化方法名称
        /// </summary>
        internal const string BinaryDeserializeMethodName = "BinaryDeserialize";
        /// <summary>
        /// Name of binary anti-data serialization method
        /// 二进制反数据序列化方法名称
        /// </summary>
        internal const string BinaryDeserializeMemberMapMethodName = "BinaryDeserializeMemberMap";

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
