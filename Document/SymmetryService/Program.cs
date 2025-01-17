using AutoCSer.Extensions;

namespace AutoCSer.Document.SymmetryService
{
    internal class Program
    {
        /// <summary>
        /// https://zhuanlan.zhihu.com/p/8581138677
        /// </summary>
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Net.CommandServerConfig config = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document) 
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListener(config
                , AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<ISymmetryService>(new SymmetryService())))
            {
                if (await commandListener.Start())
                {
                    Client.Test().NotWait();

                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
    }
}
