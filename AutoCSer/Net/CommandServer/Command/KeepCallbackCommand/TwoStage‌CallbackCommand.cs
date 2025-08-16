using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net.CommandServer
{
#if AOT
    /// <summary>
    /// Two-stage callback command
    /// 两阶段回调命令
    /// </summary>
    /// <typeparam name="T">第一阶段返回数据类型</typeparam>
    /// <typeparam name="OT">第一阶段输出数据类型</typeparam>
    /// <typeparam name="KT">保持回调阶段返回数据类型</typeparam>
    /// <typeparam name="KOT">保持回调阶段输出数据类型</typeparam>
    internal class TwoStage‌CallbackCommand<T, OT, KT, KOT> : KeepCallbackOutputCommand<KT, KOT>
        where OT : struct
        where KOT : struct
#else
    /// <summary>
    /// Two-stage callback command
    /// 两阶段回调命令
    /// </summary>
    /// <typeparam name="T">第一阶段返回数据类型</typeparam>
    /// <typeparam name="KT">保持回调阶段返回数据类型</typeparam>
    internal class TwoStage‌CallbackCommand<T, KT>  : KeepCallbackOutputCommand<KT>
#endif
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        private CommandClientCallback<T> callback;
#if AOT
        /// <summary>
        /// The initial return value
        /// 初始返回值
        /// </summary>
        private OT outputParameter;
        /// <summary>
        /// The delegate that gets the return value
        /// 获取返回值委托
        /// </summary>
        private readonly Func<OT, T> getReturnValue;
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<T> callback, Func<OT, T> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue) : base(controller, methodIndex, keepCallback, getKeepReturnValue)
        {
            this.callback = callback;
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<T> callback, Func<OT, T> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue, KOT outputParameter) : base(controller, methodIndex, keepCallback, getKeepReturnValue, outputParameter)
        {
            this.callback = callback;
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getOutputParameter"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<T> callback, Func<T, OT> getOutputParameter, Func<OT, T> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue) : base(controller, methodIndex, keepCallback, getKeepReturnValue)
        {
            this.callback = callback.Callback;
            this.outputParameter = getOutputParameter(callback.ReturnValueParameter);
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getOutputParameter"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<T> callback, Func<T, OT> getOutputParameter, Func<OT, T> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue, KOT outputParameter) : base(controller, methodIndex, keepCallback, getKeepReturnValue, outputParameter)
        {
            this.callback = callback.Callback;
            this.outputParameter = getOutputParameter(callback.ReturnValueParameter);
            this.getReturnValue = getReturnValue;
        }
#else
        /// <summary>
        /// The initial return value
        /// 初始返回值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<T> callback, CommandClientKeepCallback<KT> keepCallback) : base(controller, methodIndex, keepCallback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <param name="returnValue"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<T> callback, CommandClientKeepCallback<KT> keepCallback, ref KT returnValue) : base(controller, methodIndex, keepCallback, ref returnValue)
        {
            this.callback = callback;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<T> callback, CommandClientKeepCallback<KT> keepCallback) : base(controller, methodIndex, keepCallback)
        {
            this.callback = callback.Callback;
            this.returnValue = callback.ReturnValueParameter;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <param name="returnValue"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<T> callback, CommandClientKeepCallback<KT> keepCallback, ref KT returnValue) : base(controller, methodIndex, keepCallback, ref returnValue)
        {
            this.callback = callback.Callback;
            this.returnValue = callback.ReturnValueParameter;
        }
#endif
        /// <summary>
        /// Error handling for generating the input data of the request command
        /// 生成请求命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            try
            {
                callback.Callback(returnType, null);
            }
            finally { base.OnBuildError(returnType); }
        }
        /// <summary>
        /// 接收数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        private new void onReceiveError(CommandClientReturnTypeEnum returnType)
        {
            try
            {
                callback.Callback(returnType, Controller.Socket.ReceiveErrorMessage);
            }
            finally
            {
                base.onReceiveError(returnType);
            }
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
            if ((Controller.Socket.CallbackIdentity.Index & (uint)CallbackFlagsEnum.TwoStageCallback) == 0)
            {
                IsReceiveKeepData = true;
                return base.OnReceive(ref data);
            }
            if (!IsReceiveKeepData)
            {
                IsReceiveKeepData = true;
                if (data.Length == int.MinValue)
                {
                    onReceiveError((CommandClientReturnTypeEnum)(byte)data.Start);
                    return ClientReceiveErrorTypeEnum.Success;
                }
                CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Unknown;
#if AOT
            try
            {
                if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleSerializeTwoStage‌ReturnValue))
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    return ClientReceiveErrorTypeEnum.Success;
                }
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (returnType == CommandClientReturnTypeEnum.Success) callback.Callback(getReturnValue(outputParameter));
                else onReceiveError(CommandClientReturnTypeEnum.ClientDeserializeError);
            }
