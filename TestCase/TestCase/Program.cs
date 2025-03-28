using AutoCSer;
using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
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
            StreamPersistenceMemoryDatabase.ServiceConfig readWriteQueueDatabaseServiceConfig = new StreamPersistenceMemoryDatabase.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase)) + nameof(IReadWriteQueueService),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase) + nameof(IReadWriteQueueService) + nameof(StreamPersistenceMemoryDatabase.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
            };
#if NetStandard21
            await
#endif
            using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseServiceConfig.Create())
                .Append<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IReadWriteQueueService>(readWriteQueueDatabaseServiceConfig.Create())
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Type errorType = typeof(Program);
                    do
                    {
                        Task<bool> commandServerTask = CommandServer.TestCase();
                        Task<bool> streamPersistenceMemoryDatabaseTask = StreamPersistenceMemoryDatabase.Client.CommandClientSocketEvent.TestCase();
                        Task<bool> reusableDictionaryTask = ThreadPool.TinyBackground.RunTask(ReusableDictionary.TestCase);
                        Task<bool> searchTreeTask = ThreadPool.TinyBackground.RunTask(SearchTree.TestCase);
                        Task<bool> binarySerializeTask = ThreadPool.TinyBackground.RunTask(BinarySerialize.TestCase);
                        Task<bool> jsonTask = ThreadPool.TinyBackground.RunTask(Json.TestCase);
                        Task<bool> xmlTask = ThreadPool.TinyBackground.RunTask(Xml.TestCase);
                        if (!await commandServerTask) { errorType = typeof(CommandServer); break; }
                        Task<bool> commandReverseServerTask = CommandReverseServer.TestCase();
                        if (!await commandReverseServerTask) { errorType = typeof(CommandReverseServer); break; }
                        Task<bool> interfaceControllerTaskQueueTask = InterfaceControllerTaskQueue.TestCase();
                        if (!await binarySerializeTask) { errorType = typeof(BinarySerialize); break; }
                        if (!await jsonTask) { errorType = typeof(Json); break; }
                        if (!await xmlTask) { errorType = typeof(Xml); break; }
                        if (!await interfaceControllerTaskQueueTask) { errorType = typeof(InterfaceControllerTaskQueue); break; }
                        if (!await streamPersistenceMemoryDatabaseTask) { errorType = typeof(StreamPersistenceMemoryDatabaseService); break; }
                        if (!await searchTreeTask) { errorType = typeof(SearchTree); break; }
                        if (!await reusableDictionaryTask) { errorType = typeof(ReusableDictionary); break; }
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
