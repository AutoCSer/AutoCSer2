using AutoCSer.CommandService.DistributedLock;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 锁请求保持心跳对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DistributedLockKeepRequest<T> : AutoCSer.Threading.SecondTimerTaskArrayNode, IDisposable, IDistributedLockRequest
#if !DotNet45 && !NetStandard2
        , IAsyncDisposable
#endif
        where T : IEquatable<T>
    {
        /// <summary>
        /// 锁标识ID
        /// </summary>
        private long requestID;
        /// <summary>
        /// 分布式锁客户端套接字事件
        /// </summary>
        private readonly IDistributedLockClientSocketEvent<T> client;
        /// <summary>
        /// 锁关键字
        /// </summary>
        private readonly T key;
        /// <summary>
        /// 自动释放锁超时秒数，用于客户端掉线没有释放锁的情况
        /// </summary>
        private readonly int releaseSeconds;
        /// <summary>
        /// 锁请求对象
        /// </summary>
        /// <param name="client">分布式锁客户端套接字事件</param>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁标识ID</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        internal DistributedLockKeepRequest(IDistributedLockClientSocketEvent<T> client, T key, long requestID, int releaseSeconds, int keepSeconds)
            : base(AutoCSer.Threading.SecondTimer.TaskArray, Math.Max(keepSeconds, 1), AutoCSer.Threading.SecondTimerTaskThreadMode.AddCatchTask, AutoCSer.Threading.SecondTimerKeepMode.Before, Math.Max(keepSeconds, 1))
        {
            this.client = client;
            this.key = key;
            this.requestID = requestID;
            this.releaseSeconds = releaseSeconds;
            client.AppendRequest(this);
        }
        /// <summary>
        /// 启动心跳
        /// </summary>
        /// <returns></returns>
        internal async Task StartKeepAsync()
        {
            await TryAppendTaskArrayAsync();
        }
        /// <summary>
        /// 启动心跳
        /// </summary>
        internal void StartKeep()
        {
            TryAppendTaskArray(KeepSeconds);
        }
        /// <summary>
        /// 获取下一个超时秒计数
        /// </summary>
        /// <returns></returns>
        protected override long getNextTimeoutSeconds()
        {
            return requestID != 0 ? base.getNextTimeoutSeconds() : 0;
        }
        /// <summary>
        /// 定时心跳
        /// </summary>
        /// <returns></returns>
        protected override async Task OnTimerAsync()
        {
            long requestID = this.requestID;
            if (requestID != 0)
            {
                CommandClientReturnValue<bool> result = await client.DistributedLockClient.KeepAsync(key, requestID);
                if (result.IsSuccess && !result.Value)
                {
                    this.requestID = 0;
                    client.RemoveRequest(this);
                }
            }
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        public void Dispose()
        {
            long requestID = this.requestID;
            if (requestID != 0)
            {
                this.requestID = 0;
                client.RemoveRequest(this);
                client.DistributedLockClient.Release(key, requestID);
            }
        }
#if !DotNet45 && !NetStandard2
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await ReleaseAsync();
        }
#endif
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        public async Task ReleaseAsync()
        {
            long requestID = this.requestID;
            if (requestID != 0)
            {
                this.requestID = 0;
                client.RemoveRequest(this);
                await client.DistributedLockClient.ReleaseAsync(key, requestID);
            }
        }
        /// <summary>
        /// 锁客户端状态是否有效
        /// </summary>
        public bool IsLock { get { return requestID != 0; } }
        /// <summary>
        /// 锁请求队列节点
        /// </summary>
        private RequestLinkNode linkNode;
        /// <summary>
        /// 上一个请求节点
        /// </summary>
        IDistributedLockRequest IDistributedLockRequest.PreviousRequest { set { linkNode.PreviousRequest = value; } }
        /// <summary>
        /// 下一个请求节点
        /// </summary>
        IDistributedLockRequest IDistributedLockRequest.NextRequest { set { linkNode.NextRequest = value; } }
        /// <summary>
        /// 从链表中删除当前节点
        /// </summary>
        void IDistributedLockRequest.Remove()
        {
            linkNode.Remove();
        }
        /// <summary>
        /// 从链表中删除当前尾节点并返回下一个尾节点
        /// </summary>
        /// <returns>下一个尾节点</returns>
        IDistributedLockRequest IDistributedLockRequest.RemoveEnd()
        {
            return linkNode.RemoveEnd();
        }
        /// <summary>
        /// 从链表中删除当前尾节点并返回下一个尾节点
        /// </summary>
        /// <param name="requests">需要短线重连的锁请求集合</param>
        /// <returns>下一个尾节点</returns>
        IDistributedLockRequest IDistributedLockRequest.RemoveAgain(ref AutoCSer.LeftArray<IDistributedLockRequest> requests)
        {
            if (requestID != 0) requests.Add(this);
            return linkNode.RemoveEnd();
        }
        /// <summary>
        /// 断线重连
        /// </summary>
        /// <returns></returns>
        async Task IDistributedLockRequest.EnterAgain()
        {
            long requestID = this.requestID;
            if (requestID != 0) await client.DistributedLockClient.EnterAgain(requestID, key, releaseSeconds, (Action<CommandClientReturnValue<long>>)onEnterAgain);
        }
        /// <summary>
        /// 断线重连回调
        /// </summary>
        /// <param name="returnValue"></param>
        private void onEnterAgain(CommandClientReturnValue<long> returnValue)
        {
            try
            {
                if (requestID == 0) return;
                if (returnValue.IsSuccess)
                {
                    if (returnValue.Value == 0) requestID = 0;
                    else client.AppendRequest(this);
                }
            }
            finally
            {
                if (requestID == 0 && returnValue.IsSuccess && returnValue.Value != 0)
                {
                    client.DistributedLockClient.Release(key, returnValue.Value, (Action<CommandClientReturnValue>)null);
                }
            }
        }
    }
}
