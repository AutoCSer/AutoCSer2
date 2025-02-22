using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 异常调用按天统计信息客户端
    /// </summary>
    public abstract class ExceptionStatisticsDayClient
    {
        /// <summary>
        /// 异常调用统计信息节点客户端节点
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IExceptionStatisticsNodeClientNode> ExceptionNode;
        /// <summary>
        /// 超时调用统计信息节点客户端节点
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IExceptionStatisticsNodeClientNode> TimeoutNode;
        /// <summary>
        /// 节点日期标识
        /// </summary>
        public readonly uint Date;
        /// <summary>
        /// 异常调用统计信息客户端节点
        /// </summary>
        /// <param name="date">节点日期标识</param>
        /// <param name="exceptionNode">异常调用统计信息节点客户端节点</param>
        /// <param name="timeoutNode">超时调用统计信息节点客户端节点</param>
        protected ExceptionStatisticsDayClient(uint date, StreamPersistenceMemoryDatabaseClientNodeCache<IExceptionStatisticsNodeClientNode> exceptionNode, StreamPersistenceMemoryDatabaseClientNodeCache<IExceptionStatisticsNodeClientNode> timeoutNode)
        {
            Date = date;
            this.ExceptionNode = exceptionNode;
            this.TimeoutNode = timeoutNode;
        }
        /// <summary>
        /// 添加异常调用统计信息
        /// </summary>
        /// <param name="callData"></param>
        /// <param name="callTime"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private void append(CallData callData, DateTime callTime, Task<ResponseResult<IExceptionStatisticsNodeClientNode>> task)
        {
            if (task.IsCompleted) append(callData, callTime, task.Result);
            else appendAsync(callData, callTime, task).NotWait();
        }
        /// <summary>
        /// 添加异常调用统计信息
        /// </summary>
        /// <param name="callData"></param>
        /// <param name="callTime"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task appendAsync(CallData callData, DateTime callTime, Task<ResponseResult<IExceptionStatisticsNodeClientNode>> task)
        {
            append(callData, callTime, await task);
        }
        /// <summary>
        /// 添加异常调用统计信息
        /// </summary>
        /// <param name="callData"></param>
        /// <param name="callTime"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void append(CallData callData, DateTime callTime, ResponseResult<IExceptionStatisticsNodeClientNode> node)
        {
            if (node.IsSuccess) node.Value.notNull().Append(callData.CallType, callData.CallName, callTime);
        }
        /// <summary>
        /// 添加异常调用统计信息
        /// </summary>
        /// <param name="callData">实时调用信息</param>
        /// <param name="callTime">调用时间</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AppendException(CallData callData, DateTime callTime)
        {
            append(callData, callTime, ExceptionNode.GetSynchronousNode());
        }
        /// <summary>
        /// 添加超时调用统计信息
        /// </summary>
        /// <param name="callData">实时调用信息</param>
        /// <param name="callTime">调用时间</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AppendTimeoutNode(CallData callData, DateTime callTime)
        {
            append(callData, callTime, TimeoutNode.GetSynchronousNode());
        }
    }
}
