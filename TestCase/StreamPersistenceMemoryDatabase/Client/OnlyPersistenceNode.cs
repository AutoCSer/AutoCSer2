using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class OnlyPersistenceNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IOnlyPersistenceNodeClientNode<TestClass>> node = await client.GetOrCreateOnlyPersistenceNode<TestClass>(typeof(IOnlyPersistenceNodeClientNode<TestClass>).FullName);
            if (!Program.Breakpoint(node)) return;

            ResponseResult result = await node.Value.Save(AutoCSer.RandomObject.Creator<TestClass>.Create());
            if (!Program.Breakpoint(result)) return;
            node.Value.SaveSendOnly(AutoCSer.RandomObject.Creator<TestClass>.Create()).Discard();

            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
