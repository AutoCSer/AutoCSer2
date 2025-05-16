using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net.CommandServer
{
#if AOT
    /// <summary>
    /// 队列回调委托命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal class KeepCallbackOutputCommand<T, OT> : Net.KeepCallbackCommand
        where OT : struct
#else
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
    internal class KeepCallbackOutputCommand<T> : Net.KeepCallbackCommand
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
        /// 客户端回调委托
        /// </summary>
        private readonly CommandClientKeepCallback<T> callback;
#if AOT
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<T> callback, Func<OT, T> getReturnValue) : base(controller, methodIndex)
        {
            this.callback = callback;
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<T> callback, Func<OT, T> getReturnValue, OT outputParameter) : base(controller, methodIndex)
        {
            this.callback = callback;
            this.getReturnValue = getReturnValue;
            this.outputParameter = outputParameter;
        }
#else
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<T> callback) : base(controller, methodIndex)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="returnValue"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<T> callback, ref T returnValue) : base(controller, methodIndex)
        {
            this.callback = callback;
            this.returnValue = returnValue;
        }
#endif
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            callback.Error(returnType, null, this);
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
                callback.Error((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage, this);
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
                if (returnType == CommandClientReturnTypeEnum.Success) callback.Callback.notNull()(getReturnValue(outputParameter), this);
                else callback.Error(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage, this);
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
                if (returnType == CommandClientReturnTypeEnum.Success) callback.Callback.notNull()(outputParameter.ReturnValue, this);
                else callback.Error(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage, this);
            }
#endif
        }
        /// <summary>
        /// 取消保持回调（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            callback.Error(returnType == CommandClientReturnTypeEnum.Success ? CommandClientReturnTypeEnum.CancelKeepCallback : returnType, errorMessage, this);
        }
    }
#if AOT
    /// <summary>
    /// 保持回调命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal sealed class KeepCallbackOutputCommand<T, RT, OT> : KeepCallbackOutputCommand<RT, OT>
        where OT : struct
#else
    /// <summary>
    /// 回调委托命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class KeepCallbackOutputCommand<T, RT> : KeepCallbackOutputCommand<RT>
#endif
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
#if AOT
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<RT> callback, Func<OT, RT> getReturnValue, ref T inputParameter) : base(controller, methodIndex, callback, getReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<RT> callback, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter) : base(controller, methodIndex, callback, getReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#else
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<RT> callback, ref T inputParameter) : base(controller, methodIndex, callback)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal KeepCallbackOutputCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback<RT> callback, ref T inputParameter, ref RT returnValue) : base(controller, methodIndex, callback, ref returnValue)
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
            return BuildKeep(ref buildInfo, ref inputParameter);
        }
    }
}
