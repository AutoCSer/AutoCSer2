using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息处理节点 客户端节点接口
    /// </summary>
    public partial interface IMessageNodeClientNode<T> where T : Message<T>
    {
    }
}
