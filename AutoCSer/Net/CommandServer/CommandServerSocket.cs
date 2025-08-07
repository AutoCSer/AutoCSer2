using AutoCSer.Memory;
using AutoCSer.Threading;
using System;
using System.Net.Sockets;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Net.CommandServer;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command server socket
    /// 命令服务套接字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed class CommandServerSocket
    {
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        public readonly CommandListener Server;
        /// <summary>
        /// Socket
        /// </summary>
        private readonly Socket socket;
        /// <summary>
        /// Receive asynchronous callback for data
        /// 接收数据异步回调
        /// </summary>
        private readonly EventHandler<SocketAsyncEventArgs> onReceiveAsyncCallback;
        /// <summary>
        /// Attempt to send a data delegate
        /// 尝试发送数据委托
        /// </summary>
        private readonly Action buildOutputHandle;
        /// <summary>
        /// Asynchronously keep callback collection access lock
        /// 异步保持回调集合访问锁
        /// </summary>
        private readonly object keepCallbackLock;
        /// <summary>
        /// Socket close event
        /// 套接字关闭事件
        /// </summary>
#if NetStandard21
        private HashSet<HashObject<Action>>? onClosedHashSet;
#else
        private HashSet<HashObject<Action>> onClosedHashSet;
#endif
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
#pragma warning disable CS0169
        private readonly CpuCachePad pad0;
#pragma warning restore CS0169
        /// <summary>
        /// Command bitmap access lock
        /// 命令位图访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock commandDataLock;
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
        /// The socket context binds the collection of server instances
        /// 套接字上下文绑定服务端实例集合
        /// </summary>
        private CommandServerBindContextController[] bindControllers;
        /// <summary>
        /// The command bitmap that allows access
        /// 允许访问的命令位图
        /// </summary>
        private AutoCSer.Memory.Pointer commandData;
        /// <summary>
        /// Receive data socket asynchronous event object
        /// 接收数据套接字异步事件对象
        /// </summary>
        private SocketAsyncEventArgs receiveAsyncEventArgs;
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
        /// Verify the timeout period
        /// 验证超时时间
        /// </summary>
        private DateTime verifyTimeout;
        /// <summary>
        /// Receive data buffer
        /// 接收数据缓冲区
        /// </summary>
        private ByteArrayBuffer receiveBuffer;
        /// <summary>
        /// Temporary received data buffer
        /// 临时接收数据缓冲区
        /// </summary>
        private ByteArrayBuffer receiveBigBuffer;
        /// <summary>
        /// The starting position for receiving data
        /// 接收数据起始位置
        /// </summary>
        private unsafe byte* receiveDataStart;
        /// <summary>
        /// The number of bytes received last time
        /// 上一次接收字节数量
        /// </summary>
        private int lastReceiveSize = ushort.MaxValue;
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
        /// The current parsing command service controller
        /// 当前解析命令服务控制器
        /// </summary>
        private CommandServerController controller;
        /// <summary>
        /// The current parsing command service controller
        /// 当前解析命令服务控制器
        /// </summary>
        public CommandServerController CurrentController { get { return controller; } }
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        internal ServerInterfaceMethod Method;
        /// <summary>
        /// The current server is offline for counting objects
        /// 当前服务端下线计数对象
        /// </summary>
        internal OfflineCount OfflineCount;
        /// <summary>
        /// The session callback identifier is currently being processed
        /// 当前处理会话回调标识
        /// </summary>
        internal CallbackIdentity CallbackIdentity;
        /// <summary>
        /// Customize the data byte length
        /// 自定义数据字节长度
        /// </summary>
        private int customDataSize
        {
            get { return (int)CallbackIdentity.Index; }
            set { CallbackIdentity.Index = (uint)value; }
        }
        /// <summary>
        /// Synchronous output head node
        /// 同步输出头节点
        /// </summary>
        private ServerOutput outputHead;
        /// <summary>
        /// Synchronize the output tail node
        /// 同步输出尾节点
        /// </summary>
        private ServerOutput outputEnd;
#if !AOT
        /// <summary>
        /// Format the remote expression deserialization data
        /// 格式化远程表达式反序列化数据
        /// </summary>
#if NetStandard21
        private AutoCSer.Net.CommandServer.RemoteExpression.FormatDeserialize? remoteExpressionFormatDeserialize;
#else
        private AutoCSer.Net.CommandServer.RemoteExpression.FormatDeserialize remoteExpressionFormatDeserialize;
#endif
        /// <summary>
        /// A collection of arguments for create a remote expression
        /// 创建远程表达式参数集合
        /// </summary>
        private object[] createRemoteExpressionParameters;
#endif
        /// <summary>
        /// The current command method sequence number + command flag bit information
        /// 当前命令方法序号 + 命令标志位信息
        /// </summary>
        private uint commandMethodIndex;
        /// <summary>
        /// Current command method sequence number
        /// 当前命令方法序号
        /// </summary>
        internal int CommandMethodIndex { get { return (int)(commandMethodIndex & Command.MethodIndexAnd); } }
        /// <summary>
        /// Receive the data thread ID
        /// 接收数据线程ID
        /// </summary>
        private int onReceiveThreadId;
        /// <summary>
        /// The data receiving socket is incorrect
        /// 接收数据套接字错误
        /// </summary>
        private SocketError receiveSocketError;
        ///// <summary>
        ///// 允许验证失败次数
        ///// </summary>
        //private byte verifyMethodErrorCount;
        /// <summary>
        /// The command service verifies the result status
        /// 命令服务验证结果状态
        /// </summary>
        internal volatile CommandServerVerifyStateEnum VerifyState;
        /// <summary>
        /// Receive data callback type
        /// 接收数据回调类型
        /// </summary>
        private ServerReceiveTypeEnum receiveType;
        /// <summary>
        /// Received data error type
        /// 接收数据错误类型
        /// </summary>
        private ServerReceiveErrorTypeEnum receiveErrorType;
        /// <summary>
        /// Is short connection
        /// 是否短连接
        /// </summary>
        public readonly bool IsShortLink;
#pragma warning disable CS0169
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad1;
#pragma warning restore CS0169
        /// <summary>
        /// The next output socket
        /// 下一个输出套接字
        /// </summary>
#if NetStandard21
        private CommandServerSocket? nextOutputSocket;
#else
        private CommandServerSocket nextOutputSocket;
#endif
        /// <summary>
        /// The server-side socket outputs information
        /// 服务端套接字输出信息
        /// </summary>
        private LinkStack<ServerOutput> outputs;
        /// <summary>
        /// The first node of the unprocessed socket queue
        /// 未处理套接字队列首节点
        /// </summary>
#if NetStandard21
        private ServerOutput? buildOutputHead;
#else
        private ServerOutput buildOutputHead;
#endif
        /// <summary>
        /// The tail node of the socket queue was not processed
        /// 未处理套接字队列尾节点
        /// </summary>
#if NetStandard21
        private ServerOutput? buildOutputEnd;
#else
        private ServerOutput buildOutputEnd;
#endif
        /// <summary>
        /// Is it being output
        /// 是否正在输出
        /// </summary>
        private int isOutput;
        /// <summary>
        /// Close the socket access lock
        /// Close the socket访问锁
        /// </summary>
        private int closeLock;
        /// <summary>
        /// Whether the socket closing operation has been triggered
        /// 是否已经触发套接字关闭操作
        /// </summary>
        public bool IsClose { get { return closeLock != 0; } }
#pragma warning disable CS0169
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad2;
#pragma warning restore CS0169
        /// <summary>
        /// Asynchronously keep callback collection
        /// 异步保持回调集合
        /// </summary>
#if NetStandard21
        private ReusableDictionary<CallbackIdentity, CommandServerKeepCallback>? keepCallbacks;
#else
        private ReusableDictionary<CallbackIdentity, CommandServerKeepCallback> keepCallbacks;
#endif
        /// <summary>
        /// Short connection asynchronous keep callback
        /// 短连接异步保持回调
        /// </summary>
#if NetStandard21
        private CommandServerKeepCallback? shortLinkKeepCallback;
#else
        private CommandServerKeepCallback shortLinkKeepCallback;
#endif
        /// <summary>
        /// Output data binary serialization
        /// 输出数据二进制序列化
        /// </summary>
        internal BinarySerializer OutputSerializer;
        /// <summary>
        /// Send asynchronous callbacks for data
        /// 发送数据异步回调
        /// </summary>
#if NetStandard21
        private EventHandler<SocketAsyncEventArgs>? onSendAsyncCallback;
#else
        private EventHandler<SocketAsyncEventArgs> onSendAsyncCallback;
#endif
        /// <summary>
        /// Send asynchronous data events
        /// 发送数据异步事件
        /// </summary>
        private SocketAsyncEventArgs sendAsyncEventArgs;
        /// <summary>
        /// Output data buffer
        /// 输出数据缓冲区
        /// </summary>
        private ByteArrayBuffer sendBuffer;
        /// <summary>
        /// Output the encoded data buffer
        /// 输出编码数据缓冲区
        /// </summary>
        private ByteArrayBuffer sendTransferBuffer;
        /// <summary>
        /// Output the copied data buffer
        /// 输出复制数据缓冲区
        /// </summary>
        private ByteArrayBuffer sendCopyBuffer;
        /// <summary>
        /// Send data
        /// 发送数据
        /// </summary>
        private SubArray<byte> sendData;
        /// <summary>
        /// The number of bytes sent last time
        /// 上一次发送字节数量
        /// </summary>
        private int lastSendSize = ushort.MaxValue;
        /// <summary>
        /// Error in sending data socket
        /// 发送数据套接字错误
        /// </summary>
        private SocketError sendSocketError;
        /// <summary>
        /// Server socket sends data thread type
        /// 服务端套接字发送数据线程类型
        /// </summary>
        private readonly CommandServerSocketBuildOutputThreadEnum buildOutputThreadEnum;
        /// <summary>
        /// String binary serialization directly copies memory data
        /// 字符串二进制序列化直接复制内存数据
        /// </summary>
        private bool isSerializeCopyString;
        /// <summary>
        /// Whether to trigger the socket close operation
        /// 是否触发套接字关闭操作
        /// </summary>
        private bool isCloseSocket;
        /// <summary>
        /// Whether it is necessary to close the short connection
        /// 是否需要关闭短连接
        /// </summary>
        private bool isCloseShortLink;
        /// <summary>
        /// Whether to cancel the asynchronous keep callback
        /// 是否取消异步保持回调
        /// </summary>
        internal bool IsCancelKeepCallback;

        /// <summary>
        /// Empty command service socket, used to simulate the server-side context
        /// 空命令服务套接字，用于模拟服务端上下文
        /// </summary>
        private CommandServerSocket()
        {
            Server = CommandListener.Null;
            keepCallbackLock = socket = CommandServerConfigBase.NullSocket;
            buildOutputHandle = AutoCSer.Common.EmptyAction;
            onReceiveAsyncCallback = onReceive;
            receiveAsyncEventArgs = CommandServerConfigBase.NullSocketAsyncEventArgs;

            controller = CommandListener.Null.Controller;
#if NetStandard21
            Method = CommandServerConfig.NullServerInterfaceMethod;
#endif
            OfflineCount = OfflineCount.Null;
            OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
            sendAsyncEventArgs = CommandServerConfigBase.NullSocketAsyncEventArgs;
            outputHead = outputEnd = CommandServerConfig.NullServerOutput;
            bindControllers = EmptyArray<CommandServerBindContextController>.Array;
#if !AOT
            createRemoteExpressionParameters = EmptyArray<object>.Array;
#endif
        }
        /// <summary>
        /// Command server socket
        /// </summary>
        /// <param name="server"></param>
        /// <param name="socket"></param>
        internal CommandServerSocket(CommandListener server, Socket socket)
        {
            this.Server = server;
            keepCallbackLock = this.socket = socket;
            IsShortLink = server.Config.IsShortLink;
            buildOutputThreadEnum = server.Config.BuildOutputThread;
            isSerializeCopyString = server.Config.IsSerializeCopyString;
            buildOutputHandle = buildOutput;
            onReceiveAsyncCallback = onReceive;
            //verifyMethodErrorCount = server.Config.MaxVerifyMethodErrorCount;
            receiveAsyncEventArgs = Server.SocketAsyncEventArgsPool.Get();

            controller = CommandListener.Null.Controller;
#if NetStandard21
            Method = CommandServerConfig.NullServerInterfaceMethod;
#endif
            OfflineCount = OfflineCount.Null;
            OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
            sendAsyncEventArgs = CommandServerConfigBase.NullSocketAsyncEventArgs;
            outputHead = outputEnd = CommandServerConfig.NullServerOutput;
            bindControllers = EmptyArray<CommandServerBindContextController>.Array;
#if !AOT
            createRemoteExpressionParameters = EmptyArray<object>.Array;
#endif
        }
        /// <summary>
        /// Get the socket context binding server instance
        /// 获取套接字上下文绑定服务端实例
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal CommandServerBindContextController? GetBindController(int index)
#else
        internal CommandServerBindContextController GetBindController(int index)
#endif
        {
            if (bindControllers.Length != 0) return bindControllers[index];
            bindControllers = new CommandServerBindContextController[Server.Controllers.Length];
            return null;
        }
        /// <summary>
        /// Get the socket context binding server instance
        /// 获取套接字上下文绑定服务端实例
        /// </summary>
        /// <param name="index"></param>
        /// <param name="controller"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBindController(int index, CommandServerBindContextController controller)
        {
            bindControllers[index] = controller;
        }
        /// <summary>
        /// Set the command bitmap that is allowed for access
        /// 设置允许访问的命令位图
        /// </summary>
        /// <returns>Return false on failure</returns>
        public unsafe bool SetCommandData()
        {
            commandDataLock.Enter();
            try
            {
                if (closeLock == 0)
                {
                    int size = ((Server.CommandEndIndex + 63) >> 6) << 3;
                    if (commandData.Data == null)
                    {
                        commandDataLock.SleepFlag = 1;
                        commandData = UnmanagedPool.CachePage.GetPointer(size);
                        commandData.Clear();
                        commandData.CurrentIndex = commandData.ByteSize << 3;
                    }
                    else if (commandData.ByteSize < size)
                    {
                        commandDataLock.SleepFlag = 1;
                        if (commandData.ByteSize == UnmanagedPool.CachePageSize) commandData.NewByteSize(size, UnmanagedPool.CachePage);
                        else commandData.NewByteSize(size);
                        commandData.CurrentIndex = commandData.ByteSize << 3;
                    }
                    return true;
                }
            }
            finally { commandDataLock.ExitSleepFlag(); }
            return false;
        }
        /// <summary>
        /// Clear the commands that allow access
        /// 清除允许访问的命令
        /// </summary>
        public unsafe void ClearCommandData()
        {
            if (SetCommandData())
            {
                commandDataLock.Enter();
                commandData.Clear();
                commandDataLock.Exit();
            }
        }
        /// <summary>
        /// Set the commands allowed for access
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        private unsafe void setCommand(CommandServerController controller, int methodIndex)
        {
            int methodMapIndex = methodIndex + controller.CommandStartIndex - CommandListener.MethodStartIndex;
            commandDataLock.Enter();
            commandData.CheckSetBit(methodMapIndex);
            commandDataLock.Exit();
        }
        /// <summary>
        /// Set the commands allowed for access
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="methodIndex">The method number in the command service controller
        /// 命令服务控制器中的方法编号</param>
        /// <param name="controller">Command service controller
        /// 命令服务控制器</param>
#if NetStandard21
        public void SetCommand(int methodIndex, CommandServerController? controller = null)
#else
        public void SetCommand(int methodIndex, CommandServerController controller = null)
#endif
        {
            if (SetCommandData())
            {
                if (controller == null)
                {
                    if (Server.Controllers.Length != 1) throw new Exception(AutoCSer.Common.Culture.CommandServerMissingControllerParameter);
                    controller = Server.Controller;
                }
                else if (!object.ReferenceEquals(controller.Server, Server)) throw new Exception(AutoCSer.Common.Culture.CommandServerControllerServerNotMatch);
                if ((uint)methodIndex >= (uint)controller.Methods.Length) throw new Exception(AutoCSer.Common.Culture.GetCommandServerControllerMethodIndexLimit(methodIndex, controller.Methods.Length));
                setCommand(controller, methodIndex);
            }
        }
        /// <summary>
        /// Set the commands allowed for access
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="methodName">Command service method name
        /// 命令服务方法名称</param>
        /// <param name="controller">Command service controller
        /// 命令服务控制器</param>
        /// <returns>Return false on failure</returns>
#if NetStandard21
        public bool SetCommand(string methodName, CommandServerController? controller = null)
#else
        public bool SetCommand(string methodName, CommandServerController controller = null)
#endif
        {
            if (SetCommandData())
            {
                if (controller == null)
                {
                    if (Server.Controllers.Length != 1) throw new Exception(AutoCSer.Common.Culture.CommandServerMissingControllerParameter);
                    controller = Server.Controller;
                }
                else if (!object.ReferenceEquals(controller.Server, Server)) throw new Exception(AutoCSer.Common.Culture.CommandServerControllerServerNotMatch);
                foreach (var method in controller.Methods)
                {
                    if (method != null && method.Method.Name == methodName)
                    {
                        setCommand(controller, method.MethodIndex);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Set the commands allowed for access
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="methodNames">A collection of command service method names
        /// 命令服务方法名称集合</param>
        /// <param name="controller">Command service controller
        /// 命令服务控制器</param>
        /// <returns>The number of matching methods
        /// 匹配方法数量</returns>
#if NetStandard21
        public int SetCommand(HashSet<string> methodNames, CommandServerController? controller = null)
#else
        public int SetCommand(HashSet<string> methodNames, CommandServerController controller = null)
#endif
        {
            if (SetCommandData())
            {
                if (controller == null)
                {
                    if (Server.Controllers.Length != 1) throw new Exception(AutoCSer.Common.Culture.CommandServerMissingControllerParameter);
                    controller = Server.Controller;
                }
                else if (!object.ReferenceEquals(controller.Server, Server)) throw new Exception(AutoCSer.Common.Culture.CommandServerControllerServerNotMatch);
                int count = 0;
                foreach (var method in controller.Methods)
                {
                    if (method != null && methodNames.Contains(method.Method.Name))
                    {
                        setCommand(controller, method.MethodIndex);
                        if (++count == methodNames.Count) return count;
                    }
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        /// <param name="verifyState"></param>
        /// <returns></returns>
        internal bool SetVerifyState(CommandServerVerifyStateEnum verifyState)
        {
            VerifyState = verifyState;
            switch (verifyState)
            {
                case CommandServerVerifyStateEnum.Success:
                case CommandServerVerifyStateEnum.Retry:
                    return true;
                default: DisposeSocket(); return false;
            }
        }
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="verifyState"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetVerifyState(CommandServerSocket socket, CommandServerVerifyStateEnum verifyState)
        {
            socket.SetVerifyState(verifyState);
        }
#if !AOT
        /// <summary>
        /// Format the remote expression deserialization data
        /// 格式化远程表达式反序列化数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal AutoCSer.Net.CommandServer.RemoteExpression.FormatDeserialize GetRemoteExpressionFormatDeserialize()
        {
            if (remoteExpressionFormatDeserialize != null) return remoteExpressionFormatDeserialize;
            return remoteExpressionFormatDeserialize = new AutoCSer.Net.CommandServer.RemoteExpression.FormatDeserialize(Server.RemoteMetadata.notNull());
        }
        /// <summary>
        /// A collection of arguments for create a remote expression
        /// 创建远程表达式参数集合
        /// </summary>
        /// <returns></returns>
        internal object[] GetRemoteExpressionParameters()
        {
            if (createRemoteExpressionParameters.Length != 0) return createRemoteExpressionParameters;
            return createRemoteExpressionParameters = new object[] { GetRemoteExpressionFormatDeserialize() };
        }
        /// <summary>
        /// Has the remote metadata been obtained
        /// 是否已经获取远程元数据
        /// </summary>
        private bool isRemoteMetadata;
        /// <summary>
        /// The client get the remote metadata
        /// 客户端获取远程元数据
        /// </summary>
        private void appendRemoteMetadata()
        {
            if (!isRemoteMetadata)
            {
                isRemoteMetadata = true;
                Server.RemoteMetadata?.Append(this);
            }
        }
#endif
        /// <summary>
        /// Start receiving data
        /// 开始接收数据
        /// </summary>
        internal void Start()
        {
            try
            {
                socket.NoDelay = Server.Config.NoDelay;
                //outputs.Set(new ServerOutputReturnType(new CallbackIdentity(uint.MaxValue, uint.MaxValue), CommandClientReturnType.Unknown));
                controller = Server.Controller;
                Server.ReceiveBufferPool.Get(ref receiveBuffer);
                var method = controller.VerifyMethod;
                receiveAsyncEventArgs.Completed += onReceiveAsyncCallback;
                receiveBuffer.SetBuffer(receiveAsyncEventArgs);
                if (method != null)
                {
                    Method = method;
                    VerifyState = CommandServerVerifyStateEnum.Retry;
                    verifyTimeout = AutoCSer.Threading.SecondTimer.Now.AddTicks(Server.VerifyTimeoutTicks);
                    if (isReceiveVerifyCommand()) return;
                }
                else
                {
                    VerifyState = CommandServerVerifyStateEnum.Success;
                    if (isReceiveCommand()) return;
                }
            }
            catch (Exception exception)
            {
                if (receiveErrorType == ServerReceiveErrorTypeEnum.Success) receiveErrorType = ServerReceiveErrorTypeEnum.Exception;
                Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            close();
        }
        /// <summary>
        /// Close the socket
        /// </summary>
        internal void DisposeSocket()
        {
            isCloseSocket = true;
            Socket socket = this.socket;
            if (!object.ReferenceEquals(socket, CommandServerConfigBase.NullSocket))
            {
                try
                {
                    if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally { socket.Dispose(); }
            }
        }
        /// <summary>
        /// Close the socket
        /// </summary>
        private void close()
        {
            if (Interlocked.CompareExchange(ref closeLock, 1, 0) == 0)
            {
                isCloseSocket = true;
                try
                {
                    Server.OnClose(this);

                    SocketAsyncEventArgs receiveAsyncEventArgs = this.receiveAsyncEventArgs;
                    if (object.ReferenceEquals(receiveAsyncEventArgs, CommandServerConfigBase.NullSocketAsyncEventArgs))
                    {
                        try
                        {
                            if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                        }
                        finally { socket.Dispose(); }
                    }
                    else
                    {
                        this.receiveAsyncEventArgs = CommandServerConfigBase.NullSocketAsyncEventArgs;
                        receiveAsyncEventArgs.Completed -= onReceiveAsyncCallback;
                        try
                        {
                            if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                        }
                        finally
                        {
                            try
                            {
                                socket.Dispose();
                            }
                            finally
                            {
                                if (!IsShortLink) Server.SocketAsyncEventArgsPool.Push(receiveAsyncEventArgs);
                                else receiveAsyncEventArgs.Dispose();
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally
                {
                    receiveBuffer.Free();
                    receiveBigBuffer.Free();
                    //FreeReceiveDeserializer();
                    //Interlocked.Exchange(ref closeLock, 0);
                    if (receiveDeserializer != null)
                    {
                        receiveDeserializer.FreeContext();
                        receiveDeserializer = null;
                    }
                    commandDataLock.Enter();
                    UnmanagedPool.CachePage.Free(ref commandData);
                    commandDataLock.Exit();

                    if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0) closeSend();

                    Monitor.Enter(keepCallbackLock);
                    var keepCallbacks = this.keepCallbacks;
                    this.keepCallbacks = null;
                    Monitor.Exit(keepCallbackLock);
                    if (keepCallbacks != null)
                    {
                        foreach (CommandServerKeepCallback keepCallback in keepCallbacks.Values)
                        {
                            try
                            {
                                keepCallback.SetCancelKeep();
                            }
                            catch (Exception exception)
                            {
                                Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                            }
                        }
                    }
                    if (shortLinkKeepCallback != null)
                    {
                        try
                        {
                            shortLinkKeepCallback.SetCancelKeep();
                        }
                        catch (Exception exception)
                        {
                            Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                        }
                    }

                    if (onClosedHashSet != null)
                    {
                        foreach (HashObject<Action> onClosed in onClosedHashSet)
                        {
                            try
                            {
                                onClosed.Value();
                            }
                            catch (Exception exception)
                            {
                                Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                            }
                        }
                        onClosedHashSet = null;
                    }
                }
            }
        }
        /// <summary>
        /// Close the short connection
        /// 关闭短连接
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CloseShortLink()
        {
            if (IsShortLink) close();
        }
        ///// <summary>
        ///// 关闭短连接
        ///// </summary>
        ///// <param name="socket"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void CloseShortLink(CommandServerSocket socket)
        //{
        //    socket.CloseShortLink();
        //}
        /// <summary>
        /// Remove the close callback delegate
        /// 移除关闭回调委托
        /// </summary>
        /// <param name="onClosed"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveOnClosed(Action onClosed)
        {
            onClosedHashSet?.Remove(onClosed);
        }
        /// <summary>
        /// Set the socket close event
        /// 设置套接字关闭事件
        /// </summary>
        /// <param name="onClosed"></param>
        /// <returns></returns>
        public bool SetOnClosed(Action onClosed)
        {
            if (closeLock == 0)
            {
                if (onClosedHashSet == null) onClosedHashSet = HashSetCreator.CreateHashObject<Action>();
                onClosedHashSet.Add(onClosed);
                return closeLock == 0;
            }
            return false;
        }
        /// <summary>
        /// Get the current command method sequence number
        /// 获取当前命令方法序号
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetCommandMethodIndex(CommandServerSocket socket)
        {
            return socket.CommandMethodIndex;
        }
        /// <summary>
        /// When the received data is insufficient, check the length of the received data for two consecutive times
        /// 接收数据不足时检查连续两次接收数据长度
        /// </summary>
        /// <param name="receiveSize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool checkReceiveSize(int receiveSize)
        {
            if ((uint)lastReceiveSize + (uint)receiveSize >= Server.MinSocketSize)
            {
                lastReceiveSize = receiveSize;
                return true;
            }
            receiveErrorType = ServerReceiveErrorTypeEnum.ReceiceSizeLess;
            return false;
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="data">Data</param>
        /// <param name="value">Target object</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>Return false on failure</returns>
        private unsafe bool deserialize<T>(ref SubArray<byte> data, ref T value, bool isSimpleSerialize)
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
            receiveDeserializer.SetContext(this, Server.BinaryDeserializeConfig);
            return receiveDeserializer;
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="data">Data</param>
        /// <param name="value">Target object</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>Return false on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Deserialize<T>(CommandServerSocket socket, ref SubArray<byte> data, ref T value, bool isSimpleSerialize)
            where T : struct
        {
            return socket.deserialize(ref data, ref value, isSimpleSerialize);
        }
        /// <summary>
        /// Check whether the current serialization is in the IO synchronization environment
        /// 检查当前序列化是否 IO 同步环境
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool CheckSynchronousIO(BinaryDeserializer deserializer)
        {
            var socket = deserializer.Context as CommandServerSocket;
            return socket == null || socket.onReceiveThreadId == System.Environment.CurrentManagedThreadId;
        }
        /// <summary>
        /// If the current thread is an IO thread receiving data, await forces the Task.Run operation
        /// 如果当前线程为接收数据 IO 线程 await 强制 Task.Run 操作
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SwitchAwaiter GetSwitchAwaiter()
        {
            return onReceiveThreadId == System.Environment.CurrentManagedThreadId ? SwitchAwaiter.Default : SwitchAwaiter.Completed;
        }
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
            onReceiveThreadId = System.Environment.CurrentManagedThreadId;
            try
            {
                switch (receiveType)
                {
                    case ServerReceiveTypeEnum.VerifyCommand:
                        if (AutoCSer.Threading.SecondTimer.Now <= verifyTimeout)
                        {
                            if (isVerifyCommand()) return;
                        }
                        else receiveErrorType = ServerReceiveErrorTypeEnum.VerifyTimeout;
                        break;
                    case ServerReceiveTypeEnum.VerifyData:
                    case ServerReceiveTypeEnum.VerifyDataAgain:
                        if (AutoCSer.Threading.SecondTimer.Now <= verifyTimeout)
                        {
                            if (isVerifyData()) return;
                        }
                        else receiveErrorType = ServerReceiveErrorTypeEnum.VerifyTimeout;
                        break;
                    case ServerReceiveTypeEnum.Command:
                    case ServerReceiveTypeEnum.CommandAgain:
                        if (isCommand()) return;
                        break;
                    case ServerReceiveTypeEnum.Data:
                        if (isData()) return;
                        break;
                    case ServerReceiveTypeEnum.BigData:
                        if (isBigData()) return;
                        break;
                    case ServerReceiveTypeEnum.ShortLinkClose:
                        if (receiveAsyncEventArgs.SocketError != SocketError.Success) receiveSocketError = receiveAsyncEventArgs.SocketError;
                        else receiveErrorType = receiveAsyncEventArgs.BytesTransferred == 0 ? ServerReceiveErrorTypeEnum.ReceiceSizeLess : ServerReceiveErrorTypeEnum.ShortLinkDataSizeError;
                        break;
                }
            }
            catch (Exception exception)
            {
                if (!isCloseSocket)
                {
                    if (receiveErrorType == ServerReceiveErrorTypeEnum.Success) receiveErrorType = ServerReceiveErrorTypeEnum.Exception;
                    Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
            }
            close();
        }
        /// <summary>
        /// Receive the verification command
        /// 接收验证命令
        /// </summary>
        /// <returns></returns>
        private bool isReceiveVerifyCommand()
        {
            receiveType = ServerReceiveTypeEnum.VerifyCommand;
            receiveAsyncEventArgs.SetBuffer(receiveBuffer.StartIndex, receiveBuffer.Buffer.notNull().BufferSize);
            return socket.ReceiveAsync(receiveAsyncEventArgs) || isVerifyCommand();
        }
        /// <summary>
        /// Receive the verification command
        /// 接收验证命令
        /// </summary>
        /// <returns></returns>
        private unsafe bool isVerifyCommand()
        {
            if (VerifyState == CommandServerVerifyStateEnum.Success)
            {
                receiveBuffer.CurrentIndex = receiveIndex = 0;
                return isCommand();
            }
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                receiveBuffer.CurrentIndex = receiveAsyncEventArgs.BytesTransferred;
                if (receiveBuffer.CurrentIndex >= (sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int) * 2))
                {
                    fixed (byte* receiveDataFixed = receiveBuffer.GetFixedBuffer())
                    {
                        commandMethodIndex = *(uint*)(receiveDataStart = receiveDataFixed + receiveBuffer.StartIndex);
                        if (CommandMethodIndex == controller.VerifyMethodIndex)
                        {
                            commandMethodIndex -= CommandListener.MethodStartIndex;
                            if ((transferDataSize = *(int*)(receiveDataStart + (sizeof(uint) + sizeof(CallbackIdentity)))) > 0)
                            {
                                CallbackIdentity = *(CallbackIdentity*)(receiveDataStart + sizeof(int));
                                dataSize = transferDataSize;
                                receiveIndex = sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int);
                                return checkVerifyCommand();
                            }
                            if ((dataSize = *(int*)(receiveDataStart + (sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int)))) > 0 && transferDataSize != 0)
                            {
                                transferDataSize = -transferDataSize;
                                CallbackIdentity = *(CallbackIdentity*)(receiveDataStart + sizeof(int));
                                receiveIndex = sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int) * 2;
                                return checkVerifyCommand();
                            }
                            //if (transferDataSize < 0
                            //    && (dataSize = *(int*)(receiveDataStart + (sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int)))) > (transferDataSize = -transferDataSize))
                            //{
                            //    CallbackIdentity = *(CallbackIdentity*)(receiveDataStart + sizeof(int));
                            //    receiveIndex = sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int) * 2;
                            //    return checkVerifyCommand();
                            //}
                            receiveErrorType = ServerReceiveErrorTypeEnum.VerifyDataSizeError;
                        }
                        else
                        {
                            receiveErrorType = ServerReceiveErrorTypeEnum.VerifyCommandIdentityError;
                            Server.Config.GetAnyLevelLog(LogLevelEnum.Debug)?.InfoIgnoreException("TCP 验证函数命令错误 " + socket.RemoteEndPoint?.ToString(), LogLevelEnum.Info | LogLevelEnum.AutoCSer);
                        }
                    }
                }
                else receiveErrorType = ServerReceiveErrorTypeEnum.VerifyCommandSizeLess;
            }
            else receiveSocketError = receiveAsyncEventArgs.SocketError;
            return false;
        }
        /// <summary>
        /// Check and verify the command
        /// 检查验证命令
        /// </summary>
        /// <returns></returns>
        private bool checkVerifyCommand()
        {
            int maxVerifyDataSize = Math.Min(Server.Config.MaxVerifyDataSize, receiveBuffer.Buffer.notNull().BufferSize - receiveIndex);
            if (dataSize <= maxVerifyDataSize) return verifyData();
            receiveErrorType = ServerReceiveErrorTypeEnum.VerifyDataSizeLimitError;
            return false;
        }
        /// <summary>
        /// Get verification data
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool isReceiveVerifyData()
        {
            receiveBuffer.SetCurrent(receiveAsyncEventArgs);
            return socket.ReceiveAsync(receiveAsyncEventArgs) || isVerifyData();
        }
        /// <summary>
        /// Get verification data
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        private bool isVerifyData()
        {
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
                if (count > 0)
                {
                    receiveBuffer.CurrentIndex += count;
                    return verifyData();
                }
                if (count == 0 && receiveType != ServerReceiveTypeEnum.VerifyDataAgain)
                {
                    receiveType = ServerReceiveTypeEnum.VerifyDataAgain;
                    return isReceiveVerifyData();
                }
                receiveErrorType = ServerReceiveErrorTypeEnum.VerifyDataSizeLess;
            }
            else receiveSocketError = receiveAsyncEventArgs.SocketError;
            return false;
        }
        /// <summary>
        /// Check and verify the length of the data
        /// 检查验证数据长度
        /// </summary>
        /// <returns></returns>
        private bool verifyData()
        {
            int nextSize = transferDataSize - (receiveBuffer.CurrentIndex - receiveIndex);
            if (nextSize == 0) return doVerifyCommand();
            if (nextSize > 0)
            {
                receiveType = ServerReceiveTypeEnum.VerifyData;
                return isReceiveVerifyData();
            }
            receiveErrorType = ServerReceiveErrorTypeEnum.VerifyDataSizeOutOfRange;
            return false;
        }
        /// <summary>
        /// Execute validation method
        /// 执行验证方法
        /// </summary>
        /// <returns></returns>
        private bool doVerifyCommand()
        {
            if (transferDataSize == dataSize)
            {
                SubArray<byte> data = receiveBuffer.GetSubArray(receiveIndex, transferDataSize);
                controllerDoCommand(ref data);
            }
            else
            {
                ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
                try
                {
                    SubArray<byte> data = buffer.GetSubArray(dataSize);
                    if (Server.Config.TransferDecode(this, receiveBuffer.GetSubArray(receiveIndex, transferDataSize), ref data))
                    {
                        controllerDoCommand(ref data);
                    }
                    else
                    {
                        receiveErrorType = ServerReceiveErrorTypeEnum.VerifyDataDecodeError;
                        return false;
                    }
                }
                finally { buffer.Free(); }
            }
            if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
            switch (VerifyState)
            {
                case CommandServerVerifyStateEnum.Success: return isReceiveCommand();
                case CommandServerVerifyStateEnum.Retry: return isReceiveVerifyCommand();
                default:
                    receiveErrorType = ServerReceiveErrorTypeEnum.VerifyError;
                    return false;
            }
            //if (verifyMethodErrorCount != 0)
            //{
            //    --verifyMethodErrorCount;
            //    if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
            //    return isReceiveVerifyCommand();
            //}
            //receiveErrorType = ServerReceiveErrorTypeEnum.VerifyError;
            ////return false;
        }
        /// <summary>
        /// Wait for the short-connection client to close
        /// 等待短连接客户端关闭
        /// </summary>
        /// <returns></returns>
        private bool isReceiveShortLinkClose()
        {
            if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
            receiveType = ServerReceiveTypeEnum.ShortLinkClose;
            receiveBuffer.CurrentIndex = receiveIndex = 0;
            receiveAsyncEventArgs.SetBuffer(receiveBuffer.StartIndex, 1);
            if (closeLock == 0)
            {
                if (socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError != SocketError.Success) receiveSocketError = receiveAsyncEventArgs.SocketError;
                else receiveErrorType = receiveAsyncEventArgs.BytesTransferred == 0 ? ServerReceiveErrorTypeEnum.ReceiceSizeLess : ServerReceiveErrorTypeEnum.ShortLinkDataSizeError;
            }
            return false;
        }
        /// <summary>
        /// Get the command
        /// </summary>
        /// <returns></returns>
        private bool isReceiveCommand()
        {
            receiveType = ServerReceiveTypeEnum.Command;
            receiveBuffer.CurrentIndex = receiveIndex = 0;
            receiveAsyncEventArgs.SetBuffer(receiveBuffer.StartIndex, receiveBuffer.Buffer.notNull().BufferSize);
            return socket.ReceiveAsync(receiveAsyncEventArgs) || isCommand();
        }
        /// <summary>
        /// Get the command
        /// </summary>
        /// <returns></returns>
        private unsafe bool isCommand()
        {
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                receiveBuffer.CurrentIndex += receiveAsyncEventArgs.BytesTransferred;
                fixed (byte* receiveDataFixed = receiveBuffer.GetFixedBuffer())
                {
                    receiveDataStart = receiveDataFixed + receiveBuffer.StartIndex;
                    return loop(true);
                }
            }
            receiveSocketError = receiveAsyncEventArgs.SocketError;
            return false;
        }
        /// <summary>
        /// Determine whether the command is valid
        /// 判断命令是否有效
        /// </summary>
        /// <returns></returns>
        private unsafe bool isCommandMap()
        {
            int commandMapIndex = controller.GetCommandMapIndex(CommandMethodIndex);
            return (uint)commandMapIndex <= (uint)commandData.CurrentIndex && commandData.GetBit(commandMapIndex) != 0 && commandData.Data != null;
        }
        /// <summary>
        /// Loop processing command
        /// 循环处理命令
        /// </summary>
        /// <param name="isCommand"></param>
        /// <returns></returns>
        private unsafe bool loop(bool isCommand)
        {
        START:
            int receiveSize = receiveBuffer.CurrentIndex - receiveIndex;
            if (receiveSize >= sizeof(uint))
            {
                byte* start = receiveDataStart + receiveIndex;
                commandMethodIndex = *(uint*)start;
                controller = Server.GetCommandController(ref commandMethodIndex);
                if (object.ReferenceEquals(controller, CommandListener.Null.Controller))
                {
                    if (!IsShortLink)
                    {
                        switch (CommandMethodIndex - CommandListener.MinMethodIndex)
                        {
                            case CommandListener.MergeMethodIndex - CommandListener.MinMethodIndex:
                                if (receiveSize >= sizeof(uint) + sizeof(int) * 2)
                                {
                                    if ((transferDataSize = *(int*)(start + sizeof(uint))) > 0)
                                    {
                                        if (transferDataSize <= Server.MaxMergeInputSize)
                                        {
                                            receiveIndex += sizeof(int) + sizeof(uint);
                                            dataSize = transferDataSize;
                                            break;
                                        }
                                        receiveErrorType = ServerReceiveErrorTypeEnum.MergeDataSizeLimitError;
                                        return false;
                                    }
                                    if ((dataSize = *(int*)(start + (sizeof(int) + sizeof(uint)))) > 0 && transferDataSize != 0)
                                    {
                                        if (dataSize <= Server.MaxMergeInputSize)
                                        {
                                            transferDataSize = -transferDataSize;
                                            receiveIndex += sizeof(int) * 2 + sizeof(uint);
                                            break;
                                        }
                                        receiveErrorType = ServerReceiveErrorTypeEnum.MergeDataSizeLimitError;
                                    }
                                    else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                    //if ((dataSize = *(int*)(start + (sizeof(int) + sizeof(uint)))) > (transferDataSize = -transferDataSize) && transferDataSize != 0)
                                    //{
                                    //    if (dataSize <= Server.MaxMergeInputSize)
                                    //    {
                                    //        receiveIndex += sizeof(int) * 2 + sizeof(uint);
                                    //        break;
                                    //    }
                                    //    receiveErrorType = ServerReceiveErrorTypeEnum.MergeDataSizeLimitError;
                                    //}
                                    //else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                    return false;
                                }
                                goto COPY;
                            case CommandListener.CustomDataMethodIndex - CommandListener.MinMethodIndex:
                                if (receiveSize >= sizeof(uint) + sizeof(int) * 2)
                                {
                                    if ((transferDataSize = *(int*)(start + sizeof(uint))) > 0)
                                    {
                                        if (transferDataSize <= Server.MaxInputSize)
                                        {
                                            if ((customDataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) >= 0 && (uint)(transferDataSize - customDataSize) < 8)
                                            {
                                                dataSize = transferDataSize;
                                                receiveIndex += sizeof(uint) + sizeof(int) * 2;
                                                break;
                                            }
                                            receiveErrorType = ServerReceiveErrorTypeEnum.CustomDataSizeError;
                                        }
                                        else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                        return false;
                                    }
                                    if (receiveSize >= sizeof(uint) + sizeof(int) * 3)
                                    {
                                        if ((dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) > 0 || transferDataSize != 0)
                                        {
                                            if (dataSize <= Server.MaxInputSize)
                                            {
                                                if ((customDataSize = *(int*)(start + (sizeof(uint) + sizeof(int) * 2))) >= 0 && (uint)(dataSize - customDataSize) < 8)
                                                {
                                                    transferDataSize = -transferDataSize;
                                                    receiveIndex += sizeof(uint) + sizeof(int) * 3;
                                                    break;
                                                }
                                                receiveErrorType = ServerReceiveErrorTypeEnum.CustomDataSizeError;
                                            }
                                            else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                        }
                                        else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                        //if ((dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) > (transferDataSize = -transferDataSize) || transferDataSize != 0)
                                        //{
                                        //    if (dataSize <= Server.MaxInputSize)
                                        //    {
                                        //        if ((customDataSize = *(int*)(start + (sizeof(uint) + sizeof(int) * 2))) >= 0 && (uint)(dataSize - customDataSize) < 8)
                                        //        {
                                        //            receiveIndex += sizeof(uint) + sizeof(int) * 3;
                                        //            break;
                                        //        }
                                        //        receiveErrorType = ServerReceiveErrorTypeEnum.CustomDataSizeError;
                                        //    }
                                        //    else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                        //}
                                        //else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                        return false;
                                    }
                                }
                                goto COPY;
                            case CommandListener.ControllerMethodIndex - CommandListener.MinMethodIndex:
                                Server.QueryController(this);
                                receiveIndex += sizeof(uint);
                                isCommand = false;
                                goto START;
#if !AOT
                            case CommandListener.RemoteMetadataMethodIndex - CommandListener.MinMethodIndex:
                                appendRemoteMetadata();
                                receiveIndex += sizeof(uint);
                                isCommand = false;
                                goto START;
#endif
                            case CommandListener.CheckMethodIndex - CommandListener.MinMethodIndex:
                                receiveIndex += sizeof(uint);
                                isCommand = false;
                                goto START;
                            case CommandListener.CancelKeepMethodIndex - CommandListener.MinMethodIndex:
                                if (receiveSize >= sizeof(uint) + sizeof(CallbackIdentity))
                                {
                                    ClientCancelKeepCallback(*(CallbackIdentity*)(start + sizeof(int)));
                                    receiveIndex += sizeof(uint) + sizeof(CallbackIdentity);
                                    isCommand = false;
                                    goto START;
                                }
                                goto COPY;
                            default:
                                receiveErrorType = ServerReceiveErrorTypeEnum.CommandError;
                                return false;
                        }
                        if (transferDataSize <= receiveBuffer.CurrentIndex - receiveIndex)
                        {
                            if (doBaseCommand())
                            {
                                isCommand = false;
                                goto START;
                            }
                            return false;
                        }
                        bool isDoCommand = false;
                        if (receiveData(ref isDoCommand))
                        {
                            if (isDoCommand)
                            {
                                isCommand = false;
                                goto START;
                            }
                            return true;
                        }
                    }
                    else
                    {
                        receiveErrorType = ServerReceiveErrorTypeEnum.ShortLinkCommandError;
                        return false;
                    }
                }
                else
                {
                    var method = controller.GetMethod(CommandMethodIndex);
                    if (method != null && (commandData.Data == null || isCommandMap()))
                    {
                        Method = method;
                        int headerSize = sizeof(int);
                        if ((commandMethodIndex & (uint)CommandFlagsEnum.Callback) != 0) headerSize += sizeof(CallbackIdentity);
                        if ((commandMethodIndex & (uint)CommandFlagsEnum.SendData) != 0) headerSize += sizeof(int);
                        if (receiveSize >= headerSize)
                        {
                            if ((commandMethodIndex & (uint)CommandFlagsEnum.Callback) == 0) CallbackIdentity.SetNull();
                            else CallbackIdentity = *(CallbackIdentity*)(start + sizeof(uint));
                            if ((commandMethodIndex & (uint)CommandFlagsEnum.SendData) != 0)
                            {
                                if ((transferDataSize = *(int*)(start + (headerSize - sizeof(int)))) > 0)
                                {
                                    if (transferDataSize <= Server.MaxInputSize)
                                    {
                                        dataSize = transferDataSize;
                                        receiveIndex += headerSize;
                                        goto CHECKDATA;
                                    }
                                    receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                    return false;
                                }
                                if (receiveSize >= headerSize + sizeof(int))
                                {
                                    if ((dataSize = *(int*)(start + headerSize)) > 0 && transferDataSize != 0)
                                    {
                                        if (dataSize <= Server.MaxInputSize)
                                        {
                                            transferDataSize = -transferDataSize;
                                            receiveIndex += headerSize + sizeof(uint);
                                            goto CHECKDATA;
                                        }
                                        receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                    }
                                    else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                    //if ((dataSize = *(int*)(start + headerSize)) > (transferDataSize = -transferDataSize) && transferDataSize != 0)
                                    //{
                                    //    if (dataSize <= Server.MaxInputSize)
                                    //    {
                                    //        receiveIndex += headerSize + sizeof(uint);
                                    //        goto CHECKDATA;
                                    //    }
                                    //    receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                    //}
                                    //else receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                    return false;
                                }
                                goto COPY;
                            }
                            else
                            {
                                SubArray<byte> data = new SubArray<byte>(0, 0, EmptyArray<byte>.Array);
                                receiveIndex += headerSize;
                                if (!IsShortLink)
                                {
                                    controllerDoCommand(ref data);
                                    isCommand = false;
                                    goto START;
                                }
                                if (receiveIndex == receiveBuffer.CurrentIndex)
                                {
                                    controllerDoCommand(ref data);
                                    return isReceiveShortLinkClose();
                                }
                                receiveErrorType = ServerReceiveErrorTypeEnum.ShortLinkDataSizeError;
                                return false;
                            }
                        CHECKDATA:
                            if (transferDataSize <= receiveBuffer.CurrentIndex - receiveIndex)
                            {
                                if (!IsShortLink)
                                {
                                    if (doControllerCommand())
                                    {
                                        isCommand = false;
                                        goto START;
                                    }
                                    return false;
                                }
                                if (transferDataSize == receiveBuffer.CurrentIndex - receiveIndex) return doControllerCommand() && isReceiveShortLinkClose();
                                receiveErrorType = ServerReceiveErrorTypeEnum.ShortLinkDataSizeError;
                                return false;
                            }
                            bool isDoCommand = false;
                            if (receiveData(ref isDoCommand))
                            {
                                if (isDoCommand)
                                {
                                    isCommand = false;
                                    goto START;
                                }
                                return true;
                            }
                        }
                        else goto COPY;
                    }
                    else receiveErrorType = ServerReceiveErrorTypeEnum.CommandError;
                }
                return false;
            }
            if (receiveSize == 0)
            {
                if (!isCommand)
                {
                    receiveBuffer.TryRemoveGet();
                    receiveIndex = 0;
                    goto RECEIVE;
                }
                receiveErrorType = ServerReceiveErrorTypeEnum.CommandSizeLess;
                return false;
            }
        COPY:
            if (!isCommand)
            {
                AutoCSer.Common.CopyTo(receiveDataStart + receiveIndex, receiveDataStart, receiveBuffer.CurrentIndex = receiveSize);
                receiveIndex = 0;
            }
        RECEIVE:
            if (receiveType != ServerReceiveTypeEnum.CommandAgain || !isCommand)
            {
                if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
                receiveType = isCommand ? ServerReceiveTypeEnum.CommandAgain : ServerReceiveTypeEnum.Command;
                receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                if (socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    receiveBuffer.CurrentIndex += receiveAsyncEventArgs.BytesTransferred;
                    isCommand = true;
                    goto START;
                }
                receiveSocketError = receiveAsyncEventArgs.SocketError;
            }
            else receiveErrorType = ServerReceiveErrorTypeEnum.CommandSizeLess;
            return false;
        }
        /// <summary>
        /// Check the command data
        /// 检查命令数据
        /// </summary>
        /// <param name="isDoCommand">Is  command was executed
        /// 是否执行了命令</param>
        /// <returns></returns>
        private unsafe bool receiveData(ref bool isDoCommand)
        {
            if (transferDataSize <= receiveBuffer.Buffer.notNull().BufferSize)
            {
                if (receiveIndex + transferDataSize > receiveBuffer.Buffer.notNull().BufferSize)
                {
                    AutoCSer.Common.CopyTo(receiveDataStart + receiveIndex, receiveDataStart, receiveBuffer.CurrentIndex -= receiveIndex);
                    receiveIndex = 0;
                }
                receiveType = ServerReceiveTypeEnum.Data;
                do
                {
                    if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
                    receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        int receiveSize = receiveAsyncEventArgs.BytesTransferred;
                        if (transferDataSize <= (receiveBuffer.CurrentIndex += receiveSize) - receiveIndex)
                        {
                            if (!IsShortLink) return isDoCommand = doCommand();
                            if (transferDataSize == receiveBuffer.CurrentIndex - receiveIndex) return doCommand() && isReceiveShortLinkClose();
                            receiveErrorType = ServerReceiveErrorTypeEnum.ShortLinkDataSizeError;
                            return false;
                        }
                        if (!checkReceiveSize(receiveSize)) return false;
                    }
                    else
                    {
                        receiveSocketError = receiveAsyncEventArgs.SocketError;
                        return false;
                    }
                }
                while (true);
            }
            receiveBigBuffer.ReSize(transferDataSize, receiveBuffer.CurrentIndex - receiveIndex);
            receiveType = ServerReceiveTypeEnum.BigData;
            do
            {
                if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
                receiveAsyncEventArgs.SetBuffer(receiveBigBuffer.Buffer.notNull().Buffer, receiveBigBuffer.BufferCurrentIndex, transferDataSize - receiveBigBuffer.CurrentIndex);
                if (socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int receiveSize = receiveAsyncEventArgs.BytesTransferred;
                    if (transferDataSize == (receiveBigBuffer.CurrentIndex += receiveSize))
                    {
                        if (!IsShortLink) return isDoCommand = doCommandBig();
                        return doCommandBig() && isReceiveShortLinkClose();
                    }
                    if (!checkReceiveSize(receiveSize)) return false;
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
        /// Get Data
        /// </summary>
        /// <returns></returns>
        private unsafe bool isData()
        {
            do
            {
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int receiveSize = receiveAsyncEventArgs.BytesTransferred;
                    if (transferDataSize <= (receiveBuffer.CurrentIndex += receiveSize) - receiveIndex)
                    {
                        fixed (byte* receiveDataFixed = receiveBuffer.GetFixedBuffer())
                        {
                            receiveDataStart = receiveDataFixed + receiveBuffer.StartIndex;
                            if (!IsShortLink) return doCommand() && loop(false);
                            if (transferDataSize == receiveBuffer.CurrentIndex - receiveIndex) return doCommand() && isReceiveShortLinkClose();
                            receiveErrorType = ServerReceiveErrorTypeEnum.ShortLinkDataSizeError;
                            return false;
                        }
                    }
                    if (checkReceiveSize(receiveSize))
                    {
                        receiveBuffer.SetCurrent(receiveAsyncEventArgs);
                        if (socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                    }
                    else return false;
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
        /// Get the temporary buffer data
        /// 获取临时缓冲区数据
        /// </summary>
        /// <returns></returns>
        private bool isBigData()
        {
            do
            {
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int receiveSize = receiveAsyncEventArgs.BytesTransferred, nextSize = transferDataSize - (receiveBigBuffer.CurrentIndex += receiveSize);
                    if (nextSize == 0)
                    {
                        if (doCommandBig())
                        {
                            if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) sendLink();
                            if (!IsShortLink)
                            {
                                receiveType = ServerReceiveTypeEnum.Command;
                                return socket.ReceiveAsync(receiveAsyncEventArgs) || isCommand();
                            }
                            return isReceiveShortLinkClose();
                        }
                        return false;
                    }
                    if (checkReceiveSize(receiveSize))
                    {
                        receiveAsyncEventArgs.SetBuffer(receiveBigBuffer.Buffer.notNull().Buffer, receiveBigBuffer.BufferCurrentIndex, nextSize);
                        if (socket.ReceiveAsync(receiveAsyncEventArgs)) return true;
                    }
                    else return false;
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
        /// Execute the command
        /// </summary>
        /// <returns></returns>
        private bool doCommandBig()
        {
            System.Buffer.BlockCopy(receiveBuffer.Buffer.notNull().Buffer, receiveBuffer.StartIndex + receiveIndex, receiveBigBuffer.Buffer.notNull().Buffer, receiveBigBuffer.StartIndex, receiveBuffer.CurrentIndex - receiveIndex);
            if (transferDataSize == dataSize)
            {
                SubArray<byte> data = receiveBigBuffer.GetSubArray(transferDataSize);
                if (doCommand(ref data))
                {
                    receiveBigBuffer.Free();
                    receiveBuffer.CurrentIndex = receiveIndex = 0;
                    lastReceiveSize = ushort.MaxValue;
                    receiveBuffer.SetBuffer(receiveAsyncEventArgs);
                    return true;
                }
            }
            else
            {
                ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
                try
                {
                    SubArray<byte> data = buffer.GetSubArray(dataSize);
                    if (Server.Config.TransferDecode(this, receiveBigBuffer.GetSubArray(transferDataSize), ref data))
                    {
                        if (doCommand(ref data))
                        {
                            receiveBigBuffer.Free();
                            receiveBuffer.CurrentIndex = receiveIndex = 0;
                            lastReceiveSize = ushort.MaxValue;
                            receiveBuffer.SetBuffer(receiveAsyncEventArgs);
                            return true;
                        }
                        return false;
                    }
                }
                finally { buffer.Free(); }
                receiveErrorType = ServerReceiveErrorTypeEnum.BigDataDecodeError;
            }
            return false;
        }
        /// <summary>
        /// Execute the basic commands of the system
        /// 执行系统基础命令
        /// </summary>
        /// <returns></returns>
        private bool doBaseCommand()
        {
            if (transferDataSize == dataSize)
            {
                SubArray<byte> data = receiveBuffer.GetSubArray(receiveIndex, transferDataSize);
                if (doBaseCommand(ref data))
                {
                    receiveIndex += transferDataSize;
                    lastReceiveSize = ushort.MaxValue;
                    return true;
                }
            }
            else
            {
                ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
                try
                {
                    SubArray<byte> data = buffer.GetSubArray(dataSize);
                    if (Server.Config.TransferDecode(this, receiveBuffer.GetSubArray(receiveIndex, transferDataSize), ref data))
                    {
                        if (doBaseCommand(ref data))
                        {
                            receiveIndex += transferDataSize;
                            lastReceiveSize = ushort.MaxValue;
                            return true;
                        }
                    }
                    else receiveErrorType = ServerReceiveErrorTypeEnum.DataDecodeError;
                }
                finally { buffer.Free(); }
            }
            return false;
        }
        /// <summary>
        /// Execute the basic commands of the system
        /// 执行系统基础命令
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool doBaseCommand(ref SubArray<byte> data)
        {
            switch (CommandMethodIndex - CommandListener.MinMethodIndex)
            {
                case CommandListener.MergeMethodIndex - CommandListener.MinMethodIndex: return merge(ref data);
                case CommandListener.CustomDataMethodIndex - CommandListener.MinMethodIndex:
                    data.Length = customDataSize;
                    receiveErrorType = Server.Config.OnCustomData(this, ref data);
                    return receiveErrorType == ServerReceiveErrorTypeEnum.Success;
                default: receiveErrorType = ServerReceiveErrorTypeEnum.CommandError; return false;
            }
        }
        /// <summary>
        /// Execute the service controller command
        /// 执行服务控制器命令
        /// </summary>
        /// <returns></returns>
        private bool doControllerCommand()
        {
            if (transferDataSize == dataSize)
            {
                SubArray<byte> data = receiveBuffer.GetSubArray(receiveIndex, transferDataSize);
                controllerDoCommand(ref data);
                receiveIndex += transferDataSize;
                lastReceiveSize = ushort.MaxValue;
                return true;
            }
            else
            {
                ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
                try
                {
                    SubArray<byte> data = buffer.GetSubArray(dataSize);
                    if (Server.Config.TransferDecode(this, receiveBuffer.GetSubArray(receiveIndex, transferDataSize), ref data))
                    {
                        controllerDoCommand(ref data);
                        receiveIndex += transferDataSize;
                        lastReceiveSize = ushort.MaxValue;
                        return true;
                    }
                    else receiveErrorType = ServerReceiveErrorTypeEnum.DataDecodeError;
                }
                finally { buffer.Free(); }
            }
            return false;
        }
        /// <summary>
        /// Execute the command
        /// </summary>
        /// <returns></returns>
        private bool doCommand()
        {
            if (transferDataSize == dataSize)
            {
                SubArray<byte> data = receiveBuffer.GetSubArray(receiveIndex, transferDataSize);
                if (doCommand(ref data))
                {
                    receiveIndex += transferDataSize;
                    lastReceiveSize = ushort.MaxValue;
                    return true;
                }
            }
            else
            {
                ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(dataSize);
                try
                {
                    SubArray<byte> data = buffer.GetSubArray(dataSize);
                    if (Server.Config.TransferDecode(this, receiveBuffer.GetSubArray(receiveIndex, transferDataSize), ref data))
                    {
                        if (doCommand(ref data))
                        {
                            receiveIndex += transferDataSize;
                            lastReceiveSize = ushort.MaxValue;
                            return true;
                        }
                    }
                    else receiveErrorType = ServerReceiveErrorTypeEnum.DataDecodeError;
                }
                finally { buffer.Free(); }
            }
            return false;
        }
        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool doCommand(ref SubArray<byte> data)
        {
            if (object.ReferenceEquals(controller, CommandListener.Null.Controller)) return doBaseCommand(ref data);
            controllerDoCommand(ref data);
            return true;
        }
        /// <summary>
        /// Execute the service controller command
        /// 执行服务控制器命令
        /// </summary>
        /// <param name="data"></param>
        private void controllerDoCommand(ref SubArray<byte> data)
        {
            if (!IsShortLink)
            {
                if (!Method.IsOfflineCount)
                {
                    var exception = default(Exception);
                    CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Unknown;
                    try
                    {
                        returnType = controller.DoCommand(this, ref data);
                    }
                    catch (Exception catchException)
                    {
                        returnType = CommandClientReturnTypeEnum.ServerException;
                        Server.Config.Log.ExceptionIgnoreException(exception = catchException, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                    }
                    finally
                    {
                        if (returnType != CommandClientReturnTypeEnum.Success)
                        {
                            switch (Method.MethodType)
                            {
                                case ServerMethodTypeEnum.SendOnly:
                                case ServerMethodTypeEnum.SendOnlyQueue:
                                case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                                case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                                case ServerMethodTypeEnum.SendOnlyTask:
                                case ServerMethodTypeEnum.SendOnlyTaskQueue:
                                    break;
                                case ServerMethodTypeEnum.KeepCallback:
                                case ServerMethodTypeEnum.KeepCallbackCount:
                                case ServerMethodTypeEnum.KeepCallbackQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                                case ServerMethodTypeEnum.KeepCallbackTask:
                                case ServerMethodTypeEnum.KeepCallbackCountTask:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                                case ServerMethodTypeEnum.AsyncEnumerableTask:
                                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                    CancelKeepCallback(returnType, exception);
                                    break;
                                case ServerMethodTypeEnum.Task:
                                    SendTask(returnType, exception);
                                    break;
                                default:
                                    Send(returnType, exception);
                                    break;
                            }
                        }
                    }
                }
                else if (Server.IncrementOfflineCount())
                {
                    switch (Method.MethodType)
                    {
                        case ServerMethodTypeEnum.Callback:
                        case ServerMethodTypeEnum.CallbackTask:
                        case ServerMethodTypeEnum.Queue:
                        case ServerMethodTypeEnum.SendOnlyQueue:
                        case ServerMethodTypeEnum.CallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountQueue:
                        case ServerMethodTypeEnum.ConcurrencyReadQueue:
                        case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                        case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                        case ServerMethodTypeEnum.ReadWriteQueue:
                        case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                        case ServerMethodTypeEnum.CallbackReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                        case ServerMethodTypeEnum.Task:
                        case ServerMethodTypeEnum.SendOnlyTask:
                        case ServerMethodTypeEnum.KeepCallbackTask:
                        case ServerMethodTypeEnum.KeepCallbackCountTask:
                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                        case ServerMethodTypeEnum.TaskQueue:
                        case ServerMethodTypeEnum.CallbackTaskQueue:
                        case ServerMethodTypeEnum.SendOnlyTaskQueue:
                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                        case ServerMethodTypeEnum.AsyncEnumerableTask:
                        case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                            OfflineCount = new OfflineCount();
                            doCommandOfflineCount(ref data);
                            OfflineCount = OfflineCount.Null;
                            return;
                        default: doCommandOfflineCount(ref data); return;
                    }
                }
                else Send(CommandClientReturnTypeEnum.ServerOffline);
            }
            else
            {
                var exception = default(Exception);
                CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Unknown;
                switch (Method.MethodType)
                {
                    case ServerMethodTypeEnum.Unknown:
                    case ServerMethodTypeEnum.VersionExpired:
                    case ServerMethodTypeEnum.Synchronous:
                    case ServerMethodTypeEnum.SendOnly:
                        isCloseShortLink = true;
                        break;
                }
                try
                {
                    returnType = controller.DoCommand(this, ref data);
                }
                catch (Exception catchException)
                {
                    returnType = CommandClientReturnTypeEnum.ServerException;
                    Server.Config.Log.ExceptionIgnoreException(exception = catchException, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                }
                finally
                {
                    try
                    {
                        if (returnType != CommandClientReturnTypeEnum.Success)
                        {
                            isCloseShortLink = true;
                            switch (Method.MethodType)
                            {
                                case ServerMethodTypeEnum.Unknown:
                                case ServerMethodTypeEnum.VersionExpired:
                                case ServerMethodTypeEnum.SendOnly:
                                case ServerMethodTypeEnum.SendOnlyQueue:
                                case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                                case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                                case ServerMethodTypeEnum.SendOnlyTask:
                                case ServerMethodTypeEnum.SendOnlyTaskQueue:
                                    break;
                                case ServerMethodTypeEnum.KeepCallback:
                                case ServerMethodTypeEnum.KeepCallbackCount:
                                case ServerMethodTypeEnum.KeepCallbackQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                                case ServerMethodTypeEnum.KeepCallbackTask:
                                case ServerMethodTypeEnum.KeepCallbackCountTask:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                                case ServerMethodTypeEnum.AsyncEnumerableTask:
                                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                    shortLinkKeepCallback?.CancelKeep(returnType, exception);
                                    break;
                                default: Send(CallbackIdentity, returnType, exception); break;
                            }
                        }
                    }
                    finally
                    {
                        if (isCloseShortLink) close();
                    }
                }
            }
        }
        /// <summary>
        /// Offline notification interface command processing
        /// 下线通知接口命令处理
        /// </summary>
        /// <param name="data"></param>
        private void doCommandOfflineCount(ref SubArray<byte> data)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Unknown;
            var exception = default(Exception);
            try
            {
                returnType = controller.DoCommand(this, ref data);
            }
            catch (Exception catchException)
            {
                returnType = CommandClientReturnTypeEnum.ServerException;
                Server.Config.Log.ExceptionIgnoreException(exception = catchException, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
            }
            finally
            {
                if (returnType == CommandClientReturnTypeEnum.Success)
                {
                    if (object.ReferenceEquals(OfflineCount, OfflineCount.Null)) Server.DecrementOfflineCount();
                }
                else
                {
                    if (object.ReferenceEquals(OfflineCount, OfflineCount.Null) || OfflineCount.Get() == 0) Server.DecrementOfflineCount();

                    switch (Method.MethodType)
                    {
                        case ServerMethodTypeEnum.SendOnly:
                        case ServerMethodTypeEnum.SendOnlyQueue:
                        case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                        case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                        case ServerMethodTypeEnum.SendOnlyTask:
                        case ServerMethodTypeEnum.SendOnlyTaskQueue:
                            break;
                        case ServerMethodTypeEnum.KeepCallback:
                        case ServerMethodTypeEnum.KeepCallbackCount:
                        case ServerMethodTypeEnum.KeepCallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountQueue:
                        case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackTask:
                        case ServerMethodTypeEnum.KeepCallbackCountTask:
                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                        case ServerMethodTypeEnum.AsyncEnumerableTask:
                        case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                            CancelKeepCallback(returnType, exception);
                            break;
                        case ServerMethodTypeEnum.Task:
                            SendTask(returnType, exception);
                            break;
                        default:
                            Send(returnType, exception);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Stream merging command processing
        /// 流合并命令处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private unsafe bool merge(ref SubArray<byte> data)
        {
            int receiveSize = data.Length;
            if (receiveSize >= sizeof(uint) * 2)
            {
                fixed (byte* dataFixed = data.GetFixedBuffer())
                {
                    int receiveIndex = data.Start, receiveCount = data.EndIndex;
                    do
                    {
                        byte* start = dataFixed + receiveIndex;
                        commandMethodIndex = *(uint*)start;
                        controller = Server.GetCommandController(ref commandMethodIndex);
                        if (!object.ReferenceEquals(controller, CommandListener.Null.Controller))
                        {
                            var method = controller.GetMethod(CommandMethodIndex);
                            if (method != null && (commandData.Data == null || isCommandMap()))
                            {
                                Method = method;
                                if ((commandMethodIndex & (uint)CommandFlagsEnum.Callback) != 0)
                                {
                                    CallbackIdentity = *(CallbackIdentity*)(start + sizeof(uint));
                                    receiveIndex += sizeof(CallbackIdentity);
                                    start += sizeof(CallbackIdentity);
                                }
                                if ((commandMethodIndex & (uint)CommandFlagsEnum.SendData) != 0)
                                {
                                    if ((dataSize = *(int*)(start + sizeof(uint))) > 0
                                        && (receiveSize = receiveCount - (receiveIndex += (sizeof(uint) + sizeof(int))) - dataSize) >= 0)
                                    {
                                        if (dataSize <= Server.MaxInputSize)
                                        {
                                            data.Set(receiveIndex, dataSize);
                                            controllerDoCommand(ref data);
                                            if (receiveSize == 0) return true;
                                            receiveIndex += dataSize;
                                        }
                                        else
                                        {
                                            receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeLimitError;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        receiveErrorType = ServerReceiveErrorTypeEnum.DataSizeError;
                                        return false;
                                    }
                                }
                                else
                                {
                                    receiveIndex += sizeof(uint);
                                    SubArray<byte> emptyData = new SubArray<byte>(0, 0, EmptyArray<byte>.Array);
                                    if ((receiveSize = receiveCount - receiveIndex) >= 0)
                                    {
                                        controllerDoCommand(ref emptyData);
                                        if (receiveSize == 0) return true;
                                    }
                                    else
                                    {
                                        receiveErrorType = ServerReceiveErrorTypeEnum.CommandSizeLess;
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                receiveErrorType = ServerReceiveErrorTypeEnum.CommandError;
                                return false;
                            }
                        }
                        else if (object.ReferenceEquals(controller, CommandListener.Null.Controller))
                        {
                            switch (CommandMethodIndex - CommandListener.CustomDataMethodIndex)
                            {
                                case CommandListener.CustomDataMethodIndex - CommandListener.CustomDataMethodIndex:
                                    if ((dataSize = *(int*)(start + sizeof(uint))) > 0
                                        && (receiveSize = receiveCount - (receiveIndex += (sizeof(uint) + sizeof(int))) - dataSize) >= 0)
                                    {
                                        int customDataSize = *(int*)(start + (sizeof(uint) + sizeof(int)));
                                        if (customDataSize >= 0 && (uint)(dataSize - customDataSize) < 8)
                                        {
                                            data.Set(receiveIndex + sizeof(int), customDataSize);
                                            receiveErrorType = Server.Config.OnCustomData(this, ref data);
                                            if (receiveErrorType == ServerReceiveErrorTypeEnum.Success)
                                            {
                                                if (receiveSize == 0) return true;
                                                receiveIndex += dataSize;
                                                break;
                                            }
                                        }
                                        else receiveErrorType = ServerReceiveErrorTypeEnum.CustomDataSizeError;
                                    }
                                    else receiveErrorType = ServerReceiveErrorTypeEnum.CustomDataSizeError;
                                    return false;
                                case CommandListener.ControllerMethodIndex - CommandListener.CustomDataMethodIndex:
                                    Server.QueryController(this);
                                    if ((receiveSize -= sizeof(int)) == 0) return true;
                                    receiveIndex += sizeof(int);
                                    break;
#if !AOT
                                case CommandListener.RemoteMetadataMethodIndex - CommandListener.CustomDataMethodIndex:
                                    appendRemoteMetadata();
                                    if ((receiveSize -= sizeof(int)) == 0) return true;
                                    receiveIndex += sizeof(int);
                                    break;
#endif
                                case CommandListener.CheckMethodIndex - CommandListener.CustomDataMethodIndex:
                                    if ((receiveSize -= sizeof(int)) == 0) return true;
                                    receiveIndex += sizeof(int);
                                    break;
                                case CommandListener.CancelKeepMethodIndex - CommandListener.CustomDataMethodIndex:
                                    if ((receiveSize = receiveCount - (receiveIndex += sizeof(uint) + sizeof(CallbackIdentity))) >= 0)
                                    {
                                        ClientCancelKeepCallback(*(CallbackIdentity*)(start + sizeof(int)));
                                        if (receiveSize == 0) return true;
                                        break;
                                    }
                                    receiveErrorType = ServerReceiveErrorTypeEnum.CommandSizeLess;
                                    return false;
                                default: receiveErrorType = ServerReceiveErrorTypeEnum.CommandError; return false;
                            }
                        }
                        else
                        {
                            receiveErrorType = ServerReceiveErrorTypeEnum.CommandError;
                            return false;
                        }
                    }
                    while (receiveSize >= sizeof(uint));
                }
            }
            receiveErrorType = ServerReceiveErrorTypeEnum.MergeDataSizeLess;
            return false;
        }
        /// <summary>
        /// If data transmission fails or is abnormal, the socket needs to be closed
        /// 发送数据失败或者异常需要关闭套接字
        /// </summary>
        private void sendError()
        {
            isCloseSocket = true;
            Socket socket = this.socket;
            if (!object.ReferenceEquals(socket, CommandServerConfigBase.NullSocket))
            {
                try
                {
                    if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally
                {
                    socket.Dispose();
                    closeSend();
                }
            }
            else closeSend();
        }
        /// <summary>
        /// Release the sent data buffer and the asynchronous event object
        /// 释放发送数据缓冲区与异步事件对象
        /// </summary>
        private void closeSend()
        {
            try
            {
                SocketAsyncEventArgs sendAsyncEventArgs = this.sendAsyncEventArgs;
                if (!object.ReferenceEquals(sendAsyncEventArgs, CommandServerConfigBase.NullSocketAsyncEventArgs))
                {
                    this.sendAsyncEventArgs = CommandServerConfigBase.NullSocketAsyncEventArgs;
                    sendAsyncEventArgs.Completed -= onSendAsyncCallback;
                    if (!IsShortLink) Server.SocketAsyncEventArgsPool.Push(sendAsyncEventArgs);
                    else sendAsyncEventArgs.Dispose();
                }
            }
            catch (Exception exception)
            {
                Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            finally
            {
                BinarySerializer outputSerializer = OutputSerializer;
                if (!object.ReferenceEquals(outputSerializer, CommandServerConfigBase.NullBinarySerializer))
                {
                    OutputSerializer = CommandServerConfigBase.NullBinarySerializer;
                    outputSerializer.FreeContext(isSerializeCopyString);
                }

                sendBuffer.Free();
                sendCopyBuffer.Free();
                sendTransferBuffer.Free();

                ServerOutput.CancelLink(outputs.Get());
            }
        }
        /// <summary>
        /// Release the output copy buffer
        /// 释放输出复制缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeSendCopyBuffer()
        {
            sendCopyBuffer.Free();
            sendTransferBuffer.Free();
        }
        /// <summary>
        /// Start sending data
        /// 启动发送数据
        /// </summary>
        private void output()
        {
            if (!IsShortLink)
            {
                switch (buildOutputThreadEnum)
                {
                    case CommandServerSocketBuildOutputThreadEnum.Queue: queueOutput(); return;
                    case CommandServerSocketBuildOutputThreadEnum.Thread:
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(buildOutputHandle);
                        return;
                    default: buildOutput(); return;
                }
            }
            switch (Method.MethodType)
            {
                case ServerMethodTypeEnum.KeepCallback:
                case ServerMethodTypeEnum.KeepCallbackCount:
                case ServerMethodTypeEnum.KeepCallbackQueue:
                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackTask:
                case ServerMethodTypeEnum.KeepCallbackCountTask:
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                case ServerMethodTypeEnum.AsyncEnumerableTask:
                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                    isCloseShortLink = false;
                    switch (buildOutputThreadEnum)
                    {
                        case CommandServerSocketBuildOutputThreadEnum.Queue: queueOutput(); return;
                        case CommandServerSocketBuildOutputThreadEnum.Thread: AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(buildOutputHandle); return;
                        default: buildOutput(); return;
                    }
                default: buildOutput(); return;
            }
        }
        /// <summary>
        /// Start sending data
        /// 启动发送数据
        /// </summary>
        private void queueOutput()
        {
            do
            {
                var headValue = outputSocketHead;
                if (headValue == null)
                {
                    this.nextOutputSocket = null;
                    if (Interlocked.CompareExchange(ref outputSocketHead, this, null) == null)
                    {
                        socketOutputWaitHandle.Set();
                        return;
                    }
                }
                else
                {
                    this.nextOutputSocket = headValue;
                    if (object.ReferenceEquals(Interlocked.CompareExchange(ref outputSocketHead, this, headValue), headValue)) return;
                }
            }
            while (true);
        }
        /// <summary>
        /// Get and clear the next node
        /// 获取并清除下一个节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private CommandServerSocket? getNextOutputSocket()
#else
        private CommandServerSocket getNextOutputSocket()
#endif
        {
            var value = nextOutputSocket;
            nextOutputSocket = null;
            return value;
        }
        /// <summary>
        /// Try to send data
        /// 尝试发送数据
        /// </summary>
        private unsafe void buildOutput()
        {
            var head = default(ServerOutput);
            ServerBuildInfo buildInfo = new ServerBuildInfo { SendBufferSize = Server.SendBufferPool.Size };
            try
            {
                var end = default(ServerOutput);
                UnmanagedStream outputStream;
                if (sendBuffer.Buffer == null) Server.SendBufferPool.Get(ref sendBuffer);
                if (object.ReferenceEquals(OutputSerializer, CommandServerConfigBase.NullBinarySerializer))
                {
                    outputStream = (OutputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetContext(this, ref isSerializeCopyString);
                }
                else outputStream = OutputSerializer.Stream;
                FIXEDBUFFER:
                buildInfo.IsNewBuffer = 0;
                fixed (byte* dataFixed = sendBuffer.GetFixedBuffer())
                {
                    byte* start = dataFixed + sendBuffer.StartIndex;
                STREAM:
                    if (outputStream.Data.Pointer.Byte != start) outputStream.Reset(start, sendBuffer.Buffer.notNull().BufferSize);
                    buildInfo.Clear();
                    outputStream.Data.Pointer.CurrentIndex = ServerOutput.StreamStartIndex;
                    if (buildOutputHead == null)
                    {
                        if((head = outputs.GetQueue(out end)) == null)
                        {
                            AutoCSer.Threading.ThreadYield.YieldOnly();
                            if ((head = outputs.GetQueue(out end)) == null) goto END;
                        }
                    }
                    else
                    {
                        head = buildOutputHead;
                        end = buildOutputEnd;
                        buildOutputEnd = buildOutputHead = null;
                    }
                    do
                    {
                        head = head.Build(this, ref buildInfo);
                        if (buildInfo.IsSend != 0 || VerifyState != CommandServerVerifyStateEnum.Success)
                        {
                            buildOutputHead = head;
                            buildOutputEnd = end;
                            head = null;
                            goto SETSENDDATA;
                        }
                    }
                    while (head != null || (head = outputs.GetQueue(out end)) != null);
                    if (buildInfo.Count == 0) goto END;
                    SETSENDDATA:
                    buildInfo.IsNewBuffer = setSendData(start, buildInfo.Count);
                    switch (send())
                    {
                        case ServerSocketSendStateEnum.Asynchronous:
                            isCloseShortLink = false;
                            buildInfo.IsAsynchronous = true;
                            return;
                        case ServerSocketSendStateEnum.Error: buildInfo.IsError = true; return;
                    }
                    if (buildOutputHead == null && outputs.IsEmpty) goto END;
                    CHECKNEWBUFFER:
                    if (buildInfo.IsNewBuffer == 0) goto STREAM;
                    goto FIXEDBUFFER;
                END:
                    if (closeLock == 0)
                    {
                        Interlocked.Exchange(ref isOutput, 0);
                        if (outputs.IsEmpty)
                        {
                            if (closeLock != 0 && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
                            {
                                buildInfo.IsClose = true;
                            }
                        }
                        else if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
                        {
                            goto CHECKNEWBUFFER;
                        }
                    }
                    else buildInfo.IsClose = true;
                }
            }
            catch (Exception exception)
            {
                if (!IsClose)
                {
                    Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    buildInfo.IsError = true;
                    //sendErrorType = ServerSendErrorType.Exception;
                }
            }
            finally
            {
                if (!isCloseShortLink)
                {
                    if (closeLock == 0)
                    {
                        if (!buildInfo.IsError)
                        {
                            if (IsShortLink && !buildInfo.IsAsynchronous) onSendShortLink();
                        }
                        else
                        {
                            sendError();
                            ServerOutput.CancelLink(head);
                            freeBuildOutput();
                        }
                    }
                    else if (buildInfo.IsClose)
                    {
                        closeSend();
                        ServerOutput.CancelLink(head);
                        freeBuildOutput();
                    }
                }
                else
                {
                    isCloseShortLink = false;
                    close();
                }
            }
        }
        /// <summary>
        /// Set the data to be sent
        /// 设置待发送数据
        /// </summary>
        /// <param name="start">Data starting position
        /// 数据起始位置</param>
        /// <param name="count">Number of output bytes
        /// 输出字节数量</param>
        /// <returns>Whether the output buffer has been changed
        /// 输出缓冲区是否被改变</returns>
        private unsafe byte setSendData(byte* start, int count)
        {
            UnmanagedStream outputStream = OutputSerializer.Stream;
            int streamStartIndex = ServerOutput.StreamStartIndex, outputLength = outputStream.Data.Pointer.CurrentIndex, bufferLength = sendBuffer.Buffer.notNull().BufferSize, dataLength = outputLength - streamStartIndex, transferDataSize = 0;
            byte isNewBuffer = 0;
            if (outputLength <= bufferLength)
            {
                if (outputStream.Data.Pointer.ByteSize != bufferLength)
                {
                    AutoCSer.Common.CopyTo(outputStream.Data.Pointer.Byte + streamStartIndex, start + streamStartIndex, dataLength);
                }
                sendData.Set(sendBuffer.Buffer.notNull().Buffer, sendBuffer.StartIndex + streamStartIndex, dataLength);
            }
            else
            {
                outputStream.Data.Pointer.GetBuffer(ref sendCopyBuffer, streamStartIndex);
                sendData.Set(sendCopyBuffer.Buffer.notNull().Buffer, sendCopyBuffer.StartIndex + streamStartIndex, dataLength);
                if (sendCopyBuffer.Buffer.notNull().BufferSize <= Server.SendBufferMaxSize)
                {
                    sendBuffer.CopyFromFree(ref sendCopyBuffer);
                    isNewBuffer = 1;
                }
            }
            if (count == 1)
            {
                SubArray<byte> oldSendData = sendData;
                dataLength -= streamStartIndex;
                if (Server.Config.TransferEncode(this, sendData.Array, sendData.Start + streamStartIndex, dataLength, ref sendTransferBuffer, ref sendData, sizeof(CallbackIdentity) + sizeof(int) * 2, sizeof(CallbackIdentity) + sizeof(int) * 2))
                {
                    transferDataSize = sendData.Length;
                    sendData.MoveStart(-(sizeof(CallbackIdentity) + sizeof(int) * 2));
                    fixed (byte* dataFixed = sendData.GetFixedBuffer(), oldSendDataFixed = oldSendData.GetFixedBuffer())
                    {
                        byte* dataStart = dataFixed + sendData.Start;
                        *(CallbackIdentity*)dataStart = *(CallbackIdentity*)(oldSendDataFixed + oldSendData.Start);
                        *(int*)(dataStart + sizeof(CallbackIdentity)) = -transferDataSize;
                        *(int*)(dataStart + (sizeof(CallbackIdentity) + sizeof(int))) = dataLength;
                    }
                }
            }
            else
            {
                if (Server.Config.TransferEncode(this, sendData.Array, sendData.Start, dataLength, ref sendTransferBuffer, ref sendData, sizeof(CallbackIdentity) + sizeof(int) * 2, sizeof(int)))
                {
                    transferDataSize = sendData.Length;
                    sendData.MoveStart(-(sizeof(CallbackIdentity) + sizeof(int) * 2));
                    fixed (byte* dataFixed = sendData.GetFixedBuffer())
                    {
                        byte* dataStart = dataFixed + sendData.Start;
                        *(CallbackIdentity*)dataStart = new CallbackIdentity(CommandServer.KeepCallbackCommand.MergeIndex | (uint)CallbackFlagsEnum.SendData);
                        *(int*)(dataStart + sizeof(CallbackIdentity)) = -transferDataSize;
                        *(int*)(dataStart + (sizeof(CallbackIdentity) + sizeof(int))) = dataLength;
                    }
                }
                else
                {
                    sendData.MoveStart(-streamStartIndex);
                    fixed (byte* dataFixed = sendData.GetFixedBuffer())
                    {
                        byte* dataStart = dataFixed + sendData.Start;
                        *(CallbackIdentity*)dataStart = new CallbackIdentity(CommandServer.KeepCallbackCommand.MergeIndex | (uint)CallbackFlagsEnum.SendData);
                        *(int*)(dataStart + sizeof(CallbackIdentity)) = dataLength;
                    }
                }
            }
            return isNewBuffer;
        }
        //private static int currentSocketIdentity;
        //private readonly int socketIdentity = ++currentSocketIdentity;
        /// <summary>
        /// Send data
        /// </summary>
        /// <returns>Send data status
        /// 发送数据状态</returns>
        private ServerSocketSendStateEnum send()
        {
            do
            {
                if (object.ReferenceEquals(socket, CommandServerConfigBase.NullSocket)) return ServerSocketSendStateEnum.Error;
                if (object.ReferenceEquals(sendAsyncEventArgs, CommandServerConfigBase.NullSocketAsyncEventArgs))
                {
                    onSendAsyncCallback = onSend;
                    sendAsyncEventArgs = Server.SocketAsyncEventArgsPool.Get();
                    sendAsyncEventArgs.Completed += onSendAsyncCallback;
                }
                sendAsyncEventArgs.SetBuffer(sendData.Array, sendData.Start, sendData.Length);
                //Console.WriteLine($"[{socketIdentity}] send {sendData.Length}");
                if (socket.SendAsync(sendAsyncEventArgs)) return ServerSocketSendStateEnum.Asynchronous;
                if (sendAsyncEventArgs.SocketError == SocketError.Success)
                {
                    if (sendData.GetMoveStartLength(sendAsyncEventArgs.BytesTransferred) == 0)
                    {
                        lastSendSize = ushort.MaxValue;
                        freeSendCopyBuffer();
                        return ServerSocketSendStateEnum.Synchronize;
                    }
                }
                else
                {
                    sendSocketError = sendAsyncEventArgs.SocketError;
                    return ServerSocketSendStateEnum.Error;
                }
            }
            while (true);
        }
        /// <summary>
        /// The callback delegate after the data is sent
        /// 数据发送完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">Asynchronous callback parameters</param>
#if NetStandard21
        private void onSend(object? sender, SocketAsyncEventArgs async)
#else
        private void onSend(object sender, SocketAsyncEventArgs async)
#endif
        {
            bool isSend = false, isFreeBuildOutput;
            try
            {
                if (sendAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int sendSize = sendAsyncEventArgs.BytesTransferred;
                    if (sendData.GetMoveStartLength(sendSize) == 0)
                    {
                        lastSendSize = ushort.MaxValue;
                        freeSendCopyBuffer();
                        isSend = true;
                    }
                    else
                    {
                        if ((uint)lastSendSize + (uint)sendSize >= Server.MinSocketSize)
                        {
                            lastSendSize = sendSize;
                            switch (send())
                            {
                                case ServerSocketSendStateEnum.Asynchronous: return;
                                case ServerSocketSendStateEnum.Synchronize: isSend = true; break;
                            }
                        }
                        //else sendErrorType = ServerSendErrorType.SendSizeLess;
                    }
                }
                else sendSocketError = sendAsyncEventArgs.SocketError;
            }
            catch (Exception exception)
            {
                //sendErrorType = ServerSendErrorType.Exception;
                Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            isFreeBuildOutput = buildOutputHead == null;
            try
            {
                if (closeLock == 0)
                {
                    if (isSend)
                    {
                        if (buildOutputHead == null && outputs.IsEmpty)
                        {
                            Interlocked.Exchange(ref isOutput, 0);
                            if (outputs.IsEmpty && buildOutputHead == null)
                            {
                                if (closeLock == 0)
                                {
                                    if (IsShortLink) onSendShortLink();
                                }
                                else
                                {
                                    if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0) closeSend();
                                }
                                return;
                            }
                            if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
                            {
                                isFreeBuildOutput = true;
                                buildOutput();
                            }
                        }
                        else
                        {
                            isFreeBuildOutput = true;
                            buildOutput();
                        }
                    }
                    else sendError();
                }
                else closeSend();
            }
            finally
            {
                if (!isFreeBuildOutput) freeBuildOutput();
            }
        }
        /// <summary>
        /// After the short connection sends data, check whether the connection needs to be closed
        /// 短连接发送数据以后检查是否需要关闭连接
        /// </summary>
        private void onSendShortLink()
        {
            switch (Method.MethodType)
            {
                case ServerMethodTypeEnum.KeepCallback:
                case ServerMethodTypeEnum.KeepCallbackCount:
                case ServerMethodTypeEnum.KeepCallbackQueue:
                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackTask:
                case ServerMethodTypeEnum.KeepCallbackCountTask:
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                case ServerMethodTypeEnum.AsyncEnumerableTask:
                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                    if (IsCancelKeepCallback) close();
                    return;
                default: close(); return;
            }
        }
        /// <summary>
        /// Release the unprocessed socket queue
        /// 释放未处理套接字队列
        /// </summary>
        private void freeBuildOutput()
        {
            var head = buildOutputHead;
            buildOutputHead = null;
            ServerOutput.CancelLink(head);
        }
        /// <summary>
        /// Add synchronous output
        /// 添加同步输出
        /// </summary>
        /// <param name="output"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendOutput(ServerOutput output)
        {
            if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) output.LinkNext = outputHead;
            else outputEnd = output;
            isCloseShortLink = false;
            outputHead = output;
        }
        /// <summary>
        /// Send synchronous output
        /// 发送同步输出
        /// </summary>
        private void sendLink()
        {
            if (outputs.IsPushHeadLink(outputHead, outputEnd) && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
            {
                if(buildOutputThreadEnum == CommandServerSocketBuildOutputThreadEnum.Queue) queueOutput();
                else AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(buildOutputHandle);
            }
            outputHead = outputEnd = CommandServerConfig.NullServerOutput;
        }
        /// <summary>
        /// Add output information
        /// 添加输出信息
        /// </summary>
        /// <param name="output"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push(ServerOutput output)
        {
            if (outputs.IsPushHead(output) && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0) this.output();
        }
        /// <summary>
        /// Add output information
        /// 添加输出信息
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push(ServerOutput head, ServerOutput end)
        {
            if (outputs.IsPushHeadLink(head, end) && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0) output();
        }
        /// <summary>
        /// Add output information
        /// 添加输出信息
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckPush(ServerOutput head, ServerOutput end)
        {
            if (closeLock == 0 && outputs.IsPushHeadLink(head, end) && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0) output();
        }
        /// <summary>
        /// Add output information
        /// 添加输出信息
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool TryPush(ServerOutput output)
        {
            if (closeLock == 0)
            {
                Push(output);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
#if NetStandard21
        internal bool Send(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal bool Send(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutputReturnType);
                try
                {
                    Push(output = new ServerOutputReturnType(callbackIdentity, returnType, exception != null && Server.Config.IsOutputExceptionMessage ? exception : null));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SendLog(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
        {
            Send(callbackIdentity, returnType, exception);
            Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        internal bool Send(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType)
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutputReturnType);
                try
                {
                    Push(output = new ServerOutputReturnType(callbackIdentity, returnType));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
#if NetStandard21
        internal bool Send(CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal bool Send(CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutputReturnType);
                try
                {
                    AppendOutput(output = new ServerOutputReturnType(CallbackIdentity, returnType, exception != null && Server.Config.IsOutputExceptionMessage ? exception : null));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SendLog(CommandClientReturnTypeEnum returnType, Exception exception)
        {
            Send(returnType, exception);
            Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void SendTask(CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal void SendTask(CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            Method.SetTaskException();
            Send(returnType, exception);
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Send(CommandClientReturnTypeEnum returnType)
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutputReturnType);
                try
                {
                    AppendOutput(output = new ServerOutputReturnType(CallbackIdentity, returnType));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send the successful status synchronously
        /// 同步发送成功状态
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerSocket socket)
        {
            return socket.Send(CommandClientReturnTypeEnum.Success);
        }
        /// <summary>
        /// Get the output information synchronously
        /// 同步获取输出信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Output parameters</param>
        /// <returns>Output information</returns>
        private ServerOutput<T> getOutput<T>(ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            if (method.IsOutputPool)
            {
                var output = LinkPool<ServerOutput<T>, ServerOutput>.Default.Pop();
                if (output != null)
                {
                    output.Set(CallbackIdentity, method, ref outputParameter);
                    return output;
                }
            }
            return new ServerOutput<T>(CallbackIdentity, method, ref outputParameter);
        }
        /// <summary>
        /// Get the output information
        /// 获取输出信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Output parameters</param>
        /// <returns>Output information</returns>
        internal ServerOutput<T> GetOutput<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            if (method.IsOutputPool)
            {
                var output = LinkPool<ServerOutput<T>, ServerOutput>.Default.Pop();
                if (output != null)
                {
                    output.Set(callbackIdentity, method, ref outputParameter);
                    return output;
                }
            }
            return new ServerOutput<T>(callbackIdentity, method, ref outputParameter);
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        private bool send<T>(ServerInterfaceMethod method, T outputParameter)
            where T : struct
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutput);
                try
                {
                    AppendOutput(output = getOutput(method, ref outputParameter));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        internal bool Send<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T outputParameter)
            where T : struct
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutput);
                try
                {
                    Push(output = GetOutput(callbackIdentity, method, ref outputParameter));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <param name="onFree"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        internal bool Send<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T outputParameter, Action onFree)
            where T : struct
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutput);
                try
                {
                    Push(output = new ServerOutputFree<T>(callbackIdentity, method, ref outputParameter, onFree));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sending a data collection
        /// 发送数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="returnValues"></param>
        /// <returns></returns>
        internal bool SendKeepCallback<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, IEnumerable<T> returnValues)
        {
            if (closeLock == 0)
            {
                bool isPush = false;
                try
                {
                    var outputHead = default(ServerOutput);
                    var outputEnd = default(ServerOutput);
                    foreach (T returnValue in returnValues)
                    {
                        ServerReturnValue<T> outputParameter = new ServerReturnValue<T>(returnValue);
                        ServerOutput output = GetOutput(callbackIdentity, method, ref outputParameter);
                        if (outputHead != null) output.LinkNext = outputHead;
                        else outputEnd = output;
                        outputHead = output;
                    }
                    if (outputHead != null)
                    {
                        isPush = true;
                        Push(outputHead, outputEnd.notNull());
                    }
                }
                finally
                {
                    if (!isPush) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sending a data collection
        /// 发送数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool SendKeepCallbackLink<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T head, T? end)
#else
        internal bool SendKeepCallbackLink<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T head, T end)
#endif
             where T : KeepCallbackReturnValueLink<T>
        {
            if (closeLock == 0)
            {
                bool isPush = false;
                try
                {
                    var outputHead = default(ServerOutput);
                    var outputEnd = default(ServerOutput);
                    for (var node = head; node != end; node = node.notNull().LinkNext)
                    {
                        ServerReturnValue<T> outputParameter = new ServerReturnValue<T>(node.notNull());
                        ServerOutput output = GetOutput(callbackIdentity, method, ref outputParameter);
                        if (outputHead != null) output.LinkNext = outputHead;
                        else outputEnd = output;
                        outputHead = output;
                    }
                    if (outputHead != null)
                    {
                        isPush = true;
                        Push(outputHead, outputEnd.notNull());
                    }
                }
                finally
                {
                    if (!isPush) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Send(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, CommandServerVerifyStateEnum outputParameter)
        {
            return SetVerifyState(outputParameter) && Send(callbackIdentity, method, new ServerReturnValue<CommandServerVerifyStateEnum>(outputParameter));
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        internal bool Send<T>(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutput);
                try
                {
                    Push(output = GetOutput(callbackIdentity, method, ref outputParameter));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        private bool send<T>(ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutput);
                try
                {
                    AppendOutput(output = getOutput(method, ref outputParameter));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="socket"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendOutput<T>(CommandServerSocket socket, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            return socket.send(method, ref outputParameter);
        }
        /// <summary>
        /// Send data synchronously
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="socket"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendReturnValue<T>(CommandServerSocket socket, ServerInterfaceMethod method, T outputParameter)
        {
            return socket.send(method, new ServerReturnValue<T>(outputParameter));
        }
        /// <summary>
        /// Send custom data synchronously (in asynchronous mode, you need to wait for the next synchronous sending call to be triggered)
        /// 同步发送自定义数据（非同步模式则需要等待下次触发同步发送调用）
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool AppendCustomData(byte[] data)
        {
            if (closeLock == 0)
            {
                AppendOutput(new ServerOutputCustomData(data));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Send custom data synchronously (in asynchronous mode, you need to wait for the next synchronous sending call to be triggered)
        /// 同步发送自定义数据（非同步模式则需要等待下次触发同步发送调用）
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool AppendCustomData(ref SubArray<byte> data)
        {
            if (closeLock == 0)
            {
                AppendOutput(new ServerOutputCustomData(ref data));
                return true;
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
        public bool SendCustomData(byte[] data)
        {
            if (closeLock == 0)
            {
                Push(new ServerOutputCustomData(data));
                return true;
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
        public bool SendCustomData(ref SubArray<byte> data)
        {
            if (closeLock == 0)
            {
                Push(new ServerOutputCustomData(ref data));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Server-side offline count check
        /// 服务端下线计数检查
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckOfflineCount()
        {
            if (OfflineCount.Get() == 0) Server.DecrementOfflineCount();
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckTask(CommandServerSocket socket, Task task)
        {
            //TaskAwaiter taskAwaiter = task.GetAwaiter();
            if (task.IsCompleted)
            {
                socket.CheckOfflineCount();
                var exception = task.Exception;
                if (exception == null) socket.Send(CommandClientReturnTypeEnum.Success);
                else socket.SendLog(CommandClientReturnTypeEnum.ServerException, exception);
            }
            else task.GetAwaiter().UnsafeOnCompleted(new CommandServerCallTask(socket, task).OnCompleted);
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckTask<T>(CommandServerSocket socket, ServerInterfaceMethod method, Task<T> task)
        {
            //TaskAwaiter<T> taskAwaiter = task.GetAwaiter();
            if (task.IsCompleted)
            {
                socket.CheckOfflineCount();
                var exception = task.Exception;
                if (exception == null) socket.send(method, new ServerReturnValue<T>(task.Result));
                else socket.SendLog(CommandClientReturnTypeEnum.ServerException, exception);
            }
            else task.GetAwaiter().UnsafeOnCompleted(new CommandServerCallTask<T>(socket, method, task).OnCompleted);
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="method"></param>
        /// <param name="task"></param>
        private void checkTask(ServerInterfaceMethod method, Task<CommandServerVerifyStateEnum> task)
        {
            CheckOfflineCount();
            var exception = task.Exception;
            if (exception == null)
            {
                if (SetVerifyState(task.Result)) send(method, new ServerReturnValue<CommandServerVerifyStateEnum>(VerifyState));
            }
            else SendLog(CommandClientReturnTypeEnum.ServerException, exception);
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckTask(CommandServerSocket socket, ServerInterfaceMethod method, Task<CommandServerVerifyStateEnum> task)
        {
            //TaskAwaiter<CommandServerVerifyStateEnum> taskAwaiter = task.GetAwaiter();
            if (task.IsCompleted) socket.checkTask(method, task);
            else task.GetAwaiter().UnsafeOnCompleted(new CommandServerCallVerifyStateTask(socket, method, task).OnCompleted);
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="task"></param>
        internal static void CheckTask(CommandServerSocket socket, Task<CommandServerSendOnly> task)
        {
            //TaskAwaiter<CommandServerSendOnly> taskAwaiter = task.GetAwaiter();
            if (task.IsCompleted)
            {
                socket.CheckOfflineCount();
                var exception = task.Exception;
                if (exception != null) socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
            }
            else task.GetAwaiter().UnsafeOnCompleted(new CommandServerCallSendOnlyTask(socket, task).OnCompleted);
        }

        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="key"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum CallTaskQueueAppendQueue<T>(CommandServerCallTaskQueueSet<T> queue, T key, CommandServerCallTaskQueueNode task) where T : IEquatable<T>
        {
            return queue.Add(key, task);
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="key"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum CallTaskQueueAppendLowPriority<T>(CommandServerCallTaskQueueSet<T> queue, T key, CommandServerCallTaskQueueNode task) where T : IEquatable<T>
        {
            return queue.AddLowPriority(key, task);
        }

        /// <summary>
        /// Add asynchronous keep callback
        /// 添加异步保持回调
        /// </summary>
        /// <param name="keepCallback"></param>
        internal void Add(CommandServerKeepCallback keepCallback)
        {
            if (closeLock == 0)
            {
                if (!IsShortLink)
                {
                    var removeKeepCallback = default(CommandServerKeepCallback);
                    Monitor.Enter(keepCallbackLock);
                    try
                    {
                        if (keepCallbacks == null) keepCallbacks = new ReusableDictionary<CallbackIdentity, CommandServerKeepCallback>(0, ReusableDictionaryGroupTypeEnum.Roll);
                        keepCallbacks[keepCallback.CallbackIdentity] = keepCallback;
                        if (Server.Config.MaxKeepCallbackCount > 0 && keepCallbacks.Count > Server.Config.MaxKeepCallbackCount) keepCallbacks.RemoveRoll(out removeKeepCallback);
                    }
                    finally
                    {
                        Monitor.Exit(keepCallbackLock);
                        removeKeepCallback?.CancelKeep(CommandClientReturnTypeEnum.CancelKeepCallback);
                    }
                }
                else shortLinkKeepCallback = keepCallback;
            }
            else keepCallback.SetCancelKeep();
        }
        /// <summary>
        /// The client actively closes keep callback
        /// 客户端主动关闭保持回调
        /// </summary>
        /// <param name="callbackIdentity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClientCancelKeepCallback(CallbackIdentity callbackIdentity)
        {
            var keepCallback = default(CommandServerKeepCallback);
            Monitor.Enter(keepCallbackLock);
            if (keepCallbacks != null && keepCallbacks.Remove(callbackIdentity, out keepCallback))
            {
                Monitor.Exit(keepCallbackLock);
                keepCallback.SetCancelKeep();
            }
            else Monitor.Exit(keepCallbackLock);
        }
        /// <summary>
        /// Remove the asynchronous keep callback
        /// 移除异步保持回调
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal void RemoveKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal void RemoveKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            try
            {
                SendCancelKeepCallback(callbackIdentity, returnType, exception);
            }
            finally
            {
                Monitor.Enter(keepCallbackLock);
                keepCallbacks?.Remove(callbackIdentity);
                Monitor.Exit(keepCallbackLock);
            }
        }
        /// <summary>
        /// Remove the asynchronous keep callback
        /// 移除异步保持回调
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        internal void RemoveKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType)
        {
            try
            {
                SendCancelKeepCallback(callbackIdentity, returnType, null);
            }
            finally
            {
                Monitor.Enter(keepCallbackLock);
                keepCallbacks?.Remove(callbackIdentity);
                Monitor.Exit(keepCallbackLock);
            }
        }
        /// <summary>
        /// Remove the asynchronous keep callback
        /// 移除异步保持回调
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveKeepCallback(CallbackIdentity callbackIdentity, Exception exception)
        {
            RemoveKeepCallback(callbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
            Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
        }
        /// <summary>
        /// Server exception cancellation asynchronous keep callback
        /// 服务端异常取消异步保持回调
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal void CancelKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal void CancelKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            var keepCallback = default(CommandServerKeepCallback);
            Monitor.Enter(keepCallbackLock);
            if (keepCallbacks != null && keepCallbacks.Remove(callbackIdentity, out keepCallback))
            {
                Monitor.Exit(keepCallbackLock);
                keepCallback.CancelKeep(returnType, exception);
            }
            else Monitor.Exit(keepCallbackLock);
        }
        /// <summary>
        /// Cancel the asynchronous keep callback
        /// 取消异步保持回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void CancelKeepCallback(CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal void CancelKeepCallback(CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            CancelKeepCallback(CallbackIdentity, returnType, exception);
        }
        /// <summary>
        /// Send the output of the cancel asynchronous keep callback
        /// 发送取消异步保持回调输出
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal void SendCancelKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal void SendCancelKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            if (closeLock == 0)
            {
                var output = default(ServerOutputCancelKeepCallback);
                try
                {
                    Push(output = new ServerOutputCancelKeepCallback(callbackIdentity, returnType, exception != null && Server.Config.IsOutputExceptionMessage ? exception : null));
                }
                finally
                {
                    if (output == null) DisposeSocket();
                }
            }
        }
        ///// <summary>
        ///// 发送取消异步保持回调输出
        ///// </summary>
        ///// <param name="returnType"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void SendCancelKeepCallback(CommandClientReturnType returnType)
        //{
        //    SendCancelKeepCallback(CallbackIdentity, returnType);
        //}


        ///// <summary>
        ///// JSON 序列化配置
        ///// </summary>
        //internal static readonly AutoCSer.JsonSerializeConfig JsonSerializeConfig = AutoCSer.JsonSerializeConfig.CreateInternal();
        /// <summary>
        /// Empty command service socket, used to simulate the server-side context
        /// 空命令服务套接字，用于模拟服务端上下文
        /// </summary>
        internal static readonly CommandServerSocket CommandServerSocketContext = new CommandServerSocket();
        /// <summary>
        /// The set of sockets waiting to send data
        /// 等待发送数据的套接字集合
        /// </summary>
#if NetStandard21
        private static CommandServerSocket? outputSocketHead;
#else
        private static CommandServerSocket outputSocketHead;
#endif
        /// <summary>
        /// The socket set sends data waiting events
        /// 套接字集合发送数据等待事件
        /// </summary>
        private static readonly System.Threading.AutoResetEvent socketOutputWaitHandle = new AutoResetEvent(false);
        /// <summary>
        /// The socket set sends data
        /// 套接字集合发送数据
        /// </summary>
        private static void socketBuildOutput()
        {
            do
            {
                socketOutputWaitHandle.WaitOne();
                var head = System.Threading.Interlocked.Exchange(ref outputSocketHead, null).notNull();
                var socket = head;
                do
                {
                    try
                    {
                        do
                        {
                            socket = head;
                            head = head.getNextOutputSocket();
                            socket.buildOutput();
                        }
                        while (head != null);
                        break;
                    }
                    catch (Exception exception)
                    {
                        socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (head != null);
            }
            while (true);
        }
        /// <summary>
        /// Start the socket to send data thread
        /// 启动套接字发送数据线程
        /// </summary>
        internal static void StartSocketBuildOutputThread()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(socketBuildOutput);
        }
    }
}
