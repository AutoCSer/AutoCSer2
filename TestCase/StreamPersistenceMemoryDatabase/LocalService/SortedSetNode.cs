﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class SortedSetNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<ILongSortedSetNodeLocalClientNode> node = await client.GetOrCreateNode<ILongSortedSetNodeLocalClientNode>(typeof(ILongSortedSetNodeLocalClientNode).FullName, client.ClientNode.CreateLongSortedSetNode);
#else
            LocalResult<ISortedSetNodeLocalClientNode<long>> node = await client.GetOrCreateSortedSetNode<long>(typeof(ISortedSetNodeLocalClientNode<long>).FullName);
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
            LocalResult<bool> boolResult = await node.Value.Add(2);
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
            LocalResult<ValueResult<long>> longResult = await node.Value.GetMin();
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            longResult = await node.Value.GetMax();
            if (!Program.Breakpoint(longResult)) return;
            if (longResult.Value.Value != 3)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
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
            completed(isReadWriteQueue);
        }
        private static void completed(bool isReadWriteQueue)
        {
            string readWriteQueue = isReadWriteQueue ? "ReadWriteQueue" : null;
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} {readWriteQueue} Completed*");
        }
    }
}
