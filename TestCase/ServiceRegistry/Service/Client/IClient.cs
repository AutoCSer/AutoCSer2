using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.ServiceRegistry.Client
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
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> GetVersion();
    }
}
