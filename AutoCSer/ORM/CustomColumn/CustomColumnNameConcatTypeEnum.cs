using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 自定义数据列名称连接类型
    /// </summary>
    public enum CustomColumnNameConcatTypeEnum : byte
    {
        /// <summary>
        /// 分隔符连接
        /// </summary>
        Concat,
        /// <summary>
        /// 仅父节点名称，仅适应于一个子节点
        /// </summary>
        Parent,
        /// <summary>
        /// 仅使用子节点名称
        /// </summary>
        Node,
    }
}
