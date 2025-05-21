using AutoCSer.CommandService;
using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            FileBlockServiceConfig fileBlockServiceConfig = new FileBlockServiceConfig
            {
                Identity = 1,
                Path = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.DiskBlock))
            };
            Task<DiskBlockService> diskBlockServiceTask = fileBlockServiceConfig.CreateFileBlockService();

            trieGraphNode().NotWait();
            userNameIndexNode().NotWait();
            userRemarkIndexNode().NotWait();
            userNameWordIdentityNode().NotWait();
            userRemarkWordIdentityNode().NotWait();
            searchUserNodeCache().NotWait();

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchQueryService),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IQueryService>(new QueryService())
                .Append<IDiskBlockService>(await diskBlockServiceTask)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine("Press quit to exit.");
                    while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                }
            }
        }

        private static async Task trieGraphNode()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            LocalResult<IStaticTrieGraphNodeLocalClientNode> trieGraphNode = await LocalTrieGraphServiceConfig.StaticTrieGraphNodeCache.GetSynchronousNode();
            if (!trieGraphNode.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{trieGraphNode.CallState}");
                return;
            }
            LocalResult<bool> isGraph = await trieGraphNode.Value.IsGraph();
            if (!isGraph.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{isGraph.CallState}");
                return;
            }
            if (!isGraph.Value)
            {
                string[] words = new string[] { "吹牛", "牛B", "现在", "将来", "曾经", "以后", "努力", "张三丰", "吹牛B", "牛B大王" };
                foreach (string word in words)
                {
                    LocalResult<AppendWordStateEnum> state = await trieGraphNode.Value.AppendWord(word);
                    if (!state.IsSuccess)
                    {
                        AutoCSer.ConsoleWriteQueue.Breakpoint($"{state.CallState}");
                        return;
                    }
                    if (state.Value != AppendWordStateEnum.Success)
                    {
                        AutoCSer.ConsoleWriteQueue.Breakpoint(state.Value.ToString());
                        return;
                    }
                }
                LocalResult<int> wordCount = await trieGraphNode.Value.BuildGraph();
                if (!wordCount.IsSuccess)
                {
                    AutoCSer.ConsoleWriteQueue.Breakpoint($"{wordCount.CallState}");
                    return;
                }
                if (wordCount.Value < words.Length)
                {
                    AutoCSer.ConsoleWriteQueue.Breakpoint($"{wordCount.Value} < {words.Length}");
                    return;
                }
            }

            string text = @"张三丰偷学AutoCSer以后不再吹牛B大王了";
            Console.WriteLine(text);
            LocalResult<WordSegmentResult[]> wordSegmentResults = await trieGraphNode.Value.GetWordSegmentResult(text);
            if (!wordSegmentResults.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{wordSegmentResults.CallState}");
                return;
            }
            foreach (WordSegmentResult result in wordSegmentResults.Value)
            {
                Console.WriteLine($"{result.Identity} {text.Substring(result.StartIndex, result.Length)}");
            }
        }
        private static async Task userNameIndexNode()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            LocalResult<IRemoveMarkHashKeyIndexNodeLocalClientNode<int>> userNameIndexNode = await LocalDiskBlockIndexServiceConfig.UserNameNodeCache.GetNode();
            if (!userNameIndexNode.IsSuccess)
            {
                Console.WriteLine($"{userNameIndexNode.CallState}");
            }
        }
        private static async Task userRemarkIndexNode()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            LocalResult<IRemoveMarkHashKeyIndexNodeLocalClientNode<int>> userRemarkIndexNode = await LocalDiskBlockIndexServiceConfig.UserRemarkNodeCache.GetNode();
            if (!userRemarkIndexNode.IsSuccess)
            {
                Console.WriteLine($"{userRemarkIndexNode.CallState}");
            }
        }
        private static async Task userNameWordIdentityNode()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNodeLocalClientNode<int>> userNameWordIdentityNode = await LocalWordIdentityBlockIndexServiceConfig.UserNameNodeCache.GetNode();
            if (!userNameWordIdentityNode.IsSuccess)
            {
                Console.WriteLine($"{userNameWordIdentityNode.CallState}");
            }
        }
        private static async Task userRemarkWordIdentityNode()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNodeLocalClientNode<int>> userRemarkWordIdentityNode = await LocalWordIdentityBlockIndexServiceConfig.UserRemarkNodeCache.GetNode();
            if (!userRemarkWordIdentityNode.IsSuccess)
            {
                Console.WriteLine($"{userRemarkWordIdentityNode.CallState}");
            }
        }
        private static async Task searchUserNodeCache()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            LocalResult<ISearchUserNodeLocalClientNode> searchUserNodeCache = await LocalSearchUserServiceConfig.SearchUserNodeCache.GetNode();
            if (!searchUserNodeCache.IsSuccess)
            {
                Console.WriteLine($"{searchUserNodeCache.CallState}");
            }
        }
    }
}
