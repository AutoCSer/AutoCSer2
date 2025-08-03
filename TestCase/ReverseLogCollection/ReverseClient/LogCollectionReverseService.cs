using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseClient
{
    /// <summary>
    /// 日志收集反向服务
    /// </summary>
    internal sealed class LogCollectionReverseService : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.ClusterClient<LogCollectionReverseService>, ILogCollectionReverseService<LogInfo>
    {
        /// <summary>
        /// 反向命令服务客户端
        /// </summary>
        internal readonly CommandReverseClient CommandReverseClient;
        /// <summary>
        /// 日志收集反向服务
        /// </summary>
        /// <param name="serverRegistryClusterClient"></param>
        /// <param name="log"></param>
        internal LogCollectionReverseService(ServerRegistryClusterClient serverRegistryClusterClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog log) : base(serverRegistryClusterClient, log)
        {
            CommandReverseClientConfig commandServerConfig = new CommandReverseClientConfig { Host = log.HostEndPoint };
            CommandReverseClient = new CommandListenerBuilder(2)
                .Append<ITimestampVerifyReverseService<string>>(new TimestampVerifyReverseService<string>(AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ILogCollectionReverseService<LogInfo>>(this)
                .CreateCommandListener(commandServerConfig);
        }
        /// <summary>
        /// Get the client connection
        /// 获取客户端连接
        /// </summary>
        /// <returns></returns>
        protected override Task<bool> getSocket() { throw new InvalidOperationException(); }
        /// <summary>
        /// Close the client
        /// 关闭客户端
        /// </summary>
        protected override void close() { CommandReverseClient.Dispose(); }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log">日志数据</param>
        public void Append(CommandServerSocket socket, CommandServerCallQueue queue, LogInfo log)
        {
            Console.WriteLine($"{log.LogTime.AutoCSerExtensions().ToString()} {log.Message}");
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log">日志数据</param>
        /// <returns></returns>
        public CommandServerSendOnly AppendSendOnly(CommandServerSocket socket, CommandServerCallQueue queue, LogInfo log)
        {
            Console.WriteLine($"{log.LogTime.AutoCSerExtensions().ToString()} {log.Message}");
            return CommandServerSendOnly.Null;
        }
    }
}
