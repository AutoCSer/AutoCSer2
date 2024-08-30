using System;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 类定义生成
    /// </summary>
    internal abstract class TypeDefinition
    {
        /// <summary>
        /// 类定义开始
        /// </summary>
        protected ListArray<string> start = new ListArray<string>();
        /// <summary>
        /// 类定义开始
        /// </summary>
        internal string Start { get { return string.Concat(start); } }
        /// <summary>
        /// 类定义结束
        /// </summary>
        protected ListArray<string> end = new ListArray<string>();
        /// <summary>
        /// 类定义结束
        /// </summary>
        internal string End { get { return string.Concat(end); } }
    }
}
