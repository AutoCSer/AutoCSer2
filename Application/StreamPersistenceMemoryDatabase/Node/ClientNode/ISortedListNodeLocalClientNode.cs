using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序列表节点 客户端节点接口
    /// </summary>
    public partial interface ISortedListNodeLocalClientNode<KT, VT> where KT : IComparable<KT>
    {
    }
}
