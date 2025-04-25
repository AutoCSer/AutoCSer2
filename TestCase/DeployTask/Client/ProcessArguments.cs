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
        internal async Task<Process> Start()
        {
            Console.WriteLine(arguments == null ? fileName : $"{fileName} {string.Join(' ', arguments)}");
            return await new AutoCSer.Diagnostics.ProcessInfo(fileName, arguments).StartAsync();
        }
        /// <summary>
        /// 获取 AutoCSer 日志文件信息
        /// </summary>
        /// <returns></returns>
        internal FileInfo GetLogFileInfo()
        {
            return new FileInfo(Path.Combine(new FileInfo(fileName).Directory.FullName, "AutoCSer.log"));
        }
        /// <summary>
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
