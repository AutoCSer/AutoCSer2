using System;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// 索引类型
    /// </summary>
    [Flags]
    internal enum TableIndexStateEnum : int
    {
        ///// <summary>
        ///// 如果试图插入重复的键，则取消命令
        ///// </summary>
        //CancelDuplicateKey = 1,
        /// <summary>
        /// 唯一索引
        /// </summary>
        Unique = 2,
        ///// <summary>
        ///// 如果试图插入重复的行，则取消命令
        ///// </summary>
        //CancelDuplicateRow = 4,
        /// <summary>
        /// 聚簇索引
        /// </summary>
        Clustered = 0x10,
        ///// <summary>
        ///// 索引允许重复行
        ///// </summary>
        //AllowDuplicateRows = 0x40,
        ///// <summary>
        ///// 索引页预留空间
        ///// </summary>
        //Pad = 0x100,
        /// <summary>
        /// 强制主键约束索引
        /// </summary>
        PrimaryKeyConstraint = 0x800,
        /// <summary>
        /// 强制唯一约束索引
        /// </summary>
        UniqueConstraint = 0x1000,
        ///// <summary>
        ///// 有字段允许为空
        ///// </summary>
        //IsNullable = 0x200000,
    }
}
