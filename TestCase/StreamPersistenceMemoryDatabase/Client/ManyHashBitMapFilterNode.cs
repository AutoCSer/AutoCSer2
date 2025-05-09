using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class ManyHashBitMapFilterNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            AutoCSer.Algorithm.ManyHashBitMapCapacity capacity = new Algorithm.ManyHashBitMapCapacity(1 << 10, 2);
            int size = capacity.GetHashCapacity();
            ResponseResult<IManyHashBitMapFilterNodeClientNode> node = await client.GetOrCreateManyHashBitMapFilterNode(typeof(IManyHashBitMapFilterNodeClientNode).FullName, size);
            if (!Program.Breakpoint(node)) return;

            uint[] hashCodes = ManyHashBitMapFilter.GetHashCode2(TestClass.String7);
            ManyHashBitMapFilter.HashCodeToBits(size, hashCodes);
            var result = await node.Value.SetBits(size, hashCodes);
            if (!Program.Breakpoint(result)) return;

            var boolResult = await node.Value.CheckBits(size, hashCodes);
            if (!Program.Breakpoint(boolResult)) return;
            if (boolResult.Value != NullableBoolEnum.True)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
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
