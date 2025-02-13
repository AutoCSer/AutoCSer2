using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// 超时任务消息节点（用于分布式事务数据一致性检查）
    /// </summary>
    /// <typeparam name="T">任务消息数据类型</typeparam>
    public abstract class TimeoutMessageNode<T> : MethodParameterCreatorNode<ITimeoutMessageNode<T>, TimeoutMessageData<T>>, ITimeoutMessageNode<T>, ISnapshot<TimeoutMessageData<T>>
    {
        /// <summary>
        /// 任务超时执行时钟周期
        /// </summary>
        private readonly long timeoutTicks;
        /// <summary>
        /// 任务集合
        /// </summary>
        private readonly Dictionary<long, TimeoutMessage<T>> tasks;
        /// <summary>
        /// 超时检查定时
        /// </summary>
        private readonly CheckTimer<T> checkTimer;
        /// <summary>
        /// 任务链表首节点
        /// </summary>
#if NetStandard21
        private TimeoutMessage<T>? taskHead;
#else
        private TimeoutMessage<T> taskHead;
#endif
        /// <summary>
        /// 任务链表尾节点
        /// </summary>
#if NetStandard21
        private TimeoutMessage<T>? taskEnd;
#else
        private TimeoutMessage<T> taskEnd;
#endif
        /// <summary>
        /// 当前分配任务标识
        /// </summary>
        private long currentIdentity;
        /// <summary>
        /// 是否正在检查超时
        /// </summary>
        private int isCheckTimeout;
        /// <summary>
        /// 执行失败任务数量
        /// </summary>
        private int failedCount;
        /// <summary>
        /// 超时任务消息节点（用于分布式事务数据一致性检查）
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        protected TimeoutMessageNode(int timeoutSeconds)
        {
            timeoutTicks = TimeSpan.TicksPerSecond * Math.Max(timeoutSeconds + 1, 2);
            tasks = DictionaryCreator.CreateLong<TimeoutMessage<T>>();
            checkTimer = new CheckTimer<T>();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override ITimeoutMessageNode<T>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override ITimeoutMessageNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (TimeoutMessage<T> task in tasks.Values)
            {
                if (task.Data.CheckLoadRunTask()) task.RunTask(this).NotWait();
                if (task.Data.IsFailed) ++failedCount;
            }
            checkTimer.Set(this);
            return this;
        }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            checkTimer.Cancel();
        }
        /// <summary>
        /// 数据库服务关闭操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceDisposable()
        {
            checkTimer.Cancel();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return tasks.Count + 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<TimeoutMessageData<T>> GetSnapshotResult(TimeoutMessageData<T>[] snapshotArray, object customObject)
        {
            SnapshotResult<TimeoutMessageData<T>> result = new SnapshotResult<TimeoutMessageData<T>>(0);
            foreach (TimeoutMessage<T> task in tasks.Values) task.Data.IsLinkSnapshot = false;
            for (var head = taskHead; head != null; head = head.LinkNext)
            {
                if (head.Data.CheckLinkSnapshot()) result.Add(snapshotArray, head.Data);
            }
            foreach (TimeoutMessage<T> task in tasks.Values)
            {
                if (!task.Data.IsLinkSnapshot) result.Add(snapshotArray, task.Data);
            }
            TimeoutMessageData<T> identityData = default(TimeoutMessageData<T>);
            identityData.Set(currentIdentity);
            result.Add(snapshotArray, identityData);
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotAdd(TimeoutMessageData<T> value)
        {
            if (value.Timeout != DateTime.MaxValue)
            {
                TimeoutMessage<T> task = new TimeoutMessage<T>(ref value);
                tasks.Add(value.Identity, task);
                if (value.IsLinkSnapshot) appendLink(task);
            }
            else currentIdentity = value.Identity;
        }
        /// <summary>
        /// 任务添加到队列
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void appendLink(TimeoutMessage<T> task)
        {
            if (taskEnd != null) taskEnd.LinkNext = task;
            else taskHead = task;
            taskEnd = task;
        }
        /// <summary>
        /// 获取任务总数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return tasks.Count;
        }
        /// <summary>
        /// 获取执行失败任务数量
        /// </summary>
        /// <returns></returns>
        public int GetFailedCount()
        {
            return failedCount;
        }
        /// <summary>
        /// 添加任务 持久化前检查
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public ValueResult<long> AppendBeforePersistence(TimeoutMessage<T> task)
        {
            if (task != null)
            {
                task.Data.Timeout = AutoCSer.Threading.SecondTimer.UtcNow.AddTicks(timeoutTicks);
                return default(ValueResult<long>);
            }
            return 0;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns>任务标识，失败返回 0</returns>
        public long Append(TimeoutMessage<T> task)
        {
            tasks.Add(currentIdentity, task);
            task.Data.Identity = currentIdentity;
            appendLink(task);
            return currentIdentity++;
        }
        /// <summary>
        /// 触发任务执行
        /// </summary>
        /// <param name="identity">任务标识</param>
        public void RunTaskLoadPersistence(long identity)
        {
            var task = default(TimeoutMessage<T>);
            if (tasks.TryGetValue(identity, out task)) task.Data.IsRunTask = true;
        }
        /// <summary>
        /// 触发任务执行
        /// </summary>
        /// <param name="identity">任务标识</param>
        public void RunTask(long identity)
        {
            var task = default(TimeoutMessage<T>);
            if (tasks.TryGetValue(identity, out task) && task.Data.CheckRunTask()) task.RunTask(this).NotWait();
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns>是否执行成功</returns>
        public abstract Task<bool> RunTask(TimeoutMessage<T> task);
        /// <summary>
        /// 执行任务异常处理
        /// </summary>
        /// <param name="task"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual Task OnTaskException(TimeoutMessage<T> task, Exception exception) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isSuccess"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Completed(TimeoutMessage<T> task, bool isSuccess)
        {
            methodParameterCreator.Creator.Completed(task.Data.Identity, isSuccess);
        }
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="isSuccess"></param>
        public void Completed(long identity, bool isSuccess)
        {
            var task = default(TimeoutMessage<T>);
            if (isSuccess)
            {
                if (tasks.Remove(identity, out task)) failedCount -= task.Data.Cancel();
            }
            else
            {
                if (tasks.TryGetValue(identity, out task)) failedCount += task.Data.Failed();
            }
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="identity">任务标识</param>
        public void Cancel(long identity)
        {
            var task = default(TimeoutMessage<T>);
            if (tasks.Remove(identity, out task)) failedCount -= task.Data.Cancel();
        }
        /// <summary>
        /// 失败任务重试
        /// </summary>
        public void RetryFailed()
        {
            foreach (TimeoutMessage<T> task in tasks.Values)
            {
                if (task.Data.IsFailed) task.RunTask(this).NotWait();
            }
        }
        /// <summary>
        /// 消息超时检查
        /// </summary>
        internal void CheckTimeout()
        {
            if (taskHead != null && Interlocked.CompareExchange(ref isCheckTimeout, 1, 0) == 0)
            {
                bool isCallback = false;
                try
                {
                    StreamPersistenceMemoryDatabaseCallQueue.AddOnly(new TimeoutCallback<T>(this));
                    isCallback = true;
                }
                finally
                {
                    if (!isCallback) Interlocked.Exchange(ref isCheckTimeout, 0);
                }
            }
        }
        /// <summary>
        /// 超时检查
        /// </summary>
        internal void CheckTimeoutCallback()
        {
            try
            {
                while (taskHead != null)
                {
                    if (taskHead.Data.CheckTimeout())
                    {
                        if (taskHead.Data.Timeout > AutoCSer.Threading.SecondTimer.UtcNow) return;
                        taskHead.SetRunTask(this).NotWait();
                    }
                    taskHead = taskHead.LinkNext;
                }
                taskEnd = null;
            }
            finally { Interlocked.Exchange(ref isCheckTimeout, 0); }
        }
    }
}
