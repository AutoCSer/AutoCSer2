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
    internal sealed class CommandClientSocketEventTaskClient : TimestampVerifyCommandClientSocketEvent, IDiskBlockClientSocketEvent
    {
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        public IDiskBlockClient DiskBlockClient { get; private set; }
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        public IDiskBlockTaskClient IDiskBlockTaskClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IDiskBlockService), typeof(IDiskBlockTaskClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEventTaskClient(ICommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override Task OnSetController(CommandClientSocket socket)
        {
            DiskBlockClient = new DiskBlockTaskClient(IDiskBlockTaskClient);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
