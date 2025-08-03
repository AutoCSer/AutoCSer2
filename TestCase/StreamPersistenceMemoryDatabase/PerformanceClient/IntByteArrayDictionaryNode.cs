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
    internal sealed class IntByteArrayDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayDictionaryNodeClientNode<int>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateByteArrayDictionaryNode<int>(typeof(IByteArrayDictionaryNodeClientNode<int>).FullName));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayDictionaryNodeClientNode<int>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateByteArrayDictionaryNode<int>(typeof(IByteArrayDictionaryNodeClientNode<int>).FullName));
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data)
        {
            await test(data, false);
            await test(data, true);
        }
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task test(Data.Address data, bool isReadWriteQueue)
        {
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayDictionaryNodeClientNode<int>> client = isReadWriteQueue ? readWriteQueueNodeCache : nodeCache;
            var node = await client.GetNode();
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            var synchronousNode = await client.GetSynchronousNode();//适合轻量级回调操作

            await node.Value.Clear();
            await test(synchronousNode.Value, nameof(IntByteArrayDictionaryNode.SetBinarySerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(IntByteArrayDictionaryNode.GetBinarySerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(IntByteArrayDictionaryNode.Remove), data, isReadWriteQueue);
            await node.Value.Clear();
            await test(synchronousNode.Value, nameof(IntByteArrayDictionaryNode.SetJsonSerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(IntByteArrayDictionaryNode.GetJsonSerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(IntByteArrayDictionaryNode.Remove), data, isReadWriteQueue);
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
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(IByteArrayDictionaryNodeClientNode<int> client, string serverMethodName, Data.Address data, bool isReadWriteQueue, int taskCount = 1 << 13)
        {
            IntByteArrayDictionaryNode[] tasks = new IntByteArrayDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new IntByteArrayDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(IntByteArrayDictionaryNode.SetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayDictionaryNode task in tasks) task.SetBinarySerialize().AutoCSerNotWait();
                    break;
                case nameof(IntByteArrayDictionaryNode.GetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayDictionaryNode task in tasks) task.GetBinarySerialize().AutoCSerNotWait();
                    break;
                case nameof(IntByteArrayDictionaryNode.SetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayDictionaryNode task in tasks) task.SetJsonSerialize().AutoCSerNotWait();
                    break;
                case nameof(IntByteArrayDictionaryNode.GetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayDictionaryNode task in tasks) task.GetJsonSerialize().AutoCSerNotWait();
                    break;
                case nameof(IntByteArrayDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayDictionaryNode task in tasks) task.Remove().AutoCSerNotWait();
                    break;
            }
            await Wait(nameof(IntByteArrayDictionaryNode), isReadWriteQueue ? $"{serverMethodName}+{nameof(IReadWriteQueueService)}" : serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IByteArrayDictionaryNodeClientNode<int> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private IntByteArrayDictionaryNode(IByteArrayDictionaryNodeClientNode<int> client, Data.Address data)
        {
            this.data = data;
            this.client = client;
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task SetBinarySerialize()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
                            data.StreetNumber = left + next;
                            var result = await client.SetBinarySerialize(data.StreetNumber, data);
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
        private async Task GetBinarySerialize()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetBinaryDeserialize<int, Data.Address>(left + next);
                            if (result.Value != null) ++success;
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
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayDictionaryNode.right);
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
        /// <summary>
        /// 
        /// </summary>
        private async Task SetJsonSerialize()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
                            data.StreetNumber = left + next;
                            var result = await client.SetJsonSerialize(data.StreetNumber, data);
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
        private async Task GetJsonSerialize()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetJsonDeserialize<int, Data.Address>(left + next);
                            if (result.Value != null) ++success;
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
