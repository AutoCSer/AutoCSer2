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
        /// Server registration component
        /// 服务注册组件
        /// </summary>
#if NetStandard21
        protected CommandServiceRegistrar? serviceRegistrar;
#else
        protected CommandServiceRegistrar serviceRegistrar;
#endif
        /// <summary>
        /// TCP socket
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// Get the socket listen address
        /// 获取套接字监听地址
        /// </summary>
#if NetStandard21
        public System.Net.EndPoint? EndPoint { get { return socket?.LocalEndPoint; } }
#else
        public System.Net.EndPoint EndPoint { get { return socket?.LocalEndPoint; } }
#endif
        /// <summary>
        /// Listen for asynchronous events of the socket
        /// 监听套接字异步事件
        /// </summary>
        protected SocketAsyncEventArgs listenAcceptEvent;
        /// <summary>
        /// Socket asynchronous event object pool
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
        /// Is trigger the quick close socket
        /// 是否触发快速关闭套接字
        /// </summary>
        protected bool isSocketDisposed;
        /// <summary>
        /// The service name is a unique identifier of the server registration. If the server registration is not required, it is only used for log output
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        public abstract string? ServerName { get; }
#else
        public abstract string ServerName { get; }
#endif
        /// <summary>
        /// The server listens to host and port information
        /// 服务监听主机与端口信息
        /// </summary>
        public abstract HostEndPoint Host { get; }
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        protected CommandListenerBase()
        {
            socket = CommandServerConfigBase.NullSocket;
            listenAcceptEvent = CommandServerConfigBase.NullSocketAsyncEventArgs;
        }
        /// <summary>
        /// Release resources
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
        /// Release resources
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
        /// Release resources
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
        /// Shut down the service after releasing the server registration component
        /// 释放服务注册组件以后关闭服务
        /// </summary>
        /// <param name="delayMilliseconds">Milliseconds to wait after releasing the server registration component
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
