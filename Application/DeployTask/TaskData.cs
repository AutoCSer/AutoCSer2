using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 发布任务序列化数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class TaskData
    {
        /// <summary>
        /// 任务标识ID
        /// </summary>
        public readonly long Identity;
        /// <summary>
        /// 启动任务时间
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// 启动任务时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return isStart ? startTime : default(DateTime);
            }
        }
        /// <summary>
        /// 关闭任务时间
        /// </summary>
        private DateTime closeTime;
        /// <summary>
        /// 子任务集合
        /// </summary>
        private LeftArray<StepTaskData> tasks;
        /// <summary>
        /// 子任务数量
        /// </summary>
        public int TaskCount { get { return tasks.Count; } }
        /// <summary>
        /// 子任务集合
        /// </summary>
        public IEnumerable<StepTaskData> Tasks { get { return tasks; } }
        /// <summary>
        /// 发布任务状态变更回调日志集合
        /// </summary>
        private LeftArray<DeployTaskLog> logs;
        /// <summary>
        /// 发布任务状态变更回调日志集合
        /// </summary>
        public IEnumerable<DeployTaskLog> Logs { get { return logs; } }
        /// <summary>
        /// 当前执行子任务索引位置
        /// </summary>
        private int currentIndex;
        /// <summary>
        /// 当前执行子任务索引位置
        /// </summary>
        public int CurrentIndex { get { return currentIndex; } }
        /// <summary>
        /// 是否已经启动任务
        /// </summary>
        private bool isStart;
        /// <summary>
        /// 是否已经启动任务
        /// </summary>
        public bool IsStart { get { return isStart; } }
        /// <summary>
        /// 是否已经关闭任务
        /// </summary>
        public bool IsClosed
        {
            get { return closeTime != default(DateTime); }
        }
        /// <summary>
        /// 发布任务序列化数据
        /// </summary>
        private TaskData()
        {
            tasks.SetEmpty();
            logs.SetEmpty();
        }
        /// <summary>
        /// 发布任务序列化数据
        /// </summary>
        /// <param name="identity"></param>
        internal TaskData(long identity) : this()
        {
            this.Identity = identity;
        }
        /// <summary>
        /// 添加子任务信息
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        internal OperationStateEnum Append(StepTaskData task)
        {
            if (isStart) return OperationStateEnum.StartedError;
            if (IsClosed) return OperationStateEnum.Closed;
            tasks.Add(task);
            return OperationStateEnum.Success;
        }
        /// <summary>
        /// 发布任务状态变更回调日志回调
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal bool Callback(MethodKeepCallback<DeployTaskLog> callback)
        {
            foreach (DeployTaskLog log in logs)
            {
                if (!callback.Callback(log)) return false;
            }
            if (IsClosed)
            {
                callback.CancelKeep();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 添加发布任务状态变更回调日志
        /// </summary>
        /// <param name="log"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(DeployTaskLog log)
        {
            logs.Add(log);
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void StartLoadPersistence(DateTime startTime)
        {
            OperationStateEnum state = OperationStateEnum.Unknown;
            isStart = Start(startTime, ref state);
        }
        /// <summary>
        /// 启动任务检查
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal bool Start(DateTime startTime, ref OperationStateEnum state)
        {
            if (isStart) state = OperationStateEnum.StartedError;
            else if (tasks.Count == 0) state = OperationStateEnum.EmptyTask;
            else if (IsClosed) state = OperationStateEnum.Closed;
            else
            {
                this.startTime = startTime;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置已经启动任务
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Start()
        {
            isStart = true;
        }
        /// <summary>
        /// 冷启动加载完毕
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Loaded()
        {
            return isStart && !IsClosed;
        }
        /// <summary>
        /// 是否自动移除
        /// </summary>
        /// <param name="removeClosedTimeSpan"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsRemove(TimeSpan removeClosedTimeSpan)
        {
            return IsClosed && (AutoCSer.Threading.SecondTimer.UtcNow - closeTime) >= removeClosedTimeSpan;
        }
        /// <summary>
        /// 获取当前执行子任务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal StepTaskData GetStepTask()
        {
            return tasks.Array[currentIndex];
        }
        /// <summary>
        /// 获取发布任务状态变更回调日志
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal DeployTaskLog GetDeployTaskLog(OperationStateEnum state)
        {
            int index = currentIndex++;
            return new DeployTaskLog(Identity, index, tasks.Array[index].Type, state, currentIndex == tasks.Length || state != OperationStateEnum.Success);
        }
        /// <summary>
        /// 关闭任务
        /// </summary>
        /// <param name="closeTime"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close(DateTime closeTime)
        {
            this.closeTime = closeTime;
        }
        /// <summary>
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="closeTime"></param>
        /// <returns></returns>
        internal OperationStateEnum RemoveLoadPersistence(DateTime closeTime)
        {
            if (IsClosed) return OperationStateEnum.Success;
            if (isStart) return OperationStateEnum.StartedError;
            this.closeTime = closeTime;
            return OperationStateEnum.Success;
        }
        /// <summary>
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="closeTime"></param>
        /// <returns></returns>
        internal OperationStateEnum Remove(DateTime closeTime)
        {
            if (IsClosed) return OperationStateEnum.Success;
            if (isStart) return OperationStateEnum.StartedError;
            this.closeTime = closeTime;
            foreach (StepTaskData task in tasks) task.Task.Remove();
            return OperationStateEnum.Success;
        }
    }
}
