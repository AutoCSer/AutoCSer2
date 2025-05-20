using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 返回值命令
    /// </summary>
    public abstract class BaseReturnCommand : Command, INotifyCompletion
    {
        /// <summary>
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// 返回类型
        /// </summary>
        internal CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// 错误信息
        /// </summary>
#if NetStandard21
        internal string? ErrorMessage;
#else
        internal string ErrorMessage;
#endif
        /// <summary>
        /// 返回值命令
        /// </summary>
        internal BaseReturnCommand() { }
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        internal BaseReturnCommand(CommandClientController controller) : base(controller) { }
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal BaseReturnCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
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
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 添加命令到发送队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push()
        {
            if (Controller.Socket.TryPush(this) == CommandPushStateEnum.Closed)
            {
                ReturnType = CommandClientReturnTypeEnum.SocketClosed;
                IsCompleted = true;
                continuation = Common.EmptyAction;
            }
        }
        /// <summary>
        /// 检查等待添加队列命令
        /// </summary>
        /// <returns>是否需要继续等待</returns>
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
        /// 设置返回类型
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
        /// 设置返回类型
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
