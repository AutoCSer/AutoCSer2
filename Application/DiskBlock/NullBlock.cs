using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 默认空磁盘块
    /// </summary>
    internal sealed class NullBlock : Block
    {
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose() { }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public override ValueTask DisposeAsync()
        {
            return AutoCSer.Common.CompletedValueTask;
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
