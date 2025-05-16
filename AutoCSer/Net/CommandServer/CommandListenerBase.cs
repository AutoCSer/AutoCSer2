using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Net
{
    /// <summary>
    /// Command server to listen
    /// 命令服务端监听
    /// </summary>
    public abstract class CommandListenerBase : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 服务注册组件
        /// </summary>
#if NetStandard21
        protected CommandServiceRegistrar? serviceRegistrar;
#else
        protected CommandServiceRegistrar serviceRegistrar;
#endif
        /// <summary>
        /// TCP 套接字
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// 监听套接字异步事件
        /// </summary>
        protected SocketAsyncEventArgs listenAcceptEvent;
        /// <summary>
        /// 套接字异步事件对象池
        /// </summary>
        internal SocketAsyncEventArgsPool SocketAsyncEventArgsPool;
        /// <summary>
        /// Whether the service has been started
        /// 是否已启动服务
        /// </summary>
        public bool IsStart { get; protected set; }
        /// <summary>
        /// Is it closed
        /// 是否已经关闭
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 是否触发快速关闭套接字
        /// </summary>
        protected bool isSocketDisposed;
        /// <summary>
        /// The service name is a unique identifier of the service registration. If the service registration is not required, it is only used for log output
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        public abstract string? ServerName { get; }
#else
        public abstract string ServerName { get; }
#endif
        /// <summary>
        /// The service listens to host and port information
        /// 服务监听主机与端口信息
        /// </summary>
        public abstract HostEndPoint Host { get; }
        /// <summary>
        /// 命令服务
        /// </summary>
        protected CommandListenerBase()
        {
            socket = CommandServerConfigBase.NullSocket;
            listenAcceptEvent = CommandServerConfigBase.NullSocketAsyncEventArgs;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void dispose()
        {
            IsDisposed = isSocketDisposed = true;

            Socket socket = this.socket;
            if (!object.ReferenceEquals(socket, CommandServerConfigBase.NullSocket))
            {
                this.socket = CommandServerConfigBase.NullSocket;
                try
                {
                    if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally { socket.Dispose(); }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            dispose();

            var serviceRegister = this.serviceRegistrar;
            if (serviceRegister != null)
            {
                this.serviceRegistrar = null;
                try
                {
                    serviceRegister.Dispose();
                }
                catch { }
            }

            SocketAsyncEventArgsPool.Free();
            if (!object.ReferenceEquals(listenAcceptEvent, CommandServerConfigBase.NullSocketAsyncEventArgs)) listenAcceptEvent.Dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            dispose();

            var serviceRegister = this.serviceRegistrar;
            if (serviceRegister != null)
            {
                this.serviceRegistrar = null;
                try
                {
                    await serviceRegister.DisposeAsync();
                }
                catch { }
            }

            SocketAsyncEventArgsPool.Free();
            if (!object.ReferenceEquals(listenAcceptEvent, CommandServerConfigBase.NullSocketAsyncEventArgs)) listenAcceptEvent.Dispose();
        }
        /// <summary>
        /// Shut down the service after releasing the service registration component
        /// 释放服务注册组件以后关闭服务
        /// </summary>
        /// <param name="delayMilliseconds">Milliseconds to wait after releasing the service registration component
        /// 释放服务注册组件以后等待毫秒数</param>
        /// <returns></returns>
        public async Task DisposeServiceRegistrar(int delayMilliseconds = 1000)
        {
            var serviceRegister = this.serviceRegistrar;
            if (serviceRegister != null)
            {
                this.serviceRegistrar = null;
                try
                {
                    await serviceRegister.DisposeAsync();
                    await Task.Delay(delayMilliseconds);
                }
                catch { }
            }
            await ((IAsyncDisposable)this).DisposeAsync();
        }
        /// <summary>
        /// Fast close socket for service switching operations
        /// 快速关闭套接字，用于服务切换操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void DisposeSocket()
        {
            isSocketDisposed = true;
            if (!object.ReferenceEquals(socket, CommandServerConfigBase.NullSocket)) socket.Dispose();
        }
        /// <summary>
        /// Service offline notification
        /// 服务下线通知
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual void Offline() { }
    }
}
