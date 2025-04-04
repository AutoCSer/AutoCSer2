﻿using AutoCSer;
using AutoCSer.Extensions;
using FreeRedis;
using Newtonsoft.Json;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// FreeRedis 测试客户端
    /// </summary>
    internal sealed class FreeRedis : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// FreeRedis 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data, int taskCount = 1 << 13)
        {
            //测试代码样例由 https://www.zhihu.com/people/xie-xiang-qin-75 提供
            using (RedisClient client = new RedisClient("127.0.0.1:6379,poolsize=200,min poolsize=50"))
            {
                client.Serialize = obj => JsonConvert.SerializeObject(obj);
                client.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
                client.Ping();

                await test(client, nameof(FreeRedis.Set), data, taskCount);
                await test(client, nameof(FreeRedis.Get), data, taskCount);
                await test(client, nameof(FreeRedis.Remove), data, taskCount);
            }
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// FreeRedis 客户端测试
        /// </summary>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(RedisClient client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            FreeRedis[] tasks = new FreeRedis[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new FreeRedis(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(FreeRedis.Set):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (FreeRedis task in tasks) task.Set().NotWait();
                    break;
                case nameof(FreeRedis.Get):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (FreeRedis task in tasks) task.Get().NotWait();
                    break;
                case nameof(FreeRedis.Remove):
                    right = Reset(null, maxTestCount >> 8, taskCount) >> LoopCountBit;
                    foreach (FreeRedis task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(FreeRedis), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly RedisClient client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private FreeRedis(RedisClient client, Data.Address data)
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
                        int right = System.Threading.Interlocked.Decrement(ref FreeRedis.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                data.StreetNumber = Left + next;
                                await client.SetAsync(data.StreetNumber.toString(), data);
                                ++success;
                                //if () ++success;
                                //else ++error;
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
                        int right = System.Threading.Interlocked.Decrement(ref FreeRedis.right);
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
                        int right = System.Threading.Interlocked.Decrement(ref FreeRedis.right);
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
