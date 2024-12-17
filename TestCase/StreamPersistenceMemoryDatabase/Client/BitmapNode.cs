using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class BitmapNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            uint capacity = 10;
            ResponseResult<IBitmapNodeClientNode> node = await client.GetOrCreateBitmapNode(typeof(IBitmapNodeClientNode).FullName, capacity);
            if (!Program.Breakpoint(node)) return;
            ResponseResult<uint> uintResult = await node.Value.GetCapacity();
            if (!Program.Breakpoint(uintResult)) return;
            if (uintResult.Value != capacity)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{uintResult.Value}+ERROR*");
                return;
            }
            ResponseResult result = await node.Value.ClearMap();
            if (!Program.Breakpoint(result)) return;
            ResponseResult<ValueResult<int>> intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            ResponseResult<bool> boolResult = await node.Value.SetBit(capacity - 1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.InvertBit(capacity - 1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBitInvertBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            boolResult = await node.Value.ClearBit(capacity - 1);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBitSetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBitClearBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return;
            }
            intResult = await node.Value.GetBit(capacity - 1);
            if (!Program.Breakpoint(intResult)) return;
            if (intResult.Value.Value != 0)
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

