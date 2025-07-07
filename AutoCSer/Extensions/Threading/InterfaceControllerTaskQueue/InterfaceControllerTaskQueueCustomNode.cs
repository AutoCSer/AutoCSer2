using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 自定义任务队列节点 await AutoCSer.Net.CommandClientReturnValue
    /// </summary>
    public abstract class InterfaceControllerTaskQueueCustomNode : InterfaceControllerTaskQueueNode
    {
        /// <summary>
        /// 接口任务队列
        /// </summary>
#if NetStandard21
        private InterfaceControllerTaskQueue? queue;
#else
        private InterfaceControllerTaskQueue queue;
#endif
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueCustomNode(ClientCallbackTypeEnum callbackType) : base(callbackType) { }
        /// <summary>
        /// Check whether it has been added to the queue
        /// 检查是否已经添加到队列
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckQueue(InterfaceControllerTaskQueue queue)
        {
            return System.Threading.Interlocked.CompareExchange(ref this.queue, queue, null) == null;
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetReturn()
        {
            SetReturnType(CommandClientReturnTypeEnum.Success, queue?.ThreadId == System.Environment.CurrentManagedThreadId);
        }
    }
    /// <summary>
    /// 自定义任务队列节点 await AutoCSer.Net.CommandClientReturnValue{T}
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public abstract class InterfaceControllerTaskQueueCustomNode<T> : InterfaceControllerTaskQueueNode<T>
    {
        /// <summary>
        /// 接口任务队列
        /// </summary>
#if NetStandard21
        private InterfaceControllerTaskQueue? queue;
#else
        private InterfaceControllerTaskQueue queue;
#endif
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueCustomNode(ClientCallbackTypeEnum callbackType) : base(callbackType) { }
        /// <summary>
        /// Check whether it has been added to the queue
        /// 检查是否已经添加到队列
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckQueue(InterfaceControllerTaskQueue queue)
        {
            return System.Threading.Interlocked.CompareExchange(ref this.queue, queue, null) == null;
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetReturn(T returnValue)
        {
            base.SetReturn(returnValue, queue?.ThreadId == System.Environment.CurrentManagedThreadId);
        }
    }
}
