﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class ArrayNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            int length = 5;
            ResponseResult<IArrayNodeClientNode<string>> node = await client.GetOrCreateArrayNode<string>(typeof(IArrayNodeClientNode<string>).FullName, length);
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.ClearArray();
            if (!Program.Breakpoint(result)) return;
            ResponseResult<int> intResult = await node.Value.GetLength();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != length)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.FillArray(TestClass.String1);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.IndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != length - 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            ResponseResult<bool> boolResult = await node.Value.Fill(TestClass.String2, 1, length - 2);
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
            if (intResult.Value != length - 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.SetValue(2, TestClass.String3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            ResponseResult<ValueResult<string>> stringResult = await node.Value.GetValue(2);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.SortArray();
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.LastIndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOfArray(TestClass.String3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != length - 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.ReverseArray();
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.IndexOfArray(TestClass.String1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != length - 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValueSet(1, TestClass.String3);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.GetValue(1);
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != TestClass.String3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{stringResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Clear(1, length - 2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOfArray(TestClass.String2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != -1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Fill(TestClass.String2, 1, length - 2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != length - 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Reverse(0, 3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String3);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Sort(2, 2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.LastIndexOfArray(TestClass.String2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 2)
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
