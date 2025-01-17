using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Net;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// 服务端注册组件
    /// </summary>
    public abstract class CommandServiceRegistrar : AutoCSer.Net.CommandServiceRegistrar
    {
        /// <summary>
        /// 服务注册节点
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> nodeCache;
        /// <summary>
        /// 服务注册保持回调
        /// </summary>
#if NetStandard21
        protected volatile AutoCSer.Net.CommandKeepCallback? keepCallback;
#else
        protected volatile AutoCSer.Net.CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// 服务主机与端口信息
        /// </summary>
        protected AutoCSer.Net.HostEndPoint endPoint;
        /// <summary>
        /// 服务注册日志
        /// </summary>
#if NetStandard21
        protected ServerRegistryLog? serverRegistryLog;
#else
        protected ServerRegistryLog serverRegistryLog;
#endif
        /// <summary>
        /// 服务会话标识ID
        /// </summary>
        protected ResponseResult<long> sessionID;
        /// <summary>
        /// 注册节点类型
        /// </summary>
        private readonly ServerRegistryOperationTypeEnum serverRegistryType;
        /// <summary>
        /// 回调版本
        /// </summary>
        protected volatile int version;
        /// <summary>
        /// 是否已经添加服务端注册组件
        /// </summary>
        private bool isAppendRegistrar;
        /// <summary>
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
        /// 服务端注册组件
        /// </summary>
        /// <param name="server">待注册命令服务</param>
        /// <param name="node">服务注册节点缓存</param>
        /// <param name="serverRegistryType">注册节点类型</param>
        protected CommandServiceRegistrar(AutoCSer.Net.CommandListenerBase server, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, ServerRegistryOperationTypeEnum serverRegistryType = ServerRegistryOperationTypeEnum.Singleton) : base(server)
        {
            this.nodeCache = node;
            this.serverRegistryType = serverRegistryType;
        }
        /// <summary>
        /// 释放资源
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
        /// 释放资源
        /// </summary>
        public override async ValueTask DisposeAsync()
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
        /// 服务注册回调委托
        /// </summary>
        /// <returns></returns>
        public virtual async Task ServiceCallback()
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
                        keepCallback = await node.Value.notNull().ServiceCallback(sessionID.Value, serviceCallback);
                        if (serverRegistryLog == null && endPoint.Port != 0) serverRegistryLog = getServerRegistryLog(endPoint);
                        if (serverRegistryLog != null) await node.Value.notNull().Append(serverRegistryLog);
                    }
                }
            }
        }
        /// <summary>
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
    /// 服务端注册组件
    /// </summary>
    /// <typeparam name="T">客户端套接字事件类型</typeparam>
    public class CommandServiceRegistrar<T> : CommandServiceRegistrar
        where T : ServerRegistryCommandClientSocketEvent<T>
    {
        /// <summary>
        /// 命令客户端套接字事件缓存
        /// </summary>
        private readonly CommandClientSocketEventCache<T> client;
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            client.SocketEvent.Remove(this).NotWait();
            base.Dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override async ValueTask DisposeAsync()
        {
            await client.SocketEvent.Remove(this);
            await base.DisposeAsync();
        }
        /// <summary>
        /// 服务端注册组件
        /// </summary>
        /// <param name="server">待注册命令服务</param>
        /// <param name="client">命令客户端套接字事件缓存</param>
        /// <param name="node">服务注册节点缓存</param>
        /// <param name="serverRegistryType">注册节点类型</param>
        public CommandServiceRegistrar(AutoCSer.Net.CommandListenerBase server, CommandClientSocketEventCache<T> client, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, ServerRegistryOperationTypeEnum serverRegistryType = ServerRegistryOperationTypeEnum.Singleton) : base(server, node, serverRegistryType) 
        {
            this.client = client;
        }
        /// <summary>
        /// 服务监听成功
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override async Task OnListened(AutoCSer.Net.HostEndPoint endPoint)
        {
            this.endPoint = endPoint;
            int version = this.version;
            if (keepCallback == null) await client.Client.GetSocketAsync();
            if (version == this.version && keepCallback != null)
            {
                serverRegistryLog = getServerRegistryLog(endPoint);
                ResponseResult<IServerRegistryNodeClientNode> node = await nodeCache.GetNode();
                if (node.IsSuccess && !server.IsDisposed && version == this.version) await node.Value.notNull().Append(serverRegistryLog);
            }
        }
    }
}
