﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class SearchTreeSetNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<ISearchTreeSetNodeClientNode<long>> node = await client.GetOrCreateSearchTreeSetNode<long>(typeof(ISearchTreeSetNodeClientNode<long>).FullName);
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;
            ResponseResult<int> intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            ResponseResult<bool> boolResult = await node.Value.Add(2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Add(2);
            if (!Program.Breakpoint(boolResult)) return;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Add(3);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Add(1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Contains(2);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            ResponseResult<ValueResult<long>> longResult = await node.Value.GetFrist();
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            longResult = await node.Value.GetLast();
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.IndexOf(2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            longResult = await node.Value.GetByIndex(1);
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 2)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.CountLess(2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.CountThan(2);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
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
            intResult = await node.Value.Count();
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
