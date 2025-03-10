using System;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据类型
    /// </summary>
    internal enum IndexDataTypeEnum : byte
    {
        /// <summary>
        /// 仅创建，还没有获取索引数据磁盘块索引信息节点
        /// </summary>
        None,
        /// <summary>
        /// 未加载
        /// </summary>
        NotLoaded,
        /// <summary>
        /// 索引数据磁盘块索引数据
        /// </summary>
        BlockIndexData,
        /// <summary>
        /// 少量数据（最大数据量 1021）
        /// </summary>
        Little,
        /// <summary>
        /// 大量数据
        /// </summary>
        Many,
    }
}
