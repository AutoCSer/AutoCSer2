using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class UniformProbabilityTotalStatisticsNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IUniformProbabilityTotalStatisticsNodeClientNode> node = await client.GetOrCreateUniformProbabilityTotalStatisticsNode(typeof(UniformProbabilityTotalStatisticsNode).FullName, 8);
            if (!Program.Breakpoint(node)) return;

            var result = await node.Value.Append(AutoCSer.Random.Default.NextULong());
            if (!Program.Breakpoint(result)) return;

            var countResult = await node.Value.Count();
            if (!Program.Breakpoint(countResult)) return;

            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
