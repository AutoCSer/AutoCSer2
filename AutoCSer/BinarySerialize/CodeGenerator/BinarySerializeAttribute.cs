using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 二进制序列化代码生成配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class BinarySerializeAttribute : Attribute
    {
        /// <summary>
        /// 二进制序列化方法名称
        /// </summary>
        internal const string BinarySerializeMethodName = "BinarySerialize";
        /// <summary>
        /// 二进制序列化方法名称
        /// </summary>
        internal const string BinarySerializeMemberMapMethodName = "BinarySerializeMemberMap";
        /// <summary>
        /// 获取二进制序列化成员类型方法名称
        /// </summary>
        internal const string BinarySerializeMemberTypeMethodName = "BinarySerializeMemberTypes";
        /// <summary>
        /// 二进制反数据序列化方法名称
        /// </summary>
        internal const string BinaryDeserializeMethodName = "BinaryDeserialize";
        /// <summary>
        /// 二进制反数据序列化方法名称
        /// </summary>
        internal const string BinaryDeserializeMemberMapMethodName = "BinaryDeserializeMemberMap";

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
