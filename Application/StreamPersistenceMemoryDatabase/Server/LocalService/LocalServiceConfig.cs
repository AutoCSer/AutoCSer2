using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence memory database local service configuration
    /// 日志流持久化内存数据库本地服务配置
    /// </summary>
    public class LocalServiceConfig : StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// The default is true, indicating that only local services are supported; otherwise, it is in a mixed service mode
        /// 默认为 true 表示仅支持本地服务，否则为混合服务模式
        /// </summary>
        public bool OnlyLocalService = true;
#if DEBUG
        /// <summary>
        /// The default time for checking the execution timeout of the synchronous queue task is 5, which is used to check whether there are long-term blocking or deadlock issues with the queue task
        /// 同步队列任务执行超时检查秒数默认为 5 ，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 5;
#else
        /// <summary>
        /// The synchronization queue task execution timeout checks the number of seconds. By default, 0 indicates no check, which is used to check whether the queue task has been blocked for a long time or has deadlock issues
        /// 同步队列任务执行超时检查秒数，默认为 0 表示不检查，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 0;
#endif

        /// <summary>
        /// Create a default log stream persistence in-memory database local service (Master service node)
        /// 创建默认日志流持久化内存数据库本地服务（主服务节点）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public new LocalService Create()
        {
            return Create<IServiceNode>(service => new ServiceNode(service));
        }
        /// <summary>
        /// Create a log stream persistence in-memory database local service (Master service node)
        /// 创建日志流持久化内存数据库本地服务（主服务节点）
        /// </summary>
        /// <typeparam name="T">Node service interface type
        /// 节点服务接口类型</typeparam>
        /// <param name="createServiceNode"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalService Create<T>(Func<LocalService, T> createServiceNode)
            where T : class, IServiceNode
        {
            return new LocalService(this, service => ServiceNode.CreateServiceNode(service, createServiceNode(service)));
        }
        /// <summary>
        /// Create a default log stream persistence in-memory database local service (supporting concurrent read operations)
        /// 创建默认日志流持久化内存数据库本地服务（支持并发读取操作）
        /// </summary>
        /// <param name="maxConcurrency">The maximum concurrent number of read operations, if less than or equal to 0, indicates the number of processors minus the set value (for example, if the number of processors is 4 and the concurrent number is set to -1, then the concurrent number of reads is 4 -1 = 3)
        /// 最大读取操作并发数量，小于等于 0 表示处理器数量减去设置值（比如处理器数量为 4，并发数量设置为 -1，则读取并发数量为 4 - 1 = 3）</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalService Create(int maxConcurrency)
        {
            return Create<IServiceNode>(service => new ServiceNode(service), maxConcurrency);
        }
        /// <summary>
        /// Create a log stream persistence in-memory database service (supporting concurrent read operations)
        /// 创建日志流持久化内存数据库服务（支持并发读取操作）
        /// </summary>
        /// <typeparam name="T">Node service interface type
        /// 节点服务接口类型</typeparam>
        /// <param name="createServiceNode"></param>
        /// <param name="maxConcurrency">The maximum concurrent number of read operations, if less than or equal to 0, indicates the number of processors minus the set value (for example, if the number of processors is 4 and the concurrent number is set to -1, then the concurrent number of reads is 4 -1 = 3)
        /// 最大读取操作并发数量，小于等于 0 表示处理器数量减去设置值（比如处理器数量为 4，并发数量设置为 -1，则读取并发数量为 4 - 1 = 3）</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalService Create<T>(Func<LocalService, T> createServiceNode, int maxConcurrency)
            where T : class, IServiceNode
        {
            return new LocalService(this, service => ServiceNode.CreateServiceNode(service, createServiceNode(service)), maxConcurrency);
        }
    }
}
