using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256-base fragment dictionary node client interface
    /// 256 基分片字典 节点 客户端接口
    /// </summary>
    public partial interface IFragmentDictionaryNodeClientNode<KT, VT> where KT : IEquatable<KT>
    {
    }
}
