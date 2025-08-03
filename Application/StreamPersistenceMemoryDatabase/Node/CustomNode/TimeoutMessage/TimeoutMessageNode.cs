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
    /// <typeparam name="T">Task message data type
    /// 任务消息数据类型</typeparam>
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
        /// 获取执行任务消息数据回调集合
        /// </summary>
        private LeftArray<MethodKeepCallback<T>> callbacks;
        /// <summary>
        /// 超时任务消息节点（用于分布式事务数据一致性检查）
        /// </summary>
        /// <param name="timeoutSeconds">触发任务执行超时秒数</param>
        protected TimeoutMessageNode(int timeoutSeconds)
        {
            timeoutTicks = TimeSpan.TicksPerSecond * Math.Max(timeoutSeconds + 1, 2);
            callbacks.SetEmpty();
            tasks = DictionaryCreator.CreateLong<TimeoutMessage<T>>();
            checkTimer = new CheckTimer<T>();
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public override ITimeoutMessageNode<T>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override ITimeoutMessageNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (TimeoutMessage<T> task in tasks.Values)
            {
                if (task.Data.CheckLoadRunTask()) task.RunTask(this, TimeoutMessageRunTaskTypeEnum.Loaded).AutoCSerNotWait();
                if (task.Data.IsFailed) ++failedCount;
            }
            checkTimer.Set(this);
            return null;
        }
        /// <summary>
        /// Processing operations after node removal
        /// 节点移除后的处理操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            checkTimer.Cancel();
        }
        /// <summary>
        /// Database service shutdown operation
        /// 数据库服务关闭操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceDisposable()
        {
            checkTimer.Cancel();
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return tasks.Count + 1;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
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
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
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
        /// Get the total number of tasks
        /// 获取任务总数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return tasks.Count;
        }
        /// <summary>
        /// Get the number of failed tasks executed
        /// 获取执行失败任务数量
        /// </summary>
        /// <returns></returns>
        public int GetFailedCount()
        {
            return failedCount;
        }
        /// <summary>
        /// Add the task node (Check the input parameters before the persistence operation)
        /// 添加任务节点（持久化操作之前检查输入参数）
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
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Task identifier. Return 0 upon failure
        /// 任务标识，失败返回 0</returns>
        public long Append(TimeoutMessage<T> task)
        {
            tasks.Add(currentIdentity, task);
            task.Data.Identity = currentIdentity;
            appendLink(task);
            return currentIdentity++;
        }
        /// <summary>
        /// Add immediate execution tasks (Check the input parameters before the persistence operation)
        /// 添加立即执行任务（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        public bool AppendRunBeforePersistence(TimeoutMessage<T> task)
        {
            if (task != null)
            {
                task.Data.Timeout = AutoCSer.Threading.SecondTimer.UtcNow;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add immediate execution tasks (Initialize and load the persistent data)
        /// 添加立即执行任务（初始化加载持久化数据）
        /// </summary>
        /// <param name="task"></param>
        public void AppendRunLoadPersistence(TimeoutMessage<T> task)
        {
            tasks.Add(currentIdentity, task);
            task.Data.AppendRun(currentIdentity++);
        }
        /// <summary>
        /// Add immediate execution tasks
        /// 添加立即执行任务
        /// </summary>
        /// <param name="task"></param>
        public void AppendRun(TimeoutMessage<T> task)
        {
            tasks.Add(currentIdentity, task);
            task.Data.AppendRun(currentIdentity++);
            task.RunTask(this, TimeoutMessageRunTaskTypeEnum.ClientCall).AutoCSerNotWait();
        }
        /// <summary>
        /// Trigger task execution (Initialize and load the persistent data)
        /// 触发任务执行（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识</param>
        public void RunTaskLoadPersistence(long identity)
        {
            var task = default(TimeoutMessage<T>);
            if (tasks.TryGetValue(identity, out task)) task.Data.IsRunTask = true;
        }
        /// <summary>
        /// Trigger task execution
        /// 触发任务执行
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识</param>
        public void RunTask(long identity)
        {
            var task = default(TimeoutMessage<T>);
            if (tasks.TryGetValue(identity, out task) && task.Data.CheckRunTask()) task.RunTask(this, TimeoutMessageRunTaskTypeEnum.ClientCall).AutoCSerNotWait();
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns>是否执行成功</returns>
        public abstract Task<bool> RunTask(TimeoutMessage<T> task, TimeoutMessageRunTaskTypeEnum type);
        /// <summary>
        /// 执行任务异常处理
        /// </summary>
        /// <param name="task"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual Task OnTaskException(TimeoutMessage<T> task, Exception exception) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// Complete the completed task
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
        /// Cancel the task
        /// 取消任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识</param>
        public void Cancel(long identity)
        {
            var task = default(TimeoutMessage<T>);
            if (tasks.Remove(identity, out task)) failedCount -= task.Data.Cancel();
        }
        /// <summary>
        /// Failed task retry
        /// 失败任务重试
        /// </summary>
        public void RetryFailed()
        {
            foreach (TimeoutMessage<T> task in tasks.Values)
            {
                if (task.Data.IsFailed) task.RunTask(this, TimeoutMessageRunTaskTypeEnum.RetryFailed).AutoCSerNotWait();
            }
        }
        /// <summary>
        /// Get the execution task message data
        /// 获取执行任务消息数据
        /// </summary>
        /// <param name="callback"></param>
        public void GetRunTask(MethodKeepCallback<T> callback)
        {
            callbacks.Add(callback);
        }
        /// <summary>
        /// 获取执行任务消息数据回调
        /// </summary>
        /// <param name="data">执行任务消息数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void callback(T data)
        {
            StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(new TimeoutMessageCallback<T>(this, data));
        }
        /// <summary>
        /// 获取执行任务消息数据回调
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Callback(T data)
        {
            MethodKeepCallback<T>.Callback(ref callbacks, data);
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
                    StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(new TimeoutCallback<T>(this));
                    isCallback = true;
                }
                finally
                {
                    if (!isCallback) Interlocked.Exchange(ref isCheckTimeout, 0);
                }
            }
        }
        /// <summary>
        /// Timeout check
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
                        taskHead.Timeout(this).AutoCSerNotWait();
                    }
                    taskHead = taskHead.LinkNext;
                }
                taskEnd = null;
            }
            finally { Interlocked.Exchange(ref isCheckTimeout, 0); }
        }
    }
}
