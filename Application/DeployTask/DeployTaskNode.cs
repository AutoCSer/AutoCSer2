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
    public sealed class DeployTaskNode : MethodParameterCreatorNode<IDeployTaskNode, TaskData>, IDeployTaskNode, ISnapshot<TaskData>
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
        /// 发布任务节点
        /// </summary>
        /// <param name="removeClosedDays">自动移除已关闭任务时间天数</param>
        public DeployTaskNode(int removeClosedDays = 1)
        {
            removeClosedTimeSpan = new TimeSpan(Math.Max(removeClosedDays, 1), 0, 0, 0);
            tasks = DictionaryCreator.CreateLong<TaskBuilder>();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IDeployTaskNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IDeployTaskNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            remove();
            foreach (TaskBuilder builder in tasks.Values) builder.Loaded();
            return this;
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
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            foreach (TaskBuilder builder in tasks.Values) builder.Remove(AutoCSer.Threading.SecondTimer.UtcNow);
            tasks.Clear();
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
        public SnapshotResult<TaskData> GetSnapshotResult(TaskData[] snapshotArray, object customObject)
        {
            SnapshotResult<TaskData> result = new SnapshotResult<TaskData>(snapshotArray.Length);
            foreach (TaskBuilder builder in tasks.Values) result.Add(snapshotArray, builder.Data);
            result.Add(snapshotArray, new TaskData(-currentIdentity));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(TaskData value)
        {
            if (value.Identity >= 0) tasks.Add(value.Identity, new TaskBuilder(this, value));
            else currentIdentity = -value.Identity;
        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns>任务标识ID</returns>
        public long Create()
        {
            long identity = ++currentIdentity;
            TaskBuilder builder = new TaskBuilder(this, identity);
            tasks.Add(identity, builder);
            if (removeTime >= AutoCSer.Threading.SecondTimer.UtcNow) remove();
            return identity;
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <returns></returns>
        public OperationStateEnum StartLoadPersistence(long identity, DateTime startTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) builder.Data.StartLoadPersistence(startTime);
            return OperationStateEnum.Unknown;
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="startTime">运行任务时间 Utc</param>
        /// <returns></returns>
        public OperationStateEnum Start(long identity, DateTime startTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) return builder.Start(startTime);
            return OperationStateEnum.NotFoundTask;
        }
        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="task">执行程序任务</param>
        /// <returns></returns>
        public OperationStateEnum AppendStepTask(long identity, StepTaskData task)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) return builder.Data.Append(task);
            return OperationStateEnum.NotFoundTask;
        }
        /// <summary>
        /// 获取发布任务状态变更回调日志
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="callback">任务状态变更回调委托</param>
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
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="closeTime">关闭任务时间</param>
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
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="closeTime">关闭任务时间</param>
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
        /// 关闭任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="closeTime">关闭任务时间</param>
        public void Close(long identity, DateTime closeTime)
        {
            var builder = default(TaskBuilder);
            if (tasks.TryGetValue(identity, out builder)) builder.Data.Close(closeTime);
            
        }
        /// <summary>
        /// 关闭任务
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close(TaskData data)
        {
            methodParameterCreator.Creator.Close(data.Identity, AutoCSer.Threading.SecondTimer.UtcNow);
        }
    }
}
