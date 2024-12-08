using System;

namespace AutoCSer.Document.ServiceAuthentication
{
    /// <summary>
    /// 测试示例
    /// </summary>
    internal sealed class TestService : ITestService
    {
        /// <summary>
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ITestService.Add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 不允许访问的 API
        /// </summary>
        void ITestService.NotSetCommand()
        {
        }
    }
}
