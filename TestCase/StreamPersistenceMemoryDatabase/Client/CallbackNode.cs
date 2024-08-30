using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class CallbackNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<ICallbackNodeClientNode> node = await client.GetOrCreateNode<ICallbackNodeClientNode>(typeof(ICallbackNodeClientNode).FullName, client.ClientNode.CreateCallbackNode);
            if (!Program.Breakpoint(node)) return;
            int value = AutoCSer.Random.Default.Next();
            ResponseResult<int> intResult = await node.Value.SetValueCallback(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }
            value = AutoCSer.Random.Default.Next();
            Task<ResponseResult<int>> callbackTask = node.Value.SetCallback();
            node.Value.SetValueSendOnly(value).Discard();
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
            node.Value.SetValuePersistenceSendOnly(value).Discard();
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

            result = await node.Value.SetServerJsonBinary(new TestClass { Int = value, String = "D" });
            if (!Program.Breakpoint(result)) return;
            ResponseResult<ServerJsonBinary<TestClass>> serverJsonBinaryResult = await node.Value.GetServerJsonBinary();
            if (!Program.Breakpoint(serverJsonBinaryResult)) return;
            if (serverJsonBinaryResult.Value.Value == null || serverJsonBinaryResult.Value.Value.Int != value || serverJsonBinaryResult.Value.Value.String != "D")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{serverJsonBinaryResult.Value.Value?.Int}.{serverJsonBinaryResult.Value.Value?.String}+ERROR*");
                return;
            }

            result = await node.Value.SetServerJson(new TestClass { Int = value, String = "A" });
            if (!Program.Breakpoint(result)) return;
            ResponseResult<ServerJson<TestClass>> serverJsonResult = await node.Value.GetServerJson();
            if (!Program.Breakpoint(serverJsonResult)) return;
            if (serverJsonResult.Value.Value == null || serverJsonResult.Value.Value.Int != value || serverJsonResult.Value.Value.String != "A")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{serverJsonResult.Value.Value?.Int}.{serverJsonResult.Value.Value?.String}+ERROR*");
                return;
            }

            result = await node.Value.SetJsonValue(new TestClass { Int = value, String = "B" });
            if (!Program.Breakpoint(result)) return;
            ResponseResult<JsonValue<TestClass>> jsonValueResult = await node.Value.GetJsonValue();
            if (!Program.Breakpoint(jsonValueResult)) return;
            if (jsonValueResult.Value.Value == null || jsonValueResult.Value.Value.Int != value || jsonValueResult.Value.Value.String != "B")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{jsonValueResult.Value.Value?.Int}.{jsonValueResult.Value.Value?.String}+ERROR*");
                return;
            }

            result = await node.Value.SetServerBinary(new TestClass { Int = value, String = "C" });
            if (!Program.Breakpoint(result)) return;
            ResponseResult<ServerBinary<TestClass>> serverBinaryResult = await node.Value.GetServerBinary();
            if (!Program.Breakpoint(serverBinaryResult)) return;
            if (serverBinaryResult.Value.Value == null || serverBinaryResult.Value.Value.Int != value || serverBinaryResult.Value.Value.String != "C")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{serverBinaryResult.Value.Value?.Int}.{serverBinaryResult.Value.Value?.String}+ERROR*");
                return;
            }

            CommandClientReturnValue<CallStateEnum> repairState = await client.RepairNodeMethod(node.Value, typeof(CallbackNodeRepairMethod).GetMethod(nameof(CallbackNodeRepairMethod.SetValueCallbackPersistenceV1), BindingFlags.Static | BindingFlags.Public));
            if (!Program.Breakpoint(repairState)) return;
            value = AutoCSer.Random.Default.Next();
            intResult = await node.Value.SetValueCallbackPersistence(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
                return;
            }

            repairState = await client.BindNodeMethod(node.Value, typeof(CallbackNodeRepairMethod).GetMethod(nameof(CallbackNodeRepairMethod.BindNodeMethodTestV1), BindingFlags.Static | BindingFlags.Public));
            if (!Program.Breakpoint(repairState)) return;
            value = AutoCSer.Random.Default.Next();
            intResult = await node.Value.BindNodeMethodTest(value);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != value + 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}<>{value + 1}+ERROR*");
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
