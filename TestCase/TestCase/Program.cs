using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Type errorType = typeof(Program);
            do
            {
                if (!BinarySerialize.TestCase()) { errorType = typeof(BinarySerialize); break; }
                if (!Json.TestCase()) { errorType = typeof(Json); break; }
                if (!await CommandServer.TestCase()) { errorType = typeof(CommandServer); break; }
                Console.Write('.');
            }
            while (true);

            Console.WriteLine(errorType.FullName + " ERROR");
            Console.ReadKey();
        }
        /// <summary>
        /// 测试错误断点
        /// </summary>
        /// <returns></returns>
        internal static bool Breakpoint()
        {
            return false;
        }
    }
}
