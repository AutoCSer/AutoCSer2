using AutoCSer.Memory;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 扩展查询 SQL 信息
    /// </summary>
    public abstract class ExtensionQueryBuilder : IQueryBuilder
    {
        /// <summary>
        /// 扩展查询 SQL 信息
        /// </summary>
        internal ExtensionQueryData ExtensionData;
        /// <summary>
        ///  获取查询语句
        /// </summary>
        public abstract string Statement { get; }
        /// <summary>
        /// 是否需要查询
        /// </summary>
        public abstract bool IsQuery { get; }
        /// <summary>
        /// 扩展查询 SQL 信息
        /// </summary>
        internal ExtensionQueryBuilder()
        {
            ExtensionData.SetEmpty();
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <param name="charStream"></param>
        public abstract void GetStatement(CharStream charStream);
    }
    /// <summary>
    /// 扩展查询 SQL 信息
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class ExtensionQueryBuilder<T> : ExtensionQueryBuilder where T : class
    {
        /// <summary>
        /// SQL 查询创建器
        /// </summary>
        public readonly QueryBuilder<T> QueryBuilder;
        /// <summary>
        /// 是否需要查询
        /// </summary>
        public override bool IsQuery { get { return QueryBuilder.IsQuery; } }
        /// <summary>
        ///  获取查询语句
        /// </summary>
        public override string Statement { get { return GetStatement(0, false); } }
        /// <summary>
        /// 扩展查询 SQL 信息
        /// </summary>
        /// <param name="builder">SQL 查询创建器</param>
        internal ExtensionQueryBuilder(QueryBuilder<T> builder)
        {
            QueryBuilder = builder;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static implicit operator ExtensionQueryBuilder<T>(QueryBuilder<T> builder) { return new ExtensionQueryBuilder<T>(builder); }
        /// <summary>
        /// 清除查询信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ExtensionQueryBuilder<T> Clear()
        {
            ExtensionData.SetEmpty();
            return this;
        }
        /// <summary>
        /// 添加 GROUP BY 子项
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <param name="queryName">添加查询名称，默认为 null 表示不添加到查询</param>
        /// <returns></returns>
#if NetStandard21
        public ExtensionQueryBuilder<T> GroupBy<VT>(Expression<Func<T, VT>> member, string? queryName = null)
#else
        public ExtensionQueryBuilder<T> GroupBy<VT>(Expression<Func<T, VT>> member, string queryName = null)
#endif
        {
            ExtensionData.GroupBy(QueryBuilder.TableWriter.ConvertIsSimple(member), queryName);
            return this;
        }
        /// <summary>
        /// 添加 HAVING 条件子项
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ExtensionQueryBuilder<T> Having(Expression<Func<T, bool>> expression)
        {
            ExtensionData.Havings.Add(QueryBuilder.TableWriter.ConvertIsSimple(expression));
            return this;
        }
        /// <summary>
        /// 添加查询名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ExtensionQueryBuilder<T> Query(string member)
        {
            ExtensionData.QueryNames.Add(QueryBuilder.TableWriter.TryFormatColumnName(member));
            return this;
        }
        /// <summary>
        /// 添加查询名称
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <param name="queryName">查询返回列名称</param>
        /// <returns></returns>
#if NetStandard21
        public ExtensionQueryBuilder<T> Query<VT>(Expression<Func<T, VT>> member, string? queryName = null)
#else
        public ExtensionQueryBuilder<T> Query<VT>(Expression<Func<T, VT>> member, string queryName = null)
#endif
        {
            string memberName = QueryBuilder.TableWriter.ConvertIsSimple(member);
            if (string.IsNullOrEmpty(queryName)) ExtensionData.QueryNames.Add(memberName);
            else ExtensionData.QueryNames.Add($"{memberName} as {queryName}");
            return this;
        }
        /// <summary>
        /// 添加函数调用查询项
        /// </summary>
        /// <param name="member"></param>
        /// <param name="queryName"></param>
        /// <param name="method"></param>
#if NetStandard21
        private void call(string member, string? queryName, string method)
#else
        private void call(string member, string queryName, string method)
#endif
        {
            if (QueryBuilder.TableWriter.IsColumnName(member))
            {
                string formatName = QueryBuilder.TableWriter.ConnectionPool.Creator.FormatName(member);
                ExtensionData.QueryNames.Add($"{method}({formatName}) as {queryName ?? formatName}");
            }
            else if (string.IsNullOrEmpty(queryName)) ExtensionData.QueryNames.Add($"{method}({member})");
            else ExtensionData.QueryNames.Add($"{method}({member}) as {queryName}");
        }
        /// <summary>
        /// 添加 COUNT 查询项
        /// </summary>
        /// <param name="member">统计字段默认为 *</param>
        /// <param name="queryName">查询返回列名称，默认 null 表格表示和查询列名称一致</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ExtensionQueryBuilder<T> QueryCount(string member = "*", string? queryName = null)
#else
        public ExtensionQueryBuilder<T> QueryCount(string member = "*", string queryName = null)
#endif
        {
            call(member, queryName, "count");
            return this;
        }
        /// <summary>
        /// 添加 SUM 查询项
        /// </summary>
        /// <param name="member">求和字段</param>
        /// <param name="queryName">查询返回列名称，默认 null 表格表示和查询列名称一致</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ExtensionQueryBuilder<T> QuerySum(string member, string? queryName = null)
#else
        public ExtensionQueryBuilder<T> QuerySum(string member, string queryName = null)
#endif
        {
            call(member, queryName, "sum");
            return this;
        }
        /// <summary>
        /// 添加 MAX 查询项
        /// </summary>
        /// <param name="member">取最大值字段</param>
        /// <param name="queryName">查询返回列名称，默认 null 表格表示和查询列名称一致</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ExtensionQueryBuilder<T> QueryMax(string member, string? queryName = null)
#else
        public ExtensionQueryBuilder<T> QueryMax(string member, string queryName = null)
#endif
        {
            call(member, queryName, "max");
            return this;
        }
        /// <summary>
        /// 添加 MIN 查询项
        /// </summary>
        /// <param name="member">取最小值字段</param>
        /// <param name="queryName">查询返回列名称，默认 null 表格表示和查询列名称一致</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ExtensionQueryBuilder<T> QueryMin(string member, string? queryName = null)
#else
        public ExtensionQueryBuilder<T> QueryMin(string member, string queryName = null)
#endif
        {
            call(member, queryName, "min");
            return this;
        }
        /// <summary>
        /// 清除查询列名称集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ExtensionQueryBuilder<T> ClearQueryNames()
        {
            ExtensionData.QueryNames.ClearLength();
            return this;
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <param name="charStream"></param>
        public override void GetStatement(CharStream charStream)
        {
            QueryBuilder.TableWriter.ConnectionPool.Creator.GetQueryStatement(QueryBuilder, ref ExtensionData, 0, false);
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="isSubQuery">如果是子查询则在前后增加小括号</param>
        /// <returns></returns>
        public string GetStatement(int readCount = 0, bool isSubQuery = false)
        {
            CharStream charStream = QueryBuilder.TableWriter.ConnectionPool.Creator.GetCharStreamCache();
            try
            {
                QueryBuilder.TableWriter.ConnectionPool.Creator.GetQueryStatement(QueryBuilder, ref ExtensionData, (uint)Math.Max(readCount, 0), isSubQuery);
                return charStream.ToString();
            }
            finally { QueryBuilder.TableWriter.ConnectionPool.Creator.FreeCharStreamCache(charStream); }
        }
    }
}
