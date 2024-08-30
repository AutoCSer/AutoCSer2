using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 表格操作事件类型
    /// </summary>
    internal enum TableEventTypeEnum : byte
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        Insert,
        /// <summary>
        /// 更新数据
        /// </summary>
        Update,
        /// <summary>
        /// 删除数据
        /// </summary>
        Delete
    }
}
