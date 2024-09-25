using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 分布式锁请求超时释放处理
    /// </summary>
    internal sealed class RequestTimeout : QueueTaskNode
    {
        /// <summary>
        /// 分布式锁请求
        /// </summary>
        private readonly Request request;
        /// <summary>
        /// 请求超时处理类型
        /// </summary>
        private readonly RequestTimeoutTypeEnum type;
        /// <summary>
        /// 分布式锁请求超时释放处理
        /// </summary>
        /// <param name="request">分布式锁请求</param>
        /// <param name="type">请求超时处理类型</param>
        internal RequestTimeout(Request request, RequestTimeoutTypeEnum type)
        {
            this.request = request;
            this.type = type;
        }
        /// <summary>
        /// 分布式锁请求超时释放处理
        /// </summary>
        public override void RunTask()
        {
            request.Timeout(type);
        }
    }
}
