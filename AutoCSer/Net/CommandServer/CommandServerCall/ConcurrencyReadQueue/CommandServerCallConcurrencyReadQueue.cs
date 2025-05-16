using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端支持并行读的同步队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景）
    /// </summary>
    public sealed class CommandServerCallConcurrencyReadQueue : CommandServerCallConcurrencyReadWriteQueue
    {
        /// <summary>
        /// 空队列
        /// </summary>
        private CommandServerCallConcurrencyReadQueue() { }
        /// <summary>
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
        /// 空队列
        /// </summary>
        internal static new readonly CommandServerCallConcurrencyReadQueue Null = new CommandServerCallConcurrencyReadQueue();
    }
}
