using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.ServiceAuthentication.TimestampVerify
{
    /// <summary>
    /// No identity authentication (string matching authentication) command client socket event
    /// 无身份认证（字符串匹配认证）命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.TimestampVerifyCommandClientSocketEvent<CommandClientSocketEvent>
    {
        /// <summary>
        /// Test the client interface
        /// 测试客户端接口
        /// </summary>
        [AllowNull]
        public ITestServiceClientController TestService { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                foreach (AutoCSer.Net.CommandClientControllerCreatorParameter creatorParameter in base.ControllerCreatorParameters)
                {
                    yield return creatorParameter;
                }
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(ITestService), typeof(ITestServiceClientController));
            }
        }
        /// <summary>
        /// Command client socket event controller property binding identification
        /// 命令客户端套接字事件控制器属性绑定标识
        /// </summary>
        public override System.Reflection.BindingFlags ControllerBindingFlags { get { return System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public; } }
        /// <summary>
        /// No identity authentication (string matching authentication) command client socket event
        /// 无身份认证（字符串匹配认证）命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="verifyString">Verify string
        /// 验证字符串</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client, string verifyString) : base(client, verifyString) { }

        /// <summary>
        /// Client singleton
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        });
        /// <summary>
        /// Client testing without identity authentication (string matching authentication)
        /// 无身份认证（字符串匹配认证）客户端测试
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Test()
        {
            CommandClientSocketEvent? client = await CommandClient.SocketEvent.Wait();
            if (client == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            var result = await client.TestService.Add(1, 2);
            if (result.Value != 1 + 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
