using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class LeftArrayNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client)
        {
            ResponseResult<ILeftArrayNodeLocalClientNode<string>> node = await client.GetOrCreateLeftArrayNode<string>(typeof(ILeftArrayNodeLocalClientNode<string>).FullName, 0);
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.SetEmpty();
            if (!Program.Breakpoint(result)) return;
            ResponseResult<int> intResult = await node.Value.GetLength();
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
            ResponseResult<bool> boolResult = await node.Value.TryAdd("A");
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
            result = await node.Value.Add("A");
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(0, "C");
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
            boolResult = await node.Value.Insert(1, "B");
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add("A");
            if (!Program.Breakpoint(result)) return;
            boolResult = await node.Value.Insert(0, "C");
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
            intResult = await node.Value.IndexOfArray("A");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray("A");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 4)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Fill("B", 1, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOfArray("B");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray("B");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result =  await node.Value.SortArray();
            if (!Program.Breakpoint(result)) return;
            ResponseResult<ValueResult<string>> stringResult = await node.Value.GetTryPopValue();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "C")
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
            boolResult = await node.Value.SetValue(1, "C");
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "C")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add("C");
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
            intResult = await node.Value.IndexOfArray("C");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray("C");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValueSet(3, "A");
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "C")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(3);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "A")
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
            intResult = await node.Value.IndexOfArray("A");
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Remove("A");
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
            if (stringResult.Value.Value != "B")
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
            if (stringResult.Value.Value != "C")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "A")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(1, "B");
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Insert(1, "C");
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Add("A");
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
            if (stringResult.Value.Value != "C")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "A")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValueRemoveAt(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "A")
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
            if (stringResult.Value.Value != "C")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "A")
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
            result = await node.Value.Add("C");
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add("C");
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add("B");
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add("A");
            if (!Program.Breakpoint(result)) return;
            result = await node.Value.Add("A");
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
            if (stringResult.Value.Value != "A")
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(3);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != "C")
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
            boolResult = await node.Value.Fill("B", 1, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOf("B", 0, 3);
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
            intResult = await node.Value.LastIndexOf("B", 4, 3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
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
