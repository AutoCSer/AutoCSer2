﻿using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoCSer.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client socket
    /// 命令客户端套接字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed class CommandClientSocket
    {
        /// <summary>
        /// Command client
        /// </summary>
        public readonly CommandClient Client;
        /// <summary>
        /// Socket
        /// 套接字
        /// </summary>
        public Socket Socket { get; private set; }
        /// <summary>
        /// Server listening address
        /// 服务监听地址
        /// </summary>
        private readonly IPEndPoint serverEndPoint;
        /// <summary>
        /// Client main controller
        /// 客户端主控制器
        /// </summary>
        public readonly CommandClientController Controller;
        /// <summary>
        /// Client command pool
        /// 客户端命令池
        /// </summary>
        internal readonly CommandPool CommandPool;
        /// <summary>
        /// Receive data socket asynchronous event object
        /// 接收数据套接字异步事件对象
        /// </summary>
        private readonly SocketAsyncEventArgs receiveAsyncEventArgs;
        /// <summary>
        /// The maximum number of unprocessed commands on the client side
        /// 客户端最大未处理命令数量
        /// </summary>
        private readonly int commandQueueCount;
        /// <summary>
        /// Reserve
        /// 保留
        /// </summary>
        private int reserve;
        /// <summary>
        /// Maximum input data length
        /// 最大输入数据长度
        /// </summary>
        internal readonly int MaxInputSize;
        /// <summary>
        /// Maximum merged input data length
        /// 最大合并输入数据长度
        /// </summary>
        internal readonly int MaxMergeInputSize;
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad0;
        /// <summary>
        /// Custom session object
        /// 自定义会话对象
        /// </summary>
#if NetStandard21
        public object? SessionObject;
#else
        public object SessionObject;
#endif
        /// <summary>
        /// Client heart rate detection timing
        /// 客户端心跳检测定时
        /// </summary>
#if NetStandard21
        private ClientCheckTimer? checkTimer;
#else
        private ClientCheckTimer checkTimer;
#endif
        /// <summary>
        /// Command controller access lock
        /// 命令控制器访问锁
        /// </summary>
        private SemaphoreSlimLock controllerLock;
        /// <summary>
        /// A collection of client controller creators
        /// 客户端控制器创建器集合
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private Dictionary<string, CommandClientInterfaceControllerCreator> controllerCreators;
        /// <summary>
        /// Command client controller collection
        /// 命令客户端控制器集合
        /// </summary>
#if NetStandard21
        internal CommandClientController?[] ControllerArray;
#else
        internal CommandClientController[] ControllerArray;
#endif
        /// <summary>
        /// Command client controller collection
        /// 命令客户端控制器集合
        /// </summary>
        public IEnumerable<CommandClientController> Controllers
        {
            get
            {
                if (ControllerArray.Length != 0)
                {
                    foreach (var controller in ControllerArray)
                    {
                        if (controller != null && controller.GetType() != typeof(NullCommandClientController)) yield return controller;
                    }
                }
                else if (Controller.GetType() != typeof(NullCommandClientController)) yield return Controller;
            }
        }
        /// <summary>
        /// Get the command client controller
        /// 获取命令客户端控制器
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        public CommandClientController? this[string controllerName]
#else
        public CommandClientController this[string controllerName]
#endif
        {
            get
            {
                foreach (CommandClientController controller in Controllers)
                {
                    if (controller?.ControllerName == controllerName) return controller;
                }
                return null;
            }
        }
        /// <summary>
        /// Get the command client controller
        /// 获取命令客户端控制器
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
#if NetStandard21
        public CommandClientController? this[Type controllerType]
#else
        public CommandClientController this[Type controllerType]
#endif
        {
            get { return this[controllerType.Name]; }
        }
        /// <summary>
        /// Receive binary deserialization of data
        /// 接收数据二进制反序列化
        /// </summary>
#if NetStandard21
        private AutoCSer.BinaryDeserializer? receiveDeserializer;
#else
        private AutoCSer.BinaryDeserializer receiveDeserializer;
#endif
        /// <summary>
        /// Temporary received data buffer
        /// 临时接收数据缓冲区
        /// </summary>
        private ByteArrayBuffer receiveBigBuffer;
        /// <summary>
        /// Receive data buffer
        /// 接收数据缓冲区
        /// </summary>
        private ByteArrayBuffer receiveBuffer;
        /// <summary>
        /// The starting position for receiving data
        /// 接收数据起始位置
        /// </summary>
        private unsafe byte* receiveDataStart;
        /// <summary>
        /// Current session callback identity
        /// 当前会话回调标识
        /// </summary>
        internal CallbackIdentity callbackIdentity;
        /// <summary>
        /// Current client command
        /// 当前客户端命令
        /// </summary>
#if NetStandard21
        private Command? command;
#else
        private Command command;
#endif
        /// <summary>
        /// Received data error message
        /// 接收数据错误信息
        /// </summary>
        internal string ReceiveErrorMessage;
        /// <summary>
        /// The current number of bytes of received data being processed
        /// 当前处理接收数据字节数
        /// </summary>
        private int receiveIndex;
        /// <summary>
        /// The byte size of the current data after encoding
        /// 当前数据编码后的字节大小
        /// </summary>
        private int transferDataSize;
        /// <summary>
        /// The current data byte size
        /// 当前数据字节大小
        /// </summary>
        private int dataSize;
        /// <summary>
        /// Receive the data thread ID
        /// 接收数据线程ID
        /// </summary>
        internal int OnReceiveThreadId;
        /// <summary>
        /// The data receiving socket is incorrect
        /// 接收数据套接字错误
        /// </summary>
        private SocketError receiveSocketError;
        /// <summary>
        /// The number of failed socket creation attempts
        /// 创建套接字失败次数
        /// </summary>
        private int createErrorCount = 0;
        /// <summary>
        /// The current version number of the client being created
        /// 当前创建客户端的版本号
        /// </summary>
        internal int CreateVersion;
        /// <summary>
        /// Receive data callback type
        /// 接收数据回调类型
        /// </summary>
        private ClientReceiveTypeEnum receiveType;
        /// <summary>
        /// Received data error type
        /// 接收数据错误类型
        /// </summary>
        private ClientReceiveErrorTypeEnum receiveErrorType;
        /// <summary>
        /// Reserve
        /// 保留
        /// </summary>
        private short receiveReserve;
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad1;
        /// <summary>
        /// The current number of queue commands
        /// 当前队列命令数量
        /// </summary>
        private long commandCount;
        /// <summary>
        /// The linked list of output commands
        /// 输出命令链表
        /// </summary>
        private LinkStack<Command> commands;
        /// <summary>
        /// Output waiting event
        /// 输出等待事件
        /// </summary>
        private System.Threading.AutoResetEvent outputWaitHandle;
        /// <summary>
        /// The collection of commands waiting to be added to the queue
        /// 等待添加到队列的命令集合
        /// </summary>
        internal LinkStack<Command> waitPushCommands;
        ///// <summary>
        ///// 命令批处理
        ///// </summary>
        //internal CommandBatch CommandBatch;
        /// <summary>
        /// Has the socket been closed
        /// 是否已经关闭套接字
        /// </summary>
        private int isClosed;
        /// <summary>
        /// Has the socket been closed
        /// 是否已经关闭套接字
        /// </summary>
        public bool IsClosed { get { return isClosed != 0; } }
        /// <summary>
        /// Reserve
        /// 保留
        /// </summary>
        private int commandReserve;
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad2;
        /// <summary>
        /// Output data binary serialization
        /// 输出数据二进制序列化
        /// </summary>
        internal BinarySerializer OutputSerializer;
        /// <summary>
        /// The current number of sent commands
        /// 当前发送命令数量
        /// </summary>
        private long buildCommandCount;

        /// <summary>
        /// Default empty socket
        /// </summary>
        private CommandClientSocket()
        {
#if NetStandard21
            Client = CommandClient.Null;
            serverEndPoint = CommandServerConfigBase.NullIPEndPoint;
            ReceiveErrorMessage = string.Empty;
#endif
            Controller = new NullCommandClientController(this, string.Empty);
            receiveAsyncEventArgs = CommandServerConfigBase.NullSocketAsyncEventArgs;
            CommandPool = new CommandPool(Client, new CompletedReturnCommand<int>(Controller));
            ControllerArray = EmptyArray<CommandClientController>.Array;
            Socket = CommandServerConfigBase.NullSocket;
            outputWaitHandle = AutoCSer.Common.NullAutoResetEvent;
            OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
            //CommandBatch = new CommandBatch(this);
            controllerLock = new SemaphoreSlimLock(0, 1);
            setValue(true);
        }
        /// <summary>
        /// Default empty socket
        /// </summary>
        internal static readonly CommandClientSocket Null = new CommandClientSocket();
        /// <summary>
        /// Command client socket
        /// 命令客户端套接字
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="serverEndPoint"></param>
        /// <param name="createVersion"></param>
        internal CommandClientSocket(CommandClient client, IPEndPoint serverEndPoint, int createVersion)
        {
            this.Client = client;
            this.serverEndPoint = serverEndPoint;
            this.CreateVersion = createVersion;
#if NetStandard21
            ReceiveErrorMessage = string.Empty;
#endif
            commandQueueCount = Math.Max(client.Config.CommandQueueCount, 1);
            MaxInputSize = client.Config.MaxInputSize;
            if (MaxInputSize <= 0) MaxMergeInputSize = MaxInputSize = int.MaxValue;
            else if ((MaxMergeInputSize = MaxInputSize + client.ReceiveBufferPool.Size) < 0) MaxMergeInputSize = int.MaxValue;
            receiveAsyncEventArgs = new SocketAsyncEventArgs();
            CommandPool = new CommandPool(Client);
            ControllerArray = EmptyArray<CommandClientController>.Array;
            Socket = CommandServerConfigBase.NullSocket;
            outputWaitHandle = AutoCSer.Common.NullAutoResetEvent;
            OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
            //CommandBatch = Null.CommandBatch;
            controllerLock = new SemaphoreSlimLock(0, 1);
            setValue(false);
            client.DefaultControllerReturnType = CommandClientReturnTypeEnum.WaitConnect;
            Controller = createController();
            create().Catch();
        }
        /// <summary>
        /// Command client socket
        /// 命令客户端套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="endPoint"></param>
#if NetStandard21
        internal CommandClientSocket(CommandClientSocket socket, IPEndPoint? endPoint = null)
#else
        internal CommandClientSocket(CommandClientSocket socket, IPEndPoint endPoint = null)
#endif
        {
            Client = socket.Client;
#if NetStandard21
            ReceiveErrorMessage = string.Empty;
#endif
            serverEndPoint = endPoint ?? socket.serverEndPoint;
            CreateVersion = socket.CreateVersion + 1;
            createErrorCount = socket.createErrorCount + 1;
            commandQueueCount = socket.commandQueueCount;
            MaxInputSize = socket.MaxInputSize;
            MaxMergeInputSize = socket.MaxMergeInputSize;
            receiveAsyncEventArgs = new SocketAsyncEventArgs();
            CommandPool = new CommandPool(Client);
            ControllerArray = EmptyArray<CommandClientController>.Array;
            Socket = CommandServerConfigBase.NullSocket;
            outputWaitHandle = AutoCSer.Common.NullAutoResetEvent;
            OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
            //CommandBatch = Null.CommandBatch;
            controllerLock = new SemaphoreSlimLock(0, 1);
            setValue(false);
            Client.DefaultControllerReturnType = CommandClientReturnTypeEnum.DisconnectionReconnect;
            Controller = createController();
            try
            {
                Client.SocketEvent.Create(this, socket);
            }
            finally
            {
                create().Catch();
            }
        }
        /// <summary>
        /// 反向客户端套接字
        /// </summary>
        /// <param name="client"></param>
        /// <param name="socket"></param>
        internal CommandClientSocket(CommandClient client, Socket socket)
        {
            this.Client = client;
            this.serverEndPoint = (IPEndPoint)socket.RemoteEndPoint.notNull();
            //this.CreateVersion = client.CreateVersion;
#if NetStandard21
            ReceiveErrorMessage = string.Empty;
#endif
            commandQueueCount = Math.Max(client.Config.CommandQueueCount, 1);
            MaxInputSize = client.Config.MaxInputSize;
            if (MaxInputSize <= 0) MaxMergeInputSize = MaxInputSize = int.MaxValue;
            else if ((MaxMergeInputSize = MaxInputSize + client.ReceiveBufferPool.Size) < 0) MaxMergeInputSize = int.MaxValue;
            receiveAsyncEventArgs = new SocketAsyncEventArgs();
            CommandPool = new CommandPool(Client);
            ControllerArray = EmptyArray<CommandClientController>.Array;
            Socket = socket;
            outputWaitHandle = AutoCSer.Common.NullAutoResetEvent;
            OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
            //CommandBatch = Null.CommandBatch;
            controllerLock = new SemaphoreSlimLock(0, 1);
            setValue(false);
            client.DefaultControllerReturnType = CommandClientReturnTypeEnum.WaitConnect;
            Controller = createController();
        }
        /// <summary>
        /// Set the default value
        /// 设置默认值
        /// </summary>
        /// <param name="isNull"></param>
        private void setValue(bool isNull)
        {
            if (!isNull)
            {
                if (!Client.IsShortLink)
                {
                    CommandPool.Array[CommandServer.KeepCallbackCommand.CustomDataIndex].Command = new CustomDataCallbackCommand(this);
                    CommandPool.Array[CommandServer.KeepCallbackCommand.ControllerIndex].Command = new ControllerCallbackCommand(this);
                }
                CommandPool.Array[CommandServer.KeepCallbackCommand.MergeIndex].Command = new MergeCallbackCommand(this);
                CommandPool.Array[CommandServer.KeepCallbackCommand.CancelKeepCallbackIndex].Command = new CancelKeepCallbackCommand(this);
                outputWaitHandle = new AutoResetEvent(false);
                receiveAsyncEventArgs.Completed += onReceive;
                receiveAsyncEventArgs.UserToken = this;
            }
        }
        /// <summary>
        /// Create the main controller
        /// 创建主控制器
        /// </summary>
        /// <returns></returns>
        private CommandClientController createController()
        {
            //(Client.ControllerCreators.Length > 1 ? CommandServerController.MaxCommandCount : (int)(Command.MethodIndexAnd + 1)) - CommandListener.MethodStartIndex, 
            return Client.ControllerCreators.Array[0].Create(this, CommandListener.MethodStartIndex, null);
        }
        /// <summary>
        /// Close the socket
        /// </summary>
        internal void Close()
        {
            if (Interlocked.Exchange(ref isClosed, 1) == 0)
            {
                checkTimer?.Cancel();

                outputWaitHandle.setDispose();
                try
                {
                    Shutdown();
                    if (receiveAsyncEventArgs.UserToken != null)
                    {
                        receiveAsyncEventArgs.Dispose();
                        receiveAsyncEventArgs.UserToken = null;
                    }
                    receiveBuffer.Free();
                }
                finally
                {
                    Client.OnClosed(this);
                    CommandPool.DisposeTimeout();
                    if (receiveDeserializer != null)
                    {
                        receiveDeserializer.FreeContext();
                        receiveDeserializer = null;
                    }
                    SemaphoreSlimLock.TryExit(controllerLock);

                    closeWaitPush();
                }
            }
        }
        /// <summary>
        /// The close socket operation deals with the collection of waiting commands
        /// 关闭套接字操作处理等待命令集合
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void closeWaitPush()
        {
            var head = waitPushCommands.Get();
            while (head != null) head.CheckWaitPush(ref head);
        }
        /// <summary>
        /// Increase the current number of sent commands
        /// 增加当前发送命令数量
        /// </summary>
        /// <param name="buildCount"></param>
        private void addBuildCommandCount(int buildCount)
        {
            buildCommandCount += buildCount;
            if (!waitPushCommands.IsEmpty && commandCount - buildCommandCount <= (commandQueueCount >> 1))
            {
                var head = waitPushCommands.Get();
                while (head != null)
                {
                    if (head.CheckWaitPush(ref head))
                    {
                        if (head != null)
                        {
                            waitPushCommands.PushLink(head, Link<Command>.GetEnd(head));
                            if (isClosed != 0) closeWaitPush();
                        }
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// Close the socket
        /// </summary>
        internal void Shutdown()
        {
            Socket socket = this.Socket;
            if (!object.ReferenceEquals(socket, CommandServerConfigBase.NullSocket))
            {
                this.Socket = CommandServerConfigBase.NullSocket;
                try
                {
                    if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally { socket.Dispose(); }
            }
        }
        /// <summary>
        /// Compare whether the server listening addresses are equal
        /// 比较服务端监听地址是否相等
        /// </summary>
        /// <param name="endPoint"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool ServerEndPointEquals(IPEndPoint endPoint)
        {
            return endPoint.Port == serverEndPoint.Port && endPoint.Address == serverEndPoint.Address;
        }
        /// <summary>
        /// Create a socket
        /// 创建套接字
        /// </summary>
        /// <returns></returns>
        private async Task create()
        {
            int exceptionCount = 0;
            NullableBoolEnum isConnect = NullableBoolEnum.Null;
            bool isBuildOutput = false, isCreateError = false;
            receiveSocketError = SocketError.Success;
            CommandClientSocketEvent socketEvent = Client.SocketEvent;
            do
            {
                try
                {
                    isConnect = NullableBoolEnum.Null;
                    if (isCreateError)
                    {
                        await socketEvent.CreateSocketSleep(createErrorCount);
                        if (!Client.IsCreateVersion(CreateVersion))
                        {
                            break;
                        }
                    }
                    Socket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    Socket.ReceiveBufferSize = Client.ReceiveBufferPool.Size;
                    Socket.SendBufferSize = Client.SendBufferPool.Size;
                    Socket.NoDelay = Client.Config.NoDelay;
                    isConnect = NullableBoolEnum.False;
#if DotNet45
                    Socket.Connect(serverEndPoint);
#else
                    await Socket.ConnectAsync(serverEndPoint);
#endif
                    isConnect = NullableBoolEnum.True;
                    if (!Client.IsCreateVersion(CreateVersion))
                    {
                        //Console.Write($"[{CreateVersion},{Client.CreateVersion},{Client.IsDisposed}]");
                        break;
                    }
                    if (receiveBuffer.Buffer == null) Client.ReceiveBufferPool.Get(ref receiveBuffer);
                    if (!isBuildOutput)
                    {
                        isBuildOutput = true;
                        //commands.Set(new MergeCallbackCommand(this));
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(buildOutput);
                    }
                    if (await verify(exceptionCount))
                    {
                        return;
                    }
                    Client.DefaultControllerReturnType = CommandClientReturnTypeEnum.VerifyError;
                    ++Client.VerifyErrorCount;
                }
                catch (Exception exception)
                {
                    await socketEvent.OnCreateSocketException(this, exception, serverEndPoint, ++exceptionCount);
                }
                if (isBuildOutput)
                {
                    if (Client.VerifyErrorCount >= Client.Config.VerifyErrorCount || Client.IsShortLink || !await Client.CreateNewSocketAsync(this))
                    {
                        await Client.OnCreateError(this);
                    }
                    break;
                }
                else if (isConnect == NullableBoolEnum.False) await Client.ConnectFail(serverEndPoint);
                Client.VerifyErrorCount = 0;
                await Client.OnCreateError(this);
                Shutdown();
                ++createErrorCount;
                isCreateError = true;
            }
            while (Client.IsCreateVersion(CreateVersion) && !Client.IsShortLink);
            Close();
        }
        /// <summary>
        /// Reverse client initialization operation
        /// 反向客户端初始化操作
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Start()
        {
            receiveSocketError = SocketError.Success;
            bool isVerify = false;
            try
            {
                Socket.ReceiveBufferSize = Client.ReceiveBufferPool.Size;
                Socket.SendBufferSize = Client.SendBufferPool.Size;
                Socket.NoDelay = Client.Config.NoDelay;
                Client.ReceiveBufferPool.Get(ref receiveBuffer);
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(buildOutput);
                if (await verify(0)) return isVerify = true;
            }
            finally
            {
                if (!isVerify) Close();
            }
            return false;
        }
        /// <summary>
        /// Verification call
        /// 验证调用
        /// </summary>
        /// <param name="exceptionCount"></param>
        /// <returns></returns>
        private async Task<bool> verify(int exceptionCount)
        {
            receiveIndex = 0;
            receiveType = ClientReceiveTypeEnum.CallbackIdentityAgain;
            receiveBuffer.SetBuffer(receiveAsyncEventArgs);
            if (!Socket.ReceiveAsync(receiveAsyncEventArgs)) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(createSynchronizeOnReceive);
            CommandClientReturnValue<CommandServerVerifyStateEnum> verifyMethodState = await Client.SocketEvent.CallVerifyMethod(Controller);
            switch (verifyMethodState.Value)
            {
                case CommandServerVerifyStateEnum.Success:
                    if (verifyMethodState.ReturnType == CommandClientReturnTypeEnum.Success)
                    {
                        if (!Client.IsCreateVersion(CreateVersion))
                        {
                            break;
                        }
                        return Client.IsReverse || await OnVerify(exceptionCount);
                    }
                    break;
                case CommandServerVerifyStateEnum.LessVerifyMethod:
                    receiveErrorType = ClientReceiveErrorTypeEnum.LessVerifyMethod;
                    Client.Dispose();
                    break;
            }
            return false;
        }
        /// <summary>
        /// Service completion verification
        /// 服务完成验证
        /// </summary>
        /// <param name="exceptionCount"></param>
        /// <returns></returns>
        internal async Task<bool> OnVerify(int exceptionCount)
        {
            if (Client.ControllerCreators.Length > 1 || Client.Config.IsServerController)
            {
                if (!Client.IsShortLink)
                {
                    controllerCreators = DictionaryCreator<string>.Create<CommandClientInterfaceControllerCreator>();
                    foreach (CommandClientInterfaceControllerCreator creator in Client.ControllerCreators)
                    {
                        controllerCreators.Add(creator.ControllerName, creator);
                    }
                    ++commandCount;
                    if (commands.IsPushHead(new ControllerCommand(this))) outputWaitHandle.Set();
                    await controllerLock.EnterAsync();
                    if (!Client.IsCreateVersion(CreateVersion))
                    {
                        return false;
                    }
                    foreach (string controllerName in controllerCreators.Keys)
                    {
                        await Client.SocketEvent.NotFoundServerControllerName(this, controllerName);
                    }
                    foreach (var controller in ControllerArray)
                    {
                        if (controller != null && controller.GetType() == typeof(NullCommandClientController))
                        {
                            await Client.SocketEvent.NotFoundControllerName(this, controller.ControllerName);
                        }
                    }
                }
                else
                {
                    ControllerArray = new CommandClientController[Client.ControllerCreators.Length];
                    ControllerArray[0] = Controller;
                    int index = 0;
                    foreach (CommandClientInterfaceControllerCreator creator in Client.ControllerCreators)
                    {
                        if (index != 0) ControllerArray[index] = creator.Create(this, index << CommandServerController.MaxCommandBits, null);
                        ++index;
                    }
                }
            }
            if (await Client.OnMethodVerified(this))
            {
                if (!Client.IsCreateVersion(CreateVersion))
                {
                    return false;
                }
                ushort checkSeconds = Client.Config.CheckSeconds;
                if (checkSeconds > 0 && !Client.IsShortLink) checkTimer = new ClientCheckTimer(this, checkSeconds);
                if (exceptionCount != 0) await Client.SocketEvent.OnCreateSocketRetrySuccess(this, serverEndPoint, exceptionCount);
                createErrorCount = -1;
                Client.VerifyErrorCount = 0;
                return true;
            }
            return false;
        }
        /// <summary>
        /// The callback for the command controller to query data
        /// 命令控制器查询数据回调
        /// </summary>
        /// <param name="controllerOutputData"></param>
        internal void ControllerCallback(ref CommandControllerOutputData controllerOutputData)
        {
            controllerCreators.Remove(controllerOutputData.ControllerName);
            if (controllerOutputData.ControllerIndex > 0)
            {
                if (ControllerArray.Length == 0)
                {
                    ControllerArray = new CommandClientController[controllerOutputData.ControllerIndex + 1];
                    ControllerArray[0] = Controller;
                }
                var creator = Client.GetControllerCreator(controllerOutputData.ControllerName);
                if (creator != null)
                {
                    ControllerArray[controllerOutputData.ControllerIndex] = creator.Create(this, controllerOutputData.ControllerIndex << CommandServerController.MaxCommandBits, controllerOutputData.MethodNames);
                }
                else ControllerArray[controllerOutputData.ControllerIndex] = new NullCommandClientController(this, controllerOutputData.ControllerName);
            }
            else
            {
                try
                {
                    if (Controller.ControllerName == controllerOutputData.ControllerName)
                    {
                        if (controllerOutputData.MethodNames != null) Controller.SetServerMethodIndexs(controllerOutputData.MethodNames);
                    }
                    else
                    {
                        Client.SocketEvent.ControllerNameError(Controller, controllerOutputData.ControllerName);
                    }
                }
                finally { controllerLock.Exit(); }
            }
        }
        /// <summary>
        /// Create sockets to synchronously receive data
        /// 创建套接字同步接收数据
        /// </summary>
        private void createSynchronizeOnReceive()
        {
            onReceive(null, receiveAsyncEventArgs);
        }
        //private static int currentSocketIdentity;
        //private readonly int socketIdentity = ++currentSocketIdentity;
        /// <summary>
        /// The callback delegate after the data is received
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">Asynchronous callback parameters</param>
#if NetStandard21
        private void onReceive(object? sender, SocketAsyncEventArgs async)
#else
        private void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
            OnReceiveThreadId = System.Environment.CurrentManagedThreadId;
            try
            {
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = receiveAsyncEventArgs.BytesTransferred;
                    //Console.WriteLine($"[{socketIdentity}] onReceive {count}");
                    switch (receiveType)
                    {
                        case ClientReceiveTypeEnum.CallbackIdentity:
                            if (count >= 0)
                            {
                                if (isCallbackIdentity(count)) return;
                            }
                            else receiveErrorType = ClientReceiveErrorTypeEnum.CallbackIdentityLess;
                            break;
                        case ClientReceiveTypeEnum.CallbackIdentityAgain:
                            if (count > 0)
                            {
                                if (isCallbackIdentity(count)) return;
                            }
                            else receiveErrorType = ClientReceiveErrorTypeEnum.CallbackIdentityLess;
                            break;
                        case ClientReceiveTypeEnum.Data:
                            if (count > 0)
                            {
                                if (isData(count)) return;
                            }
                            else receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeLess;
                            break;
                        case ClientReceiveTypeEnum.BigData:
                            if (count > 0)
                            {
                                if (isBigData(count)) return;
                            }
                            else receiveErrorType = ClientReceiveErrorTypeEnum.BigDataSizeLess;
                            break;
                    }
                }
                else
                {
                    receiveSocketError = receiveAsyncEventArgs.SocketError;
                    if (!Client.IsDisposed && isClosed == 0) Client.Config.Log.ErrorIgnoreException($"{serverEndPoint} {receiveSocketError}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
                }
            }
            catch (Exception exception)
            {
                if (!Client.IsDisposed && isClosed == 0)
                {
                    if (receiveErrorType == ClientReceiveErrorTypeEnum.Success) receiveErrorType = ClientReceiveErrorTypeEnum.Exception;
                    Client.Config.Log.ExceptionIgnoreException(exception, serverEndPoint.ToString(), LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                }
            }
            if (!Client.IsReverse && !Client.IsShortLink)
            {
                createErrorCount = -1;
                try
                {
                    Client.CreateNewSocket(this);
                }
                finally { Close(); }
            }
            else Close();
        }
        /// <summary>
        /// Get the command callback number
        /// 获取命令回调序号
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Whether it is an asynchronous operation
        /// 是否异步操作</returns>
        private unsafe bool isCallbackIdentity(int count)
        {
        START:
            int receiveSize = (receiveBuffer.CurrentIndex += count) - receiveIndex;
            if (receiveSize >= sizeof(CallbackIdentity))
            {
                fixed (byte* receiveDataFixed = receiveBuffer.GetFixedBuffer())
                {
                    receiveDataStart = receiveDataFixed + receiveBuffer.StartIndex;
                    byte* start = receiveDataStart + receiveIndex;
                    callbackIdentity = *(CallbackIdentity*)start;
                    if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.SendData) != 0)
                    {
                        if (receiveSize >= (sizeof(CallbackIdentity) + sizeof(int)))
                        {
                            if ((transferDataSize = *(int*)(start + sizeof(CallbackIdentity))) > 0)
                            {
                                dataSize = transferDataSize;
                                receiveIndex += (sizeof(CallbackIdentity) + sizeof(int));
                            }
                            else if (transferDataSize != 0)
                            {
                                if (receiveSize >= (sizeof(CallbackIdentity) + sizeof(int) * 2))
                                {
                                    if ((dataSize = *(int*)(start + (sizeof(CallbackIdentity) + sizeof(int)))) > 0)
                                    {
                                        transferDataSize = -transferDataSize;
                                        receiveIndex += sizeof(CallbackIdentity) + sizeof(int) * 2;
                                    }
                                    else
                                    {
                                        receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeError;
                                        return false;
                                    }
                                    //if ((transferDataSize = -transferDataSize) < (dataSize = *(int*)(start + (sizeof(CallbackIdentity) + sizeof(int)))))
                                    //{
                                    //    receiveIndex += sizeof(CallbackIdentity) + sizeof(int) * 2;
                                    //}
                                    //else
                                    //{
                                    //    receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeError;
                                    //    return false;
                                    //}
                                }
                                else goto AGAIN;
                            }
                            else
                            {
                                receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeError;
                                return false;
                            }
                            if (transferDataSize <= receiveBuffer.CurrentIndex - receiveIndex) return isOnDataLoopFixed() && loop();
                            switch (checkDataLoopFixed())
                            {
                                case 0: return true;
                                case 1: return loop();
                                default: return false;
                            }
                        }
                    }
                    else
                    {
                        if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0)
                        {
                            if ((receiveErrorType = onReceive(CommandClientReturnTypeEnum.Success)) == ClientReceiveErrorTypeEnum.Success)
                            {
                                receiveIndex += sizeof(CallbackIdentity);
                                return loop();
                            }
                            return false;
                        }
                        if (receiveSize >= sizeof(CallbackIdentity) + sizeof(int))
                        {
                            receiveErrorType = onReceive((CommandClientReturnTypeEnum)(*(start + sizeof(CallbackIdentity))));
                            if (receiveErrorType == ClientReceiveErrorTypeEnum.Success)
                            {
                                receiveIndex += sizeof(CallbackIdentity) + sizeof(int);
                                return loop();
                            }
                            return false;
                        }
                    }
                }
            }
        AGAIN:
            if (receiveType != ClientReceiveTypeEnum.CallbackIdentityAgain)
            {
                receiveType = ClientReceiveTypeEnum.CallbackIdentityAgain;
                receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                if (object.ReferenceEquals(Socket, CommandServerConfigBase.NullSocket)) return false;
                if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    count = receiveAsyncEventArgs.BytesTransferred;
                    if (count > 0) goto START;
                    receiveErrorType = ClientReceiveErrorTypeEnum.CallbackIdentityLess;
                }
                else receiveSocketError = receiveAsyncEventArgs.SocketError;
            }
            else receiveErrorType = ClientReceiveErrorTypeEnum.CallbackIdentityLess;
            return false;
        }
        /// <summary>
        /// Check the command data
        /// 检查命令数据
        /// </summary>
        /// <returns>0 indicates asynchronous, 1 indicates successful continuation of data parsing, and 2 indicates failure
        /// 0 表示异步，1 表示成功继续解析数据，2 表示失败</returns>
        private unsafe int checkDataLoopFixed()
        {
            if (transferDataSize <= receiveBuffer.Buffer.notNull().BufferSize)
            {
                if (receiveIndex + transferDataSize > receiveBuffer.Buffer.notNull().BufferSize)
                {
                    AutoCSer.Common.CopyTo(receiveDataStart + receiveIndex, receiveDataStart, receiveBuffer.CurrentIndex -= receiveIndex);
                    receiveIndex = 0;
                }
                receiveType = ClientReceiveTypeEnum.Data;
                do
                {
                    receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                    if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return 0;
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        int count = receiveAsyncEventArgs.BytesTransferred;
                        if (count > 0)
                        {
                            if (transferDataSize <= (receiveBuffer.CurrentIndex += count) - receiveIndex) return isOnDataLoopFixed() ? 1 : 2;
                        }
                        else
                        {
                            receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeLess;
                            return 2;
                        }
                    }
                    else
                    {
                        receiveSocketError = receiveAsyncEventArgs.SocketError;
                        return 2;
                    }
                }
                while (true);
            }
            receiveBigBuffer.ReSize(transferDataSize, receiveBuffer.CurrentIndex - receiveIndex);
            receiveType = ClientReceiveTypeEnum.BigData;
            do
            {
                receiveAsyncEventArgs.SetBuffer(receiveBigBuffer.Buffer.notNull().Buffer, receiveBigBuffer.BufferCurrentIndex, transferDataSize - receiveBigBuffer.CurrentIndex);
                if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return 0;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = receiveAsyncEventArgs.BytesTransferred;
                    if (count > 0)
                    {
                        if (transferDataSize == (receiveBigBuffer.CurrentIndex += count)) return isOnBigDataLoopFixed() ? 1 : 2;
                    }
                    else
                    {
                        receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeLess;
                        return 2;
                    }
                }
                else
                {
                    receiveSocketError = receiveAsyncEventArgs.SocketError;
                    return 2;
                }
            }
            while (true);
        }
        /// <summary>
        /// Check the command data
        /// 检查命令数据
        /// </summary>
        /// <returns>Whether the execution was successful (non-asynchronous)
        /// 是否执行成功（非异步）</returns>
        private unsafe bool isOnDataLoopFixed()
        {
            if (transferDataSize == dataSize)
            {
                //Command command = CommandPool.GetCommand(callbackIdentity);
                //if (command != null)
                //{
                    if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0)
                    {
                        SubArray<byte> data = new SubArray<byte>(receiveBuffer.StartIndex + receiveIndex, dataSize, receiveBuffer.Buffer.notNull().Buffer);
                        receiveErrorType = onReceive(ref data);
                    }
                    else receiveErrorType = onReceiveErrorMessage(receiveDataStart + receiveIndex);
                    if (receiveErrorType != ClientReceiveErrorTypeEnum.Success) return false;
                //}
                receiveIndex += transferDataSize;
                return true;
            }
            ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
            try
            {
                SubArray<byte> data = buffer.GetSubArray(dataSize);
                if (Client.Config.TransferDecode(this, receiveBuffer.GetSubArray(receiveIndex, transferDataSize), ref data))
                {
                    if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0) receiveErrorType = onReceive(ref data);
                    else
                    {
                        fixed (byte* dataFixed = data.GetFixedBuffer()) receiveErrorType = onReceiveErrorMessage(dataFixed + data.Start);
                    }
                    if (receiveErrorType == ClientReceiveErrorTypeEnum.Success)
                    {
                        receiveIndex += transferDataSize;
                        return true;
                    }
                    return false;
                }
            }
            finally { buffer.Free(); }
            receiveErrorType = ClientReceiveErrorTypeEnum.DataDecodeError;
            return false;
        }
        /// <summary>
        /// Receive error messages
        /// 接收错误信息
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe ClientReceiveErrorTypeEnum onReceiveErrorMessage(byte* start)
        {
            string errorMessage = string.Empty;
            if (AutoCSer.SimpleSerialize.Deserializer.NotNull(start + sizeof(int), ref errorMessage, start + dataSize) != null)
            {
                return onReceive((CommandClientReturnTypeEnum)(*start), errorMessage);
            }
            Client.Log.ErrorIgnoreException($"{errorMessage} {nameof(ClientInterfaceMethod.DeserializeError)}");
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// Check the command data
        /// 检查命令数据
        /// </summary>
        /// <returns>Whether the execution was successful (non-asynchronous)
        /// 是否执行成功（非异步）</returns>
        private unsafe bool isOnBigDataLoopFixed()
        {
            System.Buffer.BlockCopy(receiveBuffer.Buffer.notNull().Buffer, receiveBuffer.StartIndex + receiveIndex, receiveBigBuffer.Buffer.notNull().Buffer, receiveBigBuffer.StartIndex, receiveBuffer.CurrentIndex - receiveIndex);
            if (transferDataSize == dataSize)
            {
                if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0)
                {
                    SubArray<byte> data = receiveBigBuffer.GetSubArray(transferDataSize);
                    receiveErrorType = onReceive(ref data);
                }
                else
                {
                    fixed(byte* dataFixed = receiveBigBuffer.GetFixedBuffer()) receiveErrorType = onReceiveErrorMessage(dataFixed + receiveBigBuffer.StartIndex);
                }
                if (receiveErrorType != ClientReceiveErrorTypeEnum.Success) return false;
            }
            else
            {
                ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
                try
                {
                    SubArray<byte> data = buffer.GetSubArray(dataSize);
                    if (Client.Config.TransferDecode(this, receiveBigBuffer.GetSubArray(transferDataSize), ref data))
                    {
                        if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0) receiveErrorType = onReceive(ref data);
                        else
                        {
                            fixed (byte* dataFixed = data.GetFixedBuffer()) receiveErrorType = onReceiveErrorMessage(dataFixed + data.Start);
                        }
                        if (receiveErrorType != ClientReceiveErrorTypeEnum.Success) return false;
                    }
                    else
                    {
                        receiveErrorType = ClientReceiveErrorTypeEnum.BigDataDecodeError;
                        return false;
                    }
                }
                finally { buffer.Free(); }
            }
            receiveBuffer.CurrentIndex = receiveIndex = 0;
            receiveBigBuffer.Free();
            receiveBuffer.SetBuffer(receiveAsyncEventArgs);
            return true;
        }
        /// <summary>
        /// Get data
        /// 获取数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Whether it is an asynchronous operation
        /// 是否异步操作</returns>
        private unsafe bool isData(int count)
        {
            do
            {
                if (transferDataSize <= (receiveBuffer.CurrentIndex += count) - receiveIndex)
                {
                    fixed (byte* receiveDataFixed = receiveBuffer.GetFixedBuffer())
                    {
                        receiveDataStart = receiveDataFixed + receiveBuffer.StartIndex;
                        return isOnDataLoopFixed() && loop();
                    }
                }
                receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    if ((count = receiveAsyncEventArgs.BytesTransferred) <= 0)
                    {
                        receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeLess;
                        return false;
                    }
                }
                else
                {
                    receiveSocketError = receiveAsyncEventArgs.SocketError;
                    return false;
                }
            }
            while (true);
        }
        /// <summary>
        /// Get data
        /// 获取数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Whether it is an asynchronous operation
        /// 是否异步操作</returns>
        private bool isBigData(int count)
        {
            do
            {
                int nextSize = transferDataSize - (receiveBigBuffer.CurrentIndex += count);
                if (nextSize == 0)
                {
                    if (isOnBigDataLoopFixed())
                    {
                        receiveType = ClientReceiveTypeEnum.CallbackIdentity;
                        if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                        if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                        {
                            if ((count = receiveAsyncEventArgs.BytesTransferred) > 0) return isCallbackIdentity(count);
                            receiveErrorType = ClientReceiveErrorTypeEnum.BigDataSizeLess;
                        }
                        else receiveSocketError = receiveAsyncEventArgs.SocketError;
                    }
                    return false;
                }
                receiveAsyncEventArgs.SetBuffer(receiveBigBuffer.Buffer.notNull().Buffer, receiveBigBuffer.BufferCurrentIndex, nextSize);
                if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    if ((count = receiveAsyncEventArgs.BytesTransferred) <= 0)
                    {
                        receiveErrorType = ClientReceiveErrorTypeEnum.BigDataSizeLess;
                        return false;
                    }
                }
                else
                {
                    receiveSocketError = receiveAsyncEventArgs.SocketError;
                    return false;
                }
            }
            while (true);
        }
        /// <summary>
        /// Loop processing command
        /// 循环处理命令
        /// </summary>
        /// <returns>Whether it is an asynchronous operation
        /// 是否异步操作</returns>
        private unsafe bool loop()
        {
            bool isCallback = false;
        START:
            int receiveSize = receiveBuffer.CurrentIndex - receiveIndex;
            if (receiveSize >= sizeof(CallbackIdentity))
            {
                byte* start = receiveDataStart + receiveIndex;
                callbackIdentity = *(CallbackIdentity*)start;
                if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.SendData) != 0)
                {
                    if (receiveSize >= (sizeof(CallbackIdentity) + sizeof(int)))
                    {
                        if ((transferDataSize = *(int*)(start + sizeof(CallbackIdentity))) > 0)
                        {
                            dataSize = transferDataSize;
                            receiveIndex += sizeof(CallbackIdentity) + sizeof(int);
                        }
                        else if (transferDataSize != 0)
                        {
                            if (receiveSize >= (sizeof(CallbackIdentity) + sizeof(int) * 2))
                            {
                                if ((dataSize = *(int*)(start + (sizeof(CallbackIdentity) + sizeof(int)))) > 0)
                                {
                                    transferDataSize = -transferDataSize;
                                    receiveIndex += sizeof(CallbackIdentity) + sizeof(int) * 2;
                                }
                                else
                                {
                                    receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeError;
                                    return false;
                                }
                                //if ((transferDataSize = -transferDataSize) < (dataSize = *(int*)(start + (sizeof(CallbackIdentity) + sizeof(int)))))
                                //{
                                //    receiveIndex += sizeof(CallbackIdentity) + sizeof(int) * 2;
                                //}
                                //else
                                //{
                                //    receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeError;
                                //    return false;
                                //}
                            }
                            else goto COPY;
                        }
                        else
                        {
                            receiveErrorType = ClientReceiveErrorTypeEnum.DataSizeError;
                            return false;
                        }
                        if (transferDataSize <= receiveBuffer.CurrentIndex - receiveIndex)
                        {
                            if (isOnDataLoopFixed())
                            {
                                isCallback = false;
                                goto START;
                            }
                            return false;
                        }
                        switch (checkDataLoopFixed())
                        {
                            case 0: return true;
                            case 1: isCallback = false; goto START;
                            default: return false;
                        }
                    }
                }
                else
                {
                    if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0)
                    {
                        if ((receiveErrorType = onReceive(CommandClientReturnTypeEnum.Success)) == ClientReceiveErrorTypeEnum.Success)
                        {
                            receiveIndex += sizeof(CallbackIdentity);
                            isCallback = false;
                            goto START;
                        }
                        return false;
                    }
                    if (receiveSize >= sizeof(CallbackIdentity) + sizeof(int))
                    {
                        receiveErrorType = onReceive((CommandClientReturnTypeEnum)(*(start + sizeof(CallbackIdentity))));
                        if (receiveErrorType == ClientReceiveErrorTypeEnum.Success)
                        {
                            receiveIndex += sizeof(CallbackIdentity) + sizeof(int);
                            isCallback = false;
                            goto START;
                        }
                        return false;
                    }
                }
            }
            if (receiveSize == 0)
            {
                if (!isCallback)
                {
                    receiveBuffer.CurrentIndex = receiveIndex = 0;
                    goto RECEIVE;
                }
                receiveErrorType = ClientReceiveErrorTypeEnum.CallbackIdentityLess;
                return false;
            }
        COPY:
            if (!isCallback)
            {
                AutoCSer.Common.CopyTo(receiveDataStart + receiveIndex, receiveDataStart, receiveBuffer.CurrentIndex = receiveSize);
                receiveIndex = 0;
            }
        RECEIVE:
            if (receiveType != ClientReceiveTypeEnum.CallbackIdentityAgain || !isCallback)
            {
                receiveType = isCallback ? ClientReceiveTypeEnum.CallbackIdentityAgain : ClientReceiveTypeEnum.CallbackIdentity;
                receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                if (Socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    receiveBuffer.CurrentIndex += receiveAsyncEventArgs.BytesTransferred;
                    isCallback = true;
                    goto START;
                }
                receiveSocketError = receiveAsyncEventArgs.SocketError;
            }
            else receiveErrorType = ClientReceiveErrorTypeEnum.CallbackIdentityLess;
            return false;
        }
        /// <summary>
        /// Received data processing
        /// 接收数据处理
        /// </summary>
        /// <param name="type"></param>
        /// <param name="errorMessage">Error message</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private ClientReceiveErrorTypeEnum onReceive(CommandClientReturnTypeEnum type, string errorMessage)
        {
            var command = CommandPool.GetCommand(callbackIdentity);
            if (command != null)
            {
                SubArray<byte> data = new SubArray<byte>((int)(byte)type);
                ReceiveErrorMessage = errorMessage;
                ClientReceiveErrorTypeEnum errorType = command.OnReceive(ref data);
                ReceiveErrorMessage = string.Empty;
                return errorType;
            }
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// Received data processing
        /// 接收数据处理
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private ClientReceiveErrorTypeEnum onReceive(CommandClientReturnTypeEnum type)
        {
            var command = CommandPool.GetCommand(callbackIdentity);
            if (command != null)
            {
                SubArray<byte> data = new SubArray<byte>((int)(byte)type);
                return command.OnReceive(ref data);
            }
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// Received data processing
        /// 接收数据处理
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private ClientReceiveErrorTypeEnum onReceive(ref SubArray<byte> data)
        {
            command = CommandPool.GetCommand(callbackIdentity);
            if (command != null) return command.OnReceive(ref data);
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// Merge command processing
        /// 合并命令处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal unsafe ClientReceiveErrorTypeEnum MergeCallback(ref SubArray<byte> data)
        {
            int receiveSize = data.Length;
            if (receiveSize >= sizeof(CallbackIdentity) * 2)
            {
                byte[] dataArray = data.GetFixedBuffer();
                int receiveIndex = data.Start, receiveCount = data.EndIndex;
                fixed (byte* dataFixed = dataArray)
                {
                    byte* start;
                    do
                    {
                        callbackIdentity = *(CallbackIdentity*)(start = dataFixed + receiveIndex);
                        if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.Error) == 0)
                        {
                            if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.SendData) == 0)
                            {
                                if ((receiveErrorType = onReceive(CommandClientReturnTypeEnum.Success)) == ClientReceiveErrorTypeEnum.Success)
                                {
                                    if ((receiveSize -= sizeof(CallbackIdentity)) == 0) return ClientReceiveErrorTypeEnum.Success;
                                    receiveIndex += sizeof(CallbackIdentity);
                                }
                                else return receiveErrorType;
                            }
                            else if (receiveSize >= (sizeof(CallbackIdentity) + sizeof(int)))
                            {
                                if ((dataSize = *(int*)(start + sizeof(CallbackIdentity))) > 0
                                    && (receiveSize -= dataSize + (sizeof(CallbackIdentity) + sizeof(int))) >= 0)
                                {
                                    receiveIndex += (sizeof(CallbackIdentity) + sizeof(int));
                                    if ((command = CommandPool.GetCommand(callbackIdentity)) != null)
                                    {
                                        data.Set(receiveIndex, dataSize);
                                        receiveErrorType = command.OnReceive(ref data);
                                        if (receiveErrorType != ClientReceiveErrorTypeEnum.Success) return receiveErrorType;
                                    }
                                    if (receiveSize == 0) return ClientReceiveErrorTypeEnum.Success;
                                    receiveIndex += dataSize;
                                }
                                else return ClientReceiveErrorTypeEnum.DataSizeError;
                            }
                            else return ClientReceiveErrorTypeEnum.CallbackIdentityError;
                        }
                        else if ((callbackIdentity.Index & (uint)CallbackFlagsEnum.SendData) == 0)
                        {
                            if ((receiveSize -= sizeof(CallbackIdentity) + sizeof(int)) >= 0)
                            {
                                receiveErrorType = onReceive((CommandClientReturnTypeEnum)(*(start + sizeof(CallbackIdentity))));
                                if (receiveErrorType == ClientReceiveErrorTypeEnum.Success)
                                {
                                    if (receiveSize == 0) return ClientReceiveErrorTypeEnum.Success;
                                    receiveIndex += sizeof(CallbackIdentity) + sizeof(int);
                                }
                                else return receiveErrorType;
                            }
                            else return ClientReceiveErrorTypeEnum.CallbackIdentityError;
                        }
                        else if (receiveSize >= (sizeof(CallbackIdentity) + sizeof(int) * 2))
                        {
                            CommandClientReturnTypeEnum returnType = (CommandClientReturnTypeEnum)(*(start + (sizeof(CallbackIdentity) + sizeof(int))));
                            if ((dataSize = *(int*)(start + sizeof(CallbackIdentity)) - sizeof(int)) >= 0
                                && (receiveSize -= dataSize + (sizeof(CallbackIdentity) + sizeof(int) * 2)) >= 0)
                            {
                                if (dataSize != 0)
                                {
                                    string errorMessage = string.Empty;
                                    start += sizeof(CallbackIdentity) + sizeof(int) * 2;
                                    if (AutoCSer.SimpleSerialize.Deserializer.NotNull(start, ref errorMessage, start + dataSize) != null)
                                    {
                                        receiveErrorType = onReceive(returnType, errorMessage);
                                        if (receiveErrorType == ClientReceiveErrorTypeEnum.Success)
                                        {
                                            if (receiveSize == 0) return ClientReceiveErrorTypeEnum.Success;
                                            receiveIndex += dataSize + (sizeof(CallbackIdentity) + sizeof(int) * 2);
                                        }
                                        else return receiveErrorType;
                                    }
                                    else
                                    {
                                        Client.Log.ErrorIgnoreException($"{errorMessage} {nameof(ClientInterfaceMethod.DeserializeError)}");
                                        if (receiveSize == 0) return ClientReceiveErrorTypeEnum.Success;
                                        receiveIndex += dataSize + (sizeof(CallbackIdentity) + sizeof(int) * 2);
                                    }
                                }
                                else
                                {
                                    receiveErrorType = onReceive(returnType, string.Empty);
                                    if (receiveErrorType == ClientReceiveErrorTypeEnum.Success)
                                    {
                                        if (receiveSize == 0) return ClientReceiveErrorTypeEnum.Success;
                                        receiveIndex += sizeof(CallbackIdentity) + sizeof(int) * 2;
                                    }
                                    else return receiveErrorType;
                                }
                            }
                            else return ClientReceiveErrorTypeEnum.DataSizeError;
                        }
                        else return ClientReceiveErrorTypeEnum.CallbackIdentityError;
                    }
                    while (receiveSize >= sizeof(CallbackIdentity));
                }
            }
            return ClientReceiveErrorTypeEnum.CallbackIdentityLess;
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="data">Data</param>
        /// <param name="value">Target object</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>Return false on failure</returns>
        internal unsafe bool Deserialize<T>(ref SubArray<byte> data, ref T value, bool isSimpleSerialize)
            where T : struct
        {
            if (isSimpleSerialize)
            {
                fixed (byte* dataFixed = data.GetFixedBuffer())
                {
                    byte* start = dataFixed + data.Start, end = start + data.Length;
                    return SimpleSerialize.Deserializer<T>.DefaultDeserializer(start, ref value, end) == end;
                }
            }
            return (receiveDeserializer ?? createReceiveDeserializer()).IndependentDeserialize(ref data, ref value);
        }
        /// <summary>
        /// Receive binary deserialization of data
        /// 接收数据二进制反序列化
        /// </summary>
        /// <returns></returns>
        private AutoCSer.BinaryDeserializer createReceiveDeserializer()
        {
            receiveDeserializer = AutoCSer.Threading.LinkPool<BinaryDeserializer>.Default.Pop() ?? new AutoCSer.BinaryDeserializer();
            receiveDeserializer.SetContext(this, Client.BinaryDeserializeConfig);
            return receiveDeserializer;
        }
        /// <summary>
        /// Check whether the current serialization is in an IO synchronization environment
        /// 检查当前序列化是否 IO 同步环境
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool CheckSynchronousIO(BinaryDeserializer deserializer)
        {
            var socket = deserializer.Context as CommandClientSocket;
            return socket == null || socket.OnReceiveThreadId == System.Environment.CurrentManagedThreadId;
        }
        /// <summary>
        /// Send data
        /// </summary>
        private unsafe void buildOutput()
        {
            var head = default(Command);
            var end = default(Command);
            ByteArrayBuffer sendBuffer = default(ByteArrayBuffer), sendTransferBuffer = default(ByteArrayBuffer);
#if NetStandard21
            SubArray<byte> newSendData = new SubArray<byte>(AutoCSer.EmptyArray<byte>.Array);
#else
            ByteArrayBuffer sendCopyBuffer = default(ByteArrayBuffer);
#endif
            ClientBuildInfo buildInfo = new ClientBuildInfo { SendBufferSize = Client.SendBufferPool.Size };
            bool isSerializeCopyString = Client.Config.IsSerializeCopyString;
            try
            {
                Client.SendBufferPool.Get(ref sendBuffer);
                SocketError socketError;
#if NetStandard21
                AutoCSer.Memory.Pointer sendData = default(AutoCSer.Memory.Pointer);
#else
                SubArray<byte> sendData = new SubArray<byte>(EmptyArray<byte>.Array);
#endif
                int bufferLength = sendBuffer.Buffer.notNull().BufferSize, sendBufferMaxSize = Client.Config.SendBufferMaxSize;
                if (sendBufferMaxSize <= 0) sendBufferMaxSize = int.MaxValue;
                using (UnmanagedStream outputStream = (OutputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetContext(this, ref isSerializeCopyString))
                {
                    do
                    {
                        buildInfo.IsNewBuffer = 0;
                        fixed (byte* dataFixed = sendBuffer.GetFixedBuffer())
                        {
                            byte* start = dataFixed + sendBuffer.StartIndex;
                        RESET:
                            bool isWait = true;
                            if (outputStream.Data.Pointer.Byte != start) outputStream.Reset(start, sendBuffer.Buffer.notNull().BufferSize);
                            buildInfo.Clear();
                            outputStream.Data.Pointer.CurrentIndex = Command.StreamStartIndex;
                        WAIT:
                            if (head == null)
                            {
                                outputWaitHandle.WaitOne();
                                if (isClosed != 0) return;
                                AutoCSer.Threading.ThreadYield.YieldOnly();
                                if ((head = commands.GetQueue(out end)) == null) return;
                            }
                            LOOP:
                            buildInfo.FreeCount = 0;
                            do
                            {
                                head = head.Build(ref buildInfo);
                                if (buildInfo.IsFullSend != 0)
                                {
                                    addBuildCommandCount(buildInfo.GetFreeCount());
                                    goto SETDATA;
                                }
                            }
                            while (head != null);
                            addBuildCommandCount(buildInfo.GetFreeCount());
                            if (!commands.IsEmpty)
                            {
                                isWait = buildInfo.Count == 0;
                                goto WAIT;
                            }
                            if (isWait)
                            {
                                isWait = buildInfo.Count == 0;
                                if (!commands.IsEmpty) goto WAIT;
                            }
                            if (buildInfo.Count == 0)
                            {
                                isWait = true;
                                goto WAIT;
                            }
                        SETDATA:
#if NetStandard21
                            int outputLength = outputStream.Data.Pointer.CurrentIndex;
                            sendData.CopyMoveData(ref outputStream.Data.Pointer, Command.StreamStartIndex);
                            if (outputLength > bufferLength && outputLength <= sendBufferMaxSize)
                            {
                                sendBuffer.Free();
                                ByteArrayPool.GetBuffer(ref sendBuffer, outputLength);
                                buildInfo.IsNewBuffer = 1;
                                //Console.WriteLine("*NewBuffer*");
                            }
                            //Console.WriteLine(buildInfo.Count.toString());
                            if (buildInfo.Count == 1)
                            {
                                int dataStartIndex = buildInfo.IsCallback ? (Command.StreamStartIndex + sizeof(CallbackIdentity)) : Command.StreamStartIndex, transferDataStartIndex = dataStartIndex + sizeof(int);
                                ReadOnlySpan<byte> oldDSendData = sendData.GetReadOnlySpan(dataStartIndex);
                                if (oldDSendData.Length != 0 && Client.Config.TransferEncode(this, oldDSendData, ref sendTransferBuffer, ref newSendData, transferDataStartIndex, transferDataStartIndex))
                                {
                                    int transferDataSize = newSendData.Length;
                                    newSendData.MoveStart(-transferDataStartIndex);
                                    fixed (byte* sendDataFixed = newSendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + newSendData.Start, oldDataStart = sendData.Byte;
                                        *(int*)dataStart = *(int*)oldDataStart;
                                        if (buildInfo.IsCallback)
                                        {
                                            *(CallbackIdentity*)(dataStart + sizeof(int)) = *(CallbackIdentity*)(oldDataStart + sizeof(uint));
                                        }
                                        *(int*)(dataStart + (dataStartIndex - sizeof(int))) = -transferDataSize;
                                        *(int*)(dataStart + dataStartIndex) = oldDSendData.Length;
                                    }
                                    sendData.CurrentIndex = 0;
                                }
                            }
                            else
                            {
                                if (Client.Config.TransferEncode(this, sendData.GetReadOnlySpan(), ref sendTransferBuffer, ref newSendData, sizeof(uint) + sizeof(int) * 2, sizeof(int)))
                                {
                                    int transferDataSize = newSendData.Length;
                                    newSendData.MoveStart(-(sizeof(uint) + sizeof(int) * 2));
                                    fixed (byte* sendDataFixed = newSendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + newSendData.Start;
                                        *(uint*)dataStart = (uint)CommandListener.MergeMethodIndex | (uint)CommandFlagsEnum.SendData;
                                        *(int*)(dataStart + sizeof(int)) = -transferDataSize;
                                        *(int*)(dataStart + (sizeof(uint) + sizeof(int))) = sendData.CurrentIndex;
                                    }
                                    sendData.CurrentIndex = 0;
                                }
                                else
                                {
                                    int dataLength;
                                    byte* dataStart = sendData.GetMoveData(-Command.StreamStartIndex, out dataLength);
                                    *(uint*)dataStart = (uint)CommandListener.MergeMethodIndex | (uint)CommandFlagsEnum.SendData;
                                    *(int*)(dataStart + sizeof(int)) = dataLength;
                                }
                            }
#else
                            int outputLength = outputStream.Data.Pointer.CurrentIndex, dataLength = outputLength - Command.StreamStartIndex;//, transferDataSize = 0
                            if (outputLength <= bufferLength)
                            {
                                if (outputStream.Data.Pointer.ByteSize != bufferLength)
                                {
                                    AutoCSer.Common.CopyTo(outputStream.Data.Pointer.Byte + Command.StreamStartIndex, start + Command.StreamStartIndex, dataLength);
                                }
                                sendData.Set(sendBuffer.Buffer.notNull().Buffer, sendBuffer.StartIndex + Command.StreamStartIndex, dataLength);
                            }
                            else
                            {
                                outputStream.Data.Pointer.GetBuffer(ref sendCopyBuffer, Command.StreamStartIndex);
                                sendData.Set(sendCopyBuffer.Buffer.notNull().Buffer, sendCopyBuffer.StartIndex + Command.StreamStartIndex, dataLength);
                                if (sendCopyBuffer.Buffer.notNull().BufferSize <= sendBufferMaxSize)
                                {
                                    sendBuffer.CopyFromFree(ref sendCopyBuffer);
                                    buildInfo.IsNewBuffer = 1;
                                }
                            }
                            if (buildInfo.Count == 1)
                            {
                                SubArray<byte> oldSendData = sendData;
                                int dataStartIndex = buildInfo.IsCallback ? (Command.StreamStartIndex + sizeof(CallbackIdentity)) : Command.StreamStartIndex, transferDataStartIndex = dataStartIndex + sizeof(int);
                                dataLength -= dataStartIndex;
                                if (dataLength > 0 && Client.Config.TransferEncode(this, sendData.Array, sendData.Start + dataStartIndex, dataLength, ref sendTransferBuffer, ref sendData, transferDataStartIndex, transferDataStartIndex))
                                {
                                    int transferDataSize = sendData.Length;
                                    sendData.MoveStart(-transferDataStartIndex);
                                    fixed (byte* sendDataFixed = sendData.GetFixedBuffer(), oldSendDataFixed = oldSendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + sendData.Start, oldDataStart = oldSendDataFixed + oldSendData.Start;
                                        *(int*)dataStart = *(int*)oldDataStart;
                                        if (buildInfo.IsCallback)
                                        {
                                            *(CallbackIdentity*)(dataStart + sizeof(int)) = *(CallbackIdentity*)(oldDataStart + sizeof(uint));
                                        }
                                        *(int*)(dataStart + (dataStartIndex - sizeof(int))) = -transferDataSize;
                                        *(int*)(dataStart + dataStartIndex) = dataLength;
                                    }
                                }
                            }
                            else
                            {
                                if (Client.Config.TransferEncode(this, sendData.Array, sendData.Start, dataLength, ref sendTransferBuffer, ref sendData, sizeof(uint) + sizeof(int) * 2, sizeof(int)))
                                {
                                    int transferDataSize = sendData.Length;
                                    sendData.MoveStart(-(sizeof(uint) + sizeof(int) * 2));
                                    fixed (byte* sendDataFixed = sendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + sendData.Start;
                                        *(uint*)dataStart = (uint)CommandListener.MergeMethodIndex | (uint)CommandFlagsEnum.SendData;
                                        *(int*)(dataStart + sizeof(int)) = -transferDataSize;
                                        *(int*)(dataStart + (sizeof(uint) + sizeof(int))) = dataLength;
                                    }
                                }
                                else
                                {
                                    dataLength = sendData.Length;
                                    sendData.MoveStart(-Command.StreamStartIndex);
                                    fixed (byte* sendDataFixed = sendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + sendData.Start;
                                        *(uint*)dataStart = (uint)CommandListener.MergeMethodIndex | (uint)CommandFlagsEnum.SendData;
                                        *(int*)(dataStart + sizeof(int)) = dataLength;
                                    }
                                }
                            }
#endif
                        SEND:
#if NetStandard21
                            int count = sendData.CurrentIndex != 0
                                ? Socket.Send(new ReadOnlySpan<byte>(sendData.Data, sendData.CurrentIndex), SocketFlags.None, out socketError)
                                : Socket.Send(newSendData.Array, newSendData.Start, newSendData.Length, SocketFlags.None, out socketError);
                            if ((sendData.CurrentIndex != 0 ? sendData.MoveData(count) : newSendData.GetMoveStartLength(count)) == 0)
#else
                            int count = Socket.Send(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError);
                            if (sendData.GetMoveStartLength(count) == 0)
#endif
                            {
                                sendTransferBuffer.Free();
                                if (buildInfo.IsNewBuffer == 0)
                                {
#if !NetStandard21
                                    sendCopyBuffer.Free();
#endif
                                    if (head == null) goto RESET;
                                    if (outputStream.Data.Pointer.Byte != start) outputStream.Reset(start, sendBuffer.Buffer.notNull().BufferSize);
                                    buildInfo.Clear();
                                    outputStream.Data.Pointer.CurrentIndex = Command.StreamStartIndex;
                                    isWait = true;
                                    goto LOOP;
                                }
                                //if (head != null)
                                //{
                                //    if(commands.IsPushHead(head, end.notNull())) outputWaitHandle.Set();
                                //    head = null;
                                //}
                                goto FIXEDEND;
                            }
                            if (socketError == SocketError.Success && count > 0) goto SEND;
                            buildInfo.IsError = true;
                            buildInfo.IsNewBuffer = 0;
                        FIXEDEND:;
                        }
                    }
                    while (buildInfo.IsNewBuffer != 0);
                }
            }
            catch (Exception exception)
            {
                if (!object.ReferenceEquals(Socket, CommandServerConfigBase.NullSocket))
                {
                    Client.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    buildInfo.IsError = true;
                }
            }
            finally
            {
                if (buildInfo.IsError) Shutdown();
                sendBuffer.Free();
#if !NetStandard21
                sendCopyBuffer.Free();
#endif
                sendTransferBuffer.Free();
                if (!object.ReferenceEquals(OutputSerializer, CommandServerConfigBase.NullBinarySerializer))
                {
                    OutputSerializer.FreeContext(isSerializeCopyString);
                    OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
                }
                if (head == null) head = commands.Get(out end);
#pragma warning disable CS8601
                else commands.GetToEnd(ref end);
#pragma warning restore CS8601
                //if (CommandPool.IsDisposed != 0) 
                head = CommandPool.Free(head, end, CommandServer.KeepCallbackCommand.CommandPoolIndex);
                if (head != null) Command.CancelLink(head);
            }
        }
        //        /// <summary>
        //        /// 启动命令批处理（非线程安全，不允许多线程并发操作），不支持 同步等待命令 与 保持回调命令
        //        /// </summary>
        //        /// <typeparam name="T">命令类型</typeparam>
        //        /// <returns>命令批处理，失败返回 null</returns>
        //#if NetStandard21
        //        public CommandBatch? BatchStart<T>() where T : Command
        //#else
        //        public CommandBatch BatchStart<T>() where T : Command
        //#endif
        //        {
        //            return object.ReferenceEquals(CommandBatch, Null.CommandBatch) && object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref CommandBatch, new CommandBatch(this, typeof(T)), Null.CommandBatch), Null.CommandBatch) ? CommandBatch : null;
        //        }
        //        /// <summary>
        //        /// 启动命令批处理（非线程安全，不允许多线程并发操作），不支持 同步等待命令 与 保持回调命令
        //        /// </summary>
        //        /// <returns>命令批处理，失败返回 null</returns>
        //#if NetStandard21
        //        public CommandBatch? BatchStart()
        //#else
        //        public CommandBatch BatchStart()
        //#endif
        //        {
        //            return object.ReferenceEquals(CommandBatch, Null.CommandBatch) && object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref CommandBatch, new CommandBatch(this, typeof(Command)), Null.CommandBatch), Null.CommandBatch) ? CommandBatch : null;
        //        }
        //        /// <summary>
        //        /// 结束命令批处理
        //        /// </summary>
        //        /// <param name="commandBatch"></param>
        //        internal void BatchEnd(CommandBatch commandBatch)
        //        {
        //            System.Threading.Interlocked.CompareExchange(ref this.CommandBatch, Null.CommandBatch, commandBatch);
        //            var head = commandBatch.Head;
        //            if (head != null)
        //            {
        //                if (isClosed == 0)
        //                {
        //                    Interlocked.Add(ref commandCount, commandBatch.Count);
        //                    if (commands.IsPushHeadLink(head, commandBatch.End.notNull())) outputWaitHandle.Set();
        //                }
        //                else Command.CancelLink(head);
        //            }
        //        }
        ///// <summary>
        ///// 尝试添加命令
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal CommandPushStateEnum TryPushBatch(Command command)
        //{
        //    return CommandBatch.Push(command) ? TryPush(command) : CommandPushStateEnum.Batch;
        //}
        /// <summary>
        /// Add a command without checking the count
        /// 添加命令，不检查计数
        /// </summary>
        /// <param name="command"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushNotCheckCount(Command command)
        {
            if (isClosed == 0)
            {
                Interlocked.Increment(ref commandCount);
                if (commands.IsPushHead(command)) outputWaitHandle.Set();
            }
        }
        /// <summary>
        /// Try adding commands
        /// 尝试添加命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal CommandPushStateEnum TryPush(Command command)
        {
            if (commandCount - buildCommandCount < commandQueueCount)
            {
                if (isClosed == 0)
                {
                    Interlocked.Increment(ref commandCount);
                    if (commands.IsPushHead(command)) outputWaitHandle.Set();
                    return CommandPushStateEnum.Success;
                }
            }
            else if (isClosed == 0)
            {
                waitPushCommands.Push(command);
                if (isClosed == 0)
                {
                    //AutoCSer.Threading.ThreadYield.YieldOnly();
                    return CommandPushStateEnum.WaitCount;
                }
                closeWaitPush();
            }
            return CommandPushStateEnum.Closed;
        }

        /// <summary>
        /// Heart rate detection
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Check()
        {
            if (isClosed == 0)
            {
                if (!Client.IsDisposed)
                {
#if !DEBUG
                    if (commands.TryPushHead(new CheckCommand(this)))
                    {
                        Interlocked.Increment(ref commandCount);
                        outputWaitHandle.Set();
                    }
#endif
                    return true;
                }
                Shutdown();
            }
            return false;
        }
        /// <summary>
        /// Send custom data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CustomDataCommand SendCustomData(byte[] data)
        {
            return new CustomDataCommand(this, data);
        }
        /// <summary>
        /// Send custom data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CustomDataCommand SendCustomData(SubArray<byte> data)
        {
            return new CustomDataCommand(this, ref data);
        }
    }
}
