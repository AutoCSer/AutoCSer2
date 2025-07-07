using System;

namespace AutoCSer.Document.ServiceAuthentication
{
    /// <summary>
    /// Test the service interface definition (Generate the client interface code)
    /// 测试服务接口定义（生成客户端接口代码）
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface ITestService
    {
        /// <summary>
        /// Test the service API definition
        /// 测试服务 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);

        /// <summary>
        /// Api that are not allowed to be accessed (for the specified API testing of the user identity-based authentication mode)
        /// 不允许访问的 API（用于基于用户身份的鉴权模式的指定 API 测试）
        /// </summary>
        void NotSetCommand();
    }
}
