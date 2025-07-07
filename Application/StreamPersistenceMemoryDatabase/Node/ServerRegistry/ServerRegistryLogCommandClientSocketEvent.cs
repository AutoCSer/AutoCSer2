using System;
using System.Threading.Tasks;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The client socket event of the registration server
    /// 注册服务客户端套接字事件
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public abstract class ServerRegistryLogCommandClientSocketEvent<T> : AutoCSer.Net.CommandClientSocketEventTask<T>
        where T : ServerRegistryLogCommandClientSocketEvent<T>
    {
        /// <summary>
        /// A collection of clients for registration servers
        /// 注册服务客户端集合
        /// </summary>
        private LeftArray<ServerRegistryLogClient> clients;
        /// <summary>
        /// Access lock for the registration server client
        /// 注册服务客户端访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim clientLock;
        /// <summary>
        /// Does adding a registration server client require the registration of callback delegates
        /// 添加注册服务客户端是否需要注册回调委托
        /// </summary>
        private bool isLogCallback;
        /// <summary>
        /// The client socket event of the registration server
        /// 注册服务客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        protected ServerRegistryLogCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client)
        {
            clients = new LeftArray<ServerRegistryLogClient>(0);
            clientLock = new SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// Add the registration server client
        /// 添加注册服务客户端
        /// </summary>
        /// <param name="client"></param>
        public async Task Append(ServerRegistryLogClient client)
        {
            await clientLock.WaitAsync();
            try
            {
                if (!client.IsAppendClient)
                {
                    clients.Add(client);
                    client.IsAppendClient = true;
                    if (isLogCallback) await client.LogCallback();
                }
            }
            finally { clientLock.Release(); }
        }
        /// <summary>
        /// Remove the registration server client
        /// 移除注册服务客户端
        /// </summary>
        /// <param name="client"></param>
        public async Task Remove(ServerRegistryLogClient client)
        {
            await clientLock.WaitAsync();
            try
            {
                clients.RemoveToEnd(client);
            }
            finally { clientLock.Release(); }
        }
        /// <summary>
        /// Command Client socket custom client initialization operations after the authentication API is passed and the client controller is automatically bound, used to manually bind the client controller Settings and connection initialization operations, such as the initial keep callback. This call is located in the client lock operation, should not complete the initialization operation as soon as possible, do not call the internal nested lock operation to avoid deadlock
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override async Task OnMethodVerified(AutoCSer.Net.CommandClientSocket socket)
        {
            await clientLock.WaitAsync();
            try
            {
                isLogCallback = true;
                foreach (ServerRegistryLogClient client in clients) await client.LogCallback();
            }
            finally { clientLock.Release(); }
        }
    }
}
