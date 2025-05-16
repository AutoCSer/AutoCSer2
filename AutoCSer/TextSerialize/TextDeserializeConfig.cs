using System;

namespace AutoCSer
{
    /// <summary>
    /// 文本反序列化配置参数
    /// </summary>
    public abstract class TextDeserializeConfig
    {
        /// <summary>
        /// 是否临时字符串(可修改)
        /// </summary>
        internal bool IsTempString;
        /// <summary>
        /// 是否强制匹配枚举值
        /// </summary>
        public bool IsMatchEnum;
        /// <summary>
        /// 默认数组大小为 10
        /// </summary>
        public int NewArraySize = 10;
        /// <summary>
        /// 指针模式反序列化失败时是否 new string
        /// </summary>
        public bool IsErrorNewString;
    }
}
