using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
#if AOT
                string Aot = "AOT";
#else
                string Aot = string.Empty;
#endif
                ServiceConfig databaseServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService) + Aot),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService) + Aot + nameof(ServiceConfig.PersistenceSwitchPath)),
                };
                ServiceConfig readWriteQueueDatabaseServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService) + "ReadWriteQueue" + Aot),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService) + "ReadWriteQueue" + Aot + nameof(ServiceConfig.PersistenceSwitchPath)),
                };
                using (LocalService cacheService = databaseServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p)))
                using (LocalService readWriteQueueCacheService = readWriteQueueDatabaseServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p), -1))
                {
                    LocalClient<ICustomServiceNodeLocalClientNode> client = cacheService.CreateClient<ICustomServiceNodeLocalClientNode>();
                    LocalClient<ICustomServiceNodeLocalClientNode> readWriteQueueClient = readWriteQueueCacheService.CreateClient<ICustomServiceNodeLocalClientNode>();
                    do
                    {
                        //await GameNode.Test(client);
                        //await Task.Delay(10);
                        //continue;

                        await Task.WhenAll(
                            CallbackNode.Test(client, false)
#if !AOT
                            , DistributedLockNode.Test(client, false)
#endif
                            , MessageConsumer.Test(client, false)
                            , FragmentDictionaryNode.Test(client, false)
                            , DictionaryNode.Test(client, false)
                            , SearchTreeDictionaryNode.Test(client, false)
                            , SearchTreeSetNode.Test(client, false)
                            , SortedDictionaryNode.Test(client, false)
                            , SortedListNode.Test(client, false)
                            , SortedSetNode.Test(client, false)
                            , FragmentHashSetNode.Test(client, false)
                            , HashSetNode.Test(client, false)
                            , BitmapNode.Test(client, false)
                            , QueueNode.Test(client, false)
                            , StackNode.Test(client, false)
                            , LeftArrayNode.Test(client, false)
                            , ArrayNode.Test(client, false)
                            , OnlyPersistenceNode.Test(client, false)
                            );
                        await new PerformanceDictionaryNode().Test(client, false);
                        await new PerformanceSearchTreeDictionaryNode().Test(client, false);
                        await new PerformanceMessageNode().Test(client, false);
                        await Task.WhenAll(
                            CallbackNode.Test(readWriteQueueClient, true)
#if !AOT
                            , DistributedLockNode.Test(readWriteQueueClient, true)
#endif
                            , MessageConsumer.Test(readWriteQueueClient, true)
                            , FragmentDictionaryNode.Test(readWriteQueueClient, true)
                            , DictionaryNode.Test(readWriteQueueClient, true)
                            , SearchTreeDictionaryNode.Test(readWriteQueueClient, true)
                            , SearchTreeSetNode.Test(readWriteQueueClient, true)
                            , SortedDictionaryNode.Test(readWriteQueueClient, true)
                            , SortedListNode.Test(readWriteQueueClient, true)
                            , SortedSetNode.Test(readWriteQueueClient, true)
                            , FragmentHashSetNode.Test(readWriteQueueClient, true)
                            , HashSetNode.Test(readWriteQueueClient, true)
                            , BitmapNode.Test(readWriteQueueClient, true)
                            , QueueNode.Test(readWriteQueueClient, true)
                            , StackNode.Test(readWriteQueueClient, true)
                            , LeftArrayNode.Test(readWriteQueueClient, true)
                            , ArrayNode.Test(readWriteQueueClient, true)
                            , OnlyPersistenceNode.Test(readWriteQueueClient, true)
                            );
                        await new PerformanceDictionaryNode().Test(readWriteQueueClient, true);
                        await new PerformanceSearchTreeDictionaryNode().Test(readWriteQueueClient, true);
                        await new PerformanceMessageNode().Test(readWriteQueueClient, true);
                    }
                    while (Console.ReadLine() != "quit");
                }
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
                Console.ReadLine();
            }
#if AOT
            AutoCSer.TestCase.StreamPersistenceMemoryDatabase.AotMethod.Call();
#endif
        }
        internal static bool Breakpoint(LocalResult result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (result.IsSuccess) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.CallState}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        internal static bool Breakpoint<T>(LocalResult<T> result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (result.IsSuccess) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.CallState}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        internal static bool Breakpoint<T>(LocalResult<ValueResult<T>> result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (Breakpoint<ValueResult<T>>(result, callerMemberName, callerFilePath, callerLineNumber)) return true;
            if (result.Value.IsValue) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.Value.IsValue}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
    }
}