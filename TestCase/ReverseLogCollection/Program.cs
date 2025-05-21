using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollection
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            ReverseLogCollectionService<LogInfo> controller = new ReverseLogCollectionService<LogInfo>();
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ReverseLogCollection), ServerName = LogInfo.ServerName };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IReverseLogCollectionService<LogInfo>>(controller)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => Test(controller));
                    Console.WriteLine("Press quit to exit.");
                    while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                }
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="controller"></param>
        private static void Test(ReverseLogCollectionService<LogInfo> controller)
        {
            long MessageCount = 0;
            do
            {
                controller.Appped(new LogInfo { LogTime = AutoCSer.Threading.SecondTimer.Now, Message = $"第 {++MessageCount} 条消息" });
                Console.WriteLine(MessageCount);
                System.Threading.Thread.Sleep(2000);
            }
            while (true);
        }
    }
}
