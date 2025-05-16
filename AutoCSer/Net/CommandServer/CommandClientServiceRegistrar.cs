using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务注册客户端监听组件
    /// </summary>
    public class CommandClientServiceRegistrar : IDisposable
    {
        /// <summary>
        /// 命令客户端
        /// </summary>
        public readonly ICommandClient Client;
        /// <summary>
        /// 服务注册客户端监听组件
        /// </summary>
        /// <param name="client"></param>
        public CommandClientServiceRegistrar(ICommandClient client)
        {
            this.Client = client;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose() { }
        /// <summary>
        /// 获取服务监听地址
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public virtual ValueTask<IPEndPoint?> GetServerEndPoint()
#else
        public virtual Task<IPEndPoint> GetServerEndPoint()
#endif
        {
#if NetStandard21
#pragma warning disable CS8619
#if NET8
            return ValueTask.FromResult(Client.Host.IPEndPoint);
#else
            return AutoCSer.Common.GetCompletedValueTask(Client.Host.IPEndPoint);
#endif
#pragma warning restore CS8619
#else
            return AutoCSer.Common.GetCompletedTask(Client.Host.IPEndPoint);
#endif
        }
        /// <summary>
        /// 服务连接失败
        /// </summary>
        /// <param name="endPoint"></param>
        public virtual Task ConnectFail(IPEndPoint endPoint) { return AutoCSer.Common.CompletedTask; }
    }
}
