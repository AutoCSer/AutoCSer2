using AutoCSer.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程启动信息
    /// </summary>
    public class ProcessInfo
    {
        /// <summary>
        /// 默认切换服务目录后缀名称
        /// </summary>
        public const string DefaultSwitchDirectorySuffixName = ".SwitchProcess";
        /// <summary>
        /// dotnet 调用进程名称
        /// </summary>
        public const string DotNetProcessName = "dotnet";

        /// <summary>
        /// 运行文件
        /// </summary>
        public readonly string FileName;
        /// <summary>
        /// 命令行参数集合
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
        public readonly bool IsArgumentArray;
        /// <summary>
        /// 是否显示错误弹窗
        /// </summary>
        public readonly bool IsErrorDialog;
        /// <summary>
        /// 是否在新窗口中启动进程
        /// </summary>
        public readonly bool isCreateWindow;
        /// <summary>
        /// 进程启动信息
        /// </summary>
        protected ProcessInfo()
        {
#if NetStandard21
            FileName = WorkingDirectory = string.Empty;
            Arguments = EmptyArray<string>.Array;
            IsArgumentArray = true;
#endif
        }
        /// <summary>
        /// 进程启动信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="process"></param>
        internal ProcessInfo(ProcessInfo info, Process process)
        {
            UseShellExecute = info.UseShellExecute;
            WindowStyle = info.WindowStyle;
            Arguments = info.Arguments;
            IsArgumentArray = info.IsArgumentArray;
            WorkingDirectory = info.WorkingDirectory;
            IsErrorDialog = info.IsErrorDialog;
            isCreateWindow = info.isCreateWindow;
            try
            {
                FileName = process.StartInfo.FileName;
            }
            catch
            {
                FileName = info.FileName;
            }
        }
        /// <summary>
        /// 进程启动信息
        /// </summary>
        /// <param name="process"></param>
        /// <param name="arguments">Main 传参数组</param>
#if NetStandard21
        public ProcessInfo(Process process, string[]? arguments)
#else
        public ProcessInfo(Process process, string[] arguments)
#endif
        {
            try
            {
                ProcessStartInfo startInfo = process.StartInfo;
                UseShellExecute = startInfo.UseShellExecute;
                WindowStyle = startInfo.WindowStyle;
                FileName = startInfo.FileName;
                IsErrorDialog = startInfo.ErrorDialog;
                isCreateWindow = !startInfo.CreateNoWindow;
                if (arguments != null)
                {
                    Arguments = arguments;
                    IsArgumentArray = true;
                }
                else Arguments = new string[] { startInfo.Arguments };
                WorkingDirectory = startInfo.WorkingDirectory;
            }
            catch
            {
                UseShellExecute = true;

                FileInfo file = new FileInfo(process.MainModule.notNull().FileName);
                FileName = file.FullName;
                WorkingDirectory = Environment.CurrentDirectory;
                Arguments = arguments ?? Environment.GetCommandLineArgs();
                IsArgumentArray = true;
            }
        }
        /// <summary>
        /// 当前 dotnet 进程启动信息
        /// </summary>
        /// <param name="process"></param>
        internal ProcessInfo(Process process)
        {
            Arguments = Environment.GetCommandLineArgs();
            IsArgumentArray = true;
            try
            {
                ProcessStartInfo startInfo = process.StartInfo;
                UseShellExecute = startInfo.UseShellExecute;
                WindowStyle = startInfo.WindowStyle;
                FileName = startInfo.FileName;
                IsErrorDialog = startInfo.ErrorDialog;
                isCreateWindow = !startInfo.CreateNoWindow;
                WorkingDirectory = startInfo.WorkingDirectory;
            }
            catch
            {
                UseShellExecute = true;

                FileInfo file = new FileInfo(process.MainModule.notNull().FileName);
                FileName = file.FullName;
                WorkingDirectory = Environment.CurrentDirectory;
            }
        }
        /// <summary>
        /// 进程启动信息
        /// </summary>
        /// <param name="switchFile">切换进程启动文件信息</param>
        /// <param name="arguments">Main 传参数组</param>
        /// <param name="process"></param>
        internal ProcessInfo(FileInfo switchFile, string[] arguments, Process process)
        {
            FileName = switchFile.FullName;
            WorkingDirectory = switchFile.Directory.notNull().FullName;
            Arguments = arguments;
            IsArgumentArray = true;
            try
            {
                ProcessStartInfo startInfo = process.StartInfo;
                UseShellExecute = startInfo.UseShellExecute;
                WindowStyle = startInfo.WindowStyle;
                IsErrorDialog = startInfo.ErrorDialog;
                isCreateWindow = !startInfo.CreateNoWindow;
            }
            catch
            {
                UseShellExecute = true;
            }
        }
        /// <summary>
        /// 当前 dotnet 进程启动信息
        /// </summary>
        /// <param name="switchFile">切换进程启动文件信息</param>
        internal ProcessInfo(FileInfo switchFile)
        {
            Process process = AutoCSer.Common.CurrentProcess;
            FileName = process.MainModule.notNull().FileName;
            WorkingDirectory = switchFile.Directory.notNull().FullName;
            Arguments = Environment.GetCommandLineArgs();
            Arguments[0] = switchFile.FullName;
            IsArgumentArray = true;
            try
            {
                ProcessStartInfo startInfo = process.StartInfo;
                UseShellExecute = startInfo.UseShellExecute;
                WindowStyle = startInfo.WindowStyle;
                IsErrorDialog = startInfo.ErrorDialog;
                isCreateWindow = !startInfo.CreateNoWindow;
            }
            catch
            {
                UseShellExecute = true;
            }
        }
        /// <summary>
        /// 进程启动信息
        /// </summary>
        /// <param name="fileName">运行文件</param>
        /// <param name="arguments">命令行参数集合</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="windowStyle">进程启动时要使用的窗口状态</param>
        /// <param name="useShellExecute">是否使用操作系统外壳启动进程</param>
        /// <param name="isErrorDialog">是否显示错误弹窗</param>
        /// <param name="isCreateWindow">是否在新窗口中启动进程</param>
#if NetStandard21
        public ProcessInfo(string fileName, string[]? arguments = null, string? workingDirectory = null
#else
        public ProcessInfo(string fileName, string[] arguments = null, string workingDirectory = null
#endif
            , ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal, bool useShellExecute = true, bool isErrorDialog = false, bool isCreateWindow = true)
        {
            FileName = fileName;
            Arguments = arguments ?? EmptyArray<string>.Array;
            IsArgumentArray = true;
            WorkingDirectory = workingDirectory ?? new FileInfo(fileName).Directory.notNull().FullName;
            WindowStyle = windowStyle;
            UseShellExecute = useShellExecute;
            IsErrorDialog = isErrorDialog;
            this.isCreateWindow = isCreateWindow;
        }
        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private Process? start()
#else
        private Process start()
#endif
        {
            ProcessStartInfo startInfo;
            if (!FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
#if NET8
                startInfo = IsArgumentArray ? new ProcessStartInfo(FileName, Arguments) : new ProcessStartInfo(FileName, Arguments[0]);
#else
                startInfo = IsArgumentArray ? new ProcessStartInfo(FileName, string.Join(" ", Arguments)) : new ProcessStartInfo(FileName, Arguments[0]);
#endif
            }
            else
            {
#if NET8
                startInfo = IsArgumentArray ? new ProcessStartInfo("dotnet", FileName.valueToEnumerable().Concat(Arguments)) : new ProcessStartInfo("dotnet", FileName.valueToEnumerable().Concat(Arguments[0].valueToEnumerable()));
#else
                startInfo = IsArgumentArray ? new ProcessStartInfo("dotnet", FileName + " " + string.Join(" ", Arguments)) : new ProcessStartInfo("dotnet", FileName + " " + Arguments[0]);
#endif
            }
            startInfo.UseShellExecute = UseShellExecute;
            startInfo.WindowStyle = WindowStyle;
            startInfo.WorkingDirectory = string.IsNullOrEmpty(WorkingDirectory) ? new FileInfo(FileName).Directory.notNull().FullName : WorkingDirectory;
            startInfo.ErrorDialog = IsErrorDialog;
            startInfo.CreateNoWindow = !isCreateWindow;
            return Process.Start(startInfo);
        }
        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Process? Start()
#else
        public Process Start()
#endif
        {
            return File.Exists(FileName) ? start() : null;
        }
        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public async Task<Process?> StartAsync()
#else
        public async Task<Process> StartAsync()
#endif
        {
            if (await AutoCSer.Common.FileExists(FileName)) return start();
            return null;
        }
        /// <summary>
        /// 判断当前进程是否 dotnet 启动
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsCurrentProcessDotNet()
        {
            return AutoCSer.Common.CurrentProcess.ProcessName == DotNetProcessName;
        }
        /// <summary>
        /// 获取当前进程执行文件名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentProcessFileName()
        {
            return IsCurrentProcessDotNet() ? Environment.GetCommandLineArgs()[0] : AutoCSer.Common.CurrentProcess.MainModule.notNull().FileName;
            //processFileName = System.Reflection.Assembly.GetEntryAssembly().notNull().Location;
            //if (string.CompareOrdinal(processFileName.Substring(processFileName.Length - 4), ".dll") == 0)
            //{
            //    string exeDeployServerFileName = processFileName.Substring(0, processFileName.Length - 3) + "exe";
            //    if (File.Exists(exeDeployServerFileName)) processFileName = exeDeployServerFileName;
            //}
            //processFileName = new FileInfo(processFileName).Name;
        }
    }
}
