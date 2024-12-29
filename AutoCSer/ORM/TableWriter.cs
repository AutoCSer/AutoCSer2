using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库表格持久化写入
    /// </summary>
    public abstract class TableWriter
    {
        /// <summary>
        /// 默认空属性
        /// </summary>
        internal static readonly ModelAttribute DefaultAttribute = AutoCSer.Configuration.Common.Get<ModelAttribute>()?.Value ?? new ModelAttribute();

        /// <summary>
        /// 数据库连接池
        /// </summary>
        internal readonly ConnectionPool ConnectionPool;
        /// <summary>
        /// 表格名称
        /// </summary>
        internal readonly string TableName;
        /// <summary>
        /// 数据表格模型配置
        /// </summary>
        internal readonly ModelAttribute Attribute;
        /// <summary>
        /// 数据表格模型字段成员集合
        /// </summary>
        internal readonly Member[] Members;
        /// <summary>
        /// 关键字字段成员
        /// </summary>
        internal readonly Member PrimaryKey;
        /// <summary>
        /// 数据库表格字段成员集合
        /// </summary>
        internal readonly CustomColumnName[] Columns;
        /// <summary>
        /// 数据表格模型字段成员集合
        /// </summary>
        internal readonly Dictionary<string, MemberColumnIndex> ColumnNames;
        /// <summary>
        /// 数据列对象集合 临时缓存
        /// </summary>
#if NetStandard21
        private object[]? columnValueCache;
#else
        private object[] columnValueCache;
#endif
        /// <summary>
        /// 主键是否自增ID
        /// </summary>
        internal virtual bool AutoIdentity { get { return false; } }
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <param name="members">数据表格模型字段成员集合</param>
        /// <param name="primaryKey">关键字字段成员</param>
        internal TableWriter(ConnectionPool connectionPool, ModelAttribute attribute, string tableName, Member[] members, Member primaryKey)
        {
            ConnectionPool = connectionPool;
            TableName = tableName;
            Attribute = attribute;
            Members = members;
            PrimaryKey = primaryKey;

            int columnIndexCount = Members.Length, columnCount = 0;
            foreach (Member member in Members)
            {
                if (member.CustomColumnAttribute != null)
                {
                    columnIndexCount += member.CustomColumnNames.Length;
                    --columnCount;
                }
            }
            columnCount += columnIndexCount;
            Columns = new CustomColumnName[columnCount];
            ColumnNames = DictionaryCreator.CreateAny<string, MemberColumnIndex>(columnIndexCount);
            columnCount = 0;
            MemberColumnIndex memberColumnIndex;
            foreach (Member member in Members)
            {
                MemberColumnIndex columnIndex = new MemberColumnIndex(member, -1);
                ColumnNames.Add(member.MemberIndex.Member.Name, columnIndex);
                if (member.CustomColumnAttribute == null) Columns[columnCount++] = new CustomColumnName(member, member.MemberIndex.Member.Name);
                else
                {
                    foreach (CustomColumnName name in member.CustomColumnNames)
                    {
                        ++columnIndex.Index;
                        if (ColumnNames.TryGetValue(name.Name, out memberColumnIndex))
                        {
                            if (!memberColumnIndex.Set(member, columnIndex.Index)) throw new InvalidCastException($"{tableName} 数据列名称 {name.Name} 冲突");
                            ColumnNames[name.Name] = memberColumnIndex;
                        }
                        else ColumnNames.Add(name.Name, columnIndex);
                        Columns[columnCount++] = name;
                    }
                }
            }
        }
        /// <summary>
        /// 获取创建数据库连接
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ConnectionCreator GetConnectionCreator(TableWriter tableWriter)
        {
            return tableWriter.ConnectionPool.Creator;
        }
        /// <summary>
        /// 获取数据列对象集合
        /// </summary>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe object[] GetColumnValueCache(int columnCount)
        {
            return Interlocked.Exchange(ref columnValueCache, null) ?? new object[columnCount];
        }
        /// <summary>
        /// 释放数据列对象集合
        /// </summary>
        /// <param name="columnValueCache"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeColumnValueCache(object[] columnValueCache)
        {
            if (Columns.Length == columnValueCache.Length) Interlocked.Exchange(ref this.columnValueCache, columnValueCache);
        }
        ///// <summary>
        ///// 根据成员名称获取成员
        ///// </summary>
        ///// <param name="memberName"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal Member GetMember(string memberName)
        //{
        //    return ColumnNames.TryGetValue(memberName, out MemberColumnIndex columnIndex) && columnIndex.IsMember ? columnIndex.Member : null;
        //}
        /// <summary>
        /// 根据成员定义获取成员
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
#if NetStandard21
        internal Member? GetMember(MemberInfo memberInfo)
#else
        internal Member GetMember(MemberInfo memberInfo)
#endif
        {
            foreach (Member member in Members)
            {
                if (member.MemberIndex.Member == memberInfo) return member;
            }
            return null;
        }
        /// <summary>
        /// 判断是否存在列名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsColumnName(string name)
        {
            MemberColumnIndex columnIndex;
            return ColumnNames.TryGetValue(name, out columnIndex) && columnIndex.IsColumn;
        }
        ///// <summary>
        ///// 根据数据列名称获取成员
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal Member GetColumnMember(string name)
        //{
        //    return columnIndexs.TryGetValue(name, out MemberColumnIndex columnIndex) && columnIndex.IsColumn ? columnIndex.Member : null;
        //}
        /// <summary>
        /// 数据列名称格式化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal string TryFormatColumnName(string name)
        {
            return IsColumnName(name) ? ConnectionPool.Creator.FormatName(name) : name;
        }
        /// <summary>
        /// 写操作前检查只读状态与数据库事务连接池是否匹配
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void CheckReadOnly(ref Transaction? transaction)
#else
        internal void CheckReadOnly(ref Transaction transaction)
#endif
        {
            if (Attribute.IsReadOnly) throw new InvalidOperationException($"{TableName} 处于只读状态不允许该操作");
            ConnectionPool.CheckTransaction(ref transaction);
        }
    }
    /// <summary>
    /// 数据库表格持久化写入
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public abstract class TableWriter<T> : TableWriter
        where T : class
    {
        /// <summary>
        /// 设置表格模型对象委托
        /// </summary>
        internal readonly Action<DbDataReader, T, MemberMap<T>> Read;
        /// <summary>
        /// 写入表格模型数据委托
        /// </summary>
        internal readonly Action<CharStream, T, TableWriter> InsertValue;
        /// <summary>
        /// 写入更新数据委托
        /// </summary>
        internal readonly Action<CharStream, T, TableWriter, MemberMap<T>> UpdateValue;
        /// <summary>
        /// 写入条件数据委托
        /// </summary>
        internal readonly Action<CharStream, T, TableWriter, MemberMap<T>> ConcatCondition;
        /// <summary>
        /// 复制数据委托
        /// </summary>
#if NetStandard21
        private Action<T, T, MemberMap<T>>? copyTo;
#else
        private Action<T, T, MemberMap<T>> copyTo;
#endif
        /// <summary>
        /// 数据列值转数组
        /// </summary>
#if NetStandard21
        private Action<T, object[]>? toArray;
#else
        private Action<T, object[]> toArray;
#endif
        /// <summary>
        /// 表格模型成员位图
        /// </summary>
        internal readonly MemberMap<T> MemberMap;
        /// <summary>
        /// 关键字成员位图
        /// </summary>
        internal readonly MemberMap<T> PrimaryKeyMemberMap;
        /// <summary>
        /// 可以更新数据的成员位图
        /// </summary>
        internal MemberMapData<T> DefaultUpdateMemberMap;
        /// <summary>
        /// 表格操作事件处理集合
        /// </summary>
        internal LeftArray<ITableEvent<T>> Events;
        /// <summary>
        /// 表格操作事件处理集合 访问锁
        /// </summary>
        private readonly object eventLock = new object();
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <param name="members">数据表格模型字段成员集合</param>
        /// <param name="primaryKey">关键字字段成员</param>
        /// <param name="tableEvent">表格操作事件处理</param>
#if NetStandard21
        internal TableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T>? tableEvent)
