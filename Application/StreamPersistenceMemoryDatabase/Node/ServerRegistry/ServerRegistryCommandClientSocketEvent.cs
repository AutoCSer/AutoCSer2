using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务注册命令客户端套接字事件
    /// </summary>
    /// <typeparam name="T">客户端套接字事件类型</typeparam>
    public abstract class ServerRegistryCommandClientSocketEvent<T> : AutoCSer.Net.CommandClientSocketEventTask<T>
        where T : ServerRegistryCommandClientSocketEvent<T>
    {
        /// <summary>
        /// 服务端注册组件集合
        /// </summary>
        private LeftArray<CommandServiceRegistrar> registrars;
        /// <summary>
        /// 服务端注册组件访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim registrarLock;
        /// <summary>
        /// 添加服务端注册组件是否需要注册回调委托
        /// </summary>
        private bool isServiceCallback;
        /// <summary>
        /// 服务注册命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        protected ServerRegistryCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client)
        {
            registrars = new LeftArray<CommandServiceRegistrar>(0);
            registrarLock = new SemaphoreSlim(1, 1);
        }
        /// <summary>
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
                    if (isServiceCallback) await registrar.ServiceCallback();
                }
                finally { registrarLock.Release(); }
            }
            return registrar;
        }
        /// <summary>
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
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override async Task OnMethodVerified(AutoCSer.Net.CommandClientSocket socket)
        {
            await registrarLock.WaitAsync();
            try
            {
                isServiceCallback = true;
                foreach (CommandServiceRegistrar registrar in registrars) await registrar.ServiceCallback();
            }
            finally { registrarLock.Release(); }
        }
    }
}
