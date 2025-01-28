using AutoCSer.CommandService.ReverseLogCollection;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 反向日志收集服务
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    [CommandServerController(InterfaceType = typeof(IReverseLogCollectionService<>))]
    public class ReverseLogCollectionService<T> : CommandServerBindController, IReverseLogCollectionService<T>
    {
        /// <summary>
        /// 获取日志回调委托集合
        /// </summary>
        private CommandServerKeepCallback<T>.Link callbacks;
        /// <summary>
        /// 待发送日志集合
        /// </summary>
        private RingQueue<T> logs;
        /// <summary>
        /// 反向日志收集服务
        /// </summary>
        /// <param name="logQueueCapacity">待发送日志队列数量默认为 1023 条，超出限制则抛弃日志</param>
        public ReverseLogCollectionService(int logQueueCapacity = (1 << 10) - 1)
        {
            logs = new RingQueue<T>(logQueueCapacity);
        }
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">获取日志回调委托</param>
        public virtual void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<T> callback)
        {
            callbacks.PushHead(callback);
            var log = default(T);
            while (logs.TryGetRead(out log) && callbacks.Callback(log) != 0) logs.MoveRead();
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        public virtual void Appped(T log)
        {
            Controller.CallQueue.notNull().AddOnly(new LogCallback<T>(this, log));
        }
        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="log"></param>
        internal virtual void Callback(T log)
        {
            if (callbacks.Callback(log) == 0)
            {
                var removeLog = logs.Write(log);
                if (removeLog != null) onRemove(log).NotWait();
            }
        }
        /// <summary>
        /// 队列溢出移除的未处理日志
        /// </summary>
        /// <param name="log"></param>
        protected virtual async Task onRemove(T log)
        {
            await Controller.Server.Log.Debug(AutoCSer.JsonSerializer.Serialize(log));
        }
    }
}
