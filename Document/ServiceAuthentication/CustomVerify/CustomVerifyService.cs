using AutoCSer.Net;
using System;
using System.Reflection;
using System.Security.Cryptography;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// 自定义服务认证示例
    /// </summary>
    internal class CustomVerifyService : ICustomVerifyService
    {
        /// <summary>
        /// 验证函数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="verifyData">自定义服务认证数据</param>
        /// <param name="hashData">验证 Hash 数据</param>
        /// <returns></returns>
        async Task<AutoCSer.Net.CommandServerVerifyStateEnum> ICustomVerifyService.Verify(AutoCSer.Net.CommandServerSocket socket, CustomVerifyData verifyData, byte[] hashData)
        {
            if (hashData?.Length == 16)
            {
                UserVerifyInfo user = await getUserVerifyInfo(verifyData.UserName);
                if (user != null)
                {
                    if (verifyData.Timestamp > user.Timestamp)
                    {
                        if (AutoCSer.Net.TimestampVerify.Md5Equals(verifyData.GetMd5Data(user.Key), hashData) == 0)
                        {
                            await setUserTimestamp(user, verifyData.Timestamp);
                            socket.SessionObject = user;
                            setCommand(socket);
                            return CommandServerVerifyStateEnum.Success;
                        }
                    }
                    else if (verifyData.Timestamp != user.Timestamp) return CommandServerVerifyStateEnum.Retry;
                }
            }
            return CommandServerVerifyStateEnum.Fail;
        }
        /// <summary>
        /// 仅允许访问指定 API 测试
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void setCommand(AutoCSer.Net.CommandServerSocket socket) { }
        /// <summary>
        /// 模拟获取用户验证信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private Task<UserVerifyInfo> getUserVerifyInfo(string userName)
        {
            return Task.FromResult(new UserVerifyInfo { UserName = userName, Key = AutoCSer.TestCase.Common.Config.TimestampVerifyString, Timestamp = 0 });
        }
        /// <summary>
        /// 模拟设置用户最后一次验证时间戳
        /// </summary>
        /// <param name="user"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private Task setUserTimestamp(UserVerifyInfo user, long timestamp)
        {
            user.Timestamp = timestamp;
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// 自定义服务认证测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
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
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, nameof(CustomVerifyData.UserName), AutoCSer.TestCase.Common.Config.TimestampVerifyString)
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
