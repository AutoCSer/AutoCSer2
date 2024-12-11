using AutoCSer;
using AutoCSer.Extensions;
using StackExchange.Redis;
using System;

namespace RedisPerformance
{
    /// <summary>
    /// StackExchange.Redis 测试客户端
    /// </summary>
    internal sealed class StackExchange : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// StackExchange.Redis 客户端测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        internal static async Task Test(Data.Address data, int taskCount = 1 << 11)
        {
            Left = AutoCSer.Random.Default.Next();

            await test(nameof(StackExchange.Set), data, taskCount);
            await test(nameof(StackExchange.Get), data, taskCount);
            await test(nameof(StackExchange.Remove), data, taskCount);
        }
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int right;
        /// <summary>
        /// StackExchange.Redis 客户端测试
        /// </summary>
        /// <param name="serverMethodName"></param>
        /// <param name="data"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private static async Task test(string serverMethodName, Data.Address data, int taskCount = 1 << 11)
        {
            StackExchange[] tasks = new StackExchange[taskCount];
            for (int index = 0; index != tasks.Length; tasks[index++] = new StackExchange(data)) ;
            switch (serverMethodName)
            {
                case nameof(StackExchange.Set):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    foreach (StackExchange task in tasks) task.Set().NotWait();
                    break;
                case nameof(StackExchange.Get):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    foreach (StackExchange task in tasks) task.Get().NotWait();
                    break;
                case nameof(StackExchange.Remove):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    foreach (StackExchange task in tasks) task.Get().NotWait();
                    break;
            }
            await Wait(nameof(StackExchange), serverMethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionMultiplexer connect;
        /// <summary>
        /// 
        /// </summary>
        private readonly IDatabase client;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly Data.Address data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private StackExchange(Data.Address data)
        {
            this.data = data.Clone();
            connect = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            client = connect.GetDatabase(0);
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Set()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            await using (connect)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref StackExchange.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                data.StreetNumber = Left + next;
                                if (await client.StringSetAsync(data.StreetNumber.toString(), AutoCSer.JsonSerializer.Serialize(data))) ++success;
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
            await using (connect)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref StackExchange.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                RedisValue value = await client.StringGetAsync((Left + next).toString());
                                var data = AutoCSer.JsonDeserializer.Deserialize<Data.Address>(((string?)value).notNull());
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
            await using(connect)
            {
                try
                {
                    do
                    {
                        int right = System.Threading.Interlocked.Decrement(ref StackExchange.right);
                        if (right >= 0)
                        {
                            int next = right << LoopCountBit;
                            do
                            {
                                if (await client.KeyDeleteAsync((Left + next).toString())) ++success;
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
