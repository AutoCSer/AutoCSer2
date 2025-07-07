using AutoCSer.CommandService.DeployTask;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务节点
    /// </summary>
    public sealed class DeployTaskNode : MethodParameterCreatorNode<IDeployTaskNode, TaskData>, IDeployTaskNode, ISnapshot<TaskData>, IEnumerableSnapshot<long>
    {
        /// <summary>
        /// 未完成任务集合
        /// </summary>
        private readonly Dictionary<long, TaskBuilder> tasks;
        /// <summary>
        /// 自动移除已关闭任务时间
        /// </summary>
        private readonly TimeSpan removeClosedTimeSpan;
        /// <summary>
        /// 下一次执行自动移除时间
        /// </summary>
        private DateTime removeTime;
        /// <summary>
        /// 当前分配任务标识ID
        /// </summary>
        private long currentIdentity;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<long> IEnumerableSnapshot<long>.SnapshotEnumerable { get { return new SnapshotGetValue<long>(getCurrentIdentity); } }
        /// <summary>
        /// 发布任务节点
        /// </summary>
        /// <param name="removeClosedDays">自动移除已关闭任务时间天数</param>
        public DeployTaskNode(int removeClosedDays = 1)
        {
            removeClosedTimeSpan = new TimeSpan(Math.Max(removeClosedDays, 1), 0, 0, 0);
            tasks = DictionaryCreator.CreateLong<TaskBuilder>();
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public override IDeployTaskNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IDeployTaskNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            remove();
            foreach (TaskBuilder builder in tasks.Values) builder.Loaded();
            return null;
        }
        /// <summary>
        /// 自动移除已关闭任务
        /// </summary>
        private void remove()
        {
            LeftArray<long> removeIdentitys = new LeftArray<long>(0);
            foreach (TaskBuilder builder in tasks.Values)
            {
                if (builder.Data.IsRemove(removeClosedTimeSpan)) removeIdentitys.Add(builder.Data.Identity);
            }
            int count = removeIdentitys.Length;
            if (count != 0)
            {
                foreach (long identity in removeIdentitys.Array)
                {
                    tasks.Remove(identity);
                    if (--count == 0) break;
                }
            }
            removeTime = AutoCSer.Threading.SecondTimer.UtcNow.AddDays(1);
        }
        /// <summary>
        /// Processing operations after node removal
        /// 节点移除后的处理操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            foreach (TaskBuilder builder in tasks.Values) builder.Remove(AutoCSer.Threading.SecondTimer.UtcNow);
            tasks.Clear();
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
        public SnapshotResult<TaskData> GetSnapshotResult(TaskData[] snapshotArray, object customObject)
        {
            SnapshotResult<TaskData> result = new SnapshotResult<TaskData>(snapshotArray.Length);
            foreach (TaskBuilder builder in tasks.Values) result.Add(snapshotArray, builder.Data);
            return result;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(TaskData value)
        {
            tasks.Add(value.Identity, new TaskBuilder(this, value));
        }
        /// <summary>
        /// 获取当前分配任务标识ID
        /// </summary>
        /// <returns></returns>
        private long getCurrentIdentity()
        {
            return currentIdentity;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetIdentity(long value)
        {
            currentIdentity = value;
        }
        /// <summary>
        /// Create a task
        /// 创建任务
        /// </summary>
        /// <returns>Task identity
        /// 任务标识ID</returns>
        public long Create()
        {
            long identity = ++currentIdentity;
            TaskBuilder builder = new TaskBuilder(this, identity);
            tasks.Add(identity, builder);
            if (removeTime >= AutoCSer.Threading.SecondTimer.UtcNow) remove();
            return identity;
        }
        /// <summary>
        /// Start the task (Initialize and load the persistent data)
        /// 启动任务（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="startTime">The Utc time for running the task
        /// 运行任务 Utc 时间</param>
        /// <returns></returns>
        public OperationStateEnum StartLoadPersistence(long identity, DateTime startTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) builder.Data.StartLoadPersistence(startTime);
            return OperationStateEnum.Unknown;
        }
        /// <summary>
        /// Start the task
        /// 启动任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="startTime">The Utc time for running the task
        /// 运行任务 Utc 时间</param>
        /// <returns></returns>
        public OperationStateEnum Start(long identity, DateTime startTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) return builder.Start(startTime);
            return OperationStateEnum.NotFoundTask;
        }
        /// <summary>
        /// Add a sub-task
        /// 添加子任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="task">The task of executing a program
        /// 执行程序任务</param>
        /// <returns></returns>
        public OperationStateEnum AppendStepTask(long identity, StepTaskData task)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) return builder.Data.Append(task);
            return OperationStateEnum.NotFoundTask;
        }
        /// <summary>
        /// Get the callback log of the status change of the published task
        /// 获取发布任务状态变更回调日志
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="callback">Callback delegate for task state changes
        /// 任务状态变更回调委托</param>
        public void GetLog(long identity, MethodKeepCallback<DeployTaskLog> callback)
        {
            bool isCallback = false;
            try
            {
                var builder = default(TaskBuilder);
                if (tasks.TryGetValue(identity, out builder)) builder.Append(callback);
                else callback.CallbackCancelKeep(new DeployTaskLog(identity, OperationStateEnum.NotFoundTask));
                isCallback = true;
            }
            finally
            {
                if (!isCallback) callback.CallbackCancelKeep(new DeployTaskLog(identity, OperationStateEnum.Exception));
            }
        }
        /// <summary>
        /// Remove completed or un-started task (Initialize and load the persistent data)
        /// 移除已结束或者未开始任务（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        /// <returns></returns>
        public OperationStateEnum RemoveLoadPersistence(long identity, DateTime closeTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder))
            {
                OperationStateEnum state = builder.Data.RemoveLoadPersistence(closeTime);
                if (state == OperationStateEnum.Success) tasks.Remove(identity);
                return state;
            }
            return OperationStateEnum.NotFoundTask;
        }
        /// <summary>
        /// Remove completed or un-started task
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        /// <returns></returns>
        public OperationStateEnum Remove(long identity, DateTime closeTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder))
            {
                OperationStateEnum state = builder.Remove(closeTime);
                if (state == OperationStateEnum.Success) tasks.Remove(identity);
                return state;
            }
            return OperationStateEnum.NotFoundTask;
        }
        /// <summary>
        /// Close the task
        /// 关闭任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        public void Close(long identity, DateTime closeTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) builder.Data.Close(closeTime);
        }
    }
}
