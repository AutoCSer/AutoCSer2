using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IStreamPersistenceMemoryDatabaseClientSocketEvent
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
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                //IO 线程同步回调接口避免线程调度开销，必须保证客户端调用 await 后续操作无阻塞，否则可能严重影响性能甚至死锁，如果不能保证无阻塞环境请替换为 IStreamPersistenceMemoryDatabaseTaskClient 接口避免死锁
                yield return new CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(CommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        protected override Task onMethodVerified(CommandClientSocket socket)
        {
            StreamPersistenceMemoryDatabaseClient.CreateSessionObject().Discard();
            return AutoCSer.Common.CompletedTask;
        }
    }
}
