using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Message processing node local client interface
    /// 消息处理节点本地客户端接口
    /// </summary>
    public partial interface IMessageNodeLocalClientNode<T> where T : Message<T>
    {
    }
}
