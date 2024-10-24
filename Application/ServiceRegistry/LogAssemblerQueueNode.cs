﻿using AutoCSer.CommandService.ServiceRegistry;
using System;

namespace AutoCSer.CommandService.ServiceRegister
{
    /// <summary>
    /// 单例服务器超时强制下线任务节点
    /// </summary>
    internal sealed class LogAssemblerQueueNode : AutoCSer.Net.CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 服务注册日志组装
        /// </summary>
        private readonly LogAssembler logAssembler;
        /// <summary>
        /// 需要下线的主服务日志
        /// </summary>
        private readonly SessionLog mainLog;
        /// <summary>
        /// 单例服务器超时强制下线任务节点
        /// </summary>
        /// <param name="logAssembler"></param>
        internal LogAssemblerQueueNode(LogAssembler logAssembler)
        {
            this.logAssembler = logAssembler;
            mainLog = logAssembler.MainLog;
        }
        /// <summary>
        /// 添加超时检查队列任务
        /// </summary>
        internal void AppendQueueNode()
        {
            if (object.ReferenceEquals(mainLog, logAssembler.MainLog)) logAssembler.ServiceRegistry.Controller.AddQueue(this);
        }
        /// <summary>
        /// 检查主服务日志
        /// </summary>
        public override void RunTask()
        {
            if (object.ReferenceEquals(mainLog, logAssembler.MainLog)) logAssembler.SingletonTimeout();
        }
    }
}
