using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册客户端套接字事件
    /// </summary>
    public interface IPortRegistryClientSocketEvent
    {
        /// <summary>
        /// 端口注册客户端
        /// </summary>
        PortRegistryClient PortRegistryClient { get; }
        /// <summary>
        /// 端口注册客户端接口
        /// </summary>
        IPortRegistryServiceClientController IPortRegistryClient { get; }
    }
}
