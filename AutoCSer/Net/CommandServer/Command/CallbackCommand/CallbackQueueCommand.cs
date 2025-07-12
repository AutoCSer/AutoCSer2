using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 队列回调委托命令
    /// </summary>
    internal class CallbackQueueCommand : Net.CallbackCommand
    {
        /// <summary>
        /// 客户端队列回调委托
        /// </summary>
#if NetStandard21
        private CommandClientCallbackQueueNode? callback;
#else
        private CommandClientCallbackQueueNode callback;
#endif
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal CallbackQueueCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode callback) : base(controller, methodIndex)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal CallbackQueueCommand(CommandClientDefaultController controller, int methodIndex, CommandClientCallbackQueueNode callback) : base(controller, methodIndex)
        {
            this.callback = callback;
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
                        LinkNext = null;
                        return nextCommand;
                    }
                    ++buildInfo.FreeCount;
                    appendQueue(CommandClientReturnTypeEnum.ClientBuildError, null);
                }
                else
                {
                    ++buildInfo.FreeCount;
                    appendQueue(CommandClientReturnTypeEnum.ControllerMethodIndexError, null);
                }
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// Add to the callback queue
        /// 添加到回调队列
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">Error message</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void appendQueue(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        private void appendQueue(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            var callback = this.callback;
            this.callback = null;
            if (callback != null)
            {
                callback.ReturnType = returnType;
                callback.ErrorMessage = errorMessage;
                Controller.AppendQueue(Method, callback);
            }
        }
        /// <summary>
        /// Error handling for generating the input data of the request command
        /// 生成请求命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            appendQueue(returnType, null);
        }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            appendQueue((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
            return ClientReceiveErrorTypeEnum.Success;
        }
    }
    /// <summary>
    /// 队列回调委托命令
    /// </summary>
    internal sealed class CallbackQueueCommand<T> : CallbackQueueCommand
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        internal CallbackQueueCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode callback, ref T inputParameter) : base(controller, methodIndex, callback)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            return Build(ref buildInfo, ref inputParameter);
        }
    }
}
