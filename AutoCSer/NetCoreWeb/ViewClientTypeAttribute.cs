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
        public string Name;
        /// <summary>
        /// 绑定关键字成员名称
        /// </summary>
        public string PrimaryKeyMemberName;

        /// <summary>
        /// 空数据视图绑定客户端类型
        /// </summary>
        internal static readonly ViewClientTypeAttribute Null = new ViewClientTypeAttribute();
    }
}
