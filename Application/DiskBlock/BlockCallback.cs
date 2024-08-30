using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块操作回调操作
    /// </summary>
    internal sealed class BlockCallback : CommandServerCallQueueNode
    {
        /// <summary>
        /// 磁盘块
        /// </summary>
        private readonly Block blockManager;
        /// <summary>
        /// 读取数据请求信息
        /// </summary>
        private readonly ReadRequest readRequest;
        /// <summary>
        /// 操作类型
        /// </summary>
        private readonly BlockCallbackTypeEnum type;
        /// <summary>
        /// 磁盘块队列回调操作
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="blockManager">磁盘块</param>
        /// <param name="readRequest">读取数据请求信息</param>
        internal BlockCallback(BlockCallbackTypeEnum type, Block blockManager, ReadRequest readRequest = null)
        {
            this.type = type;
            this.blockManager = blockManager;
            this.readRequest = readRequest;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            switch (type)
            {
                case BlockCallbackTypeEnum.Flush: blockManager.FlushCallback(); return;
                case BlockCallbackTypeEnum.Read: blockManager.ReadCallback(readRequest); return;
                case BlockCallbackTypeEnum.Dispose: blockManager.ServiceDisposeCallback(); return;
            }
        }
    }
}
