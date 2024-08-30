using AutoCSer.Memory;
using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    internal interface ICondition
    {
        /// <summary>
        /// 写入条件
        /// </summary>
        /// <param name="charStream"></param>
        void WriteCondition(CharStream charStream);
    }
}
