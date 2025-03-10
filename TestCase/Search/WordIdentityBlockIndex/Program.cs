using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchCommon;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            ServiceConfig databaseServiceConfig = new ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.SearchWordIdentityBlockIndex)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.SearchWordIdentityBlockIndex) + nameof(ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create<IServiceNode>(p => new ServiceNode(p));

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchWordIdentityBlockIndex),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    ResponseResult<IWordIdentityBlockIndexNodeClientNode<int>> node = await CommandClientSocketEvent.UserNameNodeCache.GetNode();
                    if (!node.IsSuccess) Console.WriteLine($"{node.ReturnType} {node.CallState}");
                    node = await CommandClientSocketEvent.UserRemarkNodeCache.GetNode();
                    if (!node.IsSuccess) Console.WriteLine($"{node.ReturnType} {node.CallState}");
                    ResponseResult<ISearchUserNodeClientNode> userNode = await CommandClientSocketEvent.SearchUserNodeCache.GetNode();
                    if (!userNode.IsSuccess) Console.WriteLine($"{userNode.ReturnType} {userNode.CallState}");

                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
    }
}
