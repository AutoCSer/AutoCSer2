﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class FragmentHashSetNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<IStringFragmentHashSetNodeLocalClientNode> node = await client.GetOrCreateNode<IStringFragmentHashSetNodeLocalClientNode>(typeof(IStringFragmentHashSetNodeLocalClientNode).FullName, client.ClientNode.CreateStringFragmentHashSetNode);
#else
            LocalResult<IFragmentHashSetNodeLocalClientNode<string>> node = await client.GetOrCreateFragmentHashSetNode<string>(typeof(IFragmentHashSetNodeLocalClientNode<string>).FullName);
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
            LocalResult<bool> boolResult = await node.Value.Add(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Add(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Contains(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Remove(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
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
            boolResult = await node.Value.Add(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            result = await node.Value.ClearArray();
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Contains(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.Add(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
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
