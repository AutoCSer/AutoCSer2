using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.ServiceRegistryClient
{
    /// <summary>
    /// 命令服务注册测试接口
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// 获取当前服务测试版本
        /// </summary>
        /// <returns>服务测试版本</returns>
        ReturnCommand<uint> GetVersion();
    }
}
