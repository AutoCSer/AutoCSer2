# AutoCSer
AutoCSer is a distributed application infrastructure framework implemented in C#. Its core is a **full-duplex RPC component** based on TCP long connections, featuring **high concurrency**, **high throughput** and other **high-performance** characteristics. The client provides [.NET NativeAOT support](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.Eng.md).  
In the high-concurrency practical environment of lightweight apis, AutoCSer RPC can provide a QPS throughput performance of **over 1 million per second** for a single node. [The test throughput performance](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/CommandServerPerformance) is **one order of magnitude higher than** [.NET gRPC](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/ThirdParty/GrpcClientPerformance).  
The [in-memory database](https://github.com/AutoCSer/AutoCSer2/tree/main/Application/StreamPersistenceMemoryDatabase) that **supports object-oriented programming** and is implemented based on AutoCSer RPC. It **supports reliable persistence** at the level of traditional databases. The persistence API has **inherent transaction characteristics**. The **local embedding mode** [supports .NET NativeAOT](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.Eng.md), which can meet the requirements of high-performance in-game services.  
In the high-concurrency practical environment of lightweight apis, the AutoCSer in-memory database can provide a TPS throughput performance of **over 1 million per second** on a single node. The [test throughput performance](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/StreamPersistenceMemoryDatabase/Performance) is much higher than the [Garnet + StackExchange.Redis](https://github.com/AutoCSer/AutoCSer2/tree/main/TestCase/ThirdParty/RedisPerformance) combination.  
NuGet has released three versions of its core assembly AutoCSer.dll, including [.NET8](https://www.nuget.org/packages/AutoCSer.NET8/) | [.NET Standard 2.1](https://www.nuget.org/packages/AutoCSer2.1/) | [.NET Standard 2.0](https://www.nuget.org/packages/AutoCSer2/). For the version .NET Framework 4.5 and other projects, you need to download the source code from github and compile it yourself.
# Out-of-the-box RPC
## Define the service interface
``` csharp
    /// <summary>
    /// Interface symmetry API interface definition
    /// </summary>
    public interface ISymmetryService
    {
        /// <summary>
        /// Asynchronous API definition
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> AddAsync(int left, int right);
        /// <summary>
        /// Synchronous API definition (It is not recommended to define the synchronous API in interface symmetric services, as the client synchronous blocking mode may cause performance bottlenecks)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
```
## Implement the service interface
``` csharp
    /// <summary>
    /// Interface symmetry API implementation
    /// </summary>
    internal sealed class SymmetryService : ISymmetryService
    {
        /// <summary>
        /// Asynchronous API implementation
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> ISymmetryService.AddAsync(int left, int right) { return System.Threading.Tasks.Task.FromResult(left + right); }
        /// <summary>
        /// Synchronous API implementation
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Add(int left, int right) { return left + right; }
    }
```
## Create the server-side listener
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
## Create the RPC client
``` csharp
        /// <summary>
        /// Test client singleton (Full duplex connection only requires creating one client)
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
1. [Interface symmetry API](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/01.SymmetryService/01.SymmetryService.Eng.md)
2. [Data serialization](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/02.ServiceDataSerialize/02.ServiceDataSerialize.Eng.md)
3. [Thread scheduling strategy](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/03.ServiceThreadStrategy/03.ServiceThreadStrategy.Eng.md)
4. [Authentication and transmission data encoding](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/04.ServiceAuthentication/04.ServiceAuthentication.Eng.md)
5. [Static code generation](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/05.CodeGenerator/05.CodeGenerator.Eng.md)
6. [Service registration and push](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/10.ServerRegistry/10.ServerRegistry.Eng.md)
7. [Reverse RPC service](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/11.ReverseServer/11.ReverseServer.Eng.md)
8. [The client supports .NET NativeAOT](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.Eng.md)

# In-memory database
## Database service configuration definition
``` csharp
    /// <summary>
    /// In-memory database service configuration
    /// </summary>
    internal sealed class ServiceConfig : AutoCSer.CommandService.StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// The test environment deletes historical persistent files from the previous 15 minutes. The production environment processes the files based on site requirements
        /// </summary>
        /// <returns></returns>
        public override DateTime GetRemoveHistoryFileTime()
        {
            return AutoCSer.Threading.SecondTimer.UtcNow.AddMinutes(-15);
        }
        /// <summary>
        /// The test environment deletes persistent files once a minute. The production environment deletes persistent files based on site requirements
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override void RemoveHistoryFile(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).Catch();
        }
        /// <summary>
        /// Set the rebuild file size to at least 10MB
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
## Create the database service instance
``` csharp
            AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig databaseServiceConfig = new AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseNode)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseNode) + nameof(AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();
```
## Create the RPC service
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
## Define the RPC client instance
``` csharp
    /// <summary>
    /// RPC client instance
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// In-memory database client interface instance
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
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
        /// RPC client instance
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }
    }
```
## Create the RPC client
``` csharp
        /// <summary>
        /// In-memory database client singleton
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
```
1. [Introduction to in-memory database](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/06.MemoryDatabase/06.MemoryDatabase.Eng.md)
2. [Built-in data structure node](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/07.MemoryDatabaseNode/07.MemoryDatabaseNode.Eng.md)
3. [Custom node](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/08.MemoryDatabaseCustomNode/08.MemoryDatabaseCustomNode.Eng.md)
4. [Local embedding mode](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/09.MemoryDatabaseLocalService/09.MemoryDatabaseLocalService.Eng.md)
5. [Static code generation](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/05.CodeGenerator/05.CodeGenerator.Eng.md)
6. [Service registration and push](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/10.ServerRegistry/10.ServerRegistry.Eng.md)
7. [The local embedding mode supports .NET NativeAOT](https://github.com/AutoCSer/AutoCSer2/blob/main/Document/12.NativeAOT/12.NativeAOT.Eng.md)