using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;

namespace AutoCSer.TestCase.LogCollectionReverseClient
{
    /// <summary>
    /// 日志收集反向服务
    /// </summary>
    internal sealed class LogCollectionReverseService : ILogCollectionReverseService<LogInfo>, IDisposable
    {
        /// <summary>
        /// 反向命令服务客户端
        /// </summary>
        private readonly CommandReverseClient commandReverseClient;
        /// <summary>
        /// 日志收集反向服务
        /// </summary>
        /// <param name="hostEndPoint"></param>
        internal LogCollectionReverseService(ref HostEndPoint hostEndPoint)
        {
            CommandReverseClientConfig commandServerConfig = new CommandReverseClientConfig { Host = hostEndPoint };
            commandReverseClient = new CommandListenerBuilder(2)
                .Append<ITimestampVerifyReverseService<string>>(new TimestampVerifyReverseService<string>(AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ILogCollectionReverseService<LogInfo>>(this)
                .CreateCommandListener(commandServerConfig);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            commandReverseClient.Dispose();
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log">日志数据</param>
        public void Append(CommandServerSocket socket, CommandServerCallQueue queue, LogInfo log)
        {
            Console.WriteLine($"{log.LogTime.toString()} {log.Message}");
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
            Console.WriteLine($"{log.LogTime.toString()} {log.Message}");
            return CommandServerSendOnly.Null;
        }
    }
}
