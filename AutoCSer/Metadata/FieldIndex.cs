using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 字段索引
    /// </summary>
    internal sealed class FieldIndex : MemberIndexInfo<FieldInfo>
    {
        /// <summary>
        /// 匿名字段绑定属性
        /// </summary>
#if NetStandard21
        internal readonly PropertyIndex? AnonymousProperty;
#else
        internal readonly PropertyIndex AnonymousProperty;
#endif
        /// <summary>
        /// 匿名字段名称（如果是属性生成则转换为属性名称）
        /// </summary>
        internal readonly string AnonymousName;
        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        /// <param name="anonymousName">匿名字段名称</param>
#if NetStandard21
        public FieldIndex(FieldInfo field, MemberFiltersEnum filter, int index, string? anonymousName = null)
#else
        public FieldIndex(FieldInfo field, MemberFiltersEnum filter, int index, string anonymousName = null)
#endif
            : base(field, field.FieldType, filter, index)
        {
            IsField = CanGet = true;
            CanSet = !field.IsInitOnly;
            AnonymousName = anonymousName ?? field.Name;
        }
        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        /// <param name="anonymousProperty">匿名字段绑定属性</param>
        public FieldIndex(FieldInfo field, MemberFiltersEnum filter, int index, PropertyIndex anonymousProperty)
            : base(field, field.FieldType, filter, index)
        {
            IsField = CanGet = true;
            CanSet = !field.IsInitOnly;
            AnonymousProperty = anonymousProperty;
            AnonymousName = anonymousProperty.Member.Name;
        }
        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public override object? GetValue(object? value)
#else
        public override object GetValue(object value)
#endif
        {
            return Member.GetValue(value);
        }
    }
}
