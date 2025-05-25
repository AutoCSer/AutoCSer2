using AutoCSer;
using AutoCSer.CommandService;
using AutoCSer.CommandService.DeployTask;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DeployTask
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
            StreamPersistenceMemoryDatabaseServiceConfig databaseServiceConfig = new StreamPersistenceMemoryDatabaseServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.DeployTask)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.DeployTask) + nameof(StreamPersistenceMemoryDatabaseServiceConfig.PersistenceSwitchPath))
            };
            StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p));

            UploadFileServiceConfig uploadFileServiceConfig = new UploadFileServiceConfig
            {
                BackupPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.DeployTask) + nameof(UploadFileServiceConfig.BackupPath)),
            };

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerCompressConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DeployTask, string.Empty),
            };
            return Task.FromResult(new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IStreamPersistenceMemoryDatabaseService>(databaseService)
                .Append<IUploadFileService>(new UploadFileService(uploadFileServiceConfig))
                .CreateCommandListener(commandServerConfig));
        }
    }
}
