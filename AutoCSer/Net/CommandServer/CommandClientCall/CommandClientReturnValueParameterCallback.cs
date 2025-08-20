using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

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
        /// <summary>
        /// Client callback delegate with return parameter initial values
        /// 带返回参数初始值的客户端回调委托
        /// </summary>
        /// <param name="callback">Client callback delegate with return parameter initial values
        /// 带返回参数初始值的客户端回调委托</param>
        public CommandClientReturnValueParameterCallback(CommandClientReturnValueParameterCallback<T> callback)
        {
            Callback = callback.Callback;
            ReturnValueParameter = callback.ReturnValueParameter;
        }

        /// <summary>
        /// Client callback delegate with return parameter initial values
        /// 带返回参数初始值的客户端回调委托
        /// </summary>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        /// <param name="returnValueParameter">The initial value of the return parameter
        /// 返回参数初始值</param>
        /// <returns>Client callback delegate with return parameter initial values
        /// 带返回参数初始值的客户端回调委托</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientReturnValueParameterCallback<T> Create(Action<T> callback, T returnValueParameter)
        {
            return new CommandClientReturnValueParameterCallback<T>(new ClientReturnValueCallback<T>(callback), returnValueParameter);
        }
    }
}
