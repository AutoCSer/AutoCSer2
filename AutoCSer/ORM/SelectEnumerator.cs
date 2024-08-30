using AutoCSer;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
#if DotNet45 || NetStandard2
using ValueTask = System.Threading.Tasks.Task;
#endif

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
        protected readonly Transaction transaction;
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected DbConnection connection;
        /// <summary>
        /// 查询命令
        /// </summary>
        protected DbCommand command;
        /// <summary>
        /// 数据读取器
        /// </summary>
        internal DbDataReader Reader;
        /// <summary>
        /// 出错次数
        /// </summary>
        protected int errorCount;
        /// <summary>
        /// 异步查询枚举器
        /// </summary>
        /// <param name="transaction"></param>
        protected SelectEnumerator(Transaction transaction)
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
                command = transaction.Connection.CreateCommand();
                transaction.Set(command);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <returns></returns>
        protected async Task free(ConnectionPool connectionPool)
        {
            try
            {
                if (Reader != null)
                {
                    ++errorCount;
#if DotNet45 || NetStandard2
                    Reader.Dispose();
#else
                    await Reader.DisposeAsync();
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
#if DotNet45 || NetStandard2
                        command.Dispose();
#else
                        await command.DisposeAsync();
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
                            if (errorCount == 0) await connectionPool.FreeConnection(connection);
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
    public sealed class SelectEnumerator<T, VT> : SelectEnumerator
#if DotNet45 || NetStandard2
        , IEnumeratorTask<VT>
#else
        , IAsyncEnumerator<VT>
#endif
        where T : class
        where VT : class, T
    {
        /// <summary>
        /// 数据库表格持久化
        /// </summary>
        private readonly TableWriter<T> tableWriter;
        /// <summary>
        /// 数据库表格模型 SQL 查询信息
        /// </summary>
        private readonly Query<T> query;
        /// <summary>
        /// 当前读取数据
        /// </summary>
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
        internal SelectEnumerator(TableWriter<T> tableWriter, Query<T> query, Transaction transaction) : base(transaction)
        {
            this.tableWriter = tableWriter;
            this.query = query;
        }
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if DotNet45 || NetStandard2
        async Task<bool> IEnumeratorTask.MoveNextAsync()
#else
        async ValueTask<bool> IAsyncEnumerator<VT>.MoveNextAsync()
#endif
        {
            if (query == null) return false;
            ++errorCount;
            MemberMap<T> memberMap = query.MemberMap;
            if (await Reader.ReadAsync())
            {
                VT value = DefaultConstructor<VT>.Constructor();
                tableWriter.Read(Reader, value, memberMap);
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
            query.Set(command);
            Reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult);
            --errorCount;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await free(tableWriter?.ConnectionPool);
        }
    }
}
