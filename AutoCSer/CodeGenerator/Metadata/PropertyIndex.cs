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
        /// XML文档注释
        /// </summary>
        public string XmlDocument
        {
            get
            {
                if (xmlDocument == null) xmlDocument = AutoCSer.Reflection.XmlDocument.Get(Property);
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string CodeGeneratorXmlDocument
        {
            get { return AutoCSer.Reflection.XmlDocument.CodeGeneratorFormat(XmlDocument); }
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
        /// 成员属性
        /// </summary>
        /// <param name="property">成员属性信息</param>
        internal PropertyIndex(PropertyInfo property) : this(new AutoCSer.Metadata.PropertyIndex(property, MemberFiltersEnum.Unknown, 0)) { }
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
