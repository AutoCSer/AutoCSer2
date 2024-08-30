using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandListener commandListener = await Server.Listener.Start();
            if (commandListener != null)
            {
                Console.WriteLine("Press quit to exit.");
                AutoCSer.Threading.CatchTask.AddIgnoreException(Client.Client.Start());
                while (Console.ReadLine() != "quit") ;
                commandListener.Dispose();
                return;
            }
            AutoCSer.ConsoleWriteQueue.Breakpoint("ERROR");
            Console.ReadKey();
        }
    }
}
