using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成模板支持 await 泛型申明
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AwaitResultTypeAttribute : Attribute
    {
    }
}
