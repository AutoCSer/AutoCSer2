using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序字典节点 客户端节点接口
    /// </summary>
    public partial interface ISortedDictionaryNodeClientNode<KT, VT> where KT : IComparable<KT>
    {
    }
}
