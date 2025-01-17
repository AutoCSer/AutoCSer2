using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent
    {
        /// <summary>
        /// 反向日志收集服务客户端
        /// </summary>
        public IReverseLogCollectionClient<LogInfo> ReverseLogCollectionClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IReverseLogCollectionService<LogInfo>), typeof(IReverseLogCollectionClient<LogInfo>));
            }
        }
        /// <summary>
        /// </summary>
        /// 命令客户端套接字事件
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
    }
}
