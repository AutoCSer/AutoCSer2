using System;

namespace AutoCSer.CommandService.ReverseLogCollection.ReverseService
{
    /// <summary>
    /// 添加日志
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    internal sealed class AppendLogTaskNode<T> : AutoCSer.Net.CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        private readonly CommandReverseListener<T> listener;
        /// <summary>
        /// 日志记录
        /// </summary>
        private readonly T log;
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="listener">反向命令服务客户端监听</param>
        /// <param name="log">日志记录</param>
        internal AppendLogTaskNode(CommandReverseListener<T> listener, T log)
        {
            this.listener = listener;
            this.log = log;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        public override void RunTask()
        {
            listener.AppendLog(log);
        }
    }
}
