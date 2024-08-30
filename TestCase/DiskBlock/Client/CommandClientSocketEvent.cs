using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DiskBlockClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IDiskBlockClientSocketEvent
    {
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        public IDiskBlockClient DiskBlockClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                //IO 线程同步回调接口避免线程调度开销，必须保证客户端调用 await 后续操作无阻塞，否则可能严重影响性能甚至死锁，如果不能保证无阻塞环境请替换为 IStreamPersistenceMemoryDatabaseTaskClient 接口避免死锁
                yield return new CommandClientControllerCreatorParameter(typeof(IDiskBlockService), typeof(IDiskBlockClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(CommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
    }
}
