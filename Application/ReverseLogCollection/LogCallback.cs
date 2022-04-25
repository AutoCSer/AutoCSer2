using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.ReverseLogCollection
{
    /// <summary>
    /// 日志回调
    /// </summary>
    internal sealed class LogCallback<T> : CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 反向日志收集服务
        /// </summary>
        private readonly ReverseLogCollectionService<T> controller;
        /// <summary>
        /// 测试日志数据定义
        /// </summary>
        private readonly T log;
        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="log"></param>
        internal LogCallback(ReverseLogCollectionService<T> controller, T log)
        {
            this.controller = controller;
            this.log = log;
        }
        /// <summary>
        /// 日志委托回调
        /// </summary>
        public override void RunTask()
        {
            controller.Callback(log);
        }
    }
}
