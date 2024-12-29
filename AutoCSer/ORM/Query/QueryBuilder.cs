using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.ORM.ConditionExpression;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// SQL 查询创建器
    /// </summary>
    public abstract class QueryBuilder : IQueryBuilder
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        internal readonly TableWriter TableWriter;
        /// <summary>
        /// 条件逻辑值类型
        /// </summary>
        public LogicTypeEnum ConditionLogicType { get; internal set; }
        /// <summary>
        /// 锁类型，事务查询默认为 NONE，否则为 NOLOCK
        /// </summary>
        public WithLockTypeEnum WithLock;
        /// <summary>
        /// 第一个表达式查询条件
        /// </summary>
#if NetStandard21
        internal ICondition? Condition;
#else
        internal ICondition Condition;
#endif
        /// <summary>
        /// 表达式查询条件集合
        /// </summary>
        internal LeftArray<ICondition> Conditions;
        /// <summary>
        /// 第一个排序项
        /// </summary>
        internal OrderItem OrderItem;
        /// <summary>
        /// 排序项集合
        /// </summary>
        internal LeftArray<OrderItem> OrderByItems;
        /// <summary>
        ///  获取查询语句
        /// </summary>
        public abstract string Statement { get; }
        /// <summary>
        /// 是否需要查询
        /// </summary>
        public bool IsQuery { get { return ConditionLogicType != LogicTypeEnum.False; } }
        /// <summary>
        /// 是否存在查询条件
        /// </summary>
        internal bool IsCondition
        {
            get
            {
                return Condition != null && ConditionLogicType != LogicTypeEnum.True;
            }
        }
        /// <summary>
        /// SQL 查询创建器
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <param name="primaryKeyCondition">关键字条件</param>
#if NetStandard21
        internal QueryBuilder(TableWriter tableWriter, bool isTransaction, ICondition? primaryKeyCondition = null)
#else
        internal QueryBuilder(TableWriter tableWriter, bool isTransaction, ICondition primaryKeyCondition = null)
#endif
        {
            TableWriter = tableWriter;
               WithLock = isTransaction ? WithLockTypeEnum.None : WithLockTypeEnum.NoLock;
            Condition = primaryKeyCondition;
        }
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AndCondition(ICondition condition)
        {
            if (Condition == null) Condition = condition;
            else Conditions.Add(condition);
        }
        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="member"></param>
        /// <param name="isAscending"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void orderBy(string member, bool isAscending)
        {
            if (OrderItem.Member == null) OrderItem.Set(member, isAscending);
            else OrderByItems.Add(new OrderItem { Member = member, IsAscending = isAscending });
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <param name="charStream"></param>
        public abstract void GetStatement(CharStream charStream);
    }
    /// <summary>
    /// SQL 查询创建器
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class QueryBuilder<T> : QueryBuilder
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        internal new readonly TableWriter<T> TableWriter;
        /// <summary>
        /// 查询表格字段集合
        /// </summary>
#if NetStandard21
        public MemberMap<T>? MemberMap;
#else
        public MemberMap<T> MemberMap;
#endif
        /// <summary>
        ///  获取查询语句
        /// </summary>
        public override string Statement
        {
            get
            {
                CharStream charStream = TableWriter.ConnectionPool.Creator.GetCharStreamCache();
                try
                {
                    GetStatement(charStream);
                    return charStream.ToString();
                }
                finally { TableWriter.ConnectionPool.Creator.FreeCharStreamCache(charStream); }
            }
        }
        /// <summary>
        /// SQL 查询创建器
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="condition">第一个表达式查询条件</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
#if NetStandard21
        internal QueryBuilder(TableWriter<T> tableWriter, Expression<Func<T, bool>>? condition, bool isTransaction) : base(tableWriter, isTransaction)
#else
        internal QueryBuilder(TableWriter<T> tableWriter, Expression<Func<T, bool>> condition, bool isTransaction) : base(tableWriter, isTransaction)
