using AutoCSer;
using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class Program
    {
#if NetStandard21
        static async Task Main(string[] args)
#else
        static void Main(string[] args)
#endif
        {
#if NetStandard21
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await test();
#else
            new UISynchronousTask(test).Wait();
#endif
        }
        private static async Task test()
        {
            AutoCSer.FieldEquals.Comparor.IsBreakpoint = true;

            //服务端设置允许的泛型参数类型
            await AutoCSer.Common.Config.AppendRemoteTypeAsync(typeof(StreamPersistenceMemoryDatabase.Data.TestClass));

            StreamPersistenceMemoryDatabase.ServiceConfig databaseServiceConfig = new StreamPersistenceMemoryDatabase.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase) + nameof(StreamPersistenceMemoryDatabase.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
            };
#if NetStandard21
            await
#endif
            using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Type errorType = typeof(Program);
                    do
                    {
                        if (!BinarySerialize.TestCase()) { errorType = typeof(BinarySerialize); break; }
                        if (!Json.TestCase()) { errorType = typeof(Json); break; }
                        if (!Xml.TestCase()) { errorType = typeof(Xml); break; }
                        if (!await CommandServer.TestCase()) { errorType = typeof(CommandServer); break; }
                        if (!await StreamPersistenceMemoryDatabase.Client.CommandClientSocketEvent.TestCase()) { errorType = typeof(StreamPersistenceMemoryDatabaseService); break; }
                        if (!await CommandReverseServer.TestCase()) { errorType = typeof(CommandReverseServer); break; }
                        if (!await InterfaceControllerTaskQueue.TestCase()) { errorType = typeof(InterfaceControllerTaskQueue); break; }
                        Console.Write('.');
                    }
                    while (true);
                    ConsoleWriteQueue.WriteLine(errorType.FullName + " ERROR", ConsoleColor.Red);
                }
                else ConsoleWriteQueue.WriteLine("内存数据库 RPC 服务启动失败。");
                Console.ReadKey();
            }
        }
    }
}
