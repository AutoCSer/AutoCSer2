using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// A synchronous queue on the server side that supports parallel reading (mainly used in scenarios where in-memory database nodes support parallel reading when obtaining persistent data)
    /// 服务端支持并行读的同步队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景）
    /// </summary>
    public sealed class CommandServerCallConcurrencyReadQueue : CommandServerCallConcurrencyReadWriteQueue
    {
        /// <summary>
        /// Empty queue
        /// </summary>
        private CommandServerCallConcurrencyReadQueue() { }
        /// <summary>
        /// A synchronous queue on the server side that supports parallel reading (mainly used in scenarios where in-memory database nodes support parallel reading when obtaining persistent data)
        /// 服务端支持并行读的同步队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
#if NetStandard21
        internal CommandServerCallConcurrencyReadQueue(CommandListener server, CommandServerController? controller) : base(server, controller)
#else
        internal CommandServerCallConcurrencyReadQueue(CommandListener server, CommandServerController controller) : base(server, controller)
#endif
        {
        }

        /// <summary>
        /// Empty queue
        /// </summary>
        internal static new readonly CommandServerCallConcurrencyReadQueue Null = new CommandServerCallConcurrencyReadQueue();
    }
}
