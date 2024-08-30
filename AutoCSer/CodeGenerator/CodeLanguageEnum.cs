using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成语言
    /// </summary>
    internal enum CodeLanguageEnum : byte
    {
        /// <summary>
        /// C#
        /// </summary>
        CSharp,
        /// <summary>
        /// JavaScript
        /// </summary>
        JavaScript,
        /// <summary>
        /// TypeScript
        /// </summary>
        TypeScript,

        /// <summary>
        /// 保留计数
        /// </summary>
        COUNT,
    }
}
