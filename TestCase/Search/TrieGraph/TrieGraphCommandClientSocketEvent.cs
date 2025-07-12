using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchTrieGraph
{
    /// <summary>
    /// 搜索 Trie 图分词服务客户端套接字事件
    /// </summary>
    internal sealed class TrieGraphCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<TrieGraphCommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Sample interface of service authentication client based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public TrieGraphCommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, AutoCSer.TestCase.Common.Config.TimestampVerifyString));
        }

        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IStaticTrieGraphServiceNodeClientNode, TrieGraphCommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IStaticTrieGraphServiceNodeClientNode, TrieGraphCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchTrieGraph),
            GetSocketEventDelegate = (client) => new TrieGraphCommandClientSocketEvent(client)
        });
        /// <summary>
        /// 字符串 Trie 图节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IStaticTrieGraphNodeClientNode> StaticTrieGraphNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IStaticTrieGraphNodeClientNode>(nameof(StaticTrieGraphNode), (index, key, nodeInfo) => client.ClientNode.CreateStaticTrieGraphNode(index, key, nodeInfo, 8, 64, 0, null)));

        /// <summary>
        /// 添加词语
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> AppendWords()
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await StaticTrieGraphNodeCache.GetSynchronousNode();
            if (!nodeResult.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{nodeResult.ReturnType} {nodeResult.CallState}");
                return false;
            }
            IStaticTrieGraphNodeClientNode node = nodeResult.Value;
            ResponseResult<bool> isGraph = await node.IsGraph();
            if (!isGraph.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{isGraph.ReturnType} {isGraph.CallState}");
                return false;
            }
            if (isGraph.Value) return true;

            string[] words = new string[] { "吹牛", "牛B", "现在", "将来", "曾经", "以后", "努力", "张三丰", "吹牛B", "牛B大王" };
            foreach(string word in words)
            {
                ResponseResult<AppendWordStateEnum> state = await node.AppendWord(word);
                if (!state.IsSuccess)
                {
                    AutoCSer.ConsoleWriteQueue.Breakpoint($"{state.ReturnType} {state.CallState}");
                    return false;
                }
                if (state.Value != AppendWordStateEnum.Success)
                {
                    AutoCSer.ConsoleWriteQueue.Breakpoint(state.Value.ToString());
                    return false;
                }
            }
            ResponseResult<int> wordCount = await node.BuildGraph();
            if (!wordCount.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{wordCount.ReturnType} {wordCount.CallState}");
                return false;
            }
            if (wordCount.Value < words.Length)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{wordCount.Value} < {words.Length}");
                return false;
            }
            return true;
        }
    }
}
