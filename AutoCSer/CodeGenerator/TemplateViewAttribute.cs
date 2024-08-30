using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 模板视图标记，用于标记搜索模板方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TemplateViewAttribute : Attribute
    {
    }
}
