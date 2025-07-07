using AutoCSer.Document.ServiceAuthentication.CustomVerify;
using System;

namespace AutoCSer.Document.ServiceAuthentication.SetCommand
{
    /// <summary>
    /// Tests that only allow access to the specified API
    /// 仅允许访问指定 API 测试
    /// </summary>
    internal class CustomVerifyService : CustomVerify.CustomVerifyService
    {
        /// <summary>
        /// Tests that only allow access to the specified API
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
        /// Only access to the specified API test is allowed
        /// 仅允许访问指定 API 测试
        /// </summary>
        /// <returns></returns>
        internal static new async Task<bool> Test()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<CustomVerify.ICustomVerifyService>(new CustomVerifyService())
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await Client.Test();
            }
        }
    }
}
