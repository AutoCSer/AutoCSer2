using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server registration command client socket event
    /// 服务注册命令客户端套接字事件
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public abstract class ServerRegistryCommandClientSocketEvent<T> : AutoCSer.Net.CommandClientSocketEventTask<T>
        where T : ServerRegistryCommandClientSocketEvent<T>
    {
        /// <summary>
        /// The service registers the collection of components
        /// 服务注册组件集合
        /// </summary>
        private LeftArray<CommandServiceRegistrar> registrars;
        /// <summary>
        /// The access lock of the service registers components
        /// 服务注册组件访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim registrarLock;
        /// <summary>
        /// Does adding the server registration component require the registration of callback delegates
        /// 添加服务端注册组件是否需要注册回调委托
        /// </summary>
        private bool isServerCallback;
        /// <summary>
        /// Server registration command client socket event
        /// 服务注册命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        protected ServerRegistryCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client)
        {
            registrars = new LeftArray<CommandServiceRegistrar>(0);
            registrarLock = new SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// Add the server-side registration component
        /// 添加服务端注册组件
        /// </summary>
        /// <param name="registrar"></param>
        public async Task<AutoCSer.Net.CommandServiceRegistrar> Append(CommandServiceRegistrar registrar)
        {
            if (registrar.IsAppendRegistrar)
            {
                await registrarLock.WaitAsync();
                try
                {
                    registrars.Add(registrar);
                    if (isServerCallback) await registrar.ServerCallback();
                }
                finally { registrarLock.Release(); }
            }
            return registrar;
        }
        /// <summary>
        /// Remove the server-side registration component
        /// 移除服务端注册组件
        /// </summary>
        /// <param name="registrar"></param>
        internal async Task Remove(CommandServiceRegistrar registrar)
        {
            await registrarLock.WaitAsync();
            try
            {
                registrars.RemoveToEnd(registrar);
            }
            finally { registrarLock.Release(); }
        }
        /// <summary>
        /// Command Client socket custom client initialization operations after the authentication API is passed and the client controller is automatically bound, used to manually bind the client controller Settings and connection initialization operations, such as the initial keep callback. This call is located in the client lock operation, should not complete the initialization operation as soon as possible, do not call the internal nested lock operation to avoid deadlock
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override async Task OnMethodVerified(AutoCSer.Net.CommandClientSocket socket)
        {
            await registrarLock.WaitAsync();
            try
            {
                isServerCallback = true;
                foreach (CommandServiceRegistrar registrar in registrars) await registrar.ServerCallback();
            }
            finally { registrarLock.Release(); }
        }
    }
}
