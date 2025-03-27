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
    internal sealed class IntByteArrayFragmentDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IByteArrayFragmentDictionaryNodeClientNode<int>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateByteArrayFragmentDictionaryNode<int>(typeof(IByteArrayFragmentDictionaryNodeClientNode<int>).FullName));
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data)
        {
            var node = await nodeCache.GetNode();
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            var synchronousNode = await nodeCache.GetSynchronousNode();//适合轻量级回调操作

            await node.Value.ClearArray();
            await test(synchronousNode.Value, nameof(IntByteArrayFragmentDictionaryNode.SetBinarySerialize), data);
            await test(synchronousNode.Value, nameof(IntByteArrayFragmentDictionaryNode.GetBinarySerialize), data);
            await test(synchronousNode.Value, nameof(IntByteArrayFragmentDictionaryNode.Remove), data);
            await node.Value.ClearArray();
            await test(synchronousNode.Value, nameof(IntByteArrayFragmentDictionaryNode.SetJsonSerialize), data);
            await test(synchronousNode.Value, nameof(IntByteArrayFragmentDictionaryNode.GetJsonSerialize), data);
            await test(synchronousNode.Value, nameof(IntByteArrayFragmentDictionaryNode.Remove), data);
            await node.Value.ClearArray();
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
        private static async Task test(IByteArrayFragmentDictionaryNodeClientNode<int> client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            IntByteArrayFragmentDictionaryNode[] tasks = new IntByteArrayFragmentDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new IntByteArrayFragmentDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(IntByteArrayFragmentDictionaryNode.SetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayFragmentDictionaryNode task in tasks) task.SetBinarySerialize().NotWait();
                    break;
                case nameof(IntByteArrayFragmentDictionaryNode.GetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayFragmentDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
                case nameof(IntByteArrayFragmentDictionaryNode.SetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayFragmentDictionaryNode task in tasks) task.SetJsonSerialize().NotWait();
                    break;
                case nameof(IntByteArrayFragmentDictionaryNode.GetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayFragmentDictionaryNode task in tasks) task.GetJsonSerialize().NotWait();
                    break;
                case nameof(IntByteArrayFragmentDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (IntByteArrayFragmentDictionaryNode task in tasks) task.Remove().NotWait();
                    break;
            }
            await Wait(nameof(IntByteArrayFragmentDictionaryNode), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IByteArrayFragmentDictionaryNodeClientNode<int> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private IntByteArrayFragmentDictionaryNode(IByteArrayFragmentDictionaryNodeClientNode<int> client, Data.Address data)
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
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayFragmentDictionaryNode.right);
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
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayFragmentDictionaryNode.right);
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
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayFragmentDictionaryNode.right);
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
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayFragmentDictionaryNode.right);
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
                    int right = System.Threading.Interlocked.Decrement(ref IntByteArrayFragmentDictionaryNode.right);
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
