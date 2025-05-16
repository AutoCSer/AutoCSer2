using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端同步读写队列
    /// </summary>
    public sealed class CommandServerCallReadQueue : CommandServerCallWriteQueue
    {
        /// <summary>
        /// 服务端同步读写队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
        /// <param name="maxConcurrency">最大读取操作并发数量</param>
#if NetStandard21
        internal CommandServerCallReadQueue(CommandListener server, CommandServerController? controller, int maxConcurrency) : base(server, controller, maxConcurrency)
#else
        internal CommandServerReadWriteQueue(CommandListener server, CommandServerController controller, int maxConcurrency) : base(server, controller, maxConcurrency)
#endif
        {
        }
        /// <summary>
        /// 添加长读操作任务节点，允许读取操作则同步执行任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void LongRead(ReadWriteQueueNode node)
        {
            node.Set(this, ReadWriteNodeTypeEnum.LongRead);
            longRead(node);
        }
        /// <summary>
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendRead(ReadWriteQueueNode node)
        {
            node.Set(this, ReadWriteNodeTypeEnum.Read);
            appendRead(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendRead(CommandServerCallReadQueue queue, CommandServerCallReadWriteQueueNode node)
        {
            queue.AppendRead(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum AppendReadIsDeserialize(CommandServerCallReadQueue queue, CommandServerCallReadWriteQueueNode node)
        {
            if (node.IsDeserialize)
            {
                queue.AppendRead(node);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
        /// <summary>
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendWrite(ReadWriteQueueNode node)
        {
            node.Set(this, ReadWriteNodeTypeEnum.Write);
            appendWrite(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendWrite(CommandServerCallReadQueue queue, CommandServerCallReadWriteQueueNode node)
        {
            queue.AppendWrite(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum AppendWriteIsDeserialize(CommandServerCallReadQueue queue, CommandServerCallReadWriteQueueNode node)
        {
            if (node.IsDeserialize)
            {
                queue.AppendWrite(node);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
    }
}
