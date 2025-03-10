using System;

namespace AutoCSer.TestCase.SearchCommon
{
    /// <summary>
    /// 内存数据库节点类型
    /// </summary>
    public enum NodeTypeEnum : byte
    {
        /// <summary>
        /// 用户搜索非索引条件数据
        /// </summary>
        SearchUserNode,
        /// <summary>
        /// 用户名称分词结果磁盘块索引信息
        /// </summary>
        UserNameNode,
        /// <summary>
        /// 用户备注分词结果磁盘块索引信息
        /// </summary>
        UserRemarkNode,
    }
}