#else
        internal TableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T> tableEvent)
#endif
            : base(connectionPool, attribute, attribute.GetTableName(typeof(T)), members, primaryKey)
        {
            MemberMapData<T> memberMap = new MemberMapData<T>();
            DefaultUpdateMemberMap = new MemberMapData<T>();
            foreach (Member member in members)
            {
                memberMap.SetMember(member.MemberIndex.MemberIndex);
                if(member.Attribute.DefaultUpdate && member.Attribute.PrimaryKeyType == PrimaryKeyTypeEnum.None) DefaultUpdateMemberMap.SetMember(member.MemberIndex.MemberIndex);
            }
            MemberMap = new MemberMap<T>(ref memberMap);

            MemberMapData<T> primaryKeyMemberMap = new MemberMapData<T>();
            primaryKeyMemberMap.SetMember(PrimaryKey.MemberIndex.MemberIndex);
            PrimaryKeyMemberMap = new MemberMap<T>(ref primaryKeyMemberMap);

            TableModel<T> tableModel = TableModel<T>.Get(this);
            Read = tableModel.Read;
            InsertValue = tableModel.Insert;
            UpdateValue = tableModel.Update;
            ConcatCondition = tableModel.ConcatCondition;

            Events = new LeftArray<ITableEvent<T>>(0);
            if (tableEvent != null) Events.Add(Events);
        }
        /// <summary>
        /// 获取查询成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal MemberMap<T> GetMemberMap(MemberMap<T>? memberMap)
#else
        internal MemberMap<T> GetMemberMap(MemberMap<T> memberMap)
#endif
        {
            if (memberMap == null) return new MemberMap<T>(MemberMap);
            memberMap.MemberMapData.And(MemberMap.MemberMapData);
            return memberMap;
        }
        /// <summary>
        /// 根据指定成员位图匹配创建表格查询字段成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
#if NetStandard21
        internal MemberMap<T> GetUpdateMemberMap(MemberMap<T>? memberMap)
#else
        internal MemberMap<T> GetUpdateMemberMap(MemberMap<T> memberMap)
