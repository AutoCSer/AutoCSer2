using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 反向收集日志集群客户端
    /// </summary>
    internal sealed class ReverseLogCollectionClusterClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.ClusterClient<ReverseLogCollectionClusterClient>
    {
        /// <summary>
        /// 客户端
        /// </summary>
        private readonly AutoCSer.Net.CommandClient client;
        /// <summary>
        /// 获取日志保持回调
        /// </summary>
        private CommandKeepCallback keepCallback;
        /// <summary>
        /// 反向收集日志集群客户端
        /// </summary>
        /// <param name="serverRegistryClusterClient">集群服务客户端</param>
        /// <param name="log">Server Registration Log
        /// 服务注册日志</param>
        internal ReverseLogCollectionClusterClient(ServerRegistryClusterClient serverRegistryClusterClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog log) : base(serverRegistryClusterClient, log)
        {
            client = new AutoCSer.Net.CommandClient(new AutoCSer.Net.CommandClientConfig
            {
                Host = new AutoCSer.Net.HostEndPoint(log.Port, log.Host),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
            });
            check().AutoCSerNotWait();
        }
        /// <summary>
        /// Get the client connection
        /// 获取客户端连接
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> getSocket()
        {
            var socket = await client.GetSocketAsync();
            if (socket != null)
            {
                keepCallback = await ((CommandClientSocketEvent)client.SocketEvent).ReverseLogCollectionClient.LogCallback(logCallback);
                if (keepCallback != null) return true;
            }
            return false;
        }
        /// <summary>
        /// Close the client
        /// 关闭客户端
        /// </summary>
        protected override void close()
        {
            client.Dispose();
        }
        /// <summary>
        /// 日志收集回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="command"></param>
        private static void logCallback(CommandClientReturnValue<LogInfo> returnValue, KeepCallbackCommand command)
        {
            if (returnValue.IsSuccess) logCallback(returnValue.Value);
        }
        /// <summary>
        /// 日志收集回调
        /// </summary>
        /// <param name="log"></param>
        private static void logCallback(LogInfo log)
        {
            Console.WriteLine($"{log.LogTime.AutoCSerExtensions().ToString()} {log.Message}");
        }
    }
}
