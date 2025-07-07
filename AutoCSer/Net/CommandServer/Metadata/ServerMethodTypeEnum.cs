using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Server-side method call types
    /// 服务端方法调用类型
    /// </summary>
    internal enum ServerMethodTypeEnum : byte
    {
        /// <summary>
        /// Unknown type, the definition is illegal
        /// 未知类型，定义不合法
        /// </summary>
        Unknown,
        /// <summary>
        /// Expiration method
        /// 过期方法
        /// </summary>
        VersionExpired,

        /// <summary>
        /// The IO thread synchronously calls and synchronously returns data. (Note: Since it is a data receiving IO thread synchronous call, it is not suitable for tasks with blocking.)
        /// IO 线程同步调用，同步返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        Synchronous,
        /// <summary>
        /// The IO thread synchronous calls does not return data. (Note: Since it is a data receiving IO thread synchronous call, it is not suitable for tasks with blocking.)
        /// IO 线程同步调用，不返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        SendOnly,
        /// <summary>
        /// I/O thread synchronous calls, callback returns data (Note: Since it is a data receiving I/O thread synchronous call, it is not suitable for tasks with blocking.)
        /// IO 线程同步调用，回调返回数据（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        Callback,
        /// <summary>
        /// Synchronous calls of IO threads, keep the callback to return data, support automatic release of callbacks (Note that since it is a data receiving IO thread synchronous call, it is not suitable for tasks with blocking).
        /// IO 线程同步调用，保持回调返回数据，支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        KeepCallback,
        /// <summary>
        /// Synchronous calls of IO threads, keep the callback count to return data, automatic release of callbacks is not supported (Note that since it is a data receiving IO thread synchronous call, it is not suitable for tasks with blocking).
        /// IO 线程同步调用，保持回调计数返回数据，不支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合存在阻塞的任务）
        /// </summary>
        KeepCallbackCount,

        /// <summary>
        /// Queue task call, synchronously return data
        /// 队列任务调用，同步返回数据
        /// </summary>
        Queue,
        /// <summary>
        /// Queue task calls do not return data
        /// 队列任务调用，不返回数据
        /// </summary>
        SendOnlyQueue,
        /// <summary>
        /// Queue task calls, callback returns data
        /// 队列任务调用，回调返回数据
        /// </summary>
        CallbackQueue,
        /// <summary>
        /// Queue task calls, keep the callback to return data, support automatic release of callbacks
        /// 队列任务调用，保持回调返回数据，支持自动释放回调
        /// </summary>
        KeepCallbackQueue,
        /// <summary>
        /// Queue task calls, keep the callback count to return data, automatic release of callbacks is not supported
        /// 队列任务调用，保持回调计数返回数据，不支持自动释放回调
        /// </summary>
        KeepCallbackCountQueue,

        /// <summary>
        /// Queue task calls that support concurrent reading and synchronously return data
        /// 支持并发读的队列任务调用，同步返回数据
        /// </summary>
        ConcurrencyReadQueue,
        /// <summary>
        /// Queue task calls that support concurrent reading do not return data
        /// 支持并发读的队列任务调用，不返回数据
        /// </summary>
        SendOnlyConcurrencyReadQueue,
        /// <summary>
        /// Queue task calls that support concurrent reading return data through callbacks
        /// 支持并发读的队列任务调用，回调返回数据
        /// </summary>
        CallbackConcurrencyReadQueue,
        /// <summary>
        /// Queue task calls that support concurrent reading, keep the callback to return data, support automatic release of callbacks
        /// 支持并发读的队列任务调用，保持回调返回数据，支持自动释放回调
        /// </summary>
        KeepCallbackConcurrencyReadQueue,
        /// <summary>
        /// Queue task calls that support concurrent reading, keep the callback count to return data, automatic release of callbacks is not supported
        /// 支持并发读的队列任务调用，保持回调计数返回数据，不支持自动释放回调
        /// </summary>
        KeepCallbackCountConcurrencyReadQueue,

        /// <summary>
        /// Read/write queue task calls, synchronous return data
        /// 读写队列任务调用，同步返回数据
        /// </summary>
        ReadWriteQueue,
        /// <summary>
        /// The read-write queue task calls does not return data
        /// 读写队列任务调用，不返回数据
        /// </summary>
        SendOnlyReadWriteQueue,
        /// <summary>
        /// Read/write queue task calls, callback returns data
        /// 读写队列任务调用，回调返回数据
        /// </summary>
        CallbackReadWriteQueue,
        /// <summary>
        /// Read/write queue task calls, keep the callback to return data, support automatic release of callbacks
        /// 读写队列任务调用，保持回调返回数据，支持自动释放回调
        /// </summary>
        KeepCallbackReadWriteQueue,
        /// <summary>
        /// Read/write queue task calls, keep the callback count to return data, automatic release of callbacks is not supported
        /// 读写队列任务调用，保持回调计数返回数据，不支持自动释放回调
        /// </summary>
        KeepCallbackCountReadWriteQueue,

        /// <summary>
        /// await Task calls (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// await Task 调用（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        Task,
        /// <summary>
        /// The await Task calls does not return data (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// await Task 调用，不返回数据（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        SendOnlyTask,
        /// <summary>
        /// await Task calls, callback returns data (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// await Task 调用，回调返回数据（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        CallbackTask,
        /// <summary>
        /// It replaces IAsyncEnumerable in.NET Satndard 2.0, has cross-platform compatibility and supports automatic release of callbacks (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        KeepCallbackTask,
        /// <summary>
        /// It replaces IAsyncEnumerable in.NET Satndard 2.0, has cross-platform compatibility and supports automatic release of callbacks (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        KeepCallbackCountTask,
        /// <summary>
        /// In.NET Satndard 2.0, collections automatically convert to KeepCallbackCountTask (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// .NET Satndard 2.0 中集合自动转 KeepCallbackCountTask（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        EnumerableKeepCallbackCountTask,

        /// <summary>
        /// await Task queue calls
        /// await Task 队列调用
        /// </summary>
        TaskQueue,
        /// <summary>
        /// The await Task queue calls does not return data
        /// await Task 队列调用，不返回数据
        /// </summary>
        SendOnlyTaskQueue,
        /// <summary>
        /// The await Task queue calls returns data through the callback
        /// await Task 队列调用，回调返回数据
        /// </summary>
        CallbackTaskQueue,
        /// <summary>
        /// It replaces IAsyncEnumerable in.NET Satndard 2.0, has cross-platform compatibility and supports automatic release of callbacks
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调
        /// </summary>
        KeepCallbackTaskQueue,
        /// <summary>
        /// It replaces IAsyncEnumerable in.NET Satndard 2.0, has cross-platform compatibility and supports automatic release of callbacks
        /// .NET Satndard 2.0 中替代 IAsyncEnumerable，具有跨平台兼容性，支持自动释放回调
        /// </summary>
        KeepCallbackCountTaskQueue,
        /// <summary>
        /// In.NET Satndard 2.0, collections automatically convert to KeepCallbackCountTaskQueue
        /// .NET Satndard 2.0 中集合自动转 KeepCallbackCountTaskQueue
        /// </summary>
        EnumerableKeepCallbackCountTaskQueue,

#if NetStandard21
        /// <summary>
        /// The calls of await IAsyncEnumerable (Note that since it is a synchronous call by the IO thread receiving data, it is not suitable for tasks where there is blocking before the first asynchronous await. For complex tasks, Socket.IsClose should be judged before core computing to avoid unnecessary overhead)
        /// await IAsyncEnumerable 调用（注意，由于是接收数据 IO 线程同步调用，不适合第一个异步 await 之前存在阻塞的任务。对于复杂任务，在核心计算之前应判断 Socket.IsClose 以避免不必要开销）
        /// </summary>
        AsyncEnumerableTask,
        /// <summary>
        /// The calls of await IAsyncEnumerable
        /// await IAsyncEnumerable 调用
        /// </summary>
        AsyncEnumerableTaskQueue,
#endif
    }
}
