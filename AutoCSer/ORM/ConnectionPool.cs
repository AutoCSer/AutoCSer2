using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库连接池
    /// </summary>
    public sealed class ConnectionPool
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        internal readonly ConnectionCreator Creator;
        /// <summary>
        /// 数据库连接集合
        /// </summary>
        private LeftArray<DbConnection> connections;
        /// <summary>
        /// 数据库连接集合访问锁
        /// </summary>
        private readonly object connectionLock = new object();
        /// <summary>
        /// 数据库连接池
        /// </summary>
        /// <param name="creator">创建数据库连接</param>
        private ConnectionPool(ConnectionCreator creator)
        {
            this.Creator = creator;
            connections = new LeftArray<DbConnection>(0);
        }
        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal async Task FreeConnection(DbConnection connection)
        {
            Monitor.Enter(connectionLock);
            if (connections.TryAdd(connection))
            {
                Monitor.Exit(connectionLock);
                return;
            }
            bool isCloseConnection = true;
            try
            {
                connections.Add(connection);
                isCloseConnection = false;
            }
            finally
            {
                Monitor.Exit(connectionLock);
                if (isCloseConnection) await ConnectionCreator.CloseConnectionAsync(connection);
            }
        }
        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        internal void FreeConnection(Transaction transaction)
        {
            Monitor.Enter(connectionLock);
            var connection = transaction.ClearConnection();
            if (connection != null && !connections.TryAdd(connection))
            {
                try
                {
                    connections.Add(connection);
                    connection = null;
                }
                finally
                {
                    Monitor.Exit(connectionLock);
                    ConnectionCreator.CloseConnection(connection);
                }
            }
            else Monitor.Exit(connectionLock);
        }
        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        internal async Task FreeConnectionAsync(Transaction transaction)
        {
            Monitor.Enter(connectionLock);
            var connection = transaction.ClearConnection();
            if (connection != null && !connections.TryAdd(connection))
            {
                try
                {
                    connections.Add(connection);
                    connection = null;
                }
                finally
                {
                    Monitor.Exit(connectionLock);
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
            else Monitor.Exit(connectionLock);
        }
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal DbConnection? GetConnection()
#else
        internal DbConnection GetConnection()
#endif
        {
            var connection = default(DbConnection);
            Monitor.Enter(connectionLock);
            connections.TryPop(out connection);
            Monitor.Exit(connectionLock);
            return connection;
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task<DbConnection> CreateConnection()
        {
            return Creator.CreateConnection();
        }
#if DotNet45
        /// <summary>
        /// 创建数据库事务
        /// </summary>
        /// <param name="level">默认 RepeatableRead</param>
        /// <returns></returns>
#else
        /// <summary>
        /// 创建数据库事务
        /// </summary>
        /// <param name="level">默认 RepeatableRead</param>
        /// <param name="isAsyncLocal">是否创建异步上下文，默认为 true</param>
        /// <returns></returns>
#endif
        public async Task<Transaction> CreateTransaction(IsolationLevel level = IsolationLevel.RepeatableRead
#if !DotNet45
            , bool isAsyncLocal = true
#endif
            )
        {
#if DotNet45
            bool isAsyncLocal = false;
#else
            if (isAsyncLocal && Transaction.AsyncLocal.Value != null) throw new InvalidOperationException("当前异步上下文已经绑定数据库事务");
#endif
            var connection = GetConnection() ?? await Creator.CreateConnection();
            var dbTransaction = default(DbTransaction);
            try
            {
#if NetStandard21
                dbTransaction = await connection.BeginTransactionAsync(level);
#else
                dbTransaction = connection.BeginTransaction(level);
#endif
                Transaction transaction = new Transaction(this, connection, dbTransaction, isAsyncLocal);
                connection = null;
                return transaction;
            }
            finally
            {
                if (connection != null)
                {
                    if (dbTransaction != null) await dbTransaction.DisposeAsync();
                    await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
        }
        /// <summary>
        /// 创建默认数据库事务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task<Transaction> CreateDefaultTransaction()
        {
#if DotNet45
            return CreateTransaction();
#else
            return CreateTransaction(isAsyncLocal: false);
#endif
        }
        /// <summary>
        /// 检查数据库事务连接池是否匹配
        /// </summary>
        /// <param name="transaction"></param>
#if NetStandard21
        internal void CheckTransaction(ref Transaction? transaction)
#else
        internal void CheckTransaction(ref Transaction transaction)
#endif
        {
#if !DotNet45
            if (transaction == null)
            {
                transaction = Transaction.AsyncLocal.Value;
                if (!object.ReferenceEquals(this, transaction?.ConnectionPool)) transaction = null;
                return;
            }
#endif
            if (!object.ReferenceEquals(this, transaction.ConnectionPool)) throw new ArgumentException("数据库事务连接池不匹配");
        }
        /// <summary>
        /// 执行 SQL 语句返回受影响数据行数
        /// </summary>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> ExecuteNonQuery(string statement, int timeoutSeconds = 0, Transaction? transaction = null)
#else
        public Task<int> ExecuteNonQuery(string statement, int timeoutSeconds = 0, Transaction transaction = null)
#endif
        {
            CheckTransaction(ref transaction);
            return ExecuteNonQueryTransaction(statement, timeoutSeconds, transaction);
        }
        /// <summary>
        /// 执行 SQL 语句返回受影响数据行数
        /// </summary>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal async Task<int> ExecuteNonQueryTransaction(string statement, int timeoutSeconds, Transaction? transaction)
#else
        internal async Task<int> ExecuteNonQueryTransaction(string statement, int timeoutSeconds, Transaction transaction)
#endif
        {
            if (transaction == null)
            {
                var connection = GetConnection() ?? await CreateConnection();
                try
                {
                    int rowCount;
#if NetStandard21
                    await using (DbCommand command = CreateCommand(connection, statement))
#else
                    using (DbCommand command = CreateCommand(connection, statement))
#endif
                    {
                        if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
                        rowCount = await command.ExecuteNonQueryAsync();
                    }
                    await FreeConnection(connection);
                    connection = null;
                    return rowCount;
                }
                finally
                {
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
#if NetStandard21
            await using (DbCommand command = transaction.CreateCommand(statement))
#else
            using (DbCommand command = transaction.CreateCommand(statement))
#endif
            {
                transaction.Set(command);
                if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// 创建数据库表格持久化
        /// </summary>
        /// <typeparam name="T">持久化表格模型类型</typeparam>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <param name="tableEvent">表格操作事件处理</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <returns></returns>
#if NetStandard21
        public async Task<TableQuery<T, KT>> CreateTableQuery<T, KT>(ITableEvent<T>? tableEvent = null, ModelAttribute? attribute = null)
#else
        public async Task<TableQuery<T, KT>> CreateTableQuery<T, KT>(ITableEvent<T> tableEvent = null, ModelAttribute attribute = null)
#endif
            where T : class
            where KT : IEquatable<KT>
        {
            Type modelType = typeof(T);
            if (attribute == null) attribute = modelType.GetCustomAttribute<ModelAttribute>(true) ?? TableWriter.DefaultAttribute;
            LeftArray<Member> memberList = Member.Get(MemberIndexGroup<T>.GetFields(attribute.MemberFilters), MemberIndexGroup<T>.GetProperties(attribute.MemberFilters), true);
            memberList.Sort(Member.Sort);
            Member[] members = memberList.ToArray();
            var primaryKey = default(Member);
            foreach (Member member in members)
            {
                if (member.Attribute.PrimaryKeyType != PrimaryKeyTypeEnum.None && member.MemberIndex.MemberSystemType == typeof(KT)
                    && (member.Attribute.PrimaryKeyType == PrimaryKeyTypeEnum.PrimaryKey || typeof(KT) == typeof(int) || typeof(KT) == typeof(long)))
                {
                    primaryKey = member;
                    break;
                }
            }
            if (primaryKey == null) throw new InvalidCastException($"{modelType.fullName()} 没有找到 {typeof(KT).fullName()} 关键字成员");
            if (primaryKey.Attribute.PrimaryKeyType == PrimaryKeyTypeEnum.AutoIdentity)
            {
                if (Creator.AutoIdentityWriter == null) throw new InvalidOperationException("当前数据库连接不支持自增ID");
                if (typeof(KT) == typeof(long))
                {
                    AutoIdentityTableWriter64<T> autoIdentityWriter = new AutoIdentityTableWriter64<T>(this, attribute, members, primaryKey, tableEvent);
                    if (attribute.AutoCreateTable) await Creator.AutoCreateTable(autoIdentityWriter);
                    await autoIdentityWriter.GetCurrentIdentity();
                    return (new TableQuery<T, long>(autoIdentityWriter) as TableQuery<T, KT>).notNull();
                }
                else
                {
                    AutoIdentityTableWriter<T> autoIdentityWriter = new AutoIdentityTableWriter<T>(this, attribute, members, primaryKey, tableEvent);
                    if (attribute.AutoCreateTable) await Creator.AutoCreateTable(autoIdentityWriter);
                    await autoIdentityWriter.GetCurrentIdentity();
                    return (new TableQuery<T, int>(autoIdentityWriter) as TableQuery<T, KT>).notNull();
                }
            }
            TableWriter<T, KT> writer = new TableWriter<T, KT>(this, attribute, members, primaryKey, tableEvent);
            if (attribute.AutoCreateTable) await Creator.AutoCreateTable(writer);
            return new TableQuery<T, KT>(writer);
        }
        /// <summary>
        /// 创建数据库表格持久化
        /// </summary>
        /// <typeparam name="T">持久化表格模型类型</typeparam>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <param name="tableEvent">表格操作事件处理</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <returns></returns>
#if NetStandard21
        public async Task<TablePersistence<T, KT>> CreateTablePersistence<T, KT>(ITableEvent<T>? tableEvent = null, ModelAttribute? attribute = null)
#else
        public async Task<TablePersistence<T, KT>> CreateTablePersistence<T, KT>(ITableEvent<T> tableEvent = null, ModelAttribute attribute = null)
#endif
            where T : class
            where KT : IEquatable<KT>
        {
            return new TablePersistence<T, KT>(await CreateTableQuery<T, KT>(tableEvent, attribute));
        }
        /// <summary>
        /// 创建业务表格持久化
        /// </summary>
        /// <typeparam name="BT">业务表格模型类型</typeparam>
        /// <typeparam name="T">持久化表格模型类型</typeparam>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <param name="tableEvent">表格操作事件处理</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <returns></returns>
#if NetStandard21
        public async Task<BusinessPersistence<BT, T, KT>> CreateBusinessPersistence<BT, T, KT>(BusinessTableEvent<BT, T>? tableEvent = null, ModelAttribute? attribute = null)
#else
        public async Task<BusinessPersistence<BT, T, KT>> CreateBusinessPersistence<BT, T, KT>(BusinessTableEvent<BT, T> tableEvent = null, ModelAttribute attribute = null)
#endif
            where BT : class, T
            where T : class
            where KT : IEquatable<KT>
        {
            return new BusinessPersistence<BT, T, KT>(await CreateTableQuery<T, KT>(tableEvent, attribute));
        }
        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<T?> SingleOrDefault<T>(string statement, int timeoutSeconds = 0, Transaction? transaction = null) where T : class
#else
        public Task<T> SingleOrDefault<T>(string statement, int timeoutSeconds = 0, Transaction transaction = null) where T : class
#endif
        {
            CheckTransaction(ref transaction);
            return SingleOrDefaultTransaction<T>(statement, timeoutSeconds, transaction);
        }
        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal async Task<T?> SingleOrDefaultTransaction<T>(string statement, int timeoutSeconds, Transaction? transaction) where T : class
#else
        internal async Task<T> SingleOrDefaultTransaction<T>(string statement, int timeoutSeconds, Transaction transaction) where T : class
#endif
        {
            if (transaction == null)
            {
                var connection = GetConnection() ?? await CreateConnection();
                try
                {
#if NetStandard21
                    var value = default(T);
                    await using (DbCommand command = CreateCommand(connection, statement))
#else
                    T value;
                    using (DbCommand command = CreateCommand(connection, statement))
#endif
                    {
                        if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
                        value = await singleOrDefault<T>(command);
                    }
                    await FreeConnection(connection);
                    connection = null;
                    return value;
                }
                finally
                {
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
#if NetStandard21
            await using (DbCommand command = transaction.CreateCommand(statement))
#else
            using (DbCommand command = transaction.CreateCommand(statement))
#endif
            {
                transaction.Set(command);
                if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
                return await singleOrDefault<T>(command);
            }
        }
        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
#if NetStandard21
        private static async Task<T?> singleOrDefault<T>(DbCommand command) where T : class
#else
        private static async Task<T> singleOrDefault<T>(DbCommand command) where T : class
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
                    T value = DefaultConstructor<T>.Constructor().notNull();
                    int[] columnIndexs = ModelReader<T>.GetColumnIndexCache(reader);
                    try
                    {
                        if (columnIndexs.Length != 0) ModelReader<T>.Reader(reader, value, columnIndexs);
                    }
                    finally { ModelReader<T>.FreeColumnIndexCache(columnIndexs); }
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<LeftArray<T>> Query<T>(string statement, int timeoutSeconds = 0, Transaction? transaction = null) where T : class
#else
        public async Task<LeftArray<T>> Query<T>(string statement, int timeoutSeconds = 0, Transaction transaction = null) where T : class
#endif
        {
            CheckTransaction(ref transaction);
            if (transaction == null)
            {
                var connection = GetConnection() ?? await CreateConnection();
                try
                {
                    LeftArray<T> array;
#if NetStandard21
                    await using (DbCommand command = CreateCommand(connection, statement))
#else
                    using (DbCommand command = CreateCommand(connection, statement))
#endif
                    {
                        if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
                        array = await Query<T>(command);
                    }
                    await FreeConnection(connection);
                    connection = null;
                    return array;
                }
                finally
                {
                    if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
                }
            }
#if NetStandard21
            await using (DbCommand command = transaction.CreateCommand(statement))
#else
            using (DbCommand command = transaction.CreateCommand(statement))
#endif
            {
                transaction.Set(command);
                if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
                return await Query<T>(command);
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static async Task<LeftArray<T>> Query<T>(DbCommand command) where T : class
        {
            LeftArray<T> array = new LeftArray<T>(0);
#if NetStandard21
            await using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#else
            using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#endif
            {
                int[] columnIndexs = ModelReader<T>.GetColumnIndexCache(reader);
                try
                {
                    while (await reader.ReadAsync())
                    {
                        T value = DefaultConstructor<T>.Constructor().notNull();
                        if (columnIndexs.Length != 0) ModelReader<T>.Reader(reader, value, columnIndexs);
                        array.Add(value);
                    }
                }
                finally { ModelReader<T>.FreeColumnIndexCache(columnIndexs); }
            }
            return array;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        public async Task<ModelSelectEnumerator<T>> Select<T>(string statement, int timeoutSeconds = 0, Transaction? transaction = null) where T : class
#else
        public async Task<ModelSelectEnumerator<T>> Select<T>(string statement, int timeoutSeconds = 0, Transaction transaction = null) where T : class
#endif
        {
            CheckTransaction(ref transaction);
            ModelSelectEnumerator<T> selectEnumerator = new ModelSelectEnumerator<T>(this, transaction);
            try
            {
                await selectEnumerator.GetReader(statement, timeoutSeconds);
                return selectEnumerator;
            }
            finally
            {
                if (selectEnumerator.Reader == null) await selectEnumerator.DisposeAsync();
            }
        }

        /// <summary>
        /// 数据库远程代理访问网络吞吐优化 查询第一个数据（需要 RPC 服务启用压缩）
        /// </summary>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <returns></returns>
        public async Task<RemoteProxy.DataRow> RemoteProxySingleOrDefault(string statement, int timeoutSeconds)
        {
            var connection = GetConnection() ?? await CreateConnection();
            try
            {
#if NetStandard21
                await using (DbCommand command = CreateCommand(connection, statement))
#else
                using (DbCommand command = CreateCommand(connection, statement))
#endif
                {
                    if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
#if NetStandard21
                    await using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#else
                    using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#endif
                    {
                        if (await reader.ReadAsync()) return new RemoteProxy.DataRow(reader, null);
                    }
                }
                await FreeConnection(connection);
                connection = null;
            }
            finally
            {
                if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
            }
            return default(RemoteProxy.DataRow);
        }
        /// <summary>
        /// 数据库远程代理访问网络吞吐优化 查询数据（需要 RPC 服务启用压缩）
        /// </summary>
        /// <param name="statement">SQL 语句</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="callback">数据行回调委托</param>
        /// <returns></returns>
        public async Task RemoteProxyQuery(string statement, int timeoutSeconds, AutoCSer.Net.CommandServerKeepCallbackCount<RemoteProxy.DataRow> callback)
        {
            var connection = GetConnection() ?? await CreateConnection();
            try
            {
#if NetStandard21
                await using (DbCommand command = CreateCommand(connection, statement))
#else
                using (DbCommand command = CreateCommand(connection, statement))
#endif
                {
                    if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
#if NetStandard21
                    await using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#else
                    using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
#endif
                    {
                        var columns = default(RemoteProxy.Column[]);
                        while (await reader.ReadAsync())
                        {
                            RemoteProxy.DataRow dataRow = new RemoteProxy.DataRow(reader, columns);
                            if (!await callback.CallbackAsync(dataRow)) return;
                            if (columns == null) columns = dataRow.Columns;
                        }
                    }
                }
                await FreeConnection(connection);
                connection = null;
            }
            finally
            {
                if (connection != null) await ConnectionCreator.CloseConnectionAsync(connection);
            }
        }

        /// <summary>
        /// 检查自增ID记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
#if NetStandard21
        internal async Task<AutoIdentity?> CheckAutoIdentity(string tableName)
#else
        internal async Task<AutoIdentity> CheckAutoIdentity(string tableName)
#endif
        {
            var autoIdentityWriter = Creator.AutoIdentityWriter.notNull();
            Query<AutoIdentity> query = autoIdentityWriter.CreateQuery(p => p.TableName == tableName, true).GetQueryData(1);
#if NetStandard21
            await using (Transaction transaction = await CreateTransaction(isAsyncLocal: false))
#else
            using (Transaction transaction = await CreateDefaultTransaction())
#endif
            {
                var autoIdentity = await autoIdentityWriter.SingleOrDefault(query, transaction);
                if (autoIdentity != null) return autoIdentity;
                await autoIdentityWriter.Insert(new AutoIdentity { TableName = tableName }, transaction);
                await transaction.CommitAsync();
            }
            return null;
        }
        /// <summary>
        /// 更新自增ID记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task UpdateAutoIdentity(string tableName, long identity)
        {
            return Creator.AutoIdentityWriter.notNull().Update(new AutoIdentity { TableName = tableName, Identity = identity }, null, p => p.TableName == tableName && p.Identity < identity);
        }

        /// <summary>
        /// 创建带有自增ID的数据库连接池
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="autoIdentityTableName">自增ID记录表格名称</param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ConnectionPool> Create(ConnectionCreator creator, string? autoIdentityTableName = null)
#else
        public static async Task<ConnectionPool> Create(ConnectionCreator creator, string autoIdentityTableName = null)
#endif
        {
            ConnectionPool connectionPool = new ConnectionPool(creator);
            var autoIdentityAttribute = default(ModelAttribute);
            if (!string.IsNullOrEmpty(autoIdentityTableName))
            {
                autoIdentityAttribute = TableWriter.DefaultAttribute.Clone();
                autoIdentityAttribute.TableName = autoIdentityTableName;
            }
            TablePersistence<AutoIdentity, string> autoIdentityTable = await connectionPool.CreateTablePersistence<AutoIdentity, string>(null, autoIdentityAttribute);
            creator.AutoIdentityWriter = autoIdentityTable.Writer;
            return connectionPool;
        }
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static DbCommand CreateCommand(DbConnection connection, string statement)
        {
            DbCommand command = connection.CreateCommand();
            SetCommand(command, statement);
            return command;
        }
        /// <summary>
        /// 设置命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="statement"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetCommand(DbCommand command, string statement)
        {
            command.CommandText = statement;
            command.CommandType = CommandType.Text;
        }
    }
}
