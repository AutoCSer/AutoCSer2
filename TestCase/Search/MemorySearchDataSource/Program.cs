using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode;
using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDataSource
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            userMessageNode().NotWait();

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchDataSource),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IUserService>(new UserService())
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine("Press quit to exit.");
                    while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                }
            }
        }
        private static async Task userMessageNode()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            LocalResult<ITimeoutMessageNodeLocalClientNode<OperationData<int>>> result = await UserMessageNode.ServiceConfig.UserMessageNodeCache.GetNode();
            if (!result.IsSuccess)
            {
                Console.WriteLine($"{result.CallState}");
            }
        }
    }
}
