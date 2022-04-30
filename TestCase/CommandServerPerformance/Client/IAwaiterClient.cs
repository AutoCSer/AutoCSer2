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
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> Synchronous(int left, int right);
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> Callback(int left, int right);
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> Queue(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> Task(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> TaskQueue(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queueKey">队列关键字</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        ReturnCommand<int> TaskQueueKey(int queueKey, int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnly 应答处理
        /// </summary>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        EnumeratorCommand<int> KeepCallback();
        /// <summary>
        /// 服务端配合 KeepCallback 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        SendOnlyCommand SendOnly(int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnlyTask 应答处理
        /// </summary>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
        EnumeratorCommand<int> KeepCallbackCount();
        /// <summary>
        /// 服务端配合 KeepCallbackCount 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous, IsInitobj = false)]
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
            CommandClientConfig<IAwaiterClient> commandClientConfig = new CommandClientConfig<IAwaiterClient> { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.Performance), CommandPoolBits = 16, CheckSeconds = 0 };
            using (CommandClient commandClient = new CommandClient(commandClientConfig, CommandClientInterfaceControllerCreator.GetCreator<IAwaiterClient, IService>()))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    Console.WriteLine("ERROR");
                    return;
                }
                Left = AutoCSer.Random.Default.Next();

                await new AwaiterClient(commandClient, nameof(Synchronous)).Wait();
                await new AwaiterClient(commandClient, nameof(Callback)).Wait();
                await new AwaiterClient(commandClient, nameof(Queue)).Wait();
                await new AwaiterClient(commandClient, nameof(Task)).Wait();
                await new AwaiterClient(commandClient, nameof(TaskQueue)).Wait();
                await new AwaiterClient(commandClient, nameof(TaskQueueKey)).Wait();

                CommandClientSocketEvent<IAwaiterClient> client = (CommandClientSocketEvent<IAwaiterClient>)commandClient.SocketEvent;

                int testCount = Reset(commandClient);
                EnumeratorCommand<int> enumeratorCommand = await client.InterfaceController.KeepCallback();
                AutoCSer.Threading.CatchTask.AddIgnoreException(checkEnumeratorCommand(enumeratorCommand));
                for (int right = testCount; right != 0; await client.InterfaceController.SendOnly(Left, --right)) ;
                await LoopCompleted(nameof(AwaiterClient), nameof(client.InterfaceController.KeepCallback));

                Reset(commandClient);
                enumeratorCommand = await client.InterfaceController.KeepCallbackCount();
                AutoCSer.Threading.CatchTask.AddIgnoreException(checkEnumeratorCommand(enumeratorCommand));
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
            right = Reset(commandClient, maxTestCount, taskCount);
            switch (serverMethodName)
            {
                case nameof(Synchronous):
                    while (--taskCount >= 0) AutoCSer.Threading.CatchTask.AddIgnoreException(Synchronous());
                    break;
                case nameof(Callback):
                    while (--taskCount >= 0) AutoCSer.Threading.CatchTask.AddIgnoreException(Callback());
                    break;
                case nameof(Queue):
                    while (--taskCount >= 0) AutoCSer.Threading.CatchTask.AddIgnoreException(Queue());
                    break;
                case nameof(Task):
                    while (--taskCount >= 0) AutoCSer.Threading.CatchTask.AddIgnoreException(Task());
                    break;
                case nameof(TaskQueue):
                    while (--taskCount >= 0) AutoCSer.Threading.CatchTask.AddIgnoreException(TaskQueue());
                    break;
                case nameof(TaskQueueKey):
                    while (--taskCount >= 0) AutoCSer.Threading.CatchTask.AddIgnoreException(TaskQueueKey());
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
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(await client.InterfaceController.Synchronous(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        private async Task Callback()
        {
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(await client.InterfaceController.Callback(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        private async Task Queue()
        {
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(await client.InterfaceController.Queue(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private async Task Task()
        {
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(await client.InterfaceController.Task(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private async Task TaskQueue()
        {
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(await client.InterfaceController.TaskQueue(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private async Task TaskQueueKey()
        {
            await System.Threading.Tasks.Task.Yield();
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(await client.InterfaceController.TaskQueueKey(0, Left, right));
                else return;
            }
            while (true);
        }
    }
}
