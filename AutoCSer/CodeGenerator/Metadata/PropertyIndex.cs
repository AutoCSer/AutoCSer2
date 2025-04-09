using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员属性
    /// </summary>
    internal sealed class PropertyIndex : MemberIndex
    {
        /// <summary>
        /// 成员属性信息
        /// </summary>
        public PropertyInfo Property { get; private set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return Property.Name; }
        }
        /// <summary>
        /// 成员属性
        /// </summary>
        /// <param name="property">成员属性信息</param>
        internal PropertyIndex(AutoCSer.Metadata.PropertyIndex property) : base(property.Member, property.MemberFilters, property.MemberIndex)
        {
            Property = property.Member;
        }
        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object GetValue(object value)
        {
            return Property.GetValue(value);
        }
    }
}
