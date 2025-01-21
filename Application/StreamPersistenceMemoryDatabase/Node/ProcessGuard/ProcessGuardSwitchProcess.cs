using AutoCSer.Deploy;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 切换进程
    /// </summary>
    public abstract class ProcessGuardSwitchProcess
    {
        /// <summary>
        /// 默认切换服务相对目录名称
        /// </summary>
        public const string DefaultSwitchDirectoryName = "Switch";

        /// <summary>
        /// Main 函数传参
        /// </summary>
        private readonly string[] arguments;
        /// <summary>
        /// 切换进程关键字，默认为当前进程名称
        /// </summary>
        protected virtual string switchProcessKey { get { return AutoCSer.Common.CurrentProcess.ProcessName; } }
        /// <summary>
        /// 执行进程文件名称，默认为当前执行文件
        /// </summary>
        protected virtual string processFileName
        {
            get { return string.Empty; }
        }
        /// <summary>
        /// 切换服务相对目录名称，默认为 Switch
        /// </summary>
        protected virtual string switchDirectoryName
        {
            get { return DefaultSwitchDirectoryName; }
        }
        /// <summary>
        /// 默认为 true 表示添加守护
        /// </summary>
        protected virtual bool isGuard { get { return true; } }
        /// <summary>
        /// 是否已经开始运行或者启动切换进程
        /// </summary>
        protected bool isStart { get; private set; }
        /// <summary>
        /// 是否准备退出操作
        /// </summary>
        private bool isExit;
        /// <summary>
        /// 获取进程守护节点客户端
        /// </summary>
        protected abstract StreamPersistenceMemoryDatabaseClientNodeCache<IProcessGuardNodeClientNode> getProcessGuardClient { get; }
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="arguments">Main 函数参数</param>
        protected ProcessGuardSwitchProcess(string[] arguments)
        {
            this.arguments = arguments;
            var switchFile = GetSwitchFile(processFileName, switchDirectoryName);
            if (switchFile != null)
            {
                StartProcessDirectory(switchFile, arguments);
                isStart = true;
            }
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> Start()
        {
            if (!isStart)
            {
                isStart = true;
                await AutoCSer.Threading.SwitchAwaiter.Default;
                try
                {
                    ResponseResult<IProcessGuardNodeClientNode> client = await getProcessGuardClient.GetNode();
                    if (client.Value == null)
                    {
                        await onError(client);
                        await initialize();
                        trySwitch().NotWait();
                        await onStart();
                        return client;
                    }
                    await initialize();
                    ResponseParameterAwaiter<bool> switchAwaiter = client.Value.Switch(switchProcessKey);
                    if (isGuard) guard().NotWait();
                    await onStart();
                    do
                    {
                        ResponseResult<bool> result = await switchAwaiter;
                        if (result.IsSuccess) break;
                        await Task.Delay(1);
                        switchAwaiter = client.Value.Switch(switchProcessKey);
                    }
                    while (true);
                    isExit = true;
                    await onExit();
                    return new ResponseResult(CallStateEnum.Success);
                }
                catch (Exception exception)
                {
                    await onException(exception);
                }
                return new ResponseResult(CallStateEnum.Unknown);
            }
            return new ResponseResult(CallStateEnum.Success);
        }
        /// <summary>
        /// 错误信息处理
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual async Task onError(ResponseResult<IProcessGuardNodeClientNode> result)
        {
            string message = $"ReturnType[{result.ReturnType}] CallState[{result.CallState}]";
            await AutoCSer.LogHelper.Error(message);
            AutoCSer.ConsoleWriteQueue.WriteLine(message, ConsoleColor.Red);
        }
        /// <summary>
        /// 异常信息处理
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual async Task onException(Exception exception)
        {
            isExit = true;
            await AutoCSer.LogHelper.Exception(exception);
            AutoCSer.ConsoleWriteQueue.WriteLine(exception.ToString(), ConsoleColor.Red);
            await Task.Delay(1000);
            await onExit();
        }
        /// <summary>
        /// 尝试重试等待切换线程
        /// </summary>
        /// <returns></returns>
        private async Task trySwitch()
        {
            if (isGuard) guard().NotWait();
            do
            {
                ResponseResult<IProcessGuardNodeClientNode> client = await getProcessGuardClient.GetNode();
                if (client.Value != null)
                {
                    Console.WriteLine();
                    do
                    {
                        ResponseResult<bool> result = await client.Value.Switch(switchProcessKey);
                        if (result.IsSuccess) break;
                        await Task.Delay(1);
                    }
                    while (true);
                    isExit = true;
                    await onExit();
                    return;
                }
                await Task.Delay(1);
            }
            while (!isExit);
        }
        /// <summary>
        /// 初始化操作，让进程服务处于准备服务状态，完成该操作以后会通知旧服务进程下线
        /// </summary>
        /// <returns></returns>
        protected virtual Task initialize() { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 添加守护
        /// </summary>
        /// <returns></returns>
        protected virtual async Task guard()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ProcessGuardInfo processGuardInfo = new ProcessGuardInfo(AutoCSer.Common.CurrentProcess, arguments);
            do
            {
                ResponseResult<IProcessGuardNodeClientNode> client = await getProcessGuardClient.GetNode();
                if (client.Value != null)
                {
                    if (isExit) return;
                    ResponseResult<bool> result = await client.Value.Guard(processGuardInfo);
                    if (result.Value) return;
                }
                await Task.Delay(1);
            }
            while (!isExit);
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected virtual Task onStart() { return AutoCSer.Common.CompletedTask; }
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
        /// 正常退出操作（尝试移除进程守护）
        /// </summary>
        /// <returns></returns>
        protected async Task exit()
        {
            isExit = true;
            if (isGuard)
            {
                ResponseResult<IProcessGuardNodeClientNode> client = await getProcessGuardClient.GetNode();
                if (client.Value != null) await client.Value.RemoveCurrentProcess();
            }
            await onExit();
        }

        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">Main 函数参数</param>
        /// <returns>是否成功</returns>
        public static bool StartProcessDirectory(FileInfo file, string[] arguments)
        {
            var process = GetStartProcessDirectory(file, arguments);
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
        /// <param name="arguments">Main 函数参数</param>
        /// <returns>是否成功</returns>
#if NetStandard21
        public static Process? GetStartProcessDirectory(FileInfo file, string[] arguments)
#else
        public static Process GetStartProcessDirectory(FileInfo file, string[] arguments)
#endif
        {
            if (file != null && file.Exists) return getStartProcessDirectory(file, arguments);
            return null;
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file"></param>
        /// <param name="arguments">Main 函数参数</param>
        /// <returns></returns>
#if NetStandard21
        private static Process? getStartProcessDirectory(FileInfo file, string[] arguments)
#else
        private static Process getStartProcessDirectory(FileInfo file, string[] arguments)
#endif
        {
#if NET8
            ProcessStartInfo info = new ProcessStartInfo(file.FullName, arguments);
#else
            ProcessStartInfo info = new ProcessStartInfo(file.FullName, string.Join(" ", arguments));
#endif
            info.UseShellExecute = true;
            info.WorkingDirectory = file.DirectoryName;
            return Process.Start(info);
        }
        /// <summary>
        /// 初始化时获取切换服务文件
        /// </summary>
        /// <param name="processFileName">执行进程文件名称，默认为当前执行文件</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称，默认为 Switch</param>
        /// <returns>切换服务文件</returns>
#if NetStandard21
        public static FileInfo? GetSwitchFile(string processFileName = "", string switchDirectoryName = DefaultSwitchDirectoryName)
#else
        public static FileInfo GetSwitchFile(string processFileName = "", string switchDirectoryName = DefaultSwitchDirectoryName)
#endif
        {
            DirectoryInfo currentDirectory = AutoCSer.Common.ApplicationDirectory, switchDirectory;
            if (currentDirectory.Name == switchDirectoryName)
            {
                switchDirectory = currentDirectory.Parent.notNull();
            }
            else
            {
                switchDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, switchDirectoryName));
            }
            if (switchDirectory.Exists)
            {
                if (string.IsNullOrEmpty(processFileName))
                {
                    processFileName = System.Reflection.Assembly.GetEntryAssembly().notNull().Location;
                    if (string.CompareOrdinal(processFileName.Substring(processFileName.Length - 4), ".dll") == 0)
                    {
                        string exeDeployServerFileName = processFileName.Substring(0, processFileName.Length - 3) + "exe";
                        if (File.Exists(exeDeployServerFileName)) processFileName = exeDeployServerFileName;
                    }
                    processFileName = new FileInfo(processFileName).Name;
                }
                FileInfo SwitchFile = new FileInfo(Path.Combine(switchDirectory.FullName, processFileName));
                if (SwitchFile.Exists)
                {
                    FileInfo CurrentFile = new FileInfo(Path.Combine(currentDirectory.FullName, processFileName));
                    if (SwitchFile.LastWriteTimeUtc > CurrentFile.LastWriteTimeUtc) return SwitchFile;
                }
            }
            return null;
        }
    }
}
