using System;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 警告提示状态
    /// </summary>
    [Flags]
    public enum WarningEnum : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        None = 0,
        /// <summary>
        /// 成员位图类型不匹配
        /// </summary>
        MemberMap = 1,
        /// <summary>
        /// 深度超出范围
        /// </summary>
        DepthOutOfRange = 2,
        /// <summary>
        /// 存在对象循环引用
        /// </summary>
        LoopReference = 4,
        /// <summary>
        /// 在不允许扩展缓存区大小的情况下产生了扩展操作
        /// </summary>
        ResizeError = 8,
    }
}
