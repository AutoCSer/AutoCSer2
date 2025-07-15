using AutoCSer;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 异步查询枚举器
    /// </summary>
    public abstract class SelectEnumerator
    {
        /// <summary>
        /// 数据库事务
        /// </summary>
#if NetStandard21
        protected readonly Transaction? transaction;
#else
        protected readonly Transaction transaction;
#endif
        /// <summary>
        /// 数据库连接
        /// </summary>
#if NetStandard21
        protected DbConnection? connection;
#else
        protected DbConnection connection;
#endif
        /// <summary>
        /// 查询命令
        /// </summary>
#if NetStandard21
        protected DbCommand? command;
#else
        protected DbCommand command;
#endif
        /// <summary>
        /// 数据读取器
        /// </summary>
#if NetStandard21
        internal DbDataReader? Reader;
#else
        internal DbDataReader Reader;
#endif
        /// <summary>
        /// 出错次数
        /// </summary>
        protected int errorCount;
        /// <summary>
        /// 异步查询枚举器
        /// </summary>
        /// <param name="transaction"></param>
#if NetStandard21
        protected SelectEnumerator(Transaction? transaction)
#else
        protected SelectEnumerator(Transaction transaction)
#endif
        {
            if (transaction != null)
            {
                this.transaction = transaction;
                connection = transaction.Connection;
            }
        }
        /// <summary>
        /// 获取数据读取器
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <returns></returns>
        protected async Task getReader(ConnectionPool connectionPool)
        {
            if (transaction == null)
            {
                connection = connectionPool.GetConnection() ?? await connectionPool.CreateConnection();
                ++errorCount;
                command = connection.CreateCommand();
            }
            else
            {
                ++errorCount;
                command = transaction.Connection.notNull().CreateCommand();
                transaction.Set(command);
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <returns></returns>
#if NetStandard21
        protected async Task free(ConnectionPool? connectionPool)
#else
        protected async Task free(ConnectionPool connectionPool)
#endif
        {
            try
            {
                if (Reader != null)
                {
                    ++errorCount;
#if NetStandard21
                    await Reader.DisposeAsync();
#else
                    Reader.Dispose();
#endif
                    Reader = null;
                    --errorCount;
                }
            }
            finally
            {
                try
                {
                    if (command != null)
                    {
                        ++errorCount;
#if NetStandard21
                        await command.DisposeAsync();
#else
                        command.Dispose();
#endif
                        command = null;
                        --errorCount;
                    }
                }
                finally
                {
                    if (connection != null)
                    {
                        if (transaction == null)
                        {
                            if (errorCount == 0 && connectionPool != null) await connectionPool.FreeConnection(connection);
                            else await ConnectionCreator.CloseConnectionAsync(connection);
                        }
                        connection = null;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 异步查询枚举器
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="VT">枚举返回数据类型</typeparam>
    public sealed class SelectEnumerator<T, VT> : SelectEnumerator, IAsyncEnumerator<VT>
        where T : class
        where VT : class, T
    {
        /// <summary>
        /// 数据库表格持久化
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly TableWriter<T> tableWriter;
        /// <summary>
        /// 数据库表格模型 SQL 查询信息
        /// </summary>
#if NetStandard21
        private readonly Query<T>? query;
#else
        private readonly Query<T> query;
#endif
        /// <summary>
        /// 当前读取数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public VT Current { get; private set; }
        /// <summary>
        /// 异步查询枚举器
        /// </summary>
        internal SelectEnumerator() : base(null) { }
        /// <summary>
        /// 异步查询枚举器
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
#if NetStandard21
        internal SelectEnumerator(TableWriter<T> tableWriter, Query<T> query, Transaction? transaction) : base(transaction)
#else
        internal SelectEnumerator(TableWriter<T> tableWriter, Query<T> query, Transaction transaction) : base(transaction)
#endif
        {
            this.tableWriter = tableWriter;
            this.query = query;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        async ValueTask<bool> IAsyncEnumerator<VT>.MoveNextAsync()
#else
        async Task<bool> IAsyncEnumerator<VT>.MoveNextAsync()
#endif
        {
            if (query == null) return false;
            ++errorCount;
            MemberMap<T> memberMap = query.MemberMap;
            if (await Reader.notNull().ReadAsync())
            {
                VT value = DefaultConstructor<VT>.Constructor().notNull();
                tableWriter.Read(Reader.notNull(), value, memberMap);
                Current = value;
                --errorCount;
                return true;
            }
            --errorCount;
            return false;
        }
        /// <summary>
        /// 获取数据读取器
        /// </summary>
        /// <returns></returns>
        internal async Task GetReader()
        {
            await getReader(tableWriter.ConnectionPool);
            query.notNull().Set(command.notNull());
            Reader = await command.notNull().ExecuteReaderAsync(CommandBehavior.SingleResult);
            --errorCount;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public async ValueTask DisposeAsync()
#else
        public async Task DisposeAsync()
#endif
        {
            await free(tableWriter?.ConnectionPool);
        }
    }
}
