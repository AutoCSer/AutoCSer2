using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.TestCase.CommandServerPerformance;
using AutoCSer.TestCase.Common;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令服务性能测试客户端接口（客户端 await 返回结果）
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IService))]
    public partial interface IAwaiterClient
    {
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> Synchronous(int left, int right);
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> Callback(int left, int right);
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> Queue(int left, int right);
        /// <summary>
        /// 服务端支持并发读队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> ConcurrencyReadQueue(int left, int right);
        /// <summary>
        /// 服务端读写队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> ReadWriteQueue(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> Task(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> SynchronousCallTask(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> TaskQueue(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queueKey">队列关键字</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> TaskQueueKey(int queueKey, int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnly 应答处理
        /// </summary>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        EnumeratorCommand<int> KeepCallback();
        /// <summary>
        /// 服务端配合 KeepCallback 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        SendOnlyCommand SendOnly(int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnlyTask 应答处理
        /// </summary>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        EnumeratorCommand<int> KeepCallbackCount();
        /// <summary>
        /// 服务端配合 KeepCallbackCount 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        SendOnlyCommand SendOnlyTask(int left, int right);
    }
    /// <summary>
    /// 客户端 await 返回结果
    /// </summary>
    internal sealed class AwaiterClientPerformance : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 客户端 await 返回结果
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            CommandClientConfig<IAwaiterClient> commandClientConfig = AutoCSer.TestCase.Common.JsonFileConfig.Default.IsCompressConfig
                ? new CommandClientCompressConfig<IAwaiterClient> { Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(Common.CommandServerPortEnum.Performance), CheckSeconds = 0 }
                : new CommandClientConfig<IAwaiterClient> { Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(Common.CommandServerPortEnum.Performance), CheckSeconds = 0 };
            using (CommandClient commandClient = new CommandClient(commandClientConfig, CommandClientInterfaceControllerCreator.GetCreator<IAwaiterClient, IService>()))
            {
                CommandClientSocketEvent<IAwaiterClient> client = await commandClient.GetSocketEvent<CommandClientSocketEvent<IAwaiterClient>>();
                if (client == null)
                {
                    ConsoleWriteQueue.WriteLine("ERROR", ConsoleColor.Red);
                    return;
                }
                //await forEachTask(client, false);
                //await forEachTask(client, true);
                //await s0611163(client, false);
                //await s0611163(client, true);

                //测试顺序是应用场景优先，不是性能优先
                int left = Left = AutoCSer.Random.Default.Next(), testCount;
                await new AwaiterClientPerformance(commandClient, nameof(ConcurrencyReadQueue), commandClientConfig.CommandQueueCount).Wait(); //服务端可并发读队列是内存数据库默认线程模式，允许重建获取快照数据操作的同时并发执行读请求队列操作以减少重建操作对吞吐性能的影响
                await new AwaiterClientPerformance(commandClient, nameof(ReadWriteQueue), commandClientConfig.CommandQueueCount).Wait(); //服务端读写队列是内存数据库的可选线程模式，允许多线程并发读取操作可以增加多核处理器环境下的吞吐量，由于任务分配较复杂所以会降低写操作的吞吐性能
                await new AwaiterClientPerformance(commandClient, nameof(Queue), commandClientConfig.CommandQueueCount).Wait(); //服务端队列模式适合轻量级的纯内存数据操作
                await new AwaiterClientPerformance(commandClient, nameof(Callback), commandClientConfig.CommandQueueCount).Wait(); //服务端回调模式适合基于事件触发的响应操作
                await new AwaiterClientPerformance(commandClient, nameof(SynchronousCallTask), commandClientConfig.CommandQueueCount).Wait(); //服务端 IO 线程同步调用 Task 模式适合异步 IO 任务并发操作，要保证在第一次触发异步操作之前不会产生大量计算阻塞 IO 线程，正确的使用方式是在大量计算操作之前主动切换线程调度避免阻塞 IO 线程
                await new AwaiterClientPerformance(commandClient, nameof(Task), commandClientConfig.CommandQueueCount).Wait(); //服务端自主调度 Task 模式适合异步 IO 任务并发操作，该模式会根据历史任务执行数据选择是否产生 IO 线程同步调用，非 IO 线程调度会降低系统吞吐性能
                await new AwaiterClientPerformance(commandClient, nameof(TaskQueueKey), commandClientConfig.CommandQueueCount).Wait(); //服务端基于关键字的 Task 队列模式适合数据库并发事务与缓存操作，队列操作可避免数据库资源竞争，避免并发死锁问题
                await new AwaiterClientPerformance(commandClient, nameof(Synchronous), commandClientConfig.CommandQueueCount).Wait(); //服务端同步模式适合轻量级的无阻塞操作，比如返回服务器当前时间
                await new AwaiterClientPerformance(commandClient, nameof(TaskQueue), commandClientConfig.CommandQueueCount).Wait(); //服务端 Task 队列适合全局性的数据库并发事务与缓存操作

                #region 服务端仅执行模式，异常会导致测试中断，属于正常现象（高性能需求场景应该使用 ICallbackClient.KeepCallback 获取 CommandKeepCallback，比如该测试中消费速度低于生产速度会导致服务端累积大量任务占用大量内存）
                testCount = Reset(commandClient, maxTestCount);
                EnumeratorCommand<int> enumeratorCommand = await client.InterfaceController.KeepCallback(); //服务端保持回调模式适合服务端无限制的推送数据操作，对于较大的集合也建议分拆成小数据流处理
                await using ((IAsyncDisposable)enumeratorCommand)
                {
                    checkEnumeratorCommand(enumeratorCommand).AutoCSerNotWait();
                    for (int right = testCount; right != 0; await client.InterfaceController.SendOnly(left, --right))
                    {
                        //if ((right & ((1 << 6) - 1)) == 0) AutoCSer.Threading.ThreadYield.YieldOnly();//降低生产速度测试
                    }
                    await LoopCompleted(nameof(AwaiterClientPerformance), nameof(client.InterfaceController.KeepCallback));
                }

                testCount = Reset(commandClient, maxTestCount);
                enumeratorCommand = await client.InterfaceController.KeepCallbackCount(); //服务端带计数的保持回调模式适合服务端推送数据操作，计数模式可以降低内存占用并且在一定程度上避免网络资源被独占，但是会降低吞吐性能
                await using ((IAsyncDisposable)enumeratorCommand)
                {
                    checkEnumeratorCommand(enumeratorCommand).AutoCSerNotWait();
                    for (int right = testCount; right != 0; await client.InterfaceController.SendOnlyTask(left, --right)) ;
                    await LoopCompleted(nameof(AwaiterClientPerformance), nameof(client.InterfaceController.KeepCallbackCount));
                }
                #endregion
            }
        }
        /// <summary>
        /// 客户端保持回调
        /// </summary>
        /// <param name="enumeratorCommand"></param>
        /// <returns></returns>
        private static async Task checkEnumeratorCommand(EnumeratorCommand<int> enumeratorCommand)
        {
            while (await enumeratorCommand.MoveNext()) CheckSynchronous(enumeratorCommand.Current);
        }
        /// <summary>
        /// https://www.zhihu.com/people/s0611163 在问题 https://www.zhihu.com/question/4877730905 评论中提供的测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isEmpty">是否轮空测试，Parallel.ForEachAsync 不适合做高性能并发测试</param>
        /// <returns></returns>
        private static async Task s0611163(CommandClientSocketEvent<IAwaiterClient> client, bool isEmpty)
        {
            long startTimestamp = Stopwatch.GetTimestamp();
            int count = 0;
            //await Parallel.ForEachAsync(Enumerable.Range(1, maxTestCount >> 2), new ParallelOptions { MaxDegreeOfParallelism = 8192 }, async (index, c) => //14:20 轮空测试耗时占比超过 2/3，Parallel.ForEachAsync 不适合做高性能并发测试
            await Parallel.ForEachAsync(Enumerable.Range(1, 1000000), new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (index, c) => //1.1:3.3 轮空测试耗时占比 1/3，Parallel.ForEachAsync 不适合做高性能并发测试
            {
                if (!isEmpty)
                {
                    var reply = (await client.InterfaceController.Synchronous(200, 500)).IsSuccess;
                }
                if ((System.Threading.Interlocked.Increment(ref count) % 100000) == 0)
                {
                    Console.WriteLine($"已完成：{count.ToString()}");
                }
            });
            Console.WriteLine($"耗时：{Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds.ToString("0.000")} 秒");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isEmpty">是否轮空测试，EnumerableTask{T} 自身开销对 AutoCSer RPC 测试影响不大</param>
        /// <returns></returns>
        private static async Task forEachTask(CommandClientSocketEvent<IAwaiterClient> client, bool isEmpty)
        {
            long startTimestamp = Stopwatch.GetTimestamp();
            int count = 0;
            await Enumerable.Range(1, maxTestCount >> 1).AutoCSerExtensions().EnumerableTask(8192, async index => //3.8:25.4 轮空测试耗时占比 15%
            //await Enumerable.Range(1, 1000000).enumerableTask(50, async index => //0.1:4.4 轮空测试耗时占比 2.3%
            {
                if (!isEmpty)
                {
                    var reply = (await client.InterfaceController.Synchronous(200, 500)).IsSuccess;
                }
                if ((System.Threading.Interlocked.Increment(ref count) % 100000) == 0)
                {
                    Console.WriteLine($"已完成：{count.ToString()}");
                }
            });
            Console.WriteLine($"耗时：{Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds.ToString("0.000")} 秒");
        }

        /// <summary>
        /// 命令服务性能测试客户端接口（客户端 await 返回结果）
        /// </summary>
        private readonly CommandClientSocketEvent<IAwaiterClient> client;
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
        private AwaiterClientPerformance(CommandClient commandClient, string serverMethodName, int taskCount = 1 << 13)
        {
            this.client = (CommandClientSocketEvent<IAwaiterClient>)commandClient.SocketEvent;
            this.serverMethodName = serverMethodName;
            switch (serverMethodName)
            {
                case nameof(Synchronous):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Synchronous().AutoCSerNotWait();
                    break;
                case nameof(Callback):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Callback().AutoCSerNotWait();
                    break;
                case nameof(Queue):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Queue().AutoCSerNotWait();
                    break;
                case nameof(ConcurrencyReadQueue):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) ConcurrencyReadQueue().AutoCSerNotWait();
                    break;
                case nameof(ReadWriteQueue):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) ReadWriteQueue().AutoCSerNotWait();
                    break;
                case nameof(TaskQueue):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) TaskQueue().AutoCSerNotWait();
                    break;
                case nameof(TaskQueueKey):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) TaskQueueKey().AutoCSerNotWait();
                    break;
                case nameof(Task):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Task().AutoCSerNotWait();
                    break;
                case nameof(SynchronousCallTask):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) SynchronousCallTask().AutoCSerNotWait();
                    break;
            }
        }
        /// <summary>
        /// 等待测试完成
        /// </summary>
        /// <returns></returns>
        internal async Task Wait()
        {
            await Wait(nameof(AwaiterClientPerformance), serverMethodName);
        }
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        private async Task Synchronous()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.Synchronous(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        private async Task Callback()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.Callback(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        private async Task Queue()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.Queue(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端支持并发读队列执行返回结果
        /// </summary>
        private async Task ConcurrencyReadQueue()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.ConcurrencyReadQueue(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端读写队列执行返回结果
        /// </summary>
        private async Task ReadWriteQueue()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.ReadWriteQueue(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private async Task Task()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.Task(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private async Task SynchronousCallTask()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.SynchronousCallTask(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private async Task TaskQueue()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.TaskQueue(left, next)).IsSuccess) ++success;
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
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private async Task TaskQueueKey()
        {
            int left = Left, success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0)
                {
                    int next = right << LoopCountBit;
                    do
                    {
                        if ((await client.InterfaceController.TaskQueueKey(left, left, next)).IsSuccess) ++success;
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
