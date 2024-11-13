using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    internal abstract class CallMethod : Method
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal CallMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.Call, flags) { }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
#if NetStandard21
        public abstract void Call(ServerNode node, ref CommandServerCallback<CallStateEnum>? callback);
#else
        public abstract void Call(ServerNode node, ref CommandServerCallback<CallStateEnum> callback);
#endif
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal CallStateEnum LoadCall(ServerNode node)
        {
            try
            {
                var callback = default(CommandServerCallback<CallStateEnum>);
                Call(node, ref callback);
                return CallStateEnum.Success;
            }
            catch (Exception exception)
            {
                node.SetLoadException();
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            return CallStateEnum.PersistenceCallbackException;
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void Callback(ref CommandServerCallback<CallStateEnum>? callback)
#else
        internal static void Callback(ref CommandServerCallback<CallStateEnum> callback)
#endif
        {
            var callbackCopy = callback;
            if (callbackCopy != null)
            {
                callback = null;
                callbackCopy.SynchronousCallback(CallStateEnum.Success);
            }
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        internal delegate void CallbackDelegate(ref CommandServerCallback<CallStateEnum>? callback);
#else
        internal delegate void CallbackDelegate(ref CommandServerCallback<CallStateEnum> callback);
#endif
    }
}
