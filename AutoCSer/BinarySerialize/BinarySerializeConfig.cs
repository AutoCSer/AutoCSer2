using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 序列化配置参数
    /// </summary>
    public sealed class BinarySerializeConfig
    {
        /// <summary>
        /// 默认最大节点检测深度
        /// </summary>
        public const int DefaultCheckDepth = 64;
        /// <summary>
        /// 序列化头部数据
        /// </summary>
        internal const uint HeaderMapValue = 0x51031000U;
        /// <summary>
        /// 序列化头部数据
        /// </summary>
        internal const uint HeaderMapAndValue = 0xffffff00U;
        /// <summary>
        /// 是否序列化成员位图
        /// </summary>
        internal const int MemberMapValue = 1;
        ///// <summary>
        ///// 是否检测全局版本编号
        ///// </summary>
        //internal const int GlobalVersionValue = 2;
        /// <summary>
        /// 是否存在对象引用
        /// </summary>
        internal const int ObjectReference = 4;

        /// <summary>
        /// 成员位图（仅支持定义字段，不支持匿名字段，也就是不支持指定属性）
        /// </summary>
#if NetStandard21
        public MemberMap? MemberMap;
#else
        public MemberMap MemberMap;
#endif
        /// <summary>
        /// 是否检查相同对象引用，同一个对象序列化类型将会当成不同的对象引用处理，默认为 true
        /// </summary>
        public bool CheckReference = true;
        /// <summary>
        /// 不支持类型是否尝试转换为真实类型处理默认为 false，设置为 true 需要反序列化端配置允许该远程类型，否则将导致反序列化失败
        /// </summary>
        public bool IsRealType;
        /// <summary>
        /// 最大节点检测深度，默认为 64（过大的深度会造成堆栈溢出，所以该序列化组件不适合序列化链表结构，如果存在该类似需求请自定义序列化转换为数组处理）
        /// </summary>
        public int CheckDepth = DefaultCheckDepth;
        ///// <summary>
        ///// 全局版本编号
        ///// </summary>
        //public uint GlobalVersion;
        /// <summary>
        /// 序列化输出缓冲区预留字节数，默认为 0 表示不预留
        /// </summary>
        public int StreamSeek;
        /// <summary>
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
        /// 写入序列化头部数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal unsafe int WriteHeader(UnmanagedStream stream)
        {
            int headerValue = (int)HeaderMapValue;
            if (MemberMap != null) headerValue |= MemberMapValue;
            int streamStartIndex = stream.GetIndexBeforeMove(sizeof(int));
            *(int*)((byte*)stream.Data.Pointer.Data + streamStartIndex) = headerValue;
            return streamStartIndex;
        }
    }
}
