using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 注册服务命令客户端套接字事件
    /// </summary>
    public class ServiceRegistryCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// 注册服务命令客户端配置
        /// </summary>
        private readonly ServiceRegistryCommandClientConfig config;
        /// <summary>
        /// 注册服务命令接口测试
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public IServiceRegistryServiceClientController ServiceRegistryClient { get; protected set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(IServiceRegistryService), typeof(IServiceRegistryServiceClientController));
            }
        }
        /// <summary>
        /// 注册服务命令客户端套接字事件
        /// </summary>
        /// <param name="commandClient">命令客户端</param>
        /// <param name="config">注册服务命令客户端配置</param>
        public ServiceRegistryCommandClientSocketEvent(ICommandClient commandClient, ServiceRegistryCommandClientConfig config) : base(commandClient)
        {
            this.config = config;
        }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected internal override async Task onMethodVerified(CommandClientSocket socket)
        {
            await config.OnMethodVerified(ServiceRegistryClient);
        }
    }
}
