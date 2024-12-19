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
    internal sealed class StringByteArrayFragmentDictionaryNode : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 字典客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<IServiceNodeClientNode> client, Data.Address data)
        {
            ResponseResult<IByteArrayFragmentDictionaryNodeClientNode<string>> node = await client.GetOrCreateByteArrayFragmentDictionaryNode<string>(typeof(IByteArrayFragmentDictionaryNodeClientNode<string>).FullName);
            if (!node.IsSuccess)
            {
                ConsoleWriteQueue.Breakpoint();
                return;
            }

            await node.Value.ClearArray();
            await test(node.Value, nameof(StringByteArrayFragmentDictionaryNode.SetBinarySerialize), data);
            await test(node.Value, nameof(StringByteArrayFragmentDictionaryNode.GetBinarySerialize), data);
            await test(node.Value, nameof(StringByteArrayFragmentDictionaryNode.Remove), data);
            await node.Value.ClearArray();
            await test(node.Value, nameof(StringByteArrayFragmentDictionaryNode.SetJsonSerialize), data);
            await test(node.Value, nameof(StringByteArrayFragmentDictionaryNode.GetJsonSerialize), data);
            await test(node.Value, nameof(StringByteArrayFragmentDictionaryNode.Remove), data);
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
        private static async Task test(IByteArrayFragmentDictionaryNodeClientNode<string> client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            StringByteArrayFragmentDictionaryNode[] tasks = new StringByteArrayFragmentDictionaryNode[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new StringByteArrayFragmentDictionaryNode(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(StringByteArrayFragmentDictionaryNode.SetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayFragmentDictionaryNode task in tasks) task.SetBinarySerialize().NotWait();
                    break;
                case nameof(StringByteArrayFragmentDictionaryNode.GetBinarySerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayFragmentDictionaryNode task in tasks) task.GetBinarySerialize().NotWait();
                    break;
                case nameof(StringByteArrayFragmentDictionaryNode.SetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayFragmentDictionaryNode task in tasks) task.SetJsonSerialize().NotWait();
                    break;
                case nameof(StringByteArrayFragmentDictionaryNode.GetJsonSerialize):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayFragmentDictionaryNode task in tasks) task.GetJsonSerialize().NotWait();
                    break;
                case nameof(StringByteArrayFragmentDictionaryNode.Remove):
                    right = Reset(null, maxTestCount >> 4, taskCount) >> LoopCountBit;
                    foreach (StringByteArrayFragmentDictionaryNode task in tasks) task.Remove().NotWait();
                    break;
            }
            await Wait(nameof(StringByteArrayFragmentDictionaryNode), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IByteArrayFragmentDictionaryNodeClientNode<string> client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private StringByteArrayFragmentDictionaryNode(IByteArrayFragmentDictionaryNodeClientNode<string> client, Data.Address data)
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetBinaryDeserialize<string, Data.Address>((Left + next).toString());
                            if (result.Value.Value != null) ++success;
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
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayFragmentDictionaryNode.right);
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
        /// <summary>
        /// 
        /// </summary>
        private async Task SetJsonSerialize()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = this.data.Clone();
                            data.StreetNumber = Left + next;
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
            await System.Threading.Tasks.Task.Yield();
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref StringByteArrayFragmentDictionaryNode.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = await client.TryGetJsonDeserialize<string, Data.Address>((Left + next).toString());
                            if (result.Value.Value != null) ++success;
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
