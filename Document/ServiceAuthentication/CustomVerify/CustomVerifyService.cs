using System;

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
                            return AutoCSer.Net.CommandServerVerifyStateEnum.Success;
                        }
                    }
                    else if (verifyData.Timestamp != user.Timestamp) return AutoCSer.Net.CommandServerVerifyStateEnum.Retry;
                }
            }
            return AutoCSer.Net.CommandServerVerifyStateEnum.Fail;
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
    }
}
