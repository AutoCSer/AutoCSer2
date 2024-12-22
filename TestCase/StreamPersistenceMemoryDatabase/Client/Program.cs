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
                IStreamPersistenceMemoryDatabaseClientSocketEvent client = (IStreamPersistenceMemoryDatabaseClientSocketEvent)await commandClient.GetSocketEvent();
                if (client == null)
                {
                    ConsoleWriteQueue.Breakpoint("ERROR");
                    Console.ReadKey();
                    return;
                }

                AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> clientNode = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode>(client);
                do
                {
                    await Task.WhenAll(
                        CallbackNode.Test(clientNode)
                        , DistributedLockNode.Test(clientNode)
                        , BinaryMessageConsumer.Test(commandClient, clientNode)
                        , ServerByteArrayMessageConsumer.Test(commandClient, clientNode)
                        , ServerByteArrayMessageStringConsumer.Test(commandClient, clientNode)
                        , ServerByteArrayMessageJsonConsumer.Test(commandClient, clientNode)
                        , ServerByteArrayMessageBinaryConsumer.Test(commandClient, clientNode)
                        , HashBytesFragmentDictionaryNode.Test(clientNode)
                        , ByteArrayFragmentDictionaryNode.Test(clientNode)
                        , FragmentDictionaryNode.Test(clientNode)
                        , HashBytesDictionaryNode.Test(clientNode)
                        , ByteArrayDictionaryNode.Test(clientNode)
                        , DictionaryNode.Test(clientNode)
                        , SearchTreeDictionaryNode.Test(clientNode)
                        , SortedDictionaryNode.Test(clientNode)
                        , SortedListNode.Test(clientNode)
                        , FragmentHashSetNode.Test(clientNode)
                        , HashSetNode.Test(clientNode)
                        , SearchTreeSetNode.Test(clientNode)
                        , SortedSetNode.Test(clientNode)
                        , ByteArrayQueueNode.Test(clientNode)
                        , QueueNode.Test(clientNode)
                        , ByteArrayStackNode.Test(clientNode)
                        , StackNode.Test(clientNode)
                        , LeftArrayNode.Test(clientNode)
                        , ArrayNode.Test(clientNode)
                        , BitmapNode.Test(clientNode)
                        );
                    await new PerformanceDictionaryNode().Test(commandClientConfig, clientNode);
                    await new PerformanceSearchTreeDictionaryNode().Test(commandClientConfig, clientNode);
                    await new PerformanceMessageNode().Test(commandClientConfig, clientNode);
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
        internal static bool Breakpoint<T>(ResponseValueResult<T> result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (result.IsSuccess)
            {
                if (result.IsValue) return true;
                ConsoleWriteQueue.Breakpoint($"*ERROR+{result.IsValue}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
            }
            else ConsoleWriteQueue.Breakpoint($"*ERROR+{result.ReturnType}+{result.CallState}+ERROR*", callerMemberName, callerFilePath, callerLineNumber);
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