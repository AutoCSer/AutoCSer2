using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Client callback delegate with return parameter initial values
    /// 带返回参数初始值的客户端回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct CommandClientReturnValueParameterCallback<T>
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue<T>> Callback;
        /// <summary>
        /// The initial value of the return parameter
        /// 返回参数初始值
        /// </summary>
        internal readonly T ReturnValueParameter;
        /// <summary>
        /// Client callback delegate with return parameter initial values
        /// 带返回参数初始值的客户端回调委托
        /// </summary>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        /// <param name="returnValueParameter">The initial value of the return parameter
        /// 返回参数初始值</param>
        public CommandClientReturnValueParameterCallback(Action<CommandClientReturnValue<T>> callback, T returnValueParameter)
        {
            Callback = callback;
            ReturnValueParameter = returnValueParameter;
        }
    }
}
