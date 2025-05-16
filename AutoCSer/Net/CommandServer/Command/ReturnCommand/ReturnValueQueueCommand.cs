using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net.CommandServer
{
#if AOT
    /// <summary>
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal class ReturnValueQueueCommand<T, OT> : Net.ReturnQueueCommand<T>
        where OT : struct
#else
        /// <summary>
        /// 队列回调委托命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
    internal class ReturnValueQueueCommand<T> : Net.ReturnQueueCommand<T>
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
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue) : base(controller, methodIndex)
        {
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue, OT outputParameter) : base(controller, methodIndex)
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
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="returnValue"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, ref T returnValue) : base(controller, methodIndex)
        {
            this.returnValue = returnValue;
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
            return BuildQueue(ref buildInfo);
        }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            SetReturnQueue(returnType, null);
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
                SetReturnQueue((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
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
                if (returnType == CommandClientReturnTypeEnum.Success) SetReturnQueue(getReturnValue(outputParameter));
                else SetReturnQueue(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage);
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
                if (returnType == CommandClientReturnTypeEnum.Success) SetReturnQueue(outputParameter.ReturnValue);
                else SetReturnQueue(CommandClientReturnTypeEnum.ClientDeserializeError, Controller.Socket.ReceiveErrorMessage);
            }
#endif
        }
    }
#if AOT
    /// <summary>
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal sealed class ReturnValueQueueCommand<T, RT, OT> : ReturnValueQueueCommand<RT, OT>
        where OT : struct
#else
    /// <summary>
    /// 返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class ReturnValueQueueCommand<T, RT> : ReturnValueQueueCommand<RT>
#endif
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
#if AOT
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter) : base(controller, methodIndex, getReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter) : base(controller, methodIndex, getReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#else
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal ReturnValueQueueCommand(CommandClientController controller, int methodIndex, ref T inputParameter, ref RT returnValue) : base(controller, methodIndex, ref returnValue)
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
