using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.InterfaceRealTimeCallMonitor
{
    class Program : CommandListenerSwitchProcess
    {
        static async Task Main(string[] args)
        {
            Program program = new Program(args);
            if (!await program.switchProcess())
            {
                program.start().AutoCSerNotWait();
                Console.WriteLine("Press quit to exit.");
                while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                await program.exit();
            }
        }
        private Program(string[] args) : base(args) { }
        /// <summary>
        /// 获取进程守护节点客户端
        /// </summary>
        protected override StreamPersistenceMemoryDatabaseClientNodeCache<IProcessGuardNodeClientNode> getProcessGuardClient
        {
            get { return ProcessGuardCommandClientSocketEvent.ProcessGuardNodeCache; }
        }
        /// <summary>
        /// 创建命令服务端监听
        /// </summary>
        /// <returns></returns>
        protected override Task<AutoCSer.Net.CommandListener> createCommandListener()
        {
            CommandServerConfig commandServerConfig = new CommandServerCompressConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.InterfaceRealTimeCallMonitor, string.Empty),
                SessionObject = new AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CommandListenerSession()
            };
            return Task.FromResult(new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IInterfaceRealTimeCallMonitorService>(server => new InterfaceRealTimeCallMonitorService(server))
                .CreateCommandListener(commandServerConfig));
        }
    }
}
