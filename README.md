# AutoCSer - [English](https://github.com/AutoCSer/AutoCSer2/blob/main/README.Eng.md)
AutoCSer 是一个 C# 实现的分布式应用基础设施框架，其核心是基于 TCP 长连接实现的**全双工 RPC 组件**，具有**高并发**、**高吞吐**等**高性能**特性，客户端提供 [.NET NativeAOT 支持](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.md)。  
AutoCSer RPC 在轻量级 API 的高并发实战环境中，单节点可提供 **100W+/s 以上**的 QPS 吞吐性能，[测试吞吐性能](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/CommandServerPerformance)超过 [.NET gRPC](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/ThirdParty/GrpcClientPerformance) **一个数量级**。  
基于 AutoCSer RPC 实现的**支持面向对象编程**的[内存数据库](https://github.com/AutoCSer/AutoCSer2/tree/main/Application/StreamPersistenceMemoryDatabase)，支持传统数据库级别的**可靠持久化**，持久化 API 具有天然的**事务特性**，**本地嵌入模式**[支持 .NET NativeAOT](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.md) 可满足高性能游戏局内服务需求。  
AutoCSer 内存数据库在轻量级 API 的高并发实战环境中，单节点可提供 **100W+/s 以上**的 TPS 吞吐性能，[测试吞吐性能](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/StreamPersistenceMemoryDatabase/Performance)远高于 [Garnet + StackExchange.Redis](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/ThirdParty/RedisPerformance) 组合。  
NuGet 发布了核心程序集 AutoCSer.dll 的 3 个版本包括 [.NET8](https://www.nuget.org/packages/AutoCSer.NET8/) | [.NET Standard 2.1](https://www.nuget.org/packages/AutoCSer2.1/) | [.NET Standard 2.0](https://www.nuget.org/packages/AutoCSer2/)，.NET Framework 4.5 版本以及其它项目需要到 github 下载源代码自行编译。
# 开箱即用的 RPC
## 定义服务接口
``` csharp
    /// <summary>
    /// 接口对称 API 接口定义
    /// </summary>
    public interface ISymmetryService
    {
        /// <summary>
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> AddAsync(int left, int right);
        /// <summary>
        /// 同步 API 定义（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
```
## 实现服务接口
``` csharp
    /// <summary>
    /// 接口对称 API 实现
    /// </summary>
    internal sealed class SymmetryService : ISymmetryService
    {
        /// <summary>
        /// 异步 API 实现
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> ISymmetryService.AddAsync(int left, int right) { return Task.FromResult(left + right); }
        /// <summary>
        /// 同步 API 实现
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Add(int left, int right) { return left + right; }
    }
```
## 创建服务端监听
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
                    Console.ReadLine();
                }
            }
```
## 创建 RPC 客户端
``` csharp
        /// <summary>
        /// 测试客户端单例（全双工长连接只需要创建一个客户端）
        /// </summary>
        private static readonly AutoCSer.Net.CommandClient<ISymmetryService> commandClient = new AutoCSer.Net.CommandClientConfig<ISymmetryService>
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
        }.CreateSymmetryClient();

        /// <summary>
        /// Client test
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            var client = await commandClient.GetSocketEvent();
            if (client != null)
            {
                Console.WriteLine($"2 + 3 = {await client.InterfaceController.AddAsync(2, 3)}");
                Console.WriteLine($"1 + 2 = {client.InterfaceController.Add(1, 2)}");
                Console.WriteLine("Completed");
            }
        }
```
1. [接口对称 API](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/01.SymmetryService/01.SymmetryService.md)
2. [数据序列化](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/02.ServiceDataSerialize/02.ServiceDataSerialize.md)
3. [线程调度策略](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/03.ServiceThreadStrategy/03.ServiceThreadStrategy.md)
4. [鉴权与传输数据编码](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/04.ServiceAuthentication/04.ServiceAuthentication.md)
5. [静态代码生成](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/05.CodeGenerator/05.CodeGenerator.md)
6. [服务注册与推送](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/10.ServerRegistry/10.ServerRegistry.md)
7. [反向 RPC 服务](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/11.ReverseServer/11.ReverseServer.md)
8. [客户端 .NET NativeAOT](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.md)

# 内存数据库
## 数据库服务配置定义
``` csharp
    /// <summary>
    /// 内存数据库服务配置
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
            new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).Catch();
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
## 创建数据库服务实例
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
                    Console.ReadKey();
                }
            }
```
## 定义 RPC 客户端实例
``` csharp
    /// <summary>
    /// RPC 客户端实例
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 内存数据库客户端接口实例
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient));
                //yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IReadWriteQueueService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// RPC 客户端实例
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
    }
```
## 创建 RPC 客户端
``` csharp
        /// <summary>
        /// 内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
```
1. [内存数据库简介](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/06.MemoryDatabase/06.MemoryDatabase.md)
2. [内置数据结构节点](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/07.MemoryDatabaseNode/07.MemoryDatabaseNode.md)
3. [自定义节点](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/08.MemoryDatabaseCustomNode/08.MemoryDatabaseCustomNode.md)
4. [本地嵌入模式](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/09.MemoryDatabaseLocalService/09.MemoryDatabaseLocalService.md)
5. [静态代码生成](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/05.CodeGenerator/05.CodeGenerator.md)
6. [服务注册与推送](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/10.ServerRegistry/10.ServerRegistry.md)
7. [本地嵌入模式 .NET NativeAOT](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.md)

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/AutoCSer/AutoCSer2)