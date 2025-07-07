using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Client method call type
    /// 客户端方法调用类型
    /// </summary>
    internal enum ClientMethodTypeEnum : byte
    {
        /// <summary>
        /// Unknown, the method definition is illegal
        /// 未知，方法定义不合法
        /// </summary>
        Unknown,
        /// <summary>
        /// Block the calling thread synchronization return data, use is not recommended, including AutoCSer.Net.CommandClientReturnValue{T} and AutoCSer.Net.CommandClientReturnValue
        /// 阻塞调用线程同步返回数据，不推荐使用，包括 AutoCSer.Net.CommandClientReturnValue{T} 与 AutoCSer.Net.CommandClientReturnValue
        /// </summary>
        Synchronous,
        /// <summary>
        /// Only send data, failure without induction operation, including AutoCSer.Net.SendOnlyCommand
        /// 仅发送数据，失败操作无感应，包括 AutoCSer.Net.SendOnlyCommand
        /// </summary>
        SendOnly,

        /// <summary>
        /// The callback return data
        /// 回调返回数据
        /// </summary>
        Callback,
        /// <summary>
        /// Keep the callback returns data
        /// 保持回调返回数据
        /// </summary>
        KeepCallback,

        /// <summary>
        /// The queue task callback return data (because it is queue thread synchronization trigger callback, It can ensure the serial execution of callback operations, but cannot guarantee the serial execution of subsequent await operations. No synchronous blocking operations are allowed after the callback; otherwise, it will seriously affect the throughput performance of the callback queue and may even cause a queue scheduling deadlock in complex dependency scenarios)
        /// 队列任务回调返回数据（由于是队列线程同步触发回调，可以保证回调操作的串行执行，但不能保证后续 await 操作的串行执行；回调后续不允许存在同步阻塞操作，否则会严重影响回调队列的吞吐性能，甚至在复杂的依赖场景中可能造成队列调度死锁）
        /// </summary>
        CallbackQueue,
        /// <summary>
        /// The queue task keep the callback to returns data (because it is queue thread synchronization trigger callback, It can ensure the serial execution of callback operations, but cannot guarantee the serial execution of subsequent await operations. No synchronous blocking operations are allowed after the callback; otherwise, it will seriously affect the throughput performance of the callback queue and may even cause a queue scheduling deadlock in complex dependency scenarios)
        /// 队列任务保持回调返回数据（由于是队列线程同步触发回调，可以保证回调操作的串行执行，但不能保证后续 await 操作的串行执行；回调后续不允许存在同步阻塞操作，否则会严重影响回调队列的吞吐性能，甚至在复杂的依赖场景中可能造成队列调度死锁）
        /// </summary>
        KeepCallbackQueue,

        /// <summary>
        /// Support await calls, the default using Task. Run trigger await callback, including AutoCSer.Net.ReturnCommand {T} and AutoCSer.Net.ReturnCommand
        /// 支持 await 调用，默认采用 Task.Run 触发 await 回调，包括 AutoCSer.Net.ReturnCommand{T} 与 AutoCSer.Net.ReturnCommand
        /// </summary>
        ReturnValue,
        /// <summary>
        /// The transformation of ReturnValue, including System.Threading.Tasks.Task{T} and System.Threading.Tasks.Task
        /// ReturnValue 的变形，包括 System.Threading.Tasks.Task{T} 与 System.Threading.Tasks.Task
        /// </summary>
        Task,
        /// <summary>
        /// Support await calls, trigger await callback queue tasks, including AutoCSer.Net.ReturnQueueCommand{T} or AutoCSer.Net.ReturnQueueCommand (because it is queue thread synchronization trigger callback, It can ensure the serial execution of callback operations, but cannot guarantee the serial execution of subsequent await operations. No synchronous blocking operations are allowed after the callback; otherwise, it will seriously affect the throughput performance of the callback queue and may even cause a queue scheduling deadlock in complex dependency scenarios)
        /// 支持 await 调用，队列任务触发 await 回调，包括 AutoCSer.Net.ReturnQueueCommand{T} 或者 AutoCSer.Net.ReturnQueueCommand（由于是队列线程同步触发回调，可以保证回调操作的串行执行，但不能保证后续 await 操作的串行执行；回调后续不允许存在同步阻塞操作，否则会严重影响回调队列的吞吐性能，甚至在复杂的依赖场景中可能造成队列调度死锁）
        /// </summary>
        ReturnValueQueue,

        /// <summary>
        /// It is used to replace IAsyncEnumerable in .NET Satndard 2.0 and has cross-platform compatibility Including AutoCSer.Net.EnumeratorCommand{T} or AutoCSer.Net.EnumeratorCommand
        /// 用于在 .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，包括 AutoCSer.Net.EnumeratorCommand{T} 或者 AutoCSer.Net.EnumeratorCommand
        /// </summary>
        Enumerator,
        /// <summary>
        /// It is used to replace IAsyncEnumerable in .NET Satndard 2.0 and has cross-platform compatibility Including AutoCSer.Net.EnumeratorQueueCommand{T} or AutoCSer.Net.EnumeratorQueueCommand (because it is queue thread synchronization trigger callback, It can ensure the serial execution of callback operations, but cannot guarantee the serial execution of subsequent await operations. No synchronous blocking operations are allowed after the callback; otherwise, it will seriously affect the throughput performance of the callback queue and may even cause a queue scheduling deadlock in complex dependency scenarios)
        /// 用于在 .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，包括 AutoCSer.Net.EnumeratorQueueCommand{T} 或者 AutoCSer.Net.EnumeratorQueueCommand（由于是队列线程同步触发回调，可以保证回调操作的串行执行，但不能保证后续 await 操作的串行执行；回调后续不允许存在同步阻塞操作，否则会严重影响回调队列的吞吐性能，甚至在复杂的依赖场景中可能造成队列调度死锁）
        /// </summary>
        EnumeratorQueue,

#if NetStandard21
        /// <summary>
        /// The await IAsyncEnumerable call requires.NET Satndard version 2.1 or above
        /// await IAsyncEnumerable 调用，需要 .NET Satndard 2.1 及以上版本
        /// </summary>
        AsyncEnumerable,
#endif
    }
}
