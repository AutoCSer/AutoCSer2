using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 分词选项
    /// </summary>
    [Flags]
    public enum WordSegmentFlags : byte
    {
        /// <summary>
        /// 是否支持单字符搜索结果（设置为 true 会造成索引占用大量内存并浪费大量计算资源）
        /// </summary>
        SingleCharacter = 1,
    }
}
