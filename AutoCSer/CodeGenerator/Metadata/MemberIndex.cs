using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员信息
    /// </summary>
    internal abstract class MemberIndex : MemberIndexInfo
    {
        /// <summary>
        /// XML文档注释
        /// </summary>
        protected string xmlDocument;
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="field">成员字段信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndex(FieldInfo field, MemberFiltersEnum filter, int index)
            : base(field, field.FieldType, filter, index)
        {
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="property">成员属性信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndex(PropertyInfo property, MemberFiltersEnum filter, int index)
            : base(property, property.PropertyType, filter, index)
        {
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndex(MethodInfo method, MemberFiltersEnum filter, int index)
            : base(method, method.ReturnType, filter, index)
        {
        }
    }
}
