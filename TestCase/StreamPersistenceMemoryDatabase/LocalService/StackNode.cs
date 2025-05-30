﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class StackNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<IStringStackNodeLocalClientNode> node = await client.GetOrCreateNode<IStringStackNodeLocalClientNode>(typeof(IStringStackNodeLocalClientNode).FullName, (index, nodeKey, nodeInfo) => client.ClientNode.CreateStringStackNode(index, nodeKey, nodeInfo, 0));
#else
            LocalResult<IStackNodeLocalClientNode<string>> node = await client.GetOrCreateStackNode<string>(typeof(IStackNodeLocalClientNode<string>).FullName, 0);
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
            string value = TestClass.RandomString();
            result = await node.Value.Push(value);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            LocalResult<bool> boolResult = await node.Value.Contains(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            LocalResult<ValueResult<string>> stringResult = await node.Value.TryPeek();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.TryPop();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.Push(value);
            if (!Program.Breakpoint(result)) return;
            completed(isReadWriteQueue);
        }
        private static void completed(bool isReadWriteQueue)
        {
            string readWriteQueue = isReadWriteQueue ? "ReadWriteQueue" : null;
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} {readWriteQueue} Completed*");
        }
    }
}
