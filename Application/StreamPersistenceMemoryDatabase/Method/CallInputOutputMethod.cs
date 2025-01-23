using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    internal abstract class CallInputOutputMethod : Method
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal CallInputOutputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.CallInputOutput, flags) { }
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="callType">方法调用类型</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal CallInputOutputMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual void CallInputOutput(CallInputOutputMethodParameter parameter) { }
        /// <summary>
        /// 持久化之前检查参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public virtual ValueResult<ResponseParameter> CallOutputBeforePersistence(CallInputOutputMethodParameter parameter) { return default(ValueResult<ResponseParameter>); }
        /// <summary>
        /// 持久化之前检查参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public virtual bool CallBeforePersistence(CallInputOutputMethodParameter parameter) { return true; }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal CallStateEnum LoadCall(CallInputOutputMethodParameter parameter)
        {
            try
            {
                CallInputOutput(parameter);
                return CallStateEnum.Success;
            }
            catch (Exception exception)
            {
                parameter.Node.SetLoadException();
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            return CallStateEnum.PersistenceCallbackException;
        }
        /// <summary>
        /// 创建方法调用参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract CallInputOutputMethodParameter CreateParameter(ServerNode node);
    }
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    internal abstract class CallInputOutputMethod<T> : CallInputOutputMethod
        where T : struct
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal CallInputOutputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, flags) { }
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="callType">方法调用类型</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal CallInputOutputMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// 创建方法调用参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override CallInputOutputMethodParameter CreateParameter(ServerNode node)
        {
            return new CallInputOutputMethodParameter<T>(node, this);
        }
    }
}
