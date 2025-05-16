using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端方法调用类型
    /// </summary>
    internal enum ServerMethodTypeEnum : byte
    {
        /// <summary>
        /// 未知，定义不合法
        /// </summary>
        Unknown,
        /// <summary>
        /// 过期方法
        /// </summary>
        VersionExpired,

        /// <summary>
        /// IO 线程同步调用，同步返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        Synchronous,
        /// <summary>
        /// IO 线程同步调用，不返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        SendOnly,
        /// <summary>
        /// IO 线程同步调用，回调返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        Callback,
        /// <summary>
        /// IO 线程同步调用，保持回调返回数据，支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        KeepCallback,
        /// <summary>
        /// IO 线程同步调用，保持回调计数返回数据，不支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        KeepCallbackCount,

        /// <summary>
        /// 队列任务调用，同步返回数据
        /// </summary>
        Queue,
        /// <summary>
        /// 队列任务调用，不返回数据
        /// </summary>
        SendOnlyQueue,
        /// <summary>
        /// 队列任务调用，回调返回数据
        /// </summary>
        CallbackQueue,
        /// <summary>
        /// 队列任务调用，保持回调返回数据，支持自动释放回调
        /// </summary>
        KeepCallbackQueue,
        /// <summary>
        /// 队列任务调用，保持回调计数返回数据，不支持自动释放回调
        /// </summary>
        KeepCallbackCountQueue,

        /// <summary>
        /// 支持并行并发读的队列任务调用，同步返回数据
        /// </summary>
        ConcurrencyReadQueue,
        /// <summary>
        /// 支持并行并发读的队列任务调用，不返回数据
        /// </summary>
        SendOnlyConcurrencyReadQueue,
        /// <summary>
        /// 支持并行并发读的队列任务调用，回调返回数据
        /// </summary>
        CallbackConcurrencyReadQueue,
        /// <summary>
        /// 支持并行并发读的队列任务调用，保持回调返回数据，支持自动释放回调
        /// </summary>
        KeepCallbackConcurrencyReadQueue,
        /// <summary>
        /// 支持并行并发读的队列任务调用，保持回调计数返回数据，不支持自动释放回调
        /// </summary>
        KeepCallbackCountConcurrencyReadQueue,

        /// <summary>
        /// 读写队列任务调用，同步返回数据
        /// </summary>
        ReadWriteQueue,
        /// <summary>
        /// 读写队列任务调用，不返回数据
        /// </summary>
        SendOnlyReadWriteQueue,
        /// <summary>
        /// 读写队列任务调用，回调返回数据
        /// </summary>
        CallbackReadWriteQueue,
        /// <summary>
        /// 读写队列任务调用，保持回调返回数据，支持自动释放回调
        /// </summary>
        KeepCallbackReadWriteQueue,
        /// <summary>
        /// 读写队列任务调用，保持回调计数返回数据，不支持自动释放回调
        /// </summary>
        KeepCallbackCountReadWriteQueue,

        /// <summary>
        /// await Task 调用（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        Task,
        /// <summary>
        /// await Task 调用，不返回数据（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        SendOnlyTask,
        /// <summary>
        /// await Task 调用，回调返回数据（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        CallbackTask,
        /// <summary>
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        KeepCallbackTask,
        /// <summary>
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        KeepCallbackCountTask,
        /// <summary>
        /// .NET Satndard 2.0 中集合自动转 KeepCallbackCountTask（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        EnumerableKeepCallbackCountTask,

        /// <summary>
        /// await Task 队列调用
        /// </summary>
        TaskQueue,
        /// <summary>
        /// await Task 队列调用，不返回数据
        /// </summary>
        SendOnlyTaskQueue,
        /// <summary>
        /// await Task 队列调用，回调返回数据
        /// </summary>
        CallbackTaskQueue,
        /// <summary>
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调
        /// </summary>
        KeepCallbackTaskQueue,
        /// <summary>
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调
        /// </summary>
        KeepCallbackCountTaskQueue,
        /// <summary>
        /// .NET Satndard 2.0 中集合自动转 KeepCallbackCountTaskQueue
        /// </summary>
        EnumerableKeepCallbackCountTaskQueue,

#if NetStandard21
        /// <summary>
        /// await IAsyncEnumerable 调用（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        AsyncEnumerableTask,
        /// <summary>
        /// await IAsyncEnumerable 调用
        /// </summary>
        AsyncEnumerableTaskQueue,
#endif
    }
}
