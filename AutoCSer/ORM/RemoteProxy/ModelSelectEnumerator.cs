using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 异步查询枚举器
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class ModelSelectEnumerator<T> : IAsyncEnumerator<T>
        where T : class
    {
        /// <summary>
        /// RPC 远程代理访问命令
        /// </summary>
        private readonly AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand;
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
        /// <param name="enumeratorCommand"></param>
        public ModelSelectEnumerator(AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand)
        {
            this.enumeratorCommand = enumeratorCommand;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        async ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
#else
        async Task<bool> IAsyncEnumerator<T>.MoveNextAsync()
#endif
        {
            if (await enumeratorCommand.MoveNext())
            {
                DataRow dataRow = enumeratorCommand.Current;
                if (columnIndexs == null) columnIndexs = ModelReader<T>.GetColumnIndexCache(dataRow.Columns.notNull());
                T value = DefaultConstructor<T>.Constructor().notNull();
                if (columnIndexs.Length != 0) ModelReader<T>.Reader(dataRow.Row, value, columnIndexs);
                Current = value;
                return true;
            }
            if (enumeratorCommand.ReturnType != AutoCSer.Net.CommandClientReturnTypeEnum.Success) throw new InvalidOperationException($"返回值类型错误 {enumeratorCommand.ReturnType}");
            return false;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public ValueTask DisposeAsync()
#else
        public Task DisposeAsync()
#endif
        {
            var columnIndexs = Interlocked.Exchange(ref this.columnIndexs, null);
            if (columnIndexs != null) ModelReader<T>.FreeColumnIndexCache(columnIndexs);
            return AutoCSer.Common.AsyncDisposableCompletedTask;
        }
    }
}
