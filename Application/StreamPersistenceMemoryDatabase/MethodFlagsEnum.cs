using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法标记
    /// </summary>
    [Flags]
    internal enum MethodFlagsEnum : byte
    {
        /// <summary>
        /// 无标记
        /// </summary>
        None,
        /// <summary>
        /// 是否持久化（涉及写入操作则需要持久化）
        /// </summary>
        IsPersistence = 1,
        /// <summary>
        /// 是否允许客户端调用，否则为服务端内存调用方法
        /// </summary>
        IsClientCall = 2,
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        IsSimpleSerializeParamter = 4,
        /// <summary>
        /// 是否简单反序列化输入数据
        /// </summary>
        IsSimpleDeserializeParamter = 8,
        /// <summary>
        /// 是否忽略持久化回调异常，节点方法必须保证异常时还原恢复内存数据状态，必须关心 new 产生的内存不足异常，在修改数据以前应该将完成所有 new 操作
        /// </summary>
        IsIgnorePersistenceCallbackException = 0x10,
        /// <summary>
        /// 本地调用是否添加到写操作队列
        /// </summary>
        IsWriteQueue = 0x80,
    }
}
