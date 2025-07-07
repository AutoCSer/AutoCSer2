using System;

namespace AutoCSer.Document.ReverseServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            //The reference activates the server to initiate the connection
            //引用激活服务端发起连接
            AutoCSer.Net.CommandReverseClient server = CommandReverseServer.CommandReverseClient;

            await CommandClientSocketEvent.Test();

            Console.WriteLine("Completed");
            Console.ReadKey();
        }
    }
}
