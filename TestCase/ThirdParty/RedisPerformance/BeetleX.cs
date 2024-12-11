using AutoCSer;
using AutoCSer.Extensions;
using BeetleX.Redis;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// BeetleX.Redis 测试客户端
    /// </summary>
    internal sealed class BeetleX : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// BeetleX.Redis 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data, int taskCount = 1 << 13)
        {
            Left = AutoCSer.Random.Default.Next();

            await test(nameof(BeetleX.Set), data, taskCount);
            await test(nameof(BeetleX.Get), data, taskCount);
            await test(nameof(BeetleX.Remove), data, taskCount);
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// BeetleX.Redis 客户端测试
        /// </summary>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            BeetleX[] tasks = new BeetleX[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new BeetleX(data)) ;
            switch (serverMethodName)
            {
                case nameof(BeetleX.Set):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    foreach (BeetleX task in tasks) task.Set().NotWait();
                    break;
                case nameof(BeetleX.Get):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    foreach (BeetleX task in tasks) task.Get().NotWait();
                    break;
                case nameof(BeetleX.Remove):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    foreach (BeetleX task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(BeetleX), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly RedisDB client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private BeetleX(Data.Address data)
        {
            this.data = data.Clone();
            client = new RedisDB();
            client.DataFormater = new JsonFormater();
            client.Host.AddWriteHost("127.0.0.1", 6379);
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Set()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref BeetleX.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                data.StreetNumber = Left + next;
                                var key = await client.Set(data.StreetNumber.toString(), data);
                                if (key != null) ++success;
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
            using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref BeetleX.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                var data = await client.Get<Data.Address>((Left + next).toString());
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
            using (client)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref BeetleX.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                var result = await client.Del((Left + next).toString());
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