#endif
        {
            TableWriter = tableWriter;
            ConditionLogicType = LogicTypeEnum.True;
            if (condition != null)
            {
                ConditionExpressionConverter converter = new ConditionExpressionConverter(condition.Body);
                switch (converter.LogicType)
                {
                    case LogicTypeEnum.False: ConditionLogicType = LogicTypeEnum.False; break;
                    case LogicTypeEnum.True: break;
                    case LogicTypeEnum.Unknown:
                        Condition = new ExpressionCondition<T>(tableWriter, converter.Expression);
                        ConditionLogicType = LogicTypeEnum.Unknown;
                        break;
                    default:
                        ConditionLogicType = LogicTypeEnum.NotSupport;
                        throw new InvalidCastException($"不支持的条件表达式 {converter.NotSupportType}");
                }
            }
        }
        /// <summary>
        /// SQL 查询创建器
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <param name="primaryKeyCondition">关键字条件</param>
        /// <param name="memberMap">查询表格字段集合</param>
#if NetStandard21
        internal QueryBuilder(TableWriter<T> tableWriter, bool isTransaction, ICondition primaryKeyCondition, MemberMap<T>? memberMap) : base(tableWriter, isTransaction, primaryKeyCondition)
#else
        internal QueryBuilder(TableWriter<T> tableWriter, bool isTransaction, ICondition primaryKeyCondition, MemberMap<T> memberMap) : base(tableWriter, isTransaction, primaryKeyCondition)
