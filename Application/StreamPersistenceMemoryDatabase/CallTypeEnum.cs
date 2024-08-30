using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 方法调用类型
    /// </summary>
    public enum CallTypeEnum : byte
    {
        /// <summary>
        /// 无输入无返回
        /// </summary>
        Call,
        /// <summary>
        /// 有输入无返回
        /// </summary>
        CallInput,
        /// <summary>
        /// 无输入有返回
        /// </summary>
        CallOutput,
        /// <summary>
        /// 有输入有返回
        /// </summary>
        CallInputOutput,
        /// <summary>
        /// 有输入无返回
        /// </summary>
        SendOnly,
        /// <summary>
        /// 无输入有返回（持续回调）
        /// </summary>
        KeepCallback,
        /// <summary>
        /// 有输入有返回（持续回调）
        /// </summary>
        InputKeepCallback,

        /// <summary>
        /// 无输入有返回（服务端调用类型，客户端对应 CallOutput）
        /// </summary>
        Callback,
        /// <summary>
        /// 有输入有返回（服务端调用类型，客户端对应 CallInputOutput）
        /// </summary>
        InputCallback,
        /// <summary>
        /// 无输入有返回（持续回调）（服务端调用类型，客户端对应 KeepCallback）
        /// </summary>
        Enumerable,
        /// <summary>
        /// 有输入有返回（持续回调）（服务端调用类型，客户端对应 InputKeepCallback）
        /// </summary>
        InputEnumerable,
        /// <summary>
        /// 未知，定义不合法
        /// </summary>
        Unknown,
    }
}
