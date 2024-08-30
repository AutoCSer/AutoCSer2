using AutoCSer.CommandService.DistributedLock;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁管理器
    /// </summary>
    public abstract class DistributedLockManager
    {
        /// <summary>
        /// 当前允许并发数量
        /// </summary>
        private byte concurrent;
        /// <summary>
        /// 当前锁请求
        /// </summary>
        private Request request;
        /// <summary>
        /// 锁请求集合
        /// </summary>
        private LeftArray<Request> requests = new LeftArray<Request>(0);
        /// <summary>
        /// 等待锁请求首节点
        /// </summary>
        private Request waitRequestHead;
        /// <summary>
        /// 等待锁请求尾节点
        /// </summary>
        private Request waitRequestEnd;
        /// <summary>
        /// 分布式锁管理器
        /// </summary>
        /// <param name="concurrent">允许并发数量</param>
        public DistributedLockManager(byte concurrent = 1)
        {
            this.concurrent = Math.Max(concurrent, (byte)1);
        }
        /// <summary>
        /// 判断请求是否存在
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal bool IsRequest(Request request)
        {
            if (object.ReferenceEquals(this.request, request)) return true;
            if (this.request != null)
            {
                foreach (Request nextRequest in requests)
                {
                    if (object.ReferenceEquals(nextRequest, request)) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 锁请求超时释放处理
        /// </summary>
        /// <param name="request"></param>
        internal void Release(Request request)
        {
            if (object.ReferenceEquals(this.request, request))
            {
                requests.TryPop(out this.request);
                next();
                if (this.request == null) remove();
                return;
            }
            if (this.request != null && requests.Remove(request)) next();
        }
        /// <summary>
        /// 移除当前锁管理器
        /// </summary>
        protected abstract void remove();
        /// <summary>
        /// 处理下一个等待请求
        /// </summary>
        private void next()
        {
            ++concurrent;
            if (waitRequestHead != null)
            {
                Request request = waitRequestHead;
                do
                {
                    if (request.WaitCallback())
                    {
                        waitRequestHead = request.WaitNext;
                        if (waitRequestHead == null) waitRequestEnd = null;
                        append(request);
                        return;
                    }
                    request = request.WaitNext;
                }
                while (request != null);
                waitRequestHead = waitRequestEnd = null;
            }
        }
        /// <summary>
        /// 添加锁请求
        /// </summary>
        /// <param name="request"></param>
        private void append(Request request)
        {
            if (this.request == null) this.request = request;
            else requests.Add(request);
            --concurrent;
        }
        /// <summary>
        /// 添加等待请求
        /// </summary>
        /// <param name="request"></param>
        private void appendWait(Request request)
        {
            if (waitRequestHead == null) waitRequestHead = waitRequestEnd = request;
            else
            {
                waitRequestEnd.WaitNext = request;
                waitRequestEnd = request;
            }
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        internal void Enter(DistributedLockController controller, int releaseSeconds, ref CommandServerCallback<long> callback)
        {
            Request request = new Request(controller, this, releaseSeconds, callback);
            if (concurrent != 0)
            {
                append(request);
                callback = null;
                request.Callback();
            }
            else
            {
                appendWait(request);
                callback = null;
            }
        }
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        internal long TryEnter(DistributedLockController controller, int releaseSeconds)
        {
            if (concurrent == 0) return 0;
            Request request = new Request(controller, this, releaseSeconds);
            append(request);
            return request.GetRequestID();
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        internal void Enter(DistributedLockController controller, int releaseSeconds, int timeoutSeconds, ref CommandServerCallback<long> callback)
        {
            Request request = new Request(controller, this, releaseSeconds, callback);
            if (concurrent != 0)
            {
                append(request);
                callback = null;
                request.Callback();
            }
            else
            {
                AutoCSer.Threading.SecondTimer.TaskArray.Append(request.WaitTimeout, Math.Max(timeoutSeconds, 1), SecondTimerTaskThreadModeEnum.Synchronous);
                appendWait(request);
                callback = null;
            }
        }
        /// <summary>
        /// 锁请求断线重连
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        internal long EnterAgain(DistributedLockController controller, long requestID, int releaseSeconds)
        {
            if (Keep(requestID)) return requestID;
            if (concurrent != 0)
            {
                Request request = new Request(controller, this, releaseSeconds);
                append(request);
                request.SetRequestID(requestID);
                return requestID;
            }
            return 0;
        }
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        internal bool Keep(long requestID)
        {
            if (request != null)
            {
                if (request.RequestID == requestID)
                {
                    request.Keep();
                    return true;
                }
                foreach (Request nextRequest in requests)
                {
                    if (nextRequest.RequestID == requestID)
                    {
                        nextRequest.Keep();
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="requestID">锁请求标识</param>
        internal void Release(long requestID)
        {
            if (request == null) return;
            if (request.RequestID == requestID)
            {
                requests.TryPop(out request);
                next();
                return;
            }
            if (requests.RemoveToEnd(p => p.RequestID == requestID)) next();
        }
    }
    /// <summary>
    /// 分布式锁管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DistributedLockManager<T> : DistributedLockManager where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁管理器
        /// </summary>
        private readonly IDistributedLockManager<T> manager;
        /// <summary>
        /// 锁关键字
        /// </summary>
        private readonly T key;
        /// <summary>
        /// 分布式锁管理器
        /// </summary>
        /// <param name="manager">分布式锁管理器</param>
        /// <param name="key">锁关键字</param>
        /// <param name="concurrent">允许并发数量</param>
        public DistributedLockManager(IDistributedLockManager<T> manager, T key, byte concurrent = 1) : base(concurrent)
        {
            this.manager = manager;
            this.key = key;
        }
        /// <summary>
        /// 移除当前锁管理器
        /// </summary>
        protected override void remove()
        {
            manager.Remove(key);
        }
    }
}
