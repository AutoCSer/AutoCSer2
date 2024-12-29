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
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client)
        {
            ResponseResult<ICallbackNodeLocalClientNode> node = await client.GetOrCreateNode<ICallbackNodeLocalClientNode>(typeof(ICallbackNodeLocalClientNode).FullName, client.ClientNode.CreateCallbackNode);
            if (!Program.Breakpoint(node)) return;
            var boolResult = await node.Value.CheckSnapshot();
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.CallState}+{boolResult.Value}+ERROR*");
                return;
            }

            int value = AutoCSer.Random.Default.Next();
            ResponseResult<int> intResult = await node.Value.SetValueCallback(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            value = AutoCSer.Random.Default.Next();
            LocalServiceQueueNode<ResponseResult<int>> callbackTask = node.Value.SetCallback();
            node.Value.SetValueSendOnly(value);
            intResult = await callbackTask;
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            value = AutoCSer.Random.Default.Next();
            intResult = await node.Value.SetValueCallbackPersistence(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            value = AutoCSer.Random.Default.Next();
            callbackTask = node.Value.SetCallbackPersistence();
            node.Value.SetValuePersistenceSendOnly(value);
            intResult = await callbackTask;
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }

            value = AutoCSer.Random.Default.Next();
            int count = 10, currentValue = value;
            KeepCallbackResponse<int> keepCallback = await node.Value.InputKeepCallback(value, count);
            await foreach (ResponseResult<int> nextValue in keepCallback.GetAsyncEnumerable())
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
            await foreach (ResponseResult<int> nextValue in keepCallback.GetAsyncEnumerable())
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
            await foreach (ResponseResult<int> nextValue in keepCallback.GetAsyncEnumerable())
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
            await foreach (ResponseResult<int> nextValue in keepCallback.GetAsyncEnumerable())
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
            ResponseResult result = await node.Value.SetValue(value);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetValue();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            callbackTask = node.Value.SetCallback();
            result = await node.Value.Callback();
            if (!Program.Breakpoint(result)) return;
            intResult = await callbackTask;
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }

            value = AutoCSer.Random.Default.Next();
            result = await node.Value.CallCustomPersistence(value);
            if (!Program.Breakpoint(result)) return;

            TestClass testClass = AutoCSer.RandomObject.Creator<TestClass>.CreateNotNull();
            ServerByteArray serverByteArray = AutoCSer.BinarySerializer.Serialize(testClass);
            result = await node.Value.SetServerByteArray(serverByteArray);
            if (!Program.Breakpoint(result)) return;
            ResponseResult<ServerByteArray> serverJsonBinaryResult = await node.Value.GetServerByteArray();
            if (!Program.Breakpoint(serverJsonBinaryResult)) return;
            if (!object.ReferenceEquals((byte[])serverByteArray, (byte[])serverJsonBinaryResult.Value))
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR*");
                return;
            }

            result = await node.Value.PersistenceCallbackException();
            if (result.ReturnType != CommandClientReturnTypeEnum.Success || result.CallState != CallStateEnum.IgnorePersistenceCallbackException)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{result.ReturnType}+{result.CallState}+ERROR*");
                return;
            }

            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
