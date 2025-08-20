using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed partial class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>
    {
        /// <summary>
        /// Client interface example
        /// 客户端接口示例
        /// </summary>
        [AllowNull]
        public IServiceControllerClientController ServiceControllerClientController { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(Service.IServiceController), typeof(IServiceControllerClientController));
            }
        }
        /// <summary>
        /// Generate the client controller encapsulation type for directly obtaining the return value
        /// 生成直接获取返回值的客户端控制器封装类型
        /// </summary>
        public override bool IsCodeGeneratorReturnValueController { get { return true; } }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }
    }
}
