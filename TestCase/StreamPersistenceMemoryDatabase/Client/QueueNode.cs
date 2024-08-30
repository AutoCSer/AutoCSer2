using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class QueueNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IQueueNodeClientNode<string>> node = await client.GetOrCreateNode<IQueueNodeClientNode<string>, int>(typeof(IQueueNodeClientNode<string>).FullName, 0, client.ClientNode.CreateQueueNode);
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
            string value = "value";
            result = await node.Value.Enqueue(value);
            if (!Program.Breakpoint(result)) return;
            intResult = await node.Value.Count();
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            ResponseResult<bool> boolResult = await node.Value.Contains(value);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            ResponseResult<ValueResult<string>> stringResult = await node.Value.TryPeek();
            if (!Program.Breakpoint(stringResult)) return;
            if (stringResult.Value.Value != value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            stringResult = await node.Value.TryDequeue();
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
            result = await node.Value.Enqueue(value);
            if (!Program.Breakpoint(result)) return;
            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
