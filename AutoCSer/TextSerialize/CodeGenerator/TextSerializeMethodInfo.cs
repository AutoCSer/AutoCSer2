using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 文本序列化信息
    /// </summary>
    public struct TextSerializeMethodInfo
    {
        /// <summary>
        /// 是否类型序列化
        /// </summary>
        internal bool IsTypeSerialize;
        /// <summary>
        /// 是否自定义序列化接口
        /// </summary>
        internal bool IsCusotm;
        /// <summary>
        /// 是否需要设置空节点输出字符串
        /// </summary>
        internal bool IsEmptyString;
    }
}
