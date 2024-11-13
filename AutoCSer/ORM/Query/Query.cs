using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库表格模型 SQL 查询信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Query<T> : IQueryBuilder
        where T : class
    {
        /// <summary>
        /// SQL 语句
        /// </summary>
#if NetStandard21
        public string? Statement { get; private set; }
#else
        public string Statement { get; private set; }
#endif
        /// <summary>
        /// 查询成员位图
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal readonly MemberMap<T> MemberMap;
        /// <summary>
        /// 读取记录数量，0 表示不限制
        /// </summary>
        internal readonly int ReadCount;
        /// <summary>
        /// 查询命令超时秒数，0 表示不设置为默认值
        /// </summary>
        internal readonly int TimeoutSeconds;
        /// <summary>
        /// 是否需要查询
        /// </summary>
        bool IQueryBuilder.IsQuery { get { return Statement != null; } }
        /// <summary>
        /// 数据库表格模型 SQL 查询信息
        /// </summary>
        private Query() { }
        /// <summary>
        /// 数据库表格模型 SQL 查询信息
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="memberMap"></param>
        /// <param name="readCount"></param>
        /// <param name="timeoutSeconds"></param>
        internal Query(string statement, MemberMap<T> memberMap, int readCount, int timeoutSeconds)
        {
            Statement = statement;
            MemberMap = memberMap;
            ReadCount = readCount;
            TimeoutSeconds = timeoutSeconds;
        }
        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="charStream"></param>
        void IQueryBuilder.GetStatement(CharStream charStream)
        {
            charStream.SimpleWrite(Statement.notNull());
        }
        ///// <summary>
        ///// 数据库表格模型 SQL 查询信息
        ///// </summary>
        //internal Query() { }
        ///// <summary>
        ///// 数据库表格模型 SQL 查询信息
        ///// </summary>
        ///// <param name="queryData">查询 SQL 信息</param>
        //internal Query(ref QueryData<T> queryData)
        //{
        //    Data = queryData;
        //}
        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(DbCommand command)
        {
            ConnectionPool.SetCommand(command, Statement.notNull());
            if (TimeoutSeconds > 0) command.CommandTimeout = TimeoutSeconds;
        }

        /// <summary>
        /// 查询条件为 false
        /// </summary>
        internal static readonly Query<T> Null = new Query<T>();
    }
}
