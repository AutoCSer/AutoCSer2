using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.ORM
{
    /// <summary>
    /// 异步查询枚举器
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class ModelSelectEnumerator<T> : SelectEnumerator
#if NetStandard21
    , IAsyncEnumerator<T>
#else
    , IEnumeratorTask<T>
#endif
        where T : class
    {
        /// <summary>
        /// 数据库连接池
        /// </summary>
        private readonly ConnectionPool connectionPool;
        /// <summary>
        /// 数据列索引集合
        /// </summary>
#if NetStandard21
        private int[]? columnIndexs;
#else
        private int[] columnIndexs;
#endif
        /// <summary>
        /// 当前读取数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Current { get; private set; }
        /// <summary>
        /// 异步查询枚举器
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <param name="transaction"></param>
#if NetStandard21
        internal ModelSelectEnumerator(ConnectionPool connectionPool, Transaction? transaction) : base(transaction)
#else
        internal ModelSelectEnumerator(ConnectionPool connectionPool, Transaction transaction) : base(transaction)
#endif
        {
            this.connectionPool = connectionPool;
        }
        /// <summary>
        /// 获取数据读取器
        /// </summary>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <returns></returns>
        internal async Task GetReader(string statement, int timeoutSeconds)
        {
            await getReader(connectionPool);
            var command = this.command.notNull();
            ConnectionPool.SetCommand(command, statement);
            if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
            Reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult);
            --errorCount;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        async ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
#else
        async Task<bool> IEnumeratorTask.MoveNextAsync()
#endif
        {
            ++errorCount;
            var reader = Reader.notNull();
            if (await reader.ReadAsync())
            {
                T value = DefaultConstructor<T>.Constructor().notNull();
                if (columnIndexs == null) columnIndexs = ModelReader<T>.GetColumnIndexCache(reader);
                if (columnIndexs.Length != 0) ModelReader<T>.Reader(reader, value, columnIndexs);
                Current = value;
                --errorCount;
                return true;
            }
            --errorCount;
            return false;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            var columnIndexs = Interlocked.Exchange(ref this.columnIndexs, null);
            if (columnIndexs != null) ModelReader<T>.FreeColumnIndexCache(columnIndexs);

            await free(connectionPool);
        }
    }
}
