using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    public abstract class CallInputOutputMethod : Method
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
        internal CallInputOutputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.CallInputOutput, flags) { }
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
        internal CallInputOutputMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual void CallInputOutput(CallInputOutputMethodParameter parameter) { }
        /// <summary>
        /// 持久化操作之前检查输入参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public virtual ValueResult<ResponseParameter> CallOutputBeforePersistence(CallInputOutputMethodParameter parameter) { return default(ValueResult<ResponseParameter>); }
        /// <summary>
        /// 持久化操作之前检查输入参数
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
    }
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    public abstract class CallInputOutputMethod<T> : CallInputOutputMethod
        where T : struct
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
        public CallInputOutputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, flags) { }
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
        public CallInputOutputMethod(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, callType, flags) { }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
#if NetStandard21
        internal override InputMethodParameter? CreateInputParameter(ServerNode node)
#else
        internal override InputMethodParameter CreateInputParameter(ServerNode node)
#endif
        {
            return new CallInputOutputMethodParameter<T>(node, this);
        }
    }
}
