using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务获取节点标识
    /// </summary>
    internal sealed class LocalServiceRebuild : LocalServiceQueueNode<RebuildResult>
    {
        /// <summary>
        /// 本地服务获取节点标识
        /// </summary>
        /// <param name="service">日志流持久化内存数据库本地服务</param>
        internal LocalServiceRebuild(LocalService service) : base(service) { }
        /// <summary>
        /// 获取节点标识
        /// </summary>
        public override void RunTask()
        {
            result = service.Rebuild();
            completed();
        }
    }
}
