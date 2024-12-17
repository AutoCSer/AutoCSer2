using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁节点 客户端节点接口
    /// </summary>
    public partial interface IDistributedLockNodeClientNode<T> where T : IEquatable<T>
    {
    }
}
