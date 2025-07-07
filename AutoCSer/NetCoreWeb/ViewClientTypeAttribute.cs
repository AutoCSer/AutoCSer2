using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图绑定客户端类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ViewClientTypeAttribute : Attribute
    {
        /// <summary>
        /// 客户端视图绑定类型名称，输出 new Name({x})
        /// </summary>
#if NetStandard21
        public string? Name;
#else
        public string Name;
#endif
        /// <summary>
        /// Bind the keyword member name
        /// 绑定关键字成员名称
        /// </summary>
#if NetStandard21
        public string? PrimaryKeyMemberName;
#else
        public string PrimaryKeyMemberName;
#endif

        /// <summary>
        /// 空数据视图绑定客户端类型
        /// </summary>
        internal static readonly ViewClientTypeAttribute Null = new ViewClientTypeAttribute();
    }
}
