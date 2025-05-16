using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 属性成员信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PropertyMethod<T> where T : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 属性索引
        /// </summary>
        public readonly PropertyIndex Property;
        /// <summary>
        /// 访问函数
        /// </summary>
#if NetStandard21
        public readonly MethodInfo? Method;
#else
        public readonly MethodInfo Method;
#endif
        /// <summary>
        /// 成员自定义属性
        /// </summary>
#if NetStandard21
        public readonly T? MemberAttribute;
#else
        public readonly T MemberAttribute;
#endif
        /// <summary>
        /// 属性成员信息
        /// </summary>
        /// <param name="property"></param>
        /// <param name="method"></param>
        /// <param name="memberAttribute"></param>
#if NetStandard21
        internal PropertyMethod(PropertyIndex property, T? memberAttribute, MethodInfo? method)
#else
        internal PropertyMethod(PropertyIndex property, T memberAttribute, MethodInfo method)
#endif
        {
            Property = property;
            Method = method;
            MemberAttribute = memberAttribute;
        }
    }
}
