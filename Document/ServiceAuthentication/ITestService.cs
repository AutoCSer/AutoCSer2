using System;

namespace AutoCSer.Document.ServiceAuthentication
{
    /// <summary>
    /// 测试接口定义
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public interface ITestService
    {
        /// <summary>
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);

        /// <summary>
        /// 不允许访问的 API
        /// </summary>
        void NotSetCommand();
    }
}
