using System;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 序列化循环引用检查类型
    /// </summary>
    internal enum PushTypeEnum : byte
    {
        /// <summary>
        /// 仅做层级计数操作
        /// </summary>
        DepthCount,
        /// <summary>
        /// 上级节点为值类型未知节点时添加循环对象检查
        /// </summary>
        UnknownNode,
        /// <summary>
        /// 当前节点为值类型未知节点，仅做层级计数与状态修改操作，不添加循环对象检查
        /// </summary>
        UnknownDepthCount,
        /// <summary>
        /// 添加循环对象检查，包括循环类型对象与未知引用类型对象
        /// </summary>
        Push,

        /// <summary>
        /// 深度超出范围
        /// </summary>
        DepthOutOfRange,
    }
}
