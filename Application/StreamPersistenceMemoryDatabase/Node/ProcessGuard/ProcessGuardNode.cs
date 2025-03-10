using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 进程守护节点（服务端需要以管理员身份运行，否则可能异常）
    /// </summary>
    public sealed class ProcessGuardNode : MethodParameterCreatorNode<IProcessGuardNode, ProcessGuardInfo>, IProcessGuardNode, ISnapshot<ProcessGuardInfo>
    {
        /// <summary>
        /// 被守护进程集合
        /// </summary>
        private readonly Dictionary<int, GuardProcess> guards;
        /// <summary>
        /// 切换进程回调委托集合
        /// </summary>
        private readonly Dictionary<string, MethodCallback<bool>> switchCallbacks;
        /// <summary>
        /// 进程守护节点
        /// </summary>
        public ProcessGuardNode()
        {
            guards = DictionaryCreator.CreateInt<GuardProcess>();
            switchCallbacks = DictionaryCreator.CreateAny<string, MethodCallback<bool>>();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return guards.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<ProcessGuardInfo> GetSnapshotResult(ProcessGuardInfo[] snapshotArray, object customObject)
        {
            SnapshotResult<ProcessGuardInfo> result = new SnapshotResult<ProcessGuardInfo>(guards.Count, snapshotArray.Length);
            foreach (GuardProcess guardProcess in guards.Values) result.Add(snapshotArray, guardProcess.ProcessInfo);
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(ProcessGuardInfo value)
        {
            Process process = GetProcessById(value.ProcessID);
            if (!object.ReferenceEquals(process, AutoCSer.Common.CurrentProcess) && process.StartTime == value.StartTime && process.ProcessName == value.ProcessName)
            {
                guards.Add(value.ProcessID, new GuardProcess(this, value, process));
            }
            else guards.Add(value.ProcessID, new GuardProcess(this, value));
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IProcessGuardNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IProcessGuardNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (GuardProcess guardProcess in guards.Values) guardProcess.Loaded();
            return null;
        }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            foreach (GuardProcess guardProcess in guards.Values) guardProcess.Remove();
            guards.Clear();
        }
        /// <summary>
        /// 数据库服务关闭操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceDisposable()
        {
            foreach (GuardProcess guardProcess in guards.Values) guardProcess.Remove();
        }
        /// <summary>
        /// 根据进程ID获取进程信息
        /// </summary>
        /// <param name="processID"></param>
        /// <returns></returns>
        internal Process GetProcessById(int processID)
        {
            try
            {
                return Process.GetProcessById(processID);
            }
            catch (System.ArgumentException exception)
            {
               if(StreamPersistenceMemoryDatabaseService.IsLoaded) StreamPersistenceMemoryDatabaseCallQueue.Server.Log.ExceptionIgnoreException(exception);
            }
            catch (Exception exception)
            {
                StreamPersistenceMemoryDatabaseCallQueue.Server.Log.ExceptionIgnoreException(exception);
            }
            return AutoCSer.Common.CurrentProcess;
        }
        /// <summary>
        /// 初始化添加待守护进程
        /// </summary>
        /// <param name="processInfo">进程信息</param>
        /// <returns>是否添加成功</returns>
        public bool GuardLoadPersistence(ProcessGuardInfo processInfo)
        {
            var guardProcess = default(GuardProcess);
            if (guards.TryGetValue(processInfo.ProcessID, out guardProcess))
            {
                ProcessGuardInfo currentInfo = guardProcess.ProcessInfo;
                if (currentInfo.StartTime == processInfo.StartTime && currentInfo.ProcessName == processInfo.ProcessName) return true;
                guards.Add(processInfo.ProcessID, new GuardProcess(this, processInfo));
                return true;
            }
            SnapshotSet(processInfo);
            return true;
        }
        /// <summary>
        /// 添加待守护进程
        /// </summary>
        /// <param name="processInfo">进程信息</param>
        /// <returns>是否添加成功</returns>
        public bool Guard(ProcessGuardInfo processInfo)
        {
            Process process = GetProcessById(processInfo.ProcessID);
            if (object.ReferenceEquals(process, AutoCSer.Common.CurrentProcess) || process.StartTime != processInfo.StartTime || process.ProcessName != processInfo.ProcessName) return false;
            var guardProcess = default(GuardProcess);
            if (!guards.TryGetValue(processInfo.ProcessID, out guardProcess))
            {
                guards.Add(processInfo.ProcessID, new GuardProcess(this, processInfo, process));
            }
            return true;
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="processId">进程标识</param>
        /// <param name="startTime">进程启动时间</param>
        /// <param name="processName">进程名称</param>
        public void Remove(int processId, DateTime startTime, string processName)
        {
            var guardProcess = default(GuardProcess);
            if (guards.TryGetValue(processId, out guardProcess))
            {
                ProcessGuardInfo info = guardProcess.ProcessInfo;
                if (info.StartTime == startTime && info.ProcessName == processName)
                {
                    guards.Remove(processId);
                    guardProcess.Remove();
                }
            }
        }
        /// <summary>
        /// 被守护进程重启以后替换被守护进程信息
        /// </summary>
        /// <param name="guardProcess"></param>
        internal void OnExited(GuardProcess guardProcess)
        {
            ProcessGuardInfo info = guardProcess.ProcessInfo;
            int processID = info.ProcessID;
            var existsGuardProcess = default(GuardProcess);
            if (guards.TryGetValue(processID, out existsGuardProcess)
                && object.ReferenceEquals(guardProcess, existsGuardProcess))
            {
                StreamPersistenceMemoryDatabaseMethodParameterCreator.Remove(processID, info.StartTime, info.ProcessName);
                guards.Remove(processID);
            }
            if (guardProcess.NewProcess == null) return;
            using (guardProcess.NewProcess)
            {
                if (guardProcess.IsRemove || guards.ContainsKey(guardProcess.NewProcess.Id)) return;
                StreamPersistenceMemoryDatabaseMethodParameterCreator.Guard(new ProcessGuardInfo(guardProcess));
            }
        }
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="key">切换进程关键字</param>
        /// <param name="callback">切换进程回调</param>
        public void Switch(string key, MethodCallback<bool> callback)
        {
            var lastCallback = default(MethodCallback<bool>);
            if (switchCallbacks.TryGetValue(key, out lastCallback)) lastCallback.Callback(false);
            switchCallbacks[key] = callback;
        }
    }
}
