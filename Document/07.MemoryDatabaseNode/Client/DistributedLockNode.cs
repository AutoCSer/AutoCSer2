using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Example of distributed lock client node
    /// 分布式锁客户端节点示例
    /// </summary>
    internal static class DistributedLockNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNodeClientNode<string>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateDistributedLockNode<string>(nameof(DistributedLockNode)));
        /// <summary>
        /// Example of distributed lock client node
        /// 分布式锁客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNodeClientNode<string> node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();

            string lockKey = AutoCSer.Random.Default.Next().AutoCSerExtensions().ToString();
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
        /// Distributed lock concurrent error check data
        /// 分布式锁并发错误检查数据
        /// </summary>
        private static int checkLock;
        /// <summary>
        /// Example of distributed lock client node
        /// 分布式锁客户端节点示例
        /// </summary>
        /// <param name="node"></param>
        /// <param name="lockKey"></param>
        /// <param name="loopCount"></param>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNodeClientNode<string> node, string lockKey, int loopCount)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            for (int count = loopCount; count != 0; --count)
            {
                var identity = await node.Enter(lockKey, 5);
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
                node.Release(lockKey, identity.Value).Discard();
            }
            return true;
        }
    }
}
