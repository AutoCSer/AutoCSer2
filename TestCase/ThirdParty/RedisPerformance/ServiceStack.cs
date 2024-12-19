using AutoCSer;
using AutoCSer.Extensions;
using ServiceStack.Redis;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// ServiceStack.Redis 测试客户端
    /// </summary>
    internal sealed class ServiceStack : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// ServiceStack.Redis 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data)
        {
            //ServiceStack.LicenseException: The free-quota limit on '6000 Redis requests per hour' has been reached. Please see https://servicestack.net to upgrade to a commercial license or visit https://github.com/ServiceStackV3/ServiceStackV3 to revert back to the free ServiceStack v3.

            await test(nameof(ServiceStack.Set), data);
            await test(nameof(ServiceStack.Get), data);
            await test(nameof(ServiceStack.Remove), data);
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// StackExchange.Redis 测试客户端
        /// </summary>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            ServiceStack[] tasks = new ServiceStack[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new ServiceStack(data)) ;
            switch (serverMethodName)
            {
                case nameof(ServiceStack.Set):
                    right = Reset(null, maxTestCount >> 10, taskCount) >> LoopCountBit;
                    foreach (ServiceStack task in tasks) task.Set().NotWait();
                    break;
                case nameof(ServiceStack.Get):
                    right = Reset(null, maxTestCount >> 10, taskCount) >> LoopCountBit;
                    foreach (ServiceStack task in tasks) task.Get().NotWait();
                    break;
                case nameof(ServiceStack.Remove):
                    right = Reset(null, maxTestCount >> 10, taskCount) >> LoopCountBit;
                    foreach (ServiceStack task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(ServiceStack), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly RedisClient redisClient;
        /// <summary>
        /// 
        /// </summary>
        private readonly IRedisClientAsync client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 客户端同步返回结果
        /// </summary>
        /// <param name="data"></param>
        private ServiceStack(Data.Address data)
        {
            this.data = data.Clone();
            redisClient = new RedisClient("127.0.0.1", 6379);
            client = redisClient.AsAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Set()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            using (redisClient)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref ServiceStack.right);
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
            await System.Threading.Tasks.Task.Yield();
            using (redisClient)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref ServiceStack.right);
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
            await System.Threading.Tasks.Task.Yield();
            using (redisClient)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref ServiceStack.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                if (await client.RemoveAsync((Left + next).toString())) ++success;
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
