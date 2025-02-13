using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 添加词语返回状态
    /// </summary>
    public enum AppendWordStateEnum : byte
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknown,
        /// <summary>
        /// 添加成功
        /// </summary>
        Success,
        /// <summary>
        /// 文字长度不足（不允许小于 2）
        /// </summary>
        WordSizeLess,
        /// <summary>
        /// 文字长度超出指定限制
        /// </summary>
        WordSizeLimit,
        /// <summary>
        /// 已建图，不允许添加新词语
        /// </summary>
        GraphBuilded,
    }
}
