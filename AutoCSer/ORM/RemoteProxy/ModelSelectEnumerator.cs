using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#if DotNet45 || NetStandard2
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 异步查询枚举器
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class ModelSelectEnumerator<T>
#if DotNet45 || NetStandard2
        : IEnumeratorTask<T>
#else
        : IAsyncEnumerator<T>
#endif
        where T : class
    {
        /// <summary>
        /// RPC 远程代理访问命令
        /// </summary>
        private readonly AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand;
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
        /// <param name="enumeratorCommand"></param>
        public ModelSelectEnumerator(AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand)
        {
            this.enumeratorCommand = enumeratorCommand;
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
            if (await enumeratorCommand.MoveNext())
            {
                DataRow dataRow = enumeratorCommand.Current;
                if (columnIndexs == null) columnIndexs = ModelReader<T>.GetColumnIndexCache(dataRow.Columns);
                T value = DefaultConstructor<T>.Constructor();
                if (columnIndexs.Length != 0) ModelReader<T>.Reader(dataRow.Row, value, columnIndexs);
                Current = value;
                return true;
            }
            if (enumeratorCommand.ReturnType != AutoCSer.Net.CommandClientReturnTypeEnum.Success) throw new InvalidOperationException($"返回值类型错误 {enumeratorCommand.ReturnType}");
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            int[] columnIndexs = Interlocked.Exchange(ref this.columnIndexs, null);
            if (columnIndexs != null) ModelReader<T>.FreeColumnIndexCache(columnIndexs);
            return AutoCSer.Common.CompletedTask.ToValueTask();
        }
    }
}
