using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256-base fragment hash table node client interface
    /// 256 基分片哈希表 节点 客户端接口
    /// </summary>
    public partial interface IFragmentHashSetNodeClientNode<T> where T : IEquatable<T>
    {
    }
}
