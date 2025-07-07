using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// The client of the registration server
    /// 注册服务客户端
    /// </summary>
    internal sealed class CommandClientServiceRegistrarLogClient : ServerRegistryLogClient
    {
        /// <summary>
        /// The server registration client listener component
        /// 服务注册客户端监听组件
        /// </summary>
        private readonly CommandClientServiceRegistrar registrar;
        /// <summary>
        /// The client callback for the registration server log
        /// 注册服务日志客户端回调
        /// </summary>
#if NetStandard21
        private CommandClientServiceRegistrarLogClientCallback? callback;
#else
        private CommandClientServiceRegistrarLogClientCallback callback;
#endif
        /// <summary>
        /// The client of the registration server
        /// 注册服务客户端
        /// </summary>
        /// <param name="registrar">The server registration client listener component
        /// 服务注册客户端监听组件</param>
        /// <param name="node">The client node for server registration
        /// 服务注册客户端节点</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        internal CommandClientServiceRegistrarLogClient(CommandClientServiceRegistrar registrar, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, string serverName) : base(node, serverName)
        {
            this.registrar = registrar;
        }
        /// <summary>
        /// Cancel the keep callback
        /// 取消保持回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Cancel()
        {
            callback?.Cancel();
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <returns></returns>
        public override Task LogCallback()
        {
            Task<ResponseResult<IServerRegistryNodeClientNode>> task = NodeCache.GetSynchronousNode();
            if (task.IsCompleted) return logCallback(task.Result);
            return logCallback(task);
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task logCallback(Task<ResponseResult<IServerRegistryNodeClientNode>> task)
        {
            await logCallback(await task);
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Task logCallback(ResponseResult<IServerRegistryNodeClientNode> node)
        {
            if (node.IsSuccess && !registrar.Client.IsDisposed)
            {
                callback?.Cancel();
                callback = new CommandClientServiceRegistrarLogClientCallback(this);
                return callback.LogCallback(node.Value.notNull());
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="log"></param>
        protected override void logCallback(ServerRegistryLog log)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="log"></param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Callback(ServerRegistryLog log, CommandClientServiceRegistrarLogClientCallback callback)
        {
            if (object.ReferenceEquals(this.callback, callback)) registrar.Callback(log);
        }
    }
}
