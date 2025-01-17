using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 内存数据库回调类型
    /// </summary>
    internal enum ServiceCallbackTypeEnum : byte
    {
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        Load,
        /// <summary>
        /// 释放节点资源
        /// </summary>
        NodeDispose,
    }
}
