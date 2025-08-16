using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Method call type
    /// 方法调用类型
    /// </summary>
    public enum CallTypeEnum : byte
    {
        /// <summary>
        /// No input parameters, no return values
        /// 无输入参数，无返回值
        /// </summary>
        Call,
        /// <summary>
        /// There are input parameters but no return values
        /// 有输入参数，无返回值
        /// </summary>
        CallInput,
        /// <summary>
        /// There are no input parameters, but there is a return value
        /// 无输入参数，有返回值
        /// </summary>
        CallOutput,
        /// <summary>
        /// There are input parameters and return values
        /// 有输入参数，有返回值
        /// </summary>
        CallInputOutput,
        /// <summary>
        /// There are input parameters but no return values
        /// 有输入参数，无返回值
        /// </summary>
        SendOnly,
        /// <summary>
        /// There are no input parameters, but there is a return value (keep callback)
        /// 无输入参数，有返回值（持续回调）
        /// </summary>
        KeepCallback,
        /// <summary>
        /// There are input parameters and return values (keep callback)
        /// 有输入参数，有返回值（持续回调）
        /// </summary>
        InputKeepCallback,
        /// <summary>
        /// No input parameters, the return value of the two-stage callback
        /// 无输入参数，二阶段回调返回值
        /// </summary>
        TwoStageCallback,
        /// <summary>
        /// There are input parameters and the return value of the two-stage callback
        /// 有输入参数，二阶段回调返回值
        /// </summary>
        InputTwoStageCallback,

        /// <summary>
        /// There are no input parameters, but there is a return value (The server-side call type corresponds to the CallOutput on the client side)
        /// 无输入参数，有返回值（服务端调用类型，客户端对应 CallOutput）
        /// </summary>
        Callback,
        /// <summary>
        /// There are input parameters and return values (The server-side call type corresponds to CallInputOutput on the client side)
        /// 有输入参数，有返回值（服务端调用类型，客户端对应 CallInputOutput） 
        /// </summary>
        InputCallback,
        /// <summary>
        /// There are no input parameters, but there is a return value (keep callback) (The server-side call type corresponds to KeepCallback on the client side)
        /// 无输入参数，有返回值（持续回调）（服务端调用类型，客户端对应 KeepCallback）
        /// </summary>
        Enumerable,
        /// <summary>
        /// There are input parameters and return values (keep callback) (The server-side call type corresponds to InputKeepCallback on the client side)
        /// 有输入参数，有返回值（持续回调）（服务端调用类型，客户端对应 InputKeepCallback）
        /// </summary>
        InputEnumerable,
        /// <summary>
        /// Unknown call type, and the definition is illegal
        /// 未知调用类型，定义不合法
        /// </summary>
        Unknown,
    }
}
