using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    public abstract class SendOnlyMethod : Method
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal SendOnlyMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.SendOnly, flags) { }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void SendOnly(SendOnlyMethodParameter parameter);
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal CallStateEnum LoadCall(SendOnlyMethodParameter parameter)
        {
            try
            {
                SendOnly(parameter);
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
        internal abstract SendOnlyMethodParameter CreateParameter(ServerNode node);
    }
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    public abstract class SendOnlyMethod<T> : SendOnlyMethod
        where T : struct
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        public SendOnlyMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, flags) { }
        /// <summary>
        /// 创建方法调用参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override SendOnlyMethodParameter CreateParameter(ServerNode node)
        {
            return new SendOnlyMethodParameter<T>(node, this);
        }
    }
}
