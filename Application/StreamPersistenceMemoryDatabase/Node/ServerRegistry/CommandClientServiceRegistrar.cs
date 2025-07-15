using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// The server registration client listener component
    /// 服务注册客户端监听组件
    /// </summary>
    public abstract class CommandClientServiceRegistrar : AutoCSer.Net.CommandClientServiceRegistrar
    {
        /// <summary>
        /// Current main log
        /// 当前主日志
        /// </summary>
#if NetStandard21
        protected ServerRegistryLog? log;
#else
        protected ServerRegistryLog log;
#endif
        /// <summary>
        /// The current listening IP address
        /// 当前监听 IP 地址
        /// </summary>
        protected IPEndPoint endPoint;
        /// <summary>
        /// Has the server listening address been obtained
        /// 是否已经获取服务监听地址
        /// </summary>
        protected bool isGetServerEndPoint;
        /// <summary>
        /// The server registration client listener component
        /// 服务注册客户端监听组件
        /// </summary>
        /// <param name="client"></param>
        protected CommandClientServiceRegistrar(CommandClient client) : base(client)
        {
            endPoint = CommandServerConfigBase.NullIPEndPoint;
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="log"></param>
        internal void Callback(ServerRegistryLog log)
        {
            if (isGetServerEndPoint)
            {
                if (this.log == null || this.log.SessionID != log.SessionID)
                {
                    endPoint = log.HostEndPoint.IPEndPoint;
                    this.log = log;
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(logChanged);
                    return;
                }
            }
            else this.log = log;
        }
        /// <summary>
        /// The server listens for address update notifications
        /// 服务监听地址更新通知
        /// </summary>
        private void logChanged()
        {
            Client.ServerEndPointChanged(endPoint);
        }
    }
    /// <summary>
    /// The server registration client listener component
    /// 服务注册客户端监听组件
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public class CommandClientServiceRegistrar<T> : CommandClientServiceRegistrar
        where T : ServerRegistryLogCommandClientSocketEvent<T>
    {
        /// <summary>
        /// Client socket event
        /// 客户端套接字事件
        /// </summary>
        private readonly T socket;
        /// <summary>
        /// The client of the registration server
        /// 注册服务客户端
        /// </summary>
        private readonly CommandClientServiceRegistrarLogClient logClient;
        /// <summary>
        /// The server registration client listener component
        /// 服务注册客户端监听组件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="socket"></param>
        /// <param name="node"></param>
        public CommandClientServiceRegistrar(CommandClient client, T socket, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node) : base(client)
        {
            this.socket = socket;
            logClient = new CommandClientServiceRegistrarLogClient(this, node, client.ServerName.notNull());
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            socket.Remove(logClient).NotWait();
            logClient.Cancel();
            base.Dispose();
        }
        /// <summary>
        /// Get the server listening address
        /// 获取服务端监听地址
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public override async Task<IPEndPoint?> GetServerEndPoint()
#else
        public override async Task<IPEndPoint> GetServerEndPoint()
#endif
        {
            isGetServerEndPoint = true;
            try
            {
                ResponseResult<IServerRegistryNodeClientNode> node = await logClient.NodeCache.GetNode();
                if (node.IsSuccess)
                {
                    if (endPoint != null) return endPoint;
                    ResponseResult<ServerRegistryLog> log = await node.Value.notNull().GetLog(Client.ServerName.notNull());
                    if (log.Value != null)
                    {
                        endPoint = log.Value.HostEndPoint.IPEndPoint;
                        this.log = log.Value;
                        return endPoint;
                    }
                }
            }
            finally
            {
                if (!logClient.IsAppendClient) await socket.Append(logClient);
            }
            return null;
        }
        /// <summary>
        /// Server connection failed
        /// 服务连接失败
        /// </summary>
        /// <param name="endPoint"></param>
        public override async Task ConnectFail(IPEndPoint endPoint)
        {
            if (object.ReferenceEquals(this.endPoint, endPoint))
            {
                var log = this.log.notNull();
                if (log != null)
                {
                    ResponseResult<IServerRegistryNodeClientNode> node = await logClient.NodeCache.GetNode();
                    if (node.IsSuccess) node.Value.notNull().Check(log.SessionID, log.ServerName).Discard();
                }
            }
        }
    }
}
