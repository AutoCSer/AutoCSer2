using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    public abstract class TwoStageCallbackMethod : Method
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
        public TwoStageCallbackMethod(int index, int beforePersistenceMethodIndex, MethodFlagsEnum flags) : base(index, beforePersistenceMethodIndex, CallTypeEnum.TwoStageCallback, flags) { }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
#if NetStandard21
        public abstract void TwoStageCallback(ServerNode node, CommandServerCallback<ResponseParameter>? callback, ref CommandServerKeepCallback<KeepCallbackResponseParameter>? keepCallback);
#else
        public abstract void TwoStageCallback(ServerNode node, CommandServerCallback<ResponseParameter> callback, ref CommandServerKeepCallback<KeepCallbackResponseParameter> keepCallback);
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
                var keepCallback = default(CommandServerKeepCallback<KeepCallbackResponseParameter>);
                TwoStageCallback(node, null, ref keepCallback);
                return CallStateEnum.Success;
            }
            catch (Exception exception)
            {
                node.SetLoadException();
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            return CallStateEnum.PersistenceCallbackException;
        }
    }
}
