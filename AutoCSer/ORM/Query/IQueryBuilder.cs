using AutoCSer.Memory;
using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// SQL 查询创建器
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// 是否需要查询
        /// </summary>
        bool IsQuery { get; }
        /// <summary>
        ///  获取查询语句
        /// </summary>
        string Statement { get; }
        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="charStream"></param>
        void GetStatement(CharStream charStream);
    }
}
