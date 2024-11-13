using AutoCSer.Extensions;
using AutoCSer.ORM.CustomColumn;
using System;
using System.Collections;
using System.Linq;
using System.Threading;

namespace AutoCSer.ORM.QueryParameter
{
    /// <summary>
    /// 成员参数匹配数据
    /// </summary>
    internal sealed class MemberParameter
    {
        /// <summary>
        /// 数据列序号
        /// </summary>
        public readonly MemberColumnIndex ColumnIndex;
        /// <summary>
        /// 查询参数字段信息
        /// </summary>
        public readonly FieldParameter Field;
        /// <summary>
        /// 成员参数匹配数据
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="field"></param>
        internal MemberParameter(ref MemberColumnIndex columnIndex, FieldParameter field)
        {
            ColumnIndex = columnIndex;
            Field = field;
        }

        /// <summary>
        /// 匹配查询成员
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="field"></param>
        /// <returns></returns>
#if NetStandard21
        internal static MemberParameter? Match(ref MemberColumnIndex columnIndex, FieldParameter field)
#else
        internal static MemberParameter Match(ref MemberColumnIndex columnIndex, FieldParameter field)
#endif
        {
            switch (field.Attribute.MatchType)
            {
                case QueryMatchTypeEnum.Equal:
                case QueryMatchTypeEnum.NotEqual:
                    return new MemberParameter(ref columnIndex, field);
                case QueryMatchTypeEnum.Less:
                case QueryMatchTypeEnum.Greater:
                case QueryMatchTypeEnum.LessOrEqual:
                case QueryMatchTypeEnum.GreaterOrEqual:

                case QueryMatchTypeEnum.In:
                case QueryMatchTypeEnum.NotIn:

                case QueryMatchTypeEnum.Like:
                case QueryMatchTypeEnum.NotLike:
                case QueryMatchTypeEnum.StartsWith:
                case QueryMatchTypeEnum.NotStartsWith:
                case QueryMatchTypeEnum.EndsWith:
                case QueryMatchTypeEnum.NotEndsWith:
                case QueryMatchTypeEnum.Contains:
                case QueryMatchTypeEnum.NotContains:
                    if (columnIndex.IsColumn) return new MemberParameter(ref columnIndex, field);
                    return null;
            }
            return null;
        }
    }
    /// <summary>
    /// 查询参数匹配表格模型
    /// </summary>
    /// <typeparam name="T">查询参数类型</typeparam>
    /// <typeparam name="MT">表格模型类型</typeparam>
    internal sealed class MemberParameter<T, MT>
        where T : class
        where MT : class
    {
        /// <summary>
        /// 添加查询参数条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="value"></param>
        public static void Set(QueryBuilder<MT> query, T value)
        {
            if (memberParameters == null)
            {
                var fieldList = default(ListArray<FieldParameter>);
                Monitor.Enter(memberParameterLock);
                try
                {
                    if (memberParameters == null)
                    {
                        LeftArray<MemberParameter> memberParameterArray = new LeftArray<MemberParameter>(FieldParameter<T>.FieldDictionary.Count);
                        foreach (Member member in query.TableWriter.Members)
                        {
                            MemberColumnIndex columnIndex = new MemberColumnIndex(member, -1);
                            if (FieldParameter<T>.FieldDictionary.TryGetValue(member.MemberIndex.Member.Name, out fieldList))
                            {
                                foreach (FieldParameter field in fieldList)
                                {
                                    var memberParameter = MemberParameter.Match(ref columnIndex, field);
                                    if (memberParameter != null) memberParameterArray.Add(memberParameter);
                                }
                            }
                            if (member.CustomColumnAttribute != null)
                            {
                                foreach (CustomColumnName name in member.CustomColumnNames)
                                {
                                    ++columnIndex.Index;
                                    if (FieldParameter<T>.FieldDictionary.TryGetValue(name.Name, out fieldList))
                                    {
                                        foreach (FieldParameter field in fieldList)
                                        {
                                            var memberParameter = MemberParameter.Match(ref columnIndex, field);
                                            if (memberParameter != null) memberParameterArray.Add(memberParameter);
                                        }
                                    }
                                }
                            }
                        }
                        memberParameters = memberParameterArray.OrderBy(p => p.Field.Attribute.Index).ThenBy(p => p.Field.Index).ToArray();
                    }
                }
                finally { Monitor.Exit(memberParameterLock); }
            }

            foreach (MemberParameter memberParameter in memberParameters)
            {
                var fieldValue = memberParameter.Field.Field.notNull().GetValue(value);
                switch (memberParameter.Field.Attribute.MatchType)
                {
                    case QueryMatchTypeEnum.Equal:
                    case QueryMatchTypeEnum.NotEqual:

                    case QueryMatchTypeEnum.Less:
                    case QueryMatchTypeEnum.Greater:
                    case QueryMatchTypeEnum.LessOrEqual:
                    case QueryMatchTypeEnum.GreaterOrEqual:
                        if(memberParameter.Field.GenericType == null)
                        {
                            if (fieldValue != null) query.And(memberParameter, fieldValue);
                        }
                        else
                        {
                            object nullableValue = fieldValue.notNull();
                            if (memberParameter.Field.GenericType.notNull().IsNullableHasValue(nullableValue)) query.And(memberParameter, nullableValue);
                        }
                        break;
                    case QueryMatchTypeEnum.In:
                        var collection = fieldValue as ICollection;
                        if (collection != null)
                        {
                            if (collection.Count == 0)
                            {
                                query.ConditionLogicType = ConditionExpression.LogicTypeEnum.False;
                                return;
                            }
                            query.And(memberParameter, collection);
                        }
                        break;
                    case QueryMatchTypeEnum.NotIn:
                        collection = fieldValue as ICollection;
                        if (collection != null && collection.Count != 0) query.And(memberParameter, collection);
                        break;
                    case QueryMatchTypeEnum.Like:
                    case QueryMatchTypeEnum.StartsWith:
                    case QueryMatchTypeEnum.EndsWith:
                    case QueryMatchTypeEnum.Contains:
                        var stringValue = fieldValue.castType<string>();
                        if (!string.IsNullOrEmpty(stringValue)) query.And(memberParameter, stringValue);
                        break;
                    case QueryMatchTypeEnum.NotLike:
                    case QueryMatchTypeEnum.NotStartsWith:
                    case QueryMatchTypeEnum.NotEndsWith:
                    case QueryMatchTypeEnum.NotContains:
                        stringValue = fieldValue.castType<string>();
                        if (string.IsNullOrEmpty(stringValue))
                        {
                            query.ConditionLogicType = ConditionExpression.LogicTypeEnum.False;
                            return;
                        }
                        query.And(memberParameter, stringValue);
                        break;
                }
            }
        }

        /// <summary>
        /// 成员参数匹配数据集合
        /// </summary>
#if NetStandard21
        private static MemberParameter[]? memberParameters;
#else
        private static MemberParameter[] memberParameters;
#endif
        /// <summary>
        /// 成员参数匹配数据集合 访问锁
        /// </summary>
        private static readonly object memberParameterLock = new object();
    }
}
