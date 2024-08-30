using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 树节点标识类型
    /// </summary>
    internal enum TreeBuilderTagTypeEnum : byte
    {
        /// <summary>
        /// 普通代码段
        /// </summary>
        Code,
        /// <summary>
        /// #region代码段
        /// </summary>
        Region,
        /// <summary>
        /// /**/注释代码段
        /// </summary>
        Note,
        /// <summary>
        /// @取值代码
        /// </summary>
        At,
    }
}
