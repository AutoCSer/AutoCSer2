using System;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// Command service port
    /// 命令服务端口
    /// </summary>
    public enum CommandServerPortEnum : ushort
    {
        /// <summary>
        /// Document test port
        /// 文档测试端口
        /// </summary>
        Document = 12900,
        /// <summary>
        /// Daemon test port
        /// 守护进程测试端口
        /// </summary>
        ProcessGuard,
        /// <summary>
        /// Publish the service test port
        /// 发布服务测试端口
        /// </summary>
        DeployTask,

        /// <summary>
        /// Server registration test port
        /// 服务注册测试端口
        /// </summary>
        ServiceRegistry,
        /// <summary>
        /// Service port Register test port
        /// 服务端口注册测试端口
        /// </summary>
        PortRegistry,
        /// <summary>
        /// Test case port
        /// 测试用例端口
        /// </summary>
        TestCase,
        /// <summary>
        /// 反向服务测试用例端口
        /// </summary>
        ReverseServer,
        /// <summary>
        /// Command service performance test port
        /// 命令服务性能测试端口
        /// </summary>
        Performance,
        /// <summary>
        /// Service authentication test port based on incremental login timestamp authentication
        /// 基于递增登录时间戳验证的服务认证测试端口
        /// </summary>
        TimestampVerify,
        /// <summary>
        /// Database backup test port
        /// 数据库备份测试端口
        /// </summary>
        DatabaseBackup,
        /// <summary>
        /// Reverse log collection test port
        /// 反向日志收集测试端口
        /// </summary>
        ReverseLogCollection,
        /// <summary>
        /// ORM business data service test port
        /// ORM 业务数据服务测试端口
        /// </summary>
        ORM,
        /// <summary>
        /// Log stream persistence memory database test port
        /// 日志流持久化内存数据库测试端口
        /// </summary>
        StreamPersistenceMemoryDatabase,
        /// <summary>
        /// Disk block service test port
        /// 磁盘块服务测试端口
        /// </summary>
        DiskBlock,
        /// <summary>
        /// File synchronization test port
        /// 文件同步测试端口
        /// </summary>
        FileSynchronous,
        /// <summary>
        /// Interface call monitor test port in real time
        /// 接口实时调用监视测试端口
        /// </summary>
        InterfaceRealTimeCallMonitor,
        /// <summary>
        /// Abnormal call statistics test port
        /// 异常调用统计信息测试端口
        /// </summary>
        ExceptionStatistics,
        /// <summary>
        /// Search Trie graph word segmentation test port
        /// 搜索 Trie 图分词测试端口
        /// </summary>
        SearchTrieGraph,
        /// <summary>
        /// Search Word segmentation Result Disk block index information Test port
        /// 搜索分词结果磁盘块索引信息测试端口
        /// </summary>
        SearchWordIdentityBlockIndex,
        /// <summary>
        /// Mock search data source service test port
        /// 模拟搜索数据源服务测试端口
        /// </summary>
        SearchDataSource,
        /// <summary>
        /// Search segmentation results index test port
        /// 搜索分词结果索引测试端口
        /// </summary>
        SearchDiskBlockIndex,
        /// <summary>
        /// Search aggregate query service test port
        /// 搜索聚合查询服务测试端口
        /// </summary>
        SearchQueryService,
        /// <summary>
        /// Short link service test port
        /// 短连接服务测试端口
        /// </summary>
        ShortLink,

        /// <summary>
        /// WEB view test port
        /// WEB 视图测试端口
        /// </summary>
        WebViewHttp = 12999,
        /// <summary>
        /// Register the service test port
        /// 注册服务测试端口
        /// </summary>
        ServiceRegistryPort = 20000,
    }
}
