using AutoCSer.Extensions;
using Grpc.Net.Client;
using System;

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
            Left = AutoCSer.Random.Default.Next();

            await new AwaiterClient(channel, nameof(Greeter.GreeterClient.Add)).Wait();
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
