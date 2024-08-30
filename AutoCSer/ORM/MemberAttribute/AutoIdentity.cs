using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 自增ID 记录表格
    /// </summary>
    [Model(TableName = "AutoCSerAutoIdentity")]
    internal class AutoIdentity
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        [String(PrimaryKeyType = PrimaryKeyTypeEnum.PrimaryKey, IsEmpty = false, IsAscii = true, Size = 256)]
        public string TableName;
        /// <summary>
        /// 当前最大自增ID
        /// </summary>
        public long Identity;
    }
}
