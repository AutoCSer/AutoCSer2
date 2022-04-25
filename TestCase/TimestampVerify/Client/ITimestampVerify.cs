using System;

namespace AutoCSer.TestCase.TimestampVerify
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证示例接口
    /// </summary>
    public interface ITimestampVerify : AutoCSer.CommandService.ITimestampVerifyService
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
}
