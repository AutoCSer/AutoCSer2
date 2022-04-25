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
            ReverseLogCollectionService<LogInfo> controller = new ReverseLogCollectionService<LogInfo>();
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.ReverseLogCollection), ServiceName = LogInfo.ServiceName };
            using (CommandListener commandListener = new CommandListener(commandServerConfig
                , CommandServerInterfaceControllerCreator.GetCreator(server => (ITimestampVerifyService)new AutoCSer.CommandService.TimestampVerifyService(AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                , CommandServerInterfaceControllerCreator.GetCreator(server => (IReverseLogCollectionService<LogInfo>)controller)
                ))
            {
                if (await commandListener.Start())
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => Test(controller));
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
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
