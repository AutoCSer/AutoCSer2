using AutoCSer.Metadata;
using System;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 序列化类型配置
    /// </summary>
    public abstract class SerializeAttribute : MemberFilterAttribute.PublicInstance
    {
        /// <summary>
        /// 是否作用与派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
        /// <summary>
        /// 是否检测循环引用（仅对引用类型有效），默认为 true
        /// </summary>
        public bool CheckLoopReference = true;

        /// <summary>
        /// 自定义序列化需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知
        /// </summary>
#if NetStandard21
        public Type?[]? CustomReferenceTypes;
#else
        public Type[] CustomReferenceTypes;
#endif
    }
}
