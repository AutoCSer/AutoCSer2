using AutoCSer.Reflection;
using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 帮助文档类型成员信息
    /// </summary>
    public sealed class MemberHelpView
    {
        /// <summary>
        /// 成员来源定义类型
        /// </summary>
        public readonly TypeHelpView DeclaringType;
        /// <summary>
        /// 成员信息
        /// </summary>
        private readonly MemberInfo member;
        /// <summary>
        /// 成员类型
        /// </summary>
#if NetStandard21
        private TypeHelpView? type;
#else
        private TypeHelpView type;
#endif
        /// <summary>
        /// 成员类型
        /// </summary>
        public TypeHelpView Type
        {
            get
            {
                if (type == null)
                {
                    var field = member as FieldInfo;
                    type = DeclaringType.ViewMiddleware.GetTypeHelpView(field?.FieldType ?? ((PropertyInfo)member).PropertyType);
                }
                return type;
            }
        }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string Name { get { return member.Name; } }
        /// <summary>
        /// 成员文档描述
        /// </summary>
#if NetStandard21
        private string? summary;
#else
        private string summary;
#endif
        /// <summary>
        /// 成员文档描述
        /// </summary>
        public string Summary
        {
            get
            {
                if (summary == null)
                {
                    var field = member as FieldInfo;
                    if(field  == null)
                    {
                        PropertyInfo property = (PropertyInfo)member;
                        summary = XmlDocument.Get(property);
                        if (string.IsNullOrEmpty(summary)) summary = DeclaringType.ViewMiddleware.GetTypeHelpView(property.PropertyType).Summary;
                    }
                    else
                    {
                        summary = XmlDocument.Get(field);
                        if (string.IsNullOrEmpty(summary)) summary = DeclaringType.ViewMiddleware.GetTypeHelpView(field.FieldType).Summary;
                    }
                    if (summary == null) summary = string.Empty;
                }
                return summary;
            }
        }
        /// <summary>
        /// 是否可读
        /// </summary>
        public readonly bool CanRead;
        /// <summary>
        /// 是否可写
        /// </summary>
        public readonly bool CanWrite;
        /// <summary>
        /// 帮助文档类型成员信息
        /// </summary>
        /// <param name="type">帮助文档类型信息</param>
        /// <param name="member">成员信息</param>
        internal MemberHelpView(TypeHelpView type, FieldInfo member)
        {
            DeclaringType = type;
            this.member = member;
            CanRead = CanWrite = true;
        }
        /// <summary>
        /// 帮助文档类型成员信息
        /// </summary>
        /// <param name="type">帮助文档类型信息</param>
        /// <param name="member">成员信息</param>
        /// <param name="canRead">是否可读</param>
        /// <param name="canWrite">是否可写</param>
        internal MemberHelpView(TypeHelpView type, PropertyInfo member, bool canRead, bool canWrite)
        {
            DeclaringType = type;
            this.member = member;
            CanRead = canRead;
            CanWrite = canWrite;
        }
    }
}
