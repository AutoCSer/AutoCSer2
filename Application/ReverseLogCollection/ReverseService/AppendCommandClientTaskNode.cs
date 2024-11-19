using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.ReverseLogCollection.ReverseService
{
    /// <summary>
    /// 添加客户端
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    internal sealed class AppendCommandClientTaskNode<T> : CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        private readonly CommandReverseListener<T> listener;
        /// <summary>
        /// 客户端
        /// </summary>
        private readonly CommandClient client;
        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="listener">反向命令服务客户端监听</param>
        /// <param name="client">客户端</param>
        internal AppendCommandClientTaskNode(CommandReverseListener<T> listener, CommandClient client)
        {
            this.listener = listener;
            this.client = client;
        }
        /// <summary>
        /// 添加客户端
        /// </summary>
        public override void RunTask()
        {
            listener.AppendCommandClient(client);
        }
    }
}
