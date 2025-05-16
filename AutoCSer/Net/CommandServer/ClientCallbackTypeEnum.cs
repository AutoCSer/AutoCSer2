using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端 await 等待返回值回调线程模式
    /// </summary>
    public enum ClientCallbackTypeEnum : byte
    {
        /// <summary>
        /// 默认如果是 IO 线程则回调操作调用 Task.Run 防止阻塞 IO 线程，如果确认后续操作不存在同步阻塞推荐采用同步调用模式 Synchronous 避免不必要的线程上下文切换
        /// </summary>
        CheckRunTask,
        /// <summary>
        /// IO 线程同步回调，后续存在同步阻塞的情况应该采用默认的 CheckRunTask 模式防止 IO 线程被阻塞可能造成死锁
        /// </summary>
        Synchronous,
        /// <summary>
        /// 回调操作调用 Task.Run
        /// </summary>
        RunTask,
        /// <summary>
        /// AutoCSer.Threading.ThreadPool.TinyBackground 线程池模式，适合并发度低的场景，否则可能造成启动大量线程
        /// </summary>
        TinyBackground,
        /// <summary>
        /// 系统线程池模式
        /// </summary>
        ThreadPool,
    }
}
