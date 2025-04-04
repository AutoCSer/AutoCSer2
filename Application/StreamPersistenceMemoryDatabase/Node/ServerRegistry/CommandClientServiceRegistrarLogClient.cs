﻿using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// 注册服务客户端
    /// </summary>
    internal sealed class CommandClientServiceRegistrarLogClient : ServerRegistryLogClient
    {
        /// <summary>
        /// 服务注册客户端监听组件
        /// </summary>
        private readonly CommandClientServiceRegistrar registrar;
        /// <summary>
        /// 注册服务客户端日志回调
        /// </summary>
#if NetStandard21
        private CommandClientServiceRegistrarLogClientCallback? callback;
#else
        private CommandClientServiceRegistrarLogClientCallback callback;
#endif
        /// <summary>
        /// 注册服务客户端
        /// </summary>
        /// <param name="registrar">服务注册客户端监听组件</param>
        /// <param name="node">服务注册节点</param>
        /// <param name="serverName">服务名称</param>
        internal CommandClientServiceRegistrarLogClient(CommandClientServiceRegistrar registrar, StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, string serverName) : base(node, serverName)
        {
            this.registrar = registrar;
        }
        /// <summary>
        /// 取消回调操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Cancel()
        {
            callback?.Cancel();
        }
        /// <summary>
        /// 服务日志回调委托
        /// </summary>
        /// <returns></returns>
        public override Task LogCallback()
        {
            Task<ResponseResult<IServerRegistryNodeClientNode>> task = NodeCache.GetSynchronousNode();
            if (task.IsCompleted) return logCallback(task.Result);
            return logCallback(task);
        }
        /// <summary>
        /// 服务日志回调委托
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task logCallback(Task<ResponseResult<IServerRegistryNodeClientNode>> task)
        {
            await logCallback(await task);
        }
        /// <summary>
        /// 服务日志回调委托
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
        /// 服务日志回调
        /// </summary>
        /// <param name="log"></param>
        protected override void logCallback(ServerRegistryLog log)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 服务日志回调
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
