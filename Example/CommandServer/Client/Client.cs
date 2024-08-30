using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Client
{
    /// <summary>
    /// 命令服务示例（客户端）
    /// </summary>
    internal static class Client
    {
        /// <summary>
        /// 启动测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Start()
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Example) };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    AutoCSer.ConsoleWriteQueue.Breakpoint("Client ERROR");
                    return;
                }
                CommandClientSocketEvent socketEvent = (CommandClientSocketEvent)commandClient.SocketEvent;
                await Synchronous.SynchronousController.Call(socketEvent);
                await AsyncTaskQueueContext.SynchronousKeyController.Call(socketEvent);

                Console.WriteLine("OVER");
            }
        }
    }
}
