using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 操作数据类型
    /// </summary>
    [Flags]
    public enum OperationDataTypeEnum : byte
    {
        /// <summary>
        /// 用户搜索非索引条件数据
        /// </summary>
        SearchUserNode = 1,
        /// <summary>
        /// 用户名称
        /// </summary>
        UserNameNode = 2,
        /// <summary>
        /// 用户备注
        /// </summary>
        UserRemarkNode = 4,

        /// <summary>
        /// 所有用户数据
        /// </summary>
        All = 0xff
    }
}
