using System;

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化成员配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlSerializeMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 集合子节点名称(不能包含非法字符)
        /// </summary>
#if NetStandard21
        public string? ItemName;
#else
        public string ItemName;
#endif
    }
}
