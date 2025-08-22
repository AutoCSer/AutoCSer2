using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT"></typeparam>
    internal sealed class LocalServiceInputTwoStageCallbackNode<T, KT> : LocalServiceTwoStageCallbackNode<T>
    {
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly InputTwoStageCallbackMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackNodeCallback<KT> keepCallback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="callback">The first stage returns the parameter callback
        /// 第一阶段返回参数回调</param>
        /// <param name="parameter">调用方法与参数信息</param>
        /// <param name="keepCallback">The second stage returns the parameter callback
        /// 第二阶段返回参数回调</param>
        internal LocalServiceInputTwoStageCallbackNode(LocalClientNode clientNode, Action<LocalResult<T>> callback, InputTwoStageCallbackMethodParameter parameter, Action<LocalResult<KT>> keepCallback) : base(clientNode, callback)
        {
            this.parameter = parameter;
            this.keepCallback = new LocalServiceKeepCallbackNodeCallback<KT>(keepCallback, true);
            Result = this.keepCallback;
            if ((parameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) service.CommandServerCallQueue.AppendReadOnly(this);
            else service.CommandServerCallQueue.AppendWriteOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="callback">The first stage returns the parameter callback
        /// 第一阶段返回参数回调</param>
        /// <param name="result"></param>
        /// <param name="keepCallback">The second stage returns the parameter callback
        /// 第二阶段返回参数回调</param>
        internal LocalServiceInputTwoStageCallbackNode(LocalClientNode clientNode, Action<LocalResult<T>> callback, CallStateEnum result, Action<LocalResult<KT>> keepCallback) : base(clientNode, callback)
        {
            IsCompleted = true;
            continuation = Common.EmptyAction;
            this.keepCallback = new LocalServiceKeepCallbackNodeCallback<KT>(keepCallback, true);
            this.Result = this.keepCallback;
            try
            {
                callback(result);
            }
            catch 
            {
                this.keepCallback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(result));
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.InputTwoStageCallback(parameter, ServerCallback, keepCallback);
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
    public static class LocalServiceInputTwoStageCallbackNode
    {
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call parameters
        /// 调用参数</param>
        /// <param name="callback">The first stage returns the parameter callback
        /// 第一阶段返回参数回调</param>
        /// <param name="keepCallback">The second stage returns the parameter callback
        /// 第二阶段返回参数回调</param>
        /// <returns></returns>
        public static LocalServiceQueueNode<IDisposable> Create<T, KT, PT>(LocalClientNode clientNode, int methodIndex, PT parameter, Action<LocalResult<T>> callback, Action<LocalResult<KT>> keepCallback) where PT : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            var inputKeepCallbackMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<InputTwoStageCallbackMethodParameter<PT>>();
            if (state == CallStateEnum.Success)
            {
                var methodParameter = inputKeepCallbackMethodParameter.notNull();
                methodParameter.Parameter = parameter;
                return new LocalServiceInputTwoStageCallbackNode<T, KT>(clientNode, callback, methodParameter, keepCallback);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceInputTwoStageCallbackNode<T, KT>(clientNode, callback, state, keepCallback);
        }
    }
}
