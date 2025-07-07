using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// Data operation type
    /// 数据操作类型
    /// </summary>
    public enum OperationTypeEnum : byte
    {
        /// <summary>
        /// Update the data
        /// 更新数据
        /// </summary>
        Update,
        /// <summary>
        /// Add data
        /// 添加数据
        /// </summary>
        Insert,
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        Delete
    }
}
