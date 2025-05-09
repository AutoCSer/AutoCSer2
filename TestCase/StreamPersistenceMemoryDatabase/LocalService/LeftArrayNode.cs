using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class LeftArrayNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<IStringLeftArrayNodeLocalClientNode> node = await client.GetOrCreateNode<IStringLeftArrayNodeLocalClientNode>(typeof(IStringLeftArrayNodeLocalClientNode).FullName, (index, nodeKey, nodeInfo) => client.ClientNode.CreateStringLeftArrayNode(index, nodeKey, nodeInfo, 0));
#else
            LocalResult<ILeftArrayNodeLocalClientNode<string>> node = await client.GetOrCreateLeftArrayNode<string>(typeof(ILeftArrayNodeLocalClientNode<string>).FullName, 0);
#endif
            if (!Program.Breakpoint(node)) return;
            LocalResult result = await node.Value.SetEmpty();
            if (!Program.Breakpoint(result)) return;
            LocalResult<int> intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetCapacity();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetFreeCount();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            LocalResult<bool> boolResult = await node.Value.TryAdd(TestClass.String1);
            if (!Program.Breakpoint(boolResult)) return;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add(TestClass.String1);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(0, TestClass.String3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(1, TestClass.String2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add(TestClass.String1);
            if (!Program.Breakpoint(result)) return;
            boolResult = await node.Value.Insert(0, TestClass.String3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 5)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 4)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Fill(TestClass.String2, 1, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOfArray(TestClass.String2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result =  await node.Value.SortArray();
            if (!Program.Breakpoint(result)) return;
            LocalResult<ValueResult<string>> stringResult = await node.Value.GetTryPopValue();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 4)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.SetValue(1, TestClass.String3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add(TestClass.String3);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 5)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.ReverseArray();
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.IndexOfArray(TestClass.String3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValueSet(3, TestClass.String1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(3);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.RemoveToEnd(1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 4)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Remove(TestClass.String1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Reverse(1, 2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.TryPop();
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(0);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(1, TestClass.String2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(1, TestClass.String3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add(TestClass.String1);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 5)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValueRemoveToEnd(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValueRemoveAt(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.RemoveAt(1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(0);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            result =  await node.Value.ClearLength();
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add(TestClass.String3);
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add(TestClass.String3);
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add(TestClass.String2);
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add(TestClass.String1);
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add(TestClass.String1);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 5)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Sort(1, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(3);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Clear(1, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != null)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(3);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != null)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Fill(TestClass.String2, 1, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOf(TestClass.String2, 0, 3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.SetValue(2, null);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOf(TestClass.String2, 4, 3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
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
