using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务重建持久化文件
    /// </summary>
    internal sealed class LocalServiceRebuild : LocalServiceQueueNode<RebuildResult>
    {
        /// <summary>
        /// 本地服务重建持久化文件
        /// </summary>
        /// <param name="service">日志流持久化内存数据库本地服务</param>
        internal LocalServiceRebuild(LocalService service) : base(service) { }
        /// <summary>
        /// 重建持久化文件
        /// </summary>
        public override void RunTask()
        {
            result = service.Rebuild();
            completed();
        }
    }
}
