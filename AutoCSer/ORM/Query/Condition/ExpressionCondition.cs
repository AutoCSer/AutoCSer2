using AutoCSer.Memory;
using System;
using System.Linq.Expressions;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 表达式条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ExpressionCondition<T> : ICondition 
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        private readonly TableWriter<T> tableWriter;
        /// <summary>
        /// 条件表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression expression;
        /// <summary>
        /// 表达式条件
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="expression"></param>
        internal ExpressionCondition(TableWriter<T> tableWriter, System.Linq.Expressions.Expression expression)
        {
            this.tableWriter = tableWriter;
            this.expression = expression;
        }
        /// <summary>
        /// 写入条件
        /// </summary>
        /// <param name="charStream"></param>
        void ICondition.WriteCondition(CharStream charStream)
        {
            new ExpressionConverter(tableWriter, charStream).ConvertAnd(expression);
        }
    }
}
