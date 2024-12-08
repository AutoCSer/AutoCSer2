using AutoCSer.Document.ServiceAuthentication.CustomVerify;
using AutoCSer.Net;
using System;

namespace AutoCSer.Document.ServiceAuthentication.SetCommand
{
    /// <summary>
    /// 仅允许访问指定 API 测试
    /// </summary>
    internal class CustomVerifyService : CustomVerify.CustomVerifyService
    {
        /// <summary>
        /// 仅允许访问指定 API 测试
        /// </summary>
        /// <param name="socket"></param>
        protected override void setCommand(AutoCSer.Net.CommandServerSocket socket) 
        {
            UserVerifyInfo? user = (UserVerifyInfo?)socket.SessionObject;
            if (user?.UserName == nameof(CustomVerifyData.UserName))
            {
                socket.SetCommand(nameof(ITestService.Add), socket.Server.GetController(typeof(ITestService)));
            }
        }

        /// <summary>
        /// 自定义服务认证测试
        /// </summary>
        /// <returns></returns>
        internal static new async Task<bool> Test()
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append<ICustomVerifyService>(new CustomVerifyService())
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
        /// 自定义服务认证客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> client()
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
                GetSocketEventDelegate = (client) => new CustomVerify.CommandClientSocketEvent(client, nameof(CustomVerifyData.UserName), AutoCSer.TestCase.Common.Config.TimestampVerifyString)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CustomVerify.CommandClientSocketEvent? client = (CustomVerify.CommandClientSocketEvent?)await commandClient.GetSocketEvent();
                if (client == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                var result = await client.TestService.Add(1, 2);
                if (result.Value != 1 + 2)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                var returnType = await client.TestService.NotSetCommand();
                if (returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
