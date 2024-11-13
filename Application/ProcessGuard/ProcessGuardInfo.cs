using AutoCSer.Extensions;
using System;
using System.Diagnostics;
using System.IO;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 被守护进程信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, IsAnonymousFields = true)]
    public sealed class ProcessGuardInfo
    {
        /// <summary>
        /// 进程标识ID
        /// </summary>
        public int ProcessID { get; internal set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public ProcessPriorityClass PriorityClass { get; internal set; }
        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; internal set; }

        /// <summary>
        /// 是否使用操作系统外壳启动进程
        /// </summary>
        public bool UseShellExecute { get; internal set; }
        /// <summary>
        /// 进程启动时要使用的窗口状态
        /// </summary>
        public ProcessWindowStyle WindowStyle { get; internal set; }
        /// <summary>
        /// 运行文件
        /// </summary>
        public string FileName { get; internal set; }
        /// <summary>
        /// 命令行参数
        /// </summary>
        public string Arguments { get; internal set; }
        /// <summary>
        /// 工作目录
        /// </summary>
        public string WorkingDirectory { get; internal set; }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        private ProcessGuardInfo()
        {
#if NetStandard21
            WorkingDirectory = Arguments = FileName = ProcessName = string.Empty;
#endif
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="process"></param>
        internal ProcessGuardInfo(Process process)
        {
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            try
            {
                ProcessStartInfo startInfo = process.StartInfo;
                UseShellExecute = startInfo.UseShellExecute;
                WindowStyle = startInfo.WindowStyle;
                FileName = startInfo.FileName;
                Arguments = startInfo.Arguments;
                WorkingDirectory = startInfo.WorkingDirectory;
            }
            catch
            {
                UseShellExecute = true;
                WindowStyle = ProcessWindowStyle.Normal;

                FileInfo file = new FileInfo(process.MainModule.notNull().FileName);
                FileName = file.FullName;
                WorkingDirectory = Environment.CurrentDirectory;
                Arguments = string.Join(" ", Environment.GetCommandLineArgs());
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
            ProcessStartInfo startInfo = new ProcessStartInfo(FileName, Arguments);
            startInfo.UseShellExecute = UseShellExecute;
            startInfo.WindowStyle = WindowStyle;
            startInfo.WorkingDirectory = WorkingDirectory;
            startInfo.ErrorDialog = false;
            startInfo.CreateNoWindow = true;
            return Process.Start(startInfo);
        }
    }
}
