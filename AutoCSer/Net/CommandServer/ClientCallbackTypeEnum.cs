using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The client's await awaits the return value callback thread mode
    /// 客户端 await 等待返回值回调线程模式
    /// </summary>
    public enum ClientCallbackTypeEnum : byte
    {
        /// <summary>
        /// By default, if it is an I/O thread, the callback operation will call Task.Run to prevent blocking the I/O thread. If it is confirmed that the subsequent operation does not have Synchronous blocking, it is recommended to use the synchronous call mode synchronous to avoid unnecessary thread context switching
        /// 默认值，默认如果是 IO 线程则回调操作调用 Task.Run 防止阻塞 IO 线程，如果确认后续操作不存在同步阻塞推荐采用同步调用模式 Synchronous 避免不必要的线程上下文切换
        /// </summary>
        CheckRunTask,
        /// <summary>
        /// If there is a synchronization callback for the IO thread and subsequent synchronization blocking occurs, the default CheckRunTask mode should be adopted to prevent the IO thread from being blocked and potentially causing a deadlock
        /// IO 线程同步回调，后续存在同步阻塞的情况应该采用默认的 CheckRunTask 模式防止 IO 线程被阻塞可能造成死锁
        /// </summary>
        Synchronous,
        /// <summary>
        /// The callback operation calls Task.Run
        /// 回调操作调用 Task.Run
        /// </summary>
        RunTask,
        /// <summary>
        /// The AutoCSer.Threading.ThreadPool.TinyBackground thread pool mode is suitable for scenarios with low concurrency; otherwise, it may result in a large number of threads being started
        /// AutoCSer.Threading.ThreadPool.TinyBackground 线程池模式，适合并发度低的场景，否则可能造成启动大量线程
        /// </summary>
        TinyBackground,
        /// <summary>
        /// The default thread pool mode of the system
        /// 系统默认线程池模式
        /// </summary>
        ThreadPool,
    }
}
