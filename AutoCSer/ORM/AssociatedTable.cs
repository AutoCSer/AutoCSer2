using AutoCSer.Memory;
using AutoCSer.ORM.ConditionExpression;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 模拟关联表格
    /// </summary>
    /// <typeparam name="LT">关联表格模型类型</typeparam>
    /// <typeparam name="RT">被关联表格模型类型</typeparam>
    /// <typeparam name="KT">关联关键字类型</typeparam>
    public sealed class AssociatedTable<LT, RT, KT>
        where LT : class
        where RT : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 关联表格
        /// </summary>
        private readonly TableWriter<LT> leftTable;
        /// <summary>
        /// 关联表格关键字列名称
        /// </summary>
        private readonly string leftKeyName;
        /// <summary>
        /// 获取关联表格关键字委托
        /// </summary>
        internal readonly Func<LT, KT> GetLeftKey;
        /// <summary>
        /// 被关联表格
        /// </summary>
        private readonly TableWriter<RT> rightTable;
        /// <summary>
        /// 被关联表格关键字列名称
        /// </summary>
        private readonly string rightKeyName;
        /// <summary>
        /// 获取被关联表格关键字委托
        /// </summary>
        private readonly Func<RT, KT> getRightKey;
        /// <summary>
        /// 设置关联查询条件以后的附加查询设置委托
        /// </summary>
#if NetStandard21
        private readonly Action<QueryBuilder<RT>>? setQuery;
#else
        private readonly Action<QueryBuilder<RT>> setQuery;
#endif
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <param name="leftTable"></param>
        /// <param name="rightTable"></param>
        /// <param name="setQuery"></param>
#if NetStandard21
        internal AssociatedTable(TableWriter<LT, KT> leftTable, TableWriter<RT, KT> rightTable, Action<QueryBuilder<RT>>? setQuery)
#else
        internal AssociatedTable(TableWriter<LT, KT> leftTable, TableWriter<RT, KT> rightTable, Action<QueryBuilder<RT>> setQuery)
#endif
        {
            this.leftTable = leftTable;
            leftKeyName = leftTable.PrimaryKey.MemberIndex.Member.Name;
            GetLeftKey = leftTable.GetPrimaryKey;
            this.rightTable = rightTable;
            rightKeyName = rightTable.PrimaryKey.MemberIndex.Member.Name;
            getRightKey = rightTable.GetPrimaryKey;
            this.setQuery = setQuery;
        }
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <param name="leftTable"></param>
        /// <param name="rightTable"></param>
        /// <param name="getRightKey"></param>
        /// <param name="setQuery"></param>
#if NetStandard21
        internal AssociatedTable(TableWriter<LT, KT> leftTable, TableWriter<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>>? setQuery)
#else
        internal AssociatedTable(TableWriter<LT, KT> leftTable, TableWriter<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>> setQuery)
#endif
        {
            this.leftTable = leftTable;
            leftKeyName = leftTable.PrimaryKey.MemberIndex.Member.Name;
            GetLeftKey = leftTable.GetPrimaryKey;
            this.rightTable = rightTable;
            rightKeyName = rightTable.Convert(getRightKey);
            this.getRightKey = getRightKey.Compile();
            this.setQuery = setQuery;
        }
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <param name="leftTable"></param>
        /// <param name="getLeftKey"></param>
        /// <param name="rightTable"></param>
        /// <param name="setQuery"></param>
#if NetStandard21
        internal AssociatedTable(TableWriter<LT> leftTable, Expression<Func<LT, KT>> getLeftKey, TableWriter<RT, KT> rightTable, Action<QueryBuilder<RT>>? setQuery)
#else
        internal AssociatedTable(TableWriter<LT> leftTable, Expression<Func<LT, KT>> getLeftKey, TableWriter<RT, KT> rightTable, Action<QueryBuilder<RT>> setQuery)
