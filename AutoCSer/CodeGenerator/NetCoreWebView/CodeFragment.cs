using System;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// 代码片段
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CodeFragment
    {
        /// <summary>
        /// 代码片段类型
        /// </summary>
        internal readonly CodeFragmentTypeEnum Type;
        /// <summary>
        /// 原代码字串索引
        /// </summary>
        internal readonly Range CodeRange;
        /// <summary>
        /// 嵌入文件信息
        /// </summary>
        internal IncludeFile IncludeFile;
        /// <summary>
        /// 代码片段
        /// </summary>
        /// <param name="type">代码片段类型</param>
        /// <param name="codeRange">原代码字串索引</param>
        internal CodeFragment(CodeFragmentTypeEnum type, Range codeRange)
        {
            this.Type = type;
            this.CodeRange = codeRange;
            IncludeFile = null;
        }
    }
}
