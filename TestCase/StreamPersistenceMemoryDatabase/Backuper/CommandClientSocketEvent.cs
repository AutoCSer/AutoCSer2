using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseBackuper
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IStreamPersistenceMemoryDatabaseClientSocketEvent, IDisposable
    {
        /// <summary>
        /// 日志流持久化内存数据库服务从节点配置
        /// </summary>
        private readonly SlaveServiceConfig slaveServiceConfig;
        /// <summary>
        /// 日志流持久化内存数据库备份
        /// </summary>
        private Backuper backuper;
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
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
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="slaveServiceConfig">日志流持久化内存数据库服务从节点配置</param>
        public CommandClientSocketEvent(ICommandClient client, SlaveServiceConfig slaveServiceConfig) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        {
            this.slaveServiceConfig = slaveServiceConfig;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        void IDisposable.Dispose()
        {
            backuper?.Dispose();
        }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override async Task OnMethodVerified(CommandClientSocket socket)
        {
            if (backuper == null) backuper = await slaveServiceConfig.Create(this, socket);
            else socket.SessionObject = backuper;
        }
    }
}
