using AutoCSer.Extensions;
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库事务
    /// </summary>
    public sealed class Transaction : IDisposable, IAsyncDisposable
    {
#if !DotNet45
        /// <summary>
        /// 异步上下文
        /// </summary>
#if NetStandard21
        internal static readonly AsyncLocal<Transaction?> AsyncLocal = new AsyncLocal<Transaction?>();
#else
        internal static readonly AsyncLocal<Transaction> AsyncLocal = new AsyncLocal<Transaction>();
#endif
        /// <summary>
        /// 获取异步上下文数据库事务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Transaction? GetAsyncLocal()
#else
        public static Transaction GetAsyncLocal()
#endif
        {
            return AsyncLocal.Value;
        }
        ///// <summary>
        ///// 提交当前异步上下文数据库事务
        ///// </summary>
        ///// <returns></returns>
        //public static Task AsyncLocalCommitAsync()
        //{
        //    Transaction transaction = AsyncLocal.Value;
        //    if (transaction != null) await transaction.CommitAsync();
        //    else throw new NullReferenceException("没有找到异步上下文数据库事务");
        //}
#endif

        /// <summary>
        /// 数据库连接池
        /// </summary>
        internal readonly ConnectionPool ConnectionPool;
        /// <summary>
        /// 数据库连接
        /// </summary>
#if NetStandard21
        internal DbConnection? Connection;
#else
        internal DbConnection Connection;
#endif
        /// <summary>
        /// 数据库事务
        /// </summary>
#if NetStandard21
        private DbTransaction? transaction;
#else
        private DbTransaction transaction;
#endif
        /// <summary>
        /// 事务提交事件集合
        /// </summary>
        internal LeftArray<TransactionCommited> Commiteds;
        /// <summary>
        /// 数据库事务
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">数据库事务</param>
        /// <param name="isAsyncLocal">是否创建异步上下文</param>
        internal Transaction(ConnectionPool connectionPool, DbConnection connection, DbTransaction transaction, bool isAsyncLocal)
        {
            ConnectionPool = connectionPool;
            this.Connection = connection;
            this.transaction = transaction;
            Commiteds = new LeftArray<TransactionCommited>(0);
#if !DotNet45
            if (isAsyncLocal) AsyncLocal.Value = this;
#endif
        }
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal DbCommand CreateCommand(string statement)
        {
            return ConnectionPool.CreateCommand(Connection.notNull(), statement);
        }
        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(DbCommand command)
        {
            command.Transaction = transaction;
        }
        /// <summary>
        /// 获取并清除数据库连接
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal DbConnection? ClearConnection()
#else
        internal DbConnection ClearConnection()
#endif
        {
            var connection = this.Connection;
            this.Connection = null;
            return connection;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            if (transaction != null)
            {
                try
                {
#if NetStandard21
                    await transaction.CommitAsync();
#else
                    transaction.Commit();
#endif
                    transaction = null;
                    try
                    {
                        await ConnectionPool.FreeConnectionAsync(this);
                    }
                    finally
                    {
                        foreach (TransactionCommited commited in Commiteds) await commited.OnCommited();
                        Commiteds.SetEmpty();
                    }
                }
                finally
                {
                    var transaction = Interlocked.Exchange(ref this.transaction, null);
#if NetStandard21
                    if (transaction != null) await transaction.DisposeAsync();
#else
                    if (transaction != null) transaction.Dispose();
#endif
                    var connection = Interlocked.Exchange(ref this.Connection, null);
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
        }
        /// <summary>
        /// 释放事务，回滚未提交事务
        /// </summary>
        public void Dispose()
        {
            try
            {
                var transaction = Interlocked.Exchange(ref this.transaction, null);
                if (transaction != null)
                {
                    using (transaction) transaction.Rollback();
                }
                ConnectionPool.FreeConnection(this);
            }
            finally
            {
#if !DotNet45
                if (object.ReferenceEquals(AsyncLocal.Value, this)) AsyncLocal.Value = null;
#endif
                var connection = Interlocked.Exchange(ref this.Connection, null);
                ConnectionCreator.CloseConnection(connection);
            }
        }
        /// <summary>
        /// 释放事务，回滚未提交事务
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            try
            {
                var transaction = Interlocked.Exchange(ref this.transaction, null);
                if (transaction != null)
                {
#if NetStandard21
                    await using (transaction) await transaction.RollbackAsync();
#else
                    using (transaction) transaction.Rollback();
#endif
                }
                await ConnectionPool.FreeConnectionAsync(this);
            }
            finally
            {
#if !DotNet45
                if (object.ReferenceEquals(AsyncLocal.Value, this)) AsyncLocal.Value = null;
#endif
                var connection = Interlocked.Exchange(ref this.Connection, null);
                if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
            }
        }
    }
}
