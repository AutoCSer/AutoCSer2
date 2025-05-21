using AutoCSer.Extensions;

namespace AutoCSer.Document.MemoryDatabaseNode
{
    /// <summary>
    /// https://zhuanlan.zhihu.com/p/14011804562
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            //服务端设置允许的泛型参数类型
            await AutoCSer.Common.Config.AppendRemoteTypeAsync(typeof(AutoCSer.Document.MemoryDatabaseNode.Data.TestClass));

            AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig databaseServiceConfig = new AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseNode)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseNode) + nameof(AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                //.Append<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IReadWriteQueueService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    await Client.CommandClientSocketEvent.Test();

                    Console.ReadKey();
                }
            }
        }
    }
}
