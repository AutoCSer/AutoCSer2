using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sort dictionary node local client interface
    /// 排序字典节点 本地客户端接口
    /// </summary>
    public partial interface ISortedDictionaryNodeLocalClientNode<KT, VT> where KT : IComparable<KT>
    {
    }
}
