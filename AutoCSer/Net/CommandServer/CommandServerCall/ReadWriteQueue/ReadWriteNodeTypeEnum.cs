using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 同步读写队列节点类型
    /// </summary>
    public enum ReadWriteNodeTypeEnum : byte
    {
        /// <summary>
        /// 普通读操作，并发受写操作限制
        /// </summary>
        Read,
        /// <summary>
        /// 并发读操作，开始执行以后其它读取操作不受写操作限制
        /// </summary>
        ConcurrencyRead,
        /// <summary>
        /// 写操作
        /// </summary>
        Write,
    }
}
