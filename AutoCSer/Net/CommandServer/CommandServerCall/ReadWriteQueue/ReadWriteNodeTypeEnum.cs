using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Synchronous read and write queue node types
    /// 同步读写队列节点类型
    /// </summary>
    public enum ReadWriteNodeTypeEnum : byte
    {
        /// <summary>
        /// Read operation
        /// 读操作
        /// </summary>
        Read,
        /// <summary>
        /// Concurrent read operations are used to concurrently execute read request operations during the acquisition of snapshot data in an in-memory database
        /// 并发读操作，用于内存数据库获取快照数据期间并发执行读请求操作
        /// </summary>
        ConcurrencyRead,
        /// <summary>
        /// Write operation
        /// 写操作
        /// </summary>
        Write,
    }
}
