using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 全局忽略
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
