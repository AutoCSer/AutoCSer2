using System;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化成员配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonSerializeMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
    }
}
