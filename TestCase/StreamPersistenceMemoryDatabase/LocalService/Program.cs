﻿using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
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
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(LocalService) + nameof(ServiceConfig.PersistenceSwitchPath)),
                };
                using (LocalService cacheService = cacheServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p)))
                {
                    LocalClient<ICustomServiceNodeLocalClientNode> client = cacheService.CreateClient<ICustomServiceNodeLocalClientNode>();
                    do
                    {
                        await Task.WhenAll(
                            CallbackNode.Test(client)
                            , DistributedLockNode.Test(client)
                            , ServerJsonBinaryMessageConsumer.Test(client)
                            , ServerBinaryMessageConsumer.Test(client)
                            , StringMessageConsumer.Test(client)
                            , BinaryMessageConsumer.Test(client)
                            , ServerJsonMessageConsumer.Test(client)
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
                        await new PerformanceDictionaryNode().Test(client, false);
                        await new PerformanceDictionaryNode().Test(client, true);
                        await new PerformanceSearchTreeDictionaryNode().Test(client, false);
                        await new PerformanceSearchTreeDictionaryNode().Test(client, true);
                        await new PerformanceMessageNode().Test(client, false);
                        await new PerformanceMessageNode().Test(client, true);
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