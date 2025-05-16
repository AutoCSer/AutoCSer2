using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 警告提示状态
    /// </summary>
    [Flags]
    public enum SerializeWarningEnum : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        None,
        /// <summary>
        /// 成员位图类型不匹配
        /// </summary>
        MemberMap = 1,
        /// <summary>
        /// 深度超出范围
        /// </summary>
        DepthOutOfRange = 2,
        /// <summary>
        /// 自定义缓冲序列化返回字节数与写入流字节数不匹配
        /// </summary>
        BufferSize = 4,
        /// <summary>
        /// 在不允许扩展缓存区大小的情况下产生了扩展操作
        /// </summary>
        ResizeError = 8,
    }
}
