using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchTrieGraph
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            ServiceConfig databaseServiceConfig = new ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.SearchTrieGraph)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.SearchTrieGraph) + nameof(ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create<IStaticTrieGraphServiceNode>(p => new StaticTrieGraphServiceNode(p));

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchTrieGraph),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    await appendWords();

                    Console.WriteLine("Press quit to exit.");
                    while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                }
            }
        }
        private static async Task appendWords()
        {
            if (!await TrieGraphCommandClientSocketEvent.AppendWords()) return;

            ResponseResult<IStaticTrieGraphNodeClientNode> trieGraphNodeResult = await TrieGraphCommandClientSocketEvent.StaticTrieGraphNodeCache.GetNode();
            if (!trieGraphNodeResult.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{trieGraphNodeResult.ReturnType} {trieGraphNodeResult.CallState}");
                return;
            }
            string text = @"张三丰偷学AutoCSer以后不再吹牛B大王了";
            Console.WriteLine(text);
            ResponseResult<WordSegmentResult[]> wordSegmentResults = await trieGraphNodeResult.Value.GetWordSegmentResult(text);
            if (!wordSegmentResults.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{wordSegmentResults.ReturnType} {wordSegmentResults.CallState}");
                return;
            }
            foreach (WordSegmentResult result in wordSegmentResults.Value)
            {
                Console.WriteLine($"{result.Identity} {text.Substring(result.StartIndex, result.Length)}");
            }
        }
    }
}
