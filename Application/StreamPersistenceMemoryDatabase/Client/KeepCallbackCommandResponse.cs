using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 持续回调委托
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    internal sealed class KeepCallbackCommandResponse<T>
    {
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> callback;
        /// <summary>
        /// 持续回调委托
        /// </summary>
        /// <param name="callback">回调委托</param>
        internal KeepCallbackCommandResponse(Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        internal void Callback(CommandClientReturnValue<KeepCallbackResponseParameter> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                if (result.Value.State == CallStateEnum.Success)
                {
                    var value = result.Value.DeserializeValue;
                    if (value != null) callback(KeepCallbackResponseDeserializeValue<T>.GetValue(value), command);
                    else callback(AutoCSer.Net.CommandClientReturnTypeEnum.ClientDeserializeError, command);
                }
                else callback(result.Value.State, command);
            }
            else callback(new ResponseResult<T>(result.ReturnType, result.ErrorMessage), command);
        }
    }
}
