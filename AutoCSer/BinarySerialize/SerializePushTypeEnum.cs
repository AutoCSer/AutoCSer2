using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化循环引用检查类型
    /// </summary>
    internal enum SerializePushTypeEnum : byte
    {
        /// <summary>
        /// 内部类型
        /// </summary>
        Primitive,
        /// <summary>
        /// 仅做层级计数操作
        /// </summary>
        DepthCount,
        /// <summary>
        /// 增加非引用计数
        /// </summary>
        NotReferenceCount,
        /// <summary>
        /// 尝试添加引用
        /// </summary>
        TryReference,

        /// <summary>
        /// 深度超出范围
        /// </summary>
        DepthOutOfRange,
    }
}
