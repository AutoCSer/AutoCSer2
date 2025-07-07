using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// Member selection
    /// 成员选择
    /// </summary>
    public abstract class MemberFilterAttribute : Attribute
    {
        /// <summary>
        /// Member selection type
        /// 成员选择类型
        /// </summary>
        public abstract MemberFiltersEnum MemberFilters { get; }
        /// <summary>
        /// Whether the member matches the custom attribute type, the default is false, indicating that all members are selected.
        /// 成员是否匹配自定义属性类型，默认为 false 表示选择所有成员。
        /// </summary>
        public bool IsAttribute;
        /// <summary>
        /// Specify whether to search the inheritance chain of this member to find these characteristics. Refer to System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)
        /// 指定是否搜索该成员的继承链以查找这些特性，参考 System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)。
        /// </summary>
        public bool IsBaseTypeAttribute;
        ///// <summary>
        ///// 成员匹配自定义属性是否可继承，true 表示允许申明默认配置类型的派生类型并且选择继承深度最小的申明配置。
        ///// </summary>
        //public bool IsInheritAttribute;
        /// <summary>
        /// Default public dynamic member
        /// 默认公有动态成员
        /// </summary>
        public abstract class Instance : MemberFilterAttribute
        {
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public MemberFiltersEnum Filter = MemberFiltersEnum.Instance;
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public override MemberFiltersEnum MemberFilters
            {
                get { return Filter & MemberFiltersEnum.Instance; }
            }
        }
        /// <summary>
        /// Default non-public member
        /// 默认非公有成员
        /// </summary>
        public abstract class NonPublic : MemberFilterAttribute
        {
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public MemberFiltersEnum Filter = MemberFiltersEnum.NonPublic;
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public override MemberFiltersEnum MemberFilters
            {
                get { return Filter; }
            }
        }
        /// <summary>
        /// Default public dynamic member
        /// 默认公有动态成员
        /// </summary>
        public abstract class PublicInstance : MemberFilterAttribute
        {
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public MemberFiltersEnum Filter = MemberFiltersEnum.PublicInstance;
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public override MemberFiltersEnum MemberFilters
            {
                get { return Filter & MemberFiltersEnum.Instance; }
            }
        }
        /// <summary>
        /// Default public dynamic field member
        /// 默认公有动态字段成员
        /// </summary>
        public abstract class PublicInstanceField : MemberFilterAttribute
        {
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public MemberFiltersEnum Filter = MemberFiltersEnum.PublicInstanceField;
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public override MemberFiltersEnum MemberFilters
            {
                get { return Filter & MemberFiltersEnum.Instance; }
            }
        }
        /// <summary>
        /// Default dynamic field member
        /// 默认动态字段成员
        /// </summary>
        public abstract class InstanceField : MemberFilterAttribute
        {
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public MemberFiltersEnum Filter = MemberFiltersEnum.InstanceField;
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public override MemberFiltersEnum MemberFilters
            {
                get { return Filter & MemberFiltersEnum.Instance; }
            }
        }
        /// <summary>
        /// Default dynamic property member
        /// 默认动态属性成员
        /// </summary>
        public abstract class PublicInstanceProperty : MemberFilterAttribute
        {
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public MemberFiltersEnum Filter = MemberFiltersEnum.PublicInstanceProperty;
            /// <summary>
            /// Member selection type
            /// 成员选择类型
            /// </summary>
            public override MemberFiltersEnum MemberFilters
            {
                get { return Filter & MemberFiltersEnum.Instance; }
            }
        }
    }
}