#endif
        {
            this.leftTable = leftTable;
            leftKeyName = leftTable.Convert(getLeftKey);
            this.GetLeftKey = getLeftKey.Compile();
            this.rightTable = rightTable;
            rightKeyName = rightTable.PrimaryKey.MemberIndex.Member.Name;
            getRightKey = rightTable.GetPrimaryKey;
            this.setQuery = setQuery;

        }
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <param name="leftTable"></param>
        /// <param name="getLeftKey"></param>
        /// <param name="rightTable"></param>
        /// <param name="getRightKey"></param>
        /// <param name="setQuery"></param>
#if NetStandard21
        internal AssociatedTable(TableWriter<LT> leftTable, Expression<Func<LT, KT>> getLeftKey, TableWriter<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>>? setQuery)
#else
        internal AssociatedTable(TableWriter<LT> leftTable, Expression<Func<LT, KT>> getLeftKey, TableWriter<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>> setQuery)
#endif
        {
            this.leftTable = leftTable;
            leftKeyName = leftTable.Convert(getLeftKey);
            this.GetLeftKey = getLeftKey.Compile();
            this.rightTable = rightTable;
            rightKeyName = rightTable.Convert(getRightKey);
            this.getRightKey = getRightKey.Compile();
            this.setQuery = setQuery;
        }
        /// <summary>
        /// 获取被连接模拟查询信息
        /// </summary>
        /// <param name="leftValues">连接数据集合</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <returns></returns>
        public OnJoinQuery<LT, RT, KT> GetOnJoinQuery(IEnumerable<LT> leftValues, bool isTransaction = false)
        {
            QueryBuilder<RT> query = rightTable.CreateQuery(null, isTransaction);
            MemberColumnIndex columnIndex;
            if (!rightTable.ColumnNames.TryGetValue(rightKeyName, out columnIndex)) throw new InvalidCastException($"{rightTable.TableName} 没有找到数据列 {rightKeyName}");
            var keys = default(HashSet<KT>);
            foreach (LT leftValue in leftValues)
            {
                KT key = GetLeftKey(leftValue);
                if (keys == null) keys = HashSetCreator<KT>.Create();
                keys.Add(key);
            }
            if (keys == null) query.ConditionLogicType = LogicTypeEnum.False;
            else query.And(new QueryParameter.MemberParameter(ref columnIndex, new QueryParameter.FieldParameter(null, QueryParameterAttribute.In, typeof(KT), null, 0)), keys);
            return new OnJoinQuery<LT, RT, KT>(this, query, leftValues, keys != null ? keys.Count : 0);
        }
        /// <summary>
        /// 主表格创建 EXISTS 子查询条件
        /// </summary>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <returns></returns>
        public QueryBuilder<RT> CreateExistsSqlQuery(bool isTransaction = false)
        {
            return rightTable.CreateQuery(null, isTransaction).And(new AssociatedTableExistsCondition<LT, RT, KT>(this, false));
        }
        /// <summary>
        /// 子表格创建 EXISTS 条件
        /// </summary>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <returns></returns>
        public QueryBuilder<LT> CreateLeftExistsSqlQuery(bool isTransaction = false)
        {
            return leftTable.CreateQuery(null, isTransaction).And(new AssociatedTableExistsCondition<LT, RT, KT>(this, true));
        }
        /// <summary>
        /// 写入 EXISTS 条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="isLeft"></param>
        internal void WriteExists(CharStream charStream, bool isLeft)
        {
            if (isLeft)
            {
                leftTable.ConnectionPool.Creator.FormatName(charStream, leftKeyName);
                charStream.Write('=');
                rightTable.ConnectionPool.Creator.FormatName(charStream, rightTable.TableName);
                charStream.Write('.');
                rightTable.ConnectionPool.Creator.FormatName(charStream, rightKeyName);
            }
            else
            {
                rightTable.ConnectionPool.Creator.FormatName(charStream, rightKeyName);
                charStream.Write('=');
                leftTable.ConnectionPool.Creator.FormatName(charStream, leftTable.TableName);
                charStream.Write('.');
                leftTable.ConnectionPool.Creator.FormatName(charStream, leftKeyName);
            }
        }
    }
}
