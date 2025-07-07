using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Distributed lock node client interface
    /// 分布式锁节点客户端接口
    /// </summary>
    public partial interface IDistributedLockNodeClientNode<T> where T : IEquatable<T>
    {
    }
}
