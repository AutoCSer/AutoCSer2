using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 分词类型
    /// </summary>
    internal enum WordSegmentTypeEnum : byte
    {
        /// <summary>
        /// 查询词语编号（忽略未匹配词语）
        /// </summary>
        QueryIdentity,
        /// <summary>
        /// 查询分词结果
        /// </summary>
        Query,
        /// <summary>
        /// 添加文本前持久化检查
        /// </summary>
        AddTextBeforePersistence,
        /// <summary>
        /// 添加文本
        /// </summary>
        AddText,
    }
}
