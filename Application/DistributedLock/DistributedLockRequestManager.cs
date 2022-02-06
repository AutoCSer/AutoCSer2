using AutoCSer.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 锁请求链表管理，用于断线重连
    /// </summary>
    public sealed class DistributedLockRequestManager
    {
        /// <summary>
        /// 分布式锁客户端套接字事件
        /// </summary>
        private readonly IDistributedLockClientSocketEvent<int> client;
        /// <summary>
        /// 锁请求链表访问锁
        /// </summary>
        private readonly object linkLock = new object();
        /// <summary>
        /// 锁请求链表尾节点
        /// </summary>
        private IDistributedLockRequest linkEnd;
        /// <summary>
        /// 锁请求数量
        /// </summary>
        private int count;
        /// <summary>
        /// 锁请求队列管理
        /// </summary>
        /// <param name="client">分布式锁客户端套接字事件</param>
        public DistributedLockRequestManager(IDistributedLockClientSocketEvent<int> client)
        {
            this.client = client;
        }
        /// <summary>
        /// 添加锁请求
        /// </summary>
        /// <param name="request"></param>
        public void Append(IDistributedLockRequest request)
        {
            if (request == null) return;
            Monitor.Enter(linkLock);
            if (request.IsLock)
            {
                if (count != 0)
                {
                    request.PreviousRequest = linkEnd;
                    linkEnd.NextRequest = request;
                }
                linkEnd = request;
                ++count;
            }
            Monitor.Exit(linkLock);
        }
        /// <summary>
        /// 删除锁请求
        /// </summary>
        /// <param name="request"></param>
        public void Remove(IDistributedLockRequest request) 
        {
            if (request == null) return;
            Monitor.Enter(linkLock);
            if (request == linkEnd) request.RemoveEnd();
            else request.Remove();
            --count;
            Monitor.Exit(linkLock);
        }
        /// <summary>
        /// 断线重新申请
        /// </summary>
        public void EnterAgain()
        {
            if (count != 0)
            {
                AutoCSer.LeftArray<IDistributedLockRequest> requests = new LeftArray<IDistributedLockRequest>(count + Math.Min(AutoCSer.Common.CpuCacheBlockObjectCount, count));
                Monitor.Enter(linkLock);
                try
                {
                    while (linkEnd != null)
                    {
                        linkEnd = linkEnd.RemoveAgain(ref requests);
                        --count;
                    }
                    count = 0;
                }
                finally
                {
                    Monitor.Exit(linkLock);
                    if (requests.Count != 0) CatchTask.AddIgnoreException(enterAgain(requests));
                }
            }
        }
        /// <summary>
        /// 断线重新申请
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        private static async Task enterAgain(AutoCSer.LeftArray<IDistributedLockRequest> requests)
        {
            do
            {
                try
                {
                    foreach (IDistributedLockRequest request in requests.PopAll()) await request.EnterAgain();
                }
                catch { }
            }
            while (requests.Count != 0);
        }
    }
}
