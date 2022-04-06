using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 反向日志收集服务
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public class ReverseLogCollection<T> : CommandServerBindController, IReverseLogCollection<T>
    {
        /// <summary>
        /// 获取日志回调委托集合
        /// </summary>
        private LeftArray<CommandServerKeepCallback<T>> callbacks = new LeftArray<CommandServerKeepCallback<T>>(0);
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">获取日志回调委托</param>
        public virtual void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<T> callback)
        {
            callbacks.Add(callback);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        public virtual void Appped(T log)
        {
            Controller.AddQueue(new ReverseLogCollectionCallback<T>(this, log));
        }
        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="log"></param>
        internal virtual void Callback(T log)
        {
            callbacks.RemoveAllToEnd(p => !p.Callback(log));
        }
    }
}
