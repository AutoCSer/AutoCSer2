using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片字典 节点接口 客户端节点接口
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public partial interface IByteArrayFragmentDictionaryNodeClientNode<KT> where KT : IEquatable<KT>
    {
    }
}
