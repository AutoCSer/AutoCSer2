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
        private DistributedLockClient<int> client;
        /// <summary>
        /// 客户端编号
        /// </summary>
        private readonly int clientID;
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
            do
            {
                CommandClientReturnValue<DistributedLockRequest<int>> request = await client.EnterAsync(lockKey, 5);
                if (request.IsSuccess)
                {
                    int checkValue = System.Threading.Interlocked.Increment(ref checkLock);
                    if (checkValue != 1)
                    {
                        Console.Write($"*ERROR+{clientID}.{checkValue}+ERROR*");
                        return;
                    }
                    using (request.Value)
                    {
                        Console.Write(clientID);
                        checkValue = System.Threading.Interlocked.Decrement(ref checkLock);
                        if (checkValue != 0)
                        {
                            Console.Write($"*ERROR-{clientID}.{checkValue}-ERROR*");
                            return;
                        }
                    }
                }
                else Console.Write("X");
            }
            while (true);
        }
    }
}
