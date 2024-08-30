using AutoCSer.ORM.ConditionExpression;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 被连接模拟查询信息 
    /// </summary>
    /// <typeparam name="LT">连接数据类型</typeparam>
    /// <typeparam name="RT">被连接表格模型类型</typeparam>
    /// <typeparam name="KT">连接关键字类型</typeparam>
    public sealed class OnJoinQuery<LT, RT, KT>
        where LT : class
        where RT : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// SQL 查询创建器
        /// </summary>
        public readonly QueryBuilder<RT> Query;
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        private readonly AssociatedTable<LT, RT, KT> associatedTable;
        /// <summary>
        /// 连接数据集合
        /// </summary>
        private readonly IEnumerable<LT> leftValues;
        /// <summary>
        /// 查询关键字数量
        /// </summary>
        private readonly int keyCount;
        /// <summary>
        /// 被连接模拟查询信息
        /// </summary>
        /// <param name="associatedTable">模拟关联表格</param>
        /// <param name="query">SQL 查询创建器</param>
        /// <param name="leftValues">连接数据集合</param>
        /// <param name="keyCount">查询关键字数量</param>
        internal OnJoinQuery(AssociatedTable<LT, RT, KT> associatedTable, QueryBuilder<RT> query,IEnumerable<LT> leftValues, int keyCount)
        {
            this.associatedTable = associatedTable;
            Query = query;
            this.leftValues = leftValues;
            this.keyCount = keyCount;
        }
        /// <summary>
        /// 模拟被连接操作
        /// </summary>
        /// <typeparam name="VT">被连接数据类型</typeparam>
        /// <param name="getJoinKey">获取被连接数据关键字委托</param>
        /// <param name="join">设置查询数据结果连接操作</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task OnJoin<VT>(Func<RT, KT> getJoinKey, Action<LT, VT> join, int timeoutSeconds = 0, Transaction transaction = null)
            where VT : class, RT
        {
            switch (Query.ConditionLogicType)
            {
                case LogicTypeEnum.False: return;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            Dictionary<KT, VT> values = null;
            Query.TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            VT value;
#if DotNet45 || NetStandard2
            IEnumeratorTask<VT> selectEnumerator = await Query.TableWriter.Select<VT>(Query.GetQuery(keyCount, timeoutSeconds, 0), transaction);
            try
            {
                while (await selectEnumerator.MoveNextAsync())
                {
                    value = selectEnumerator.Current;
                    if (values == null) values = keyCount == 0 ? DictionaryCreator<KT>.Create<VT>() : DictionaryCreator<KT>.Create<VT>(keyCount);
                    values.Add(getJoinKey(value), value);
                }
            }
            finally { await selectEnumerator.DisposeAsync(); }
#else
            await using (IAsyncEnumerator<VT> selectEnumerator = await Query.TableWriter.Select<VT>(Query.GetQuery(keyCount, timeoutSeconds, 0), transaction))
            {
                while (await selectEnumerator.MoveNextAsync())
                {
                    value = selectEnumerator.Current;
                    if (values == null) values = keyCount == 0 ? DictionaryCreator<KT>.Create<VT>() : DictionaryCreator<KT>.Create<VT>(keyCount);
                    values.Add(getJoinKey(value), value);
                }
            }
#endif
            if (values == null) return;
            foreach (LT leftValue in leftValues)
            {
                if (values.TryGetValue(associatedTable.GetLeftKey(leftValue), out value)) join(leftValue, value);
            }
        }
        /// <summary>
        /// 模拟被连接操作
        /// </summary>
        /// <typeparam name="VT">被连接数据类型</typeparam>
        /// <param name="getJoinKey">获取被连接数据关键字委托</param>
        /// <param name="join">设置查询数据结果连接操作</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task OnJoin<VT>(Func<RT, KT> getJoinKey, Action<LT, List<VT>> join, int timeoutSeconds = 0, Transaction transaction = null)
            where VT : class, RT
        {
            switch (Query.ConditionLogicType)
            {
                case LogicTypeEnum.False: return;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            Dictionary<KT, List<VT>> values = null;
            Query.TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            List<VT> valueList;
#if DotNet45 || NetStandard2
            IEnumeratorTask<VT> selectEnumerator = await Query.TableWriter.Select<VT>(Query.GetQuery(keyCount, timeoutSeconds, 0), transaction);
            try
            {
                while (await selectEnumerator.MoveNextAsync())
                {
                    VT value = selectEnumerator.Current;
                    KT key = getJoinKey(value);
                    if (values == null) values = keyCount == 0 ? DictionaryCreator<KT>.Create<List<VT>>() : DictionaryCreator<KT>.Create<List<VT>>(keyCount);
                    if (!values.TryGetValue(key, out valueList)) values.Add(key, valueList = new List<VT>());
                    valueList.Add(value);
                }
            }
            finally { await selectEnumerator.DisposeAsync(); }
#else
            await using (IAsyncEnumerator<VT> selectEnumerator = await Query.TableWriter.Select<VT>(Query.GetQuery(keyCount, timeoutSeconds, 0), transaction))
            {
                while (await selectEnumerator.MoveNextAsync())
                {
                    VT value = selectEnumerator.Current;
                    KT key = getJoinKey(value);
                    if (values == null) values = keyCount == 0 ? DictionaryCreator<KT>.Create<List<VT>>() : DictionaryCreator<KT>.Create<List<VT>>(keyCount);
                    if (!values.TryGetValue(key, out valueList)) values.Add(key, valueList = new List<VT>());
                    valueList.Add(value);
                }
            }
#endif
            if (values == null) return;
            foreach (LT leftValue in leftValues)
            {
                if (values.TryGetValue(associatedTable.GetLeftKey(leftValue), out valueList)) join(leftValue, valueList);
            }
        }
    }
}
