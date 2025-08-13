using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 二阶段回调的第一阶段回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CommandServerTwoStage‌Callback<T> : CommandServerCallback<T>
    {
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        private readonly ServerInterfaceMethod method;
        /// <summary>
        /// 二阶段回调的第一阶段回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerTwoStage‌Callback(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, OfflineCount.Null)
        {
            this.method = method;
        }
        /// <summary>
        /// 二阶段回调的第一阶段回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        internal CommandServerTwoStage‌Callback(CommandServerCallQueueNode node, ServerInterfaceMethod method) : base(node, OfflineCount.Null)
        {
            this.method = method;
        }
        /// <summary>
        /// 二阶段回调的第一阶段回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        internal CommandServerTwoStage‌Callback(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method) : base(node, OfflineCount.Null)
        {
            this.method = method;
        }
        /// <summary>
        /// 二阶段回调的第一阶段回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        internal CommandServerTwoStage‌Callback(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method) : base(node, OfflineCount.Null)
        {
            this.method = method;
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public override bool Callback(T returnValue)
        {
            return CommandServerCallback<T>.TwoStage‌Callback(this, method, returnValue);
        }
        ///// <summary>
        ///// Queue synchronization callback
        ///// 队列同步回调
        ///// </summary>
        ///// <param name="returnValue"></param>
        ///// <returns></returns>
        //internal override bool SynchronousCallback(T returnValue)
        //{
        //    return CommandServerCallback<string>.SynchronousCallback(queue, this, method, returnValue);
        //}
    }
}
