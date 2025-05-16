using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端同步读写队列空节点
    /// </summary>
    internal sealed class NullReadWriteQueueNode : ReadWriteQueueNode
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask() { }
        /// <summary>
        /// 空节点
        /// </summary>
        internal NullReadWriteQueueNode() : base() { }

        /// <summary>
        /// 空节点
        /// </summary>
        internal static readonly NullReadWriteQueueNode Null = new NullReadWriteQueueNode();
    }
}
