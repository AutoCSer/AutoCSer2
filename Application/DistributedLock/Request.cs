using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 分布式锁请求
    /// </summary>
    internal sealed class Request : AutoCSer.Threading.SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 分布式锁服务端
        /// </summary>
        private readonly DistributedLockController controller;
        /// <summary>
        /// 分布式锁管理器
        /// </summary>
        internal readonly DistributedLockManager LockManager;
        /// <summary>
        /// 锁请求回调
        /// </summary>
        private readonly CommandServerCallback<long> callback;
        /// <summary>
        /// 自动释放锁超时秒数，用于客户端掉线没有释放锁的情况
        /// </summary>
        private readonly int releaseSeconds;
        /// <summary>
        /// 是否已经超时
        /// </summary>
        private bool isTimeout;
        /// <summary>
        /// 是否已经申请到锁
        /// </summary>
        private bool isLock;
        /// <summary>
        /// 请求标识ID
        /// </summary>
        internal long RequestID;
        /// <summary>
        /// 心跳设置超时秒计数
        /// </summary>
        private long keepTimeoutSeconds;
        /// <summary>
        /// 等待链表下一个请求
        /// </summary>
        internal Request WaitNext;
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        /// <param name="lockManager">分布式锁管理器</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        public Request(DistributedLockController controller, DistributedLockManager lockManager, int releaseSeconds)
            : base(AutoCSer.Threading.SecondTimer.TaskArray, SecondTimerTaskThreadModeEnum.Synchronous, SecondTimerKeepModeEnum.After)
        {
            this.controller = controller;
            this.LockManager = lockManager;
            this.releaseSeconds = Math.Min(Math.Max(releaseSeconds, 0), int.MaxValue - 1) + 1;
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        /// <param name="lockManager">分布式锁管理器</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        public Request(DistributedLockController controller, DistributedLockManager lockManager, int releaseSeconds, CommandServerCallback<long> callback)
            : this(controller, lockManager, releaseSeconds)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 锁请求回调
        /// </summary>
        /// <returns></returns>
        internal bool Callback()
        {
            if (callback.Callback(RequestID = controller.IdentityGenerator.GetNext()))
            {
                TryAppendTaskArray(releaseSeconds);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加超时任务并返回请求标识ID
        /// </summary>
        /// <returns></returns>
        internal long GetRequestID()
        {
            RequestID = controller.IdentityGenerator.GetNext();
            TryAppendTaskArray(releaseSeconds);
            return RequestID;
        }
        /// <summary>
        /// 断线重连
        /// </summary>
        /// <param name="requestID"></param>
        internal void SetRequestID(long requestID)
        {
            RequestID = requestID;
            TryAppendTaskArray(releaseSeconds);
        }
        /// <summary>
        /// 获取下一个超时秒计数
        /// </summary>
        /// <returns></returns>
        protected override long getNextTimeoutSeconds()
        {
            long keepTimeoutSeconds = this.keepTimeoutSeconds;
            if (keepTimeoutSeconds == 0) return 0;
            this.keepTimeoutSeconds = 0;
            return keepTimeoutSeconds;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        protected internal override void OnTimer()
        {
            if (keepTimeoutSeconds == 0) controller.Controller.CallQueue.AddOnly(new RequestTimeout(this, RequestTimeoutTypeEnum.Release));
        }
        /// <summary>
        /// 等待超时处理
        /// </summary>
        internal void WaitTimeout()
        {
            if (!isLock) controller.Controller.CallQueue.AddOnly(new RequestTimeout(this, RequestTimeoutTypeEnum.Wait));
        }
        /// <summary>
        /// 超时处理
        /// </summary>
        /// <param name="type">请求超时处理类型</param>
        internal void Timeout(RequestTimeoutTypeEnum type)
        {
            switch (type)
            {
                case RequestTimeoutTypeEnum.Release: LockManager.Release(this); break;
                case RequestTimeoutTypeEnum.Wait:
                    if (!isLock)
                    {
                        isTimeout = true;
                        callback.Callback(0);
                    }
                    break;
            }
        }
        /// <summary>
        /// 等待锁请求回调
        /// </summary>
        /// <returns></returns>
        internal bool WaitCallback()
        {
            if (!isTimeout && Callback()) return isLock = true;
            return false;
        }
        /// <summary>
        /// 保持心跳
        /// </summary>
        internal void Keep()
        {
            keepTimeoutSeconds = SecondTimer.GetCurrentSeconds + releaseSeconds;
        }
    }
}
