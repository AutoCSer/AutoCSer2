using System;

namespace AutoCSer.Document.ReverseServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Net.CommandReverseClient server = CommandReverseServer.CommandReverseClient;//引用激活服务端发起连接

            await CommandClientSocketEvent.Test();

            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
        }
    }
}
