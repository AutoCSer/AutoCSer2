using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseBackuper
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IStreamPersistenceMemoryDatabaseClientSocketEvent, IDisposable
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端从节点配置
        /// </summary>
        private readonly SlaveServiceConfig slaveServiceConfig;
        /// <summary>
        /// 日志流持久化内存数据库备份
        /// </summary>
        private Backuper backuper;
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
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="slaveServiceConfig">日志流持久化内存数据库服务端从节点配置</param>
        public CommandClientSocketEvent(ICommandClient client, SlaveServiceConfig slaveServiceConfig) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        {
            this.slaveServiceConfig = slaveServiceConfig;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            backuper?.Dispose();
        }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        protected override async Task onMethodVerified(CommandClientSocket socket)
        {
            if (backuper == null) backuper = await slaveServiceConfig.Create(this, socket);
            else socket.SessionObject = backuper;
        }
    }
}
