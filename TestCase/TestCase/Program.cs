using AutoCSer;
using AutoCSer.Threading;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class Program
    {
#if NetStandard21
        static async Task Main(string[] args)
#else
        static void Main(string[] args)
#endif
        {
#if NetStandard21
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await test();
#else
            new UISynchronousTask(test).Wait();
#endif
        }
        private static async Task test()
        {
            AutoCSer.FieldEquals.Comparor.IsBreakpoint = true;
            Type errorType = typeof(Program);
            do
            {
                if (!BinarySerialize.TestCase()) { errorType = typeof(BinarySerialize); break; }
                if (!Json.TestCase()) { errorType = typeof(Json); break; }
                if (!Xml.TestCase()) { errorType = typeof(Xml); break; }
                if (!await CommandServer.TestCase()) { errorType = typeof(CommandServer); break; }
                if (!await CommandReverseServer.TestCase()) { errorType = typeof(CommandReverseServer); break; }
                if (!await InterfaceControllerTaskQueue.TestCase()) { errorType = typeof(InterfaceControllerTaskQueue); break; }
                Console.Write('.');
            }
            while (true);
            ConsoleWriteQueue.WriteLine(errorType.FullName + " ERROR", ConsoleColor.Red);
            Console.ReadKey();
        }
    }
}
