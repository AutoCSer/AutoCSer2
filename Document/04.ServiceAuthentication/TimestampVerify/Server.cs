using System;

namespace AutoCSer.Document.ServiceAuthentication.TimestampVerify
{
    /// <summary>
    /// The test without identity authentication (string matching authentication)
    /// 无身份认证测试（字符串匹配认证）
    /// </summary>
    internal static class Server
    {
        /// <summary>
        /// The test without identity authentication (string matching authentication)
        /// 无身份认证测试（字符串匹配认证）
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await CommandClientSocketEvent.Test();
            }
        }
    }
}
