using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 异常调用统计信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class ExceptionStatistics
    {
        /// <summary>
        /// 最后添加的调用时间集合
        /// </summary>
        private DateTime[] callTimes;
        /// <summary>
        /// 异常调用次数
        /// </summary>
        private int count;
        /// <summary>
        /// 异常调用次数
        /// </summary>
        public int Count { get { return count; } }
        /// <summary>
        /// 当前添加调用时间索引位置
        /// </summary>
        private int callTimeIndex;
        /// <summary>
        /// 最后添加的调用时间集合
        /// </summary>
        public IEnumerable<DateTime> CallTimes
        {
            get
            {
                if (callTimes.Length != 0)
                {
                    int index = callTimeIndex;
                    do
                    {
                        DateTime callTime = callTimes[index];
                        if (callTime != default(DateTime)) yield return callTime;
                        if (++index == callTimes.Length) index = 0;
                    }
                    while (index != callTimeIndex);
                }
            }
        }
        /// <summary>
        /// 异常调用统计信息
        /// </summary>
        private ExceptionStatistics()
        {
#if NetStandard21
            callTimes = EmptyArray<DateTime>.Array;
#endif
        }
        /// <summary>
        /// 异常调用统计信息
        /// </summary>
        /// <param name="node">异常调用统计信息节点</param>
        /// <param name="callTime">Exception call time
        /// 异常调用时间</param>
        internal ExceptionStatistics(ExceptionStatisticsNode node, DateTime callTime)
        {
            if (node.CallTimeCount > 0)
            {
                callTimes = new DateTime[node.CallTimeCount];
                Append(callTime);
            }
            else
            {
                count = 1;
                callTimes = EmptyArray<DateTime>.Array;
            }
        }
        /// <summary>
        /// Add exception call time
        /// 添加异常调用时间
        /// </summary>
        /// <param name="callTime">Exception call time
        /// 异常调用时间</param>
        internal void Append(DateTime callTime)
        {
            ++count;
            if (callTimes.Length != 0)
            {
                callTimes[callTimeIndex] = callTime;
                if (++callTimeIndex == callTimes.Length) callTimeIndex = 0;
            }
        }
    }
}
