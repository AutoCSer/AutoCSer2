using AutoCSer.Configuration;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command server to listen
    /// 命令服务端监听
    /// </summary>
    public class CommandListener : CommandListenerBase, ICommandListenerSession
    {
        /// <summary>
        /// 用户命令起始位置
        /// </summary>
        internal const int MethodStartIndex = 256;
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        internal const int CancelKeepMethodIndex = MethodStartIndex - 1;
        /// <summary>
        /// 连接心跳检测命令
        /// </summary>
        internal const int CheckMethodIndex = CancelKeepMethodIndex - 1;

        /// <summary>
        /// 获取控制器索引
        /// </summary>
        internal const int ControllerMethodIndex = CheckMethodIndex - 1;
        /// <summary>
        /// 自定义数据包命令
        /// </summary>
        internal const int CustomDataMethodIndex = ControllerMethodIndex - 1;
        /// <summary>
        /// 客户端流合并命令
        /// </summary>
        internal const int MergeMethodIndex = CustomDataMethodIndex - 1;
        /// <summary>
        /// 最小系统命令
        /// </summary>
        internal const int MinMethodIndex = MergeMethodIndex;
        /// <summary>
        /// 最大命令控制器数量
        /// </summary>
        internal const int MaxControllerCount = (1 << ((CommandServer.Command.MethodIndexBits) - CommandServerController.MaxCommandBits)) - 1;

        /// <summary>
        /// 是否已经启动套接字发送数据线程
        /// </summary>
        private static int isSocketBuildOutputThread;

        /// <summary>
        /// 命令服务配置
        /// </summary>
        internal readonly CommandServerConfig Config;
        /// <summary>
        /// Log processing instance
        /// 日志处理实例
        /// </summary>
        public ILog Log { get { return Config.Log; } }
        /// <summary>
        /// The service name is a unique identifier of the service registration. If the service registration is not required, it is only used for log output
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        public override string? ServerName { get { return Config.ServerName; } }
#else
        public override string ServerName { get { return Config.ServerName; } }
#endif
        /// <summary>
        /// Information about the server host and port
        /// 服务端主机与端口信息
        /// </summary>
        public override HostEndPoint Host { get { return Config.Host; } }
        /// <summary>
        /// 二进制反序列化配置参数
        /// </summary>
        internal readonly AutoCSer.BinarySerialize.DeserializeConfig BinaryDeserializeConfig;
        /// <summary>
        /// Asynchronous task queue collection management
        /// 异步任务队列集合管理
        /// </summary>
        public readonly CommandServerCallTaskQueueTypeSet TaskQueueSet;
        /// <summary>
        /// 接受数据缓存区池
        /// </summary>
        internal readonly ByteArrayPool ReceiveBufferPool;
        /// <summary>
        /// 发送数据缓存区池
        /// </summary>
        internal readonly ByteArrayPool SendBufferPool;
        /// <summary>
        /// 验证超时时钟周期
        /// </summary>
        internal readonly long VerifyTimeoutTicks;
        /// <summary>
        /// 最大输入数据长度
        /// </summary>
        internal readonly int MaxInputSize;
        /// <summary>
        /// 最大合并输入数据长度
        /// </summary>
        internal readonly int MaxMergeInputSize;
        /// <summary>
        /// 接收发送数据不完整时连续两次最小字节数
        /// </summary>
        internal readonly uint MinSocketSize;
        /// <summary>
        /// 发送数据缓存区最大字节大小
        /// </summary>
        internal readonly int SendBufferMaxSize;
        /// <summary>
        /// Command service socket User-defined session object operation interface
        /// 命令服务套接字自定义会话对象操作接口
        /// </summary>
        public readonly ICommandListenerSession SessionObject;
//        /// <summary>
//        /// Get listening address
//        /// 获取监听地址
//        /// </summary>
//#if NetStandard21
//        public EndPoint? EndPoint
//#else
//        public EndPoint EndPoint
//#endif
//        {
//            get { return socket.LocalEndPoint; }
//        }
        /// <summary>
        /// TCP 服务器端同步调用队列数组
        /// </summary>
        private KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue>[] callQueues;
        /// <summary>
        /// TCP 服务器端支持并行读的同步队列
        /// </summary>
        internal CommandServerCallConcurrencyReadQueue CallConcurrencyReadQueue;
        /// <summary>
        /// TCP 服务器端读写队列
        /// </summary>
        internal CommandServerCallReadQueue CallReadWriteQueue;
        /// <summary>
        /// 命令控制器访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock controllerLock;
        /// <summary>
        /// 命令服务控制器集合
        /// </summary>
        internal LeftArray<CommandServerController> Controllers;
        /// <summary>
        /// 主服务控制器
        /// </summary>
        internal CommandServerController Controller;
        /// <summary>
        /// 结束命令序号
        /// </summary>
        internal int CommandEndIndex
        {
            get { return Controllers.Array[Controllers.Length - 1].CommandEndIndex; }
        }
        /// <summary>
        /// Task.Run 允许并发数量
        /// </summary>
        private int taskRunConcurrent;
        /// <summary>
        /// 下线通知接口调用计数
        /// </summary>
        private int offlineCount;
        /// <summary>
        /// 是否已经接收到下线通知
        /// </summary>
        internal bool IsOffline;
        /// <summary>
        /// 默认空命令服务
        /// </summary>
        private CommandListener()
        {
            Config = CommandServerConfig.Null;
            SessionObject = this;
            callQueues = EmptyArray<KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue>>.Array;
            CallConcurrencyReadQueue = CommandServerCallConcurrencyReadQueue.Null;
            CallReadWriteQueue = CommandServerCallReadQueue.Null;
            TaskQueueSet = new CommandServerCallTaskQueueTypeSet(this);
            BinaryDeserializeConfig = BinaryDeserializer.DefaultConfig;
            SendBufferPool = ReceiveBufferPool = ByteArrayPool.GetPool(Config.ReceiveBufferSizeBits);
            Controller = new NullCommandServerController(this);
        }
        /// <summary>
        /// 默认空命令服务
        /// </summary>
        internal static readonly CommandListener Null = new CommandListener();
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        /// <param name="config"></param>
        private CommandListener(CommandServerConfig config)
        {
            Config = config;
            SessionObject = config.SessionObject ?? this;
            callQueues = EmptyArray<KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue>>.Array;
            CallConcurrencyReadQueue = CommandServerCallConcurrencyReadQueue.Null;
            CallReadWriteQueue = CommandServerCallReadQueue.Null;
            VerifyTimeoutTicks = config.VerifyTimeoutSeconds * TimeSpan.TicksPerSecond;
            TaskQueueSet = config.GetTaskQueueTypeSet(this);
            MinSocketSize = config.MinSocketSize;
            BinaryDeserializeConfig = config.GetBinaryDeserializeConfig();
            ReceiveBufferPool = ByteArrayPool.GetPool(config.ReceiveBufferSizeBits);
            SendBufferPool = ByteArrayPool.GetPool(config.SendBufferSizeBits);
            SendBufferMaxSize = config.SendBufferMaxSize;
            if (SendBufferMaxSize <= 0) SendBufferMaxSize = int.MaxValue;
            MaxInputSize = config.MaxInputSize;
            if (MaxInputSize <= 0) MaxMergeInputSize = MaxInputSize = int.MaxValue;
            else if ((MaxMergeInputSize = MaxInputSize + ReceiveBufferPool.Size) < 0) MaxMergeInputSize = int.MaxValue;
            taskRunConcurrent = Math.Max(config.MaxTaskRunConcurrent, 0);
            SocketAsyncEventArgsPool = new SocketAsyncEventArgsPool(config.SocketAsyncEventArgsMaxCount);
            Controller = Null.Controller;
            Controllers.SetEmpty();
            if (config.BuildOutputThread == CommandServerSocketBuildOutputThreadEnum.Queue && Interlocked.CompareExchange(ref isSocketBuildOutputThread, 1, 0) == 0)
            {
                CommandServerSocket.StartSocketBuildOutputThread();
            }
        }
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        /// <param name="config">Configure the command server
        /// 命令服务端配置</param>
        /// <param name="creators">Service controller creator collection
        /// 服务控制器创建器集合</param>
        public CommandListener(CommandServerConfig config, params CommandServerInterfaceControllerCreator[] creators) : this(config)
        {
            if (creators != null)
            {
                foreach (CommandServerInterfaceControllerCreator creator in creators) creator.Create(this);
            }
        }
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        /// <param name="config">Configure the command server
        /// 命令服务端配置</param>
        /// <param name="creators">Service controller creator collection
        /// 服务控制器创建器集合</param>
        internal CommandListener(CommandServerConfig config, ref LeftArray<CommandServerInterfaceControllerCreator> creators) : this(config)
        {
            foreach (CommandServerInterfaceControllerCreator creator in creators) creator.Create(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void dispose()
        {
            base.dispose();

            controllerLock.Enter();
            try
            {
                if (callQueues.Length != 0)
                {
                    foreach (KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue> queue in callQueues) queue.Key?.Close();
                    callQueues = EmptyArray<KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue>>.Array;
                }
                CallConcurrencyReadQueue.Close();
                CallReadWriteQueue.Close();
            }
            finally { controllerLock.Exit(); }

            foreach (CommandServerController controller in Controllers) controller.Close();

            TaskQueueSet.Close();
        }
        /// <summary>
        /// Gets the command service socket Custom Session object operation object
        /// 获取命令服务套接字自定义会话对象操作对象
        /// </summary>
        /// <typeparam name="T">User-defined session object operation types
        /// 自定义会话对象操作类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? GetSessionObject<T>()
#else
        public T GetSessionObject<T>()
#endif
        {
            if (object.ReferenceEquals(this, SessionObject)) return default(T);
            return SessionObject.castType<T>();
        }
        /// <summary>
        /// Service offline notification
        /// 服务下线通知
        /// </summary>
        public override void Offline()
        {
            IsOffline = true;
            if (!IsDisposed && offlineCount == 0) Dispose();
        }
        /// <summary>
        /// 增加下线通知接口调用计数
        /// </summary>
        /// <returns></returns>
        internal bool IncrementOfflineCount()
        {
            Interlocked.Increment(ref offlineCount);
            if (!IsOffline) return true;
            if (Interlocked.Decrement(ref offlineCount) == 0 && !IsDisposed) Dispose();
            return false;
        }
        /// <summary>
        /// 下线通知接口调用完毕
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void DecrementOfflineCount()
        {
#if DEBUG
            int offlineCount = Interlocked.Decrement(ref this.offlineCount);
            if (offlineCount <= 0)
            {
                if (IsOffline && !IsDisposed) Dispose();
                if (offlineCount < 0) throw new Exception();
            }
#else
            if (Interlocked.Decrement(ref offlineCount) == 0 && IsOffline && !IsDisposed) Dispose();
#endif
        }
        /// <summary>
        /// Task.Run 并发检查
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckTaskRunConcurrent()
        {
            if (taskRunConcurrent > 0)
            {
                Interlocked.Decrement(ref taskRunConcurrent);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 增加 Task.Run 并发
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TaskRunConcurrent()
        {
            Interlocked.Decrement(ref taskRunConcurrent);
        }
        /// <summary>
        /// 释放 Task.Run 并发
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeTaskRunConcurrent()
        {
            Interlocked.Increment(ref taskRunConcurrent);
        }
        /// <summary>
        /// Add command controller
        /// 添加命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controller">Controller interface operation example
        /// 控制器接口操作实例</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandServerController AppendController<T>(T controller, string? controllerName = null)
#else
        public CommandServerController AppendController<T>(T controller, string controllerName = null)
#endif
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controller, controllerName).Create(this);
        }
        /// <summary>
        /// Add command controller
        /// 添加命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandServerController AppendController<T>(Func<T> controllerCreator, string? controllerName = null)
#else
        public CommandServerController AppendController<T>(Func<T> controllerCreator, string controllerName = null)
#endif
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controllerCreator, controllerName).Create(this);
        }
        /// <summary>
        /// Add command controller
        /// 添加命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandServerController AppendController<T>(Func<CommandListener, T> controllerCreator, string? controllerName = null)
#else
        public CommandServerController AppendController<T>(Func<CommandListener, T> controllerCreator, string controllerName = null)
#endif
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controllerCreator, controllerName).Create(this);
        }
        /// <summary>
        /// Adding an asynchronous queue command controller
        /// 添加异步队列命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <typeparam name="KT">Asynchronous queue keyword type
        /// 异步队列关键字类型</typeparam>
        /// <param name="getTaskQueue">Gets the queue context delegate
        /// 获取队列上下文委托</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandServerController AppendController<T, KT>(Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue, string? controllerName = null)
