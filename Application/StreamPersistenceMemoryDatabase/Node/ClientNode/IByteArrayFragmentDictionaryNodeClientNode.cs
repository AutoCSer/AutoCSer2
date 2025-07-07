using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 base fragment dictionary node client interface 
    /// 256 基分片字典节点 客户端接口
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public partial interface IByteArrayFragmentDictionaryNodeClientNode<KT> where KT : IEquatable<KT>
    {
    }
}
