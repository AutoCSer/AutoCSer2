using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块回调操作类型
    /// </summary>
    internal enum BlockCallbackTypeEnum
    {
        /// <summary>
        /// 写入操作回调
        /// </summary>
        Flush,
        /// <summary>
        /// 读取数据操作回调
        /// </summary>
        Read,
        /// <summary>
        /// 检查磁盘块服务缓存位置索引节点
        /// </summary>
        CheckServiceIndexCacheNode,
        /// <summary>
        /// 释放资源
        /// </summary>
        Dispose,
    }
}
