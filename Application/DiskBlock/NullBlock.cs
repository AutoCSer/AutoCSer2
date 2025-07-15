using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 默认空磁盘块
    /// </summary>
    internal sealed class NullBlock : Block
    {
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose() { }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public override ValueTask DisposeAsync()
#else
        public override Task DisposeAsync()
#endif
        {
            return AutoCSer.Common.AsyncDisposableCompletedTask;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns>磁盘块当前写入位置</returns>
        internal override Task<long> Write(WriteRequest request)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns>磁盘块当前写入位置</returns>
        public override Task<long> Flush()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context">获取读取数据上下文</param>
        /// <returns></returns>
        protected override Task read(ReadRequest request, object context)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 删除磁盘块
        /// </summary>
        /// <returns>是否删除成功</returns>
        internal override Task<bool> Delete()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 默认空磁盘块
        /// </summary>
        internal static readonly NullBlock Null = new NullBlock();
    }
}
