using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if AOT
using ClientMethodType = AutoCSer.Net.CommandServer.ClientMethod;
#else
using ClientMethodType = AutoCSer.Net.CommandServer.ClientInterfaceMethod;
#endif

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    public abstract class Command : AutoCSer.Threading.Link<Command>
    {
        /// <summary>
        /// 输出流起始位置
        /// </summary>
        internal const int StreamStartIndex = sizeof(uint) + sizeof(int);

        /// <summary>
        /// 命令函数序号有效位
        /// </summary>
        internal const int MethodIndexBits = 29;
        /// <summary>
        /// 命令函数序号最大值
        /// </summary>
        internal const uint MethodIndexAnd = ((1U << MethodIndexBits) - 1);

        //      29b		        1b		            1b		    1b
        //+4	MethodIndex     JsonSerialize       SendData    Callback    #数据头部4字节
        //	    4B		        4B
        //[+8]  CallbackIndex   CallbackIdentity				            #回调会话序号4字节 + 标识4字节 [当需要服务端返回数据时存在]
        //[+4]  SendDataSize							                    #发送数据字节长度4字节，负数表示数据被压缩 [当存在输入参数时存在]
        //[+4]  DataSize							                        #数据真实字节长度4字节 [当数据被压缩时存在]
        //[+n]  Data...								                        #参数数据4字节对齐

        /// <summary>
        /// 命令客户端控制器
        /// </summary>
        internal readonly CommandClientController Controller;
        /// <summary>
        /// 命令客户端套接字
        /// </summary>
        public CommandClientSocket Socket { get { return Controller.Socket; } }
        /// <summary>
        /// 客户端接口方法信息
        /// </summary>
        internal readonly ClientMethodType Method;
        /// <summary>
        /// 超时秒计数
        /// </summary>
        internal uint TimeoutSeconds;
        /// <summary>
        /// 是否保持回调命令
        /// </summary>
        internal virtual bool IsKeepCallback { get { return false; } }
        /// <summary>
        /// 客户端命令
        /// </summary>
        internal Command()
        {
#if NetStandard21
            Controller = CommandClientSocket.Null.Controller;
            Method = KeepCallbackCommand.KeepCallbackMethod;
#endif
        }
        /// <summary>
        /// 客户端命令
        /// </summary>
        /// <param name="method"></param>
        internal Command(ClientMethodType method)
        {
#if NetStandard21
            Controller = CommandClientSocket.Null.Controller;
#endif
            Method = method;
        }
        /// <summary>
        /// 客户端命令
        /// </summary>
        /// <param name="controller"></param>
        internal Command(CommandClientController controller)
        {
            Controller = controller;
#if NetStandard21
            Method = KeepCallbackCommand.KeepCallbackMethod;
#endif
        }
        /// <summary>
        /// 客户端命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal Command(CommandClientController controller, int methodIndex)
        {
            Controller = controller;
            Method = Controller.Methods[methodIndex];
        }
        /// <summary>
        /// 设置超时秒计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetTimeoutSeconds()
        {
            var timeoutCount = Controller.Socket.CommandPool.TimeoutCount;
            if (timeoutCount != null) TimeoutSeconds = timeoutCount.TryIncrement(Method.TimeoutSeconds);
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>下一个命令</returns>
#if NetStandard21
        internal virtual Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal virtual Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <param name="inputParameter"></param>
        /// <returns>是否成功</returns>
#if NetStandard21
        internal unsafe Command? Build<T>(ref ClientBuildInfo buildInfo, ref T inputParameter)
#else
        internal unsafe Command Build<T>(ref ClientBuildInfo buildInfo, ref T inputParameter)
#endif
            where T : struct
        {
            uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
            if (methodIndex != 0)
            {
                UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
                int streamLength = stream.SetCanResizeGetCurrentIndex(buildInfo.Count == 0);
                if (stream.MoveSize(sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int)))
                {
                    if (Method.IsSimpleSerializeParamter)
                    {
                        SimpleSerialize.Serializer<T>.DefaultSerializer(stream, ref inputParameter);
                        methodIndex |= (uint)(CommandFlagsEnum.Callback | CommandFlagsEnum.SendData);
                    }
                    else
                    {
                        Controller.Socket.OutputSerializer.IndependentSerialize(ref inputParameter);
                        methodIndex |= (uint)(CommandFlagsEnum.Callback | CommandFlagsEnum.SendData);
                    }
                    if (!stream.IsResizeError)
                    {
                        inputParameter = default(T);
                        SetTimeoutSeconds();
                        uint identity;
                        int callbackIndex = Controller.Socket.CommandPool.Push(this, out identity);
                        if (callbackIndex != 0)
                        {
                            int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int));
                            byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                            *(uint*)dataFixed = methodIndex;
                            *(CallbackIdentity*)(dataFixed + sizeof(uint)) = new CallbackIdentity((uint)callbackIndex, identity);
                            *(int*)(dataFixed + (sizeof(uint) + sizeof(CallbackIdentity))) = dataLength;
                            buildInfo.SetIsCallback();
                            return LinkNext;
                        }
                        stream.Data.Pointer.CurrentIndex = streamLength;
                        ++buildInfo.FreeCount;
                        OnBuildError(CommandClientReturnTypeEnum.ClientBuildError);
                        return LinkNext;
                    }
                }
                stream.Data.Pointer.CurrentIndex = streamLength;
                buildInfo.IsFullSend = 1;
                return this;
            }
            inputParameter = default(T);
            ++buildInfo.FreeCount;
            OnBuildError(CommandClientReturnTypeEnum.ControllerMethodIndexError);
            return LinkNext;
        }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected virtual void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        /// <returns></returns>
        internal virtual ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            return ClientReceiveErrorTypeEnum.OnReceiveInvalidOperation;
        }
        /// <summary>
        /// 取消保持回调（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal virtual void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal virtual void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 关闭套接字等待添加队列命令处理
        /// </summary>
        /// <param name="next"></param>
        /// <returns>是否需要继续等待</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool CheckWaitPush(ref Command? next)
