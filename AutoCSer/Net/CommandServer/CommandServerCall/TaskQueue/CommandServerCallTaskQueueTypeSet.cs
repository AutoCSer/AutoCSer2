using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 异步任务队列集合管理
    /// </summary>
    public class CommandServerCallTaskQueueTypeSet : SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 命令服务
        /// </summary>
        protected readonly CommandListener server;
        /// <summary>
        /// 队列集合
        /// </summary>
        private readonly Dictionary<HashObject<System.Type>, CommandServerCallTaskQueueSet> queues = DictionaryCreator.CreateHashObject<System.Type, CommandServerCallTaskQueueSet>();
        /// <summary>
        /// 异步任务队列集合管理
        /// </summary>
        /// <param name="server"></param>
        public CommandServerCallTaskQueueTypeSet(CommandListener server)
            : base(SecondTimer.TaskArray, server.Config.QueueTimeoutSeconds, SecondTimerTaskThreadModeEnum.WaitTask, SecondTimerKeepModeEnum.After, server.Config.QueueTimeoutSeconds)
        {
            this.server = server;
            if (KeepSeconds > 0) AppendTaskArray();
        }
        /// <summary>
        /// 获取服务端异步调用队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        public CommandServerCallTaskQueueSet<T>? TryGet<T>() where T : IEquatable<T>
#else
        public CommandServerCallTaskQueueSet<T> TryGet<T>() where T : IEquatable<T>
#endif
        {
            HashObject<System.Type> type = typeof(T);
            var queue = default(CommandServerCallTaskQueueSet);
            Monitor.Enter(queues);
            try
            {
                queues.TryGetValue(type, out queue);
            }
            finally { Monitor.Exit(queues); }
#if NetStandard21
            return (CommandServerCallTaskQueueSet<T>?)queue;
#else
            return (CommandServerCallTaskQueueSet<T>)queue;
#endif
        }
        /// <summary>
        /// 获取服务端异步调用队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        internal CommandServerCallTaskQueueSet<T>? Get<T>() where T : IEquatable<T>
#else
        internal CommandServerCallTaskQueueSet<T> Get<T>() where T : IEquatable<T>
#endif
        {
            HashObject<System.Type> type = typeof(T);
            var queue = default(CommandServerCallTaskQueueSet);
            Monitor.Enter(queues);
            try
            {
                if (!queues.TryGetValue(type, out queue) && !server.IsDisposed)
                {
                    queue = createQueue<T>();
                    queues.Add(type, queue);
                }
            }
            finally { Monitor.Exit(queues); }
#if NetStandard21
            return (CommandServerCallTaskQueueSet<T>?)queue;
#else
            return (CommandServerCallTaskQueueSet<T>)queue;
#endif
        }
        /// <summary>
        /// 创建服务端执行队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual CommandServerCallTaskQueueSet<T> createQueue<T>() where T : IEquatable<T>
        {
            return new CommandServerCallTaskQueueSet<T>(server, KeepSeconds > 0);
        }
        /// <summary>
        /// 关闭服务端异步调用队列
        /// </summary>
        internal void Close()
        {
            Monitor.Enter(queues);
            try
            {
                foreach (CommandServerCallTaskQueueSet queue in queues.Values) queue.Close();
                queues.Clear();
            }
            finally
            {
                Monitor.Exit(queues);
                onClosed();
            }
        }
        /// <summary>
        /// 关闭队列处理
        /// </summary>
        protected virtual void onClosed() { }
        /// <summary>
        /// 队列任务执行超时检查
        /// </summary>
        /// <returns></returns>
        protected internal override async Task OnTimerAsync()
        {
            int keepSeconds = KeepSeconds;
            foreach (CommandServerCallTaskQueueSet queue in queues.Values) await queue.CheckTaskTimeoutAsync(keepSeconds);
        }
    }
}
