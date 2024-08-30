using System;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// 代码片段类型
    /// </summary>
    internal enum CodeFragmentTypeEnum : byte
    {
        /// <summary>
        /// 原代码
        /// </summary>
        Code,
        /// <summary>
        /// 嵌入文件
        /// </summary>
        Include,
        /// <summary>
        /// 嵌入参数
        /// </summary>
        Parameter,
    }
}