#endif
        {
            TableWriter = tableWriter;
            MemberMap = memberMap;
            ConditionLogicType = LogicTypeEnum.Unknown;
        }
        /// <summary>
        /// SQL 查询创建器
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <param name="primaryKeyCondition">关键字条件</param>
        internal QueryBuilder(TableWriter<T> tableWriter, bool isTransaction, ICondition primaryKeyCondition) : base(tableWriter, isTransaction, primaryKeyCondition)
        {
            TableWriter = tableWriter;
            ConditionLogicType = LogicTypeEnum.Unknown;
        }
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal QueryBuilder<T> And(ICondition condition)
        {
            AndCondition(condition);
            return this;
        }
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        public QueryBuilder<T> And(Expression<Func<T, bool>> condition)
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.True:
                case LogicTypeEnum.Unknown:
                    ConditionExpressionConverter converter = new ConditionExpressionConverter(condition.Body);
                    switch (converter.LogicType)
                    {
                        case LogicTypeEnum.False: ConditionLogicType = LogicTypeEnum.False; break;
                        case LogicTypeEnum.True: break;
                        case LogicTypeEnum.Unknown:
                            AndCondition(new ExpressionCondition<T>(TableWriter, converter.Expression)); 
                            ConditionLogicType = LogicTypeEnum.Unknown;
                            break;
                        default:
                            ConditionLogicType = LogicTypeEnum.NotSupport;
                            throw new InvalidCastException($"不支持的条件表达式 {converter.NotSupportType}");
                    }
                    break;
                default: break;
            }
            return this;
        }
        /// <summary>
        /// 当指定逻辑值为 true 时添加条件
        /// </summary>
        /// <param name="boolean">指定逻辑值</param>
        /// <param name="condition"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public QueryBuilder<T> IfAnd(bool boolean, Expression<Func<T, bool>> condition)
        {
            if (boolean) And(condition);
            return this;
        }
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="memberParameter"></param>
        /// <param name="fieldValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void And(QueryParameter.MemberParameter memberParameter, object fieldValue)
        {
            AndCondition(new QueryCondition<T>(TableWriter, memberParameter, fieldValue));
        }
        /// <summary>
        /// 添加查询参数条件
        /// </summary>
        /// <typeparam name="VT">查询参数对象类型</typeparam>
        /// <param name="value">查询参数对象</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public QueryBuilder<T> AndQueryParameter<VT>(VT value) where VT : class
        {
            QueryParameter.MemberParameter<VT, T>.Set(this, value);
            return this;
        }
        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="member">排序名称</param>
        /// <param name="isAscending">是否升序</param>
        /// <param name="checkMemberName">是否在表格模型中检查排序名称</param>
        /// <returns></returns>
        public QueryBuilder<T> OrderBy(string member, bool isAscending = false, bool checkMemberName = true)
        {
            bool isMemberName = TableWriter.IsColumnName(member);
            if (checkMemberName && !isMemberName) throw new InvalidCastException(TableWriter.TableName + " 没有找到 " + member);
            orderBy(isMemberName ? TableWriter.ConnectionPool.Creator.FormatName(member) : member, isAscending);
            return this;
        }
        /// <summary>
        /// 添加排序
        /// </summary>
        /// <typeparam name="VT">排序数据列类型</typeparam>
        /// <param name="member">排序数据列表达式</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns></returns>
        public QueryBuilder<T> OrderBy<VT>(Expression<Func<T, VT>> member, bool isAscending = false)
        {
            orderBy(TableWriter.ConvertIsSimple(member), isAscending);
            return this;
        }
        /// <summary>
        /// 添加排序
        /// </summary>
        /// <typeparam name="VT">排序子查询表格模型类型</typeparam>
        /// <param name="query">排序子查询</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns></returns>
        public QueryBuilder<T> OrderBy<VT>(QueryBuilder<VT> query, bool isAscending = false)
            where VT : class
        {
            orderBy(query.getOrderByMember(), isAscending);
            return this;
        }
        /// <summary>
        /// 获取 ORDER BY 子查询字符串
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private string getOrderByMember()
        {
            return TableWriter.ConnectionPool.Creator.GetQueryStatement(this, TableWriter.GetMemberMap(MemberMap), 1, 0, true);
        }
        /// <summary>
        /// 清除排序
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public QueryBuilder<T> ClearOrderBy()
        {
            OrderItem.Member = null;
            OrderByItems.SetEmpty();
            return this;
        }

        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <param name="charStream"></param>
        public override void GetStatement(CharStream charStream)
        {
            TableWriter.ConnectionPool.Creator.GetQueryStatement(this, TableWriter.GetMemberMap(MemberMap), 0, 0, charStream);
        }
        /// <summary>
        /// 获取数据库表格模型 SQL 查询信息
        /// </summary>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量</param>
        /// <returns></returns>
        public Query<T> GetQuery(int readCount = 1, int timeoutSeconds = 0, long skipCount = 0)
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return AutoCSer.ORM.Query<T>.Null;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            return GetQueryData(Math.Max(readCount, 0), timeoutSeconds, Math.Max(skipCount, 0));
        }
        /// <summary>
        /// 获取查询 SQL 信息
        /// </summary>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Query<T> GetQueryData(int readCount = 1, int timeoutSeconds = 0, long skipCount = 0)
        {
            MemberMap<T> memberMap = TableWriter.GetMemberMap(MemberMap);
            return new Query<T>(TableWriter.ConnectionPool.Creator.GetQueryStatement(this, memberMap, (uint)readCount, (uint)skipCount, false), memberMap, readCount, timeoutSeconds);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<T?> SingleOrDefault(int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<T> SingleOrDefault(int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return SingleOrDefault<T>(timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<VT?> SingleOrDefault<VT>(int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, T
#else
        public Task<VT> SingleOrDefault<VT>(int timeoutSeconds = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return CompletedTask<VT>.Default;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return TableWriter.SingleOrDefault<VT>(GetQueryData(1, timeoutSeconds, 0), transaction);
        }
        /// <summary>
        /// 判断是否存在表格记录（设置查询列为表格主键）
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<bool> Exists(int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public async Task<bool> Exists(int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            MemberMap = new MemberMap<T>(TableWriter.PrimaryKeyMemberMap);
            return await SingleOrDefault(timeoutSeconds, transaction) != null;
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<T>> Query(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction? transaction = null)
#else
        public Task<LeftArray<T>> Query(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction transaction = null)
#endif
        {
            return Query<T>(readCount, timeoutSeconds, skipCount, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<LeftArray<VT>> Query<VT>(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction? transaction = null) where VT : class, T
#else
        public Task<LeftArray<VT>> Query<VT>(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return EmptyLeftArrayCompletedTask<VT>.EmptyArray;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return TableWriter.Query<VT>(GetQueryData(readCount, timeoutSeconds, skipCount), transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<CT>> Query<CT>(Func<T, CT> getValue, int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction? transaction = null)
#else
        public Task<LeftArray<CT>> Query<CT>(Func<T, CT> getValue, int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction transaction = null)
#endif
        {
            return Query<T, CT>(getValue, readCount, timeoutSeconds, skipCount, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<LeftArray<CT>> Query<VT, CT>(Func<VT, CT> getValue, int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction? transaction = null) where VT : class, T
#else
        public Task<LeftArray<CT>> Query<VT, CT>(Func<VT, CT> getValue, int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return EmptyLeftArrayCompletedTask<CT>.EmptyArray;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return TableWriter.Query<VT, CT>(GetQueryData(readCount, timeoutSeconds, skipCount), getValue, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<SelectEnumerator<T, T>> Select(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction? transaction = null)
#else
        public Task<SelectEnumerator<T, T>> Select(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction transaction = null)
#endif
        {
            return Select<T>(readCount, timeoutSeconds, skipCount, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="skipCount">跳过记录数量，比如用于分页</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<SelectEnumerator<T, VT>> Select<VT>(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction? transaction = null) where VT : class, T
#else
        public Task<SelectEnumerator<T, VT>> Select<VT>(int readCount = 0, int timeoutSeconds = 0, long skipCount = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return Task.FromResult(new SelectEnumerator<T, VT>());
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return TableWriter.Select<VT>(GetQuery(readCount, timeoutSeconds, skipCount), transaction);
        }
        /// <summary>
        /// 获取数据库表格模型 SQL 分页查询信息
        /// </summary>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <returns></returns>
        public Query<T> GetPageQuery(int pageIndex, int pageSize = 10, int timeoutSeconds = 0)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 1;
            return GetQuery(pageSize, timeoutSeconds, (pageIndex - 1) * (long)pageSize);
        }
        /// <summary>
        /// 获取查询 SQL 信息
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="page">分页查询信息</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Query<T> GetQueryPageData<VT>(ref PageResult<VT> page, int timeoutSeconds = 0)
        {
            MemberMap<T> memberMap = TableWriter.GetMemberMap(MemberMap);
            return new Query<T>(TableWriter.ConnectionPool.Creator.GetQueryStatement(this, memberMap, (uint)page.PageSize, (uint)page.SkipCount, false), memberMap, page.PageSize, timeoutSeconds);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<T>> Page(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<LeftArray<T>> Page(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return Page<T>(pageIndex, pageSize, timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<LeftArray<VT>> Page<VT>(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, T
#else
        public Task<LeftArray<VT>> Page<VT>(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return EmptyLeftArrayCompletedTask<VT>.EmptyArray;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 1;
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return TableWriter.Query<VT>(GetQueryData(pageSize, timeoutSeconds, (pageIndex - 1) * (long)pageSize), transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<CT>> Page<CT>(Func<T, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<LeftArray<CT>> Page<CT>(Func<T, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return Page<T, CT>(getValue, pageIndex, pageSize, timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<LeftArray<CT>> Page<VT, CT>(Func<VT, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, T
#else
        public Task<LeftArray<CT>> Page<VT, CT>(Func<VT, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return EmptyLeftArrayCompletedTask<CT>.EmptyArray;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 1;
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return TableWriter.Query<VT, CT>(GetQueryData(pageSize, timeoutSeconds, (pageIndex - 1) * (long)pageSize), getValue, transaction);
        }
        /// <summary>
        /// 查询表格数据数量
        /// </summary>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<long> Count(int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public async Task<long> Count(int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return 0;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            string countStatement = TableWriter.ConnectionPool.Creator.GetQueryStatement(this, ref ExtensionQueryData.Count, 0, false);
            var count = await TableWriter.ConnectionPool.SingleOrDefaultTransaction<ValueResult<long>>(countStatement, timeoutSeconds, transaction);
            return count != null ? count.Value : 0;
        }
        /// <summary>
        /// 调用函数
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="defaultValue"></param>
        /// <param name="transaction"></param>
        /// <param name="method"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<VT?> call<VT>(Expression<Func<T, VT>> member, int timeoutSeconds, VT? defaultValue, Transaction? transaction, string method)
#else
        private async Task<VT> call<VT>(Expression<Func<T, VT>> member, int timeoutSeconds, VT defaultValue, Transaction transaction, string method)
#endif
        {
            ExtensionQueryData queryData = new ExtensionQueryData { QueryNames = new LeftArray<string>(new string[] { $"{method}({TableWriter.Convert(member)}){nameof(ValueResult<VT>.Value)}" }) };
            string countStatement = TableWriter.ConnectionPool.Creator.GetQueryStatement(this, ref queryData, 0, false);
            var value = await TableWriter.ConnectionPool.SingleOrDefaultTransaction<ValueResult<VT>>(countStatement, timeoutSeconds, transaction);
            return value != null ? value.Value : defaultValue;
        }
        /// <summary>
        /// 查询字段求和结果
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member">求和字段</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="defaultValue">查询失败返回的默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Sum<VT>(Expression<Func<T, VT>> member, int timeoutSeconds = 0, VT? defaultValue = default(VT), Transaction? transaction = null)
#else
        public Task<VT> Sum<VT>(Expression<Func<T, VT>> member, int timeoutSeconds = 0, VT defaultValue = default(VT), Transaction transaction = null)
#endif
        {
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return call(member, timeoutSeconds, defaultValue, transaction, "sum");
        }
        /// <summary>
        /// 查询字段最大值
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member">取最大值字段</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="defaultValue">查询失败返回的默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Max<VT>(Expression<Func<T, VT>> member, int timeoutSeconds = 0, VT? defaultValue = default(VT), Transaction? transaction = null)
#else
        public Task<VT> Max<VT>(Expression<Func<T, VT>> member, int timeoutSeconds = 0, VT defaultValue = default(VT), Transaction transaction = null)
#endif
        {
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return call(member, timeoutSeconds, defaultValue, transaction, "max");
        }
        /// <summary>
        /// 查询字段最小值
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member">取最小值字段</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="defaultValue">查询失败返回的默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Min<VT>(Expression<Func<T, VT>> member, int timeoutSeconds = 0, VT? defaultValue = default(VT), Transaction? transaction = null)
#else
        public Task<VT> Min<VT>(Expression<Func<T, VT>> member, int timeoutSeconds = 0, VT defaultValue = default(VT), Transaction transaction = null)
#endif
        {
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            return call(member, timeoutSeconds, defaultValue, transaction, "min");
        }
        /// <summary>
        /// 查询表格分页数据
        /// </summary>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<PageResult<T>> PageResult(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<PageResult<T>> PageResult(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return PageResult<T>(pageIndex, pageSize, timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询表格分页数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<PageResult<VT>> PageResult<VT>(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, T
#else
        public async Task<PageResult<VT>> PageResult<VT>(int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            PageResult<VT> page = new PageResult<VT>(pageIndex, pageSize);
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return page;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            string countStatement = TableWriter.ConnectionPool.Creator.GetQueryStatement(this, ref ExtensionQueryData.Count, 0, false);
            var count = await TableWriter.ConnectionPool.SingleOrDefaultTransaction<ValueResult<long>>(countStatement, timeoutSeconds, transaction);
            if (page.Set(count)) page.Result = await TableWriter.Query<VT>(GetQueryPageData(ref page, timeoutSeconds), transaction);
            return page;
        }
        /// <summary>
        /// 查询表格分页数据
        /// </summary>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<PageResult<CT>> PageResult<CT>(Func<T, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<PageResult<CT>> PageResult<CT>(Func<T, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return PageResult<T, CT>(getValue, pageIndex, pageSize, timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询表格分页数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="pageIndex">分页号，从 1 开始</param>
        /// <param name="pageSize">分页记录数，最小值为 1</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<PageResult<CT>> PageResult<VT, CT>(Func<VT, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, T
#else
        public async Task<PageResult<CT>> PageResult<VT, CT>(Func<VT, CT> getValue, int pageIndex, int pageSize = 10, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, T
#endif
        {
            PageResult<CT> page = new PageResult<CT>(pageIndex, pageSize);
            switch (ConditionLogicType)
            {
                case LogicTypeEnum.False: return page;
                case LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            TableWriter.ConnectionPool.CheckTransaction(ref transaction);
            string countStatement = TableWriter.ConnectionPool.Creator.GetQueryStatement(this, ref ExtensionQueryData.Count, 0, false);
            var count = await TableWriter.ConnectionPool.SingleOrDefaultTransaction<ValueResult<long>>(countStatement, timeoutSeconds, transaction);
            if (page.Set(count)) page.Result = await TableWriter.Query(GetQueryPageData(ref page, timeoutSeconds), getValue, transaction);
            return page;
        }

        /// <summary>
        /// 获取扩展查询 SQL 信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ExtensionQueryBuilder<T> GetExtension()
        {
            return this;
        }
        /// <summary>
        /// 添加 GROUP BY 子项
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <param name="queryName">添加查询名称，默认为 null 表示不添加到查询</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ExtensionQueryBuilder<T> GroupBy<VT>(Expression<Func<T, VT>> member, string? queryName = null)
#else
        public ExtensionQueryBuilder<T> GroupBy<VT>(Expression<Func<T, VT>> member, string queryName = null)
#endif
        {
            return new ExtensionQueryBuilder<T>(this).GroupBy(member, queryName);
        }
    }
}
