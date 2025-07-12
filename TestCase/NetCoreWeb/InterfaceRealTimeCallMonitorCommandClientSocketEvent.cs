using AutoCSer.CommandService;
using AutoCSer.CommandService.InterfaceRealTimeCallMonitor;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    /// 接口实时调用监视服务客户端套接字事件
    /// </summary>
    internal sealed class InterfaceRealTimeCallMonitorCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<InterfaceRealTimeCallMonitorCommandClientSocketEvent>, IInterfaceRealTimeCallMonitorClientSocketEvent
    {
        /// <summary>
        /// Service authentication client interface based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 接口实时调用监视服务客户端接口
        /// </summary>
        public IInterfaceRealTimeCallMonitorServiceClientController InterfaceRealTimeCallMonitor { get; private set; }
        /// <summary>
        /// 接口监视服务在线检查保持回调
        /// </summary>
        private AutoCSer.Net.CommandKeepCallback checkCommandKeepCallback;
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IInterfaceRealTimeCallMonitorService), typeof(IInterfaceRealTimeCallMonitorServiceClientController));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public InterfaceRealTimeCallMonitorCommandClientSocketEvent(CommandClient client) : base(client) { }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, AutoCSer.TestCase.Common.Config.TimestampVerifyString));
        }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override async Task OnMethodVerified(CommandClientSocket socket)
        {
            checkCommandKeepCallback = await InterfaceRealTimeCallMonitor.Check(KeepCallbackCommand.NullCallback);
            await base.OnMethodVerified(socket);
        }

        /// <summary>
        /// Client singleton
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<InterfaceRealTimeCallMonitorCommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<InterfaceRealTimeCallMonitorCommandClientSocketEvent>(new AutoCSer.Net.CommandClientCompressConfig
        {
            Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(AutoCSer.TestCase.Common.CommandServerPortEnum.InterfaceRealTimeCallMonitor),
            GetSocketEventDelegate = (client) => new InterfaceRealTimeCallMonitorCommandClientSocketEvent(client)
        });
    }
}
