using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// 注册服务客户端日志回调
    /// </summary>
    internal sealed class CommandClientServiceRegistrarLogClientCallback
    {
        /// <summary>
        /// 注册服务客户端
        /// </summary>
        private readonly CommandClientServiceRegistrarLogClient client;
        /// <summary>
        /// 服务日志保持回调
        /// </summary>
#if NetStandard21
        private AutoCSer.Net.CommandKeepCallback? keepCallback;
#else
        private AutoCSer.Net.CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// 当前主日志
        /// </summary>
#if NetStandard21
        private ServerRegistryLog? mainLog;
#else
        private ServerRegistryLog mainLog;
#endif
        /// <summary>
        /// 附加日志集合
        /// </summary>
        private LeftArray<ServerRegistryLog> logs;
        /// <summary>
        /// 是否已经加载完初始数据
        /// </summary>
        private bool isLoaded;
        /// <summary>
        /// 注册服务客户端日志回调
        /// </summary>
        /// <param name="client"></param>
        internal CommandClientServiceRegistrarLogClientCallback(CommandClientServiceRegistrarLogClient client)
        {
            this.client = client;
            logs = new LeftArray<ServerRegistryLog>(0);
        }
        /// <summary>
        /// 取消回调操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Cancel()
        {
            keepCallback?.Dispose();
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal async Task LogCallback(IServerRegistryNodeClientNode node)
        {
            keepCallback = await node.LogCallback(client.ServerName, logCallback);
        }
        /// <summary>
        /// 服务日志回调
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        private void logCallback(ResponseResult<ServerRegistryLog> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                var log = result.Value;
                if (log != null)
                {
                    switch (log.OperationType)
                    {
                        case ServerRegistryOperationTypeEnum.ClusterMain:
                        case ServerRegistryOperationTypeEnum.Singleton:
                            if (mainLog != null) logs.Add(mainLog);
                            mainLog = log;
                            if (isLoaded) client.Callback(log, this);
                            return;
                        case ServerRegistryOperationTypeEnum.ClusterNode:
                            if (mainLog == null)
                            {
                                mainLog = log;
                                if (isLoaded) client.Callback(log, this);
                            }
                            else logs.Add(log);
                            return;
                        case ServerRegistryOperationTypeEnum.Logout:
                        case ServerRegistryOperationTypeEnum.LostContact:
                            if (mainLog != null)
                            {
                                if (mainLog.SessionID == log.SessionID)
                                {
                                    if (logs.TryPop(out mainLog) && isLoaded) client.Callback(mainLog, this);
                                }
                                else
                                {
                                    int count = logs.Length;
                                    if (count != 0)
                                    {
                                        ServerRegistryLog[] logArray = logs.Array;
                                        do
                                        {
                                            if (logArray[--count].SessionID == log.SessionID)
                                            {
                                                logs.RemoveAtToEnd(count);
                                                return;
                                            }
                                        }
                                        while (count != 0);
                                    }
                                }
                            }
                            return;
                        case ServerRegistryOperationTypeEnum.CallbackLoaded:
                            isLoaded = true;
                            if (mainLog != null) client.Callback(mainLog, this);
                            return;
                    }
                }
            }
        }
    }
}
