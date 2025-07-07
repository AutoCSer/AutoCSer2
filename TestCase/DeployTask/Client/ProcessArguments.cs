using AutoCSer.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DeployTaskClient
{
    /// <summary>
    /// 带命令行参数的进程调用信息
    /// </summary>
    internal sealed class ProcessArguments
    {
        /// <summary>
        /// 执行进程文件名称
        /// </summary>
        private readonly string fileName;
        /// <summary>
        /// 命令行参数集合
        /// </summary>
        private readonly string[] arguments;
        /// <summary>
        /// 最后一次启动的进程信息
        /// </summary>
        private Process process;
        /// <summary>
        /// 进程名称
        /// </summary>
        private string processName;
        /// <summary>
        /// 进程使用内存大小
        /// </summary>
        private long workingSet64;
        /// <summary>
        /// 进程是否已经退出
        /// </summary>
        private bool isProcessExited;
        /// <summary>
        /// 带命令行参数的进程调用信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        internal ProcessArguments(string fileName, string[] arguments = null)
        {
            this.fileName = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerPath, fileName);
            this.arguments = arguments;
        }
        /// <summary>
        /// 带命令行参数的进程调用信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="argument"></param>
        internal ProcessArguments(string fileName, string argument) : this(fileName, new string[] { argument }) { }
        /// <summary>
        /// 字符串隐式转换
        /// </summary>
        /// <param name="fileName"></param>
        public static implicit operator ProcessArguments(string fileName)
        {
            return new ProcessArguments(fileName);
        }
        /// <summary>
        /// 显示执行进程文件名称并启动进程
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Start()
        {
            Console.WriteLine(arguments == null ? fileName : $"{fileName} {string.Join(' ', arguments)}");
            process = await new AutoCSer.Diagnostics.ProcessInfo(fileName, arguments).StartAsync();
            if (process != null)
            {
                isProcessExited = false;
                processName = process.ProcessName;
                process.Exited += processExited;
                workingSet64 = 0;
                getWorkingSet64().NotWait();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取进程使用内存大小
        /// </summary>
        /// <returns></returns>
        private async Task getWorkingSet64()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            while (!isProcessExited)
            {
                try
                {
                    process.Refresh();
                    long memory = process.WorkingSet64;
                    if (memory > workingSet64) workingSet64 = memory;
                }
                catch { return; }
                await Task.Delay(100);
            }
        }
        /// <summary>
        /// 进程退出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void processExited(object sender, EventArgs e)
        {
            isProcessExited = true;
            process.Dispose();
        }
        /// <summary>
        /// 等待日志文件
        /// </summary>
        /// <returns></returns>
        internal async Task WaitLogFile()
        {
            if (!isProcessExited)
            {
                try
                {
                    await process.WaitForExitAsync();
                }
                catch { }
            }
            if (workingSet64 >= (1 << 28)) Console.WriteLine($"{processName} {workingSet64 >> 20}MB");
            FileInfo logFile = new FileInfo(Path.Combine(new FileInfo(fileName).Directory.FullName, "AutoCSer.log"));
            if (await AutoCSer.Common.FileExists(logFile))
            {
                try
                {
                    using (Process process = await new AutoCSer.Diagnostics.ProcessInfo(logFile.FullName).StartAsync()) await process.WaitForExitAsync();
                    await AutoCSer.Common.DeleteFile(logFile);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        /// <summary>
        /// Determine whether the file exists
        /// 判断文件是否存在
        /// </summary>
        /// <returns></returns>
        internal Task<bool> FileExists()
        {
            return AutoCSer.Common.FileExists(fileName);
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <returns></returns>
        internal FileInfo GetFileInfo()
        {
            return new FileInfo(fileName);
        }
    }
}
