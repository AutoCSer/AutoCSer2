using AutoCSer.Metadata;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class BinarySerializeAttribute : MemberFilterAttribute.InstanceField
    {
        /// <summary>
        /// 是否作用于未知派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
        /// <summary>
        /// 是否检查相同对象引用，默认为 true
        /// </summary>
        public bool IsReferenceMember = true;
        /// <summary>
        /// 默认为 false 表示不转换为二进制混杂 JSON 序列，否则忽略其它二进制序列化配置使用二进制混杂 JSON 序列化化整个对象（Value 数据采用二进制序列化）
        /// </summary>
        public bool IsMixJsonSerialize;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记，默认为 false
        /// </summary>
        public bool IsJsonMember;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记，默认为 false
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetIsJsonMember(Type type)
        {
            return IsJsonMember & GetIsMemberMap(type);
        }
        /// <summary>
        /// 是否序列化成员位图
        /// </summary>
        public bool IsMemberMap;
#if AOT
        /// <summary>
        /// 默认为 true 表示调用代码生成
        /// </summary>
        public bool IsCodeGenerator = true;
#endif
        ///// <summary>
        ///// 是否选择匿名字段，默认为 false
        ///// </summary>
        //public bool IsAnonymousFields;
        /// <summary>
        /// 是否序列化成员位图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetIsMemberMap(Type type)
        {
            return IsMemberMap && !ServerMethodParameter.IsType(type);// & !IsAnonymousFields，默认为 true 在 IsAnonymousFields 为 false 时生效
        }

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
