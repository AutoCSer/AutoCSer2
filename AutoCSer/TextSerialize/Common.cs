using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 公共调用
    /// </summary>
    internal static class Common
    {
        ///// <summary>
        ///// 字符串缓存区大小
        ///// </summary>
        //internal const int StringBufferSize = 40;//33

        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <typeparam name="T">序列化成员配置类型</typeparam>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>字段成员集合</returns>
#if NetStandard21
        internal static LeftArray<KeyValue<FieldIndex, T?>> GetSerializeFields<T>(LeftArray<FieldIndex> fields, SerializeAttribute typeAttribute)
#else
        internal static LeftArray<KeyValue<FieldIndex, T>> GetSerializeFields<T>(LeftArray<FieldIndex> fields, SerializeAttribute typeAttribute)
#endif
            where T : AutoCSer.Metadata.IgnoreMemberAttribute
        {
#if NetStandard21
            LeftArray<KeyValue<FieldIndex, T?>> values = new LeftArray<KeyValue<FieldIndex, T?>>(fields.Length);
#else
            LeftArray<KeyValue<FieldIndex, T>> values = new LeftArray<KeyValue<FieldIndex, T>>(fields.Length);
#endif
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.isSerializeNotSupportOrArrayRank() && !field.IsIgnore)
                {
                    var memberAttribute = field.GetAttribute<T>(typeAttribute.IsBaseTypeAttribute);
                    if (typeAttribute.IsAttribute ? (memberAttribute != null && !memberAttribute.GetIsIgnoreCurrent) : (memberAttribute == null || !memberAttribute.GetIsIgnoreCurrent))
                    {
                        values.Add(KeyValue.From(field, memberAttribute));
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 获取属性成员集合
        /// </summary>
        /// <typeparam name="T">序列化成员配置类型</typeparam>
        /// <param name="properties">属性成员集合</param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>属性成员集合</returns>
        internal static LeftArray<PropertyMethod<T>> GetSerializeProperties<T>(LeftArray<PropertyIndex> properties, SerializeAttribute typeAttribute)
            where T : AutoCSer.Metadata.IgnoreMemberAttribute
        {
            LeftArray<PropertyMethod<T>> values = new LeftArray<PropertyMethod<T>>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.Member.CanRead)
                {
                    Type propertyType = property.Member.PropertyType;
                    if (!propertyType.isSerializeNotSupportOrArrayRank() && !property.IsIgnore)
                    {
                        var attribute = property.GetAttribute<T>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (attribute != null && !attribute.GetIsIgnoreCurrent) : (attribute == null || !attribute.GetIsIgnoreCurrent))
                        {
                            var method = property.Member.GetGetMethod(true);
                            if (method != null && method.GetParameters().Length == 0) values.Add(new PropertyMethod<T>(property, attribute, method));
                        }
                    }
                }
            }
            return values;
        }

        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <typeparam name="T">序列化成员配置类型</typeparam>
        /// <param name="fields"></param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>字段成员集合</returns>
#if NetStandard21
        internal static LeftArray<KeyValue<FieldIndex, T?>> GetDeserializeFields<T>(LeftArray<FieldIndex> fields, SerializeAttribute typeAttribute)
#else
        internal static LeftArray<KeyValue<FieldIndex, T>> GetDeserializeFields<T>(LeftArray<FieldIndex> fields, SerializeAttribute typeAttribute)
#endif
            where T : AutoCSer.Metadata.IgnoreMemberAttribute
        {
#if NetStandard21
            LeftArray<KeyValue<FieldIndex, T?>> values = new LeftArray<KeyValue<FieldIndex, T?>>(fields.Length);
#else
            LeftArray<KeyValue<FieldIndex, T>> values = new LeftArray<KeyValue<FieldIndex, T>>(fields.Length);
#endif
            foreach (FieldIndex field in fields)
            {
                Type type = field.Member.FieldType;
                if (!type.isSerializeNotSupportOrArrayRank() && !field.IsIgnore)
                {
                    var property = field.AnonymousProperty?.Member;
                    if (property == null || !property.IsDefined(typeof(IgnoreAttribute), true))
                    {
                        var memberAttribute = default(T);
                        if (property == null) memberAttribute = field.GetAttribute<T>(typeAttribute.IsBaseTypeAttribute);
                        else memberAttribute = property.GetCustomAttribute<T>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (memberAttribute != null && !memberAttribute.GetIsIgnoreCurrent) : (memberAttribute == null || !memberAttribute.GetIsIgnoreCurrent))
                        {
                            values.Add(KeyValue.From(field, memberAttribute));
                        }
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 获取属性成员集合
        /// </summary>
        /// <typeparam name="T">序列化成员配置类型</typeparam>
        /// <param name="properties">属性成员集合</param>
        /// <param name="typeAttribute">类型配置</param>
        /// <returns>属性成员集合</returns>
        internal static LeftArray<PropertyMethod<T>> GetDeserializeProperties<T>(LeftArray<PropertyIndex> properties, SerializeAttribute typeAttribute)
            where T : AutoCSer.Metadata.IgnoreMemberAttribute
        {
            LeftArray<PropertyMethod<T>> values = new LeftArray<PropertyMethod<T>>(properties.Length);
            foreach (PropertyIndex property in properties)
            {
                if (property.AnonymousField != null || property.Member.CanWrite)
                {
                    Type type = property.Member.PropertyType;
                    if (!type.isSerializeNotSupportOrArrayRank() && !property.IsIgnore)
                    {
                        var attribute = property.GetAttribute<T>(typeAttribute.IsBaseTypeAttribute);
                        if (typeAttribute.IsAttribute ? (attribute != null && !attribute.GetIsIgnoreCurrent) : (attribute == null || !attribute.GetIsIgnoreCurrent))
                        {
                            if (property.AnonymousField != null) values.Add(new PropertyMethod<T>(property, attribute, null));
                            else
                            {
                                var method = property.Member.GetSetMethod(true);
                                if (method?.GetParameters().Length == 1) values.Add(new PropertyMethod<T>(property, attribute, method));
                            }
                        }
                    }
                }
            }
            return values;
        }
    }
}
