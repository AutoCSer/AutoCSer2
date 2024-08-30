using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义简单组合模板参数
    /// </summary>
    internal abstract class CombinationTemplateConfig
    {
        /// <summary>
        /// 默认自定义模板相对项目路径
        /// </summary>
        internal const string DefaultTemplatePath = "CombinationTemplate";

        /// <summary>
        /// 自定义模板相对项目路径
        /// </summary>
        internal virtual IEnumerable<string> TemplatePath { get { yield return DefaultTemplatePath; } }
        /// <summary>
        /// 获取目标代码文件名称（默认为项目命名空间）
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <returns></returns>
        internal virtual string GetCodeFileName(string defaultNamespace)
        {
            return defaultNamespace;
        }
    }
}
