using System;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 忽略成员配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
