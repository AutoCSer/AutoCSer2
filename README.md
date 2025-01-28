# AutoCSer
[AutoCSer](https://github.com/AutoCSer/AutoCSer2) 是一个 C# 实现的**开箱即用**的 RPC 框架，具有高并发、高吞吐等**高性能**特性，是理想的分布式应用基础设施。

基于 AutoCSer RPC 实现的**面向对象编程**的[内存数据库](https://github.com/AutoCSer/AutoCSer2/tree/main/Application/StreamPersistenceMemoryDatabase)，支持传统数据库级别的**可靠持久化**，持久化 API 具有天然的**事务特性**，支持根据业务逻辑**自定义数据结构节点**，支持**本地嵌入模式**满足高性能游戏局内服务需求。

# 开箱即用的 RPC
## 定义服务接口
``` csharp
    /// <summary>
    /// 接口对称服务定义
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public interface ISymmetryService
    {
        /// <summary>
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> AddAsync(int left, int right);
        /// <summary>
        /// 同步 API 定义（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
```
## 实现服务接口逻辑
``` csharp
    /// <summary>
    /// 接口对称服务
    /// </summary>
    internal sealed class SymmetryService : ISymmetryService
    {
        /// <summary>
        /// 同步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Add(int left, int right) { return left + right; }
        /// <summary>
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> ISymmetryService.AddAsync(int left, int right) { return Task.FromResult(left + right); }
    }
```
## 创建服务监听
``` csharp
            AutoCSer.Net.CommandServerConfig config = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document) 
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListener(config
                , AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<ISymmetryService>(new SymmetryService())))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
```
## 创建 RPC 客户端
``` csharp
        /// <summary>
        /// 测试客户端
        /// </summary>
        private static readonly AutoCSer.Net.CommandClient<ISymmetryService> commandClient = new AutoCSer.Net.CommandClientConfig<ISymmetryService>
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
        }.CreateSymmetryClient();

        /// <summary>
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            var client = await commandClient.GetSocketEvent();
            if (client != null)
            {
                Console.WriteLine($"2 + 3 = {await client.InterfaceController.AddAsync(2, 3)}");
                Console.WriteLine($"1 + 2 = {client.InterfaceController.Add(1, 2)}");
            }
        }
```

RPC 是 AutoCSer 的核心基础组件，[高并发吞吐测试性能](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/CommandServerPerformance)超过 [.NET gRPC](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/ThirdParty/GrpcClientPerformance) 两个数量级。
1. [从接口对称 RPC 开始](https://zhuanlan.zhihu.com/p/8581138677) - [1.SymmetryService](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/SymmetryService)
2. [数据序列化](https://zhuanlan.zhihu.com/p/8762985779) - [2.ServiceDataSerialize](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/ServiceDataSerialize)
3. [线程调度策略](https://zhuanlan.zhihu.com/p/10102634904) - [3.ServiceThreadStrategy](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/ServiceThreadStrategy)
4. [鉴权与传输数据编码](https://zhuanlan.zhihu.com/p/11427440200) - [4.ServiceAuthentication](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/ServiceAuthentication)
5. [服务信息注册与推送](https://zhuanlan.zhihu.com/p/19143730420) - [9.ServerRegistry](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/ServerRegistry)
6. [反向 RPC 服务](https://zhuanlan.zhihu.com/p/20033747254) - [10.ReverseServer](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/ReverseServer)

# 面向对象编程的内存数据库
## 服务端配置定义
``` csharp
    /// <summary>
    /// 日志流持久化内存数据库服务端配置
    /// </summary>
    internal sealed class ServiceConfig : AutoCSer.CommandService.StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// 测试环境删除 15 分钟以前的历史持久化文件，生产环境根据实际需求处理
        /// </summary>
        /// <returns></returns>
        public override DateTime GetRemoveHistoryFileTime()
        {
            return AutoCSer.Threading.SecondTimer.UtcNow.AddMinutes(-15);
        }
        /// <summary>
        /// 测试环境每分钟执行一次删除历史持久化文件操作，生产环境根据实际需求处理
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override void RemoveHistoryFile(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).NotWait();
        }
        /// <summary>
        /// 重建文件大小设置为至少 10MB
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override bool CheckRebuild(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 10 << 20;
        }
    }
```
## 创建服务端实例
``` csharp
            AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig databaseServiceConfig = new AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseNode)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseNode) + nameof(AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();
```
## 创建 RPC 服务
``` csharp
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
```
## 定义 RPC 客户端套接字事件配置
``` csharp
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
```
## 创建 RPC 客户端
``` csharp
        /// <summary>
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
```

基于 AutoCSer RPC 实现的内存数据库，[高并发吞吐测试性能](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/StreamPersistenceMemoryDatabase/Performance)远高于 [Garnet + StackExchange.Redis](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/ThirdParty/RedisPerformance) 组合。
1. [AutoCSer 内存数据库与 Redis 的区别](https://zhuanlan.zhihu.com/p/13167457731)
2. [入门 - 使用内置数据结构](https://zhuanlan.zhihu.com/p/14011804562) - [6.MemoryDatabaseNode](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/MemoryDatabaseNode)
3. [自定义数据结构](https://zhuanlan.zhihu.com/p/15454610569) - [7.MemoryDatabaseCustomNode](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/MemoryDatabaseCustomNode)
4. [本地嵌入模式](https://zhuanlan.zhihu.com/p/16409903680) - [8.MemoryDatabaseLocalService](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/MemoryDatabaseLocalService)
5. [服务信息注册与推送](https://zhuanlan.zhihu.com/p/19143730420) - [9.ServerRegistry](https://github.com/AutoCSer/AutoCSer2/tree/main/Document/ServerRegistry)
