﻿using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }
    }
}