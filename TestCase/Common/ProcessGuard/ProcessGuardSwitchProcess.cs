using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 切换进程
    /// </summary>
    public abstract class ProcessGuardSwitchProcess : AutoCSer.CommandService.ProcessGuardSwitchProcess
    {
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="args"></param>
        protected ProcessGuardSwitchProcess(string[] args) : base(args) { }
        /// <summary>
        /// 获取守护进程客户端配置
        /// </summary>
        /// <returns></returns>
        protected override CommandClientConfig getCommandClientConfig()
        {
            return new ProcessGuardCommandClientConfig { Host = new HostEndPoint((ushort)CommandServerPortEnum.ProcessGuard) };
        }
        /// <summary>
        /// 创建守护进程客户端后续处理
        /// </summary>
        /// <returns></returns>
        protected override Task onProcessGuardClient()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(consoleCommand);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 控制台命令处理
        /// </summary>
        private void consoleCommand()
        {
            do
            {
                try
                {
                    do
                    {
                        ConsoleWriteQueue.WriteLine($"{DateTime.Now.toString()}\r\nclear cache / quit");
                        string command = Console.ReadLine();
                        switch (command)
                        {
                            case "quit": exitLock.Exit(); return;
                            case "clear cache": CatchTask.AddIgnoreException(AutoCSer.Memory.Common.ClearCache()); break;
                            default: onUnknownConsoleCommand(command); break;
                        }
                    }
                    while (true);
                }
                catch (Exception exception)
                {
                    ConsoleWriteQueue.WriteLine(exception.ToString(), ConsoleColor.Red);
                }
            }
            while (true);
        }
        /// <summary>
        /// 未知控制台命令处理
        /// </summary>
        /// <param name="command"></param>
        protected virtual void onUnknownConsoleCommand(string command) { }

        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="SwitchProcess"></param>
        /// <returns></returns>
        protected static async Task start(ProcessGuardSwitchProcess switchProcess)
        {
            try
            {
                await switchProcess.Start();
            }
            catch (Exception exception)
            {
                ConsoleWriteQueue.WriteLine(exception.ToString(), ConsoleColor.Red);
                ConsoleWriteQueue.WriteLine("进程启动异常，按回车退出程序！", ConsoleColor.Red);
                await AutoCSer.LogHelper.Flush();
                Console.ReadLine();
            }
        }
    }
}
