using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistryService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.ServiceRegistryCommandClientSocketEvent, IPortRegistryClientSocketEvent
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
        /// 端口注册客户端
        /// </summary>
        public PortRegistryClient PortRegistryClient { get; private set; }
        /// <summary>
        /// 端口注册客户端接口
        /// </summary>
        public IPortRegistryClient IPortRegistryClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IPortRegistryService), typeof(IPortRegistryClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IServiceRegistryService), typeof(IServiceRegistryClient));
            }
        }
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="config">注册服务命令客户端配置</param>
        /// <param name="verifyString">服务认证验证字符串</param>
        public CommandClientSocketEvent(ICommandClient client, ServiceRegistryCommandClientConfig config, string verifyString) : base(client, config)
        {
            this.verifyString = verifyString;
            PortRegistryClient = new PortRegistryClient(this);
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
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected override async Task onMethodVerified(CommandClientSocket socket)
        {
            await base.onMethodVerified(socket);
            PortRegistryClient.OnClientMethodVerified();
        }
    }
}
