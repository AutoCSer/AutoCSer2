using System;

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 忽略成员配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
