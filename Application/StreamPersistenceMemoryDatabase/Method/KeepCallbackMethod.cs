using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    public abstract class KeepCallbackMethod : Method
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
        public KeepCallbackMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.KeepCallback, flags) { }
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
        public KeepCallbackMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
#if NetStandard21
        public abstract void KeepCallback(ServerNode node, ref CommandServerKeepCallback<KeepCallbackResponseParameter>? callback);
#else
        public abstract void KeepCallback(ServerNode node, ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
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
                var callback = default(CommandServerKeepCallback<KeepCallbackResponseParameter>);
                KeepCallback(node, ref callback);
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
        /// 枚举回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="callback"></param>
        /// <param name="flag">服务端节点方法标记</param>
#if NetStandard21
        public static void EnumerableCallback<T>(System.Collections.Generic.IEnumerable<T> values, ref CommandServerKeepCallback<KeepCallbackResponseParameter>? callback, MethodFlagsEnum flag)
#else
        internal static void EnumerableCallback<T>(System.Collections.Generic.IEnumerable<T> values, ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, MethodFlagsEnum flag)
#endif
        {
            if (callback != null)
            {
                if (values == null || callback.Callback(KeepCallbackResponseParameter.CreateValues(values, flag)))
                {
                    callback.CancelKeep();
                    callback = null;
                }
            }
        }
    }
}
