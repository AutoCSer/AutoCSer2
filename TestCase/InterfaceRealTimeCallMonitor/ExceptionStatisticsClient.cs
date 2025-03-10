using AutoCSer.CommandService.InterfaceRealTimeCallMonitor;
using AutoCSer.Extensions;
using System;

namespace AutoCSer.TestCase.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 异常调用统计信息客户端节点
    /// </summary>
    internal sealed class ExceptionStatisticsClient : ExceptionStatisticsDayClient
    {
        /// <summary>
        /// 异常调用统计信息客户端节点
        /// </summary>
        /// <param name="date">节点日期标识</param>
        /// <param name="hexSuffix"></param>
        /// <param name="removeTime"></param>
        private ExceptionStatisticsClient(uint date, DateTime removeTime, string hexSuffix) : base(date
                  , ExceptionStatisticsCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IExceptionStatisticsNodeClientNode>(nameof(ExceptionNode) + hexSuffix, (index, key, nodeInfo) => client.ClientNode.CreateExceptionStatisticsNode(index, key, nodeInfo, removeTime, 10)))
                  , ExceptionStatisticsCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IExceptionStatisticsNodeClientNode>(nameof(TimeoutNode) + hexSuffix, (index, key, nodeInfo) => client.ClientNode.CreateExceptionStatisticsNode(index, key, nodeInfo, removeTime, 10))))
        {
        }
        /// <summary>
        /// 异常调用统计信息客户端节点
        /// </summary>
        /// <param name="date">节点日期标识</param>
        internal ExceptionStatisticsClient(uint date) : this(date, date.fromIntDate().AddDays(7), date.toHex())
        {
        }
        /// <summary>
        /// 异常调用统计信息客户端节点
        /// </summary>
        internal ExceptionStatisticsClient() : this(AutoCSer.Threading.SecondTimer.UtcNow.toIntDate()) { }
    }
}
