using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ReverseLogCollection),
                ServerName = LogInfo.ServerName
            };
            await using (CommandReverseListener commandListener = new CommandReverseListener(commandClientConfig))
            {
                if (await commandListener.Start())
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => Test(commandListener));
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="commandListener"></param>
        private static void Test(CommandReverseListener commandListener)
        {
            long MessageCount = 0;
            do
            {
                commandListener.Append(new LogInfo { LogTime = AutoCSer.Threading.SecondTimer.Now, Message = $"第 {++MessageCount} 条消息" });
                Console.WriteLine(MessageCount);
                System.Threading.Thread.Sleep(2000);
            }
            while (true);
        }
    }
}
