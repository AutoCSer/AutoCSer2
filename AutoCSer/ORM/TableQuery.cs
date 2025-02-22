using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库表格持久化查询
    /// </summary>
    public abstract class TableQuery
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        internal readonly TableWriter Writer;
        /// <summary>
        /// 数据库连接池
        /// </summary>
        public ConnectionPool ConnectionPool { get { return Writer.ConnectionPool; } }
        /// <summary>
        /// 表格名称
        /// </summary>
        public string TableName { get { return Writer.TableName; } }
        /// <summary>
        /// 数据库表格持久化查询
        /// </summary>
        /// <param name="writer">数据库表格持久化写入</param>
        internal TableQuery(TableWriter writer)
        {
            Writer = writer;
        }
    }
    /// <summary>
    /// 数据库表格持久化查询
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public abstract class TableQuery<T> : TableQuery
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        internal readonly new TableWriter<T> Writer;
        /// <summary>
        /// 数据库表格持久化查询
        /// </summary>
        /// <param name="writer">数据库表格持久化写入</param>
        internal TableQuery(TableWriter<T> writer) : base(writer)
        {
            Writer = writer;
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
            return Writer.CreateQuery(condition, isTransaction);
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
            ConnectionPool.CheckTransaction(ref transaction);
            return Writer.CreateQuery(condition, transaction != null);
        }
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <typeparam name="RT">被关联表格模型类型</typeparam>
        /// <typeparam name="KT">关联关键字类型</typeparam>
        /// <param name="getLeftKey">获取关联表格关键字</param>
        /// <param name="rightTable">被关联表格</param>
        /// <param name="setQuery">设置关联查询条件以后的附加查询设置委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT, KT>(Expression<Func<T, KT>> getLeftKey, TableWriter<RT, KT> rightTable, Action<QueryBuilder<RT>>? setQuery = null)
#else
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT, KT>(Expression<Func<T, KT>> getLeftKey, TableWriter<RT, KT> rightTable, Action<QueryBuilder<RT>> setQuery = null)
#endif
            where RT : class
            where KT : IEquatable<KT>
        {
            return new AssociatedTable<T, RT, KT>(Writer, getLeftKey, rightTable, setQuery);
        }
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <typeparam name="RT">被关联表格模型类型</typeparam>
        /// <typeparam name="KT">关联关键字类型</typeparam>
        /// <param name="getLeftKey">获取关联表格关键字</param>
        /// <param name="rightTable">被关联表格</param>
        /// <param name="getRightKey">获取被关联表格关键字</param>
        /// <param name="setQuery">设置关联查询条件以后的附加查询设置委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT, KT>(Expression<Func<T, KT>> getLeftKey, TableWriter<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>>? setQuery = null)
