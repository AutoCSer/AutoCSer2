using AutoCSer.Metadata;
using System;

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlSerializeAttribute : AutoCSer.TextSerialize.SerializeAttribute
    {
        /// <summary>
        /// 匿名类型序列化配置
        /// </summary>
        internal static readonly XmlSerializeAttribute AnonymousTypeMember = new XmlSerializeAttribute { IsBaseType = false, Filter = MemberFiltersEnum.InstanceField };
    }
}
