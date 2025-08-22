using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence memory database client cache for client singleton
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="T">Basic service operation client interface type
    /// 服务基础操作客户端接口类型</typeparam>
    public abstract class ClientTaskCache<T> where T : class, IServiceNodeClientNode
    {
        /// <summary>
        /// Has the client released the resources
        /// 客户端是否已经释放资源
        /// </summary>
        public abstract bool IsDisposed { get; }
        /// <summary>
        /// Log stream persistence in-memory database client
        /// 日志流持久化内存数据库客户端
        /// </summary>
#if NetStandard21
        protected Task<StreamPersistenceMemoryDatabaseClient<T>?>? clientTask;
#else
        protected Task<StreamPersistenceMemoryDatabaseClient<T>> clientTask;
#endif
        /// <summary>
        /// Get the log stream persistent memory database client
        /// 获取日志流持久化内存数据库客户端
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<StreamPersistenceMemoryDatabaseClient<T>?> GetClient()
#else
        public Task<StreamPersistenceMemoryDatabaseClient<T>> GetClient()
#endif
        {
            return clientTask ?? getClient();
        }
        /// <summary>
        /// Get the log stream persistent memory database client
        /// 获取日志流持久化内存数据库客户端
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        protected abstract Task<StreamPersistenceMemoryDatabaseClient<T>?> getClient();
#else
        protected abstract Task<StreamPersistenceMemoryDatabaseClient<T>> getClient();
#endif
        /// <summary>
        /// Get the log stream persistent in-memory database client node
        /// 获取日志流持久化内存数据库客户端节点
        /// </summary>
        /// <typeparam name="NT">Client node type
        /// 客户端节点类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseClientNodeCache<NT, T> CreateNode<NT>(Func<StreamPersistenceMemoryDatabaseClient<T>, Task<ResponseResult<NT>>> getNodeTask)
            where NT : class
        {
            return new StreamPersistenceMemoryDatabaseClientNodeCache<NT, T>(this, getNodeTask);
        }
    }
}