#else
                ServerReturnValue<T> outputParameter = new ServerReturnValue<T>(returnValue);
                try
                {
                    if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleSerializeTwoStage‌ReturnValue))
                    {
                        returnType = CommandClientReturnTypeEnum.Success;
                        return ClientReceiveErrorTypeEnum.Success;
                    }
                    Method.DeserializeError(Controller);
                    return ClientReceiveErrorTypeEnum.Success;
                }
                finally
                {
                    if (returnType == CommandClientReturnTypeEnum.Success) callback.Callback(outputParameter.ReturnValue);
                    else onReceiveError(CommandClientReturnTypeEnum.ClientDeserializeError);
                }
#endif
            }
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// Cancel the hold callback (Note that since it is a synchronous call by the IO thread receiving data, if there is a blockage, please open a new thread task to handle it)
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
            try
            {
                callback.Callback(returnType, errorMessage);
            }
            finally { base.CancelKeepCallback(returnType, errorMessage); }
        }
    }
#if AOT
    /// <summary>
    /// Two-stage callback command
    /// 两阶段回调命令
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    /// <typeparam name="RT">第一阶段返回数据类型</typeparam>
    /// <typeparam name="OT">第一阶段输出数据类型</typeparam>
    /// <typeparam name="KT">保持回调阶段返回数据类型</typeparam>
    /// <typeparam name="KOT">保持回调阶段输出数据类型</typeparam>
    internal sealed class TwoStage‌CallbackCommand<T, RT, OT, KT, KOT> : TwoStage‌CallbackCommand<RT, OT, KT, KOT>
        where OT : struct
        where KOT : struct
#else
    /// <summary>
    /// Two-stage callback command
    /// 两阶段回调命令
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    /// <typeparam name="RT">第一阶段返回数据类型</typeparam>
    /// <typeparam name="KT">保持回调阶段返回数据类型</typeparam>
    internal sealed class TwoStage‌CallbackCommand<T, RT, KT> : TwoStage‌CallbackCommand<RT, KT>
#endif
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        private T inputParameter;
#if AOT
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<RT> callback, Func<OT, RT> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue, ref T inputParameter) : base(controller, methodIndex, callback, getReturnValue, keepCallback, getKeepReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<RT> callback, Func<OT, RT> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue, ref T inputParameter, KOT outputParameter) : base(controller, methodIndex, callback, getReturnValue, keepCallback, getKeepReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getOutputParameter"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<RT> callback, Func<RT, OT> getOutputParameter, Func<OT, RT> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue, ref T inputParameter) : base(controller, methodIndex, callback, getOutputParameter, getReturnValue, keepCallback, getKeepReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getOutputParameter"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getKeepReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<RT> callback, Func<RT, OT> getOutputParameter, Func<OT, RT> getReturnValue, CommandClientKeepCallback<KT> keepCallback, Func<KOT, KT> getKeepReturnValue, ref T inputParameter, KOT outputParameter) : base(controller, methodIndex, callback, getOutputParameter, getReturnValue, keepCallback, getKeepReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#else
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<RT> callback, CommandClientKeepCallback<KT> keepCallback, ref T inputParameter) : base(controller, methodIndex, callback, keepCallback)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientCallback<RT> callback, CommandClientKeepCallback<KT> keepCallback, ref T inputParameter, ref KT returnValue) : base(controller, methodIndex, callback, keepCallback, ref returnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<RT> callback, CommandClientKeepCallback<KT> keepCallback, ref T inputParameter) : base(controller, methodIndex, callback, keepCallback)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal TwoStage‌CallbackCommand(CommandClientController controller, int methodIndex, CommandClientReturnValueParameterCallback<RT> callback, CommandClientKeepCallback<KT> keepCallback, ref T inputParameter, ref KT returnValue) : base(controller, methodIndex, callback, keepCallback, ref returnValue)
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
            return BuildKeep(ref buildInfo, ref inputParameter);
        }
    }
}
