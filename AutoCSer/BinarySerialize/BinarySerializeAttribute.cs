using AutoCSer.Metadata;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization type configuration
    /// 二进制数据序列化类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class BinarySerializeAttribute : MemberFilterAttribute.InstanceField
    {
        /// <summary>
        /// The default is true, indicating that it acts on an unknown derived type
        /// 默认为 true 表示作用于未知派生类型
        /// </summary>
        public bool IsBaseType = true;
        /// <summary>
        /// The default is true, indicating that the same object reference is checked
        /// 默认为 true 表示检查相同对象引用
        /// </summary>
        public bool IsReferenceMember = true;
        /// <summary>
        /// By default, false indicates that it will not be converted to JSON mixed binary serialization; otherwise, other binary serialization configurations will be ignored. Use JSON mixed binary serialization to serialize the entire object (Value data is serialized in binary)
        /// 默认为 false 表示不转换为 JSON 混杂二进制序列化，否则忽略其它二进制序列化配置使用 JSON 混杂二进制序列化化整个对象（Value 数据采用二进制序列化）
        /// </summary>
        public bool IsJsonMix;
        /// <summary>
        /// By default, false indicates that no JSON serialization member tags are reserved, and setting it to true indicates that some members are serialized using JSON(It takes effect when IsMemberMap is true)
        /// 默认为 false 表示不预留 JSON 序列化成员标记，设置为 true 表示部分成员采用 JSON 序列化（IsMemberMap 为 true 时生效）
        /// </summary>
        public bool IsJsonMember;
        /// <summary>
        /// Is reserve the JSON serialization member tag
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetIsJsonMember(Type type)
        {
            return IsJsonMember & GetIsMemberMap(type);
        }
        /// <summary>
        /// Is serialize the member bitmap
        /// 是否序列化成员位图
        /// </summary>
        public bool IsMemberMap;
#if AOT
        /// <summary>
        /// The default is true, indicating that the AOT code generation is called
        /// 默认为 true 表示调用 AOT 代码生成
        /// </summary>
        public bool IsCodeGenerator = true;
#endif
        ///// <summary>
        ///// 是否选择匿名字段，默认为 false
        ///// </summary>
        //public bool IsAnonymousFields;
        /// <summary>
        /// Is serialize the member bitmap
        /// 是否序列化成员位图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetIsMemberMap(Type type)
        {
            return IsMemberMap && !ServerMethodParameter.Types.Contains(type);// & !IsAnonymousFields，默认为 true 在 IsAnonymousFields 为 false 时生效
        }

        /// <summary>
        /// For the types that require circular reference checking in custom serialization, an array length of 0 indicates no circular reference checking is needed, and null indicates unknown
        /// 自定义序列化需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知
        /// </summary>
#if NetStandard21
        public Type?[]? CustomReferenceTypes;
#else
        public Type[] CustomReferenceTypes;
#endif
    }
}
