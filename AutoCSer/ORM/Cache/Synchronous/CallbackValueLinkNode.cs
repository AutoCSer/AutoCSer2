using System;

namespace AutoCSer.ORM.Cache.Synchronous
{
    /// <summary>
    /// 缓存数据同步回调数据节点
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    internal sealed class CallbackValueLinkNode<T> : AutoCSer.Threading.Link<CallbackValueLinkNode<T>>
        where T : class
    {
        /// <summary>
        /// 缓存数据同步回调数据
        /// </summary>
        internal CallbackValue<T> Value;
        /// <summary>
        /// 缓存数据同步回调数据节点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        internal CallbackValueLinkNode(T value, OperationTypeEnum operationType)
        {
            Value.Set(value, operationType);
        }
        /// <summary>
        /// 缓存数据同步回调数据节点
        /// </summary>
        /// <param name="operationType"></param>
        internal CallbackValueLinkNode(OperationTypeEnum operationType)
        {
            Value.OperationType = operationType;
        }
    }
}
