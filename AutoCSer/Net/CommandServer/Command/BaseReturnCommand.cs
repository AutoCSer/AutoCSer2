using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The return value command
    /// 返回值命令
    /// </summary>
    public abstract class BaseReturnCommand : Command, INotifyCompletion
    {
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
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        internal CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        internal string? ErrorMessage;
#else
        internal string ErrorMessage;
#endif
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        internal BaseReturnCommand() { }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        internal BaseReturnCommand(CommandClientController controller) : base(controller) { }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal BaseReturnCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        internal BaseReturnCommand(CommandClientDefaultController controller) : base(controller)
        {
            ReturnType = controller.DefaultControllerReturnType;
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal unsafe Command? BuildQueue(ref ClientBuildInfo buildInfo)
#else
        internal unsafe Command BuildQueue(ref ClientBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
            if (stream.Data.Pointer.FreeSize >= sizeof(uint) + sizeof(CallbackIdentity) || buildInfo.Count == 0)
            {
                var nextCommand = LinkNext;
                uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
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
                        LinkNext = null;
                        return nextCommand;
                    }
                    ++buildInfo.FreeCount;
                    SetReturnQueue(CommandClientReturnTypeEnum.ClientBuildError, null);
                }
                else
                {
                    ++buildInfo.FreeCount;
                    SetReturnQueue(CommandClientReturnTypeEnum.ControllerMethodIndexError, null);
                }
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
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
            //if (Controller.Socket.TryPushBatch(this) == CommandPushStateEnum.Closed)
            if (Controller.Socket.TryPush(this) == CommandPushStateEnum.Closed)
            {
                ReturnType = CommandClientReturnTypeEnum.SocketClosed;
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
            switch (Controller.Socket.TryPush(this))
            {
                case CommandPushStateEnum.WaitCount: return true;
                case CommandPushStateEnum.Closed:
                    SetReturn(CommandClientReturnTypeEnum.SocketClosed, null);
                    return false;
            }
            return false;
        }
        /// <summary>
        /// Set the error call return type
        /// 设置错误调用返回类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal void SetReturn(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal void SetReturn(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            this.ReturnType = returnType;
            this.ErrorMessage = errorMessage;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                Callback(continuation);
            }
        }
        /// <summary>
        /// Set the error call return type
        /// 设置错误调用返回类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal void SetReturnQueue(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal void SetReturnQueue(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            this.ReturnType = returnType;
            this.ErrorMessage = errorMessage;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                Controller.AppendQueue(Method, continuation);
            }
        }
    }
}
