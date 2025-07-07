using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sort dictionary node client interface
    /// 排序字典节点 客户端接口
    /// </summary>
    public partial interface ISortedDictionaryNodeClientNode<KT, VT> where KT : IComparable<KT>
    {
    }
}
