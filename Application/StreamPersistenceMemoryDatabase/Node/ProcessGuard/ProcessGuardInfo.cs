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
        public ProcessGuardInfo(Process process, string[]? arguments) : base(process, arguments)
#else
        public ProcessGuardInfo(Process process, string[] arguments) : base(process, arguments)
#endif
        {
            ProcessID = process.Id;
            StartTime = process.StartTime;
            ProcessName = process.ProcessName;
        }
        /// <summary>
        /// 当前进程 dotnet 被守护进程信息
        /// </summary>
        /// <param name="process"></param>
#if NetStandard21
        private ProcessGuardInfo(Process process) : base(process)
#else
        private ProcessGuardInfo(Process process) : base(process)
#endif
        {
            ProcessID = process.Id;
            StartTime = process.StartTime;
            ProcessName = process.ProcessName;
        }
        /// <summary>
        /// 判断进程信息是否匹配
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        internal bool IsMatch(Process process)
        {
            if (StartTime != process.StartTime)
            {
                //Linux 环境中当前进程与外部进程获取的时间精度不一致，具体精度差异未知，允许 20 毫秒差异
                //Console.WriteLine(StartTime.Ticks - process.StartTime.Ticks);
                //Console.WriteLine(TimeSpan.TicksPerMillisecond * 20);
                if (Math.Abs(StartTime.Ticks - process.StartTime.Ticks) > TimeSpan.TicksPerMillisecond * 20) return false;
            }
            return ProcessName == process.ProcessName;
        }
        /// <summary>
        /// 获取当前进程被守护进程信息
        /// </summary>
        /// <param name="arguments">Main 函数传参</param>
        /// <returns></returns>
#if NetStandard21
        internal static ProcessGuardInfo GetCurrent(string[]? arguments)
#else
        internal static ProcessGuardInfo GetCurrent(string[] arguments)
#endif
        {
            if (AutoCSer.Diagnostics.ProcessInfo.IsCurrentProcessDotNet()) return new ProcessGuardInfo(AutoCSer.Common.CurrentProcess);
            return new ProcessGuardInfo(AutoCSer.Common.CurrentProcess, arguments);
        }
    }
}
