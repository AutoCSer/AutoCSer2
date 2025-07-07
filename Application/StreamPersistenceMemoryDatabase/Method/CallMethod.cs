using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    public abstract class CallMethod : Method
    {
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        /// <param name="index">Method Number
        /// 方法编号</param>
        /// <param name="beforePersistenceMethodIndex">The method number that checks the input parameter before the persistence operation
        /// 持久化操作之前检查输入参数的方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        public CallMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.Call, flags) { }
        /// <summary>
        /// Call the node method
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
        public static void Callback(ref CommandServerCallback<CallStateEnum>? callback)
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
