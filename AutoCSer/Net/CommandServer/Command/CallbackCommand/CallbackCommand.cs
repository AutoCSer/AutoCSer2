using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 回调委托命令
    /// </summary>
    internal class CallbackCommand : Net.CallbackCommand
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        private CommandClientCallback callback;
        /// <summary>
        /// 回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback callback) : base(controller, methodIndex)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        internal CallbackCommand(CommandClientDefaultController controller) : base(controller) { }
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
                    callback.Callback(CommandClientReturnTypeEnum.ClientBuildError, null);
                }
                else
                {
                    ++buildInfo.FreeCount;
                    callback.Callback(CommandClientReturnTypeEnum.ControllerMethodIndexError, null);
                }
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// Error handling for generating the input data of the request command
        /// 生成请求命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            callback.Callback(returnType, null);
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
            callback.Callback((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
            return ClientReceiveErrorTypeEnum.Success;
        }
    }
    /// <summary>
    /// 回调委托命令
    /// </summary>
    internal sealed class CallbackCommand<T> : CallbackCommand
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// 回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        internal CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback callback, ref T inputParameter) : base(controller, methodIndex, callback)
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
