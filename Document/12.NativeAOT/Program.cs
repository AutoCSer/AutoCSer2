using AutoCSer.Document.NativeAOT.Service;
using AutoCSer.Extensions;

namespace AutoCSer.Document.NativeAOT
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Net.CommandServerConfig config = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document)
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<IServiceController>(new ServiceController())
                .CreateCommandListener(config))
            {
                if (!await commandListener.Start())
                {
                    Console.WriteLine($"{nameof(ServiceController)} start failed.");
                }
                Console.ReadKey();
            }
        }
    }
}
