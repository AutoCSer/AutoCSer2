using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 切换进程
    /// </summary>
    public abstract class SwitchProcess
    {
        /// <summary>
        /// 切换进程进关闭参数
        /// </summary>
        public const string CloseSwitchProcessArgument = AutoCSer.Common.NamePrefix + "CloseSwitchProcess";

        /// <summary>
        /// 切换服务锁
        /// </summary>
        protected AutoCSer.Threading.SemaphoreSlimLock exitLock;
        /// <summary>
        /// 是否需要初始化处理
        /// </summary>
        private bool isInitialize;
        /// <summary>
        /// 切换进程等待关闭处理
        /// </summary>
        private SwitchWait switchWait;
        /// <summary>
        /// 切换进程（默认规则）
        /// </summary>
        /// <param name="arguments"></param>
        protected SwitchProcess(string[] arguments)
            : this(arguments.Length == 0 ? null : arguments[0], Array.IndexOf(arguments, CloseSwitchProcessArgument) >= 0)
        {

        }
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="isOnlySet"></param>
        /// <param name="deployServerFileName"></param>
        /// <param name="switchDirectoryName"></param>
        protected SwitchProcess(string arguments, bool isOnlySet = false, string deployServerFileName = null, string switchDirectoryName = DefaultSwitchDirectoryName)
        {
            FileInfo SwitchFile = GetSwitchFile(deployServerFileName, switchDirectoryName);
            if (SwitchFile != null)
            {
                StartProcessDirectory(SwitchFile, arguments);
                return;
            }
            if (isOnlySet) SwitchWait.Set(switchWaitPrefix);
            else isInitialize = true;
        }
        /// <summary>
        /// 切换进程名称前缀，可用于区分环境版本
        /// </summary>
        protected virtual string switchWaitPrefix { get { return null; } }
        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <returns></returns>
        protected virtual Task initialize() { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 切换进程等待关闭处理退出
        /// </summary>
        protected virtual void switchExit()
        {
            exitLock.Exit();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        public async Task Start()
        {
            if (isInitialize)
            {
                isInitialize = false;
                await initialize();
                exitLock = new Threading.SemaphoreSlimLock(0);
                switchWait = new SwitchWait(switchExit, switchWaitPrefix);
#if !MONO
                //Win32.Kernel32.SetErrorMode(Win32.ErrorMode.SEM_NOGPFAULTERRORBOX | Win32.ErrorMode.SEM_NOOPENFILEERRORBOX);
#endif
                await onStart();
                await exitLock.EnterAsync();
                await onExit();
            }
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected abstract Task onStart();
        /// <summary>
        /// 退出运行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task onExit()
        {
            await AutoCSer.LogHelper.Flush();
            Environment.Exit(0);
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <returns></returns>
        protected virtual Task removeGuard() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static bool StartProcessDirectory(FileInfo file, string arguments = null)
        {
            System.Diagnostics.Process process = GetStartProcessDirectory(file, arguments);
            if (process != null)
            {
                process.Dispose();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static System.Diagnostics.Process GetStartProcessDirectory(FileInfo file, string arguments = null)
        {
            if (file != null && file.Exists) return getStartProcessDirectory(file, arguments);
            return null;
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static System.Diagnostics.Process getStartProcessDirectory(FileInfo file, string arguments)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(file.FullName, arguments);
            info.UseShellExecute = true;
            info.WorkingDirectory = file.DirectoryName;
            return System.Diagnostics.Process.Start(info);
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static async Task<bool> StartProcessDirectoryAsync(FileInfo file, string arguments = null)
        {
            System.Diagnostics.Process process = await GetStartProcessDirectoryAsync(file, arguments);
            if (process != null)
            {
                process.Dispose();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static async Task<System.Diagnostics.Process> GetStartProcessDirectoryAsync(FileInfo file, string arguments = null)
        {
            if (file != null && await AutoCSer.Common.Config.FileExists(file)) return getStartProcessDirectory(file, arguments);
            return null;
        }
        /// <summary>
        /// 初始化时获取切换服务文件
        /// </summary>
        /// <param name="deployServerFileName">发布服务文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <returns>切换服务文件</returns>
        public static FileInfo GetSwitchFile(string deployServerFileName = null, string switchDirectoryName = DefaultSwitchDirectoryName)
        {
            DirectoryInfo CurrentDirectory = AutoCSer.Common.ApplicationDirectory, SwitchDirectory;
            if (CurrentDirectory.Name == switchDirectoryName)
            {
                SwitchDirectory = CurrentDirectory.Parent;
            }
            else
            {
                SwitchDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, switchDirectoryName));
            }
            if (SwitchDirectory.Exists)
            {
                if (deployServerFileName == null)
                {
                    deployServerFileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                    if (string.CompareOrdinal(deployServerFileName.Substring(deployServerFileName.Length - 4), ".dll") == 0)
                    {
                        string exeDeployServerFileName = deployServerFileName.Substring(0, deployServerFileName.Length - 3) + "exe";
                        if (File.Exists(exeDeployServerFileName)) deployServerFileName = exeDeployServerFileName;
                    }
                    deployServerFileName = new FileInfo(deployServerFileName).Name;
                }
                FileInfo SwitchFile = new FileInfo(Path.Combine(SwitchDirectory.FullName, deployServerFileName));
                if (SwitchFile.Exists)
                {
                    FileInfo CurrentFile = new FileInfo(Path.Combine(CurrentDirectory.FullName, deployServerFileName));
                    if (SwitchFile.LastWriteTimeUtc > CurrentFile.LastWriteTimeUtc) return SwitchFile;
                }
            }
            return null;
        }
        /// <summary>
        /// 默认切换服务相对目录名称
        /// </summary>
        public const string DefaultSwitchDirectoryName = "Switch";
    }
}
