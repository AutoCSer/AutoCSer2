using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据状态
    /// </summary>
    public enum ReadBufferStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 磁盘块已经释放资源
        /// </summary>
        Disposed,
        /// <summary>
        /// 磁盘块服务唯一编号不匹配
        /// </summary>
        Identity,
        /// <summary>
        /// 直接使用索引记录数据的应该在客户端直接生成数据
        /// </summary>
        IndexSize,
        /// <summary>
        /// 不可识别的字节大小
        /// </summary>
        UnidentifiableSize,
        /// <summary>
        /// 数据字节数与索引不匹配
        /// </summary>
        Size,
        /// <summary>
        /// 读取磁盘块字节数与预期字节数不匹配
        /// </summary>
        ReadSize,
        /// <summary>
        /// 根据索引没有找到磁盘块
        /// </summary>
        BlockIndex,
    }
}
