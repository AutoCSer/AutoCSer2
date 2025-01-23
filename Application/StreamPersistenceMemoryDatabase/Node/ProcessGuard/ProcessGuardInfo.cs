using AutoCSer.Extensions;
using System;
using System.Diagnostics;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 被守护进程信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class ProcessGuardInfo
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
        /// 运行文件
        /// </summary>
        public readonly string FileName;
        /// <summary>
        /// 命令行参数
        /// </summary>
        public readonly string[] Arguments;
        /// <summary>
        /// 工作目录
        /// </summary>
        public readonly string WorkingDirectory;
        /// <summary>
        /// 进程启动时要使用的窗口状态
        /// </summary>
        public readonly ProcessWindowStyle WindowStyle;
        /// <summary>
        /// 是否使用操作系统外壳启动进程
        /// </summary>
        public readonly bool UseShellExecute;
        /// <summary>
        /// 命令行参数是否 Main 函数传参数组
        /// </summary>
        public readonly bool isArgumentArray;
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        private ProcessGuardInfo()
        {
#if NetStandard21
            ProcessName = FileName = WorkingDirectory = string.Empty;
            Arguments = EmptyArray<string>.Array;
            isArgumentArray = true;
#endif
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="guardProcess"></param>
        internal ProcessGuardInfo(GuardProcess guardProcess)
        {
            Process process = guardProcess.NewProcess.notNull();
            ProcessID = process.Id;
            StartTime = process.StartTime;
            ProcessName = process.ProcessName;
            ProcessGuardInfo processGuardInfo = guardProcess.ProcessInfo;
            UseShellExecute = processGuardInfo.UseShellExecute;
            WindowStyle = processGuardInfo.WindowStyle;
            Arguments = processGuardInfo.Arguments;
            isArgumentArray = processGuardInfo.isArgumentArray;
            WorkingDirectory = processGuardInfo.WorkingDirectory;
            try
            {
                FileName = process.StartInfo.FileName;
            }
            catch
            {
                FileName = processGuardInfo.FileName;
            }
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="process"></param>
        /// <param name="arguments">Main 传参数组</param>
#if NetStandard21
        public ProcessGuardInfo(Process process, string[]? arguments = null)
#else
        public ProcessGuardInfo(Process process, string[] arguments = null)
#endif
        {
            ProcessID = process.Id;
            StartTime = process.StartTime;
            ProcessName = process.ProcessName;
            try
            {
                ProcessStartInfo startInfo = process.StartInfo;
                UseShellExecute = startInfo.UseShellExecute;
                WindowStyle = startInfo.WindowStyle;
                FileName = startInfo.FileName;
                if (arguments != null)
                {
                    Arguments = arguments;
                    isArgumentArray = true;
                }
                else Arguments = new string[] { startInfo.Arguments };
                WorkingDirectory = startInfo.WorkingDirectory;
            }
            catch
            {
                UseShellExecute = true;
                WindowStyle = ProcessWindowStyle.Normal;

                FileInfo file = new FileInfo(process.MainModule.notNull().FileName);
                FileName = file.FullName;
                WorkingDirectory = Environment.CurrentDirectory;
                Arguments = arguments ?? Environment.GetCommandLineArgs();
                isArgumentArray = true;
            }
        }
        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public Process? Start()
#else
        public Process Start()
#endif
        {
#if NET8
            ProcessStartInfo startInfo = isArgumentArray ? new ProcessStartInfo(FileName, Arguments) :  new ProcessStartInfo(FileName, Arguments[0]);
#else
            ProcessStartInfo startInfo = isArgumentArray ? new ProcessStartInfo(FileName, string.Join(" ", Arguments)) : new ProcessStartInfo(FileName, Arguments[0]);
#endif
            startInfo.UseShellExecute = UseShellExecute;
            startInfo.WindowStyle = WindowStyle;
            startInfo.WorkingDirectory = WorkingDirectory;
            startInfo.ErrorDialog = false;
            startInfo.CreateNoWindow = true;
            return Process.Start(startInfo);
        }
    }
}
