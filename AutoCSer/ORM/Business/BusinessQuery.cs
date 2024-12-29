using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 业务表格模型持久化查询
    /// </summary>
    /// <typeparam name="BT">业务表格模型类型</typeparam>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    public sealed class BusinessQuery<BT, T, KT>
        where BT : class, T
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 数据库表格持久化查询
        /// </summary>
        public readonly TableQuery<T, KT> TableQuery;
        /// <summary>
        /// 业务表格模型持久化查询
        /// </summary>
        /// <param name="tableQuery"></param>
        public BusinessQuery(TableQuery<T, KT> tableQuery)
        {
            TableQuery = tableQuery;
        }
        /// <summary>
        /// 创建 SQL 查询创建器
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public QueryBuilder<T> CreateQuery(Expression<Func<T, bool>>? condition = null, bool isTransaction = false)
#else
        public QueryBuilder<T> CreateQuery(Expression<Func<T, bool>> condition = null, bool isTransaction = false)
#endif
        {
            return TableQuery.CreateQuery(condition, isTransaction);
        }
        /// <summary>
        /// 创建 SQL 查询创建器
        /// </summary>
        /// <param name="transaction">事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public QueryBuilder<T> CreateQuery(ref Transaction? transaction, Expression<Func<T, bool>>? condition = null)
#else
        public QueryBuilder<T> CreateQuery(ref Transaction transaction, Expression<Func<T, bool>> condition = null)
