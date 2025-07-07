using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    public abstract class CallOutputMethod : Method
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
        public CallOutputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.CallOutput, flags) { }
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        /// <param name="index">Method Number
        /// 方法编号</param>
        /// <param name="beforePersistenceMethodIndex">The method number that checks the input parameter before the persistence operation
        /// 持久化操作之前检查输入参数的方法编号</param>
        /// <param name="callType">Method call type
        /// 方法调用类型</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        public CallOutputMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
#if NetStandard21
        public virtual void CallOutput(ServerNode node, ref CommandServerCallback<ResponseParameter>? callback) { }
#else
        public virtual void CallOutput(ServerNode node, ref CommandServerCallback<ResponseParameter> callback) { }
#endif
        /// <summary>
        /// 持久化操作之前检查输入参数
        /// </summary>
        /// <param name="node"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public virtual ValueResult<ResponseParameter> CallOutputBeforePersistence(ServerNode node) { return default(ValueResult<ResponseParameter>); }
        /// <summary>
        /// 持久化操作之前检查输入参数
        /// </summary>
        /// <param name="node"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public virtual bool CallBeforePersistence(ServerNode node) { return true; }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal CallStateEnum LoadCall(ServerNode node)
        {
            try
            {
                var callback = default(CommandServerCallback<ResponseParameter>);
                CallOutput(node, ref callback);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="callback"></param>
        /// <param name="flag">服务端节点方法标记</param>
#if NetStandard21
        public static void Callback<T>(T value, ref CommandServerCallback<ResponseParameter>? callback, MethodFlagsEnum flag)
#else
        internal static void Callback<T>(T value, ref CommandServerCallback<ResponseParameter> callback, MethodFlagsEnum flag)
#endif
        {
            var callbackCopy = callback;
            if (callbackCopy != null)
            {
                callback = null;
                bool isCallback = false;
                try
                {
                    ResponseParameter responseParameter = ResponseParameter.Create(value, flag);
                    isCallback = true;
                    callbackCopy.SynchronousCallback(responseParameter);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    if (!isCallback) callbackCopy.Socket.DisposeSocket();
                }
            }
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="value"></param>
        /// <param name="callback"></param>
#if NetStandard21
        public static void CallbackResponseParameter(ResponseParameter value, ref CommandServerCallback<ResponseParameter>? callback)
#else
        internal static void CallbackResponseParameter(ResponseParameter value, ref CommandServerCallback<ResponseParameter> callback)
#endif
        {
            var callbackCopy = callback;
            if (callbackCopy != null)
            {
                callback = null;
                try
                {
                    callbackCopy.SynchronousCallback(value);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 获取持久化检查方法返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="flag">服务端节点方法标记</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ValueResult<ResponseParameter> GetBeforePersistenceResponseParameter<T>(ValueResult<T> value, MethodFlagsEnum flag)
        {
            if (value.IsValue) return ResponseParameter.Create(value.Value, flag);
            return default(ValueResult<ResponseParameter>);
        }
    }
}
