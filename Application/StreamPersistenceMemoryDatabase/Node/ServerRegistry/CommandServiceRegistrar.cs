using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// Server registration component
    /// 服务注册组件
    /// </summary>
    public abstract class CommandServiceRegistrar : AutoCSer.Net.CommandServiceRegistrar
    {
        /// <summary>
        /// Server registration node
        /// 服务注册节点
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> nodeCache;
        /// <summary>
        /// The keep callback for server registration
        /// 服务注册保持回调
        /// </summary>
#if NetStandard21
        protected volatile AutoCSer.Net.CommandKeepCallback? keepCallback;
#else
        protected volatile AutoCSer.Net.CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// Server host and port information
        /// 服务端主机与端口信息
        /// </summary>
        protected AutoCSer.Net.HostEndPoint endPoint;
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
#if NetStandard21
        protected ServerRegistryLog? serverRegistryLog;
#else
        protected ServerRegistryLog serverRegistryLog;
#endif
        /// <summary>
        /// Server session Identifier
        /// 服务会话标识ID
        /// </summary>
        protected ResponseResult<long> sessionID;
        /// <summary>
        /// Server registration log operation type
        /// 服务注册日志操作类型
        /// </summary>
        private readonly ServerRegistryOperationTypeEnum serverRegistryType;
        /// <summary>
        /// Callback version
        /// 回调版本
        /// </summary>
        protected volatile int version;
        /// <summary>
        /// Has the server registration component been added
        /// 是否已经添加服务端注册组件
        /// </summary>
        private bool isAppendRegistrar;
        /// <summary>
        /// Can the server registration component be added
        /// 是否可以添加服务端注册组件
        /// </summary>
        internal bool IsAppendRegistrar
        {
            get
            {
                if (!isAppendRegistrar)
                {
                    isAppendRegistrar = true;
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Server registration component
        /// 服务注册组件
        /// </summary>
        /// <param name="server">The server is listening for the command to be registered
        /// 待注册命令服务端监听</param>
        /// <param name="node">Server registration node cache
        /// 服务注册节点缓存</param>
        /// <param name="serverRegistryType">Register node type
        /// 注册节点类型</param>
        protected CommandServiceRegistrar(AutoCSer.Net.CommandListenerBase server, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, ServerRegistryOperationTypeEnum serverRegistryType = ServerRegistryOperationTypeEnum.Singleton) : base(server)
        {
            this.nodeCache = node;
            this.serverRegistryType = serverRegistryType;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            if (serverRegistryLog != null)
            {
                Task<ResponseResult<IServerRegistryNodeClientNode>> task = nodeCache.GetNode();
                if (task.IsCompleted)
                {
                    ResponseResult<IServerRegistryNodeClientNode> node = task.Result;
                    if (node.IsSuccess) node.Value.notNull().Append(serverRegistryLog.CreateLogout());
                }
            }
            keepCallback?.Dispose();
            base.Dispose();
        }
        /// <summary>
        /// Release resources
        /// </summary>
#if NetStandard21
        public override async ValueTask DisposeAsync()
#else
        public override async Task DisposeAsync()
#endif
        {
            if (serverRegistryLog != null)
            {
                ResponseResult<IServerRegistryNodeClientNode> node = await nodeCache.GetNode();
                if (node.IsSuccess) await node.Value.notNull().Append(serverRegistryLog.CreateLogout());
            }
            keepCallback?.Dispose();
            await base.DisposeAsync();
        }
        /// <summary>
        /// Server registration callback
        /// 服务注册回调
        /// </summary>
        /// <returns></returns>
        public virtual async Task ServerCallback()
        {
            keepCallback = null;
            ++version;
            if (!server.IsDisposed)
            {
                ResponseResult<IServerRegistryNodeClientNode> node = await nodeCache.GetNode();
                if (node.IsSuccess)
                {
                    if (!sessionID.IsSuccess) sessionID = await node.Value.notNull().GetSessionID();
                    if (sessionID.IsSuccess)
                    {
                        keepCallback = await node.Value.notNull().ServerCallback(sessionID.Value, serviceCallback);
                        if (serverRegistryLog == null && endPoint.Port != 0) serverRegistryLog = getServerRegistryLog(endPoint);
                        if (serverRegistryLog != null) await node.Value.notNull().Append(serverRegistryLog);
                    }
                }
            }
        }
        /// <summary>
        /// Server registration callback
        /// 服务注册回调
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="command"></param>
        protected virtual void serviceCallback(ResponseResult<ServerRegistryOperationTypeEnum> operationType, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (operationType.IsSuccess)
            {
                switch (operationType.Value)
                {
                    case ServerRegistryOperationTypeEnum.Offline:
                        server.Offline();
                        return;
                }
            }
        }
        /// <summary>
        /// Gets the server registration log
        /// 获取服务注册日志
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        protected virtual ServerRegistryLog getServerRegistryLog(AutoCSer.Net.HostEndPoint endPoint)
        {
            return new ServerRegistryLog(sessionID.Value, server.ServerName.notNull(), serverRegistryType, endPoint);
        }
    }
    /// <summary>
    /// Server registration component
    /// 服务注册组件
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public class CommandServiceRegistrar<T> : CommandServiceRegistrar
        where T : ServerRegistryCommandClientSocketEvent<T>
    {
        /// <summary>
        /// Command client socket event caching
        /// 命令客户端套接字事件缓存
        /// </summary>
        private readonly CommandClientSocketEventCache<T> client;
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            client.SocketEvent.Remove(this).NotWait();
            base.Dispose();
        }
        /// <summary>
        /// Release resources
        /// </summary>
#if NetStandard21
        public override async ValueTask DisposeAsync()
#else
        public override async Task DisposeAsync()
#endif
        {
            await client.SocketEvent.Remove(this);
            await base.DisposeAsync();
        }
        /// <summary>
        /// Server registration component
        /// 服务注册组件
        /// </summary>
        /// <param name="server">The server is listening for the command to be registered
        /// 待注册命令服务端监听</param>
        /// <param name="client">Command client socket event caching
        /// 命令客户端套接字事件缓存</param>
        /// <param name="node">Server registration node cache
        /// 服务注册节点缓存</param>
        /// <param name="serverRegistryType">Register node type
        /// 注册节点类型</param>
        public CommandServiceRegistrar(AutoCSer.Net.CommandListenerBase server, CommandClientSocketEventCache<T> client, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, ServerRegistryOperationTypeEnum serverRegistryType = ServerRegistryOperationTypeEnum.Singleton) : base(server, node, serverRegistryType) 
        {
            this.client = client;
        }
        /// <summary>
        /// The server listener was successful
        /// 服务监听成功
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override async Task OnListened(AutoCSer.Net.HostEndPoint endPoint)
        {
            this.endPoint = endPoint;
            int version = this.version;
            if (keepCallback == null) await client.Client.GetSocketEvent<T>();
            if (version == this.version && keepCallback != null)
            {
                serverRegistryLog = getServerRegistryLog(endPoint);
                ResponseResult<IServerRegistryNodeClientNode> node = await nodeCache.GetNode();
                if (node.IsSuccess && !server.IsDisposed && version == this.version) await node.Value.notNull().Append(serverRegistryLog);
            }
        }
    }
}
