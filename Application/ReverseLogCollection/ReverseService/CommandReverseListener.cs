using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.ReverseLogCollection
{
    /// <summary>
    /// 日志收集反向命令服务客户端监听
    /// </summary>
    public abstract class CommandReverseListener : AutoCSer.Net.CommandReverseListener
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        protected readonly MD5 md5;
        /// <summary>
        /// 发送日志任务队列
        /// </summary>
        protected readonly TaskQueue queue;
        /// <summary>
        /// 递增登录时间戳检查器
        /// </summary>
        protected TimestampVerifyChecker timestampVerifyChecker;
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        /// <param name="config">反向命令服务客户端监听配置</param>
        internal CommandReverseListener(CommandReverseListenerConfig config) : base(config)
        {
            md5 = MD5.Create();
            timestampVerifyChecker = new TimestampVerifyChecker(0);
            queue = new TaskQueue();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void dispose()
        {
            base.dispose();
            queue.Dispose();
            md5.Dispose();
        }
    }
    /// <summary>
    /// 日志收集反向命令服务客户端监听
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public class CommandReverseListener<T> : CommandReverseListener
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        internal LeftArray<ILogCollectionReverseClientSocketEvent<T>> clients;
        /// <summary>
        /// 待发送日志集合
        /// </summary>
        private RingQueue<T> logs;
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        /// <param name="config">反向命令服务客户端监听配置</param>
        protected CommandReverseListener(CommandReverseListenerConfig config) : base(config)
        {
            logs = new RingQueue<T>(config.LogQueueCapacity);
            clients = new LeftArray<ILogCollectionReverseClientSocketEvent<T>>(0);
        }
        /// <summary>
        /// 设置当前客户端
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected override ValueTask setCommandClient(CommandClient client)
        {
            queue.Add(new ReverseService.AppendCommandClientTaskNode<T>(this, client));
            return AutoCSer.Common.CompletedValueTask;
        }
        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="client"></param>
        internal void AppendCommandClient(CommandClient client)
        {
            bool isClient = false;
            try
            {
                clients.Add((ILogCollectionReverseClientSocketEvent<T>)client.SocketEvent);
                isClient = true;
            }
            finally
            {
                if (!isClient) client.Dispose();
            }
            var log = default(T);
            while (logs.TryGetRead(out log) && send(log) != 0) logs.MoveRead();
        }
        /// <summary>
        /// 发送日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private int send(T log)
        {
            int count = 0;
            foreach (KeyValue<ILogCollectionReverseClientSocketEvent<T>, int> client in clients.GetReverseIndexEnumerable())
            {
                AutoCSer.Net.SendOnlyCommand command = client.Key.LogCollectionReverseClient.AppendSendOnly(log);
                if (command != null && command.GetResult()) ++count;
                else clients.RemoveAtToEnd(client.Value);
            }
            return count;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Append(T log)
        {
            queue.Add(new ReverseService.AppendLogTaskNode<T>(this, log));
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        internal void AppendLog(T log)
        {
            if (send(log) == 0) logs.Write(log);
        }
    }
}
