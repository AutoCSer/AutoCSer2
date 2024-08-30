using System;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// 树节点标识类型
    /// </summary>
    internal enum TreeBuilderTagTypeEnum : byte
    {
        /// <summary>
        /// 普通HTML段
        /// </summary>
        Html,
        /// <summary>
        /// 注释子段
        /// </summary>
        Note,
        /// <summary>
        /// {{x}}取值代码
        /// </summary>
        Get,
    }
}