#else
        internal bool CheckWaitPush(ref Command next)
#endif
        {
            next = LinkNext;
            LinkNext = null;
            return CheckWaitPush();
        }
        /// <summary>
        /// 检查等待添加队列命令
        /// </summary>
        /// <returns>是否需要继续等待</returns>
        internal abstract bool CheckWaitPush();
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Callback(Action callback)
        {
            try
            {
#if DEBUG
#if AOT
                Callback(callback, Method.CallbackType, Controller.Socket.OnReceiveThreadId == System.Environment.CurrentManagedThreadId, Method.MatchMethodName, Controller.ControllerName, Method.MethodIndex);
#else
                Callback(callback, Method.CallbackType, Controller.Socket.OnReceiveThreadId == System.Environment.CurrentManagedThreadId, Method.Method.Name, Controller.ControllerName, Method.MethodIndex);
#endif
#else
                Callback(callback, Method.CallbackType, Controller.Socket.OnReceiveThreadId == System.Environment.CurrentManagedThreadId);
#endif
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
        }
#if DEBUG
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="callbackType"></param>
        /// <param name="isSynchronousThread"></param>
        /// <param name="methodName"></param>
        /// <param name="controllerName"></param>
        /// <param name="methodIndex"></param>
#else
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="callbackType"></param>
        /// <param name="isSynchronousThread"></param>
#endif
        public static void Callback(Action callback, ClientCallbackTypeEnum callbackType, bool isSynchronousThread
#if DEBUG
#if NetStandard21
            , string? methodName, string? controllerName, int methodIndex
#else
            , string methodName, string controllerName, int methodIndex
#endif
#endif
            )
        {
            switch (callbackType)
            {
                case ClientCallbackTypeEnum.CheckRunTask:
                    if (isSynchronousThread)
                    {
                        Task.Run(callback);
                        return;
                    }
                    goto SYNCHRONOUS;
                case ClientCallbackTypeEnum.Synchronous:
                SYNCHRONOUS:
#if DEBUG
                        AutoCSer.Threading.LockObject lockCaller = new AutoCSer.Threading.LockObject(callback);
                        System.Threading.Monitor.Enter(callback);
                        try
                        {
                            lockCaller.CheckThread(methodName ?? string.Empty, controllerName ?? string.Empty, methodIndex);
                            callback();
                        }
                        catch (Exception exception)
                        {
                            AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                        }
                        finally { System.Threading.Monitor.Exit(callback); }
#else
                    try
                    {
                        callback();
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
#endif
                    return;
                case ClientCallbackTypeEnum.RunTask: Task.Run(callback); return;
                case ClientCallbackTypeEnum.TinyBackground: AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(callback); return;
                case ClientCallbackTypeEnum.ThreadPool:
                    if (!System.Threading.ThreadPool.QueueUserWorkItem(new AutoCSer.Threading.ThreadPoolQueueUserWorkItem(callback).Callback)) Task.Run(callback);
                    return;
            }
        }
        /// <summary>
        /// 添加到回调队列
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendQueue(Action callback)
        {
            Controller.AppendQueue(Method, callback);
        }

        /// <summary>
        /// 取消命令调用
        /// </summary>
        /// <param name="head"></param>
        /// <param name="returnType"></param>
        internal static void CancelLink(Command head, CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.SocketClosed)
        {
            SubArray<byte> data = new SubArray<byte>((int)(byte)returnType);
            var next = default(Command);
            do
            {
                try
                {
                    do
                    {
                        next = head.GetLinkNextClear();
                        head.OnReceive(ref data);
                        if (next == null) return;
                        head = next;
                    }
                    while (true);
                }
                catch { }
                if (next == null) return;
                head = next;
            }
            while (true);
        }
    }
}
