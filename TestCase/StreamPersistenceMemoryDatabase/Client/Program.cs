using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                CheckSeconds = 0,
                GetSocketEventDelegate = (client) => new CommandClientSocketEventTaskClient(client)
                //GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)//IO 线程回调 await 后续操作，可以避免线程调度开销，适合后续无阻塞场景
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    ConsoleWriteQueue.Breakpoint("ERROR");
                    Console.ReadKey();
                    return;
                }

                AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode>((IStreamPersistenceMemoryDatabaseClientSocketEvent)commandClient.SocketEvent);
                do
                {
                    await Task.WhenAll(
                        CallbackNode.Test(client)
                        , DistributedLockNode.Test(client)
                        , ServerJsonBinaryMessageConsumer.Test(commandClient, client)
                        , ServerBinaryMessageConsumer.Test(commandClient, client)
                        , StringMessageConsumer.Test(commandClient, client)
                        , BinaryMessageConsumer.Test(commandClient, client)
                        , ServerJsonMessageConsumer.Test(commandClient, client)
                        , HashStringFragmentDictionaryNode.Test(client)
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
                    await new PerformanceDictionaryNode().Test(commandClientConfig, client, false);
                    await new PerformanceDictionaryNode().Test(commandClientConfig, client, true);
                    await new PerformanceSearchTreeDictionaryNode().Test(commandClientConfig, client, false);
                    await new PerformanceSearchTreeDictionaryNode().Test(commandClientConfig, client, true);
                    await new PerformanceMessageNode().Test(commandClientConfig, client, false);
                    await new PerformanceMessageNode().Test(commandClientConfig, client, true);
                }
                while (true);
            }
        }
        internal static bool Breakpoint(ResponseResult result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (result.IsSuccess) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.ReturnType}+{result.CallState}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        internal static bool Breakpoint<T>(ResponseResult<T> result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (result.IsSuccess) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.ReturnType}+{result.CallState}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        internal static bool Breakpoint<T>(ResponseResult<ValueResult<T>> result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (Breakpoint<ValueResult<T>>(result, callerMemberName, callerFilePath, callerLineNumber)) return true;
            if (result.Value.IsValue) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.Value.IsValue}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        internal static bool Breakpoint(CommandClientReturnValue<CallStateEnum> result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (ConsoleWriteQueue.Breakpoint<CallStateEnum>(result, callerMemberName, callerFilePath, callerLineNumber)) return true;
            if (result.Value == CallStateEnum.Success) return true;
            ConsoleWriteQueue.Breakpoint($"*ERROR+{result.Value}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
    }
}