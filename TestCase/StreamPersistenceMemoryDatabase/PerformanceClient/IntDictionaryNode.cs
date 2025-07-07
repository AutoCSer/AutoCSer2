using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 字典测试客户端
    /// </summary>
    internal sealed class IntDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IDictionaryNodeClientNode<int, int>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateDictionaryNode<int, int>(typeof(IDictionaryNodeClientNode<int, int>).FullName));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IDictionaryNodeClientNode<int, int>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateDictionaryNode<int, int>(typeof(IDictionaryNodeClientNode<int, int>).FullName));
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            await test(false);
            await test(true);
        }
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task test(bool isReadWriteQueue)
        {
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IDictionaryNodeClientNode<int, int>> client = isReadWriteQueue ? readWriteQueueNodeCache : nodeCache;
            var node = await client.GetNode();
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            var synchronousNode = await client.GetSynchronousNode();//适合轻量级回调操作

            await node.Value.Renew(maxTestCount >> 2);
            await test(synchronousNode.Value, nameof(IntDictionaryNode.Set), isReadWriteQueue);
            await test(node.Value, nameof(IntDictionaryNode.Get), isReadWriteQueue);
            await test(synchronousNode.Value, nameof(IntDictionaryNode.Remove), isReadWriteQueue);
            await node.Value.Renew(0);
            Console.WriteLine();
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverMethodName"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(IDictionaryNodeClientNode<int, int> client, string serverMethodName, bool isReadWriteQueue, int taskCount = 1 << 13)
        {
            IntDictionaryNode[] tasks = new IntDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new IntDictionaryNode(client)) ;
            switch (serverMethodName)
            {
                case nameof(IntDictionaryNode.Set):
                    right = Reset(null, maxTestCount >> 2, taskCount) >> LoopCountBit;
                    foreach (IntDictionaryNode task in tasks) task.Set().NotWait();
                    break;
                case nameof(IntDictionaryNode.Get):
                    right = Reset(null, maxTestCount >> 2, taskCount) >> LoopCountBit;
                    foreach (IntDictionaryNode task in tasks) task.Get().NotWait();
                    break;
                case nameof(IntDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 2, taskCount) >> LoopCountBit;
                    foreach (IntDictionaryNode task in tasks) task.Remove().NotWait();
                    break;
            }
            await Wait(nameof(IntDictionaryNode), isReadWriteQueue ? $"{serverMethodName}+{nameof(IReadWriteQueueService)}" : serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IDictionaryNodeClientNode<int, int> client;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private IntDictionaryNode(IDictionaryNodeClientNode<int, int> client)
        {
            this.client = client;
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Set()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.Set(left + next, next);
                            if (result.Value) ++success;
                            else ++error;
                        }
                        while ((--next & (LoopCount - 1)) != 0);
                    }
                    else
                    {
                        CheckLock(success, error);
                        return;
                    }
                }
                while (true);
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Get()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetValue(left + next);
                            if (result.Value.Value == next) ++success;
                            else ++error;
                        }
                        while ((--next & (LoopCount - 1)) != 0);
                    }
                    else
                    {
                        CheckLock(success, error);
                        return;
                    }
                }
                while (true);
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Remove()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.Remove(left + next);
                            if (result.Value) ++success;
                            else ++error;
                        }
                        while ((--next & (LoopCount - 1)) != 0);
                    }
                    else
                    {
                        CheckLock(success, error);
                        return;
                    }
                }
                while (true);
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
            }
        }
    }
}
