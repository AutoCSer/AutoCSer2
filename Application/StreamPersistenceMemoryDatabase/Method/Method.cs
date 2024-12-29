using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法
    /// </summary>
    internal abstract class Method
    {
        /// <summary>
        /// 方法编号
        /// </summary>
        internal readonly int Index;
        /// <summary>
        /// 持久化之前参数检查方法编号
        /// </summary>
        internal readonly int BeforePersistenceMethodIndex;
        /// <summary>
        /// 方法调用类型
        /// </summary>
        internal readonly CallTypeEnum CallType;
        /// <summary>
        /// 服务端节点方法标记
        /// </summary>
        internal readonly MethodFlagsEnum Flags;
        /// <summary>
        /// 是否持久化（涉及写入操作则需要持久化）
        /// </summary>
        internal readonly bool IsPersistence;
        /// <summary>
        /// 是否允许客户端调用，否则为服务端内存调用方法
        /// </summary>
        internal readonly bool IsClientCall;
        ///// <summary>
        ///// 是否简单序列化输出数据
        ///// </summary>
        //internal bool IsSimpleSerializeParamter
        //{
        //    get { return (Flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0; }
        //}
        /// <summary>
        /// 是否简单反序列化输入数据
        /// </summary>
        internal bool IsSimpleDeserializeParamter
        {
            get { return (Flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0; }
        }
        /// <summary>
        /// 是否忽略持久化回调异常，节点方法必须保证异常时还原恢复内存数据状态，必须关心 new 产生的内存不足异常，在修改数据以前应该将完成所有 new 操作
        /// </summary>
        internal bool IsIgnorePersistenceCallbackException
        {
            get { return (Flags & MethodFlagsEnum.IsIgnorePersistenceCallbackException) != 0; }
        }
        ///// <summary>
        ///// 是否快照调用方法，该方法必须只有 1 个参数且类型匹配快照数据
        ///// </summary>
        //internal bool IsSnapshotMethod
        //{
        //    get { return (flags & MethodFlags.IsSnapshotMethod) != 0; }
        //}
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        /// <param name="index">方法编号</param>
        /// <param name="beforePersistenceMethodIndex">持久化之前参数检查方法编号</param>
        /// <param name="callType">方法调用类型</param>
        /// <param name="flags">服务端节点方法标记</param>
        internal Method(int index, int beforePersistenceMethodIndex, CallTypeEnum callType, MethodFlagsEnum flags)
        {
            Index = index;
            BeforePersistenceMethodIndex = beforePersistenceMethodIndex;
            CallType = callType;
            this.Flags = flags;
            IsPersistence = (flags & MethodFlagsEnum.IsPersistence) != 0;
            IsClientCall = (flags & MethodFlagsEnum.IsClientCall) != 0;
        }
    }
}
