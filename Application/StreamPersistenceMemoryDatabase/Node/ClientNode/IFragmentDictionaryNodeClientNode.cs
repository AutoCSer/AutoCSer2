using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片字典 节点接口 客户端节点接口
    /// </summary>
    public partial interface IFragmentDictionaryNodeClientNode<KT, VT> where KT : IEquatable<KT>
    {
    }
}