#else
        public CommandServerController AppendController<T, KT>(Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue, string controllerName = null)
#endif
            where KT : IEquatable<KT>
        {
            return CommandServerInterfaceControllerCreator.GetCreator(getTaskQueue, controllerName).Create(this);
        }
        /// <summary>
        /// Add define asymmetric command controllers
        /// 添加定义非对称命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controller">Controller interface operation example
        /// 控制器接口操作实例</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerController AppendController<T>(string controllerName, T controller)
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controllerName, controller).Create(this);
        }
        /// <summary>
        /// Add define asymmetric command controllers
        /// 添加定义非对称命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerController AppendController<T>(string controllerName, Func<T> controllerCreator)
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controllerName, controllerCreator).Create(this);
        }
        /// <summary>
        /// Add define asymmetric command controllers
        /// 添加定义非对称命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerController AppendController<T>(string controllerName, Func<CommandListener, T> controllerCreator)
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controllerName, controllerCreator).Create(this);
        }
        /// <summary>
        /// Adding an asynchronous queue defines an asymmetric command controller
        /// 添加异步队列定义非对称命令控制器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <typeparam name="KT">Asynchronous queue keyword type
        /// 异步队列关键字类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="getTaskQueue">Gets the queue context delegate
        /// 获取队列上下文委托</param>
        /// <returns>Command controller
        /// 命令控制器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerController AppendController<T, KT>(string controllerName, Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue)
            where KT : IEquatable<KT>
        {
            return CommandServerInterfaceControllerCreator.GetCreator(controllerName, getTaskQueue).Create(this);
        }
        /// <summary>
        /// 添加命令控制器
        /// </summary>
        /// <param name="controller"></param>
        internal unsafe void Append(CommandServerController controller)
        {
            bool isController = false;
            controllerLock.EnterSleepFlag();
            try
            {
                foreach (CommandServerController checkController in Controllers)
                {
                    if (checkController.ControllerName == controller.ControllerName) throw new Exception(AutoCSer.Common.Culture.GetCommandServerControllerNameRepeatedly(controller.ControllerName));
                }
                if (Controllers.Length == 0)
                {
                    controller.SetCommandStartIndex(MethodStartIndex);
                    Controller = controller;
                }
                else
                {
                    if (Controllers.Length == MaxControllerCount) throw new Exception(AutoCSer.Common.Culture.GetCommandServerControllerCountLimit(MaxControllerCount));
                    if (controller.Methods.Length > CommandServerController.MaxCommandCount)
                    {
                        throw new Exception(AutoCSer.Common.Culture.GetCommandServerControllerMethodCountLimit(controller.ControllerName, controller.Methods.Length, CommandServerController.MaxCommandCount));
                    }
                    if (Controller.CommandEndIndex > CommandServerController.MaxCommandCount)
                    {
                        throw new Exception(AutoCSer.Common.Culture.GetCommandServerControllerMethodCountLimit(Controller.CommandEndIndex, CommandServerController.MaxCommandCount));
                    }
                    controller.SetCommandStartIndex(Controllers.Array[Controllers.Length - 1].CommandEndIndex);
                }
                Controllers.Add(controller);
                isController = true;
                controller.ControllerIndex = Controllers.Count - 1;
            }
            finally
            {
                controllerLock.ExitSleepFlag();
                if(!isController) controller.Close();
            }
            if (!object.ReferenceEquals(Controller, controller) && controller.VerifyMethodIndex != 0) Config.IgnoreVerifyMethod(controller);
        }
        /// <summary>
        /// Gets a collection of command service controllers
        /// 获取命令服务控制器集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IEnumerable<CommandServerController> GetControllers()
        {
            return Controllers;
        }
        /// <summary>
        /// Get the controller by its name
        /// 根据控制器名称获取控制器
        /// </summary>
        /// <param name="controllerName">Controller name
        /// 控制器名称</param>
        /// <returns>null is returned when no matching name is found
        /// 没有找到匹配名称返回 null</returns>
