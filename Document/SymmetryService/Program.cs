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
            AutoCSer.Net.CommandServerConfig config = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document) 
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListener(config
                , AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<ISymmetryService>(new SymmetryService())))
            {
                if (await commandListener.Start())
                {
                    client().NotWait();
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
        /// <summary>
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task client()
        {
            var config = new AutoCSer.Net.CommandClientConfig<ISymmetryService>
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            using (AutoCSer.Net.CommandClient<ISymmetryService> commandClient = config.CreateSymmetryClient())
            {
                var client = await commandClient.GetSocketEvent();
                if (client != null)
                {
                    Console.WriteLine($"2 + 3 = {await client.InterfaceController.AddAsync(2, 3)}");
                    Console.WriteLine($"1 + 2 = {client.InterfaceController.Add(1, 2)}");
                }
            }
        }
    }
}
