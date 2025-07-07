using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Distributed lock node local client interface
    /// 分布式锁节点本地客户端接口
    /// </summary>
    public partial interface IDistributedLockNodeLocalClientNode<T> where T : IEquatable<T>
    {
    }
}
