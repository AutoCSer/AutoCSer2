using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端方法调用类型
    /// </summary>
    internal enum ClientMethodTypeEnum : byte
    {
        /// <summary>
        /// 未知，定义不合法
        /// </summary>
        Unknown,
        /// <summary>
        /// 同步调用，阻塞工作线程，不推荐使用
        /// </summary>
        Synchronous,
        /// <summary>
        /// 只发送数据，失败操作无感应
        /// </summary>
        SendOnly,

        /// <summary>
        /// IO 线程同步回调，回调返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        Callback,
        /// <summary>
        /// IO 线程同步回调，保持回调返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        KeepCallback,

        /// <summary>
        /// 队列任务回调，回调返回数据
        /// </summary>
        CallbackQueue,
        /// <summary>
        /// 队列任务回调，保持回调返回数据
        /// </summary>
        KeepCallbackQueue,

        /// <summary>
        /// await 同步调用，默认采用 Task.Run 触发 await 回调
        /// </summary>
        ReturnValue,
        /// <summary>
        /// 替代 ReturnValue
        /// </summary>
        Task,
        /// <summary>
        /// await 同步调用，队列任务触发 await 回调（注意，由于是队列线程同步触发回调，并不能保证队列执行后续操作，不适合存在阻塞的 await 后续操作）
        /// </summary>
        ReturnValueQueue,

        /// <summary>
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性（默认采用 Task.Run 触发 await 回调）
        /// </summary>
        Enumerator,
        /// <summary>
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性（注意，由于是队列线程同步触发回调，并不能保证队列执行后续操作，不适合存在阻塞的 await 后续操作）
        /// </summary>
        EnumeratorQueue,

#if NetStandard21
        /// <summary>
        /// await IAsyncEnumerable 调用，需要 .NET Satndard 2.1
        /// </summary>
        AsyncEnumerable,
#endif
    }
}
