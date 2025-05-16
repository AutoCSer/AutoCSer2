using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端套接字发送数据线程类型
    /// </summary>
    public enum CommandServerSocketBuildOutputThreadEnum : byte
    {
        /// <summary>
        /// 队列模式，适合预期并发连接数量为 1 或者大量并发连接的场景
        /// </summary>
        Queue,
        /// <summary>
        /// 线程并发，适合预期并发连接数量不为 1 并且低于 CPU 线程数量的场景，可以充分利用 CPU 多线程并行处理
        /// </summary>
        Thread,
        /// <summary>
        /// 纯同步输出或者低频输出场景可以设置为 false 以避免输出线程调度，否则可能造成流程性阻塞对系统并发能力造成巨大影响
        /// </summary>
        Synchronous
    }
}
