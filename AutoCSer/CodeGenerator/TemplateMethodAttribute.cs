using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 模板方法标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TemplateMethodAttribute : Attribute
    {
    }
}
