using AutoCSer;
using AutoCSer.Extensions;
using SharpRedis;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// SharpRedis 测试客户端
    /// </summary>
    internal sealed class SharpRedis : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// SharpRedis 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data, int taskCount = 1 << 10)
        {
            Left = AutoCSer.Random.Default.Next();

            using (Redis client = Redis.UseStandalone(option =>
            {
                option.Host = "127.0.0.1";
                option.Port = 6379;
            }))
            {
                await test(client, nameof(SharpRedis.Set), data, taskCount);
                await test(client, nameof(SharpRedis.Get), data, taskCount);
                await test(client, nameof(SharpRedis.Remove), data, taskCount);
            }
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// SharpRedis 客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(Redis client, string serverMethodName, Data.Address data, int taskCount = 1 << 10)
        {
            SharpRedis[] tasks = new SharpRedis[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new SharpRedis(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(SharpRedis.Set):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (SharpRedis task in tasks) task.Set().NotWait();
                    break;
                case nameof(SharpRedis.Get):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (SharpRedis task in tasks) task.Get().NotWait();
                    break;
                case nameof(SharpRedis.Remove):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (SharpRedis task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(SharpRedis), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly Redis client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        private SharpRedis(Redis client, Data.Address data)
        {
            this.client = client;
            this.data = data.Clone();
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Set()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            //using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref SharpRedis.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                data.StreetNumber = Left + next;
                                if (await client.String.SetAsync(data.StreetNumber.toString(), AutoCSer.JsonSerializer.Serialize(data))) ++success;
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
        /// <summary>
        /// 
        /// </summary>
        private async Task Get()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            //using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref SharpRedis.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                var json = await client.String.GetAsync((Left + next).toString());
                                var data = json != null ? AutoCSer.JsonDeserializer.Deserialize<Data.Address>(json) : null;
                                if (data != null) ++success;
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
        /// <summary>
        /// 
        /// </summary>
        private async Task Remove()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            //using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref SharpRedis.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                var result = await client.String.GetDelAsync((Left + next).toString());
                                if (result != null) ++success;
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
}
