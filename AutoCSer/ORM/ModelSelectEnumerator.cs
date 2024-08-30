using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
#if DotNet45 || NetStandard2
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.ORM
{
    /// <summary>
    /// 异步查询枚举器
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class ModelSelectEnumerator<T> : SelectEnumerator
#if DotNet45 || NetStandard2
    , IEnumeratorTask<T>
#else
    , IAsyncEnumerator<T>
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
        private int[] columnIndexs;
        /// <summary>
        /// 当前读取数据
        /// </summary>
        public T Current { get; private set; }
        /// <summary>
        /// 异步查询枚举器
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <param name="transaction"></param>
        internal ModelSelectEnumerator(ConnectionPool connectionPool, Transaction transaction) : base(transaction)
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
            ConnectionPool.SetCommand(command, statement);
            if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
            Reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult);
            --errorCount;
        }
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if DotNet45 || NetStandard2
        async Task<bool> IEnumeratorTask.MoveNextAsync()
#else
        async ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
#endif
        {
            ++errorCount;
            if (await Reader.ReadAsync())
            {
                T value = DefaultConstructor<T>.Constructor();
                if (columnIndexs == null) columnIndexs = ModelReader<T>.GetColumnIndexCache(Reader);
                if (columnIndexs.Length != 0) ModelReader<T>.Reader(Reader, value, columnIndexs);
                Current = value;
                --errorCount;
                return true;
            }
            --errorCount;
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            int[] columnIndexs = Interlocked.Exchange(ref this.columnIndexs, null);
            if (columnIndexs != null) ModelReader<T>.FreeColumnIndexCache(columnIndexs);

            await free(connectionPool);
        }
    }
}
