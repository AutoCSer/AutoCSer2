using System;

namespace AutoCSer.TestCase.ServiceRegistryService
{
    /// <summary>
    /// 命令服务注册测试接口
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 获取当前服务测试版本
        /// </summary>
        /// <returns>服务测试版本</returns>
        uint GetVersion();
    }
    /// <summary>
    /// 命令服务注册测试接口
    /// </summary>
    internal sealed class Service : IService
    {
        /// <summary>
        /// 当前服务测试版本
        /// </summary>
        private readonly uint currentVersion;
        /// <summary>
        /// 命令服务注册测试接口
        /// </summary>
        /// <param name="currentVersion">当前服务测试版本</param>
        internal Service(uint currentVersion)
        {
            this.currentVersion = currentVersion;
        }
        /// <summary>
        /// 获取当前服务测试版本
        /// </summary>
        /// <returns>服务测试版本</returns>
        uint IService.GetVersion() { return currentVersion; }
    }
}
