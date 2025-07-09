using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class OnlyPersistenceNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<ITestClassOnlyPersistenceNodeLocalClientNode> node = await client.GetOrCreateNode<ITestClassOnlyPersistenceNodeLocalClientNode>(typeof(ITestClassOnlyPersistenceNodeLocalClientNode).FullName, (index, nodeKey, nodeInfo) => client.ClientNode.CreateTestClassOnlyPersistenceNode(index, nodeKey, nodeInfo));
#else
            LocalResult<IOnlyPersistenceNodeLocalClientNode<TestClass>> node = await client.GetOrCreateOnlyPersistenceNode<TestClass>(typeof(IOnlyPersistenceNodeLocalClientNode<TestClass>).FullName);
#endif
            if (!Program.Breakpoint(node)) return;
            LocalResult result = await node.Value.Save(AutoCSer.RandomObject.Creator<TestClass>.CreateNotNull());
            if (!Program.Breakpoint(result)) return;
            node.Value.SaveSendOnly(AutoCSer.RandomObject.Creator<TestClass>.CreateNotNull());

            completed(isReadWriteQueue);
        }
        private static void completed(bool isReadWriteQueue)
        {
            string readWriteQueue = isReadWriteQueue ? "ReadWriteQueue" : null;
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} {readWriteQueue} Completed*");
        }
    }
}
