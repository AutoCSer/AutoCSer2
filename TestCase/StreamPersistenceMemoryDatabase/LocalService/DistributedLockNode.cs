using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class DistributedLockNode
    {
        /// <summary>
        /// 分布式锁测试关键字
        /// </summary>
        private static readonly int lockKey = AutoCSer.Random.Default.Next();
        /// <summary>
        /// 分布式锁并发错误检查
        /// </summary>
        private static int checkLock;
        /// <summary>
        /// 循环调用次数
        /// </summary>
        private const int loopCount = 1 << 10;

        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client)
        {
            ResponseResult<IDistributedLockNodeLocalClientNode<int>> node = await client.GetOrCreateNode<IDistributedLockNodeLocalClientNode<int>>(typeof(IDistributedLockNodeLocalClientNode<int>).FullName, client.ClientNode.CreateDistributedLockNode);
            if (!Program.Breakpoint(node)) return;

            Task[] tasks = new Task[10];
            for (int index = 0; index != 10; ++index) tasks[index] = Test(node.Value);
            await Task.WhenAll(tasks);

            completed();
        }
        private static async Task Test(IDistributedLockNodeLocalClientNode<int> node)
        {
            for (int count = loopCount; count != 0; --count)
            {
                ResponseResult<long> identity = await node.Enter(lockKey, 5);
                if (!Program.Breakpoint(identity)) return;
                int checkValue = System.Threading.Interlocked.Increment(ref checkLock);
                if (checkValue != 1)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{checkValue}+ERROR*");
                    return;
                }
                checkValue = System.Threading.Interlocked.Decrement(ref checkLock);
                if (checkValue != 0)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR-{checkValue}-ERROR*");
                    return;
                }
                node.Release(lockKey, identity.Value);
            }
        }

        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
