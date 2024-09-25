using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 从节点客户端信息回调
    /// </summary>
    internal sealed class ServiceSlaveCallback : QueueTaskNode
    {
        /// <summary>
        /// 从节点客户端信息
        /// </summary>
        private readonly ServiceSlave slave;
        /// <summary>
        /// 从节点客户端信息回调类型
        /// </summary>
        private readonly ServiceSlaveCallbackTypeEnum callbackType;
        /// <summary>
        /// 从节点客户端信息回调
        /// </summary>
        /// <param name="slave">从节点客户端信息</param>
        /// <param name="callbackType">从节点客户端信息回调类型</param>
        internal ServiceSlaveCallback(ServiceSlave slave, ServiceSlaveCallbackTypeEnum callbackType)
        {
            this.slave = slave;
            this.callbackType = callbackType;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            switch (callbackType)
            {
                case ServiceSlaveCallbackTypeEnum.Remove: slave.RemoveCallback(); return;
                case ServiceSlaveCallbackTypeEnum.CheckPersistenceCallbackExceptionPosition: slave.CheckPersistenceCallbackExceptionPositionCallback(); return;
            }
        }
    }
}
