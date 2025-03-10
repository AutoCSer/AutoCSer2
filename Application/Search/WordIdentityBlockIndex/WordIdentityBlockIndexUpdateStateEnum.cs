using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 分词结果磁盘块索引信息更新状态
    /// </summary>
    public enum WordIdentityBlockIndexUpdateStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 未找到关键字触发删除操作
        /// </summary>
        DeletedNotFoundKey,
        /// <summary>
        /// 不支持删除存在数据的关键字
        /// </summary>
        NotSupportDeleteKey,
        /// <summary>
        /// 关键字不允许为 null
        /// </summary>
        NullKey,
        /// <summary>
        /// 获取需要分词的文本数据失败
        /// </summary>
        GetTextFailed,
        /// <summary>
        /// 获取分词词语标识集合失败
        /// </summary>
        GetWordIdentityFailed,
        /// <summary>
        /// 设置本文分词索引失败
        /// </summary>
        SetWordIndexFailed,
        /// <summary>
        /// 获取磁盘块索引信息失败
        /// </summary>
        GetBlockIndexFailed,
        /// <summary>
        /// 获取文本分词结果失败
        /// </summary>
        GetBlockIndexResultFailed,

        /// <summary>
        /// 已回调
        /// </summary>
        Callbacked,
    }
}
