using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.ReverseLogCollection
{
    /// <summary>
    /// 日志收集反向命令服务客户端监听
    /// </summary>
    public abstract class CommandReverseListener : AutoCSer.Net.CommandReverseListener
    {
        /// <summary>
        /// 发送日志任务队列
        /// </summary>
        protected readonly TaskQueue queue;
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        /// <param name="config">反向命令服务客户端监听配置</param>
        internal CommandReverseListener(CommandReverseListenerConfig config) : base(config)
        {
            queue = new TaskQueue();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        protected override void dispose()
        {
            base.dispose();
            queue.Dispose();
        }
    }
    /// <summary>
    /// 日志收集反向命令服务客户端监听
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public class CommandReverseListener<T> : CommandReverseListener
    {
        /// <summary>
        /// 待发送日志集合
        /// </summary>
        private RingQueue<T> logs;
        /// <summary>
        /// 日志收集反向服务客户端套接字事件
        /// </summary>
        private ILogCollectionReverseClientSocketEvent<T> client;
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        /// <param name="config">反向命令服务客户端监听配置</param>
        public CommandReverseListener(CommandReverseListenerConfig config) : base(config)
        {
            logs = new RingQueue<T>(config.LogQueueCapacity);
            client = (ILogCollectionReverseClientSocketEvent<T>)CommandClient.SocketEvent;
        }
        /// <summary>
        /// 客户端验证完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onVerified()
        {
            queue.Add(new ReverseService.CommandClientVerifiedTaskNode<T>(this));
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 客户端验证完成处理
        /// </summary>
        internal void OnVerified()
        {
            var log = default(T);
            while (logs.TryGetRead(out log) && send(log)) logs.MoveRead();
        }
        /// <summary>
        /// 发送日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private bool send(T log)
        {
            if (client.LogCollectionReverseClient == null) return false;
            AutoCSer.Net.SendOnlyCommand command = client.LogCollectionReverseClient.AppendSendOnly(log);
            return command != null && command.GetResult();
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendLog(T log)
        {
            if (!send(log))
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
            await Config.Log.Debug(AutoCSer.JsonSerializer.Serialize(log));
        }
    }
}
