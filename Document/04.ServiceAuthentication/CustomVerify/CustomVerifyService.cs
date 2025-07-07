using System;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// An example of service authentication for custom user identity authentication
    /// 自定义用户身份鉴权服务认证示例
    /// </summary>
    internal class CustomVerifyService : ICustomVerifyService
    {
        /// <summary>
        /// Verification method
        /// 验证方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="verifyData">Customize user identity authentication data
        /// 自定义用户身份鉴权数据</param>
        /// <param name="hashData">Hash data to be verified
        /// 待验证 Hash 数据</param>
        /// <returns></returns>
        async System.Threading.Tasks.Task<AutoCSer.Net.CommandServerVerifyStateEnum> ICustomVerifyService.Verify(AutoCSer.Net.CommandServerSocket socket, CustomVerifyData verifyData, byte[] hashData)
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
                            return AutoCSer.Net.CommandServerVerifyStateEnum.Success;
                        }
                    }
                    else if (verifyData.Timestamp != user.Timestamp) return AutoCSer.Net.CommandServerVerifyStateEnum.Retry;
                }
            }
            return AutoCSer.Net.CommandServerVerifyStateEnum.Fail;
        }
        /// <summary>
        /// Only access to the specified API test is allowed
        /// 仅允许访问指定 API 测试
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void setCommand(AutoCSer.Net.CommandServerSocket socket) { }
        /// <summary>
        /// Simulate the acquisition of user verification information
        /// 模拟获取用户验证信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private System.Threading.Tasks.Task<UserVerifyInfo> getUserVerifyInfo(string userName)
        {
            return Task.FromResult(new UserVerifyInfo { UserName = userName, Key = AutoCSer.TestCase.Common.Config.TimestampVerifyString, Timestamp = 0 });
        }
        /// <summary>
        /// Simulate and set the timestamp of the user's last verification
        /// 模拟设置用户最后一次验证时间戳
        /// </summary>
        /// <param name="user"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private System.Threading.Tasks.Task setUserTimestamp(UserVerifyInfo user, long timestamp)
        {
            user.Timestamp = timestamp;
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// Custom user identity authentication service authentication test
        /// 自定义用户身份鉴权服务认证测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ICustomVerifyService>(new CustomVerifyService())
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
