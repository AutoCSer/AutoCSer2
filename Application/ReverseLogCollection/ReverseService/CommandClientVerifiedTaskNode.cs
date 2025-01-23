using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.ReverseLogCollection.ReverseService
{
    /// <summary>
    /// 客户端验证完成处理
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    internal sealed class CommandClientVerifiedTaskNode<T> : CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        private readonly CommandReverseListener<T> listener;
        /// <summary>
        /// 客户端验证完成处理
        /// </summary>
        /// <param name="listener">反向命令服务客户端监听</param>
        internal CommandClientVerifiedTaskNode(CommandReverseListener<T> listener)
        {
            this.listener = listener;
        }
        /// <summary>
        /// 客户端验证完成处理
        /// </summary>
        public override void RunTask()
        {
            listener.OnVerified();
        }
    }
}
