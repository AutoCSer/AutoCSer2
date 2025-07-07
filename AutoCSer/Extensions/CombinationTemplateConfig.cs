using System;
using System.Collections.Generic;
using System.IO;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Customize simple combination template parameters
    /// 自定义简单组合模板参数
    /// </summary>
    internal sealed class CombinationTemplateConfig : AutoCSer.CodeGenerator.CombinationTemplateConfig
    {
        /// <summary>
        /// Custom template relative to project path
        /// 自定义模板相对项目路径
        /// </summary>
        internal override IEnumerable<string> TemplatePath
        {
            get
            {
                yield return Path.Combine("Algorithm", DefaultTemplatePath);
                yield return Path.Combine("Xml", DefaultTemplatePath);
            }
        }
    }
}
