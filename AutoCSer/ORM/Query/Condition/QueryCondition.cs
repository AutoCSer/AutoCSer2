using AutoCSer.Memory;
using AutoCSer.ORM.QueryParameter;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class QueryCondition<T> : ICondition
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        private readonly TableWriter<T> tableWriter;
        /// <summary>
        /// 查询参数成员匹配数据
        /// </summary>
        private readonly MemberParameter memberParameter;
        /// <summary>
        /// 查询参数字段值
        /// </summary>
        private readonly object fieldValue;
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="memberParameter"></param>
        /// <param name="fieldValue"></param>
        internal QueryCondition(TableWriter<T> tableWriter, MemberParameter memberParameter, object fieldValue)
        {
            this.tableWriter = tableWriter;
            this.memberParameter = memberParameter;
            this.fieldValue = fieldValue;
        }
        /// <summary>
        /// 写入条件
        /// </summary>
        /// <param name="charStream"></param>
        void ICondition.WriteCondition(CharStream charStream)
        {
            ConnectionCreator connectionCreator = tableWriter.ConnectionPool.Creator;
            switch (memberParameter.Field.Attribute.MatchType)
            {
                case QueryMatchTypeEnum.Equal:
                    if (memberParameter.ColumnIndex.IsColumn) writeCompareTo(charStream, connectionCreator, '=');
                    else connectionCreator.WriteCustomColumnCondition(charStream, tableWriter, memberParameter.ColumnIndex.Member, fieldValue);
                    break;
                case QueryMatchTypeEnum.NotEqual:
                    if (memberParameter.ColumnIndex.IsColumn) writeCompareTo(charStream, connectionCreator, '<', '>');
                    else
                    {
                        int index = 0;
                        Member member = memberParameter.ColumnIndex.Member;
                        object[] array = tableWriter.GetColumnValueCache(member.CustomColumnNames.Length);
                        try
                        {
                            member.StructGenericType.CustomColumnToArray(fieldValue, array, ref index);

                            index = 0;
                            if (member.CustomColumnNames.Length != 1) charStream.Write('(');
                            foreach (CustomColumnName name in member.CustomColumnNames)
                            {
                                object value = array[index];
                                if (index++ != 0) charStream.SimpleWrite(" or ");
                                connectionCreator.FormatName(charStream, name.Name);
                                if (value == null) charStream.SimpleWrite(" is not null");
                                else
                                {
                                    charStream.Write('<');
                                    charStream.Write('>');
                                    connectionCreator.GetConstantConverter(value.GetType(), name.Member)(charStream, value);
                                }
                            }
                            if (member.CustomColumnNames.Length != 1) charStream.Write(')');
                        }
                        finally { tableWriter.FreeColumnValueCache(array); }
                    }
                    break;
                case QueryMatchTypeEnum.Less: writeCompareTo(charStream, connectionCreator, '<'); break;
                case QueryMatchTypeEnum.Greater: writeCompareTo(charStream, connectionCreator, '>'); break;
                case QueryMatchTypeEnum.LessOrEqual: writeCompareTo(charStream, connectionCreator, '<', '='); break;
                case QueryMatchTypeEnum.GreaterOrEqual: writeCompareTo(charStream, connectionCreator, '>', '='); break;
                case QueryMatchTypeEnum.In: writeIn(charStream, connectionCreator, false); break;
                case QueryMatchTypeEnum.NotIn: writeIn(charStream, connectionCreator, true); break;
                case QueryMatchTypeEnum.Like:
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.SimpleWrite(" like ");
                    connectionCreator.ConvertLike(charStream, (string)fieldValue, true, true);
                    break;
                case QueryMatchTypeEnum.StartsWith:
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.SimpleWrite(" like ");
                    connectionCreator.ConvertLike(charStream, (string)fieldValue, false, true);
                    break;
                case QueryMatchTypeEnum.EndsWith:
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.SimpleWrite(" like ");
                    connectionCreator.ConvertLike(charStream, (string)fieldValue, true, false);
                    break;
                case QueryMatchTypeEnum.Contains:
                    charStream.SimpleWrite(" contains(");
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    connectionCreator.Convert(charStream, (string)fieldValue);
                    charStream.Write(')');
                    break;
                case QueryMatchTypeEnum.NotLike:
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.SimpleWrite(" not like ");
                    connectionCreator.ConvertLike(charStream, (string)fieldValue, true, true);
                    break;
                case QueryMatchTypeEnum.NotStartsWith:
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.SimpleWrite(" not like ");
                    connectionCreator.ConvertLike(charStream, (string)fieldValue, false, true);
                    break;
                case QueryMatchTypeEnum.NotEndsWith:
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.SimpleWrite(" not like ");
                    connectionCreator.ConvertLike(charStream, (string)fieldValue, true, false);
                    break;
                case QueryMatchTypeEnum.NotContains:
                    charStream.SimpleWrite(" not contains(");
                    connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
                    charStream.Write(',');
                    connectionCreator.Convert(charStream, (string)fieldValue);
                    charStream.Write(')');
                    break;
            }
        }
        /// <summary>
        /// 写入 IN 条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="connectionCreator"></param>
        /// <param name="isNot"></param>
        private void writeIn(CharStream charStream, ConnectionCreator connectionCreator, bool isNot)
        {
            Action<CharStream, object> toString = connectionCreator.GetConstantConverter(memberParameter.Field.ElementType, memberParameter.ColumnIndex.Member);
            connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
            charStream.SimpleWrite(isNot ? " not in(" : " in(");
            int index = 0;
            ICollection collection = fieldValue as ICollection;
            foreach (object value in collection)
            {
                if (index++ == 0) charStream.Write(',');
                toString(charStream, value);
            }
            charStream.Write(')');
        }
        /// <summary>
        /// 写入比较条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="connectionCreator"></param>
        /// <param name="compareTo"></param>
        private void writeCompareTo(CharStream charStream, ConnectionCreator connectionCreator, char compareTo)
        {
            connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
            charStream.Write(compareTo);
            object value = memberParameter.Field.GenericType == null ? fieldValue : memberParameter.Field.GenericType.GetNullableValue(fieldValue);
            connectionCreator.GetConstantConverter(value.GetType(), memberParameter.ColumnIndex.Member)(charStream, value);
        }
        /// <summary>
        /// 写入比较条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="connectionCreator"></param>
        /// <param name="char1"></param>
        /// <param name="char2"></param>
        private void writeCompareTo(CharStream charStream, ConnectionCreator connectionCreator, char char1, char char2)
        {
            connectionCreator.FormatName(charStream, memberParameter.ColumnIndex.ColumnName);
            charStream.Write(char1);
            charStream.Write(char2);
            object value = memberParameter.Field.GenericType == null ? fieldValue : memberParameter.Field.GenericType.GetNullableValue(fieldValue);
            connectionCreator.GetConstantConverter(value.GetType(), memberParameter.ColumnIndex.Member)(charStream, value);
        }
    }
}
