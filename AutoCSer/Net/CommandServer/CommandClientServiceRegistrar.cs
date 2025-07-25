﻿using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The server registration client listener component
    /// 服务注册客户端监听组件
    /// </summary>
    public class CommandClientServiceRegistrar : IDisposable
    {
        /// <summary>
        /// Command client
        /// </summary>
        public readonly ICommandClient Client;
        /// <summary>
        /// The server registration client listener component
        /// 服务注册客户端监听组件
        /// </summary>
        /// <param name="client"></param>
        public CommandClientServiceRegistrar(ICommandClient client)
        {
            this.Client = client;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public virtual void Dispose() { }
        /// <summary>
        /// Get the server listening address
        /// 获取服务端监听地址
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public virtual Task<IPEndPoint?> GetServerEndPoint()
#else
        public virtual Task<IPEndPoint> GetServerEndPoint()
#endif
        {
            return AutoCSer.Common.GetCompletedTask(Client.Host.IPEndPoint);
        }
        /// <summary>
        /// Server connection failed
        /// 服务连接失败
        /// </summary>
        /// <param name="endPoint"></param>
        public virtual Task ConnectFail(IPEndPoint endPoint) { return AutoCSer.Common.CompletedTask; }
    }
}
