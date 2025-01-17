using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// 服务注册客户端监听组件
    /// </summary>
    public abstract class CommandClientServiceRegistrar : AutoCSer.Net.CommandClientServiceRegistrar
    {
        /// <summary>
        /// 当前主日志
        /// </summary>
#if NetStandard21
        protected ServerRegistryLog? log;
#else
        protected ServerRegistryLog log;
#endif
        /// <summary>
        /// 是否已经获取服务监听地址
        /// </summary>
        protected bool isGetServerEndPoint;
        /// <summary>
        /// 服务注册客户端监听组件
        /// </summary>
        /// <param name="client"></param>
        protected CommandClientServiceRegistrar(ICommandClient client) : base(client) { }
        /// <summary>
        /// 服务日志回调
        /// </summary>
        /// <param name="log"></param>
        internal void Callback(ServerRegistryLog log)
        {
            if (isGetServerEndPoint)
            {
                if (this.log == null || this.log.SessionID != log.SessionID)
                {
                    this.log = log;
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(logChanged);
                    return;
                }
            }
            else this.log = log;
        }
        /// <summary>
        /// 服务端监听地址更新通知
        /// </summary>
        private void logChanged()
        {
            Client.ServerEndPointChanged(this.log.notNull().HostEndPoint.IPEndPoint);
        }
    }
    /// <summary>
    /// 服务注册客户端监听组件
    /// </summary>
    /// <typeparam name="T">客户端套接字事件类型</typeparam>
    public class CommandClientServiceRegistrar<T> : CommandClientServiceRegistrar
        where T : ServerRegistryLogCommandClientSocketEvent<T>
    {
        /// <summary>
        /// 客户端套接字事件
        /// </summary>
        private readonly T socket;
        /// <summary>
        /// 注册服务客户端
        /// </summary>
        private readonly CommandClientServiceRegistrarLogClient logClient;
        /// <summary>
        /// 服务注册客户端监听组件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="socket"></param>
        /// <param name="node"></param>
        public CommandClientServiceRegistrar(ICommandClient client, T socket, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node) : base(client)
        {
            this.socket = socket;
            logClient = new CommandClientServiceRegistrarLogClient(this, node, client.ServerName.notNull());
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            socket.Remove(logClient).NotWait();
            logClient.Cancel();
            base.Dispose();
        }
        /// <summary>
        /// 获取服务监听地址
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public override async ValueTask<IPEndPoint?> GetServerEndPoint()
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
                    if (this.log != null) return this.log.HostEndPoint.IPEndPoint;
                    ResponseResult<ServerRegistryLog> log = await node.Value.notNull().GetLog(Client.ServerName.notNull());
                    if (log.Value != null)
                    {
                        this.log = log.Value;
                        return this.log.HostEndPoint.IPEndPoint;
                    }
                }
            }
            finally
            {
                if (!logClient.IsAppendClient) await socket.Append(logClient);
            }
            return null;
        }
    }
}
