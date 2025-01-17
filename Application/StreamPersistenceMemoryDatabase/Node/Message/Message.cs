using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息数据
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    public abstract class Message<T> : KeepCallbackReturnValueLink<T>
        where T : Message<T>
    {
        /// <summary>
        /// 消息唯一编号（节点内唯一编号）
        /// </summary>
        public MessageIdeneity MessageIdeneity;
    }
}
