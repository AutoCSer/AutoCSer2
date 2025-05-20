using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class CallbackNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
            LocalResult<ICallbackNodeLocalClientNode> node = await client.GetOrCreateNode<ICallbackNodeLocalClientNode>(typeof(ICallbackNodeLocalClientNode).FullName, client.ClientNode.CreateCallbackNode);
            if (!Program.Breakpoint(node)) return;
            var boolResult = await node.Value.CheckSnapshot();
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.CallState}+{boolResult.Value}+ERROR*");
                return;
            }

            int value = AutoCSer.Random.Default.Next();
            LocalResult<int> intResult = await node.Value.SetValueCallback(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            if (!isReadWriteQueue)
            {//读写队列模式可能影响调用执行顺序
                value = AutoCSer.Random.Default.Next();
                LocalServiceQueueNode<LocalResult<int>> callbackTask = node.Value.SetCallback();
                node.Value.SetValueSendOnly(value);
                intResult = await callbackTask;
                if (!Program.Breakpoint(intResult)) return;
                if (intResult.Value != value + 1)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                    return;
                }
            }
            value = AutoCSer.Random.Default.Next();
            intResult = await node.Value.SetValueCallbackPersistence(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            if (!isReadWriteQueue)
            {//读写队列模式可能影响调用执行顺序
                value = AutoCSer.Random.Default.Next();
                LocalServiceQueueNode<LocalResult<int>> callbackTask = node.Value.SetCallbackPersistence();
                node.Value.SetValuePersistenceSendOnly(value);
                intResult = await callbackTask;
                if (!Program.Breakpoint(intResult)) return;
                if (intResult.Value != value + 1)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                    return;
                }
            }
            value = AutoCSer.Random.Default.Next();
            int count = 10, currentValue = value;
            LocalKeepCallback<int> keepCallback = await node.Value.InputKeepCallback(value, count);
            await foreach (LocalResult<int> nextValue in keepCallback.GetAsyncEnumerable())
            {
                if (!Program.Breakpoint(nextValue)) return;
                if (nextValue.Value != currentValue)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{nextValue.Value}<>{currentValue}+ERROR*");
                    return;
                }
                ++currentValue;
            }
            if (currentValue != value + count)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{value + count}<>{currentValue}+ERROR*");
                return;
            }
            currentValue = value;
            keepCallback = await node.Value.KeepCallback();
            await foreach (LocalResult<int> nextValue in keepCallback.GetAsyncEnumerable())
            {
                if (!Program.Breakpoint(nextValue)) return;
                if (nextValue.Value != currentValue)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{nextValue.Value}<>{currentValue}+ERROR*");
                    return;
                }
                ++currentValue;
            }
            if (currentValue != value + count)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{value + count}<>{currentValue}+ERROR*");
                return;
            }

            value = AutoCSer.Random.Default.Next();
            count = 10;
            currentValue = value;
            keepCallback = await node.Value.InputEnumerable(value, count);
            await foreach (LocalResult<int> nextValue in keepCallback.GetAsyncEnumerable())
            {
                if (!Program.Breakpoint(nextValue)) return;
                if (nextValue.Value != currentValue)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{nextValue.Value}<>{currentValue}+ERROR*");
                    return;
                }
                ++currentValue;
            }
            if (currentValue != value + count)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{value + count}<>{currentValue}+ERROR*");
                return;
            }
            currentValue = value;
            keepCallback = await node.Value.Enumerable();
            await foreach (LocalResult<int> nextValue in keepCallback.GetAsyncEnumerable())
            {
                if (!Program.Breakpoint(nextValue)) return;
                if (nextValue.Value != currentValue)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{nextValue.Value}<>{currentValue}+ERROR*");
                    return;
                }
                ++currentValue;
            }
            if (currentValue != value + count)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{value + count}<>{currentValue}+ERROR*");
                return;
            }

            value = AutoCSer.Random.Default.Next();
            LocalResult result = await node.Value.SetValue(value);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetValue();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            if (!isReadWriteQueue)
            {//读写队列模式可能影响调用执行顺序
                LocalServiceQueueNode<LocalResult<int>> callbackTask = node.Value.SetCallback();
                result = await node.Value.Callback();
                if (!Program.Breakpoint(result)) return;
                intResult = await callbackTask;
                if (!Program.Breakpoint(intResult)) return;
                if (intResult.Value != value + 1)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                    return;
                }
            }

            using (System.Threading.AutoResetEvent callbackWait = new System.Threading.AutoResetEvent(false))
            {
                value = AutoCSer.Random.Default.Next();
                node.Value.SetValueCommand(value, p =>
                {
                    result = p;
                    callbackWait.Set();
                });
                callbackWait.WaitOne();
                if (!Program.Breakpoint(result)) return;
                node.Value.GetValueCommand(p =>
                {
                    intResult = p;
                    callbackWait.Set();
                });
                callbackWait.WaitOne();
                if (!Program.Breakpoint(intResult)) return;
                if (intResult.Value != value + 1)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                    return;
                }

                if (!isReadWriteQueue)
                {//读写队列模式可能影响调用执行顺序
                    LocalServiceQueueNode<LocalResult<int>> callbackTask = node.Value.SetCallback();
                    node.Value.CallbackCommand(p =>
                    {
                        result = p;
                        callbackWait.Set();
                    });
                    callbackWait.WaitOne();
                    if (!Program.Breakpoint(result)) return;
                    intResult = await callbackTask;
                    if (!Program.Breakpoint(intResult)) return;
                    if (intResult.Value != value + 1)
                    {
                        ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                        return;
                    }
                }

                node.Value.CallInoutOutputCommand(value, p =>
                {
                    intResult = p;
                    callbackWait.Set();
                });
                callbackWait.WaitOne();
                if (!Program.Breakpoint(intResult)) return;
                if (intResult.Value != value + 1)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                    return;
                }
            }

            value = AutoCSer.Random.Default.Next();
            result = await node.Value.CallCustomPersistence(value);
            if (!Program.Breakpoint(result)) return;
#if !AOT
            TestClass testClass = AutoCSer.RandomObject.Creator<TestClass>.CreateNotNull();
            ServerByteArray serverByteArray = AutoCSer.BinarySerializer.Serialize(testClass);
            result = await node.Value.SetServerByteArray(serverByteArray);
            if (!Program.Breakpoint(result)) return;
            LocalResult<ServerByteArray> serverJsonBinaryResult = await node.Value.GetServerByteArray();
            if (!Program.Breakpoint(serverJsonBinaryResult)) return;
            if (!object.ReferenceEquals((byte[])serverByteArray, (byte[])serverJsonBinaryResult.Value))
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR*");
                return;
            }
#endif
            result = await node.Value.PersistenceCallbackException();
            if (result.CallState != CallStateEnum.IgnorePersistenceCallbackException)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{result.CallState}+ERROR*");
                return;
            }

            completed(isReadWriteQueue);
        }
        private static void completed(bool isReadWriteQueue)
        {
            string readWriteQueue = isReadWriteQueue ? "ReadWriteQueue" : null;
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} {readWriteQueue} Completed*");
        }
    }
}
