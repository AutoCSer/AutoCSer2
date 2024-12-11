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
    internal sealed class HashStringFragmentDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client, Data.Address data)
        {
            ResponseResult<IHashStringFragmentDictionaryNodeClientNode<string>> node = await client.GetOrCreateNode<IHashStringFragmentDictionaryNodeClientNode<string>>(typeof(IHashStringFragmentDictionaryNodeClientNode<string>).FullName, client.ClientNode.CreateFragmentHashStringDictionaryNode);
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }
            Left = AutoCSer.Random.Default.Next();

            await node.Value.ClearArray();
            await test(node.Value, nameof(HashStringFragmentDictionaryNode.SetJsonString), data);
            await test(node.Value, nameof(HashStringFragmentDictionaryNode.GetJsonString), data);
            await test(node.Value, nameof(HashStringFragmentDictionaryNode.Remove), data);
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
        private static async Task test(IHashStringFragmentDictionaryNodeClientNode<string> client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            HashStringFragmentDictionaryNode[] tasks = new HashStringFragmentDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new HashStringFragmentDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(HashStringFragmentDictionaryNode.SetJsonString):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (HashStringFragmentDictionaryNode task in tasks) task.SetJsonString().NotWait();
                    break;
                case nameof(HashStringFragmentDictionaryNode.GetJsonString):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (HashStringFragmentDictionaryNode task in tasks) task.GetJsonString().NotWait();
                    break;
                case nameof(HashStringFragmentDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (HashStringFragmentDictionaryNode task in tasks) task.GetJsonString().NotWait();
                    break;
            }
            await Wait(nameof(HashStringFragmentDictionaryNode), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IHashStringFragmentDictionaryNodeClientNode<string> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private HashStringFragmentDictionaryNode(IHashStringFragmentDictionaryNodeClientNode<string> client, Data.Address data)
        {
            this.data = data.Clone();
            this.client = client;
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task SetJsonString()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref HashStringFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            data.StreetNumber = Left + next;
                            var result = await client.SetJson(data.StreetNumber.toString(), data);
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
        private async Task GetJsonString()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref HashStringFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetFromJson<Data.Address>((Left + next).toString());
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
                    int right = System.Threading.Interlocked.Decrement(ref HashStringFragmentDictionaryNode.right);
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
