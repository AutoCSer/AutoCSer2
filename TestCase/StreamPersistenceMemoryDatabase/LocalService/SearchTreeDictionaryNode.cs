using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class SearchTreeDictionaryNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<ILongStringSearchTreeDictionaryNodeLocalClientNode> node = await client.GetOrCreateNode<ILongStringSearchTreeDictionaryNodeLocalClientNode>(typeof(ILongStringSearchTreeDictionaryNodeLocalClientNode).FullName, client.ClientNode.CreateLongStringSearchTreeDictionaryNode);
#else
            LocalResult<ISearchTreeDictionaryNodeLocalClientNode<long, string>> node = await client.GetOrCreateSearchTreeDictionaryNode<long, string>(typeof(ISearchTreeDictionaryNodeLocalClientNode<long, string>).FullName);
#endif
            if (!Program.Breakpoint(node)) return;
            LocalResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;
            LocalResult<int> intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            LocalResult<bool> boolResult = await node.Value.TryAdd(6, TestClass.String6);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Set(4, TestClass.String4);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Set(1, TestClass.String1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Set(3, TestClass.String3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Set(5, TestClass.String5);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Set(7, TestClass.String7);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Set(2, TestClass.String2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.TryAdd(6, TestClass.String6);
            if (!Program.Breakpoint(boolResult)) return;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 7)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult =  await node.Value.ContainsKey(2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            LocalResult<ValueResult<string>> stringResult = await node.Value.TryGetValue(6);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String6)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOf(5);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 4)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            LocalResult<ValueResult<KeyValue<long, string>>> keyValueResult = await node.Value.TryGetKeyValueByIndex(2);
            if (!Program.Breakpoint(keyValueResult)) return;
            if (keyValueResult.Value.Value.Key != 3 || keyValueResult.Value.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{keyValueResult.Value.Value.Key}.{keyValueResult.Value.Value.Value}+ERROR*");
                return;
            }
            stringResult =  await node.Value.TryGetValueByIndex(4);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String5)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            keyValueResult = await node.Value.TryGetFirstKeyValue();
            if (!Program.Breakpoint(keyValueResult)) return;
            if (keyValueResult.Value.Value.Key != 1 || keyValueResult.Value.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{keyValueResult.Value.Value.Key}.{keyValueResult.Value.Value.Value}+ERROR*");
                return;
            }
            keyValueResult = await node.Value.TryGetLastKeyValue();
            if (!Program.Breakpoint(keyValueResult)) return;
            if (keyValueResult.Value.Value.Key != 7 || keyValueResult.Value.Value.Value != TestClass.String7)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{keyValueResult.Value.Value.Key}.{keyValueResult.Value.Value.Value}+ERROR*");
                return;
            }
            LocalResult<ValueResult<long>> longResult = await node.Value.TryGetFirstKey();
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{longResult.Value}+ERROR*");
                return;
            }
            longResult = await node.Value.TryGetLastKey();
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 7)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{longResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.TryGetFirstValue();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.TryGetLastValue();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String7)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetRemove(6);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String6)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 6)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Remove(2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.CountLess(4);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.CountThan(4);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            LocalKeepCallback<ValueResult<string>> keepCallbackResponse = await node.Value.GetValues(1,3);
            char checkValue = 'C';
            await foreach (LocalResult<ValueResult<string>> value in keepCallbackResponse.GetAsyncEnumerable())
            {
                if (!Program.Breakpoint(value)) return;
                if (value.Value.Value[0] != checkValue)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{value.Value}+ERROR*");
                    return;
                }
                ++checkValue;
            }
            if (checkValue != 'F')
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{checkValue}+ERROR*");
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
