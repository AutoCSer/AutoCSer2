using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Message processing node client interface
    /// 消息处理节点客户端接口
    /// </summary>
    public partial interface IMessageNodeClientNode<T> where T : Message<T>
    {
    }
}
