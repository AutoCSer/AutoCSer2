using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256-base fragment hash table node local client interface
    /// 256 基分片哈希表 节点 本地客户端接口
    /// </summary>
    public partial interface IFragmentHashSetNodeLocalClientNode<T> where T : IEquatable<T>
    {
    }
}
