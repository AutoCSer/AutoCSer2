using System;

namespace AutoCSer.Html
{
    /// <summary>
    /// 原始 HTML 节点类型
    /// </summary>
    internal enum RawNodeTypeEnum
    {
        /// <summary>
        /// 开始标签 {xxx}
        /// </summary>
        StartTag,
        /// <summary>
        /// 自回合标签 {xxx/}
        /// </summary>
        Tag,
        /// <summary>
        /// 原始 HTML 代码段
        /// </summary>
        Html,
        /// <summary>
        /// 普通注释 {!--} 或者 {!--xxx--}
        /// </summary>
        Note,
        /// <summary>
        /// 文本注释 {![CDATA[xxx]]}
        /// </summary>
        DataNote,
        /// <summary>
        /// 注释标签 {!xxx}
        /// </summary>
        NoteTag,
        /// <summary>
        /// 回合标签 {/xxx}
        /// </summary>
        RoundTag,
        /// <summary>
        /// 空标签 {}
        /// </summary>
        NullTag,
    }
}
