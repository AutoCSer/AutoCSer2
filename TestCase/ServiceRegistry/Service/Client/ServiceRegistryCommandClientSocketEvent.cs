using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistryClient
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证客户端套接字事件
    /// </summary>
    internal sealed class ServiceRegistryCommandClientSocketEvent : AutoCSer.CommandService.ServiceRegistryCommandClientSocketEvent
    {
        /// <summary>
        /// 服务认证验证字符串
        /// </summary>
        private readonly string verifyString;
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IServiceRegistryService), typeof(IServiceRegistryClient));
            }
        }
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="config">注册服务命令客户端配置</param>
        /// <param name="verifyString">服务认证验证字符串</param>
        public ServiceRegistryCommandClientSocketEvent(ICommandClient client, ServiceRegistryCommandClientConfig config, string verifyString) : base(client, config)
        {
            this.verifyString = verifyString;
        }
        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, verifyString));
        }
        /// <summary>
        /// 没有找到服务端控制器名称通知
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override async Task NotFoundControllerName(CommandClientSocket socket, string controllerName)
        {
            if (controllerName != typeof(AutoCSer.CommandService.IPortRegistryService).fullName()) await base.NotFoundControllerName(socket, controllerName);
        }
    }
}
