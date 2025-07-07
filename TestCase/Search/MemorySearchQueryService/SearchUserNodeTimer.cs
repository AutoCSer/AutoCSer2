using AutoCSer.Threading;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 定时释放缓冲区
    /// </summary>
    internal sealed class SearchUserNodeTimer : AutoCSer.Threading.SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        private SearchUserNode node;
        /// <summary>
        /// 定时释放缓冲区
        /// </summary>
        /// <param name="checkSeconds">释放缓冲区间隔秒数</param>
        internal SearchUserNodeTimer(int checkSeconds) : base(AutoCSer.Threading.SecondTimer.TaskArray, SecondTimerTaskThreadModeEnum.Synchronous, SecondTimerKeepModeEnum.After)
        {
            KeepSeconds = checkSeconds;
        }
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected override void OnTimer()
        {
            node?.FreeCache();
        }
        /// <summary>
        /// 设置消息处理节点
        /// </summary>
        /// <param name="node"></param>
        internal void Set(SearchUserNode node)
        {
            if (KeepSeconds != 0)
            {
                this.node = node;
                TryAppendTaskArray(KeepSeconds);
            }
        }
        /// <summary>
        /// 取消定时
        /// </summary>
        internal void Cancel()
        {
            KeepSeconds = 0;
            node = null;
        }
    }
}
