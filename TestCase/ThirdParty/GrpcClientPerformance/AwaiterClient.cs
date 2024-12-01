using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.Common;
using Grpc.Net.Client;
using System;
using System.Diagnostics;
using System.Threading.Channels;

namespace GrpcClientPerformance
{
    /// <summary>
    /// 客户端 await 返回结果
    /// </summary>
    internal sealed class AwaiterClient : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 客户端 await 返回结果
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        internal static async Task Test(GrpcChannel channel)
        {
            Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

            //await forEachTask(client, false);
            //await forEachTask(client, true);
            //await s0611163(client, false);
            //await s0611163(client, true);

            Left = AutoCSer.Random.Default.Next();

            await new AwaiterClient(channel, nameof(Greeter.GreeterClient.Add)).Wait();
        }
        /// <summary>
        /// https://www.zhihu.com/people/s0611163 在问题 https://www.zhihu.com/question/4877730905 评论中提供的测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isEmpty">是否轮空测试，Parallel.ForEachAsync 自身开销对 .NET gRPC 测试影响不大</param>
        /// <returns></returns>
        private static async Task s0611163(Greeter.GreeterClient client, bool isEmpty)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int count = 0;
            //await Parallel.ForEachAsync(Enumerable.Range(1, 1000000), new ParallelOptions { MaxDegreeOfParallelism = 8192 }, async (index, c) => //0.84:46 轮空测试耗时占比 18%
            await Parallel.ForEachAsync(Enumerable.Range(1, 1000000), new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (index, c) => //0.84:27.2 轮空测试耗时占比 3%
            {
                if (!isEmpty)
                {
                    var reply = await client.AddAsync(new AddRequest { Left = 200, Right = 500 });
                }
                if ((System.Threading.Interlocked.Increment(ref count) % 100000) == 0)
                {
                    Console.WriteLine($"已完成：{count.ToString()}");
                }
            });
            Console.WriteLine($"耗时：{stopwatch.Elapsed.TotalSeconds.ToString("0.000")} 秒");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isEmpty">是否轮空测试，EnumerableTask{T} 自身开销对 .NET gRPC 测试影响不大</param>
        /// <returns></returns>
        private static async Task forEachTask(Greeter.GreeterClient client, bool isEmpty)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int count = 0;
            //await Enumerable.Range(1, 1000000).enumerableTask(8192, async index => //0.1:46 轮空测试耗时占比 0.22%
            await Enumerable.Range(1, 1000000).enumerableTask(50, async index => //0.1:27.4 轮空测试耗时占比 0.36%
            {
                if (!isEmpty)
                {
                    var reply = await client.AddAsync(new AddRequest { Left = 200, Right = 500 });
                }
                if ((System.Threading.Interlocked.Increment(ref count) % 100000) == 0)
                {
                    Console.WriteLine($"已完成：{count.ToString()}");
                }
            });
            Console.WriteLine($"耗时：{stopwatch.Elapsed.TotalSeconds.ToString("0.000")} 秒");
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly Greeter.GreeterClient client;
        /// <summary>
        /// 调用服务端方法名称
        /// </summary>
        private readonly string serverMethodName;
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private int right;
        /// <summary>
        /// 客户端同步返回结果
        /// </summary>
        /// <param name="commandClient"></param>
        /// <param name="serverMethodName"></param>
        /// <param name="taskCount"></param>
        private AwaiterClient(GrpcChannel channel, string serverMethodName, int taskCount = 1 << 13)
        {
            this.client = new Greeter.GreeterClient(channel);
            this.serverMethodName = serverMethodName;
            switch (serverMethodName)
            {
                case nameof(Greeter.GreeterClient.Add):
                    right = Reset(null, maxTestCount >> 6, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Add().NotWait();
                    break;
            }
        }
        /// <summary>
        /// 等待测试完成
        /// </summary>
        /// <returns></returns>
        internal async Task Wait()
        {
            await Wait(nameof(AwaiterClient), serverMethodName);
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task Add()
        {
            int left = Left, success = 0, error = 0;
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        var reply = await client.AddAsync(new AddRequest { Left = left, Right = next });
                        if (reply != null) ++success;
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
    }
}
