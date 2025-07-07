using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// A read operation queue for synchronous read and write operations on the server side (mainly used in scenarios where in-memory database nodes support parallel reading when obtaining persistent data, and can also be used in scenarios where multiple threads support concurrent reading. It is not suitable for scenarios with high write operation frequencies)
    /// 服务端同步读写的读操作队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景，也可用于支持多线程并发读取的场景，不适合写操作频率高的需求）
    /// </summary>
    public sealed class CommandServerCallReadQueue : CommandServerCallWriteQueue
    {
        /// <summary>
        /// Empty queue
        /// </summary>
        private CommandServerCallReadQueue() { }
        /// <summary>
        /// A read operation queue for synchronous read and write operations on the server side (mainly used in scenarios where in-memory database nodes support parallel reading when obtaining persistent data, and can also be used in scenarios where multiple threads support concurrent reading. It is not suitable for scenarios with high write operation frequencies)
        /// 服务端同步读写的读操作队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景，也可用于支持多线程并发读取的场景，不适合写操作频率高的需求）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
        /// <param name="maxConcurrency">The maximum concurrent number of read operations, if less than or equal to 0, indicates the number of processors minus the set value (for example, if the number of processors is 4 and the concurrent number is set to -1, then the concurrent number of reads is 4 -1 = 3)
        /// 最大读取操作并发数量，小于等于 0 表示处理器数量减去设置值（比如处理器数量为 4，并发数量设置为 -1，则读取并发数量为 4 - 1 = 3）</param>
#if NetStandard21
        internal CommandServerCallReadQueue(CommandListener server, CommandServerController? controller, int maxConcurrency) : base(server, controller, maxConcurrency)
#else
        internal CommandServerCallReadQueue(CommandListener server, CommandServerController controller, int maxConcurrency) : base(server, controller, maxConcurrency)
#endif
        {
        }

        /// <summary>
        /// Empty queue
        /// </summary>
        internal static new readonly CommandServerCallReadQueue Null = new CommandServerCallReadQueue();
    }
}
