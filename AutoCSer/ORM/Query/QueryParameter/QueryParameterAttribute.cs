using AutoCSer.ORM.QueryParameter;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 查询参数配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// 查询匹配类型
        /// </summary>
        public QueryMatchTypeEnum MatchType;
        /// <summary>
        /// OperationName 为 null 并且 MatchType 为默认值时是否检查前后缀
        /// </summary>
        public bool CheckPrefixSuffix = true;
        /// <summary>
        /// 检查空值
        /// </summary>
        public bool CheckNull = true;
        /// <summary>
        /// 查询条件顺序
        /// </summary>
        public int Index = int.MaxValue;
        /// <summary>
        /// 数据库表格字段名称，null 表示查询参数属性名称
        /// </summary>
#if NetStandard21
        public string? TableMemberName;
#else
        public string TableMemberName;
#endif
        /// <summary>
        /// 是否检查前后缀
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetCheckPrefixSuffix()
        {
            return object.ReferenceEquals(this, Default) || (CheckPrefixSuffix && TableMemberName == null && MatchType == QueryMatchTypeEnum.Default);
        }

        /// <summary>
        /// 默认查询参数属性
        /// </summary>
        internal static readonly QueryParameterAttribute Default = new QueryParameterAttribute { MatchType = QueryMatchTypeEnum.Equal };
        /// <summary>
        /// 默认查询参数属性
        /// </summary>
        internal static readonly QueryParameterAttribute GreaterOrEqual = new QueryParameterAttribute { MatchType = QueryMatchTypeEnum.GreaterOrEqual };
        /// <summary>
        /// 默认查询参数属性
        /// </summary>
        internal static readonly QueryParameterAttribute Less = new QueryParameterAttribute { MatchType = QueryMatchTypeEnum.Less };
        /// <summary>
        /// 默认查询参数属性
        /// </summary>
        internal static readonly QueryParameterAttribute In = new QueryParameterAttribute { MatchType = QueryMatchTypeEnum.In };
    }
}
