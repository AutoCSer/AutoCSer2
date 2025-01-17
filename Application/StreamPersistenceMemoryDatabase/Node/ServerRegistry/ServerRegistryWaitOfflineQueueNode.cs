using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 单例服务超时强制下线任务节点
    /// </summary>
    internal sealed class ServerRegistryWaitOfflineQueueNode : AutoCSer.Net.CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 服务注册日志组装
        /// </summary>
        private readonly ServerRegistryLogAssembler logAssembler;
        /// <summary>
        /// 需要下线的主服务日志
        /// </summary>
        private readonly ServerRegistrySessionLog mainLog;
        /// <summary>
        /// 等待上线的服务日志
        /// </summary>
        private readonly ServerRegistrySessionLog log;
        /// <summary>
        /// 单例服务器超时强制下线任务节点
        /// </summary>
        /// <param name="logAssembler"></param>
        /// <param name="mainLog"></param>
        /// <param name="log"></param>
        internal ServerRegistryWaitOfflineQueueNode(ServerRegistryLogAssembler logAssembler, ServerRegistrySessionLog mainLog, ServerRegistrySessionLog log)
        {
            this.logAssembler = logAssembler;
            this.mainLog = mainLog;
            this.log = log;
        }
        /// <summary>
        /// 添加超时检查队列任务
        /// </summary>
        internal void AppendQueueNode()
        {
            if (object.ReferenceEquals(mainLog, logAssembler.MainLog)) logAssembler.Node.StreamPersistenceMemoryDatabaseCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 检查主服务日志
        /// </summary>
        public override void RunTask()
        {
            logAssembler.SingletonTimeout(mainLog, log);
        }
    }
}
