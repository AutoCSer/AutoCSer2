using System;

namespace AutoCSer.TestCase.ServerRegistryService
{
    /// <summary>
    /// 命令服务注册测试接口
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 获取当前服务端口号
        /// </summary>
        /// <returns>服务端口号</returns>
        ushort GetPort();
    }
}
