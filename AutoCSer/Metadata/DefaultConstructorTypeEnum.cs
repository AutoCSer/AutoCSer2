using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 默认构造函数类型
    /// </summary>
    internal enum DefaultConstructorTypeEnum : byte
    {
        /// <summary>
        /// 没有构造函数
        /// </summary>
        None,
        /// <summary>
        /// 无参构造函数
        /// </summary>
        Constructor,
        /// <summary>
        /// 自定义构造函数
        /// </summary>
        Custom,
        /// <summary>
        /// 值类型返回默认值
        /// </summary>
        Default,
        /// <summary>
        /// 未初始化对象浅克隆，仅用户数据反序列化
        /// </summary>
        UninitializedObjectClone,
    }
}
