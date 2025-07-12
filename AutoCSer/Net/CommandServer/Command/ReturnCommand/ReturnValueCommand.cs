using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net.CommandServer
{
#if AOT
    /// <summary>
    /// The return value command
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ReturnValueCommand<T> : AutoCSer.Net.ReturnCommand<T>
    {
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        internal ReturnValueCommand(CommandClientDefaultController controller) : base(controller) { }
    }
    /// <summary>
    /// The return value command
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal class ReturnValueCommand<T, OT> : AutoCSer.Net.ReturnCommand<T>
        where OT : struct
#else
    /// <summary>
    /// The return value command
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ReturnValueCommand<T> : AutoCSer.Net.ReturnCommand<T>
#endif
    {
        /// <summary>
        /// The initial return value
        /// 初始返回值
        /// </summary>
#if AOT
        private OT outputParameter;
        /// <summary>
        /// The delegate that gets the return value
        /// 获取返回值委托
        /// </summary>
        private readonly Func<OT, T> getReturnValue;
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue) : base(controller, methodIndex)
        {
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue, OT outputParameter) : base(controller, methodIndex)
        {
            this.getReturnValue = getReturnValue;
            this.outputParameter = outputParameter;
        }
#else
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="returnValue"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, ref T returnValue) : base(controller, methodIndex)
        {
            this.returnValue = returnValue;
        }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        internal ReturnValueCommand(CommandClientDefaultController controller) : base(controller) { }
#endif
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
                    SetReturn(CommandClientReturnTypeEnum.ClientBuildError, null);
                }
                else
                {
                    ++buildInfo.FreeCount;
                    SetReturn(CommandClientReturnTypeEnum.ControllerMethodIndexError, null);
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
            SetReturn(returnType, null);
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
            if (data.Length == int.MinValue)
            {
                SetReturn((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
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
                //if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleDeserializeParamter))
                //{
                //    returnType = CommandClientReturnTypeEnum.Success;
                //    return ClientReceiveErrorTypeEnum.Success;
                //}
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (returnType == CommandClientReturnTypeEnum.Success) SetReturn(getReturnValue(outputParameter));
                else SetReturn(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage);
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
                if (returnType == CommandClientReturnTypeEnum.Success) SetReturn(outputParameter.ReturnValue);
                else SetReturn(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage);
            }
#endif
        }
    }
#if AOT
    /// <summary>
        /// The return value command
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal sealed class ReturnValueCommand<T, RT, OT> : ReturnValueCommand<RT, OT>
        where OT : struct
#else
    /// <summary>
    /// The return value command
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class ReturnValueCommand<T, RT> : ReturnValueCommand<RT>
#endif
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        private T inputParameter;
#if AOT
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter) : base(controller, methodIndex, getReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter) : base(controller, methodIndex, getReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#else
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal ReturnValueCommand(CommandClientController controller, int methodIndex, ref T inputParameter, ref RT returnValue) : base(controller, methodIndex, ref returnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#endif
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
