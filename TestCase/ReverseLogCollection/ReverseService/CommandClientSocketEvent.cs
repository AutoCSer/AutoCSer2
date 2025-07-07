using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseService
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.TimestampVerifyReverseServiceCommandClientSocketEvent<CommandClientSocketEvent>, ILogCollectionReverseClientSocketEvent<LogInfo>
    {
        /// <summary>
        /// Reverse service authentication client interface based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的反向服务认证客户端接口
        /// </summary>
        public ITimestampVerifyReverseServiceClientController<string> TimestampVerifyReverseClient { get; private set; }
        /// <summary>
        /// 日志收集反向服务客户端
        /// </summary>
        public ILogCollectionReverseServiceClientController<LogInfo> LogCollectionReverseClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
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
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(CommandClient client) : base(client) { }
        /// <summary>
        /// The reverse command server client listens for the authentication socket
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
