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
                        await BeetleX.Test(data);
                        //BeetleX+Server.Set 8192 Concurrent Completed 37698ms 27/ms
                        //BeetleX+Server.Get 8192 Concurrent Completed 31063ms 33/ms
                        //BeetleX+Server.Remove 8192 Concurrent Completed 29677ms 35/ms

                        await NewLife.Test(data);
                        //NewLife+Server.Set 8192 Concurrent Completed 41239ms 12/ms
                        //NewLife+Server.Get 8192 Concurrent Completed 25673ms 20/ms
                        //NewLife+Server.Remove 8192 Concurrent Completed 24846ms 21/ms

                        await StackExchange.Test(data);
                        //客户端并发内存占用非常大
                        //StackExchange+Server.Set 2048 Concurrent Completed 59102ms 17/ms
                        //StackExchange+Server.Get 2048 Concurrent Completed 38462ms 27/ms
                        //StackExchange+Server.Remove 2048 Concurrent Completed 38075ms 27/ms

                        await CSRedisCore.Test(data);
                        //在高并发环境下该组件有一定崩溃概率，一旦组件崩溃异常各种各样，无法恢复
                        //修改为单例模式以后没有出现异常
                        //CSRedisCore+Server.Set 1024 Concurrent Completed 19048ms 13/ms
                        //CSRedisCore+Server.Get 1024 Concurrent Completed 17468ms 15/ms
                        //CSRedisCore+Server.Remove 1024 Concurrent Completed 16021ms 16/ms

                        await FreeRedis.Test(data);
                        //偶发性 convert failed 异常，异常以后无法恢复，应该是和 CSRedisCore 存在一样的网络数据解析 BUG
                        //修改为单例模式以后没有出现异常
                        //FreeRedis+Server.Set 8192 Concurrent Completed 27980ms 9/ms
                        //FreeRedis+Server.Get 8192 Concurrent Completed 26136ms 10/ms
                        //FreeRedis+Server.Remove 8192 Concurrent Completed 47175ms 5/ms

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
