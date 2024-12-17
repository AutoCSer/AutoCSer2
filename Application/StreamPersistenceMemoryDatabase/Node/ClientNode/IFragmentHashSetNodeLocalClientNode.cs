using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 哈希表 节点接口 客户端节点接口
    /// </summary>
    public partial interface IFragmentHashSetNodeLocalClientNode<T> where T : IEquatable<T>
    {
    }
}
