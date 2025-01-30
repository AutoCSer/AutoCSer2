using AutoCSer;
using AutoCSer.Extensions;
using NewLife.Caching;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// NewLife.Redis 测试客户端
    /// </summary>
    internal sealed class NewLife : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// NewLife.Redis 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data, int taskCount = 1 << 13)
        {
            using (FullRedis client = new FullRedis("127.0.0.1:6379", string.Empty, 0))
            {
                await test(client, nameof(NewLife.Set), data, taskCount);
                await test(client, nameof(NewLife.Get), data, taskCount);
                await test(client, nameof(NewLife.Remove), data, taskCount);
            }
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// NewLife.Redis 客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(FullRedis client, string serverMethodName, Data.Address data, int taskCount = 1 << 13)
        {
            NewLife[] tasks = new NewLife[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new NewLife(client, data)) ;
            switch (serverMethodName)
            {
                case nameof(NewLife.Set):
                    right = Reset(null, maxTestCount >> 7, taskCount) >> LoopCountBit;
                    foreach (NewLife task in tasks) task.Set().NotWait();
                    break;
                case nameof(NewLife.Get):
                    right = Reset(null, maxTestCount >> 7, taskCount) >> LoopCountBit;
                    foreach (NewLife task in tasks) task.Get().NotWait();
                    break;
                case nameof(NewLife.Remove):
                    right = Reset(null, maxTestCount >> 7, taskCount) >> LoopCountBit;
                    foreach (NewLife task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(NewLife), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly FullRedis client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="data"></param>
        private NewLife(FullRedis client, Data.Address data)
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
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref NewLife.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            data.StreetNumber = Left + next;
                            if (client.Set(data.StreetNumber.toString(), data)) ++success;
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
        private async Task Get()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    int right = System.Threading.Interlocked.Decrement(ref NewLife.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var data = client.Get<Data.Address>((Left + next).toString());
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
                    int right = System.Threading.Interlocked.Decrement(ref NewLife.right);
                    if (right >= 0)
                    {
                        int next = right << LoopCountBit;
                        do
                        {
                            var result = client.Remove((Left + next).toString());
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
