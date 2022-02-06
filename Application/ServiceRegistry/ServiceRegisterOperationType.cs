using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册日志操作类型
    /// </summary>
    public enum ServiceRegisterOperationType : byte
    {
        /// <summary>
        /// 注册集群服务主节点
        /// </summary>
        ClusterMain,
        /// <summary>
        /// 注册集群服务普通节点
        /// </summary>
        ClusterNode,

        /// <summary>
        /// 注册单例服务并通知当前服务下线，用于避免多队列类型服务并发写冲突的问题
        /// </summary>
        Singleton,

        /// <summary>
        /// 注销服务
        /// </summary>
        Logout,
        /// <summary>
        /// 强制注销所有注册服务
        /// </summary>
        Clear,

        /// <summary>
        /// 通知单例服务下线
        /// </summary>
        Offline,
        /// <summary>
        /// 失联服务
        /// </summary>
        LostContact,
    }
}