#endif
        {
            return TableQuery.CreateQuery(ref transaction, condition);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<BT?> GetByPrimaryKey(KT primaryKey, MemberMap<T>? memberMap = null, Transaction? transaction = null)
#else
        public Task<BT> GetByPrimaryKey(KT primaryKey, MemberMap<T> memberMap = null, Transaction transaction = null)
#endif
        {
            return TableQuery.GetByPrimaryKey<BT>(primaryKey, memberMap, transaction);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> GetByPrimaryKey<VT>(KT primaryKey, MemberMap<T>? memberMap = null, Transaction? transaction = null) where VT : class, BT
#else
        public Task<VT> GetByPrimaryKey<VT>(KT primaryKey, MemberMap<T> memberMap = null, Transaction transaction = null) where VT : class, BT
#endif
        {
            return TableQuery.GetByPrimaryKey<VT>(primaryKey, memberMap, transaction);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<BT?> SingleOrDefault(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<BT> SingleOrDefault(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return CreateQuery(ref transaction, condition).SingleOrDefault<BT>(timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<BT?> SingleOrDefault(Query<T> query, Transaction? transaction = null)
#else
        public Task<BT> SingleOrDefault(Query<T> query, Transaction transaction = null)
#endif
        {
            return TableQuery.SingleOrDefault<BT>(query, transaction);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> SingleOrDefault<VT>(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, BT
#else
        public Task<VT> SingleOrDefault<VT>(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, BT
#endif
        {
            return CreateQuery(ref transaction, condition).SingleOrDefault<VT>(timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> SingleOrDefault<VT>(Query<T> query, Transaction? transaction = null) where VT : class, BT
#else
        public Task<VT> SingleOrDefault<VT>(Query<T> query, Transaction transaction = null) where VT : class, BT
#endif
        {
            return TableQuery.SingleOrDefault<VT>(query, transaction);
        }
        /// <summary>
        /// 判断是否存在表格记录（设置查询列为表格主键）
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Exists(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<bool> Exists(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return CreateQuery(ref transaction, condition).Exists(timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<BT>> Query(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<LeftArray<BT>> Query(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return CreateQuery(ref transaction, condition).Query<BT>(0, timeoutSeconds, 0, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<BT>> Query(Query<T> query, Transaction? transaction = null)
#else
        public Task<LeftArray<BT>> Query(Query<T> query, Transaction transaction = null)
#endif
        {
            return TableQuery.Query<BT>(query, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<VT>> Query<VT>(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, BT
#else
        public Task<LeftArray<VT>> Query<VT>(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, BT
#endif
        {
            return CreateQuery(ref transaction, condition).Query<VT>(0, timeoutSeconds, 0, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<VT>> Query<VT>(Query<T> query, Transaction? transaction = null) where VT : class, BT
#else
        public Task<LeftArray<VT>> Query<VT>(Query<T> query, Transaction transaction = null) where VT : class, BT
#endif
        {
            return TableQuery.Query<VT>(query, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<CT>> QueryCast<CT>(Func<T, CT> getValue, Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<LeftArray<CT>> QueryCast<CT>(Func<T, CT> getValue, Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return CreateQuery(ref transaction, condition).Query<CT>(getValue, 0, timeoutSeconds, 0, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="query"></param>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<CT>> QueryCast<CT>(Query<T> query, Func<T, CT> getValue, Transaction? transaction = null)
#else
        public Task<LeftArray<CT>> QueryCast<CT>(Query<T> query, Func<T, CT> getValue, Transaction transaction = null)
#endif
        {
            return TableQuery.Query<BT, CT>(query, getValue, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<CT>> QueryCast<VT, CT>(Func<VT, CT> getValue, Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, BT
#else
        public Task<LeftArray<CT>> QueryCast<VT, CT>(Func<VT, CT> getValue, Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, BT
#endif
        {
            return CreateQuery(ref transaction, condition).Query<VT, CT>(getValue, 0, timeoutSeconds, 0, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT">转换数据类型</typeparam>
        /// <param name="query"></param>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<CT>> QueryCast<VT, CT>(Query<T> query, Func<VT, CT> getValue, Transaction? transaction = null) where VT : class, BT
#else
        public Task<LeftArray<CT>> QueryCast<VT, CT>(Query<T> query, Func<VT, CT> getValue, Transaction transaction = null) where VT : class, BT
#endif
        {
            return TableQuery.Query<VT, CT>(query, getValue, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<SelectEnumerator<T, BT>> Select(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<SelectEnumerator<T, BT>> Select(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return CreateQuery(ref transaction, condition).Select<BT>(0, timeoutSeconds, 0, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<SelectEnumerator<T, BT>> Select(Query<T> query, Transaction? transaction = null)
#else
        public Task<SelectEnumerator<T, BT>> Select(Query<T> query, Transaction transaction = null)
#endif
        {
            TableQuery.Writer.ConnectionPool.CheckTransaction(ref transaction);
            return TableQuery.Writer.Select<BT>(query, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<SelectEnumerator<T, VT>> Select<VT>(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, BT
#else
        public Task<SelectEnumerator<T, VT>> Select<VT>(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, BT
#endif
        {
            return CreateQuery(ref transaction, condition).Select<VT>(0, timeoutSeconds, 0, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<SelectEnumerator<T, VT>> Select<VT>(Query<T> query, Transaction? transaction = null) where VT : class, BT
#else
        public Task<SelectEnumerator<T, VT>> Select<VT>(Query<T> query, Transaction transaction = null) where VT : class, BT
#endif
        {
            TableQuery.Writer.ConnectionPool.CheckTransaction(ref transaction);
            return TableQuery.Writer.Select<VT>(query, transaction);
        }
        /// <summary>
        /// 查询表格数据数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<long> Count(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<long> Count(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return CreateQuery(ref transaction, condition).Count(timeoutSeconds, transaction);
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
            return CreateQuery(ref transaction).Sum<VT>(member, timeoutSeconds, defaultValue, transaction);
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
            return CreateQuery(ref transaction).Max<VT>(member, timeoutSeconds, defaultValue, transaction);
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
            return CreateQuery(ref transaction).Min<VT>(member, timeoutSeconds, defaultValue, transaction);
        }
        /// <summary>
        /// 获取表格关键字数据字典
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<Dictionary<KT, BT>> GetDictionary(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<Dictionary<KT, BT>> GetDictionary(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            return TableQuery.GetDictionary<BT>(CreateQuery(ref transaction, condition).GetQuery(0, timeoutSeconds, 0), transaction);
        }
        /// <summary>
        /// 获取表格关键字数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<Dictionary<KT, BT>> GetDictionary(Query<T> query, Transaction? transaction = null)
#else
        public Task<Dictionary<KT, BT>> GetDictionary(Query<T> query, Transaction transaction = null)
#endif
        {
            return TableQuery.GetDictionary<BT>(query, transaction);
        }
        /// <summary>
        /// 获取表格关键字数据字典
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<Dictionary<KT, VT>> GetDictionary<VT>(Expression<Func<T, bool>>? condition = null, int timeoutSeconds = 0, Transaction? transaction = null) where VT : class, BT
#else
        public Task<Dictionary<KT, VT>> GetDictionary<VT>(Expression<Func<T, bool>> condition = null, int timeoutSeconds = 0, Transaction transaction = null) where VT : class, BT
#endif
        {
            return TableQuery.GetDictionary<VT>(CreateQuery(ref transaction, condition).GetQuery(0, timeoutSeconds, 0), transaction);
        }
        /// <summary>
        /// 获取表格关键字数据字典
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<Dictionary<KT, VT>> GetDictionary<VT>(Query<T> query, Transaction? transaction = null) where VT : class, BT
#else
        public Task<Dictionary<KT, VT>> GetDictionary<VT>(Query<T> query, Transaction transaction = null) where VT : class, BT
#endif
        {
            return TableQuery.GetDictionary<VT>(query, transaction);
        }
        /// <summary>
        /// 根据数据库关键字创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="reserveCapacity">字典初始预留容器大小</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<DictionaryEventCache<T, BT, KT>> CreateDictionaryCache(Action<Func<Task>> appendQueue, int reserveCapacity = 256, bool isEventAvailable = true)
        {
            return TableQuery.CreateDictionaryCache<BT>(appendQueue, reserveCapacity, isEventAvailable);
        }
        /// <summary>
        /// 根据数据库关键字创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="reserveCapacity">字典初始预留容器大小</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<DictionaryEventCache<T, VT, KT>> CreateDictionaryCache<VT>(Action<Func<Task>> appendQueue, int reserveCapacity = 256, bool isEventAvailable = true)
            where VT : class, BT
        {
            return TableQuery.CreateDictionaryCache<VT>(appendQueue, reserveCapacity, isEventAvailable);
        }
        /// <summary>
        /// 创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="CKT">缓存数据关键字类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="reserveCapacity">字典初始预留容器大小</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<DictionaryEventCache<T, BT, CKT>> CreateDictionaryCache<CKT>(Action<Func<Task>> appendQueue, Func<T, CKT> getKey, int reserveCapacity = 256, bool isEventAvailable = true)
            where CKT : IEquatable<CKT>
        {
            return TableQuery.CreateDictionaryCache<BT, CKT>(appendQueue, getKey, reserveCapacity, isEventAvailable);
        }
        /// <summary>
        /// 创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <typeparam name="CKT">缓存数据关键字类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="reserveCapacity">字典初始预留容器大小</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<DictionaryEventCache<T, VT, CKT>> CreateDictionaryCache<VT, CKT>(Action<Func<Task>> appendQueue, Func<T, CKT> getKey, int reserveCapacity = 256, bool isEventAvailable = true)
            where VT : class, BT
            where CKT : IEquatable<CKT>
        {
            return TableQuery.CreateDictionaryCache<VT, CKT>(appendQueue, getKey, reserveCapacity, isEventAvailable);
        }
        /// <summary>
        /// 根据数据库关键字创建 256 基分片 字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<FragmentDictionaryEventCache<T, BT, KT>> CreateFragmentDictionaryCache(Action<Func<Task>> appendQueue, bool isEventAvailable = true)
        {
            return TableQuery.CreateFragmentDictionaryCache<BT>(appendQueue, isEventAvailable);
        }
        /// <summary>
        /// 根据数据库关键字创建 256 基分片 字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<FragmentDictionaryEventCache<T, VT, KT>> CreateFragmentDictionaryCache<VT>(Action<Func<Task>> appendQueue, bool isEventAvailable = true)
            where VT : class, BT
        {
            return TableQuery.CreateFragmentDictionaryCache<VT>(appendQueue, isEventAvailable);
        }
        /// <summary>
        /// 创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="CKT">缓存数据关键字类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<FragmentDictionaryEventCache<T, BT, CKT>> CreateFragmentDictionaryCache<CKT>(Action<Func<Task>> appendQueue, Func<T, CKT> getKey, bool isEventAvailable = true)
            where CKT : IEquatable<CKT>
        {
            return TableQuery.CreateFragmentDictionaryCache<BT, CKT>(appendQueue, getKey, isEventAvailable);
        }
        /// <summary>
        /// 创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <typeparam name="CKT">缓存数据关键字类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<FragmentDictionaryEventCache<T, VT, CKT>> CreateFragmentDictionaryCache<VT, CKT>(Action<Func<Task>> appendQueue, Func<T, CKT> getKey, bool isEventAvailable = true)
            where VT : class, BT
            where CKT : IEquatable<CKT>
        {
            return TableQuery.CreateFragmentDictionaryCache<VT, CKT>(appendQueue, getKey, isEventAvailable);
        }
        /// <summary>
        /// 根据数据库关键字创建先进先出队列缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <param name="capacity">字典容器大小</param>
        /// <param name="isClear">默认为 true 表示清理容器数据，否则可能会产生临时性的局部内存泄露</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public FifoPriorityQueueCache<T, BT, KT> CreateFifoPriorityQueueCache(int capacity, bool isClear = true)
        {
            return TableQuery.CreateFifoPriorityQueueCache<BT>(capacity, isClear);
        }
        /// <summary>
        /// 根据数据库关键字创建先进先出队列缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <param name="capacity">字典容器大小</param>
        /// <param name="isClear">默认为 true 表示清理容器数据，否则可能会产生临时性的局部内存泄露</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public FifoPriorityQueueCache<T, VT, KT> CreateFifoPriorityQueueCache<VT>(int capacity, bool isClear = true)
            where VT : class, BT
        {
            return TableQuery.CreateFifoPriorityQueueCache<VT>(capacity, isClear);
        }
        /// <summary>
        /// 创建先进先出队列缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="CKT">缓存数据关键字类型</typeparam>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="getValue">从数据库获取数据委托</param>
        /// <param name="capacity">字典容器大小</param>
        /// <param name="isClear">默认为 true 表示清理容器数据，否则可能会产生临时性的局部内存泄露</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public FifoPriorityQueueCache<T, BT, CKT> CreateFifoPriorityQueueCache<CKT>(Func<T, CKT> getKey, Func<CKT, Task<BT?>> getValue, int capacity, bool isClear = true)
#else
        public FifoPriorityQueueCache<T, BT, CKT> CreateFifoPriorityQueueCache<CKT>(Func<T, CKT> getKey, Func<CKT, Task<BT>> getValue, int capacity, bool isClear = true)
#endif
            where CKT : IEquatable<CKT>
        {
            return TableQuery.CreateFifoPriorityQueueCache<BT, CKT>(getKey, getValue, capacity, isClear);
        }
        /// <summary>
        /// 创建先进先出队列缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <typeparam name="CKT">缓存数据关键字类型</typeparam>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="getValue">从数据库获取数据委托</param>
        /// <param name="capacity">字典容器大小</param>
        /// <param name="isClear">默认为 true 表示清理容器数据，否则可能会产生临时性的局部内存泄露</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public FifoPriorityQueueCache<T, VT, CKT> CreateFifoPriorityQueueCache<VT, CKT>(Func<T, CKT> getKey, Func<CKT, Task<VT?>> getValue, int capacity, bool isClear = true)
#else
        public FifoPriorityQueueCache<T, VT, CKT> CreateFifoPriorityQueueCache<VT, CKT>(Func<T, CKT> getKey, Func<CKT, Task<VT>> getValue, int capacity, bool isClear = true)
#endif
            where VT : class, BT
            where CKT : IEquatable<CKT>
        {
            return TableQuery.CreateFifoPriorityQueueCache<VT, CKT>(getKey, getValue, capacity, isClear);
        }
    }
}
