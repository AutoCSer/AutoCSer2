using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Serialization configuration parameters
    /// 序列化配置参数
    /// </summary>
    public sealed class BinarySerializeConfig
    {
        /// <summary>
        /// The default maximum node detection depth value is 64
        /// 默认最大节点检测深度值为 64
        /// </summary>
        public const int DefaultCheckDepth = 64;
        /// <summary>
        /// Serialize the header data
        /// 序列化头部数据
        /// </summary>
        internal const uint HeaderMapValue = 0x51031000U;
        /// <summary>
        /// Serialize the binary bits of the header data
        /// 序列化头部数据二进制位
        /// </summary>
        internal const uint HeaderMapAndValue = 0xffffff00U;
        /// <summary>
        /// Is serialize the member bitmap
        /// 是否序列化成员位图
        /// </summary>
        internal const int MemberMapValue = 1;
        ///// <summary>
        ///// 是否检测全局版本编号
        ///// </summary>
        //internal const int GlobalVersionValue = 2;
        /// <summary>
        /// Whether there is an object reference
        /// 是否存在对象引用
        /// </summary>
        internal const int ObjectReference = 4;

        /// <summary>
        /// Member bitmap (only supports defined fields and anonymous fields for automatically implemented attributes)
        /// 成员位图（仅支持定义字段与自动实现属性的匿名字段）
        /// </summary>
#if NetStandard21
        public MemberMap? MemberMap;
#else
        public MemberMap MemberMap;
#endif
        /// <summary>
        /// The default is true, indicating that the same object reference is checked
        /// 默认为 true 表示检查相同对象引用
        /// </summary>
        public bool CheckReference = true;
        /// <summary>
        /// By default, false indicates that the type is not supported and no attempt is made to convert it to a real type for processing. Setting it to true requires the deserialization end configuration to allow this remote type; otherwise, deserialization will fail
        /// 默认为 false 表示不支持类型不尝试转换为真实类型处理，设置为 true 需要反序列化端配置允许该远程类型，否则将导致反序列化失败
        /// </summary>
        public bool IsRealType;
        /// <summary>
        /// The default value of the maximum node detection depth is 64. (Excessive depth can cause stack overflow, so this serialization component is not suitable for serializing linked list structures. If there is a similar requirement, please customize the serialization conversion to an array for processing.)
        /// 最大节点检测深度值默认为 64（过大的深度会造成堆栈溢出，所以该序列化组件不适合序列化链表结构，如果存在该类似需求请自定义序列化转换为数组处理）
        /// </summary>
        public int CheckDepth = DefaultCheckDepth;
        ///// <summary>
        ///// 全局版本编号
        ///// </summary>
        //public uint GlobalVersion;
        /// <summary>
        /// The number of bytes reserved in the serialized output buffer is 0 by default, indicating that no bytes are reserved
        /// 序列化输出缓冲区预留字节数，默认为 0 表示不预留
        /// </summary>
        public int StreamSeek;
        /// <summary>
        /// Copy the serialization configuration parameters
        /// 复制序列化配置参数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BinarySerializeConfig Clone()
        {
            return (BinarySerializeConfig)MemberwiseClone();
        }

        ///// <summary>
        ///// 写入序列化头部数据
        ///// </summary>
        ///// <param name="stream"></param>
        ///// <param name="notReferenceCount"></param>
        ///// <returns></returns>
        //internal unsafe int WriteHeader(UnmanagedStream stream, bool notReferenceCount)
        //{
        //    int headerValue = (int)HeaderMapValue;
        //    if (MemberMap != null) headerValue |= MemberMapValue;
        //    if (!notReferenceCount) headerValue |= ObjectReference;
        //    stream.Write(headerValue);
        //}
        /// <summary>
        /// Write the serialized header data
        /// 写入序列化头部数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe int WriteHeader(UnmanagedStream stream)
        {
            return stream.GetIndexBeforeMove(sizeof(int), MemberMap == null ? (int)HeaderMapValue : ((int)HeaderMapValue | MemberMapValue));
        }
    }
}
