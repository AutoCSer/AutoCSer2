using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 属性索引
    /// </summary>
    internal sealed class PropertyIndex : MemberIndexInfo<PropertyInfo>
    {
        /// <summary>
        /// 匿名字段
        /// </summary>
#if NetStandard21
        internal FieldIndex? AnonymousField;
#else
        internal FieldIndex AnonymousField;
#endif
        /// <summary>
        /// 属性信息
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        /// <param name="anonymousField">匿名字段</param>
#if NetStandard21
        public PropertyIndex(PropertyInfo property, MemberFiltersEnum filter, int index, FieldInfo? anonymousField = null)
#else
        public PropertyIndex(PropertyInfo property, MemberFiltersEnum filter, int index, FieldInfo anonymousField = null)
#endif
            : base(property, property.PropertyType, filter, index)
        {
            CanSet = property.CanWrite;
            CanGet = property.CanRead;
            if (anonymousField != null) AnonymousField = new FieldIndex(anonymousField, MemberFiltersEnum.NonPublicInstanceField, index, this);
        }
        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public override object? GetValue(object? value)
#else
        public override object GetValue(object value)
#endif
        {
            return Member.GetValue(value, null);
        }
    }
}
