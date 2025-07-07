using AutoCSer.Diagnostics;
using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Switch processes
    /// 切换进程
    /// </summary>
    public abstract class ProcessGuardSwitchProcess
    {
        /// <summary>
        /// 默认切换服务目录后缀名称
        /// </summary>
        public const string DefaultSwitchDirectorySuffixName = ".SwitchProcess";

        /// <summary>
        /// Main 函数传参
        /// </summary>
        protected readonly string[] arguments;
        /// <summary>
        /// 切换进程关键字，默认为当前进程名称
        /// </summary>
        protected virtual string switchProcessKey 
        {
            get
            {
                if (AutoCSer.Diagnostics.ProcessInfo.IsCurrentProcessDotNet())
                {
                    string fileName = Environment.GetCommandLineArgs()[0];
                    int index = fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1;
                    return index != 0 ? fileName.Substring(index) : fileName;
                }
                return AutoCSer.Common.CurrentProcess.ProcessName;
            }
        }
        /// <summary>
        /// 执行进程文件名称，默认为当前执行文件
        /// </summary>
        protected virtual string processFileName
        {
            get { return ProcessInfo.GetCurrentProcessFileName(); }
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
        /// Switch processes
        /// 切换进程
        /// </summary>
        /// <param name="arguments">Main 函数参数</param>
        protected ProcessGuardSwitchProcess(string[] arguments)
        {
            this.arguments = arguments;
        }
        /// <summary>
        /// 获取切换执行进程文件信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        protected virtual async Task<FileInfo?> getSwitchProcessFile()
#else
        protected virtual async Task<FileInfo> getSwitchProcessFile()
#endif
        {
            FileInfo processFile = new FileInfo(processFileName);
            var directory = processFile.Directory;
            if (directory == null) return null;
            var parent = directory.Parent;
            if (parent == null) return null;
            string directoryName = directory.Name;
            DirectoryInfo switchDirectory = new DirectoryInfo(Path.Combine(parent.FullName, directoryName + ProcessInfo.DefaultSwitchDirectorySuffixName));
            if (await AutoCSer.Common.DirectoryExists(switchDirectory)) return await getSwitchProcessFile(switchDirectory, processFile);
            if (directoryName.EndsWith(ProcessInfo.DefaultSwitchDirectorySuffixName, StringComparison.Ordinal))
            {
                int nameLength = directoryName.Length - ProcessInfo.DefaultSwitchDirectorySuffixName.Length;
                if (nameLength > 0)
                {
                    switchDirectory = new DirectoryInfo(Path.Combine(parent.FullName, directoryName.Substring(0, nameLength)));
                    if (await AutoCSer.Common.DirectoryExists(switchDirectory)) return await getSwitchProcessFile(switchDirectory, processFile);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取切换执行进程文件信息
        /// </summary>
        /// <param name="directoryName">匹配目录名称</param>
        /// <returns></returns>
#if NetStandard21
        protected async Task<FileInfo?> getSwitchProcessFile(string directoryName)
#else
        protected async Task<FileInfo> getSwitchProcessFile(string directoryName)
#endif
        {
            FileInfo processFile = new FileInfo(processFileName);
            var directory = processFile.Directory;
            if (directory == null) return null;
            LeftArray<string> directoryNames = new LeftArray<string>(5);
            do
            {
                directoryNames.Append(directory.Name);
                if (directory.Name == directoryName) break;
                if ((directory = directory.Parent) == null) return null;
            }
            while (true);
            if ((directory = directory.Parent) == null) return null;
            var parent = directory.Parent;
            if (parent == null) return null;
            DirectoryInfo switchDirectory = new DirectoryInfo(Path.Combine(parent.FullName, (directoryName = directory.Name) + ProcessInfo.DefaultSwitchDirectorySuffixName));
            if (await AutoCSer.Common.DirectoryExists(switchDirectory)) return await getSwitchProcessFile(switchDirectory, directoryNames, processFile);
            if (directoryName.EndsWith(ProcessInfo.DefaultSwitchDirectorySuffixName, StringComparison.Ordinal))
            {
                int nameLength = directoryName.Length - ProcessInfo.DefaultSwitchDirectorySuffixName.Length;
                if (nameLength > 0)
                {
                    switchDirectory = new DirectoryInfo(Path.Combine(parent.FullName, directoryName.Substring(0, nameLength)));
                    if (await AutoCSer.Common.DirectoryExists(switchDirectory)) return await getSwitchProcessFile(switchDirectory, directoryNames, processFile);
                }
            }
            return null;
        }
        /// <summary>
        /// 切换进程检查
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<bool> switchProcess()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            var switchProcessFile = await getSwitchProcessFile();
            if (switchProcessFile != null)
            {
                if (AutoCSer.Diagnostics.ProcessInfo.IsCurrentProcessDotNet()) new ProcessInfo(switchProcessFile).Start();
                else new ProcessInfo(switchProcessFile, arguments, AutoCSer.Common.CurrentProcess).Start();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected async Task<ResponseResult> start()
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
                        client.ErrorMessage = "Get guard node failed";
                        await onError(client);
                        await initialize();
                        trySwitch().NotWait();
                        await onStart();
                        return client;
                    }
                    await initialize();
                    string switchProcessKey = this.switchProcessKey;
                    ResponseParameterAwaiter<bool> switchAwaiter = client.Value.Switch(switchProcessKey);
                    if (isGuard) guard().NotWait();
                    await onStart();
                    bool isConsole = true;
                    do
                    {
                        ResponseResult<bool> result = await switchAwaiter;
                        if (result.IsSuccess) break;
                        if (isConsole) Console.WriteLine($"SwitchError {switchProcessKey} {result.ReturnType}.{result.CallState}");
                        isConsole = false;
                        await Task.Delay(1);
                        switchAwaiter = client.Value.Switch(switchProcessKey);
                    }
                    while (true);
                    await exit();
                    return CallStateEnum.Success;
                }
                catch (Exception exception)
                {
                    await onException(exception);
                }
                return CallStateEnum.Unknown;
            }
            return CallStateEnum.Success;
        }
        /// <summary>
        /// 错误信息处理
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual async Task onError(ResponseResult<IProcessGuardNodeClientNode> result)
        {
            string message = $"{result.ErrorMessage} ReturnType[{result.ReturnType}] CallState[{result.CallState}]";
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
                    await exit();
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
            ProcessGuardInfo processGuardInfo = ProcessGuardInfo.GetCurrent(arguments);
            bool isConsole = true;
            do
            {
                ResponseResult<IProcessGuardNodeClientNode> client = await getProcessGuardClient.GetNode();
                if (client.Value != null)
                {
                    if (isExit) return;
                    ResponseResult<bool> result = await client.Value.Guard(processGuardInfo);
                    if (result.Value)
                    {
                        Console.WriteLine("添加守护成功");
                        return;
                    }
                    if (result.IsSuccess)
                    {
                        Console.WriteLine("添加守护失败");
                        return;
                    }
                    if (isConsole) Console.WriteLine($"GuardError {result.ReturnType}.{result.CallState}");
                    isConsole = false;
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
        /// 获取切换执行进程文件信息
        /// </summary>
        /// <param name="switchDirectory"></param>
        /// <param name="processFile"></param>
        /// <returns></returns>
#if NetStandard21
        private static async Task<FileInfo?> getSwitchProcessFile(DirectoryInfo switchDirectory, FileInfo processFile)
#else
        private static async Task<FileInfo> getSwitchProcessFile(DirectoryInfo switchDirectory, FileInfo processFile)
#endif
        {
            FileInfo switchProcessFile = new FileInfo(Path.Combine(switchDirectory.FullName, processFile.Name));
            return await AutoCSer.Common.FileExists(switchProcessFile) && switchProcessFile.LastWriteTimeUtc > processFile.LastWriteTimeUtc ? switchProcessFile : null;
        }
        /// <summary>
        /// 获取切换执行进程文件信息
        /// </summary>
        /// <param name="switchDirectory"></param>
        /// <param name="directoryNames"></param>
        /// <param name="processFile"></param>
        /// <returns></returns>
#if NetStandard21
        private static async Task<FileInfo?> getSwitchProcessFile(DirectoryInfo switchDirectory, LeftArray<string> directoryNames, FileInfo processFile)
#else
        private static async Task<FileInfo> getSwitchProcessFile(DirectoryInfo switchDirectory, LeftArray<string> directoryNames, FileInfo processFile)
#endif
        {
            directoryNames.Add(switchDirectory.FullName);
            directoryNames.Reverse();
            directoryNames.Add(processFile.Name);
            FileInfo switchProcessFile = new FileInfo(Path.Combine(directoryNames.ToArray()));
            return await AutoCSer.Common.FileExists(switchProcessFile) && switchProcessFile.LastWriteTimeUtc > processFile.LastWriteTimeUtc ? switchProcessFile : null;
        }
    }
}
