using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The result of the remote expression delegate call and the deserialization state
    /// 远程表达式委托调用结果与反序列化状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionFuncResult<T>
    {
        /// <summary>
        /// The result of the call delegate
        /// 委托调用结果
        /// </summary>
        public T Value;
        /// <summary>
        /// Server deserialization state
        /// 服务端反序列化状态
        /// </summary>
        public RemoteExpressionSerializeStateEnum State;
        /// <summary>
        /// The result of the remote expression delegate call
        /// 远程表达式委托调用结果
        /// </summary>
        /// <param name="value">The result of the call delegate
        /// 委托调用结果</param>
        internal RemoteExpressionFuncResult(T value)
        {
            Value = value;
            State = RemoteExpressionSerializeStateEnum.Success;
        }
        /// <summary>
        /// Whether the server deserialization was successful
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return State == RemoteExpressionSerializeStateEnum.Success; } }
    }
}
