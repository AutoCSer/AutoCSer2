using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 分布式锁客户端示例
    /// </summary>
    internal static class DistributedLockNode
    {
        /// <summary>
        /// 分布式锁客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateDistributedLockNode<string>(nameof(DistributedLockNode));
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNodeClientNode<string> node = nodeResult.Value.notNull();

            string lockKey = AutoCSer.Random.Default.Next().toString();
            Task<bool>[] tasks = new Task<bool>[Math.Max(AutoCSer.Common.ProcessorCount, 4)];
            for (int index = 0; index != tasks.Length; ++index) tasks[index] = test(node, lockKey, 1 << 10);
            foreach (Task<bool> task in tasks)
            {
                bool result = await task;
                if (!result) return false;
            }

            return true;
        }
        /// <summary>
        /// 分布式锁并发错误检查
        /// </summary>
        private static int checkLock;
        /// <summary>
        /// 分布式锁客户端示例
        /// </summary>
        /// <param name="node"></param>
        /// <param name="lockKey"></param>
        /// <param name="loopCount"></param>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNodeClientNode<string> node, string lockKey, int loopCount)
        {
            await Task.Yield();
            for (int count = loopCount; count != 0; --count)
            {
                var identity = await node.Enter(lockKey, 5);//申请分布式锁 5 秒超时
                if (!identity.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (System.Threading.Interlocked.Increment(ref checkLock) != 1)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (System.Threading.Interlocked.Decrement(ref checkLock) != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                node.Release(lockKey, identity.Value).Discard();//释放分布式锁
            }
            return true;
        }
    }
}
