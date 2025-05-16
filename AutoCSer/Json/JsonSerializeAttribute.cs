using AutoCSer.Metadata;
using System;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonSerializeAttribute : AutoCSer.TextSerialize.SerializeAttribute
    {
        /// <summary>
        /// 文档类型，用于 WEB API 等接口文档的 JSON 序列化描述
        /// </summary>
#if NetStandard21
        public Type? DocumentType;
#else
        public Type DocumentType;
#endif

        ///// <summary>
        ///// 匿名类型序列化配置
        ///// </summary>
        //internal static readonly JsonSerializeAttribute AnonymousTypeMember = new JsonSerializeAttribute { IsBaseType = false, Filter = MemberFiltersEnum.PublicInstance };
    }
}
