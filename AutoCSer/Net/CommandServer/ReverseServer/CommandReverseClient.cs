using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 反向命令服务客户端（发起连接端）
    /// </summary>
    public class CommandReverseClient : CommandListener, ICommandClient
    {
        /// <summary>
        /// 反向命令服务客户端配置
        /// </summary>
        private readonly CommandReverseClientConfig config;
        /// <summary>
        /// 命令客户端套接字事件控制器属性绑定标识
        /// </summary>
        BindingFlags ICommandClient.ControllerCreatorBindingFlags { get { return config.ControllerCreatorBindingFlags; } }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        public CommandClientSocketEvent SocketEvent { get; private set; }
        /// <summary>
        /// 命令客户端套接字事件任务缓存
        /// </summary>
        private readonly Task<CommandClientSocketEvent> socketEventTask;
        /// <summary>
        /// 服务监听地址
        /// </summary>
        private IPEndPoint serverEndPoint;
        /// <summary>
        /// 当前命令服务套接字
        /// </summary>
        private CommandServerSocket currentSocket;
        /// <summary>
        /// 判断当前客户端套接字是否已经关闭
        /// </summary>
        public bool IsSocketClosed
        {
            get
            {
                return IsDisposed || object.ReferenceEquals(currentSocket, CommandServerSocket.CommandServerSocketContext) || currentSocket.IsClose;
            }
        }
        /// <summary>
        /// 服务注册客户端监听组件
        /// </summary>
#if NetStandard21
        private CommandClientServiceRegistrar? clientRegistrar;
#else
        private CommandClientServiceRegistrar clientRegistrar;
#endif
        /// <summary>
        /// 服务更新版本号
        /// </summary>
        private int createVersion;
        /// <summary>
        /// 反向命令服务客户端（发起连接端）
        /// </summary>
        /// <param name="config">命令服务配置</param>
        /// <param name="creators">服务控制器创建器集合</param>
        public CommandReverseClient(CommandReverseClientConfig config, params CommandServerInterfaceControllerCreator[] creators) : base(config, creators)
        {
            if (Controllers.Length == 0) throw new ArgumentException(AutoCSer.Common.Culture.GetReverseCommandServerNotFoundController(config.ServerName));
            this.config = config;
            currentSocket = CommandServerSocket.CommandServerSocketContext;
            serverEndPoint = CommandServerConfigBase.NullIPEndPoint;
            SocketEvent = config.GetSocketEvent(this) ?? new CommandClientSocketEvent(this);
            socketEventTask = Task.FromResult(SocketEvent);
            IsStart = true;
            config.CreateSocket(this);
        }
        /// <summary>
        /// 反向命令服务客户端
        /// </summary>
        /// <param name="config">命令服务配置</param>
        /// <param name="creators">服务控制器创建器集合</param>
        internal CommandReverseClient(CommandReverseClientConfig config, ref LeftArray<CommandServerInterfaceControllerCreator> creators) : base(config, ref creators)
        {
            if (Controllers.Length == 0) throw new ArgumentException(AutoCSer.Common.Culture.GetReverseCommandServerNotFoundController(config.ServerName));
            this.config = config;
            currentSocket = CommandServerSocket.CommandServerSocketContext;
            serverEndPoint = CommandServerConfigBase.NullIPEndPoint;
            SocketEvent = config.GetSocketEvent(this) ?? new CommandClientSocketEvent(this);
            socketEventTask = Task.FromResult(SocketEvent);
            IsStart = true;
            config.CreateSocket(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void dispose()
        {
            base.dispose();
            clientRegistrar?.Dispose();
            currentSocket.DisposeSocket();
        }
        /// <summary>
        /// 自动启动连接
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CreateSocket()
        {
            serverEndPoint = config.Host.IPEndPoint;
            create(Interlocked.Increment(ref createVersion)).NotWait();
        }
        /// <summary>
        /// 自动启动连接
        /// </summary>
        /// <returns></returns>
        public async Task CreateSocketAsync()
        {
            if (clientRegistrar == null) clientRegistrar = await config.GetRegistrar(this);
        }
        /// <summary>
        /// 启动连接
        /// </summary>
        /// <param name="createVersion"></param>
        /// <returns></returns>
        private async Task create(int createVersion)
        {
            int createErrorCount = 0, exceptionCount = 0;
            do
            {
                try
                {
                    if (createErrorCount != 0)
                    {
                        await SocketEvent.CreateSocketSleep(createErrorCount);
                        if (IsDisposed || createVersion != this.createVersion) break;
                    }
                    Socket socket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.ReceiveBufferSize = ReceiveBufferPool.Size;
                    socket.SendBufferSize = SendBufferPool.Size;
                    socket.NoDelay = config.NoDelay;
#if DotNet45
                    socket.Connect(serverEndPoint);
#else
                    await socket.ConnectAsync(serverEndPoint);
#endif
                    if (IsDisposed || createVersion != this.createVersion) break;

                    CommandServerSocket commandServerSocket = new CommandServerSocket(this, socket), closeSocket = Interlocked.Exchange(ref currentSocket, commandServerSocket);
                    commandServerSocket.Start();
                    closeSocket.DisposeSocket();
                    if (exceptionCount != 0) await SocketEvent.OnCreateSocketRetrySuccess(null, serverEndPoint, exceptionCount);
                    return;
                }
                catch (Exception exception)
                {
                    await SocketEvent.OnCreateSocketException(null, exception, serverEndPoint, ++exceptionCount);
                }
                ++createErrorCount;
            }
            while (!IsDisposed && createVersion == this.createVersion);
        }
        /// <summary>
        /// 等待服务监听地址
        /// </summary>
        /// <returns>是否需要取消定时任务</returns>
#if NetStandard21
        ValueTask<bool> ICommandClient.WaitServerEndPoint()
#else
        Task<bool> ICommandClient.WaitServerEndPoint()
#endif
        {
#if NetStandard21
#if NET8
            return ValueTask.FromResult(IsDisposed);
#else
            return AutoCSer.Common.GetCompletedValueTask(AutoCSer.Common.GetCompletedTask(IsDisposed));
#endif
#else
            return AutoCSer.Common.GetCompletedTask(IsDisposed);
#endif
        }
        /// <summary>
        /// 服务端监听地址更新通知
        /// </summary>
        /// <param name="endPoint"></param>
        void ICommandClient.ServerEndPointChanged(IPEndPoint endPoint)
        {
            if (!IsDisposed && (endPoint.Port != serverEndPoint.Port || endPoint.Address != serverEndPoint.Address))
            {
                serverEndPoint = endPoint;
                create(Interlocked.Increment(ref createVersion)).NotWait();
            }
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        /// <param name="socket"></param>
        internal override void OnClose(CommandServerSocket socket) 
        {
            if (!IsDisposed && object.ReferenceEquals(Interlocked.CompareExchange(ref currentSocket, CommandServerSocket.CommandServerSocketContext, socket), socket))
            {
                create(Interlocked.Increment(ref createVersion)).NotWait();
            }
        }
        /// <summary>
        /// 获取发送数据缓存区池
        /// </summary>
        /// <returns>发送数据缓存区池</returns>
        ByteArrayPool ICommandClient.GetSendBufferPool() { return SendBufferPool; }
        /// <summary>
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        public Task<CommandClientSocketEvent?> GetSocketEvent()
#else
        public Task<CommandClientSocketEvent> GetSocketEvent()
#endif
        {
#pragma warning disable CS8619
            return socketEventTask;
#pragma warning restore CS8619
        }
        /// <summary>
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        public async Task<T?> GetSocketEvent<T>()
#else
        public async Task<T> GetSocketEvent<T>()
#endif
            where T : CommandClientSocketEvent
        {
            return (await socketEventTask).castType<T>();
        }
    }
}
