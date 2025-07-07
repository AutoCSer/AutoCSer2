using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 发布任务创建器
    /// </summary>
    internal sealed class TaskBuilder : SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 发布任务节点
        /// </summary>
        private readonly DeployTaskNode node;
        /// <summary>
        /// 发布任务序列化数据
        /// </summary>
        internal readonly TaskData Data;
        /// <summary>
        /// 任务状态变更回调委托集合
        /// </summary>
        private LeftArray<MethodKeepCallback<DeployTaskLog>> callbacks;
        /// <summary>
        /// 发布任务创建器
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity"></param>
        internal TaskBuilder(DeployTaskNode node, long identity) : base(AutoCSer.Threading.SecondTimer.TaskArray, SecondTimerTaskThreadModeEnum.AddCatchTask)
        {
            this.node = node;
            Data = new TaskData(identity);
            callbacks.SetEmpty();
        }
        /// <summary>
        /// 发布任务创建器
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        internal TaskBuilder(DeployTaskNode node, TaskData data)
        {
            this.node = node;
            Data = data;
        }
        /// <summary>
        /// 添加任务状态变更回调委托
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Append(MethodKeepCallback<DeployTaskLog> callback)
        {
            if (Data.Callback(callback)) callbacks.Add(callback);
        }
        /// <summary>
        /// 发布任务状态变更回调日志回调
        /// </summary>
        /// <param name="log"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Callback(DeployTaskLog log)
        {
            Data.Add(log);
            MethodKeepCallback<DeployTaskLog>.Callback(ref callbacks, log);
        }
        /// <summary>
        /// Start the task
        /// 启动任务
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        internal OperationStateEnum Start(DateTime startTime)
        {
            OperationStateEnum state = OperationStateEnum.Exception;
            try
            {
                if (Data.Start(startTime, ref state)) state = start();
            }
            finally
            {
                if (state != OperationStateEnum.Unknown) Callback(new DeployTaskLog(Data.Identity, state));
            }
            return state;
        }
        /// <summary>
        /// Start the task
        /// 启动任务
        /// </summary>
        /// <returns></returns>
        private OperationStateEnum start()
        {
            long seconds = (long)(Data.StartTime - AutoCSer.Threading.SecondTimer.UtcNow).TotalSeconds;
            if (seconds <= 0)
            {
                OnTimerAsync().Catch();
                Data.Start();
                return OperationStateEnum.Success;
            }
            if (seconds <= int.MaxValue)
            {
                TryAppendTaskArray((int)seconds);
                Data.Start();
                return OperationStateEnum.Unknown;
            }
            return OperationStateEnum.StartTimeError;
        }
        /// <summary>
        /// 冷启动加载完毕
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Loaded()
        {
            if (Data.Loaded()) start();
        }
        /// <summary>
        /// 定时启动任务
        /// </summary>
        /// <returns></returns>
        protected internal override async Task OnTimerAsync()
        {
            Data.Start();
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                do
                {
                    StepTaskData task = Data.GetStepTask();
                    OperationStateEnum state = OperationStateEnum.Exception;
                    try
                    {
                        state = await task.Task.Run();
                        if (state != OperationStateEnum.Success) return;
                    }
                    finally
                    {
                        if (state != OperationStateEnum.Unknown) node.StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(new TaskBuilderCallback(this, state));
                    }
                }
                while (Data.IsNextStep);
            }
            finally { node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Close(Data.Identity, AutoCSer.Threading.SecondTimer.UtcNow); }
        }
        /// <summary>
        /// Remove completed or un-started task
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        /// <returns></returns>
        internal OperationStateEnum Remove(DateTime closeTime)
        {
            OperationStateEnum state = Data.Remove(closeTime);
            if (state == OperationStateEnum.Success)
            {
                DeployTaskLog log = new DeployTaskLog(Data.Identity, OperationStateEnum.Remove, true);
                foreach (MethodKeepCallback<DeployTaskLog> callback in callbacks) callback.CallbackCancelKeep(log);
                callbacks.SetEmpty();
            }
            return state;
        }
    }
}
