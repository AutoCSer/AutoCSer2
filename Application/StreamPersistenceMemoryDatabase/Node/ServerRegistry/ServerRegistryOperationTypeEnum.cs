using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server registration log operation type
    /// 服务注册日志操作类型
    /// </summary>
    public enum ServerRegistryOperationTypeEnum : byte
    {
        /// <summary>
        /// Register the master node of the cluster service
        /// 注册集群服务主节点
        /// </summary>
        ClusterMain,
        /// <summary>
        /// Register the ordinary node of the cluster service
        /// 注册集群服务普通节点
        /// </summary>
        ClusterNode,

        /// <summary>
        /// Register the singleton service and notify the current service to go offline to avoid concurrent write conflicts among multi-queue type services
        /// 注册单例服务并通知当前服务下线，用于避免多队列类型服务并发写冲突的问题
        /// </summary>
        Singleton,

        /// <summary>
        /// Log out of service
        /// 注销服务
        /// </summary>
        Logout,

        /// <summary>
        /// Online server inspection
        /// 服务端在线检查
        /// </summary>
        CheckOnline,
        /// <summary>
        /// Notify the singleton server to go offline
        /// 通知单例服务端下线
        /// </summary>
        Offline,
        /// <summary>
        /// Missing contact service
        /// 失联服务
        /// </summary>
        LostContact,
        /// <summary>
        /// The initialization log callback is completed
        /// 初始化日志回调完成
        /// </summary>
        CallbackLoaded,
        /// <summary>
        /// Initialize to load the currently assigned service session identity
        /// 初始化加载当前已分配服务会话标识
        /// </summary>
        LoadCurrentSessionID,
    }
}
