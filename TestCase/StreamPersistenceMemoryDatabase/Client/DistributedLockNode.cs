using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
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

        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IDistributedLockNodeClientNode<int>> node = await client.GetOrCreateDistributedLockNode<int>(typeof(IDistributedLockNodeClientNode<int>).FullName);
            if (!Program.Breakpoint(node)) return;

            Task[] tasks = new Task[Math.Max(AutoCSer.Common.ProcessorCount, 4)];
            for (int index = 0; index != tasks.Length; ++index) tasks[index] = Test(node.Value);
            await Task.WhenAll(tasks);

            completed();
        }
        private static async Task Test(IDistributedLockNodeClientNode<int> node)
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
                node.Release(lockKey, identity.Value).Discard();
            }
        }

        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