#if NetStandard21
        public CommandServerController? GetController(string controllerName)
#else
        public CommandServerController GetController(string controllerName)
#endif
        {
            foreach (CommandServerController controller in Controllers)
            {
                if (controller.ControllerName == controllerName) return controller;
            }
            return null;
        }
        /// <summary>
        /// Get the controller by its name
        /// 根据控制器名称获取控制器
        /// </summary>
        /// <param name="interfaceType">Controller interface type
        /// 控制器接口类型</param>
        /// <returns>null is returned when no matching name is found
        /// 没有找到匹配名称返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandServerController? GetController(Type interfaceType)
#else
        public CommandServerController GetController(Type interfaceType)
#endif
        {
            return GetController(interfaceType.FullName.notNull());
        }
        /// <summary>
        /// Start server listening
        /// 启动服务端监听
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Start()
        {
            if (!object.ReferenceEquals(Controller, Null.Controller))
            {
                controllerLock.Enter();
                try
                {
                    if (!IsStart && !IsDisposed)
                    {
                        controllerLock.SleepFlag = 1;
                        serviceRegistrar = await Config.GetRegistrar(this);
                        AutoCSer.Net.CommandServer.HostEndPoint endPoint = await serviceRegistrar.GetEndPoint();
                        socket = new Socket(endPoint.IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        //if (Config.IsTcpFastOpen) socket.SetSocketOption(SocketOptionLevel.Tcp, (SocketOptionName)15, true);
                        socket.Bind(endPoint.IPEndPoint);
                        listenAcceptEvent = new SocketAsyncEventArgs();
                        listenAcceptEvent.Completed += listenAcceptCompleted;
                        socket.Listen(int.MaxValue);
                        IsStart = true;
                        if (!socket.AcceptAsync(listenAcceptEvent)) listenAcceptCompleted(null, listenAcceptEvent);
                        //AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(acceptSocket);
                        await serviceRegistrar.OnListened(endPoint.RegisterHost);
                        return true;
                    }
                }
                finally { controllerLock.ExitSleepFlag(); }
            }
            return false;
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        /// <param name="socket"></param>
        internal virtual void OnClose(CommandServerSocket socket) { }
        ///// <summary>
        ///// 获取客户端请求套接字
        ///// </summary>
        //private void acceptSocket()
        //{
        //    Socket listenSocket = socket;
        //    while (!isSocketDisposed)
        //    {
        //        Socket serverSocket = null;
        //        try
        //        {
        //            do
        //            {
        //                serverSocket = listenSocket.Accept();
        //                serverSocket.NoDelay = true;
        //                if (Config.Verify(serverSocket, this)) new CommandServerSocket(this, serverSocket).Start();
        //                else serverSocket.Dispose();
        //            }
        //            while (!IsDisposed);
        //        }
        //        catch (Exception exception)
        //        {
        //            if (isSocketDisposed) return;
        //            AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
        //            System.Threading.Thread.Sleep(1);
        //        }
        //        finally
        //        {
        //            if (serverSocket != null) serverSocket.Dispose();
        //        }
        //    }
        //}
        /// <summary>
        /// 获取客户端请求套接字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listenAcceptEvent"></param>
#if NetStandard21
        private void listenAcceptCompleted(object? sender, SocketAsyncEventArgs listenAcceptEvent)
#else
        private void listenAcceptCompleted(object sender, SocketAsyncEventArgs listenAcceptEvent)
#endif
        {
            Socket listenSocket = socket;
            var serverSocket = default(Socket);
            do
            {
                try
                {
                    while (listenAcceptEvent.SocketError == SocketError.Success)
                    {
                        serverSocket = listenAcceptEvent.AcceptSocket.notNull();
                        if (Config.Verify(serverSocket, this)) new CommandServerSocket(this, serverSocket).Start();
                        else serverSocket.Dispose();
                        serverSocket = null;
                        if (isSocketDisposed) return;
                        listenAcceptEvent.AcceptSocket = null;
                        if (listenSocket.AcceptAsync(listenAcceptEvent)) return;
                    }
                    if (isSocketDisposed) return;
                    AutoCSer.LogHelper.ErrorIgnoreException(listenAcceptEvent.SocketError.ToString(), LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                }
                catch (Exception exception)
                {
                    if (isSocketDisposed) return;
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally
                {
                    if (serverSocket != null) serverSocket.Dispose();
                }
                System.Threading.Thread.Sleep(1);
            }
            while (!isSocketDisposed);
        }
        /// <summary>
        /// 获取命令服务控制器
        /// </summary>
        /// <param name="commandMethodIndex"></param>
        /// <returns></returns>
        internal CommandServerController GetCommandController(ref uint commandMethodIndex)
        {
            uint methodIndex = commandMethodIndex & Command.MethodIndexAnd;
            if (Controllers.Length == 1)
            {
                if (methodIndex >= MethodStartIndex)
                {
                    commandMethodIndex -= MethodStartIndex;
                    return Controller;
                }
                return Null.Controller;
            }
            uint index = methodIndex >> CommandServerController.MaxCommandBits;
            if (index == 0)
            {
                if (methodIndex >= MethodStartIndex)
                {
                    commandMethodIndex -= MethodStartIndex;
                    return Controller;
                }
                return Null.Controller;
            }
            if (index < (uint)Controllers.Length)
            {
                commandMethodIndex &= uint.MaxValue ^ ((1U << Command.MethodIndexBits) - CommandServerController.MaxCommandCount);
                return Controllers.Array[(int)index];
            }
            return Null.Controller;
        }
        /// <summary>
        /// 获取服务端执行队列
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue> createServerCallQueue(int index)
        {
            controllerLock.Enter();
            try
            {
                if (index >= callQueues.Length)
                {
                    if (!IsDisposed)
                    {
                        controllerLock.SleepFlag = 1;
                        int length = callQueues.Length;
                        callQueues = AutoCSer.Common.GetCopyArray(callQueues, index + 1);
                        do
                        {
                            CommandServerCallQueue queue = new CommandServerCallQueue(this, null, index);
                            callQueues[length].Set(queue, queue.CreateLink());
                        }
                        while (++length <= index);
                    }
                    else return default(KeyValue<CommandServerCallQueue, CommandServerCallLowPriorityQueue>);
                }
            }
            finally { controllerLock.ExitSleepFlag(); }
            return callQueues[index];
        }
        /// <summary>
        /// 获取服务端执行队列
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private CommandServerCallQueue getServerCallQueue(int index)
        {
            return index < callQueues.Length ? callQueues[index].Key : createServerCallQueue(index).Key;
        }
        /// <summary>
        /// 获取服务端执行队列
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private CommandServerCallLowPriorityQueue getServerCallLowPriorityQueue(int index)
        {
            return index < callQueues.Length ? callQueues[index].Value : createServerCallQueue(index).Value;
        }
        /// <summary>
        /// Add a synchronous call queue task
        /// </summary>
        /// <param name="index">Queue number index
        /// 队列编号索引</param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool AddQueue(byte index, CommandServerCallQueueCustomNode node)
        {
            if (!IsDisposed)
            {
                CommandServerCallQueue callQueue = getServerCallQueue(index);
                if (callQueue != null)
                {
                    callQueue.Add(node);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取服务端执行队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallQueue GetServerCallQueue(CommandListener server, int index)
        {
           return server.getServerCallQueue(index);
        }
        /// <summary>
        /// Add a synchronous call queue task (low priority)
        /// 添加同步调用队列任务（低优先级）
        /// </summary>
        /// <param name="index">Queue number index
        /// 队列编号索引</param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool AddQueueLowPriority(byte index, CommandServerCallQueueCustomNode node)
        {
            if (!IsDisposed)
            {
                CommandServerCallLowPriorityQueue callQueue = getServerCallLowPriorityQueue(index);
                if (callQueue != null)
                {
                    callQueue.Add(node);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取服务端执行队列（低优先级）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallLowPriorityQueue GetServerCallQueueLowPriority(CommandListener server, int index)
        {
            return server.getServerCallLowPriorityQueue(index);
        }
        /// <summary>
        /// 获取服务端读写队列
        /// </summary>
        /// <returns></returns>
        private CommandServerCallReadQueue createServerCallReadWriteQueue()
        {
            controllerLock.Enter();
            try
            {
                if (object.ReferenceEquals(CallReadWriteQueue, CommandServerCallReadQueue.Null) && !IsDisposed)
                {
                    controllerLock.SleepFlag = 1;
                    CallReadWriteQueue = new CommandServerCallReadQueue(this, null, Config.MaxReadWriteQueueConcurrency);
                }
            }
            finally { controllerLock.ExitSleepFlag(); }
            return CallReadWriteQueue;
        }
        /// <summary>
        /// 获取服务端读写队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private CommandServerCallReadQueue getServerCallReadWriteQueue()
        {
            return !object.ReferenceEquals(CallReadWriteQueue, CommandServerCallReadQueue.Null) ? CallReadWriteQueue : createServerCallReadWriteQueue();
        }
        /// <summary>
        /// 获取服务端读写队列
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallReadQueue GetServerCallReadWriteQueue(CommandListener server)
        {
            return server.getServerCallReadWriteQueue();
        }
        /// <summary>
        /// 获取服务端支持并行读的同步队列
        /// </summary>
        /// <returns></returns>
        private CommandServerCallConcurrencyReadQueue createServerCallConcurrencyReadQueue()
        {
            controllerLock.Enter();
            try
            {
                if (object.ReferenceEquals(CallConcurrencyReadQueue, CommandServerCallConcurrencyReadQueue.Null) && !IsDisposed)
                {
                    controllerLock.SleepFlag = 1;
                    CallConcurrencyReadQueue = new CommandServerCallConcurrencyReadQueue(this, null);
                }
            }
            finally { controllerLock.ExitSleepFlag(); }
            return CallConcurrencyReadQueue;
        }
        /// <summary>
        /// 获取服务端支持并行读的同步队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private CommandServerCallConcurrencyReadQueue getServerCallConcurrencyReadQueue()
        {
            return !object.ReferenceEquals(CallConcurrencyReadQueue, CommandServerCallConcurrencyReadQueue.Null) ? CallConcurrencyReadQueue : createServerCallConcurrencyReadQueue();
        }
        /// <summary>
        /// 获取服务端支持并行读的同步队列
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallConcurrencyReadQueue GetServerCallConcurrencyReadQueue(CommandListener server)
        {
            return server.getServerCallConcurrencyReadQueue();
        }
        /// <summary>
        /// 获取服务端异步调用队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandServerCallTaskQueueSet<T>? GetServerCallTaskQueueSet<T>(CommandListener server) where T : IEquatable<T>
#else
        internal static CommandServerCallTaskQueueSet<T> GetServerCallTaskQueueSet<T>(CommandListener server) where T : IEquatable<T>
#endif
        {
            return server.TaskQueueSet.Get<T>();
        }
        /// <summary>
        /// 获取控制器信息
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal void QueryController(CommandServerSocket socket)
        {
            int controllerCount = Controllers.Length;
            CommandServerController[] controllers = Controllers.Array;
            while (controllerCount != 0)
            {
                --controllerCount;
                var output = default(ServerOutputController);
                try
                {
                    socket.AppendOutput(output = new ServerOutputController(controllerCount, controllers[controllerCount]));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
            }
        }
    }
}
