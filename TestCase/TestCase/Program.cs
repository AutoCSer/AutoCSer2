using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AutoCSer.FieldEquals.Comparor.IsBreakpoint = true;
            Type errorType = typeof(Program);
            do
            {
                if (!BinarySerialize.TestCase()) { errorType = typeof(BinarySerialize); break; }
                if (!Json.TestCase()) { errorType = typeof(Json); break; }
                if (!Xml.TestCase()) { errorType = typeof(Xml); break; }
                if (!await CommandServer.TestCase()) { errorType = typeof(CommandServer); break; }
                Console.Write('.');
            }
            while (true);

            ConsoleWriteQueue.WriteLine(errorType.FullName + " ERROR", ConsoleColor.Red);
            Console.ReadKey();
        }
    }
}
