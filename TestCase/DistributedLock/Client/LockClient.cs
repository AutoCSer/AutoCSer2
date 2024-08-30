using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DistributedLockClient
{
    /// <summary>
    /// 分布式锁测试客户端
    /// </summary>
    internal sealed class LockClient
    {
        /// <summary>
        /// 分布式锁测试关键字
        /// </summary>
        private static readonly int lockKey = AutoCSer.Random.Default.Next();
        /// <summary>
        /// 分布式锁并发错误检查
        /// </summary>
        private static int checkLock;

        /// <summary>
        /// 分布式锁客户端
        /// </summary>
        private readonly DistributedLockClient<int> client;
        /// <summary>
        /// 客户端编号
        /// </summary>
        private readonly int clientID;
        /// <summary>
        /// 异步可重入锁客户端
        /// </summary>
        private DistributedLockAsynchronousReentrantClient<int> reentrantClient;
        /// <summary>
        /// 分布式锁测试客户端
        /// </summary>
        /// <param name="client">分布式锁客户端套接字事件</param>
        /// <param name="clientID">客户端编号</param>
        internal LockClient(IDistributedLockClientSocketEvent<int> client, int clientID)
        {
            this.client = new DistributedLockClient<int>(client);
            this.clientID = clientID;
        }
        /// <summary>
        /// 开始获取锁
        /// </summary>
        /// <returns></returns>
        internal async Task Start()
        {
            reentrantClient = client.GetAsynchronousReentrant();//注意要调用点的异步上下文中初始化，对于上层异步上下文无效
            do
            {
                CommandClientReturnValue<DistributedLockRequest<int>> request = await client.Enter(lockKey, 5);
                if (ConsoleWriteQueue.Breakpoint(request))
                {
                    int checkValue = System.Threading.Interlocked.Increment(ref checkLock);
                    if (checkValue != 1)
                    {
                        ConsoleWriteQueue.Breakpoint($"*ERROR+{clientID}.{checkValue}+ERROR*");
                        return;
                    }
                    await using (request.Value)
                    {
                        Console.Write(clientID);
                        checkValue = System.Threading.Interlocked.Decrement(ref checkLock);
                        if (checkValue != 0)
                        {
                            ConsoleWriteQueue.Breakpoint($"*ERROR-{clientID}.{checkValue}-ERROR*");
                            return;
                        }
                    }
                }

                CommandClientReturnValue<DistributedLockAsynchronousReentrant> reentrantLock = await reentrantClient.Enter(lockKey, 5, 5);
                if (ConsoleWriteQueue.Breakpoint(reentrantLock))
                {
                    int checkValue = System.Threading.Interlocked.Increment(ref checkLock);
                    if (checkValue != 1)
                    {
                        ConsoleWriteQueue.Breakpoint($"*ERROR+{clientID}.{checkValue}+ERROR*");
                        return;
                    }
                    await using (reentrantLock.Value)
                    {
                        await Reentrant();
                        checkValue = System.Threading.Interlocked.Decrement(ref checkLock);
                        if (checkValue != 0)
                        {
                            ConsoleWriteQueue.Breakpoint($"*ERROR-{clientID}.{checkValue}-ERROR*");
                            return;
                        }
                    }
                }
            }
            while (true);
        }
        /// <summary>
        /// 锁重入
        /// </summary>
        /// <returns></returns>
        private async Task Reentrant()
        {
            CommandClientReturnValue<DistributedLockAsynchronousReentrant> reentrantLock1 = await reentrantClient.Enter(lockKey, 5, 5);
            if (ConsoleWriteQueue.Breakpoint(reentrantLock1))
            {
                int checkValue = System.Threading.Interlocked.Increment(ref checkLock);
                if (checkValue != 2)
                {
                    ConsoleWriteQueue.Breakpoint($"*ERROR+{clientID}.{checkValue}+ERROR*");
                    return;
                }
                await using (reentrantLock1.Value)
                {
                    Console.Write(clientID);
                    checkValue = System.Threading.Interlocked.Decrement(ref checkLock);
                    if (checkValue != 1)
                    {
                        ConsoleWriteQueue.Breakpoint($"*ERROR-{clientID}.{checkValue}-ERROR*");
                        return;
                    }
                }
            }
        }
    }
}
