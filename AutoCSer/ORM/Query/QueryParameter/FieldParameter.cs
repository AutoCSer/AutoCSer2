using AutoCSer.Extensions;
using AutoCSer.ORM.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.ORM.QueryParameter
{
    /// <summary>
    /// 查询参数字段信息
    /// </summary>
    internal sealed class FieldParameter
    {
        /// <summary>
        /// 查询参数字段
        /// </summary>
        public FieldInfo Field;
        /// <summary>
        /// 列表成员类型
        /// </summary>
        public Type ElementType;
        /// <summary>
        /// 值类型泛型对象
        /// </summary>
        public AutoCSer.ORM.Metadata.StructGenericType GenericType;
        /// <summary>
        /// 查询参数属性
        /// </summary>
        public QueryParameterAttribute Attribute;
        /// <summary>
        /// 查询参数字段序号
        /// </summary>
        public int Index;
    }
    /// <summary>
    /// 查询参数
    /// </summary>
    /// <typeparam name="T">查询参数类型</typeparam>
    internal static class FieldParameter<T> where T : class
    {
        /// <summary>
        /// 查询字段集合
        /// </summary>
        internal static readonly Dictionary<string, ListArray<FieldParameter>> FieldDictionary;
        ///// <summary>
        ///// 指定查询列
        ///// </summary>
        //internal static readonly FieldInfo QueryMembers;

        static FieldParameter()
        {
            FieldDictionary = new Dictionary<string, ListArray<FieldParameter>>();
            int fieldIndex = 0;
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                QueryParameterAttribute attribute = (QueryParameterAttribute)field.GetCustomAttribute(typeof(QueryParameterAttribute), false) ?? QueryParameterAttribute.Default;
                string operationName = attribute.TableMemberName ?? field.Name;
                Type fieldType = field.FieldType, elementType = null;
                AutoCSer.ORM.Metadata.StructGenericType genericType = null;
                if (fieldType.IsArray) elementType = fieldType.GetElementType();
                else if (fieldType != typeof(string)) elementType = fieldType.getGenericInterfaceType(typeof(IList<>))?.GetGenericArguments()[0];
                if (attribute.GetCheckPrefixSuffix())
                {
                    if (elementType == null)
                    {
                        if (operationName.StartsWith("_", StringComparison.Ordinal))
                        {
                            operationName = operationName.Substring(1);
                            if (object.ReferenceEquals(attribute, QueryParameterAttribute.Default)) attribute = QueryParameterAttribute.GreaterOrEqual;
                            else attribute.MatchType = QueryMatchTypeEnum.GreaterOrEqual;
                        }
                        else if (operationName.EndsWith("_", StringComparison.Ordinal))
                        {
                            operationName = operationName.Substring(0, operationName.Length - 1);
                            if (object.ReferenceEquals(attribute, QueryParameterAttribute.Default)) attribute = QueryParameterAttribute.Less;
                            else attribute.MatchType = QueryMatchTypeEnum.Less;
                        }
                    }
                    else
                    {
                        if (operationName.EndsWith("_List", StringComparison.Ordinal))
                        {
                            operationName = operationName.Substring(0, operationName.Length - 5);
                            if (object.ReferenceEquals(attribute, QueryParameterAttribute.Default)) attribute = QueryParameterAttribute.In;
                            else if (attribute.MatchType == QueryMatchTypeEnum.Equal) attribute.MatchType = QueryMatchTypeEnum.In;
                        }
                        else if (operationName.EndsWith("_Array", StringComparison.Ordinal))
                        {
                            operationName = operationName.Substring(0, operationName.Length - 6);
                            if (object.ReferenceEquals(attribute, QueryParameterAttribute.Default)) attribute = QueryParameterAttribute.In;
                            else if (attribute.MatchType == QueryMatchTypeEnum.Equal) attribute.MatchType = QueryMatchTypeEnum.In;
                        }
                    }
                }
                if (attribute.MatchType == QueryMatchTypeEnum.Default) attribute.MatchType = QueryMatchTypeEnum.Equal;
                bool isMatchType = false;
                switch (attribute.MatchType)
                {
                    case QueryMatchTypeEnum.Equal:
                    case QueryMatchTypeEnum.NotEqual:
                    case QueryMatchTypeEnum.Less:
                    case QueryMatchTypeEnum.Greater:
                    case QueryMatchTypeEnum.LessOrEqual:
                    case QueryMatchTypeEnum.GreaterOrEqual:
                        if (elementType == null)
                        {
                            if (fieldType.IsValueType)
                            {
                               Type nullableType = fieldType.getNullableType();
                                if (nullableType != null)
                                {
                                    genericType = AutoCSer.ORM.Metadata.StructGenericType.Get(nullableType);
                                    isMatchType = true;
                                }
                                else LogHelper.DebugIgnoreException($"{typeof(T).fullName()}.{field.Name} 值类型必须为 Nullable<> 才能匹配 {attribute.MatchType}");
                            }
                            else isMatchType = true;
                        }
                        else LogHelper.DebugIgnoreException($"{typeof(T).fullName()}.{field.Name} 类型为 IList 不能匹配 {attribute.MatchType}");
                        break;
                    case QueryMatchTypeEnum.In:
                    case QueryMatchTypeEnum.NotIn:
                        if (elementType == null) LogHelper.DebugIgnoreException($"{typeof(T).fullName()}.{field.Name} 类型必须为 IList 才能匹配 {attribute.MatchType}");
                        else isMatchType = true;
                        break;
                    case QueryMatchTypeEnum.Like:
                    case QueryMatchTypeEnum.NotLike:
                    case QueryMatchTypeEnum.StartsWith:
                    case QueryMatchTypeEnum.NotStartsWith:
                    case QueryMatchTypeEnum.EndsWith:
                    case QueryMatchTypeEnum.NotEndsWith:
                    case QueryMatchTypeEnum.Contains:
                    case QueryMatchTypeEnum.NotContains:
                        if (fieldType == typeof(string)) isMatchType = true;
                        else LogHelper.DebugIgnoreException($"{typeof(T).fullName()}.{field.Name} 类型必须为 string 才能匹配 {attribute.MatchType}");
                        break;
                    default:
                        LogHelper.DebugIgnoreException($"{typeof(T).fullName()}.{field.Name} 不可识别匹配类型 {attribute.MatchType}");
                        break;
                }
                if (isMatchType)
                {
                    ListArray<FieldParameter> fieldList;
                    if (!FieldDictionary.TryGetValue(operationName, out fieldList))
                    {
                        FieldDictionary.Add(operationName, fieldList = new ListArray<FieldParameter>());
                    }
                    fieldList.Add(new FieldParameter { Attribute = attribute, Field = field, ElementType = elementType, GenericType = genericType, Index = ++fieldIndex });
                }
                //if (field.Name == nameof(QueryMembers) && field.FieldType == typeof(string[])) QueryMembers = field;
            }
        }
    }
}