#endif
        {
            if (DefaultUpdateMemberMap.IsDefault) throw new ArgumentNullException("该表格缺少指定更新列");
            if (memberMap == null) return new MemberMap<T>(DefaultUpdateMemberMap.Copy());
            memberMap.MemberMapData.And(DefaultUpdateMemberMap);
            if (!memberMap.MemberMapData.IsAnyMember) throw new ArgumentNullException("缺少指定更新列");
            return memberMap;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CopyTo(T source, T destination)
        {
            if (copyTo == null) copyTo = TableModel<T>.GetCopy(this);
            copyTo(source, destination, MemberMap);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="memberMap"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CopyTo(T source, T destination, MemberMap<T> memberMap)
        {
            if (copyTo == null) copyTo = TableModel<T>.GetCopy(this);
            copyTo(source, destination, memberMap);
        }
        /// <summary>
        /// 数据列值转数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal object[] ToArray(T value)
        {
            if (toArray == null) toArray = TableModel<T>.GetToArray(this);
            object[] array = new object[Columns.Length];
            toArray(value, array);
            return array;
        }
        /// <summary>
        /// 创建 SQL 查询创建器
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="isTransaction">是否事务查询，事务查询默认锁为 NONE，否则锁为 NOLOCK</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal QueryBuilder<T> CreateQuery(Expression<Func<T, bool>>? condition, bool isTransaction)
#else
        internal QueryBuilder<T> CreateQuery(Expression<Func<T, bool>> condition, bool isTransaction)
#endif
        {
            return new QueryBuilder<T>(this, condition, isTransaction);
        }
        /// <summary>
        /// 设置新增对象自增ID
        /// </summary>
        /// <param name="value"></param>
        internal virtual void SetInsertAutoIdentity(T value) { }
        /// <summary>
        /// 添加表格操作事件处理对象（表格增删改操作必须在队列中调用）
        /// </summary>
        /// <param name="tableEvent"></param>
        public void AppendEvent(ITableEvent<T> tableEvent)
        {
            if (tableEvent != null)
            {
                Monitor.Enter(eventLock);
                if (getIndexOfEvent(tableEvent) < 0 && !Events.TryAdd(tableEvent))
                {
                    try
                    {
                        Events.Add(tableEvent);
                    }
                    finally { Monitor.Exit(eventLock); }
                }
                else Monitor.Exit(eventLock);
            }
        }
        /// <summary>
        /// 移除表格操作事件处理对象
        /// </summary>
        /// <param name="tableEvent"></param>
        /// <returns></returns>
        public bool RemoveEvent(ITableEvent<T> tableEvent)
        {
            Monitor.Enter(eventLock);
            if (Events.Count != 0)
            {
                int index = getIndexOfEvent(tableEvent);
                if (index >= 0)
                {
                    Events.RemoveAt(index);
                    Monitor.Exit(eventLock);
                    return true;
                }
            }
            Monitor.Exit(eventLock);
            return false;
        }
        /// <summary>
        /// 获取表格操作事件处理对象索引位置
        /// </summary>
        /// <param name="tableEvent"></param>
        /// <returns></returns>
        private int getIndexOfEvent(ITableEvent<T> tableEvent)
        {
            if (Events.Count == 0) return -1;
            int count = Events.Count;
            foreach (ITableEvent<T> matchEvent in Events.Array)
            {
                if (object.ReferenceEquals(matchEvent, tableEvent)) return Events.Count - count;
                if (--count == 0) return -1;
            }
            return -1;
        }
        /// <summary>
        /// 创建表格索引
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="indexNameSuffix">索引名称后缀</param>
        /// <param name="isUnique">是否唯一索引</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <returns>指定的索引名称已经存在则返回 false</returns>
#if NetStandard21
        public Task<bool> CreateIndex(string[] columnNames, string? indexNameSuffix = null, bool isUnique = false, int timeoutSeconds = 0)
#else
        public Task<bool> CreateIndex(string[] columnNames, string indexNameSuffix = null, bool isUnique = false, int timeoutSeconds = 0)
#endif
        {
            CustomColumnName[] columns = new CustomColumnName[columnNames.Length];
            int index = 0;
            MemberColumnIndex columnIndex;
            foreach (string name in columnNames)
            {
                if (!ColumnNames.TryGetValue(name, out columnIndex)) throw new InvalidCastException($"{TableName} 没有找到数据列 {name}");
                columns[index++].Set(columnIndex.Member, name);
            }
            return ConnectionPool.Creator.CreateIndex(this, columns, indexNameSuffix, isUnique, timeoutSeconds);
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<T?> SingleOrDefault(Query<T> query, Transaction? transaction)
#else
        internal Task<T> SingleOrDefault(Query<T> query, Transaction transaction)
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
        internal async Task<VT?> SingleOrDefault<VT>(Query<T> query, Transaction? transaction) where VT : class, T
#else
        internal async Task<VT> SingleOrDefault<VT>(Query<T> query, Transaction transaction) where VT : class, T
#endif
        {
            if (transaction == null)
            {
                if (query.Statement == null) return null;
                var connection = ConnectionPool.GetConnection() ?? await ConnectionPool.CreateConnection();
                try
                {
#if NetStandard21
                    var value = default(VT);
                    await using (DbCommand command = connection.CreateCommand())
#else
                    VT value;
                    using (DbCommand command = connection.CreateCommand())
#endif
                    {
                        query.Set(command);
                        value = await singleOrDefault<VT>(command, query.MemberMap);
                    }
                    await ConnectionPool.FreeConnection(connection);
                    connection = null;
                    return value;
                }
                finally
                {
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
            if (query.Statement == null) return null;
#if NetStandard21
            await using (DbCommand command = transaction.Connection.notNull().CreateCommand())
#else
            using (DbCommand command = transaction.Connection.CreateCommand())
#endif
            {
                transaction.Set(command);
                query.Set(command);
                return await singleOrDefault<VT>(command, query.MemberMap);
            }
        }
        /// <summary>
        /// 查询第一个表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="command"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<VT?> singleOrDefault<VT>(DbCommand command, MemberMap<T> memberMap) where VT : class, T
#else
        private async Task<VT> singleOrDefault<VT>(DbCommand command, MemberMap<T> memberMap) where VT : class, T
#endif
        {
#if NetStandard21
            await using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#else
            using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#endif
            {
                if (await reader.ReadAsync())
                {
                    VT value = DefaultConstructor<VT>.Constructor().notNull();
                    Read(reader, value, memberMap);
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal async Task<LeftArray<VT>> Query<VT>(Query<T> query, Transaction? transaction) where VT : class, T
#else
        internal async Task<LeftArray<VT>> Query<VT>(Query<T> query, Transaction transaction) where VT : class, T
#endif
        {
            if (transaction == null)
            {
                if (query.Statement == null) return new LeftArray<VT>(0);
                var connection = ConnectionPool.GetConnection() ?? await ConnectionPool.CreateConnection();
                try
                {
                    LeftArray<VT> array;
#if NetStandard21
                    await using (DbCommand command = connection.CreateCommand())
#else
                    using (DbCommand command = connection.CreateCommand())
#endif
                    {
                        query.Set(command);
                        array = await this.query<VT>(command, query.MemberMap, query.ReadCount);
                    }
                    await ConnectionPool.FreeConnection(connection);
                    connection = null;
                    return array;
                }
                finally
                {
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
            if (query.Statement == null) return new LeftArray<VT>(0);
#if NetStandard21
            await using (DbCommand command = transaction.Connection.notNull().CreateCommand())
#else
            using (DbCommand command = transaction.Connection.CreateCommand())
#endif
            {
                transaction.Set(command);
                query.Set(command);
                return await this.query<VT>(command, query.MemberMap, query.ReadCount);
            }
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="command"></param>
        /// <param name="memberMap"></param>
        /// <param name="readCount"></param>
        /// <returns></returns>
        private async Task<LeftArray<VT>> query<VT>(DbCommand command, MemberMap<T> memberMap, int readCount) where VT : class, T
        {
            LeftArray<VT> array = new LeftArray<VT>(0);
#if NetStandard21
            await using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#else
            using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#endif
            {
                while (await reader.ReadAsync())
                {
                    VT value = DefaultConstructor<VT>.Constructor().notNull();
                    Read(reader, value, memberMap);
                    if (readCount > 0 && array.Array.Length == 0) array.PrepLength(readCount);
                    array.Add(value);
                }
            }
            return array;
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="query"></param>
        /// <param name="getValue"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal async Task<LeftArray<CT>> Query<VT, CT>(Query<T> query, Func<VT, CT> getValue, Transaction? transaction) where VT : class, T
#else
        internal async Task<LeftArray<CT>> Query<VT, CT>(Query<T> query, Func<VT, CT> getValue, Transaction transaction) where VT : class, T
#endif
        {
            if (transaction == null)
            {
                if (query.Statement == null) return new LeftArray<CT>(0);
                var connection = ConnectionPool.GetConnection() ?? await ConnectionPool.CreateConnection();
                try
                {
                    LeftArray<CT> array;
#if NetStandard21
                    await using (DbCommand command = connection.CreateCommand())
#else
                    using (DbCommand command = connection.CreateCommand())
#endif
                    {
                        query.Set(command);
                        array = await this.query<VT, CT>(command, query.MemberMap, getValue, query.ReadCount);
                    }
                    await ConnectionPool.FreeConnection(connection);
                    connection = null;
                    return array;
                }
                finally
                {
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
            if (query.Statement == null) return new LeftArray<CT>(0);
#if NetStandard21
            await using (DbCommand command = transaction.Connection.notNull().CreateCommand())
#else
            using (DbCommand command = transaction.Connection.CreateCommand())
#endif
            {
                transaction.Set(command);
                query.Set(command);
                return await this.query<VT, CT>(command, query.MemberMap, getValue, query.ReadCount);
            }
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="command"></param>
        /// <param name="memberMap"></param>
        /// <param name="getValue"></param>
        /// <param name="readCount"></param>
        /// <returns></returns>
        private async Task<LeftArray<CT>> query<VT, CT>(DbCommand command, MemberMap<T> memberMap, Func<VT, CT> getValue, int readCount) where VT : class, T
        {
            LeftArray<CT> array = new LeftArray<CT>(0);
#if NetStandard21
            await using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#else
            using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#endif
            {
                while (await reader.ReadAsync())
                {
                    VT value = DefaultConstructor<VT>.Constructor().notNull();
                    Read(reader, value, memberMap);
                    if (readCount > 0 && array.Array.Length == 0) array.PrepLength(readCount);
                    array.Add(getValue(value));
                }
            }
            return array;
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<SelectEnumerator<T, T>> Select(Query<T> query, Transaction? transaction)
#else
        internal Task<SelectEnumerator<T, T>> Select(Query<T> query, Transaction transaction)
#endif
        {
            return Select<T>(query, transaction);
        }
        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal async Task<SelectEnumerator<T, VT>> Select<VT>(Query<T> query, Transaction? transaction) where VT : class, T
#else
        internal async Task<SelectEnumerator<T, VT>> Select<VT>(Query<T> query, Transaction transaction) where VT : class, T
#endif
        {
            if (query.Statement == null) return new SelectEnumerator<T, VT>();
            SelectEnumerator<T, VT> selectEnumerator = new SelectEnumerator<T, VT>(this, query, transaction);
            try
            {
                await selectEnumerator.GetReader();
                return selectEnumerator;
            }
            finally
            {
                if (selectEnumerator.Reader == null) await selectEnumerator.DisposeAsync();
            }
        }
        /// <summary>
        /// 写入添加数据 SQL 语句
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Insert(CharStream charStream, T value)
        {
            InsertValue(charStream, value, this);
        }
        /// <summary>
        /// 成员表达式转换
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        internal string Convert<VT>(Expression<Func<T, VT>> member)
        {
            ExpressionConverter converter = new ExpressionConverter(this);
            try
            {
                converter.ConditionConvert(member.Body);
                return converter.CharStream.ToString();
            }
            finally { converter.FreeCharStream(); }
        }
        /// <summary>
        /// 成员表达式转换
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        internal string ConvertIsSimple<VT>(Expression<Func<T, VT>> member)
        {
            ExpressionConverter converter = new ExpressionConverter(this);
            try
            {
                converter.ConditionConvertIsSimple(member.Body);
                return converter.CharStream.ToString();
            }
            finally { converter.FreeCharStream(); }
        }
        /// <summary>
        /// 根据缓存更新数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="cache"></param>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Update<VT, KT>(ICachePersistence<T, VT, KT> cache, VT value, MemberMap<T>? memberMap = null, bool isClone = true, Transaction? transaction = null)
#else
        public Task<VT> Update<VT, KT>(ICachePersistence<T, VT, KT> cache, VT value, MemberMap<T> memberMap = null, bool isClone = true, Transaction transaction = null)
#endif
            where VT : class, T
            where KT : IEquatable<KT>
        {
            return cache.Update(value, memberMap, isClone, transaction);
        }
        /// <summary>
        /// 根据缓存更新数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="cache"></param>
        /// <param name="value"></param>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Update<VT, KT>(ICachePersistence<T, VT, KT> cache, MemberMapValue<T, VT> value, bool isClone = true, Transaction? transaction = null)
#else
        public Task<VT> Update<VT, KT>(ICachePersistence<T, VT, KT> cache, MemberMapValue<T, VT> value, bool isClone = true, Transaction transaction = null)
#endif
            where VT : class, T
            where KT : IEquatable<KT>
        {
            return cache.Update(value.Value.notNull(), value.MemberMap, isClone, transaction);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="cacheValue"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract Task<bool> Update<VT>(VT value, MemberMap<T>? memberMap, VT cacheValue, Transaction? transaction) where VT : class, T;
#else
        internal abstract Task<bool> Update<VT>(VT value, MemberMap<T> memberMap, VT cacheValue, Transaction transaction) where VT : class, T;
#endif
        /// <summary>
        /// 根据缓存关键字删除数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Delete<VT, KT>(ICachePersistence<T, VT, KT> cache, KT key, Transaction? transaction = null)
#else
        public Task<VT> Delete<VT, KT>(ICachePersistence<T, VT, KT> cache, KT key, Transaction transaction = null)
#endif
            where VT : class, T
            where KT : IEquatable<KT>
        {
            return cache.Delete(key, transaction);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="isEventAvailable"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract Task<bool> Delete<VT>(VT value, bool isEventAvailable, Transaction? transaction) where VT : class, T;
#else
        internal abstract Task<bool> Delete<VT>(VT value, bool isEventAvailable, Transaction transaction) where VT : class, T;
#endif
    }
    /// <summary>
    /// 数据库表格持久化写入
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    public class TableWriter<T, KT> : TableWriter<T>
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 获取关键字委托
        /// </summary>
        internal readonly Func<T, KT> GetPrimaryKey;
        /// <summary>
        /// 设置关键字委托
        /// </summary>
        internal readonly Action<T, KT> SetPrimaryKey;
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <param name="members">数据表格模型字段成员集合</param>
        /// <param name="primaryKey">关键字字段成员</param>
        /// <param name="tableEvent">表格操作事件处理</param>
#if NetStandard21
        internal TableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T>? tableEvent)
#else
        internal TableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T> tableEvent) 
#endif
            : base(connectionPool, attribute, members, primaryKey, tableEvent)
        {
            if (primaryKey.MemberIndex.IsField)
            {
                FieldInfo field = (FieldInfo)primaryKey.MemberIndex.Member;
                GetPrimaryKey = AutoCSer.ORM.Reflection.Emit.Field.UnsafeGetField<T, KT>(field, AutoCSer.Common.NamePrefix + "GetSqlPrimaryKey");
                SetPrimaryKey = AutoCSer.Reflection.Emit.Field.UnsafeSetField<T, KT>(field, AutoCSer.Common.NamePrefix + "SetSqlPrimaryKey");
            }
            else
            {
                PropertyInfo property = (PropertyInfo)primaryKey.MemberIndex.Member;
                GetPrimaryKey = AutoCSer.ORM.Reflection.Emit.Property.UnsafeGetProperty<T, KT>(property, AutoCSer.Common.NamePrefix + "GetSqlPrimaryKey");
                SetPrimaryKey = AutoCSer.ORM.Reflection.Emit.Property.UnsafeSetProperty<T, KT>(property, AutoCSer.Common.NamePrefix + "SetSqlPrimaryKey");
            }
        }
        /// <summary>
        /// 更新自增ID记录
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        internal virtual Task CheckUpdateAutoIdentity(KT primaryKey) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 写入关键字查询条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="primaryKey"></param>
        internal void PrimaryKeyCondition(CharStream charStream, KT primaryKey)
        {
            T value = DefaultConstructor<T>.Constructor().notNull();
            SetPrimaryKey(value, primaryKey);
            ConcatCondition(charStream, value, this, PrimaryKeyMemberMap);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<T?> GetByPrimaryKey(KT primaryKey, MemberMap<T>? memberMap, Transaction? transaction)
#else
        internal Task<T> GetByPrimaryKey(KT primaryKey, MemberMap<T> memberMap, Transaction transaction)
#endif
        {
            return GetByPrimaryKey<T>(primaryKey, memberMap, transaction);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<VT?> GetByPrimaryKey<VT>(KT primaryKey, MemberMap<T>? memberMap, Transaction? transaction) where VT : class, T
#else
        internal Task<VT> GetByPrimaryKey<VT>(KT primaryKey, MemberMap<T> memberMap, Transaction transaction) where VT : class, T
#endif
        {
            Query<T> query = (new QueryBuilder<T>(this, transaction != null, new PrimaryKeyCondition<T, KT>(this, primaryKey), memberMap)).GetQueryData(1);
            return SingleOrDefault<VT>(query, transaction);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<T?> GetByPrimaryKey(KT primaryKey, Transaction? transaction)
#else
        internal Task<T> GetByPrimaryKey(KT primaryKey, Transaction transaction)
#endif
        {
            return GetByPrimaryKey<T>(primaryKey, transaction);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<VT?> GetByPrimaryKey<VT>(KT primaryKey, Transaction? transaction) where VT : class, T
#else
        internal Task<VT> GetByPrimaryKey<VT>(KT primaryKey, Transaction transaction) where VT : class, T
#endif
        {
            Query<T> query = (new QueryBuilder<T>(this, transaction != null, new PrimaryKeyCondition<T, KT>(this, primaryKey))).GetQueryData(1);
            return SingleOrDefault<VT>(query, transaction);
        }
        /// <summary>
        /// 根据关键字查询表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Task<VT?> GetByPrimaryKey<VT>(KT primaryKey) where VT : class, T
#else
        internal Task<VT> GetByPrimaryKey<VT>(KT primaryKey) where VT : class, T
#endif
        {
            return GetByPrimaryKey<VT>(primaryKey, null);
        }
        /// <summary>
        /// 获取表格关键字数据字典
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <returns>没有数据时返回 null</returns>
#if NetStandard21
        internal async Task<Dictionary<KT, VT>?> GetDictionary<VT>(Query<T> query, Transaction? transaction) where VT : class, T
#else
        internal async Task<Dictionary<KT, VT>> GetDictionary<VT>(Query<T> query, Transaction transaction) where VT : class, T
#endif
        {
#if NetStandard21
            var values = default(Dictionary<KT, VT>);
            await using (IAsyncEnumerator<VT> selectEnumerator = await Select<VT>(query, transaction))
            {
                while (await selectEnumerator.MoveNextAsync())
                {
                    VT value = selectEnumerator.Current;
                    if (values == null) values = query.ReadCount == 0 ? DictionaryCreator<KT>.Create<VT>() : DictionaryCreator<KT>.Create<VT>(query.ReadCount);
                    values.Add(GetPrimaryKey(value), value);
                }
            }
#else
            Dictionary<KT, VT> values = null;
            IEnumeratorTask<VT> selectEnumerator = await Select<VT>(query, transaction);
            try
            {
                while (await selectEnumerator.MoveNextAsync())
                {
                    VT value = selectEnumerator.Current;
                    if (values == null) values = query.ReadCount == 0 ? DictionaryCreator<KT>.Create<VT>() : DictionaryCreator<KT>.Create<VT>(query.ReadCount);
                    values.Add(GetPrimaryKey(value), value);
                }
            }
            finally { await selectEnumerator.DisposeAsync(); }
#endif
            return values;
        }

        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Insert<VT>(VT value, Transaction? transaction = null) where VT : class, T
#else
        public Task<bool> Insert<VT>(VT value, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return insert(value, transaction);
        }
        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<bool> insert<VT>(VT value, Transaction? transaction) where VT : class, T
#else
        private async Task<bool> insert<VT>(VT value, Transaction transaction) where VT : class, T
#endif
        {
            if (Events.Length != 0)
            {
                foreach (ITableEvent<T> tableEvent in Events)
                {
                    if (!await tableEvent.BeforeInsert(value)) return false;
                }
            }
            SetInsertAutoIdentity(value);
            string statement = ConnectionPool.Creator.GetInsertStatement(this, value);
            int rowCount = await ConnectionPool.ExecuteNonQueryTransaction(statement, 0, transaction);
            if (rowCount <= 0) return false;
            if (Events.Length == 0) return true;
            bool isGetValue = false;
            try
            {
                var newValue = await GetByPrimaryKey<VT>(GetPrimaryKey(value), transaction);
                if (newValue != null)
                {
                    isGetValue = true;
                    return await TransactionCommited<T>.OnInserted(this, newValue, transaction);
                }
                return transaction == null;
            }
            finally
            {
                if (transaction == null && !isGetValue)
                {
                    await TransactionCommited<T>.OnInserted(this, value, null);
                    await LogHelper.Error($"{TableName} 添加数据以后读取失败 {AutoCSer.JsonSerializer.Serialize((T)value)}");
                }
            }
        }
        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="values"></param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
#if NetStandard21
        public async Task<int> Insert<VT>(IEnumerable<VT> values, Transaction? transaction = null) where VT : class, T
#else
        public async Task<int> Insert<VT>(IEnumerable<VT> values, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            int insertCount = 0;
            bool isAutoTransaction = false;
            try
            {
                foreach (VT value in values)
                {
                    if (transaction == null)
                    {
                        transaction = await ConnectionPool.CreateDefaultTransaction();
                        isAutoTransaction = true;
                    }
                    if (await insert(value, transaction)) ++insertCount;
                    else
                    {
                        if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                        return -1;
                    }
                }
                if (isAutoTransaction)
                {
                    await transaction.notNull().CommitAsync();
                    isAutoTransaction = false;
                }
            }
            finally
            {
                if (isAutoTransaction) await transaction.notNull().DisposeAsync();
            }
            return insertCount;
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Update<VT>(VT value, MemberMap<T>? memberMap = null, Transaction? transaction = null) where VT : class, T
#else
        public Task<bool> Update<VT>(VT value, MemberMap<T> memberMap = null, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return update(value, memberMap, transaction);
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Update<VT>(MemberMapValue<T, VT> value, Transaction? transaction = null) where VT : class, T
#else
        public Task<bool> Update<VT>(MemberMapValue<T, VT> value, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return update(value.Value.notNull(), value.MemberMap, transaction);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
#if NetStandard21
        internal override Task<bool> Update<VT>(VT value, MemberMap<T>? memberMap, VT cacheValue, Transaction? transaction)
#else
        internal override Task<bool> Update<VT>(VT value, MemberMap<T> memberMap, VT cacheValue, Transaction transaction)
#endif
        {
            return update(value, memberMap, transaction);
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<bool> update<VT>(VT value, MemberMap<T>? memberMap, Transaction? transaction) where VT : class, T
#else
        private async Task<bool> update<VT>(VT value, MemberMap<T> memberMap, Transaction transaction) where VT : class, T
#endif
        {
            memberMap = GetUpdateMemberMap(memberMap);
            if (Events.Length != 0)
            {
                foreach (ITableEvent<T> tableEvent in Events)
                {
                    if (!await tableEvent.BeforeUpdate(value, memberMap)) return false;
                }
            }
            KT primaryKey = GetPrimaryKey(value);
            QueryBuilder<T> query = new QueryBuilder<T>(this, transaction != null, new PrimaryKeyCondition<T, KT>(this, primaryKey));
            string statement = ConnectionPool.Creator.GetUpdateStatement(query, value, memberMap);
            int rowCount = await ConnectionPool.ExecuteNonQueryTransaction(statement, 0, transaction);
            if (rowCount <= 0) return false;
            if (Events.Length == 0) return true;
            bool isGetValue = false;
            try
            {
                var newValue = await SingleOrDefault<VT>(query.GetQueryData(1), transaction);
                if (newValue != null)
                {
                    isGetValue = true;
                    return await TransactionCommited<T>.OnUpdated(this, newValue, memberMap, transaction);
                }
                return transaction == null;
            }
            finally
            {
                if (transaction == null && !isGetValue)
                {
                    await TransactionCommited<T>.OnUpdated(this, value, memberMap, null);
                    await LogHelper.Error($"{TableName} 更新数据以后读取失败 {AutoCSer.JsonSerializer.Serialize((T)value)}");
                }
            }
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="values"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
#if NetStandard21
        public async Task<int> Update<VT>(IEnumerable<VT> values, MemberMap<T>? memberMap = null, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public async Task<int> Update<VT>(IEnumerable<VT> values, MemberMap<T> memberMap = null, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            int updateCount = 0;
            bool isAutoTransaction = false;
            try
            {
                foreach (VT value in values)
                {
                    if (transaction == null && !ignoreFail)
                    {
                        transaction = await ConnectionPool.CreateDefaultTransaction();
                        isAutoTransaction = true;
                    }
                    if (await update(value, memberMap, transaction)) ++updateCount;
                    else if (!ignoreFail)
                    {
                        if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                        return -1;
                    }
                }
                if (isAutoTransaction)
                {
                    await transaction.notNull().CommitAsync();
                    isAutoTransaction = false;
                }
            }
            finally
            {
                if (isAutoTransaction) await transaction.notNull().DisposeAsync();
            }
            return updateCount;
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="condition"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(VT value, MemberMap<T>? memberMap, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public Task<int> Update<VT>(VT value, MemberMap<T> memberMap, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return update(value, memberMap, new QueryBuilder<T>(this, condition, transaction != null), timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return update(value.Value.notNull(), value.MemberMap, new QueryBuilder<T>(this, condition, transaction != null), timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(VT value, MemberMap<T>? memberMap, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public Task<int> Update<VT>(VT value, MemberMap<T> memberMap, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return update(value, memberMap, query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return update(value.Value.notNull(), value.MemberMap, query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
#if NetStandard21
        private async Task<int> update<VT>(VT value, MemberMap<T>? memberMap, QueryBuilder<T> query, int timeoutSeconds, bool ignoreFail, Transaction? transaction) where VT : class, T
#else
        private async Task<int> update<VT>(VT value, MemberMap<T> memberMap, QueryBuilder<T> query, int timeoutSeconds, bool ignoreFail, Transaction transaction) where VT : class, T
#endif
        {
            switch (query.ConditionLogicType)
            {
                case ConditionExpression.LogicTypeEnum.False: return 0;
                case ConditionExpression.LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            if (!query.IsCondition) throw new Exception("缺少更新条件");
            memberMap = GetUpdateMemberMap(memberMap);
            if (Events.Length == 0) return await ConnectionPool.ExecuteNonQueryTransaction(ConnectionPool.Creator.GetUpdateStatement(query, value, memberMap), timeoutSeconds, transaction);
            int updateCount = 0;
            bool isAutoTransaction = false;
            try
            {
                query.MemberMap = new MemberMap<T>(PrimaryKeyMemberMap);
#if NetStandard21
                await using (IAsyncEnumerator<VT> selectEnumerator = await query.Select<VT>())
                {
                    while (await selectEnumerator.MoveNextAsync())
                    {
                        VT updateValue = selectEnumerator.Current;
                        CopyTo(value, updateValue, memberMap);
                        if (transaction == null && !ignoreFail)
                        {
                            transaction = await ConnectionPool.CreateTransaction(isAsyncLocal: false);
                            isAutoTransaction = true;
                        }
                        if (await update(updateValue, memberMap, transaction)) ++updateCount;
                        else if (!ignoreFail)
                        {
                            if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                            return -1;
                        }
                    }
                }
#else
                IEnumeratorTask<VT> selectEnumerator = await query.Select<VT>();
                try
                {
                    while (await selectEnumerator.MoveNextAsync())
                    {
                        VT updateValue = selectEnumerator.Current;
                        CopyTo(value, updateValue, memberMap);
                        if (transaction == null && !ignoreFail)
                        {
                            transaction = await ConnectionPool.CreateDefaultTransaction();
                            isAutoTransaction = true;
                        }
                        if (await update(updateValue, memberMap, transaction)) ++updateCount;
                        else if (!ignoreFail)
                        {
                            if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                            return -1;
                        }
                    }
                }
                finally { await selectEnumerator.DisposeAsync(); }
#endif
                if (isAutoTransaction)
                {
                    await transaction.notNull().CommitAsync();
                    isAutoTransaction = false;
                }
            }
            finally
            {
                if (isAutoTransaction) await transaction.notNull().DisposeAsync();
            }
            return updateCount;
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Delete(KT primaryKey, Transaction? transaction = null)
#else
        public Task<bool> Delete(KT primaryKey, Transaction transaction = null)
#endif
        {
            CheckReadOnly(ref transaction);
            return delete<T>(primaryKey, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Delete<VT>(KT primaryKey, Transaction? transaction = null) where VT : class, T
#else
        public Task<bool> Delete<VT>(KT primaryKey, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return delete<VT>(primaryKey, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<bool> delete<VT>(KT primaryKey, Transaction? transaction) where VT : class, T
#else
        private async Task<bool> delete<VT>(KT primaryKey, Transaction transaction) where VT : class, T
#endif
        {
            var value = default(VT);
            if (Events.Length != 0) value = await GetByPrimaryKey<VT>(primaryKey, transaction);
            return await delete(value, primaryKey, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Delete<VT>(VT value, Transaction? transaction = null) where VT : class, T
#else
        public Task<bool> Delete<VT>(VT value, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return delete(value, false, transaction);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <param name="isEventAvailable"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal override Task<bool> Delete<VT>(VT value, bool isEventAvailable, Transaction? transaction)
#else
        internal override Task<bool> Delete<VT>(VT value, bool isEventAvailable, Transaction transaction)
#endif
        {
            CheckReadOnly(ref transaction);
            return delete(value, isEventAvailable, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="isEventAvailable">传参对象是否事件可用</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<bool> delete<VT>(VT value, bool isEventAvailable, Transaction? transaction) where VT : class, T
#else
        private async Task<bool> delete<VT>(VT value, bool isEventAvailable, Transaction transaction) where VT : class, T
#endif
        {
            KT primaryKey = GetPrimaryKey(value);
            if (Events.Length != 0 && !isEventAvailable)
            {
                var getValue = await GetByPrimaryKey<VT>(primaryKey, transaction);
                if (getValue == null) return false;
                value = getValue;
            }
            return await delete(value, primaryKey, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task<bool> delete<VT>(VT? value, KT primaryKey, Transaction? transaction) where VT : class, T
#else
        private async Task<bool> delete<VT>(VT value, KT primaryKey, Transaction transaction) where VT : class, T
#endif
        {
            if (Events.Length != 0)
            {
                if (value == null) return false;
                foreach (ITableEvent<T> tableEvent in Events)
                {
                    if (!await tableEvent.BeforeDelete(value)) return false;
                }
            }
            string statement = ConnectionPool.Creator.GetDeleteStatement(new QueryBuilder<T>(this, transaction != null, new PrimaryKeyCondition<T, KT>(this, primaryKey)));
            await CheckUpdateAutoIdentity(primaryKey);
            int rowCount = await ConnectionPool.ExecuteNonQueryTransaction(statement, 0, transaction);
            if (rowCount <= 0) return false;
            return await TransactionCommited<T>.OnDeleted(this, value, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量，失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction? transaction = null)
#else
        public Task<int> Delete(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction transaction = null)
#endif
        {
            return Delete<T>(primaryKeys, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKeys"></param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量，失败返回 -1</returns>
#if NetStandard21
        public async Task<int> Delete<VT>(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public async Task<int> Delete<VT>(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            int deleteCount = 0;
            bool isAutoTransaction = false;
            try
            {
                foreach (KT primaryKey in primaryKeys)
                {
                    if (transaction == null && !ignoreFail)
                    {
                        transaction = await ConnectionPool.CreateDefaultTransaction();
                        isAutoTransaction = true;
                    }
                    if (await delete<VT>(primaryKey, transaction)) ++deleteCount;
                    else if (!ignoreFail)
                    {
                        if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                        return -1;
                    }
                }
                if (isAutoTransaction)
                {
                    await transaction.notNull().CommitAsync();
                    isAutoTransaction = false;
                }
            }
            finally
            {
                if (isAutoTransaction) await transaction.notNull().DisposeAsync();
            }
            return deleteCount;
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="values"></param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量，失败返回 -1</returns>
#if NetStandard21
        public async Task<int> Delete<VT>(IEnumerable<VT> values, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public async Task<int> Delete<VT>(IEnumerable<VT> values, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            int deleteCount = 0;
            bool isAutoTransaction = false;
            try
            {
                foreach (VT value in values)
                {
                    if (transaction == null && !ignoreFail)
                    {
                        transaction = await ConnectionPool.CreateDefaultTransaction();
                        isAutoTransaction = true;
                    }
                    if (await delete(value, false, transaction)) ++deleteCount;
                    else if (!ignoreFail)
                    {
                        if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                        return -1;
                    }
                }
                if (isAutoTransaction)
                {
                    await transaction.notNull().CommitAsync();
                    isAutoTransaction = false;
                }
            }
            finally
            {
                if (isAutoTransaction) await transaction.notNull().DisposeAsync();
            }
            return deleteCount;
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null)
#else
        public Task<int> Delete(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null)
#endif
        {
            CheckReadOnly(ref transaction);
            return delete<T>(new QueryBuilder<T>(this, condition, transaction != null), timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete<VT>(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public Task<int> Delete<VT>(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return delete<VT>(new QueryBuilder<T>(this, condition, transaction != null), timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null)
#else
        public Task<int> Delete(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null)
#endif
        {
            CheckReadOnly(ref transaction);
            return delete<T>(query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete<VT>(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, T
#else
        public Task<int> Delete<VT>(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, T
#endif
        {
            CheckReadOnly(ref transaction);
            return delete<VT>(query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
#if NetStandard21
        private async Task<int> delete<VT>(QueryBuilder<T> query, int timeoutSeconds, bool ignoreFail, Transaction? transaction) where VT : class, T
#else
        private async Task<int> delete<VT>(QueryBuilder<T> query, int timeoutSeconds, bool ignoreFail, Transaction transaction) where VT : class, T
#endif
        {
            switch (query.ConditionLogicType)
            {
                case ConditionExpression.LogicTypeEnum.False: return 0;
                case ConditionExpression.LogicTypeEnum.NotSupport: throw new InvalidCastException("不支持的条件表达式");
            }
            if (!query.IsCondition) throw new Exception("缺少删除条件");
            if (Events.Length == 0 && !AutoIdentity) return await ConnectionPool.ExecuteNonQueryTransaction(ConnectionPool.Creator.GetDeleteStatement(query), timeoutSeconds, transaction);
            int deleteCount = 0;
            bool isAutoTransaction = false;
            try
            {
                query.MemberMap = new MemberMap<T>(PrimaryKeyMemberMap);
#if NetStandard21
                await using (IAsyncEnumerator<VT> selectEnumerator = await query.Select<VT>())
                {
                    while(await selectEnumerator.MoveNextAsync())
                    {
                        VT value = selectEnumerator.Current;
                        if (transaction == null && !ignoreFail)
                        {
                            transaction = await ConnectionPool.CreateTransaction(isAsyncLocal : false);
                            isAutoTransaction = true;
                        }
                        if (await delete(value, false, transaction)) ++deleteCount;
                        else if (!ignoreFail)
                        {
                            if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                            return -1;
                        }
                    }
                }
#else
                IEnumeratorTask<VT> selectEnumerator = await query.Select<VT>();
                try
                {
                    while (await selectEnumerator.MoveNextAsync())
                    {
                        VT value = selectEnumerator.Current;
                        if (transaction == null && !ignoreFail)
                        {
                            transaction = await ConnectionPool.CreateDefaultTransaction();
                            isAutoTransaction = true;
                        }
                        if (await delete(value, false, transaction)) ++deleteCount;
                        else if (!ignoreFail)
                        {
                            if (!isAutoTransaction && transaction != null) await transaction.DisposeAsync();
                            return -1;
                        }
                    }
                }
                finally { await selectEnumerator.DisposeAsync(); }
#endif
                if (isAutoTransaction)
                {
                    await transaction.notNull().CommitAsync();
                    isAutoTransaction = false;
                }
            }
            finally
            {
                if (isAutoTransaction) await transaction.notNull().DisposeAsync();
            }
            return deleteCount;
        }
    }
}
