﻿using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    internal abstract class KeepCallbackMethod : Method
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal KeepCallbackMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.KeepCallback, flags) { }
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="callType">方法调用类型</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal KeepCallbackMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
        public abstract void KeepCallback(ServerNode node, ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal CallStateEnum LoadCall(ServerNode node)
        {
            try
            {
                CommandServerKeepCallback<KeepCallbackResponseParameter> callback = null;
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
        /// <param name="isSimpleSerialize"></param>
        internal static void EnumerableCallback<T>(System.Collections.Generic.IEnumerable<T> values, ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, bool isSimpleSerialize)
        {
            if (callback != null)
            {
                if (values == null || callback.Callback(KeepCallbackResponseParameter.CreateValues(values, isSimpleSerialize)))
                {
                    callback.CancelKeep();
                    callback = null;
                }
            }
        }
    }
}