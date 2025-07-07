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
    public sealed class ProcessGuardNode : MethodParameterCreatorNode<IProcessGuardNode>, IProcessGuardNode, IEnumerableSnapshot<ProcessGuardInfo>
    {
        /// <summary>
        /// 被守护进程集合
        /// </summary>
        private readonly SnapshotDictionary<int, GuardProcess> guards;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<ProcessGuardInfo> IEnumerableSnapshot<ProcessGuardInfo>.SnapshotEnumerable { get { return guards.Nodes.Cast<GuardProcess, ProcessGuardInfo>(p => p.ProcessInfo); } }
        /// <summary>
        /// 切换进程回调委托集合
        /// </summary>
        private readonly Dictionary<string, MethodCallback<bool>> switchCallbacks;
        /// <summary>
        /// 进程守护节点
        /// </summary>
        public ProcessGuardNode()
        {
            guards = new SnapshotDictionary<int, GuardProcess>();
            switchCallbacks = DictionaryCreator<string>.Create<MethodCallback<bool>>();
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(ProcessGuardInfo value)
        {
            Process process = GetProcessById(value.ProcessID);
            if (!object.ReferenceEquals(process, AutoCSer.Common.CurrentProcess) && value.IsMatch(process)) guards.TryAdd(value.ProcessID, new GuardProcess(this, value, process));
            else guards.TryAdd(value.ProcessID, new GuardProcess(this, value));
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
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
        /// Processing operations after node removal
        /// 节点移除后的处理操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            foreach (GuardProcess guardProcess in guards.Values) guardProcess.Remove();
            guards.ClearArray();
        }
        /// <summary>
        /// Database service shutdown operation
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
        /// Initialize and add the daemon process to be added (Initialize and load the persistent data)
        /// 初始化添加待守护进程（初始化加载持久化数据）
        /// </summary>
        /// <param name="processInfo">Process information
        /// 进程信息</param>
        /// <returns>Add failed and return false
        /// 添加失败返回 false</returns>
        public bool GuardLoadPersistence(ProcessGuardInfo processInfo)
        {
            if(!guards.ContainsKey(processInfo.ProcessID)) SnapshotSet(processInfo);
            //var guardProcess = default(GuardProcess);
            //if (guards.TryGetValue(processInfo.ProcessID, out guardProcess))
            //{
            //    ProcessGuardInfo currentInfo = guardProcess.ProcessInfo;
            //    if (currentInfo.StartTime == processInfo.StartTime && currentInfo.ProcessName == processInfo.ProcessName) return true;
            //    guards.TryAdd(processInfo.ProcessID, new GuardProcess(this, processInfo));
            //    return true;
            //}
            //SnapshotSet(processInfo);
            return true;
        }
        /// <summary>
        /// Add the process to be daemon
        /// 添加待守护进程
        /// </summary>
        /// <param name="processInfo">Process information
        /// 进程信息</param>
        /// <returns>Add failed and return false
        /// 添加失败返回 false</returns>
        public bool Guard(ProcessGuardInfo processInfo)
        {
            Process process = GetProcessById(processInfo.ProcessID);
            if (object.ReferenceEquals(process, AutoCSer.Common.CurrentProcess) || !processInfo.IsMatch(process)) return false;
            var guardProcess = default(GuardProcess);
            if (!guards.TryGetValue(processInfo.ProcessID, out guardProcess))
            {
                guards.TryAdd(processInfo.ProcessID, new GuardProcess(this, processInfo, process));
                Output(nameof(Guard), processInfo);
                return true;
            }
            if (guardProcess.IsLoad)
            {
                guards[processInfo.ProcessID] = new GuardProcess(this, processInfo, process);
                guardProcess.Remove();
                Output(nameof(Guard), processInfo);
            }
            return true;
        }
        /// <summary>
        /// 输出进程信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="processInfo"></param>
        internal static void Output(string type, ProcessGuardInfo processInfo)
        {
            Console.WriteLine();
            Console.Write(type);
            Console.Write(' ');
            Console.Write(processInfo.ProcessID);
            Console.Write(' ');
            Console.Write(processInfo.ProcessName);
            if (processInfo.Arguments != null && processInfo.Arguments.Length != 0)
            {
                Console.Write(' ');
                Console.Write(string.Join(" ", processInfo.Arguments));
            }
        }
        /// <summary>
        /// Delete the daemon process
        /// 删除被守护进程
        /// </summary>
        /// <param name="processId">Process identity
        /// 进程标识</param>
        /// <param name="startTime">Process startup time
        /// 进程启动时间</param>
        /// <param name="processName">Process name
        /// 进程名称</param>
        public void Remove(int processId, DateTime startTime, string processName)
        {
            var guardProcess = default(GuardProcess);
            if (guards.TryGetValue(processId, (uint)processId.GetHashCode(), out guardProcess))
            {
                ProcessGuardInfo processInfo = guardProcess.ProcessInfo;
                if (processInfo.StartTime == startTime && processInfo.ProcessName == processName)
                {
                    guards.Remove(processId);
                    guardProcess.Remove();
                    if(StreamPersistenceMemoryDatabaseService.IsLoaded) Output(nameof(Remove), processInfo);
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
            if (guards.TryGetValue(processID, (uint)processID.GetHashCode(), out existsGuardProcess)
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
        /// Switch processes
        /// 切换进程
        /// </summary>
        /// <param name="key">The key words of the switched process
        /// 切换进程关键字</param>
        /// <param name="callback">Switch process callback
        /// 切换进程回调</param>
        public void Switch(string key, MethodCallback<bool> callback)
        {
            var lastCallback = default(MethodCallback<bool>);
            if (switchCallbacks.TryGetValue(key, out lastCallback)) lastCallback.Callback(false);
            switchCallbacks[key] = callback;
        }
    }
}
