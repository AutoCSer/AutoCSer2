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
            try
            {
                ServiceConfig cacheServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService) + nameof(ServiceConfig.PersistenceSwitchPath)),
                };
                using (LocalService cacheService = cacheServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p)))
                {
                    LocalClient<ICustomServiceNodeLocalClientNode> client = cacheService.CreateClient<ICustomServiceNodeLocalClientNode>();
                    do
                    {
                        //await GameNode.Test(client);
                        //await Task.Delay(10);
                        //continue;

                        await Task.WhenAll(
                            CallbackNode.Test(client)
                            , DistributedLockNode.Test(client)
                            , MessageConsumer.Test(client)
                            , FragmentDictionaryNode.Test(client)
                            , DictionaryNode.Test(client)
                            , SearchTreeDictionaryNode.Test(client)
                            , SearchTreeSetNode.Test(client)
                            , SortedDictionaryNode.Test(client)
                            , SortedListNode.Test(client)
                            , SortedSetNode.Test(client)
                            , FragmentHashSetNode.Test(client)
                            , HashSetNode.Test(client)
                            , BitmapNode.Test(client)
                            , QueueNode.Test(client)
                            , StackNode.Test(client)
                            , LeftArrayNode.Test(client)
                            , ArrayNode.Test(client)
                            );
                        await new PerformanceDictionaryNode().Test(client);
                        await new PerformanceSearchTreeDictionaryNode().Test(client);
                        await new PerformanceMessageNode().Test(client);
                    }
                    while (true);
                }
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
                Console.ReadLine();
            }
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