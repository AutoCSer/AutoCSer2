using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 写操作类型
    /// </summary>
    public enum WriteOperationTypeEnum : byte
    {
        /// <summary>
        /// 追加数据
        /// </summary>
        Append,
        /// <summary>
        /// 切换磁盘块
        /// </summary>
        SwitchBlock,
    }
}
