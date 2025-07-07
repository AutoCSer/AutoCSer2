using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    public abstract class CallInputMethod : Method
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
        internal CallInputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.CallInput, flags) { }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void CallInput(CallInputMethodParameter parameter);
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal CallStateEnum LoadCall(CallInputMethodParameter parameter)
        {
            try
            {
                CallInput(parameter);
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
    public abstract class CallInputMethod<T> : CallInputMethod
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
        public CallInputMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, flags) { }
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
            return new CallInputMethodParameter<T>(node, this);
        }
    }
}
