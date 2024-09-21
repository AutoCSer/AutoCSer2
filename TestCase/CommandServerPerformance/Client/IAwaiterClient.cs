using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.CommandServerPerformance;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令服务性能测试客户端接口（客户端 await 返回结果）
    /// </summary>
    public interface IAwaiterClient
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
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<int> Task(int left, int right);
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
    internal sealed class AwaiterClient : Client
    {
        /// <summary>
        /// 客户端 await 返回结果
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            CommandClientConfig<IAwaiterClient> commandClientConfig = new CommandClientConfig<IAwaiterClient> { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Performance), CheckSeconds = 0 };
            using (CommandClient commandClient = new CommandClient(commandClientConfig, CommandClientInterfaceControllerCreator.GetCreator<IAwaiterClient, IService>()))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    ConsoleWriteQueue.WriteLine("ERROR", ConsoleColor.Red);
                    return;
                }
                Left = AutoCSer.Random.Default.Next();

                await new AwaiterClient(commandClient, nameof(Synchronous), commandClientConfig.CommandQueueCount).Wait();
                await new AwaiterClient(commandClient, nameof(Callback), commandClientConfig.CommandQueueCount).Wait();
                await new AwaiterClient(commandClient, nameof(Queue), commandClientConfig.CommandQueueCount).Wait();
                await new AwaiterClient(commandClient, nameof(TaskQueue), commandClientConfig.CommandQueueCount).Wait();
                await new AwaiterClient(commandClient, nameof(TaskQueueKey), commandClientConfig.CommandQueueCount).Wait();
                await new AwaiterClient(commandClient, nameof(Task), commandClientConfig.CommandQueueCount).Wait();

                CommandClientSocketEvent<IAwaiterClient> client = (CommandClientSocketEvent<IAwaiterClient>)commandClient.SocketEvent;

                int testCount = Reset(commandClient, maxTestCount);
                EnumeratorCommand<int> enumeratorCommand = await client.InterfaceController.KeepCallback();
                checkEnumeratorCommand(enumeratorCommand).NotWait();
                for (int right = testCount; right != 0; await client.InterfaceController.SendOnly(Left, --right)) ;
                await LoopCompleted(nameof(AwaiterClient), nameof(client.InterfaceController.KeepCallback));

                Reset(commandClient, maxTestCount);
                enumeratorCommand = await client.InterfaceController.KeepCallbackCount();
                checkEnumeratorCommand(enumeratorCommand).NotWait();
                for (int right = testCount; right != 0; await client.InterfaceController.SendOnly(Left, --right)) ;
                await LoopCompleted(nameof(AwaiterClient), nameof(client.InterfaceController.KeepCallbackCount));
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
        private AwaiterClient(CommandClient commandClient, string serverMethodName, int taskCount = 1 << 13)
        {
            this.client = (CommandClientSocketEvent<IAwaiterClient>)commandClient.SocketEvent;
            this.serverMethodName = serverMethodName;
            switch (serverMethodName)
            {
                case nameof(Synchronous):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Synchronous().NotWait();
                    break;
                case nameof(Callback):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Callback().NotWait();
                    break;
                case nameof(Queue):
                    right = Reset(commandClient, maxTestCount, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Queue().NotWait();
                    break;
                case nameof(TaskQueue):
                    right = Reset(commandClient, maxTestCount >> 1, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) TaskQueue().NotWait();
                    break;
                case nameof(TaskQueueKey):
                    right = Reset(commandClient, maxTestCount >> 1, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) TaskQueueKey().NotWait();
                    break;
                case nameof(Task):
                    right = Reset(commandClient, maxTestCount >> 1, taskCount) >> LoopCountBit;
                    while (--taskCount >= 0) Task().NotWait();
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
        /// 服务端同步返回结果
        /// </summary>
        private async Task Synchronous()
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
            await System.Threading.Tasks.Task.Yield();
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
            await System.Threading.Tasks.Task.Yield();
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
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private async Task Task()
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
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private async Task TaskQueue()
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
            await System.Threading.Tasks.Task.Yield();
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
