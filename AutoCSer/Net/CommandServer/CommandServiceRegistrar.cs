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
    /// Server registration component
    /// 服务注册组件
    /// </summary>
    public class CommandServiceRegistrar : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Server registration component
        /// 服务注册组件
        /// </summary>
        protected readonly CommandListenerBase server;
        /// <summary>
        /// Server registration component
        /// 服务注册组件
        /// </summary>
        /// <param name="server">Command server to listen
        /// 命令服务端监听</param>
        public CommandServiceRegistrar(CommandListenerBase server)
        {
            this.server = server;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public virtual void Dispose() { }
        /// <summary>
        /// Release resources
        /// </summary>
        public virtual ValueTask DisposeAsync()
        {
            return AutoCSer.Common.CompletedValueTask;
        }
        /// <summary>
        /// Get the server listening address
        /// 获取服务端监听地址
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
        /// Get the server listening port number
        /// 获取服务端监听端口号
        /// </summary>
        /// <returns></returns>
        public virtual Task<ushort> GetHostPort() { throw new InvalidOperationException(AutoCSer.Common.Culture.GetCommandServerNotFoundPort(server.ServerName)); }
        /// <summary>
        /// The server listener was successful
        /// 服务端监听成功
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public virtual Task OnListened(HostEndPoint endPoint) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// Notify the singleton server to go offline
        /// 通知单例服务端下线
        /// </summary>
        public virtual void Offline()
        {
            server.Offline();
        }
    }
}
