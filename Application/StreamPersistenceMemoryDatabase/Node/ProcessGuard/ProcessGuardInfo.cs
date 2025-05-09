using AutoCSer.Diagnostics;
using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 被守护进程信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class ProcessGuardInfo : ProcessInfo
    {
        /// <summary>
        /// 进程标识ID
        /// </summary>
        public readonly int ProcessID;
        /// <summary>
        /// 优先级
        /// </summary>
        public readonly ProcessPriorityClass PriorityClass;
        /// <summary>
        /// 进程启动时间
        /// </summary>
        public readonly DateTime StartTime;
        /// <summary>
        /// 进程名称
        /// </summary>
        public readonly string ProcessName;
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        private ProcessGuardInfo()
        {
#if NetStandard21
            ProcessName = string.Empty;
#endif
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="guardProcess"></param>
        internal ProcessGuardInfo(GuardProcess guardProcess) :base(guardProcess.ProcessInfo, guardProcess.NewProcess.notNull())
        {
            Process process = guardProcess.NewProcess.notNull();
            ProcessID = process.Id;
            StartTime = process.StartTime;
            ProcessName = process.ProcessName;
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="process"></param>
        /// <param name="arguments">Main 传参数组</param>
#if NetStandard21
        public ProcessGuardInfo(Process process, string[]? arguments = null) : base(process, arguments)
#else
        public ProcessGuardInfo(Process process, string[] arguments = null) : base(process, arguments)
#endif
        {
            ProcessID = process.Id;
            StartTime = process.StartTime;
            ProcessName = process.ProcessName;
        }
    }
}
