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
    internal sealed class ServerBinaryAddressFragmentDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client, Data.Address data)
        {
            ResponseResult<IFragmentDictionaryNodeClientNode<int, ServerBinary<Data.Address>>> node = await client.GetOrCreateNode<IFragmentDictionaryNodeClientNode<int, ServerBinary<Data.Address>>>(typeof(IFragmentDictionaryNodeClientNode<int, ServerBinary<Data.Address>>).FullName, client.ClientNode.CreateServerBinaryAddressFragmentDictionaryNode);
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            Left = AutoCSer.Random.Default.Next();

            await node.Value.ClearArray();
            await test(node.Value, nameof(ServerBinaryAddressFragmentDictionaryNode.SetBinarySerialize), data);
            await test(node.Value, nameof(ServerBinaryAddressFragmentDictionaryNode.GetBinarySerialize), data);
            await test(node.Value, nameof(ServerBinaryAddressFragmentDictionaryNode.Remove), data);
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
        private static async Task test(IFragmentDictionaryNodeClientNode<int, ServerBinary<Data.Address>> client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            ServerBinaryAddressFragmentDictionaryNode[] tasks = new ServerBinaryAddressFragmentDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new ServerBinaryAddressFragmentDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(ServerBinaryAddressFragmentDictionaryNode.SetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (ServerBinaryAddressFragmentDictionaryNode task in tasks) task.SetBinarySerialize().NotWait();
                    break;
                case nameof(ServerBinaryAddressFragmentDictionaryNode.GetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (ServerBinaryAddressFragmentDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
                case nameof(ServerBinaryAddressFragmentDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (ServerBinaryAddressFragmentDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
            }
            await Wait(nameof(ServerBinaryAddressFragmentDictionaryNode), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IFragmentDictionaryNodeClientNode<int, ServerBinary<Data.Address>> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private ServerBinaryAddressFragmentDictionaryNode(IFragmentDictionaryNodeClientNode<int, ServerBinary<Data.Address>> client, Data.Address data)
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
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref ServerBinaryAddressFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
                            data.StreetNumber = Left + next;
                            var result = await client.Set(data.StreetNumber, data);
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
                    int right = System.Threading.Interlocked.Decrement(ref ServerBinaryAddressFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetValue(Left + next);
                            if (result.Value.Value.Value != null) ++success;
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
                    int right = System.Threading.Interlocked.Decrement(ref ServerBinaryAddressFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.Remove(Left + next);
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
