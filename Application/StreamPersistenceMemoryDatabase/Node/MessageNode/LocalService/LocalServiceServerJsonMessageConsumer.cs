﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端 JSON 字符串 / 客户端对象 消息消费者
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public abstract class LocalServiceServerJsonMessageConsumer<T> : LocalServiceMessageConsumer<ServerJsonMessage<T>>
    {
        /// <summary>
        /// 服务端 JSON 字符串 / 客户端对象 消息消费者
        /// </summary>
        /// <param name="client">日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">服务端 JSON 字符串 / 客户端对象 消息客户端节点</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceServerJsonMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<ServerJsonMessage<T>> node, int delayMilliseconds = MessageConsumer.DefaultDelayMilliseconds) : base(client, node, delayMilliseconds) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override async Task onMessage(ServerJsonMessage<T> message)
        {
            await onMessage((T)message);
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task onMessage(T message);
    }
}