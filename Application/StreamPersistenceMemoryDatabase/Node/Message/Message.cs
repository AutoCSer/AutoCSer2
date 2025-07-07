using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Message data
    /// 消息数据
    /// </summary>
    /// <typeparam name="T">Message data type
    /// 消息数据类型</typeparam>
    public abstract class Message<T> : KeepCallbackReturnValueLink<T>
        where T : Message<T>
    {
        /// <summary>
        /// Message unique number (Unique number within the node)
        /// 消息唯一编号（节点内唯一编号）
        /// </summary>
        public MessageIdeneity MessageIdeneity;
    }
}
