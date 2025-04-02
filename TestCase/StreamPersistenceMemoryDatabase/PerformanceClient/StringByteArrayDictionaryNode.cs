using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 字典测试客户端
    /// </summary>
    internal sealed class StringByteArrayDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayDictionaryNodeClientNode<string>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateByteArrayDictionaryNode<string>(typeof(IByteArrayDictionaryNodeClientNode<string>).FullName));
        /// <summary>
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayDictionaryNodeClientNode<string>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateByteArrayDictionaryNode<string>(typeof(IByteArrayDictionaryNodeClientNode<string>).FullName));
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Test(Data.Address data)
        {
            await test(data, false);
            await test(data, true);
        }
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task test(Data.Address data, bool isReadWriteQueue)
        {
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayDictionaryNodeClientNode<string>> client = isReadWriteQueue ? readWriteQueueNodeCache : nodeCache;
            var node = await client.GetNode();
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            var synchronousNode = await client.GetSynchronousNode();//适合轻量级回调操作

            await node.Value.Clear();
            await test(synchronousNode.Value, nameof(StringByteArrayDictionaryNode.SetBinarySerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(StringByteArrayDictionaryNode.GetBinarySerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(StringByteArrayDictionaryNode.Remove), data, isReadWriteQueue);
            await node.Value.Clear();
            await test(synchronousNode.Value, nameof(StringByteArrayDictionaryNode.SetJsonSerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(StringByteArrayDictionaryNode.GetJsonSerialize), data, isReadWriteQueue);
            await test(synchronousNode.Value, nameof(StringByteArrayDictionaryNode.Remove), data, isReadWriteQueue);
            await node.Value.Clear();
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
        private static async Task test(IByteArrayDictionaryNodeClientNode<string> client, string serverMethodName, Data.Address data, bool isReadWriteQueue, int taskCount = 1 << 13)
        {
            StringByteArrayDictionaryNode[] tasks = new StringByteArrayDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new StringByteArrayDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(StringByteArrayDictionaryNode.SetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayDictionaryNode task in tasks) task.SetBinarySerialize().NotWait();
                    break;
                case nameof(StringByteArrayDictionaryNode.GetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
                case nameof(StringByteArrayDictionaryNode.SetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayDictionaryNode task in tasks) task.SetJsonSerialize().NotWait();
                    break;
                case nameof(StringByteArrayDictionaryNode.GetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayDictionaryNode task in tasks) task.GetJsonSerialize().NotWait();
                    break;
                case nameof(StringByteArrayDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayDictionaryNode task in tasks) task.Remove().NotWait();
                    break;
            }
            await Wait(nameof(StringByteArrayDictionaryNode), isReadWriteQueue ? $"{serverMethodName}+{nameof(IReadWriteQueueService)}" : serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IByteArrayDictionaryNodeClientNode<string> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private StringByteArrayDictionaryNode(IByteArrayDictionaryNodeClientNode<string> client, Data.Address data)
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
                            data.StreetNumber = left + next;
                            var result = await client.SetBinarySerialize(data.StreetNumber.toString(), data);
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetBinaryDeserialize<string, Data.Address>((left + next).toString());
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.Remove((left + next).toString());
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
                            data.StreetNumber = left + next;
                            var result = await client.SetJsonSerialize(data.StreetNumber.toString(), data);
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetJsonDeserialize<string, Data.Address>((left + next).toString());
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
