using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护服务端管理器
    /// </summary>
    public class ProcessGuardManager : CommandServerBindController, IProcessGuard
    {
        /// <summary>
        /// 被守护进程集合
        /// </summary>
        protected readonly Dictionary<int, GuardProcess> guards = DictionaryCreator.CreateInt<GuardProcess>();
        /// <summary>
        /// 添加待守护进程
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="processInfo">进程信息</param>
        /// <returns>是否添加成功</returns>
        public virtual bool Guard(CommandServerSocket socket, CommandServerCallQueue queue, ProcessGuardInfo processInfo)
        {
            Process process = Process.GetProcessById(processInfo.ProcessID);
            if (process == null || process.ProcessName != processInfo.ProcessName) return false;
            if (!guards.TryGetValue(processInfo.ProcessID, out GuardProcess guardProcess))
            {
                guards.Add(processInfo.ProcessID, new GuardProcess(this, processInfo, process));
            }
            return true;
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="processId">进程标识</param>
        /// <param name="processName">进程名称</param>
        public virtual void Remove(CommandServerSocket socket, CommandServerCallQueue queue, int processId, string processName)
        {
            if (guards.TryGetValue(processId, out GuardProcess guardProcess) && guardProcess.ProcessInfo.ProcessName == processName)
            {
                guards.Remove(processId);
                guardProcess.Remove();
            }
        }
        /// <summary>
        /// 被守护进程重启以后替换被守护进程信息
        /// </summary>
        /// <param name="guardProcess"></param>
        public virtual void OnStart(GuardProcess guardProcess)
        {
            if (guards.TryGetValue(guardProcess.ProcessInfo.ProcessID, out GuardProcess existsGuardProcess)
                && object.ReferenceEquals(guardProcess, existsGuardProcess))
            {
                guards.Remove(guardProcess.ProcessInfo.ProcessID);
            }
            if (guardProcess.NewProcess == null) return;
            if (guardProcess.IsRemove && guards.ContainsKey(guardProcess.NewProcess.Id))
            {
                guardProcess.NewProcess.Dispose();
                return;
            }
            guardProcess = new GuardProcess(guardProcess);
            guards.Add(guardProcess.ProcessInfo.ProcessID, guardProcess);
        }
        /// <summary>
        /// 启动新进程失败
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="guardProcess"></param>
        public virtual void OnProcessStartError(Exception exception, GuardProcess guardProcess)
        {
            CatchTask.AddIgnoreException(Controller.Server.Log.Exception(exception, $"新进程启动失败 {AutoCSer.JsonSerializer.Serialize(guardProcess)}", LogLevel.AutoCSer | LogLevel.Exception | LogLevel.Fatal));
        }
    }
}
