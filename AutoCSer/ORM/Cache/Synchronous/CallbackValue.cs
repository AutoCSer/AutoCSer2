using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Cache.Synchronous
{
    /// <summary>
    /// 缓存数据同步回调数据
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CallbackValue<T> where T : class
    {
        /// <summary>
        /// 同步数据
        /// </summary>
#if NetStandard21
        public T? Value;
#else
        public T Value;
#endif
        /// <summary>
        /// 同步操作类型
        /// </summary>
        public OperationTypeEnum OperationType;
        /// <summary>
        /// 缓存数据同步回调数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        internal CallbackValue(T value, OperationTypeEnum operationType)
        {
            Value = value;
            OperationType = operationType;
        }
        /// <summary>
        /// 缓存数据同步回调数据
        /// </summary>
        /// <param name="operationType"></param>
        internal CallbackValue(OperationTypeEnum operationType)
        {
            Value = null;
            OperationType = operationType;
        }
        /// <summary>
        /// 设置缓存数据同步回调数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T value, OperationTypeEnum operationType)
        {
            Value = value;
            OperationType = operationType;
        }
    }
}
