using AutoCSer.Configuration;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令客户端
    /// </summary>
    public class CommandClient : ICommandClient, IDisposable
    {
        /// <summary>
        /// 命令服务配置
        /// </summary>
        internal readonly CommandClientConfig Config;
        /// <summary>
        /// The service name is a unique identifier of the service registration. If the service registration is not required, it is only used for log output
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        public string? ServerName { get { return Config.ServerName; } }
#else
        public string ServerName { get { return Config.ServerName; } }
#endif
        /// <summary>
        /// Log processing interface
        /// 日志处理接口
        /// </summary>
        public ILog Log { get { return Config.Log; } }
        /// <summary>
        /// The service listens to host and port information
        /// 服务监听主机与端口信息
        /// </summary>
        public HostEndPoint Host { get { return Config.Host; } }
        /// <summary>
        /// 命令客户端套接字事件控制器属性绑定标识
        /// </summary>
        BindingFlags ICommandClient.ControllerCreatorBindingFlags { get { return Config.ControllerCreatorBindingFlags; } }
        /// <summary>
        /// 二进制反序列化配置参数
        /// </summary>
        internal readonly AutoCSer.BinarySerialize.DeserializeConfig BinaryDeserializeConfig;
        /// <summary>
        /// 接受数据缓存区池
        /// </summary>
        internal readonly ByteArrayPool ReceiveBufferPool;
        /// <summary>
        /// 发送数据缓存区池
        /// </summary>
        internal readonly ByteArrayPool SendBufferPool;
        /// <summary>
        /// 客户端套接字操作锁
        /// </summary>
#if DEBUG && NetStandard21
        [AllowNull]
#endif
        private readonly SemaphoreSlimLock socketLock;
        /// <summary>
        /// Command client socket event
        /// 命令客户端套接字事件
        /// </summary>
        public CommandClientSocketEvent SocketEvent { get; private set; }
        /// <summary>
        /// 服务注册客户端监听组件
        /// </summary>
#if NetStandard21
        private CommandClientServiceRegistrar? serviceRegistrar;
#else
        private CommandClientServiceRegistrar serviceRegistrar;
#endif
        /// <summary>
        /// 客户端控制器创建器集合
        /// </summary>
        internal LeftArray<CommandClientInterfaceControllerCreator> ControllerCreators;
        /// <summary>
        /// 客户端回调队列数组
        /// </summary>
        private KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>[] callbackQueues;
        /// <summary>
        /// 客户端回调队列数组访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock callbackQueueLock;

        /// <summary>
        /// Create a client socket update version number
        /// 创建客户端套接字更新版本号
        /// </summary>
        public int CreateVersion { get; internal set; }
        /// <summary>
        /// 验证连续失败次数
        /// </summary>
        internal int VerifyErrorCount;
        /// <summary>
        /// Gets the current client socket
        /// 获取当前客户端套接字
        /// </summary>
#if NetStandard21
        public CommandClientSocket? CurrentSocket { get; private set; }
#else
        public CommandClientSocket CurrentSocket { get; private set; }
#endif
        /// <summary>
        /// Determines whether the current client socket is closed
        /// 判断当前客户端套接字是否已经关闭
        /// </summary>
        public bool IsSocketClosed
        {
            get
            {
                return IsDisposed || CurrentSocket == null || CurrentSocket.IsClosed;
            }
        }
        /// <summary>
        /// 当前客户端套接字
        /// </summary>
#if NetStandard21
        private Task<CommandClientSocket?>? socketTask;
#else
        private Task<CommandClientSocket> socketTask;
#endif
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
#if NetStandard21
        private Task<CommandClientSocketEvent?>? socketEventTask;
#else
        private Task<CommandClientSocketEvent> socketEventTask;
#endif
        /// <summary>
        /// 正在创建的客户端套接字
        /// </summary>
#if NetStandard21
        private CommandClientSocket? createSocket;
#else
        private CommandClientSocket createSocket;
#endif
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 是否反向服务
        /// </summary>
        internal readonly bool IsReverse;
#if NetStandard21
        /// <summary>
        /// 默认空命令客户端
        /// </summary>
        private CommandClient()
        {
            Config = CommandClientConfig.Null;
            callbackQueues = EmptyArray<KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>>.Array;
            BinaryDeserializeConfig = BinaryDeserializer.DefaultConfig;
            SendBufferPool = ReceiveBufferPool = ByteArrayPool.GetPool(Config.ReceiveBufferSizeBits);
            SocketEvent = new CommandClientSocketEvent(this);
        }
        /// <summary>
        /// 默认空命令客户端
        /// </summary>
        internal static readonly CommandClient Null = new CommandClient();
#endif
        /// <summary>
        /// Command client
        /// 命令客户端
        /// </summary>
        /// <param name="config">Command service configuration
        /// 命令服务配置</param>
        /// <param name="creators">Client controller creator collection
        /// 客户端控制器创建器集合</param>
        public CommandClient(CommandClientConfig config, params CommandClientInterfaceControllerCreator[] creators)
        {
            if (config == null) throw new ArgumentNullException();
            Config = config;
            callbackQueues = EmptyArray<KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>>.Array;
            ReceiveBufferPool = ByteArrayPool.GetPool(config.ReceiveBufferSizeBits);
            SendBufferPool = ByteArrayPool.GetPool(config.SendBufferSizeBits);
            BinaryDeserializeConfig = config.GetBinaryDeserializeConfig();
            //serviceRegistrar = config.GetRegistrar(this);
            SocketEvent = config.GetSocketEvent(this) ?? new CommandClientSocketEvent(this);
            socketLock = new SemaphoreSlimLock(1, 1);
            ControllerCreators.SetEmpty();
            if (creators?.Length > 0)
            {
                AppendCreators(creators, ref ControllerCreators);
                config.AutoCreateSocket(this);
            }
            else
            {
                SocketEvent.AppendCreators(ref ControllerCreators);
                if (ControllerCreators.Length != 0) config.AutoCreateSocket(this);
            }
        }
        /// <summary>
        /// 反向命令客户端
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="creators"></param>
        internal CommandClient(CommandReverseListener listener, CommandClientInterfaceControllerCreator[] creators)
        {
            IsReverse = true;
            Config = listener.Config;
            //CreateVersion = 1;
            callbackQueues = EmptyArray<KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>>.Array;
            ReceiveBufferPool = ByteArrayPool.GetPool(Config.ReceiveBufferSizeBits);
            SendBufferPool = ByteArrayPool.GetPool(Config.SendBufferSizeBits);
            BinaryDeserializeConfig = Config.GetBinaryDeserializeConfig();
            SocketEvent = Config.GetSocketEvent(this) ?? new CommandClientSocketEvent(this);
            socketLock = new SemaphoreSlimLock(1, 1);
            ControllerCreators.SetEmpty();
            if (creators?.Length > 0)
            {
                AppendCreators(creators, ref ControllerCreators);
            }
            else
            {
                SocketEvent.AppendCreators(ref ControllerCreators);
            }
            if (ControllerCreators.Length == 0) throw new Exception($"反向服务 {Config.ServerName} 创建客户端失败，缺少客户端控制器信息");

        }
        ///// <summary>
        ///// 反向命令客户端
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <param name="socket"></param>
        //internal CommandClient(CommandReverseListener listener, Socket socket)
        //{
        //    Config = listener.Config;
        //    CreateVersion = 1;
        //    callbackQueues = EmptyArray<KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>>.Array;
        //    ReceiveBufferPool = listener.ReceiveBufferPool;
        //    SendBufferPool = listener.SendBufferPool;
        //    BinaryDeserializeConfig = listener.BinaryDeserializeConfig;
        //    SocketEvent = Config.GetSocketEvent(this) ?? new CommandClientSocketEvent(this);
        //    socketLock = new SemaphoreSlimLock(1, 1);
        //    ControllerCreators = listener.ControllerCreators;
        //    if (ControllerCreators.Length == 0)
        //    {
        //        SocketEvent.AppendCreators(ref ControllerCreators);
        //        if (ControllerCreators.Length != 0) listener.ControllerCreators = ControllerCreators;
        //    }
        //    if (ControllerCreators.Length != 0) createSocket = new CommandClientSocket(this, socket);
        //}
        /// <summary>
        /// 添加客户端控制器创建器集合
        /// </summary>
        /// <param name="creators"></param>
        /// <param name="controllerCreators"></param>
        internal static void AppendCreators(CommandClientInterfaceControllerCreator[] creators, ref LeftArray<CommandClientInterfaceControllerCreator> controllerCreators)
        {
            if ((creators.Length + controllerCreators.Length) > CommandListener.MaxControllerCount) throw new Exception(AutoCSer.Common.Culture.GetCommandClientControllerCountLimit(creators.Length + controllerCreators.Length, CommandListener.MaxControllerCount));
            controllerCreators.PrepLength(creators.Length);
            foreach (CommandClientInterfaceControllerCreator creator in creators)
            {
                foreach (CommandClientInterfaceControllerCreator controllerCreator in controllerCreators)
                {
                    if (controllerCreator.ControllerName == creator.ControllerName)
                    {
                        throw new Exception(AutoCSer.Common.Culture.GetCommandClientControllerNameRepeatedly(creator.ControllerName));
                    }
                }
                var controllerConstructorException = creator.ControllerConstructorException;
                if (controllerConstructorException != null) throw controllerConstructorException;
                controllerCreators.Add(creator);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            bool isDisposed = false;
            socketLock.Enter();
            try
            {
                if (!IsDisposed)
                {
                    isDisposed = IsDisposed = true;

                    var currentSocket = CurrentSocket;
                    if (currentSocket != null)
                    {
                        CurrentSocket = null;
                        socketTask = null;
                        socketEventTask = null;
                        currentSocket.Shutdown();
                    }
                    currentSocket = createSocket;
                    if (currentSocket != null)
                    {
                        createSocket = null;
                        currentSocket.Shutdown();
                    }
                    SocketEvent.OnDisposeClient();
                }
            }
            finally { socketLock.Exit(); }

            if (isDisposed)
            {
                callbackQueueLock.Enter();
                try
                {
                    if (callbackQueues.Length != 0)
                    {
                        foreach (KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink> queue in callbackQueues) queue.Key?.Close();
                        callbackQueues = EmptyArray<KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>>.Array;
                    }
                }
                finally { callbackQueueLock.ExitSleepFlag(); }

                serviceRegistrar?.Dispose();
            }
        }
        /// <summary>
        /// 自动启动连接
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AutoCreateSocket()
        {
            createSocket = new CommandClientSocket(this, Config.Host.IPEndPoint, CreateVersion = 1);
        }
        /// <summary>
        /// 验证服务更新版本号
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsCreateVersion(int version)
        {
            return !IsDisposed && (CreateVersion == version || IsReverse);
        }
        ///// <summary>
        ///// 反向客户端初始化操作
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal Task<bool> Start()
        //{
        //    return createSocket.notNull().Start();
        //}
        /// <summary>
        /// 服务连接失败
        /// </summary>
        /// <param name="endPoint"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task ConnectFail(IPEndPoint endPoint)
        {
            if (serviceRegistrar != null) return serviceRegistrar.ConnectFail(endPoint);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Wait for the service to listen to address
        /// 等待服务监听地址
        /// </summary>
        /// <returns>Whether to cancel a scheduled task
        /// 是否需要取消定时任务</returns>
#if NetStandard21
        public async ValueTask<bool> WaitServerEndPoint()
#else
        public async Task<bool> WaitServerEndPoint()
#endif
        {
            await socketLock.EnterAsync();
            try
            {
                if (createSocket != null || IsDisposed) return true;
                SocketEvent.ReleaseSocketWaitLock();
            }
            finally { socketLock.Exit(); }
            return false;
        }
        /// <summary>
        /// The server listens for address update notifications
        /// 服务端监听地址更新通知
        /// </summary>
        /// <param name="endPoint"></param>
        public void ServerEndPointChanged(IPEndPoint endPoint)
        {
            socketLock.Enter();
            try
            {
                if (!IsDisposed)
                {
                    if (createSocket != null)
                    {
                        if (!createSocket.ServerEndPointEquals(endPoint))
                        {
                            ++CreateVersion;
                            createSocket = new CommandClientSocket(createSocket, endPoint);
                        }
                    }
                    else createSocket = new CommandClientSocket(this, endPoint, CreateVersion = 1);
                }
            }
            finally { socketLock.Exit(); }
        }
        /// <summary>
        /// 获取发送数据缓存区池
        /// </summary>
        /// <returns>发送数据缓存区池</returns>
        ByteArrayPool ICommandClient.GetSendBufferPool() { return SendBufferPool; }
        /// <summary>
        /// Try to wait for a client socket
        /// 尝试等待客户端套接字
        /// </summary>
        /// <returns>Return null on failure
        /// 失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<CommandClientSocket?> GetSocketAsync()
#else
        public Task<CommandClientSocket> GetSocketAsync()
#endif
        {
            return socketTask ?? getSocketAsync();
        }
        /// <summary>
        /// 尝试客户端套接字，获取失败返回 null
        /// </summary>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        private async Task<CommandClientSocket?> getSocketAsync()
#else
        private async Task<CommandClientSocket> getSocketAsync()
#endif
        {
            var waitLock = default(System.Threading.SemaphoreSlim);
            await socketLock.EnterAsync();
            if (CurrentSocket == null && !IsDisposed)
            {
                try
                {
                    if (CreateVersion == 0 && !IsReverse)
                    {
                        serviceRegistrar = await Config.GetRegistrar(this);
                        var endPoint = await serviceRegistrar.GetServerEndPoint();
                        CreateVersion = 1;
                        if (endPoint != null) createSocket = new CommandClientSocket(this, endPoint, 1);
                    }
                    waitLock = SocketEvent.GetWaitLock();
                }
                finally { socketLock.Exit(); }
                await waitLock.WaitAsync();
            }
            else socketLock.Exit();
            return CurrentSocket;
        }
#if AOT
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure
        /// 失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task<CommandClientSocketEvent?> GetSocketEvent()
        {
            return socketEventTask ?? getSocketEvent();
        }
#else
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure
        /// 失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<CommandClientSocketEvent?> GetSocketEvent()
#else
        public Task<CommandClientSocketEvent> GetSocketEvent()
#endif
        {
            return socketEventTask ?? getSocketEvent();
        }
#endif
        /// <summary>
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        private async Task<CommandClientSocketEvent?> getSocketEvent()
#else
        private async Task<CommandClientSocketEvent> getSocketEvent()
#endif
        {
            if (await GetSocketAsync() != null) return SocketEvent;
            return null;
        }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Return null on failure
        /// 失败返回 null</returns>
#if NetStandard21
        public async Task<T?> GetSocketEvent<
#if AOT
            [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]
#endif
        T>()
#else
        public async Task<T> GetSocketEvent<T>()
#endif
            where T : CommandClientSocketEvent
        {
            if (await GetSocketAsync() != null) return SocketEvent.notNullCastType<T>();
            return null;
        }
        /// <summary>
        /// 命令客户端套接字初始化失败
        /// </summary>
        /// <param name="socket"></param>
        internal async ValueTask OnCreateError(CommandClientSocket socket)
        {
            await socketLock.EnterAsync();
            try
            {
                if (!IsDisposed && socket.CreateVersion == CreateVersion) SocketEvent.OnCreateError(socket);
            }
            finally { socketLock.Exit(); }
        }
        /// <summary>
        /// 关闭命令客户端套接字
        /// </summary>
        /// <param name="socket"></param>
        internal void OnClosed(CommandClientSocket socket)
        {
            socketLock.Enter();
            try
            {
                if (object.ReferenceEquals(socket, createSocket)) createSocket = null;
                bool isCurrentSocket = false;
                if (object.ReferenceEquals(socket, CurrentSocket))
                {
                    CurrentSocket = null;
                    socketTask = null;
                    socketEventTask = null;
                    isCurrentSocket = true;
                }
                if (!IsDisposed)
                {
                    if (!IsReverse)
                    {
                        if (socket.CreateVersion == CreateVersion) SocketEvent.OnClosed(socket);
                    }
                    else
                    {
                        if (CurrentSocket == null && !isCurrentSocket) SocketEvent.ReleaseSocketWaitLock();
                    }
                }
            }
            finally { socketLock.Exit(); }
        }
        /// <summary>
        /// 命令超时触发事件
        /// </summary>
        /// <param name="head">超时首节点</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void OnTimeout(Command? head)
#else
        internal void OnTimeout(Command head)
#endif
        {
            if (head != null) Command.CancelLink(head, CommandClientReturnTypeEnum.Timeout);
        }
        /// <summary>
        /// 套接字验证通过以后的处理
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal async Task<bool> OnMethodVerified(CommandClientSocket socket)
        {
            var closeSocket = default(CommandClientSocket);
            await socketLock.EnterAsync();
            try
            {
                if (!IsReverse)
                {
                    if (socket.CreateVersion != CreateVersion) return false;
                }
                else
                {
                    socket.CreateVersion = ++CreateVersion;
                    closeSocket = CurrentSocket;
                }
                if (!IsDisposed)
                {
                    SocketEvent.SetController(socket);
                    CurrentSocket = socket;
                    socketTask = AutoCSer.Common.GetCompletedTask(socket);
                    socketEventTask = AutoCSer.Common.GetCompletedTask(SocketEvent);
                    try
                    {
                        await SocketEvent.OnMethodVerified(socket);
                    }
                    finally { SocketEvent.ReleaseSocketWaitLock(); }
                    return true;
                }
            }
            finally
            {
                socketLock.Exit();
                closeSocket?.Close();
            }
            return false;
        }
        /// <summary>
        /// 套接字操作失败重新创建版本检测
        /// </summary>
        /// <returns></returns>
        internal bool CreateNewSocket(CommandClientSocket socket)
        {
            if (!IsDisposed)
            {
                socketLock.Enter();
                try
                {
                    if (socket.CreateVersion == CreateVersion && !IsDisposed)
                    {
                        ++CreateVersion;
                        createSocket = new CommandClientSocket(socket);
                        return true;
                    }
                }
                finally { socketLock.Exit(); }
            }
            return false;
        }
        /// <summary>
        /// 套接字操作失败重新创建版本检测
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> CreateNewSocketAsync(CommandClientSocket socket)
        {
            if (!IsDisposed)
            {
                await socketLock.EnterAsync();
                try
                {
                    if (socket.CreateVersion == CreateVersion && !IsDisposed)
                    {
                        ++CreateVersion;
                        createSocket = new CommandClientSocket(socket);
                        return true;
                    }
                }
                finally { socketLock.Exit(); }
            }
            return false;
        }
        /// <summary>
        /// 获取扩展控制器创建器
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal CommandClientInterfaceControllerCreator? GetControllerCreator(string controllerName)
#else
        internal CommandClientInterfaceControllerCreator GetControllerCreator(string controllerName)
#endif
        {
            foreach (CommandClientInterfaceControllerCreator creator in ControllerCreators.Skip(1))
            {
                if (creator.ControllerName == controllerName) return creator;
            }
            return null;
        }
        /// <summary>
        /// 获取客户端执行队列
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink> getClientCallQueue(int index)
        {
            callbackQueueLock.Enter();
            try
            {
                if (index >= callbackQueues.Length)
                {
                    if (!IsDisposed)
                    {
                        callbackQueueLock.SleepFlag = 1;
                        int length = callbackQueues.Length;
                        callbackQueues = AutoCSer.Common.GetCopyArray(callbackQueues, index + 1);
                        do
                        {
                            CommandClientCallQueue queue = new CommandClientCallQueue(this);
                            callbackQueues[length].Set(queue, queue.CreateLink());
                        }
                        while (++length <= index);
                    }
                    else return default(KeyValue<CommandClientCallQueue, CommandClientCallQueueLowPriorityLink>);
                }
            }
            finally { callbackQueueLock.ExitSleepFlag(); }
            return callbackQueues[index];
        }
        /// <summary>
        /// 获取客户端执行队列
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandClientCallQueue GetCommandClientCallQueue(int index)
        {
            return index < callbackQueues.Length ? callbackQueues[index].Key : getClientCallQueue(index).Key;
        }
        /// <summary>
        /// 获取客户端执行队列（低优先级）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandClientCallQueueLowPriorityLink GetCommandClientCallQueueLowPriority(int index)
        {
            return index < callbackQueues.Length ? callbackQueues[index].Value : getClientCallQueue(index).Value;
        }
    }
    /// <summary>
    /// Interface symmetry command client
    /// 接口对称命令客户端
    /// </summary>
    /// <typeparam name="T">Controller interface type
    /// 控制器接口类型</typeparam>
    public class CommandClient<T> : CommandClient where T : class
    {
        /// <summary>
        /// Interface symmetry command client
        /// 接口对称命令客户端
        /// </summary>
        /// <param name="config">Command service configuration
        /// 命令服务配置</param>
        public CommandClient(CommandClientConfig<T> config) : base(config) { }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure
        /// 失败返回 null</returns>
#if NetStandard21
        public new async Task<CommandClientSocketEvent<T>?> GetSocketEvent()
#else
        public new async Task<CommandClientSocketEvent<T>> GetSocketEvent()
#endif
        {
            if (await GetSocketAsync() != null) return (CommandClientSocketEvent<T>)SocketEvent;
            return null;
        }
    }
}
