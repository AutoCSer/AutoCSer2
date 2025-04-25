using AutoCSer.Net;
using AutoCSer.TestCase.CommandServerPerformance;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令服务性能测试客户端接口（客户端同步返回结果）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IService))]
#endif
    public partial interface ISynchronousCllient
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
        /// 服务端支持并发读队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> ConcurrencyReadQueue(int left, int right);
        /// <summary>
        /// 服务端读写队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> ReadWriteQueue(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> Task(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> SynchronousCallTask(int left, int right);
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
    internal sealed class SynchronousCllientPerformance : AutoCSer.TestCase.Common.ClientPerformance
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
                CommandClientSocketEvent<ISynchronousCllient> client = await commandClient.GetSocketEvent<CommandClientSocketEvent<ISynchronousCllient>>();
                if (client == null)
                {
                    ConsoleWriteQueue.WriteLine("ERROR", ConsoleColor.Red);
                    return;
                }
                Left = AutoCSer.Random.Default.Next();

                //int testCount = Reset(commandClient, 10000);
                //for (int right = testCount; right != 0; CheckSynchronous(client.InterfaceController.Synchronous(Left, --right))) ;
                //await LoopCompleted(nameof(SynchronousCllient), nameof(client.InterfaceController.Synchronous));

                await new SynchronousCllientPerformance(commandClient, nameof(ConcurrencyReadQueue), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(ReadWriteQueue), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(Queue), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(Callback), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(SynchronousCallTask), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(Task), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(TaskQueueKey), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(Synchronous), commandClientConfig.CommandQueueCount).Wait();
                await new SynchronousCllientPerformance(commandClient, nameof(TaskQueue), commandClientConfig.CommandQueueCount).Wait();
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
        private SynchronousCllientPerformance(CommandClient commandClient, string serverMethodName, int threadCount = 1 << 10)
        {
            Action task = null;
            switch(serverMethodName)
            {
                case nameof(Synchronous): task = Synchronous; break;
                case nameof(Callback): task = Callback; break;
                case nameof(Queue): task = Queue; break;
                case nameof(ConcurrencyReadQueue): task = ConcurrencyReadQueue; break;
                case nameof(ReadWriteQueue): task = ReadWriteQueue; break;
                case nameof(Task): task = Task; break;
                case nameof(SynchronousCallTask): task = SynchronousCallTask; break;
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
            await Wait(nameof(SynchronousCllientPerformance), serverMethodName);
        }
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        private void Synchronous()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Synchronous(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        private void Callback()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Callback(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        private void Queue()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Queue(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端支持并发读队列执行返回结果
        /// </summary>
        private void ConcurrencyReadQueue()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.ConcurrencyReadQueue(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端读写队列执行返回结果
        /// </summary>
        private void ReadWriteQueue()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.ReadWriteQueue(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private void Task()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Task(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        private void SynchronousCallTask()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.Task(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private void TaskQueue()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.TaskQueue(left, right));
                else return;
            }
            while (true);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        private void TaskQueueKey()
        {
            int left = Left;
            do
            {
                int right = System.Threading.Interlocked.Decrement(ref this.right);
                if (right >= 0) CheckLock(client.InterfaceController.TaskQueueKey(0, left, right));
                else return;
            }
            while (true);
        }
    }
}
