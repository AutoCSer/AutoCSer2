using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 数据操作类型
    /// </summary>
    public enum OperationTypeEnum : byte
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        Update,
        /// <summary>
        /// 添加数据
        /// </summary>
        Insert,
        /// <summary>
        /// 删除数据
        /// </summary>
        Delete
    }
}
