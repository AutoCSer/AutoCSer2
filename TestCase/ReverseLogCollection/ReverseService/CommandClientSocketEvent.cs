using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseService
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.TimestampVerifyReverseServiceCommandClientSocketEvent<CommandClientSocketEvent>, ILogCollectionReverseClientSocketEvent<LogInfo>
    {
        /// <summary>
        /// 基于递增登录时间戳验证的反向服务认证客户端接口
        /// </summary>
        public ITimestampVerifyReverseServiceClientController<string> TimestampVerifyReverseClient { get; private set; }
        /// <summary>
        /// 日志收集反向服务客户端
        /// </summary>
        public ILogCollectionReverseServiceClientController<LogInfo> LogCollectionReverseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyReverseService<string>), typeof(ITimestampVerifyReverseServiceClientController<string>));
                yield return new CommandClientControllerCreatorParameter(typeof(ILogCollectionReverseService<LogInfo>), typeof(ILogCollectionReverseServiceClientController<LogInfo>));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(CommandClient client) : base(client) { }
        /// <summary>
        /// 反向命令服务客户端监听验证套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override Task<bool> CallVerify(CommandClientSocket socket)
        {
            return callVerify(socket, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
        }
    }
}
