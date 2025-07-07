using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sort the collection node local client interface
    /// 排序集合节点 本地客户端接口
    /// </summary>
    public partial interface ISortedSetNodeLocalClientNode<T> where T : IComparable<T>
    {
    }
}
