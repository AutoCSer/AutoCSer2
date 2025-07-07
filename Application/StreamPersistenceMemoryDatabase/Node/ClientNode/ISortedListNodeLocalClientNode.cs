using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sorting list node local client interface
    /// 排序列表节点 本地客户端接口
    /// </summary>
    public partial interface ISortedListNodeLocalClientNode<KT, VT> where KT : IComparable<KT>
    {
    }
}
