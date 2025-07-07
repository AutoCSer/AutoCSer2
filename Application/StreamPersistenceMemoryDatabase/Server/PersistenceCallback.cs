using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库回调操作
    /// </summary>
    internal sealed class PersistenceCallback : ReadWriteQueueNode
    {
        /// <summary>
        /// The written location of the persistent stream
        /// 持久化流已写入位置
        /// </summary>
        internal long PersistencePosition;
        /// <summary>
        /// 持久化回调头节点
        /// </summary>
        private readonly MethodParameter head;
        /// <summary>
        /// 持久化回调尾节点
        /// </summary>
#if NetStandard21
        private readonly MethodParameter? end;
#else
        private readonly MethodParameter end;
#endif
        /// <summary>
        /// 持久化文件是否需要重建
        /// </summary>
        internal bool CheckRebuild;
        /// <summary>
        /// 日志流持久化内存数据库回调操作
        /// </summary>
        /// <param name="head">持久化回调头节点</param>
        /// <param name="end">持久化回调尾节点</param>
#if NetStandard21
        internal PersistenceCallback(MethodParameter head, MethodParameter? end)
#else
        internal PersistenceCallback(MethodParameter head, MethodParameter end)
#endif
        {
            this.head = head;
            this.end = end;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            head.Node.NodeCreator.Service.PersistenceCallback(head, end, PersistencePosition, CheckRebuild);
        }
    }
}
