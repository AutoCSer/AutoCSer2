using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
#if AOT
    /// <summary>
    /// 队列回调委托命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal class CallbackQueueOutputCommand<T, OT> : Net.CallbackCommand
        where OT : struct
#else
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
    internal class CallbackQueueOutputCommand<T> : Net.CallbackCommand
#endif
    {
        /// <summary>
        /// 返回初始值
        /// </summary>
#if AOT
        private OT outputParameter;
        /// <summary>
        /// 获取返回值委托
        /// </summary>
        private readonly Func<OT, T> getReturnValue;
#else
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
#endif
        /// <summary>
        /// 客户端队列回调委托
        /// </summary>
#if NetStandard21
        private CommandClientCallbackQueueNode<T>? callback;
#else
        private CommandClientCallbackQueueNode<T> callback;
#endif
#if AOT
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<T> callback, Func<OT, T> getReturnValue) : base(controller, methodIndex)
        {
            this.callback = callback;
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<T> callback, Func<OT, T> getReturnValue, OT outputParameter) : base(controller, methodIndex)
        {
            this.callback = callback;
            this.getReturnValue = getReturnValue;
            this.outputParameter = outputParameter;
        }
#else
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<T> callback) : base(controller, methodIndex)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="returnValue"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<T> callback, ref T returnValue) : base(controller, methodIndex)
        {
            this.callback = callback;
            this.returnValue = returnValue;
        }
#endif
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
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
                        return LinkNext;
                    }
                    ++buildInfo.FreeCount;
                    appendQueue(CommandClientReturnTypeEnum.ClientBuildError, null);
                }
                else
                {
                    ++buildInfo.FreeCount;
                    appendQueue(CommandClientReturnTypeEnum.ControllerMethodIndexError, null);
                }
                return LinkNext;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// 添加到回调队列
        /// </summary>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void appendQueue(CommandClientReturnValue<T> returnValue)
        {
            var callback = this.callback;
            this.callback = null;
            if (callback != null)
            {
                callback.ReturnValue = returnValue;
                Controller.AppendQueue(Method, callback);
            }
        }
        /// <summary>
        /// 添加到回调队列
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void appendQueue(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        private void appendQueue(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            appendQueue(new CommandClientReturnValue<T>(returnType, errorMessage));
        }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            appendQueue(returnType, null);
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            if (data.Length == int.MinValue)
            {
                appendQueue((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
                return ClientReceiveErrorTypeEnum.Success;
            }
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Unknown;
#if AOT
            try
            {
                if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleDeserializeParamter))
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    return ClientReceiveErrorTypeEnum.Success;
                }
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (returnType == CommandClientReturnTypeEnum.Success) appendQueue(getReturnValue(outputParameter));
                else appendQueue(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage);
            }
#else
            ServerReturnValue<T> outputParameter = new ServerReturnValue<T>(returnValue);
            try
            {
                if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleDeserializeParamter))
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    return ClientReceiveErrorTypeEnum.Success;
                }
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (returnType == CommandClientReturnTypeEnum.Success) appendQueue(outputParameter.ReturnValue);
                else appendQueue(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage);
            }
#endif
        }
    }
#if AOT
    /// <summary>
    /// 回调委托命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal sealed class CallbackQueueOutputCommand<T, RT, OT> : CallbackQueueOutputCommand<RT, OT>
        where OT : struct
#else
    /// <summary>
    /// 回调委托命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class CallbackQueueOutputCommand<T, RT> : CallbackQueueOutputCommand<RT>
#endif
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
#if AOT
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<RT> callback, Func<OT, RT> getReturnValue, ref T inputParameter) : base(controller, methodIndex, callback, getReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<RT> callback, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter) : base(controller, methodIndex, callback, getReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#else
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<RT> callback, ref T inputParameter) : base(controller, methodIndex, callback)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal CallbackQueueOutputCommand(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<RT> callback, ref T inputParameter, ref RT returnValue) : base(controller, methodIndex, callback, ref returnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#endif
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
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
