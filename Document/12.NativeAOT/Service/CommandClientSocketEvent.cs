using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// RPC client instance
    /// RPC 客户端实例
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>
    {
        /// <summary>
        /// Example of service controller client interface 
        /// 服务控制器客户端接口示例
        /// </summary>
        [AllowNull]
        public IServiceControllerClientController ServiceController { get; private set; }
        /// <summary>
        /// Client controller creates a set of parameters used to command client socket initialization to create client controller objects and to command client socket events to automatically bind controller properties based on the client controller interface type after passing the authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(IServiceController), typeof(IServiceControllerClientController));
            }
        }
        /// <summary>
        /// RPC client instance
        /// RPC 客户端实例
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }

        /// <summary>
        /// Client singleton
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
        });
    }
}
