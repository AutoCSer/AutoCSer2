using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志回调
    /// </summary>
    internal sealed class ReverseLogCollectionCallback<T> : CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 反向日志收集服务
        /// </summary>
        private readonly ReverseLogCollection<T> controller;
        /// <summary>
        /// 测试日志数据定义
        /// </summary>
        private readonly T log;
        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="log"></param>
        internal ReverseLogCollectionCallback(ReverseLogCollection<T> controller, T log)
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
