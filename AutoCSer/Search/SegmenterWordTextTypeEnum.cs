using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 分词文本类型
    /// </summary>
    internal enum SegmenterWordTextTypeEnum
    {
        /// <summary>
        /// 原始文本字符串
        /// </summary>
        Raw,
        /// <summary>
        /// 允许通过非安全指针方式修改的原始文本字符串，避免申请内存
        /// </summary>
        Temp,
        /// <summary>
        /// 已格式化的文本字符串
        /// </summary>
        Formated
    }
}
