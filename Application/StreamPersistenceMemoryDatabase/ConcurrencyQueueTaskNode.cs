using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 并发队列节点
    /// </summary>
    public abstract class ConcurrencyQueueTaskNode : AutoCSer.Threading.Link<ConcurrencyQueueTaskNode>
    {
        /// <summary>
        /// 并发队列（相当于读写锁）
        /// </summary>
        private readonly ConcurrencyQueue queue;
        /// <summary>
        /// 并发队列节点
        /// </summary>
        /// <param name="queue">并发队列（相当于读写锁）</param>
        protected ConcurrencyQueueTaskNode(ConcurrencyQueue queue)
        {
            this.queue = queue;
        }
        /// <summary>
        /// 执行读取操作
        /// </summary>
        internal void RunTask()
        {
            try
            {
                runTask();
            }
            finally { queue.CheckNext(); }
        }
        /// <summary>
        /// 执行读取操作
        /// </summary>
        protected abstract void runTask();
    }
}
