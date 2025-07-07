using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// Customize simple combination template parameters
    /// 自定义简单组合模板参数
    /// </summary>
    internal abstract class CombinationTemplateConfig
    {
        /// <summary>
        /// The default custom template relative to the project path CombinationTemplate
        /// 默认自定义模板相对项目路径 CombinationTemplate
        /// </summary>
        internal const string DefaultTemplatePath = "CombinationTemplate";

        /// <summary>
        /// Custom template relative to project path
        /// 自定义模板相对项目路径
        /// </summary>
        internal virtual IEnumerable<string> TemplatePath { get { yield return DefaultTemplatePath; } }
        /// <summary>
        /// Get the name of the target code file (default is the project namespace)
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
