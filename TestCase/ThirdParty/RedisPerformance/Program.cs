using AutoCSer;
using AutoCSer.Net;
using Garnet;
using Garnet.server;
using System;

namespace RedisPerformance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Data.Address data = AutoCSer.RandomObject.Creator<Data.Address>.CreateNotNull();
            try
            {
                GarnetServerOptions options = new GarnetServerOptions
                {
                    Address = "127.0.0.1",
                    Port = 6379,
                    LogDir = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(GarnetServer), nameof(GarnetServerOptions.LogDir)),
                    CheckpointDir = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(GarnetServer), nameof(GarnetServerOptions.CheckpointDir)),
                    EnableAOF = true,
                    Recover = true,
                    EnableStorageTier = true,
                };
                await AutoCSer.Common.TryCreateDirectory(options.LogDir);
                await AutoCSer.Common.TryCreateDirectory(options.CheckpointDir);
                using (GarnetServer server = new GarnetServer(options))
                {
                    server.Start();

                    do
                    {
                        AutoCSer.TestCase.Common.ClientPerformance.Left = AutoCSer.Random.Default.Next();

                        await StackExchange.Test(data);
                        //StackExchange+Server.Set 8192 Concurrent Completed 27315ms 153/ms
                        //StackExchange+Server.Get 8192 Concurrent Completed 20372ms 205/ms
                        //StackExchange+Server.Remove 8192 Concurrent Completed 20097ms 208/ms

                        await BeetleX.Test(data);
                        //BeetleX+Server.Set 8192 Concurrent Completed 34639ms 30/ms
                        //BeetleX+Server.Get 8192 Concurrent Completed 28209ms 37/ms
                        //BeetleX+Server.Remove 8192 Concurrent Completed 27904ms 37/ms

                        await NewLife.Test(data);
                        //NewLife+Server.Set 8192 Concurrent Completed 62537ms 8/ms
                        //NewLife+Server.Get 8192 Concurrent Completed 25828ms 20/ms
                        //NewLife+Server.Remove 8192 Concurrent Completed 23156ms 22/ms

                        await CSRedisCore.Test(data);
                        //修改为单例模式以后还是有一定崩溃概率，一旦组件崩溃异常各种各样，无法恢复
                        //CSRedisCore+Server.Set 8192 Concurrent Completed 15017ms 17/ms
                        //CSRedisCore+Server.Get 8192 Concurrent Completed 14761ms 17/ms
                        //CSRedisCore+Server.Remove 8192 Concurrent Completed 14009ms 18/ms

                        await FreeRedis.Test(data);
                        //修改为单例模式以后还是有一定异常概率，异常以后无法恢复，应该是和 CSRedisCore 存在一样的网络数据解析 BUG
                        //FreeRedis+Server.Set 8192 Concurrent Completed 16701ms 15/ms
                        //FreeRedis+Server.Get 8192 Concurrent Completed 13692ms 19/ms
                        //FreeRedis+Server.Remove 8192 Concurrent Completed 13623ms 19/ms

                        //await ServiceStack.Test(data);//由于免费客户端版本无法满足测试需求，商业版本存在版权问题放弃测试

                        //await SharpRedis.Test(data);//SharpRedis.RedisConnectionException: Failed to connect to Redis, please check the network and Redis configuration.

                        Console.WriteLine("Press quit to exit.");
                        if (Console.ReadLine() == "quit") return;
                    }
                    while (true);
                }
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
                Console.ReadLine();
            }
        }
    }
}
