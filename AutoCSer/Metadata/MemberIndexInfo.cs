using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员索引
    /// </summary>
    internal abstract class MemberIndexInfo
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public MemberInfo Member { get; protected set; }
        /// <summary>
        /// 成员类型
        /// </summary>
        public Type MemberSystemType { get; protected set; }
        /// <summary>
        /// 模板成员类型
        /// </summary>
        public virtual Type TemplateMemberType { get { return MemberSystemType; } }
        /// <summary>
        /// 成员编号
        /// </summary>
        public int MemberIndex { get; protected set; }
        /// <summary>
        /// 选择类型
        /// </summary>
        public MemberFiltersEnum MemberFilters;
        /// <summary>
        /// 是否字段
        /// </summary>
        public bool IsField { get; protected set; }
        /// <summary>
        /// 是否可赋值
        /// </summary>
        public bool CanSet { get; protected set; }
        /// <summary>
        /// 是否可读取
        /// </summary>
        public bool CanGet { get; protected set; }
        /// <summary>
        /// 是否忽略该成员
        /// </summary>
        private NullableBoolEnum isIgnore;
        /// <summary>
        /// 是否忽略该成员
        /// </summary>
        public bool IsIgnore
        {
            get
            {
                if (isIgnore == NullableBoolEnum.Null) isIgnore = Member != null && GetAttribute<IgnoreAttribute>(true) != null ? NullableBoolEnum.True : NullableBoolEnum.False;
                return isIgnore == NullableBoolEnum.True;
            }
        }
        /// <summary>
        /// 自定义属性集合
        /// </summary>
#if NetStandard21
        private object[]? attributes;
#else
        private object[] attributes;
#endif
        /// <summary>
        /// 自定义属性集合(包括基类成员属性)
        /// </summary>
#if NetStandard21
        private object[]? baseAttributes;
#else
        private object[] baseAttributes;
#endif
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="memberType">成员类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        internal MemberIndexInfo(MemberInfo member, Type memberType, MemberFiltersEnum filter, int index)
        {
            Member = member;
            MemberIndex = index;
            MemberFilters = filter;
            MemberSystemType = memberType;
        }
        /// <summary>
        /// 获取自定义属性集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns></returns>
        internal IEnumerable<T> Attributes<T>(bool isBaseType) where T : Attribute
        {
            if (Member != null)
            {
                object[] values;
                if (isBaseType)
                {
                    if (baseAttributes == null)
                    {
                        baseAttributes = Member.GetCustomAttributes(true);
                        if (baseAttributes.Length == 0) attributes = baseAttributes;
                    }
                    values = baseAttributes;
                }
                else
                {
                    if (attributes == null) attributes = Member.GetCustomAttributes(false);
                    values = attributes;
                }
                foreach (object value in values)
                {
                    var attribute = value as T;
                    if (attribute != null) yield return attribute;
                }
            }
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="T">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <returns>自定义属性</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? GetAttribute<T>(bool isBaseType) where T : Attribute
#else
        public T GetAttribute<T>(bool isBaseType) where T : Attribute
#endif
        {
            foreach (T attribute in Attributes<T>(isBaseType)) return attribute;
            return null;
        }
        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public abstract object? GetValue(object? value);
#else
        public abstract object GetValue(object value);
#endif
    }
    /// <summary>
    /// 成员索引
    /// </summary>
    /// <typeparam name="T">成员类型</typeparam>
    internal abstract class MemberIndexInfo<T> : MemberIndexInfo where T : MemberInfo
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public new T Member;
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="memberType">成员类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndexInfo(T member, Type memberType, MemberFiltersEnum filter, int index)
            : base(member, memberType, filter, index)
        {
            Member = member;
        }
    }
}
