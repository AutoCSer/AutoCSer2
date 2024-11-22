using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.LogCollectionReverseService
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent, ILogCollectionReverseClientSocketEvent<LogInfo>
    {
        /// <summary>
        /// 基于递增登录时间戳验证的反向服务认证客户端接口
        /// </summary>
        public ITimestampVerifyReverseClient<string> TimestampVerifyReverseClient { get; private set; }
        /// <summary>
        /// 日志收集反向服务客户端
        /// </summary>
        public ILogCollectionReverseClient<LogInfo> LogCollectionReverseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyReverseService<string>), typeof(ITimestampVerifyReverseClient<string>));
                yield return new CommandClientControllerCreatorParameter(typeof(ILogCollectionReverseService<LogInfo>), typeof(ILogCollectionReverseClient<LogInfo>));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(CommandClient client) : base(client) { }
    }
}
