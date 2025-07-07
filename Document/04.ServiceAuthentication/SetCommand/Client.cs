using System;

namespace AutoCSer.Document.ServiceAuthentication.SetCommand
{
    /// <summary>
    /// Client testing of only allow access to the specified API
    /// 仅允许访问指定 API 客户端测试
    /// </summary>
    internal static class Client
    {
        /// <summary>
        /// Client singleton
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CustomVerify.CommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<CustomVerify.CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CustomVerify.CommandClientSocketEvent(client, nameof(AutoCSer.Document.ServiceAuthentication.CustomVerify.CustomVerifyData.UserName), AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        });
        /// <summary>
        /// Client testing of only allow access to the specified API
        /// 仅允许访问指定 API 客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            CustomVerify.CommandClientSocketEvent? client = await CommandClient.SocketEvent.Wait();
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
            return true;
        }
    }
}
