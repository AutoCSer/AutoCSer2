using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sorting list node client interface
    /// 排序列表节点 客户端接口
    /// </summary>
    public partial interface ISortedListNodeClientNode<KT, VT> where KT : IComparable<KT>
    {
    }
}