#else
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT, KT>(Expression<Func<T, KT>> getLeftKey, TableWriter<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>> setQuery = null)
#endif
            where RT : class
            where KT : IEquatable<KT>
        {
            return new AssociatedTable<T, RT, KT>(Writer, getLeftKey, rightTable, getRightKey, setQuery);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<T?> SingleOrDefault(Query<T> query, Transaction? transaction = null)
#else
        public Task<T> SingleOrDefault(Query<T> query, Transaction transaction = null)
#endif
        {
            return SingleOrDefault<T>(query, transaction);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<VT?> SingleOrDefault<VT>(Query<T> query, Transaction? transaction = null) where VT : class, T
#else
        public Task<VT> SingleOrDefault<VT>(Query<T> query, Transaction transaction = null) where VT : class, T
#endif
        {
            if (query != null)
            {
                ConnectionPool.CheckTransaction(ref transaction);
                return Writer.SingleOrDefault<VT>(query, transaction);
            }
            throw new ArgumentNullException();
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<LeftArray<T>> Query(Query<T> query, Transaction? transaction = null)
#else
        public Task<LeftArray<T>> Query(Query<T> query, Transaction transaction = null)
#endif
        {
            return Query<T>(query, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<LeftArray<VT>> Query<VT>(Query<T> query, Transaction? transaction = null) where VT : class, T
#else
        public Task<LeftArray<VT>> Query<VT>(Query<T> query, Transaction transaction = null) where VT : class, T
#endif
        {
            if (query != null)
            {
                ConnectionPool.CheckTransaction(ref transaction);
                return Writer.Query<VT>(query, transaction);
            }
            throw new ArgumentNullException();
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
        public Task<LeftArray<CT>> Query<CT>(Query<T> query, Func<T, CT> getValue, Transaction? transaction = null)
#else
        public Task<LeftArray<CT>> Query<CT>(Query<T> query, Func<T, CT> getValue, Transaction transaction = null)
#endif
        {
            return Query<T, CT>(query, getValue, transaction);
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
#if NetStandard21
        public Task<LeftArray<CT>> Query<VT, CT>(Query<T> query, Func<VT, CT> getValue, Transaction? transaction = null) where VT : class, T
#else
        public Task<LeftArray<CT>> Query<VT, CT>(Query<T> query, Func<VT, CT> getValue, Transaction transaction = null) where VT : class, T
#endif
        {
            if (query != null)
            {
                ConnectionPool.CheckTransaction(ref transaction);
                return Writer.Query(query, getValue, transaction);
            }
            throw new ArgumentNullException();
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<SelectEnumerator<T, T>> Select(Query<T> query, Transaction? transaction = null)
#else
        public Task<SelectEnumerator<T, T>> Select(Query<T> query, Transaction transaction = null)
#endif
        {
            if (query != null)
            {
                ConnectionPool.CheckTransaction(ref transaction);
                return Writer.Select(query, transaction);
            }
            throw new ArgumentNullException();
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Task<SelectEnumerator<T, VT>> Select<VT>(Query<T> query, Transaction? transaction = null) where VT : class, T
#else
        public Task<SelectEnumerator<T, VT>> Select<VT>(Query<T> query, Transaction transaction = null) where VT : class, T
#endif
        {
            if (query != null)
            {
                ConnectionPool.CheckTransaction(ref transaction);
                return Writer.Select<VT>(query, transaction);
            }
            throw new ArgumentNullException();
        }
        /// <summary>
        /// 创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <typeparam name="KT">缓存数据关键字类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="reserveCapacity">字典初始预留容器大小</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        public async Task<DictionaryEventCache<T, VT, KT>> CreateDictionaryCache<VT, KT>(Action<Func<Task>> appendQueue, Func<T, KT> getKey, int reserveCapacity = 256, bool isEventAvailable = true)
            where VT : class, T
            where KT : IEquatable<KT>
        {
            DictionaryEventCache<T, VT, KT> cache = new DictionaryEventCache<T, VT, KT>(Writer, isEventAvailable, appendQueue, (int)Math.Min(await CreateQuery(null, false).Count() + reserveCapacity, int.MaxValue), getKey);
            await cache.Initialize();
            return cache;
        }
        /// <summary>
        /// 创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <typeparam name="KT">缓存数据关键字类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        public async Task<FragmentDictionaryEventCache<T, VT, KT>> CreateFragmentDictionaryCache<VT, KT>(Action<Func<Task>> appendQueue, Func<T, KT> getKey, bool isEventAvailable = true)
            where VT : class, T
            where KT : IEquatable<KT>
        {
            FragmentDictionaryEventCache<T, VT, KT> cache = new FragmentDictionaryEventCache<T, VT, KT>(Writer, isEventAvailable, appendQueue, getKey);
            await cache.Initialize();
            return cache;
        }
        /// <summary>
        /// 创建先进先出队列缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <typeparam name="KT">缓存数据关键字类型</typeparam>
        /// <param name="getKey">获取缓存数据关键字委托</param>
        /// <param name="getValue">从数据库获取数据委托</param>
        /// <param name="capacity">字典容器大小</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public FifoPriorityQueueCache<T, VT, KT> CreateFifoPriorityQueueCache<VT, KT>(Func<T, KT> getKey, Func<KT, Task<VT?>> getValue, int capacity)
#else
        public FifoPriorityQueueCache<T, VT, KT> CreateFifoPriorityQueueCache<VT, KT>(Func<T, KT> getKey, Func<KT, Task<VT>> getValue, int capacity)
#endif
            where VT : class, T
            where KT : IEquatable<KT>
        {
            return new FifoPriorityQueueCache<T, VT, KT>(Writer, capacity, getKey, getValue);
        }
    }
    /// <summary>
    /// 数据库表格持久化查询
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    public class TableQuery<T, KT> : TableQuery<T>
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        internal readonly new TableWriter<T, KT> Writer;
        /// <summary>
        /// 数据库表格持久化查询
        /// </summary>
        /// <param name="writer">数据库表格持久化写入</param>
        internal TableQuery(TableWriter<T, KT> writer) : base(writer)
        {
            Writer = writer;
        }
        /// /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <typeparam name="RT">被关联表格模型类型</typeparam>
        /// <param name="rightTable">被关联表格</param>
        /// <param name="setQuery">设置关联查询条件以后的附加查询设置委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT>(TableQuery<RT, KT> rightTable, Action<QueryBuilder<RT>>? setQuery = null)
#else
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT>(TableQuery<RT, KT> rightTable, Action<QueryBuilder<RT>> setQuery = null)
#endif
            where RT : class
        {
            return new AssociatedTable<T, RT, KT>(Writer, rightTable.Writer, setQuery);
        }
        /// <summary>
        /// 模拟关联表格
        /// </summary>
        /// <typeparam name="RT">被关联表格模型类型</typeparam>
        /// <param name="rightTable">被关联表格</param>
        /// <param name="getRightKey">获取被关联表格关键字</param>
        /// <param name="setQuery">设置关联查询条件以后的附加查询设置委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT>(TableQuery<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>>? setQuery = null)
#else
        public AssociatedTable<T, RT, KT> GetAssociatedTable<RT>(TableQuery<RT> rightTable, Expression<Func<RT, KT>> getRightKey, Action<QueryBuilder<RT>> setQuery = null)
#endif
            where RT : class
        {
            return new AssociatedTable<T, RT, KT>(Writer, rightTable.Writer, getRightKey, setQuery);
        }
        /// <summary>
        /// 根据关键字获取数据库表格模型 SQL 查询信息
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public Query<T> GetQueryByPrimaryKey(KT primaryKey, MemberMap<T>? memberMap = null, Transaction? transaction = null)
#else
        public Query<T> GetQueryByPrimaryKey(KT primaryKey, MemberMap<T> memberMap = null, Transaction transaction = null)
#endif
        {
            ConnectionPool.CheckTransaction(ref transaction);
            return new QueryBuilder<T>(Writer, transaction != null, new PrimaryKeyCondition<T, KT>(Writer, primaryKey), memberMap).GetQuery(1);
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
        public Task<T?> GetByPrimaryKey(KT primaryKey, MemberMap<T>? memberMap = null, Transaction? transaction = null)
#else
        public Task<T> GetByPrimaryKey(KT primaryKey, MemberMap<T> memberMap = null, Transaction transaction = null)
#endif
        {
            ConnectionPool.CheckTransaction(ref transaction);
            return Writer.GetByPrimaryKey(primaryKey, memberMap, transaction);
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
        public Task<VT?> GetByPrimaryKey<VT>(KT primaryKey, MemberMap<T>? memberMap = null, Transaction? transaction = null) where VT : class, T
#else
        public Task<VT> GetByPrimaryKey<VT>(KT primaryKey, MemberMap<T> memberMap = null, Transaction transaction = null) where VT : class, T
#endif
        {
            ConnectionPool.CheckTransaction(ref transaction);
            return Writer.GetByPrimaryKey<VT>(primaryKey, memberMap, transaction);
        }
        /// <summary>
        /// 获取表格关键字数据字典
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<Dictionary<KT, VT>> GetDictionary<VT>(Query<T> query, Transaction? transaction = null) where VT : class, T
#else
        public async Task<Dictionary<KT, VT>> GetDictionary<VT>(Query<T> query, Transaction transaction = null) where VT : class, T
#endif
        {
            if (query != null)
            {
                ConnectionPool.CheckTransaction(ref transaction);
                return await Writer.GetDictionary<VT>(query, transaction) ?? DictionaryCreator<KT>.Create<VT>();
            }
            throw new ArgumentNullException();
        }
        /// <summary>
        /// 根据数据库关键字创建字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="reserveCapacity">字典初始预留容器大小</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        public async Task<DictionaryEventCache<T, VT, KT>> CreateDictionaryCache<VT>(Action<Func<Task>> appendQueue, int reserveCapacity = 256, bool isEventAvailable = true)
            where VT : class, T
        {
            DictionaryEventCache<T, VT, KT> cache = new DictionaryEventCache<T, VT, KT>(Writer, isEventAvailable, appendQueue, (int)Math.Min(await CreateQuery(null, false).Count() + reserveCapacity, int.MaxValue));
            await cache.Initialize();
            return cache;
        }
        /// <summary>
        /// 根据数据库关键字创建 256 基分片 字典事件缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <param name="appendQueue">添加队列任务</param>
        /// <param name="isEventAvailable">默认为 true 缓存对象事件可用</param>
        /// <returns></returns>
        public async Task<FragmentDictionaryEventCache<T, VT, KT>> CreateFragmentDictionaryCache<VT>(Action<Func<Task>> appendQueue, bool isEventAvailable = true)
            where VT : class, T
        {
            FragmentDictionaryEventCache<T, VT, KT> cache = new FragmentDictionaryEventCache<T, VT, KT>(Writer, isEventAvailable, appendQueue);
            await cache.Initialize();
            return cache;
        }
        /// <summary>
        /// 根据数据库关键字创建先进先出队列缓存（缓存操作与表格增删改操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT">缓存数据类型</typeparam>
        /// <param name="capacity">字典容器大小</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public FifoPriorityQueueCache<T, VT, KT> CreateFifoPriorityQueueCache<VT>(int capacity)
            where VT : class, T
        {
            return new FifoPriorityQueueCache<T, VT, KT>(Writer, capacity);
        }
    }
}
