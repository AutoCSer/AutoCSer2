using AutoCSer.CommandService;
using AutoCSer.Deploy;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ProcessGuardSwitchProcess
{
    class Program : AutoCSer.CommandService.ProcessGuardSwitchProcess
    {
        private Program(string[] args) : base(args) { }
        static async Task Main(string[] args)
        {
            try
            {
                await new Program(args).Start();
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.WriteLine(exception.ToString(), ConsoleColor.Red);
                Console.ReadLine();
            }
        }
        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <returns></returns>
        protected override async Task initialize()
        {
            //初始化操作
            await base.initialize();
        }
        /// <summary>
        /// 获取守护进程客户端配置
        /// </summary>
        /// <returns></returns>
        protected override CommandClientConfig getCommandClientConfig()
        {
            return new ProcessGuardCommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ProcessGuard) };
        }
        /// <summary>
        /// 创建守护进程客户端后续处理
        /// </summary>
        /// <returns></returns>
        protected override Task onProcessGuardClient()
        {
            AutoCSer.Threading.CatchTask.AddIgnoreException(startSwitch());
            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
            exitLock.Exit();
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 模拟发布启动切换进程
        /// </summary>
        /// <returns></returns>
        private async Task startSwitch()
        {
            await Task.Yield();
            DirectoryInfo directory = AutoCSer.Common.ApplicationDirectory;
            if (directory.Name == SwitchProcess.DefaultSwitchDirectoryName) directory = directory.Parent;
            else
            {
                DirectoryInfo switchDirectory = new DirectoryInfo(Path.Combine(directory.FullName, SwitchProcess.DefaultSwitchDirectoryName));
                if (!await AutoCSer.Common.DirectoryExists(switchDirectory)) await AutoCSer.Common.TryCreateDirectory(switchDirectory);
                foreach(FileInfo file in await AutoCSer.Common.DirectoryGetFiles(directory))
                {
                    try
                    {
                        await AutoCSer.Common.FileCopyTo(file, Path.Combine(switchDirectory.FullName, file.Name));
                    }
                    catch { }
                }
                directory = switchDirectory;
            }
            string switchFile = Path.Combine(directory.FullName, new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Name);
            Console.WriteLine(switchFile);
            if (!await AutoCSer.Common.FileExists(switchFile))
            {
                ConsoleWriteQueue.WriteLine("没有找到切换进程文件", ConsoleColor.Red);
                return;
            }
            for (int count = 10; count != 0;)
            {
                Console.Write(--count);
                await Task.Delay(1000);
            }
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("dotnet", switchFile);
            info.UseShellExecute = true;
            info.WorkingDirectory = directory.FullName;
            System.Diagnostics.Process.Start(info);
        }
    }
}
