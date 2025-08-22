using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LocalServiceTwoStageCallbackNode<T> : LocalServiceQueueNode<IDisposable>
    {
        /// <summary>
        /// Local service client node
        /// 本地服务客户端节点
        /// </summary>
        protected readonly LocalClientNode clientNode;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        internal readonly LocalServiceTwoStageCallbackNodeCallback<T> ServerCallback;
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        protected readonly Action<LocalResult<T>> callback;
        /// <summary>
        /// 回调访问锁
        /// </summary>
        private int isCallback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="callback">The first stage returns the parameter callback
        /// 第一阶段返回参数回调</param>
        protected LocalServiceTwoStageCallbackNode(LocalClientNode clientNode, Action<LocalResult<T>> callback) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.callback = callback;
            ServerCallback = new LocalServiceTwoStageCallbackNodeCallback<T>(this);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="responseParameter"></param>
        /// <returns></returns>
        internal bool Callback(ResponseParameter responseParameter)
        {
            if (Interlocked.CompareExchange(ref isCallback, 1, 0) == 0)
            {
                if (responseParameter.State == CallStateEnum.Success) callback(((ResponseParameter<T>)responseParameter).Value.ReturnValue);
                else
                {
                    CallStateEnum state = responseParameter.State;
                    try
                    {
                        callback(state);
                    }
                    finally
                    {
                        cancelKeep(state);
                        clientNode.CheckState(clientNode.Index, responseParameter.State);
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        /// <param name="state"></param>
        protected abstract void cancelKeep(CallStateEnum state);
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT"></typeparam>
    public sealed class LocalServiceTwoStageCallbackNode<T, KT> : LocalServiceTwoStageCallbackNode<T>
    {
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackNodeCallback<KT> keepCallback;
        /// <summary>
        /// 调用方法编号
        /// </summary>
        private readonly int methodIndex;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The first stage returns the parameter callback
        /// 第一阶段返回参数回调</param>
        /// <param name="keepCallback">The second stage returns the parameter callback
        /// 第二阶段返回参数回调</param>
        /// <param name="isWriteQueue"></param>
        internal LocalServiceTwoStageCallbackNode(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback, Action<LocalResult<KT>> keepCallback, bool isWriteQueue) : base(clientNode, callback)
        {
            this.methodIndex = methodIndex;
            this.keepCallback = new LocalServiceKeepCallbackNodeCallback<KT>(keepCallback, true);
            Result = this.keepCallback;
            if (isWriteQueue) service.CommandServerCallQueue.AppendWriteOnly(this);
            else service.CommandServerCallQueue.AppendReadOnly(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.TwoStageCallback(clientNode.Index, methodIndex, ServerCallback, keepCallback);
            completed();
        }
        /// <summary>
        /// 调用方主动取消回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            keepCallback.Dispose();
        }
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        /// <param name="state"></param>
        protected override void cancelKeep(CallStateEnum state)
        {
            keepCallback.CancelKeep(state);
        }
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public static class LocalServiceTwoStageCallbackNode
    {
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The first stage returns the parameter callback
        /// 第一阶段返回参数回调</param>
        /// <param name="keepCallback">The second stage returns the parameter callback
        /// 第二阶段返回参数回调</param>
        /// <param name="isWriteQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<IDisposable> Create<T, KT>(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback, Action<LocalResult<KT>> keepCallback, bool isWriteQueue)
        {
            return new LocalServiceTwoStageCallbackNode<T, KT>(clientNode, methodIndex, callback, keepCallback, isWriteQueue);
        }
    }
}
