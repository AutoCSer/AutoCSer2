using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServerRegistry
{
    class Program : CommandListenerSwitchProcess
    {
        static async Task Main(string[] args)
        {
            Program program = new Program(args);
            if (!await program.switchProcess())
            {
                program.start().NotWait();
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
            AutoCSer.TestCase.ServerRegistry.ServiceConfig databaseServiceConfig = new AutoCSer.TestCase.ServerRegistry.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.ServerRegistry)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.ServerRegistry) + nameof(AutoCSer.TestCase.ServerRegistry.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();

            CommandServerConfig commandServerConfig = new CommandServerConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry, string.Empty),
            };
            return Task.FromResult(new CommandListenerBuilder(0)
               .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
               .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
               .CreateCommandListener(commandServerConfig));
        }
    }
}
