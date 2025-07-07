using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Keep callback command
    /// 保持回调命令
    /// </summary>
    public abstract class KeepCommand : Command, INotifyCompletion
    {
        /// <summary>
        /// Keep callback object of the command
        /// 命令保持回调对象
        /// </summary>
        protected readonly CommandKeepCallback keepCallback;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// The status of the reqeust command added to the output queue
        /// 请求命令添加到输出队列的状态
        /// </summary>
        internal CommandPushStateEnum PushState;
        /// <summary>
        /// Request the return value type
        /// 请求返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType { get; protected set; }
        /// <summary>
        /// Keep callback command returning true
        /// 保持回调命令返回 true
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        internal bool IsDisposed;
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal KeepCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex)
        {
            keepCallback = new CommandKeepCallback(this);
            ReturnType = CommandClientReturnTypeEnum.Success;
        }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal unsafe override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal unsafe override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
            if (stream.Data.Pointer.FreeSize >= sizeof(uint) + sizeof(CallbackIdentity) || buildInfo.Count == 0)
            {
                uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
                var nextCommand = LinkNext;
                if (methodIndex != 0)
                {
                    SetTimeoutSeconds();
                    uint identity;
                    int callbackIndex = Controller.Socket.CommandPool.Push(this, out identity);
                    if (callbackIndex != 0)
                    {
                        byte* data = stream.GetBeforeMove(sizeof(uint) + sizeof(CallbackIdentity));
                        *(uint*)data = methodIndex | (uint)CommandFlagsEnum.Callback;
                        *(CallbackIdentity*)(data + sizeof(uint)) = new CallbackIdentity((uint)callbackIndex, identity);
                        buildInfo.SetIsCallback();
                        keepCallback.Set(callbackIndex, identity);
                        LinkNext = null;
                        return nextCommand;
                    }
                    ++buildInfo.FreeCount;
                    OnBuildError(CommandClientReturnTypeEnum.ClientBuildError);
                }
                else
                {
                    ++buildInfo.FreeCount;
                    OnBuildError(CommandClientReturnTypeEnum.ControllerMethodIndexError);
                }
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <param name="inputParameter"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal unsafe Command? BuildKeep<T>(ref ClientBuildInfo buildInfo, ref T inputParameter)
#else
        internal unsafe Command BuildKeep<T>(ref ClientBuildInfo buildInfo, ref T inputParameter)
#endif
            where T : struct
        {
            uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
            var nextCommand = LinkNext;
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
                            keepCallback.Set(callbackIndex, identity);
                            LinkNext = null;
                            return nextCommand;
                        }
                        stream.Data.Pointer.CurrentIndex = streamLength;
                        ++buildInfo.FreeCount;
                        OnBuildError(CommandClientReturnTypeEnum.ClientBuildError);
                        LinkNext = null;
                        return nextCommand;
                    }
                }
                stream.Data.Pointer.CurrentIndex = streamLength;
                buildInfo.IsFullSend = 1;
                return this;
            }
            inputParameter = default(T);
           ++buildInfo.FreeCount;
            OnBuildError(CommandClientReturnTypeEnum.ControllerMethodIndexError);
            LinkNext = null;
            return nextCommand;
        }
        ///// <summary>
        ///// 创建命令输入数据
        ///// </summary>
        ///// <param name="buildInfo"></param>
        ///// <param name="inputParameter"></param>
        ///// <returns>Return false on failure</returns>
        //internal unsafe Command BuildKeep<T>(ref ClientBuildInfo buildInfo, ref T inputParameter)
        //    where T : struct
        //{
        //    UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
        //    ClientInterfaceMethod method = Controller.Methods[MethodIndex];
        //    if (buildInfo.Count == 0 || method.CheckMaxDataSize(buildInfo.SendBufferSize, stream.Data.Pointer.CurrentIndex))
        //    {
        //        uint methodIndex = Controller.GetMethodIndex(MethodIndex);
        //        if (methodIndex != 0)
        //        {
        //            SetTimeoutSeconds();
        //            int callbackIndex = Controller.Socket.CommandPool.Push(this, out uint identity);
        //            if (callbackIndex != 0)
        //            {
        //                int streamLength = stream.Data.Pointer.CurrentIndex;
        //                stream.MoveSize(sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int));
        //                if (method.IsSimpleSerializeParamter)
        //                {
        //                    SimpleSerialize.Serializer<T>.DefaultSerializer(stream, ref inputParameter);
        //                    methodIndex |= (uint)(CommandFlags.Callback | CommandFlags.SendData);
        //                }
        //                else if (!method.IsJsonSerializeParamter)
        //                {
        //                    Controller.Socket.BinarySerialize(ref inputParameter);
        //                    methodIndex |= (uint)(CommandFlags.Callback | CommandFlags.SendData);
        //                }
        //                else
        //                {
        //                    Controller.Socket.JsonSerialize(ref inputParameter);
        //                    methodIndex |= (uint)(CommandFlags.Callback | CommandFlags.SendData | CommandFlags.JsonSerialize);
        //                }
        //                int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(uint) + sizeof(CallbackIdentity) + sizeof(int));
        //                byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
        //                *(uint*)dataFixed = methodIndex;
        //                *(CallbackIdentity*)(dataFixed + sizeof(uint)) = new CallbackIdentity((uint)callbackIndex, identity);
        //                *(int*)(dataFixed + (sizeof(uint) + sizeof(CallbackIdentity))) = dataLength;
        //                method.SetMaxDataSize(dataSize);
        //                buildInfo.SetIsCallback();
        //                keepCallback.Set(callbackIndex, identity);
        //                return LinkNext;
        //            }
        //            ++buildInfo.FreeCount;
        //            OnBuildError(CommandClientReturnType.ClientBuildError);
        //        }
        //        else
        //        {
        //            ++buildInfo.FreeCount;
        //            OnBuildError(CommandClientReturnType.ControllerMethodIndexError);
        //        }
        //        return LinkNext;
        //    }
        //    buildInfo.IsFullSend = 1;
        //    return this;
        //}

        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }

        /// <summary>
        /// Add commands to the output queue
        /// 添加命令到输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push()
        {
            PushState = Controller.Socket.TryPush(this);
            if (PushState != CommandPushStateEnum.WaitCount)
            {
                IsCompleted = true;
                continuation = Common.EmptyAction;
            }
        }
        /// <summary>
        /// The command waiting for idle output attempts to be added to the output queue again
        /// 等待空闲输出的命令再次尝试添加到输出队列
        /// </summary>
        /// <returns>Is it necessary to keep waiting
        /// 是否需要继续等待</returns>
        internal override bool CheckWaitPush()
        {
            PushState = Controller.Socket.TryPush(this);
            if (PushState != CommandPushStateEnum.WaitCount)
            {
                IsCompleted = true;
                if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
                {
                    Callback(continuation);
                }
                return false;
            }
            return true;
        }
    }
}
