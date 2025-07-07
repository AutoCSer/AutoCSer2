using System;

namespace AutoCSer.Document.ServiceAuthentication
{
    /// <summary>
    /// Test service example
    /// 测试服务示例
    /// </summary>
    internal sealed class TestService : ITestService
    {
        /// <summary>
        /// Test service API
        /// 测试服务 API
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ITestService.Add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// Api that are not allowed to be accessed (for the specified API testing of the user identity-based authentication mode)
        /// 不允许访问的 API（用于基于用户身份的鉴权模式的指定 API 测试）
        /// </summary>
        void ITestService.NotSetCommand()
        {
        }
    }
}
