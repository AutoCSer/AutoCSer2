using AutoCSer.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端注册组件
    /// </summary>
    public class CommandServiceRegistrar : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 命令服务
        /// </summary>
        protected readonly CommandListenerBase server;
        /// <summary>
        /// 服务注册组件
        /// </summary>
        /// <param name="server">命令服务</param>
        public CommandServiceRegistrar(CommandListenerBase server)
        {
            this.server = server;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose() { }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual ValueTask DisposeAsync()
        {
            return AutoCSer.Common.CompletedValueTask;
        }
        /// <summary>
        /// 获取服务监听地址
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public virtual async ValueTask<AutoCSer.Net.CommandServer.HostEndPoint> GetEndPoint()
#else
        public virtual async Task<AutoCSer.Net.CommandServer.HostEndPoint> GetEndPoint()
#endif
        {
            HostEndPoint host = server.Host;
            return host.Port != 0 ? (AutoCSer.Net.CommandServer.HostEndPoint)host : (AutoCSer.Net.CommandServer.HostEndPoint)host.Get(await GetHostPort());
        }
        /// <summary>
        /// 获取服务监听端口号
        /// </summary>
        /// <returns></returns>
        public virtual Task<ushort> GetHostPort() { throw new InvalidOperationException(AutoCSer.Common.Culture.GetCommandServerNotFoundPort(server.ServerName)); }
        /// <summary>
        /// 服务监听成功
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public virtual Task OnListened(HostEndPoint endPoint) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 通知单例服务下线
        /// </summary>
        public virtual void Offline()
        {
            server.Offline();
        }
    }
}
