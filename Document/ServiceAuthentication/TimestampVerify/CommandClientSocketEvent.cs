using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Serialization;

namespace AutoCSer.Document.ServiceAuthentication.TimestampVerify
{
    /// <summary>
    /// 无身份认证（字符串匹配认证）命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent
    {
        /// <summary>
        /// 测试接口
        /// </summary>
        [AllowNull]
        public ITestServiceClientController TestService { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                foreach (CommandClientControllerCreatorParameter creatorParameter in base.ControllerCreatorParameters)
                {
                    yield return creatorParameter;
                }
                yield return new CommandClientControllerCreatorParameter(typeof(ITestService), typeof(ITestServiceClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="verifyString">服务认证验证字符串</param>
        public CommandClientSocketEvent(ICommandClient client, string verifyString) : base(client, verifyString) { }

        /// <summary>
        /// 无身份认证（字符串匹配认证）测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await client();
            }
        }
        /// <summary>
        /// 无身份认证（字符串匹配认证）客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> client()
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocketEvent? client = (CommandClientSocketEvent?)await commandClient.GetSocketEvent();
                if (client == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                var result = await client.TestService.Add(1, 2);
                if (result.Value != 1 + 2)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
