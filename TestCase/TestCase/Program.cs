using AutoCSer;
using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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
#if NET8 && !AOT
            CommandServer.IsAotClient = args.Length != 0 && args[0] == AutoCSer.TestCase.Common.Config.AotClientArgument;
            if (CommandServer.IsAotClient) Console.WriteLine(AutoCSer.TestCase.Common.Config.AotClientArgument);
#endif
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

            //The server sets the allowed generic parameter types
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
            using (AutoCSer.Net.CommandListener databaseListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseServiceConfig.Create())
                .Append<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IReadWriteQueueService>(readWriteQueueDatabaseServiceConfig.Create())
                .CreateCommandListener(commandServerConfig))
            using (AutoCSer.Net.CommandListener commandListener = CommandServer.CreateCommandListener(false))
            using (AutoCSer.Net.CommandListener shortLinkCommandListener = CommandServer.CreateCommandListener(true))
            {
                if (!await databaseListener.Start())
                {
                    ConsoleWriteQueue.WriteLine("内存数据库 RPC 服务启动失败。");
                    Console.ReadKey();
                    return;
                }
                if (!await commandListener.Start())
                {
                    ConsoleWriteQueue.WriteLine("测试 RPC 服务启动失败。");
                    Console.ReadKey();
                    return;
                }
                if (!await shortLinkCommandListener.Start())
                {
                    ConsoleWriteQueue.WriteLine("短连接测试 RPC 服务启动失败。");
                    Console.ReadKey();
                    return;
                }
                long streamPersistenceMemoryDatabaseCount = 2;
                Type errorType = typeof(Program);
                do
                {
                    Task<bool> defaultControllerTask = CommandClientDefaultController.TestCase();
                    Task<bool> streamPersistenceMemoryDatabaseTask = streamPersistenceMemoryDatabaseCount > 0 ? StreamPersistenceMemoryDatabase.Client.CommandClientSocketEvent.TestCase() : null;
                    Task<bool> shortLinkCommandServerTask = CommandServer.IsAotClient ? null : ShortLinkCommandServer.TestCase();
                    Task<bool> reusableDictionaryTask = ThreadPool.TinyBackground.RunTask(ReusableDictionary.TestCase);
                    Task<bool> searchTreeTask = ThreadPool.TinyBackground.RunTask(SearchTree.TestCase);
                    Task<bool> binarySerializeTask = BinarySerialize.TestCase();
                    Task<bool> jsonTask = ThreadPool.TinyBackground.RunTask(Json.TestCase);
                    Task<bool> xmlTask = ThreadPool.TinyBackground.RunTask(Xml.TestCase);
                    if (shortLinkCommandServerTask != null && !await shortLinkCommandServerTask) { errorType = typeof(ShortLinkCommandServer); break; }
                    if (!CommandServer.IsAotClient && !await CommandServer.TestCase()) { errorType = typeof(CommandServer); break; }
                    if (!CommandServer.IsAotClient && !await CommandReverseServer.TestCase()) { errorType = typeof(CommandReverseServer); break; }
                    Task<bool> interfaceControllerTaskQueueTask = InterfaceControllerTaskQueue.TestCase();
                    if (!await binarySerializeTask) { errorType = typeof(BinarySerialize); break; }
                    if (!await jsonTask) { errorType = typeof(Json); break; }
                    if (!await xmlTask) { errorType = typeof(Xml); break; }
                    if (!await interfaceControllerTaskQueueTask) { errorType = typeof(InterfaceControllerTaskQueue); break; }
                    if (!await searchTreeTask) { errorType = typeof(SearchTree); break; }
                    if (!await reusableDictionaryTask) { errorType = typeof(ReusableDictionary); break; }
                    if (!await defaultControllerTask) { errorType = typeof(CommandClientDefaultController); break; }
                    if (streamPersistenceMemoryDatabaseTask != null && !await streamPersistenceMemoryDatabaseTask) { errorType = typeof(StreamPersistenceMemoryDatabaseService); break; }
                    Console.Write('.');
                    --streamPersistenceMemoryDatabaseCount;
                    if (CommandServer.IsAotClient) await AutoCSer.Threading.SwitchAwaiter.Default;
                }
                while (true);
                ConsoleWriteQueue.WriteLine(errorType.FullName + " ERROR", ConsoleColor.Red);
                Console.ReadKey();
            }
        }
    }
}
