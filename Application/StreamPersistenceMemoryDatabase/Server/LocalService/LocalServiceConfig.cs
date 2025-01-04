using System;
using System.Runtime.CompilerServices;
namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库本地服务配置
    /// </summary>
    public class LocalServiceConfig : StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// 默认为 true 表示仅支持本地服务，否则为混合服务模式
        /// </summary>
        public bool OnlyLocalService = true;
#if DEBUG
        /// <summary>
        /// 同步队列任务执行超时检查秒数默认为 5 ，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 5;
#else
        /// <summary>
        /// 同步队列任务执行超时检查秒数，默认为 0 表示不检查，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 0;
#endif

        /// <summary>
        /// 创建默认日志流持久化内存数据库服务端（主服务节点）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public new LocalService Create()
        {
            return Create<IServiceNode>(service => new ServiceNode(service));
        }
        /// <summary>
        /// 日志流持久化内存数据库服务端（主服务节点）
        /// </summary>
        /// <typeparam name="T">节点服务接口类型</typeparam>
        /// <param name="createServiceNode"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalService Create<T>(Func<LocalService, T> createServiceNode)
            where T : class, IServiceNode
        {
            return new LocalService(this, service => ServiceNode.CreateServiceNode(service, createServiceNode(service)));
        }
    }
}
