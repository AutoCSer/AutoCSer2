using AutoCSer;
using AutoCSer.Extensions;
using CSRedis;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// CSRedisCore 测试客户端
    /// </summary>
    internal sealed class CSRedisCore : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// CSRedisCore 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data, int taskCount = 1 << 13)
        {
            using (CSRedisClient client = new CSRedisClient("127.0.0.1:6379,poolsize=200,min poolsize=50"))
            {
                await test(client, nameof(CSRedisCore.Set), data, taskCount);
                await test(client, nameof(CSRedisCore.Get), data, taskCount);
                await test(client, nameof(CSRedisCore.Remove), data, taskCount);
            }
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// CSRedisCore 客户端测试
        /// </summary>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(CSRedisClient client, string serverMethodName, Data.Address data, int taskCount = 1 << 10)
        {
            CSRedisCore[] tasks = new CSRedisCore[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new CSRedisCore(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(CSRedisCore.Set):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (CSRedisCore task in tasks) task.Set().NotWait();
                    break;
                case nameof(CSRedisCore.Get):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (CSRedisCore task in tasks) task.Get().NotWait();
                    break;
                case nameof(CSRedisCore.Remove):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (CSRedisCore task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(CSRedisCore), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly CSRedisClient client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        private CSRedisCore(CSRedisClient client, Data.Address data)
        {
            this.data = data.Clone();
            this.client = client;
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Set()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            //using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref CSRedisCore.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                data.StreetNumber = Left + next;
                                if (await client.SetAsync(data.StreetNumber.toString(), data)) ++success;
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
                catch(Exception exception)
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
            await AutoCSer.Threading.SwitchAwaiter.Default;
            //using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref CSRedisCore.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                var data = await client.GetAsync<Data.Address>((Left + next).toString());
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
            await AutoCSer.Threading.SwitchAwaiter.Default;
            //using(client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref CSRedisCore.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                var result = await client.DelAsync((Left + next).toString());
                                if (result != 0) ++success;
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
