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
    /// 命令服务套接字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed class CommandServerSocket
    {
        /// <summary>
        /// 命令服务
        /// </summary>
        public readonly CommandListener Server;
        /// <summary>
        /// 套接字
        /// </summary>
        private readonly Socket socket;
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private readonly EventHandler<SocketAsyncEventArgs> onReceiveAsyncCallback;
        /// <summary>
        /// 尝试发送数据委托
        /// </summary>
        private readonly Action buildOutputHandle;
        /// <summary>
        /// 异步保持回调集合访问锁
        /// </summary>
        private readonly object keepCallbackLock;
        /// <summary>
        /// 套接字关闭事件
        /// </summary>
#if NetStandard21
        private HashSet<HashObject<Action>>? onClosedHashSet;
#else
        private HashSet<HashObject<Action>> onClosedHashSet;
#endif
        /// <summary>
        /// 填充隔离数据
        /// </summary>
#pragma warning disable CS0169
        private readonly CpuCachePad pad0;
#pragma warning restore CS0169
        /// <summary>
        /// 命令位图访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock commandDataLock;
        /// <summary>
        /// 自定义会话对象
        /// </summary>
#if NetStandard21
        public object? SessionObject;
#else
        public object SessionObject;
#endif
        /// <summary>
        /// 套接字上下文绑定服务端实例集合
        /// </summary>
        private CommandServerBindContextController[] bindControllers;
        /// <summary>
        /// 允许访问的命令位图
        /// </summary>
        private AutoCSer.Memory.Pointer commandData;
        /// <summary>
        /// 接收数据套接字异步事件对象
        /// </summary>
        private SocketAsyncEventArgs receiveAsyncEventArgs;
        /// <summary>
        /// 接收数据二进制反序列化
        /// </summary>
#if NetStandard21
        private AutoCSer.BinaryDeserializer? receiveDeserializer;
#else
        private AutoCSer.BinaryDeserializer receiveDeserializer;
#endif
        /// <summary>
        /// 验证超时时间
        /// </summary>
        private DateTime verifyTimeout;
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        private ByteArrayBuffer receiveBuffer;
        /// <summary>
        /// 临时接收数据缓冲区
        /// </summary>
        private ByteArrayBuffer receiveBigBuffer;
        /// <summary>
        /// 接收数据起始位置
        /// </summary>
        private unsafe byte* receiveDataStart;
        /// <summary>
        /// 上一次接收字节数量
        /// </summary>
        private int lastReceiveSize = ushort.MaxValue;
        /// <summary>
        /// 当前处理接收数据字节数
        /// </summary>
        private int receiveIndex;
        /// <summary>
        /// 当前数据编码后的字节大小
        /// </summary>
        private int transferDataSize;
        /// <summary>
        /// 当前数据字节大小
        /// </summary>
        private int dataSize;
        /// <summary>
        /// 当前解析命令服务控制器
        /// </summary>
        private CommandServerController controller;
        /// <summary>
        /// 当前解析命令服务控制器
        /// </summary>
        public CommandServerController CurrentController { get { return controller; } }
        /// <summary>
        /// 服务端接口方法信息
        /// </summary>
        internal ServerInterfaceMethod Method;
        /// <summary>
        /// 当前服务端下线计数对象
        /// </summary>
        internal OfflineCount OfflineCount;
        /// <summary>
        /// 当前处理会话标识
        /// </summary>
        internal CallbackIdentity CallbackIdentity;
        /// <summary>
        /// 自定义数据字节长度
        /// </summary>
        private int customDataSize
        {
            get { return (int)CallbackIdentity.Index; }
            set { CallbackIdentity.Index = (uint)value; }
        }
        /// <summary>
        /// 同步输出头节点
        /// </summary>
        private ServerOutput outputHead;
        /// <summary>
        /// 同步输出尾节点
        /// </summary>
        private ServerOutput outputEnd;
        /// <summary>
        /// 当前命令方法序号 + 命令标志位信息
        /// </summary>
        private uint commandMethodIndex;
        /// <summary>
        /// 当前命令方法序号
        /// </summary>
        internal int CommandMethodIndex { get { return (int)(commandMethodIndex & Command.MethodIndexAnd); } }
        /// <summary>
        /// 接收数据线程ID
        /// </summary>
        private int onReceiveThreadId;
        /// <summary>
        /// 接收数据套接字错误
        /// </summary>
        private SocketError receiveSocketError;
        ///// <summary>
        ///// 允许验证失败次数
        ///// </summary>
        //private byte verifyMethodErrorCount;
        /// <summary>
        /// 是否通过函数验证
        /// </summary>
        internal volatile CommandServerVerifyStateEnum VerifyState;
        /// <summary>
        /// 接收数据回调类型
        /// </summary>
        private ServerReceiveTypeEnum receiveType;
        /// <summary>
        /// 接收数据错误类型
        /// </summary>
        private ServerReceiveErrorTypeEnum receiveErrorType;
        /// <summary>
        /// 是否触发套接字关闭操作
        /// </summary>
        private bool isCloseSocket;
#pragma warning disable CS0169
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad1;
#pragma warning restore CS0169
        /// <summary>
        /// 下一个输出套接字
        /// </summary>
#if NetStandard21
        private CommandServerSocket? nextOutputSocket;
#else
        private CommandServerSocket nextOutputSocket;
#endif
        /// <summary>
        /// 服务端套接字输出信息
        /// </summary>
        private LinkStack<ServerOutput> outputs;
        /// <summary>
        /// 未处理套接字队列首节点
        /// </summary>
#if NetStandard21
        private ServerOutput? buildOutputHead;
#else
        private ServerOutput buildOutputHead;
#endif
        /// <summary>
        /// 未处理套接字队列尾节点
        /// </summary>
#if NetStandard21
        private ServerOutput? buildOutputEnd;
#else
        private ServerOutput buildOutputEnd;
#endif
        /// <summary>
        /// 是否正在输出
        /// </summary>
        private int isOutput;
        /// <summary>
        /// 关闭套接字访问锁
        /// </summary>
        private int closeLock;
        /// <summary>
        /// 是否已经触发套接字关闭操作
        /// </summary>
        public bool IsClose { get { return closeLock != 0; } }
#pragma warning disable CS0169
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad2;
#pragma warning restore CS0169
        /// <summary>
        /// 异步保持回调集合
        /// </summary>
#if NetStandard21
        private Dictionary<CallbackIdentity, CommandServerKeepCallback>? keepCallbacks;
#else
        private Dictionary<CallbackIdentity, CommandServerKeepCallback> keepCallbacks;
#endif
        /// <summary>
        /// 输出数据二进制序列化
        /// </summary>
        internal BinarySerializer OutputSerializer;
        /// <summary>
        /// 发送数据异步回调
        /// </summary>
#if NetStandard21
        private EventHandler<SocketAsyncEventArgs>? onSendAsyncCallback;
#else
        private EventHandler<SocketAsyncEventArgs> onSendAsyncCallback;
#endif
        /// <summary>
        /// 发送数据异步事件
        /// </summary>
        private SocketAsyncEventArgs sendAsyncEventArgs;
        /// <summary>
        /// 输出数据缓冲区
        /// </summary>
        private ByteArrayBuffer sendBuffer;
        /// <summary>
        /// 输出编码数据缓冲区
        /// </summary>
        private ByteArrayBuffer sendTransferBuffer;
        /// <summary>
        /// 输出复制数据缓冲区
        /// </summary>
        private ByteArrayBuffer sendCopyBuffer;
        /// <summary>
        /// 发送数据
        /// </summary>
        private SubArray<byte> sendData;
        /// <summary>
        /// 上一次发送字节数量
        /// </summary>
        private int lastSendSize = ushort.MaxValue;
        /// <summary>
        /// 发送数据套接字错误
        /// </summary>
        private SocketError sendSocketError;
        /// <summary>
        /// 服务端套接字发送数据线程类型
        /// </summary>
        private readonly CommandServerSocketBuildOutputThreadEnum buildOutputThreadEnum;
        /// <summary>
        /// 字符串二进制序列化直接复制内存数据
        /// </summary>
        private bool isSerializeCopyString;

        /// <summary>
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
        }
        /// <summary>
        /// 命令服务套接字
        /// </summary>
        /// <param name="server"></param>
        /// <param name="socket"></param>
        internal CommandServerSocket(CommandListener server, Socket socket)
        {
            this.Server = server;
            keepCallbackLock = this.socket = socket;
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
        }
        /// <summary>
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
        /// 设置允许访问的命令位图
        /// </summary>
        /// <returns>是否设置成功</returns>
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
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="methodIndex">命令服务控制器中的方法编号</param>
        /// <param name="controller">命令服务控制器</param>
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
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="methodName">命令服方法名称</param>
        /// <param name="controller">命令服务控制器</param>
        /// <returns>是否设置成功</returns>
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
        /// 设置允许访问的命令
        /// </summary>
        /// <param name="methodNames">命令服方法名称集合</param>
        /// <param name="controller">命令服务控制器</param>
        /// <returns>匹配方法数量</returns>
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
        /// 设置命令服务验证结果状态
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="verifyState"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetVerifyState(CommandServerSocket socket, CommandServerVerifyStateEnum verifyState)
        {
            socket.SetVerifyState(verifyState);
        }
        /// <summary>
        /// 开始接受数据
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
        /// 关闭套接字
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
        /// 关闭套接字
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
                            finally { Server.SocketAsyncEventArgsPool.Push(receiveAsyncEventArgs); }
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
        /// 移除关闭回调委托
        /// </summary>
        /// <param name="onClosed"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveOnClosed(Action onClosed)
        {
            onClosedHashSet?.Remove(onClosed);
        }
        /// <summary>
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
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>是否成功</returns>
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
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="socket">命令服务套接字</param>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Deserialize<T>(CommandServerSocket socket, ref SubArray<byte> data, ref T value, bool isSimpleSerialize)
            where T : struct
        {
            return socket.deserialize(ref data, ref value, isSimpleSerialize);
        }
        /// <summary>
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
        /// 如果当前线程为接收数据 IO 线程 await 强制 Task.Run 操作
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SwitchAwaiter GetSwitchAwaiter()
        {
            return onReceiveThreadId == System.Environment.CurrentManagedThreadId ? SwitchAwaiter.Default : SwitchAwaiter.Completed;
        }
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
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
        /// 执行函数验证
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
        /// 获取命令
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
        /// 获取命令
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
        /// 判断命令是否有效
        /// </summary>
        /// <returns></returns>
        private unsafe bool isCommandMap()
        {
            int commandMapIndex = controller.GetCommandMapIndex(CommandMethodIndex);
            return (uint)commandMapIndex <= (uint)commandData.CurrentIndex && commandData.GetBit(commandMapIndex) != 0 && commandData.Data != null;
        }
        /// <summary>
        /// 循环处理命令
        /// </summary>
        /// <param name="isCommand">是否接收命令后的处理</param>
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
                                controllerDoCommand(ref data);
                                isCommand = false;
                                goto START;
                            }
                        CHECKDATA:
                            if (transferDataSize <= receiveBuffer.CurrentIndex - receiveIndex)
                            {
                                if (doControllerCommand())
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
        /// 检查命令数据
        /// </summary>
        /// <param name="isDoCommand">是否执行了命令</param>
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
                            return isDoCommand = doCommand();
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
                    if (transferDataSize == (receiveBigBuffer.CurrentIndex += receiveSize)) return isDoCommand = doCommandBig();
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
        /// 获取数据
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
                            return doCommand() && loop(false);
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
        /// 获取数据
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
                            receiveType = ServerReceiveTypeEnum.Command;
                            return socket.ReceiveAsync(receiveAsyncEventArgs) || isCommand();
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
        /// 执行命令
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
        /// 执行命令
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
        /// 执行命令
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
        /// 执行命令
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
        /// 执行命令
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
        /// 执行命令
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
        /// 控制器名利处理
        /// </summary>
        /// <param name="data"></param>
        private void controllerDoCommand(ref SubArray<byte> data)
        {
            if (!Method.IsOfflineCount) controller.DoCommandOnly(this, ref data);
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
                        controller.DoCommandOfflineCount(this, ref data);
                        OfflineCount = OfflineCount.Null;
                        return;
                    default: controller.DoCommandOfflineCount(this, ref data); return;
                }
            }
            else Send(CommandClientReturnTypeEnum.ServerOffline);
        }
        /// <summary>
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
                    Server.SocketAsyncEventArgsPool.Push(sendAsyncEventArgs);
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
        /// 释放输出复制缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeSendCopyBuffer()
        {
            sendCopyBuffer.Free();
            sendTransferBuffer.Free();
        }
        /// <summary>
        /// 启动发送数据
        /// </summary>
        private void output()
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
        /// <summary>
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
                        case ServerSocketSendStateEnum.Asynchronous: return;
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
                if (closeLock == 0)
                {
                    if (buildInfo.IsError)
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
        }
        /// <summary>
        /// 设置发送数据
        /// </summary>
        /// <param name="start">数据起始位置</param>
        /// <param name="count">输出数量</param>
        /// <returns>是否改变输出缓冲区</returns>
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
        /// 发送数据
        /// </summary>
        /// <returns>发送数据状态</returns>
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
        /// 数据发送完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
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
                                if (closeLock != 0 && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
                                {
                                    closeSend();
                                }
                            }
                            else if (Interlocked.CompareExchange(ref isOutput, 1, 0) == 0)
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
        /// 释放未处理套接字队列
        /// </summary>
        private void freeBuildOutput()
        {
            var head = buildOutputHead;
            buildOutputHead = null;
            ServerOutput.CancelLink(head);
        }
        /// <summary>
        /// 添加同步输出
        /// </summary>
        /// <param name="output"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendOutput(ServerOutput output)
        {
            if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) output.LinkNext = outputHead;
            else outputEnd = output;
            outputHead = output;
        }
        /// <summary>
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
        /// 添加输出信息
        /// </summary>
        /// <param name="output">当前输出信息</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push(ServerOutput output)
        {
            if (outputs.IsPushHead(output) && Interlocked.CompareExchange(ref isOutput, 1, 0) == 0) this.output();
        }
        /// <summary>
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
        /// 发送数据
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 发送数据
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
        /// 发送数据
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 同步发送数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 同步发送数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 同步发送成功返回值类型
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerSocket socket)
        {
            return socket.Send(CommandClientReturnTypeEnum.Success);
        }
        /// <summary>
        /// 同步获取输出信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>输出信息</returns>
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
        /// 获取输出信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callbackIdentity">会话标识</param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>输出信息</returns>
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
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="callbackIdentity">会话标识</param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="callbackIdentity">会话标识</param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <param name="onFree"></param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 发送数据
        /// </summary>
        /// <param name="callbackIdentity">会话标识</param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Send(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, CommandServerVerifyStateEnum outputParameter)
        {
            return SetVerifyState(outputParameter) && Send(callbackIdentity, method, new ServerReturnValue<CommandServerVerifyStateEnum>(outputParameter));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="callbackIdentity">会话标识</param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
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
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="socket"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendOutput<T>(CommandServerSocket socket, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            return socket.send(method, ref outputParameter);
        }
        /// <summary>
        /// 同步发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="socket"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendReturnValue<T>(CommandServerSocket socket, ServerInterfaceMethod method, T outputParameter)
        {
            return socket.send(method, new ServerReturnValue<T>(outputParameter));
        }
        /// <summary>
        /// 同步发送自定义数据（非同步模式则需要等待下次触发同步发送调用）
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
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
        /// 同步发送自定义数据（非同步模式则需要等待下次触发同步发送调用）
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
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
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
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
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
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
        /// 服务端下线计数检查
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckOfflineCount()
        {
            if (OfflineCount.Get() == 0) Server.DecrementOfflineCount();
        }
        /// <summary>
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
        /// 添加队列任务（低优先级）
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
        /// 添加 TCP 服务器端异步保持调用
        /// </summary>
        /// <param name="keepCallback"></param>
        internal void Add(CommandServerKeepCallback keepCallback)
        {
            if (closeLock == 0)
            {
                Monitor.Enter(keepCallbackLock);
                try
                {
                    if (keepCallbacks == null) keepCallbacks = DictionaryCreator<CallbackIdentity>.Create<CommandServerKeepCallback>();
                    keepCallbacks[keepCallback.CallbackIdentity] = keepCallback;
                }
                finally { Monitor.Exit(keepCallbackLock); }
            }
            else keepCallback.SetCancelKeep();
        }
        /// <summary>
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
        /// 移除 TCP 服务器端异步保持调用
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
        /// 移除 TCP 服务器端异步保持调用
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
        /// 移除 TCP 服务器端异步保持调用
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
        /// 服务端异常取消异步保持调用
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
        /// 取消异步保持调用
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
        /// 发送取消异步保持调用输出
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
        ///// 发送取消异步保持调用输出
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
        /// 空命令服务套接字，用于模拟服务端上下文
        /// </summary>
        internal static readonly CommandServerSocket CommandServerSocketContext = new CommandServerSocket();
        /// <summary>
        /// 等待发送数据的套接字集合
        /// </summary>
#if NetStandard21
        private static CommandServerSocket? outputSocketHead;
#else
        private static CommandServerSocket outputSocketHead;
#endif
        /// <summary>
        /// 套接字集合发送数据等待事件
        /// </summary>
        private static OnceAutoWaitHandle socketOutputWaitHandle;
        /// <summary>
        /// 套接字集合发送数据
        /// </summary>
        private static void socketBuildOutput()
        {
            do
            {
                socketOutputWaitHandle.Wait();
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
        /// 启动套接字发送数据线程
        /// </summary>
        internal static void StartSocketBuildOutputThread()
        {
            socketOutputWaitHandle.Set(new object());
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(socketBuildOutput);
        }
    }
}
