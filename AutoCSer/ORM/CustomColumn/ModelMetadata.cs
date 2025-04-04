using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoCSer.ORM.CustomColumn
{
    /// <summary>
    /// 自定义数据列模型元数据
    /// </summary>
    /// <typeparam name="T">自定义数据列类型</typeparam>
    internal static class ModelMetadata<T> where T : struct
    {
        /// <summary>
        /// 自定义数据列配置
        /// </summary>
#if NetStandard21
        internal static readonly CustomColumnAttribute? Attribute;
#else
        internal static readonly CustomColumnAttribute Attribute;
#endif
        /// <summary>
        /// 字段成员集合
        /// </summary>
        internal static readonly Member[] Members;
        /// <summary>
        /// 字段总数
        /// </summary>
        internal static readonly int MemberCount;
        /// <summary>
        /// 递归获取所有数据列名称
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal static IEnumerable<CustomColumnName> GetMemberNames(string? parentName, string? nameConcatSplit)
#else
        internal static IEnumerable<CustomColumnName> GetMemberNames(string parentName, string nameConcatSplit)
#endif
        {
            foreach (Member member in Members)
            {
                if (member.CustomColumnAttribute == null) yield return new CustomColumnName(member, member.ConcatMemberName(parentName, nameConcatSplit));
                else
                {
                    foreach (CustomColumnName name in member.GetCustomColumnMemberNames(parentName, nameConcatSplit)) yield return name;
                }
            }
        }
        /// <summary>
        /// 获取自定义列名称
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal static CustomColumnName GetMemberName(MemberExpression? memberExpression, LeftArray<MemberExpression> memberExpressions, string? parentName, string? nameConcatSplit)
#else
        internal static CustomColumnName GetMemberName(MemberExpression memberExpression, LeftArray<MemberExpression> memberExpressions, string parentName, string nameConcatSplit)
#endif
        {
            var expression = default(MemberExpression);
            if (!memberExpressions.TryPop(out expression))
            {
                expression = memberExpression;
                memberExpression = null;
            }
            if (expression == null && Members.Length != 1) throw new MemberAccessException($"不支持的查询成员定义 {typeof(T).fullName()}");
            foreach (Member member in Members)
            {
                if (expression == null || member.MemberIndex.Member == expression.Member)
                {
                    if (member.CustomColumnAttribute == null) return new CustomColumnName(member, member.ConcatMemberName(parentName, nameConcatSplit));
                    return member.GetCustomColumnMemberName(memberExpression, ref memberExpressions, parentName, nameConcatSplit);
                }
            }
            throw new MemberAccessException($"没有找到成员定义 {expression.notNull().Member.Name}");
        }
        /// <summary>
        /// 递归匹配自定义数据列获取数值
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <param name="value"></param>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal static IEnumerable<KeyValue<CustomColumnName, object?>> GetMemberNameValues(MemberExpression? memberExpression, LeftArray<MemberExpression> memberExpressions, object value, string? parentName, string? nameConcatSplit)
#else
        internal static IEnumerable<KeyValue<CustomColumnName, object>> GetMemberNameValues(MemberExpression memberExpression, LeftArray<MemberExpression> memberExpressions, object value, string parentName, string nameConcatSplit)
#endif
        {
            var expression = default(MemberExpression);
            if (!memberExpressions.TryPop(out expression))
            {
                expression = memberExpression;
                memberExpression = null;
            }
            if (expression == null)
            {
                foreach (Member member in Members)
                {
                    var memberVaue = member.MemberIndex.GetValue(value);
                    if (member.CustomColumnAttribute == null) yield return KeyValue.From(new CustomColumnName(member, member.ConcatMemberName(parentName, nameConcatSplit)), memberVaue);
                    else
                    {
                        foreach (var nameValue in member.GetCustomColumnMemberNameValues(memberExpression, ref memberExpressions, memberVaue.notNull(), parentName, nameConcatSplit))
                        {
                            yield return nameValue;
                        }
                    }
                }
            }
            else
            {
                bool isMember = false;
                foreach (Member member in Members)
                {
                    if (member.MemberIndex.Member == expression.Member)
                    {
                        isMember = true;
                        if (member.CustomColumnAttribute == null)
                        {
#if NetStandard21
                            yield return new KeyValue<CustomColumnName, object?>(new CustomColumnName(member, member.ConcatMemberName(parentName, nameConcatSplit)), value);
#else
                            yield return new KeyValue<CustomColumnName, object>(new CustomColumnName(member, member.ConcatMemberName(parentName, nameConcatSplit)), value);
#endif
                        }
                        else
                        {
                            foreach (var nameValue in member.GetCustomColumnMemberNameValues(memberExpression, ref memberExpressions, value, parentName, nameConcatSplit))
                            {
                                yield return nameValue;
                            }
                        }
                        break;
                    }
                }
                if (!isMember) throw new MemberAccessException($"没有找到成员定义 {expression.Member.Name}");
            }
        }
        ///// <summary>
        ///// 根据名称获取成员
        ///// </summary>
        ///// <param name="memberName"></param>
        ///// <returns></returns>
        //internal static Member GetMember(string memberName)
        //{
        //    foreach (Member member in Members)
        //    {
        //        if (member.MemberIndex.Member.Name == memberName) return member;
        //    }
        //    return null;
        //}

        static ModelMetadata()
        {
            Attribute = typeof(T).GetCustomAttribute<CustomColumnAttribute>(true);
            if (Attribute != null)
            {
                LeftArray<Member> members = Member.Get(MemberIndexGroup.GetFields(typeof(T), Attribute.MemberFilters), MemberIndexGroup.GetProperties(typeof(T), Attribute.MemberFilters), false);
                members.Sort(Member.Sort);
                Members = members.ToArray();
                if (Members.Length == 0) Attribute = null;
                else
                {
                    MemberCount = Members.Length;
                    foreach (Member member in Members)
                    {
                        if (member.CustomColumnAttribute != null) MemberCount += member.StructGenericType.CustomColumnMemberCount - 1;
                    }
                }
            }
            else Members = EmptyArray<Member>.Array;
        }
    }
}
