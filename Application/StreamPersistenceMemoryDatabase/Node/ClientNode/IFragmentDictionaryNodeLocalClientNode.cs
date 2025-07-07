using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256-base fragment dictionary node local client interface
    /// 256 基分片字典 节点 本地客户端接口
    /// </summary>
    public partial interface IFragmentDictionaryNodeLocalClientNode<KT, VT> where KT : IEquatable<KT>
    {
    }
}
