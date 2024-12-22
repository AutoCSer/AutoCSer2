using AutoCSer.Net;
using AutoCSer.TestCase.CommandServerPerformance;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令服务性能测试客户端接口（客户端同步返回结果）
    /// </summary>
    public interface ISynchronousCllient
    {
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> Synchronous(int left, int right);
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> Callback(int left, int right);
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> Queue(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> Task(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> TaskQueue(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queueKey">队列关键字</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> TaskQueueKey(int queueKey, int left, int right);
    }
    /// <summary>
    /// 客户端同步返回结果
    /// </summary>
    internal sealed class SynchronousCllient : AutoCSer.TestCase.Common.ClientPerformance
    {
        /// <summary>
        /// 客户端同步返回结果测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            CommandClientConfig<ISynchronousCllient> commandClientConfig = new CommandClientConfig<ISynchronousCllient> { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Performance), CommandPoolBits = 11, CheckSeconds = 0, CommandQueueCount = 1 << 10 };
            using (CommandClient commandClient = new CommandClient(commandClientConfig, CommandClientInterfaceControllerCreator.GetCreator<ISynchronousCllient, IService>()))
            {
                CommandClientSocketEvent<ISynchronousCllient> client = (CommandClientSocketEvent<ISynchronousCllient>)await commandClient.GetSocketEvent();
                if (client == null)
                {
                    ConsoleWriteQueue.WriteLine("ERROR", ConsoleColor.Red);
                    return;
                }
                Left = AutoCSer.Random.Default.Next();

                //int testCount = Reset(commandClient, 10000);
                //for (int right = testCount; right != 0; CheckSynchronous(client.InterfaceController.Synchronous(Left, --right))) ;
                //await LoopCompleted(nameof(SynchronousCllient), nameof(client.InterfaceController.Synchronous));

                await new SynchronousCllient(commandClient, nameof(Synchronous), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllient(commandClient, nameof(Callback), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllient(commandClient, nameof(Queue), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllient(commandClient, nameof(TaskQueue), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllient(commandClient, nameof(TaskQueueKey), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllient(commandClient, nameof(Task), commandClientConfig.CommandQueueCount).Wait();
            }
        }

        /// <summary>
        /// 命令服务性能测试客户端接口（客户端同步返回结果）
        /// </summary>
        private readonly CommandClientSocketEvent<ISynchronousCllient> client;
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
        /// <param name="threadCount"></param>
        private SynchronousCllient(CommandClient commandClient, string serverMethodName, int threadCount = 1 << 10)
        {
            Action task = null;
            switch(serverMethodName)
            {
                case nameof(Synchronous): task = Synchronous; break;
                case nameof(Callback): task = Callback; break;
                case nameof(Queue): task = Queue; break;
                case nameof(Task): task = Task; break;
                case nameof(TaskQueue): task = TaskQueue; break;
                case nameof(TaskQueueKey): task = TaskQueueKey; break;
            }
            this.client = (CommandClientSocketEvent<ISynchronousCllient>)commandClient.SocketEvent;
            this.serverMethodName = serverMethodName;
            right = Reset(commandClient, maxTestCount >> 4, threadCount);
            while (--threadCount >= 0) AutoCSer.Threading.ThreadPool.TinyBackground.Start(task);
        }
        /// <summary>
        /// 等待测试完成
        /// </summary>
        /// <returns></returns>
        internal async Task Wait()
        {
            await Wait(nameof(SynchronousCllient), serverMethodName);
        }
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        private void Synchronous()
        {
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Synchronous(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        private void Callback()
        {
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Callback(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        private void Queue()
        {
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Queue(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private void Task()
        {
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Task(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private void TaskQueue()
        {
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.TaskQueue(Left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private void TaskQueueKey()
        {
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.TaskQueueKey(0, Left, right));
                else return;
            }
            while (true);
        }
    }
}
