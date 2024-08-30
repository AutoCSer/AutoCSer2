using AutoCSer.Memory;
using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 模拟关联表格 EXISTS 查询条件
    /// </summary>
    internal sealed class AssociatedTableExistsCondition<LT, RT, KT> : ICondition
        where LT : class
        where RT : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        private readonly AssociatedTable<LT, RT, KT> associatedTable;
        /// <summary>
        /// 是否子表格 EXISTS 查询条件
        /// </summary>
        private readonly bool isLeft;
        /// <summary>
        /// 模拟关联表格 EXISTS 查询条件
        /// </summary>
        /// <param name="associatedTable"></param>
        /// <param name="isLeft"></param>
        internal AssociatedTableExistsCondition(AssociatedTable<LT, RT, KT> associatedTable, bool isLeft)
        {
            this.associatedTable = associatedTable;
            this.isLeft = isLeft;
        }
        /// <summary>
        /// 写入条件
        /// </summary>
        /// <param name="charStream"></param>
        void ICondition.WriteCondition(CharStream charStream)
        {
            associatedTable.WriteExists(charStream, isLeft);
        }
    }
}
