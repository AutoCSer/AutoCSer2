using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sort the collection node client interface
    /// 排序集合节点 客户端接口
    /// </summary>
    public partial interface ISortedSetNodeClientNode<T> where T : IComparable<T>
    {
    }
}
