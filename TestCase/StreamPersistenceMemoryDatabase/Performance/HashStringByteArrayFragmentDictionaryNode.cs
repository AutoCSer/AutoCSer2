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
    internal sealed class HashStringByteArrayFragmentDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client, Data.Address data)
        {
            ResponseResult<IHashStringFragmentDictionaryNodeClientNode<byte[]>> node = await client.GetOrCreateNode<IHashStringFragmentDictionaryNodeClientNode<byte[]>>(typeof(IHashStringFragmentDictionaryNodeClientNode<byte[]>).FullName, client.ClientNode.CreateFragmentHashStringByteArrayDictionaryNode);
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            Left = AutoCSer.Random.Default.Next();

            await node.Value.ClearArray();
            await test(node.Value, nameof(HashStringByteArrayFragmentDictionaryNode.SetBinarySerialize), data);
            await test(node.Value, nameof(HashStringByteArrayFragmentDictionaryNode.GetBinarySerialize), data);
            await test(node.Value, nameof(HashStringByteArrayFragmentDictionaryNode.Remove), data);
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
        private static async Task test(IHashStringFragmentDictionaryNodeClientNode<byte[]> client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            HashStringByteArrayFragmentDictionaryNode[] tasks = new HashStringByteArrayFragmentDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new HashStringByteArrayFragmentDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(HashStringByteArrayFragmentDictionaryNode.SetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (HashStringByteArrayFragmentDictionaryNode task in tasks) task.SetBinarySerialize().NotWait();
                    break;
                case nameof(HashStringByteArrayFragmentDictionaryNode.GetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (HashStringByteArrayFragmentDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
                case nameof(HashStringByteArrayFragmentDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (HashStringByteArrayFragmentDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
            }
            await Wait(nameof(HashStringByteArrayFragmentDictionaryNode), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IHashStringFragmentDictionaryNodeClientNode<byte[]> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private HashStringByteArrayFragmentDictionaryNode(IHashStringFragmentDictionaryNodeClientNode<byte[]> client, Data.Address data)
        {
            this.data = data.Clone();
            this.client = client;
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task SetBinarySerialize()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref HashStringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            data.StreetNumber = Left + next;
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
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref HashStringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetFromBinarySerialize<Data.Address>((Left + next).toString());
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
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref HashStringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.Remove((Left + next).toString());
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
