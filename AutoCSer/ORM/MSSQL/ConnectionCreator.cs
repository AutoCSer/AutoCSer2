using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// 创建数据库连接（适合2005以及以上版本）
    /// </summary>
    public class ConnectionCreator : AutoCSer.ORM.ConnectionCreator
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private readonly string connectionString;
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        public ConnectionCreator(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        internal override async Task<DbConnection> CreateConnection()
        {
            bool isOpen = false;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
                isOpen = true;
                return connection;
            }
            finally
            {
                if (!isOpen) await CloseConnectionAsync(connection);
            }
        }
        /// <summary>
        /// 名称格式化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal override string FormatName(string name) { return $"[{name}]"; }
        /// <summary>
        /// 名称格式化
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="name"></param>
        internal override void FormatName(CharStream charStream, string name)
        {
            charStream.Write('[');
            charStream.SimpleWrite(name);
            charStream.Write(']');
        }
        /// <summary>
        /// 获取表格列备注信息 SQL 语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected virtual string GetSelectTableColumnRemarkStatement(string tableName)
        {
            return @"select value from ::fn_listextendedproperty(null,'user','dbo','table','" + tableName + @"','column',syscolumns.name)as property where property.name='MS_Description'";
        }
        /// <summary>
        /// 自动创建数据库表格
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <returns></returns>
        internal override async Task AutoCreateTable(TableWriter tableWriter)
        {
            LeftArray<TableColumn> columns = await tableWriter.ConnectionPool.Query<TableColumn>(@"declare @id int
set @id=object_id(N'[dbo].[" + tableWriter.TableName + @"]')
if(select top 1 id from sysobjects where id=@id and objectproperty(id,N'IsUserTable')=1)is not null begin
 select columnproperty(id,name,'IsIdentity')as isidentity,id,xusertype,name,length,xprec,xscale,isnullable,colid,isnull((select top 1 text from syscomments where id=syscolumns.cdefault and colid=1),'')as defaultvalue,isnull((" + GetSelectTableColumnRemarkStatement(tableWriter.TableName) + @"),'')as remark from syscolumns where id=@id order by colid
end");
            if (columns.Count == 0)
            {
                await createTable(tableWriter);
                return;
            }
            Dictionary<string, TableColumn> columnNames = columns.getDictionary(p => p.name);
            LeftArray<CustomColumnName> newColumns = new LeftArray<CustomColumnName>(0);
            TableColumn column;
            foreach (CustomColumnName name in tableWriter.Columns)
            {
                if (columnNames.TryGetValue(name.Name, out column)) column.Match(name.Member, tableWriter);
                else newColumns.Add(name);
            }
            if (newColumns.Count != 0) await createColumn(tableWriter, newColumns);

            LeftArray<TableIndexColumn> indexs = await tableWriter.ConnectionPool.Query<TableIndexColumn>(@"declare @id int
set @id=object_id(N'[dbo].[" + tableWriter.TableName + @"]')
select indid,colid,(select top 1 status from sysindexes where id=@id and indid=sysindexkeys.indid)as status from sysindexkeys where id=@id order by indid,keyno");
            bool isPrimaryKey = false;
            if (indexs.Count != 0)
            {
                Dictionary<short, TableColumn> columnIDs = columns.getDictionary(p => p.colid);
                LeftArray<TableIndexColumn> matchIndexs = new LeftArray<TableIndexColumn>(0);
                foreach (TableIndexColumn index in indexs)
                {
                    if ((index.status & TableIndexStateEnum.PrimaryKeyConstraint) != 0)
                    {
                        isPrimaryKey = true;
                        if (matchIndexs.Count != 0 && matchIndexs.Array[0].indid != index.indid)
                        {
                            if (TableIndexColumn.MatchPrimaryKey(tableWriter.PrimaryKey, ref matchIndexs, columnIDs)) return;
                            matchIndexs.SetEmpty();
                        }
                        matchIndexs.Add(index);
                    }
                }
                if (TableIndexColumn.MatchPrimaryKey(tableWriter.PrimaryKey, ref matchIndexs, columnIDs)) return;
            }
            if (isPrimaryKey)
            {
                if (tableWriter.PrimaryKey.CustomColumnAttribute == null) throw new Exception($"关键字不匹配模型定义 {tableWriter.PrimaryKey.MemberIndex.Member.Name}");
                throw new Exception($"关键字不匹配模型定义 {string.Join(",", tableWriter.PrimaryKey.CustomColumnNames)}");
            }
            await createPrimaryKey(tableWriter);
        }
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <returns></returns>
        private async Task createTable(TableWriter tableWriter)
        {
            string statement;
            CharStream charStream = GetCharStreamCache();
            try
            {
                charStream.SimpleWrite("create table dbo.");
                FormatName(charStream, tableWriter.TableName);
                charStream.Write('(');
                bool isNext = false;
                foreach (CustomColumnName name in tableWriter.Columns)
                {
                    if (isNext) charStream.Write(',');
                    else isNext = true;
                    appendColumn(charStream, name, true);
                }
                Member primaryKey = tableWriter.PrimaryKey;
                charStream.SimpleWrite(",primary key(");
                if (primaryKey.CustomColumnAttribute == null) FormatName(charStream, primaryKey.MemberIndex.Member.Name);
                else
                {
                    isNext = false;
                    foreach (CustomColumnName name in primaryKey.CustomColumnNames)
                    {
                        if (isNext) charStream.Write(',');
                        else isNext = true;
                        FormatName(charStream, name.Name);
                    }
                }
                charStream.Write(')');
                charStream.SimpleWrite(")on[primary]");
                foreach (CustomColumnName name in tableWriter.Columns) appendRemark(charStream, tableWriter.TableName, name);
                statement = charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
            await tableWriter.ConnectionPool.ExecuteNonQueryTransaction(statement, 0, null);
        }
        /// <summary>
        /// 写入列信息
        /// </summary>
        /// <param name="charStream">SQL语句流</param>
        /// <param name="name"></param>
        /// <param name="isCreateTable"></param>
        private void appendColumn(CharStream charStream, CustomColumnName name, bool isCreateTable)
        {
            FormatName(charStream, name.Name);
            charStream.Write(' ');
            StringAttribute stringAttribute = null;
            Member member = name.Member;
            switch (member.ReaderDataType)
            {
                case ReaderDataTypeEnum.Int: charStream.SimpleWrite(nameof(SqlDbType.Int)); break;
                case ReaderDataTypeEnum.Long: charStream.SimpleWrite(nameof(SqlDbType.BigInt)); break;
                case ReaderDataTypeEnum.Short: charStream.SimpleWrite(nameof(SqlDbType.SmallInt)); break;
                case ReaderDataTypeEnum.Byte: charStream.SimpleWrite(nameof(SqlDbType.TinyInt)); break;
                case ReaderDataTypeEnum.Bool: charStream.SimpleWrite(nameof(SqlDbType.Bit)); break;
                case ReaderDataTypeEnum.DateTime:
                    DateTimeAttribute dateTimeAttribute = member.Attribute as DateTimeAttribute ?? DateTimeAttribute.Default;
                    charStream.SimpleWrite(dateTimeAttribute.Type.ToString());
                    break;
                case ReaderDataTypeEnum.DateTimeOffset: charStream.SimpleWrite(nameof(SqlDbType.DateTimeOffset)); break;
                case ReaderDataTypeEnum.TimeSpan: charStream.SimpleWrite(nameof(SqlDbType.Time)); break;
                case ReaderDataTypeEnum.Decimal:
                    DecimalAttribute decimalAttribute = member.Attribute as DecimalAttribute;
                    if (decimalAttribute == null)
                    {
                        MoneyAttribute moneyAttribute = member.Attribute as MoneyAttribute;
                        if (moneyAttribute != null)
                        {
                            charStream.SimpleWrite(moneyAttribute.IsSmall ? nameof(SqlDbType.SmallMoney) : nameof(SqlDbType.Money));
                            break;
                        }
                    }
                    if (decimalAttribute == null) decimalAttribute = DecimalAttribute.Default;
                    charStream.SimpleWrite(nameof(SqlDbType.Decimal));
                    charStream.Write('(');
                    AutoCSer.Extensions.NumberExtension.ToString(Math.Min(decimalAttribute.Precision, DecimalAttribute.MaxPrecision), charStream);
                    charStream.Write(',');
                    AutoCSer.Extensions.NumberExtension.ToString(decimalAttribute.Scale, charStream);
                    charStream.Write(')');
                    break;
                case ReaderDataTypeEnum.Guid: charStream.SimpleWrite(nameof(SqlDbType.UniqueIdentifier)); break;
                case ReaderDataTypeEnum.Double: charStream.SimpleWrite(nameof(SqlDbType.Float)); break;
                case ReaderDataTypeEnum.Float: charStream.SimpleWrite(nameof(SqlDbType.Real)); break;
                default:
                    stringAttribute = member.Attribute as StringAttribute ?? StringAttribute.Default;
                    if (stringAttribute.Size == 0)
                    {
                        charStream.SimpleWrite(stringAttribute.IsAscii ? nameof(SqlDbType.VarChar) : nameof(SqlDbType.NVarChar));
                        charStream.SimpleWrite("(max)");
                    }
                    else
                    {
                        ushort size = stringAttribute.Size;
                        if (stringAttribute.IsAscii)
                        {
                            if (size > 8000) size = 8000;
                            charStream.SimpleWrite(stringAttribute.IsFixed ? nameof(SqlDbType.Char) : nameof(SqlDbType.VarChar));
                        }
                        else
                        {
                            if (size > 4000) size = 4000;
                            charStream.SimpleWrite(stringAttribute.IsFixed ? nameof(SqlDbType.NChar) : nameof(SqlDbType.NVarChar));
                        }
                        charStream.Write('(');
                        AutoCSer.Extensions.NumberExtension.ToString(size, charStream);
                        charStream.Write(')');
                    }
                    break;
            }
            string defaultValue = member.Attribute.DefaultValue;
            if (!isCreateTable && !member.IsNullable && string.IsNullOrEmpty(defaultValue))
            {
                switch (member.ReaderDataType)
                {
                    case ReaderDataTypeEnum.Int:
                    case ReaderDataTypeEnum.Long:
                    case ReaderDataTypeEnum.Short:
                    case ReaderDataTypeEnum.Byte:
                    case ReaderDataTypeEnum.Bool:
                    case ReaderDataTypeEnum.Decimal:
                    case ReaderDataTypeEnum.Double:
                    case ReaderDataTypeEnum.Float:
                        defaultValue = "0";
                        break;
                    case ReaderDataTypeEnum.DateTime:
                    case ReaderDataTypeEnum.DateTimeOffset:
                        defaultValue = "'1900/1/1'";
                        break;
                    case ReaderDataTypeEnum.TimeSpan:
                        defaultValue = "'00:00:00'";
                        break;
                    case ReaderDataTypeEnum.Guid: defaultValue = "newid()"; break;
                    default: defaultValue = "''"; break;
                }
            }
            if (!string.IsNullOrEmpty(defaultValue))
            {
                charStream.SimpleWrite(" default ");
                charStream.SimpleWrite(defaultValue);
            }
            switch (member.ReaderDataType)
            {
                case ReaderDataTypeEnum.Json:
                case ReaderDataTypeEnum.String:
                    if (!stringAttribute.IsNullable) charStream.SimpleWrite(" not");
                    break;
                default:
                    if (!member.IsNullable) charStream.SimpleWrite(" not");
                    break;
            }
            charStream.SimpleWrite(" null");
        }
        /// <summary>
        /// 写入备注信息
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableName"></param>
        /// <param name="name"></param>
        private void appendRemark(CharStream charStream, string tableName, CustomColumnName name)
        {
            if (!string.IsNullOrEmpty(name.Member.Attribute.Remark))
            {
                charStream.SimpleWrite(@"
exec dbo.sp_addextendedproperty @name=N'MS_Description',@value=N");
                Convert(charStream, name.Member.Attribute.Remark);
                charStream.SimpleWrite(",@level0type=N'USER',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'");
                FormatName(charStream, tableName);
                charStream.SimpleWrite("', @level2type=N'COLUMN',@level2name=N'");
                FormatName(charStream, name.Name);
                charStream.Write('\'');
            }
        }
        /// <summary>
        /// 自动补全创建数据列
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private async Task createColumn(TableWriter tableWriter, LeftArray<CustomColumnName> columns)
        {
            string statement;
            CharStream charStream = GetCharStreamCache();
            try
            {
                bool isUpdate = false;
                foreach (CustomColumnName name in columns)
                {
                    charStream.SimpleWrite(@"
alter table dbo.");
                    FormatName(charStream, tableWriter.TableName);
                    charStream.SimpleWrite(" add ");
                    appendColumn(charStream, name, false);
                    isUpdate |= !string.IsNullOrEmpty(name.Member.Attribute.CreateColumnUpdateValue);
                }
                if (isUpdate)
                {
                    charStream.SimpleWrite(@"
update ");
                    charStream.SimpleWrite(tableWriter.TableName);
                    charStream.SimpleWrite(" set ");
                    foreach (CustomColumnName column in columns)
                    {
                        if (!string.IsNullOrEmpty(column.Member.Attribute.CreateColumnUpdateValue))
                        {
                            if (isUpdate) isUpdate = false;
                            else charStream.Write(',');
                            FormatName(charStream, column.Name);
                            charStream.Write('=');
                            charStream.SimpleWrite(column.Member.Attribute.CreateColumnUpdateValue);
                        }
                    }
                }
                statement = charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
            await tableWriter.ConnectionPool.ExecuteNonQueryTransaction(statement, 0, null);
        }
        /// <summary>
        /// 自动不全创建主键
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <returns></returns>
        private async Task createPrimaryKey(TableWriter tableWriter)
        {
            string statement;
            CharStream charStream = GetCharStreamCache();
            try
            {
                charStream.SimpleWrite("alter table dbo.");
                FormatName(charStream, tableWriter.TableName);
                charStream.SimpleWrite(" add constraint pk_");
                charStream.SimpleWrite(tableWriter.TableName);
                if (tableWriter.PrimaryKey.CustomColumnAttribute == null)
                {
                    charStream.Write('_');
                    charStream.SimpleWrite(tableWriter.PrimaryKey.MemberIndex.Member.Name);
                }
                else
                {
                    foreach (CustomColumnName name in tableWriter.PrimaryKey.CustomColumnNames)
                    {
                        charStream.Write('_');
                        charStream.SimpleWrite(name.Name);
                    }
                }
                charStream.SimpleWrite(" primary key(");
                if (tableWriter.PrimaryKey.CustomColumnAttribute == null)
                {
                    FormatName(charStream, tableWriter.PrimaryKey.MemberIndex.Member.Name);
                }
                else
                {
                    bool isNext = false;
                    foreach (CustomColumnName name in tableWriter.PrimaryKey.CustomColumnNames)
                    {
                        if (isNext) charStream.Write(',');
                        else isNext = true;
                        FormatName(charStream, name.Name);
                    }
                }
                charStream.Write(')');
                statement = charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
            await tableWriter.ConnectionPool.ExecuteNonQueryTransaction(statement, 0, null);
        }
        /// <summary>
        /// 创建表格索引
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columns"></param>
        /// <param name="indexNameSuffix"></param>
        /// <param name="isUnique"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        internal override async Task<bool> CreateIndex(TableWriter tableWriter, CustomColumnName[] columns, string indexNameSuffix, bool isUnique, int timeoutSeconds)
        {
            CharStream charStream;
            if (indexNameSuffix == null)
            {
                charStream = GetCharStreamCache();
                try
                {
                    bool isNext = false;
                    foreach (CustomColumnName name in columns)
                    {
                        if (isNext) charStream.Write('_');
                        else isNext = true;
                        charStream.SimpleWrite(name.Name);
                    }
                    indexNameSuffix = charStream.ToString();
                }
                finally { FreeCharStreamCache(charStream); }
            }
            TableIndex index = await tableWriter.ConnectionPool.SingleOrDefaultTransaction<TableIndex>("select top 1 indid from sysindexes where name='ix_" + tableWriter.TableName + "_" + indexNameSuffix + "'", 0, null);
            if (index != null) return false;

            string statement;
            charStream = GetCharStreamCache();
            try
            {
                if (isUnique) charStream.SimpleWrite("create unique index[");
                else charStream.SimpleWrite("create index[");
                charStream.SimpleWrite("ix_");
                charStream.SimpleWrite(tableWriter.TableName);
                charStream.Write('_');
                charStream.SimpleWrite(indexNameSuffix);
                charStream.SimpleWrite("]on dbo.[");
                charStream.SimpleWrite(tableWriter.TableName);
                charStream.SimpleWrite("](");
                bool isNext = false;
                foreach (CustomColumnName name in columns)
                {
                    if (isNext) charStream.Write(',');
                    FormatName(charStream, name.Name);
                    isNext = true;
                }
                charStream.SimpleWrite(")on[primary]");
                statement = charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
            await tableWriter.ConnectionPool.ExecuteNonQueryTransaction(statement, timeoutSeconds, null);
            return true;
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="memberMap"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="skipCount">跳过记录数量</param>
        /// <param name="isSubQuery">如果是子查询则在前后增加小括号</param>
        /// <returns></returns>
        internal override string GetQueryStatement<T>(QueryBuilder<T> query, MemberMap<T> memberMap, uint readCount, ulong skipCount, bool isSubQuery)
        {
            CharStream charStream = GetCharStreamCache();
            try
            {
                if (isSubQuery) charStream.Write('(');
                GetQueryStatement(query, memberMap, readCount, skipCount, charStream);
                if (isSubQuery) charStream.Write(')');
                return charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="memberMap"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="skipCount">跳过记录数量</param>
        /// <param name="charStream"></param>
        internal override void GetQueryStatement<T>(QueryBuilder<T> query, MemberMap<T> memberMap, uint readCount, ulong skipCount, CharStream charStream)
        {
            if (skipCount > 0)
            {
                if (query.OrderItem.Member == null) throw new InvalidCastException("缺少排序字段，不支持查询跳过记录");
                charStream.SimpleWrite("select ");
                writeColumnName(charStream, query.TableWriter, memberMap);
                charStream.SimpleWrite(" from(select row_number()over(");
                writeOrder(charStream, query);
                charStream.SimpleWrite(")__rownumber__,");
            }
            else
            {
                charStream.SimpleWrite("select ");
                if (readCount > 0)
                {
                    charStream.SimpleWrite("top ");
                    AutoCSer.Extensions.NumberExtension.ToString(readCount, charStream);
                    charStream.Write(' ');
                }
            }
            writeColumnName(charStream, query.TableWriter, memberMap);
            charStream.SimpleWrite(" from ");
            FormatName(charStream, query.TableWriter.TableName);
            writeWithLock(charStream, query);
            wirteWhere(charStream, query);
            if (skipCount > 0)
            {
                charStream.SimpleWrite(")__queryskip__ where __rownumber__>");
                AutoCSer.Extensions.NumberExtension.ToString(skipCount, charStream);
                charStream.SimpleWrite(" and __rownumber__<=");
                AutoCSer.Extensions.NumberExtension.ToString(skipCount + readCount, charStream);
            }
            else writeOrder(charStream, query);
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="extensionQueryData"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="isSubQuery">如果是子查询则在前后增加小括号</param>
        /// <returns></returns>
        internal override string GetQueryStatement<T>(QueryBuilder<T> query, ref ExtensionQueryData extensionQueryData, uint readCount, bool isSubQuery)
        {
            CharStream charStream = GetCharStreamCache();
            try
            {
                if (isSubQuery) charStream.Write('(');
                GetQueryStatement(query, ref extensionQueryData, readCount, charStream);
                if (isSubQuery) charStream.Write(')');
                return charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
        }
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="extensionQueryData"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="charStream"></param>
        internal override void GetQueryStatement<T>(QueryBuilder<T> query, ref ExtensionQueryData extensionQueryData, uint readCount, CharStream charStream)
        {
            charStream.SimpleWrite("select ");
            if (readCount > 0)
            {
                charStream.SimpleWrite("top ");
                AutoCSer.Extensions.NumberExtension.ToString(readCount, charStream);
                charStream.Write(' ');
            }
            if (extensionQueryData.QueryNames.Count == 0) throw new ArgumentNullException($"{query.TableWriter.TableName} 缺少查询列");
            writeConcat(charStream, ref extensionQueryData.QueryNames);
            charStream.SimpleWrite(" from ");
            FormatName(charStream, query.TableWriter.TableName);
            writeWithLock(charStream, query);
            wirteWhere(charStream, query);
            if (extensionQueryData.GroupBys.Length != 0)
            {
                charStream.SimpleWrite(" group by ");
                writeConcat(charStream, ref extensionQueryData.GroupBys);
                if (extensionQueryData.Havings.Length != 0)
                {
                    charStream.SimpleWrite(" having ");
                    writeConcatCondition(charStream, ref extensionQueryData.Havings);
                }
            }
            writeOrder(charStream, query);
        }
        /// <summary>
        /// 写入查询列名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        /// <param name="memberMap"></param>
        private void writeColumnName<T>(CharStream charStream, TableWriter tableWriter, MemberMap<T> memberMap) where T : class
        {
            int isNext = 0;
            foreach (Member member in tableWriter.Members)
            {
                if (memberMap.MemberMapData.IsMember(member.MemberIndex.MemberIndex))
                {
                    if (isNext == 0) isNext = 1;
                    else charStream.Write(',');
                    writeConcatName(charStream, member);
                }
            }
            if (isNext == 0) throw new ArgumentNullException($"{tableWriter.TableName} 缺少查询列");
        }
        /// <summary>
        /// 写入数据列名称
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="member"></param>
        private void writeConcatName(CharStream charStream, Member member)
        {
            if (member.CustomColumnAttribute == null) FormatName(charStream, member.MemberIndex.Member.Name);
            else
            {
                int isNext = 0;
                foreach (CustomColumnName name in member.CustomColumnNames)
                {
                    if (isNext == 0) isNext = 1;
                    else charStream.Write(',');
                    FormatName(charStream, name.Name);
                }
            }
        }
        /// <summary>
        /// 写入 WITH LOCK 表达式
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="query"></param>
        private static void writeWithLock(CharStream charStream, QueryBuilder query)
        {
            WithLockTypeEnum withLock = query.WithLock;
            if ((withLock & WithLockTypeEnum.NoLock) != 0)
            {
                withLock ^= WithLockTypeEnum.NoLock;
                if (withLock == 0)
                {
                    charStream.SimpleWrite(" with(nolock)");
                    return;
                }
            }
            if (withLock == 0) return;
            bool isLock = false;
            if ((withLock & WithLockTypeEnum.Row) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("ROWLOCK");
            }
            if ((withLock & WithLockTypeEnum.Page) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("PAGLOCK");
            }
            if ((withLock & WithLockTypeEnum.Table) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("TABLOCK");
            }
            if ((withLock & WithLockTypeEnum.Exclusive) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("XLOCK");
            }
            if ((withLock & WithLockTypeEnum.TableExclusive) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("TABLOCKX");
            }
            if ((withLock & WithLockTypeEnum.ReadPast) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("READPAST");
            }
            if ((withLock & WithLockTypeEnum.Update) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("UPDLOCK");
            }
            if ((withLock & WithLockTypeEnum.Hold) != 0)
            {
                if (isLock) charStream.Write(',');
                else
                {
                    isLock = true;
                    charStream.SimpleWrite(" with(");
                }
                charStream.SimpleWrite("HOLDLOCK");
            }
            if (isLock) charStream.Write(')');
        }
        /// <summary>
        /// 写入 WHERE 表达式
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="query"></param>
        private void wirteWhere(CharStream charStream, QueryBuilder query)
        {
            if (query.ConditionLogicType != ConditionExpression.LogicTypeEnum.True)
            {
                charStream.SimpleWrite(" where ");
                query.Condition.WriteCondition(charStream);
                foreach (ICondition condition in query.Conditions)
                {
                    charStream.SimpleWrite(" and ");
                    condition.WriteCondition(charStream);
                }
            }
        }
        /// <summary>
        /// 写入 ORDER BY 子表达式
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="query"></param>
        private static void writeOrder(CharStream charStream, QueryBuilder query)
        {
            if (query.OrderItem.Member != null)
            {
                charStream.SimpleWrite(" order by ");
                write(charStream, query.OrderItem);
                foreach (OrderItem orderItem in query.OrderByItems)
                {
                    charStream.Write(',');
                    write(charStream, orderItem);
                }
            }
        }
        /// <summary>
        /// 写入 ORDER BY 子项
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="orderItem"></param>
        private static void write(CharStream charStream, OrderItem orderItem)
        {
            charStream.SimpleWrite(orderItem.Member);
            charStream.Write(' ');
            charStream.SimpleWrite(orderItem.IsAscending ? "asc" : "desc");
        }
        /// <summary>
        /// 写入字符串集合
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="values"></param>
        private static void writeConcat(CharStream charStream, ref LeftArray<string> values)
        {
            int isNext = 0;
            foreach (string groupBy in values)
            {
                if (isNext == 0) isNext = 1;
                else charStream.Write(',');
                charStream.SimpleWrite(groupBy);
            }
        }
        /// <summary>
        /// 写入条件字符串集合
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="values"></param>
        private static void writeConcatCondition(CharStream charStream, ref LeftArray<string> values)
        {
            int isNext = 0;
            foreach (string groupBy in values)
            {
                if (isNext == 0) isNext = 1;
                else charStream.SimpleWrite(" and ");
                charStream.SimpleWrite(groupBy);
            }
        }

        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        internal unsafe override void Convert(CharStream charStream, string value)
        {
            if (value == null) charStream.WriteJsonNull();
            else
            {
                fixed (char* valueFixed = value)
                {
                    int length = 0, wideSize = 0;
                    for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                    {
                        if (*start == '\'') ++length;
                        else if (wideSize == 0 && *start >= 256) wideSize = 1;
                    }
                    if (length == 0)
                    {
                        charStream.PrepCharSize(value.Length + wideSize + 2);
                        if (wideSize != 0) charStream.Data.Pointer.Write('N');
                        charStream.Data.Pointer.Write(value, '\'');
                        return;
                    }
                    charStream.PrepCharSize((length += value.Length) + wideSize + 2);
                    if (wideSize != 0) charStream.Data.Pointer.Write('N');
                    charStream.Data.Pointer.Write('\'');
                    byte* write = (byte*)charStream.CurrentChar;
                    for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                    {
                        if (*start != '\'')
                        {
                            *(char*)write = *start;
                            write += sizeof(char);
                        }
                        else
                        {
                            *(int*)write = ('\'' << 16) + '\'';
                            write += sizeof(int);
                        }
                    }
                    charStream.Data.Pointer.CurrentIndex += length * sizeof(char);
                    charStream.Data.Pointer.Write('\'');
                }
            }
        }
        /// <summary>
        /// LIKE 字符串转义
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        internal unsafe override void ConvertLike(CharStream charStream, string value, bool isStart, bool isEnd)
        {
            if (value != null)
            {
                foreach (char code in value)
                {
                    if (code >= 256)
                    {
                        charStream.Write('N');
                        break;
                    }
                }
            }
            base.ConvertLike(charStream, value, isStart, isEnd);
        }

        /// <summary>
        /// 获取添加数据 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableWriter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override string GetInsertStatement<T>(TableWriter<T> tableWriter, T value)
        {
            CharStream charStream = GetCharStreamCache();
            try
            {
                charStream.SimpleWrite("insert into ");
                FormatName(charStream, tableWriter.TableName);
                charStream.Write('(');
                writeColumnName(charStream, tableWriter);
                charStream.SimpleWrite(")values(");
                tableWriter.Insert(charStream, value);
                charStream.Write(')');
                return charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
        }
        /// <summary>
        /// 写入所有列名称集合
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        private void writeColumnName(CharStream charStream, TableWriter tableWriter)
        {
            int isNext = 0;
            foreach (CustomColumnName name in tableWriter.Columns)
            {
                if (isNext == 0) isNext = 1;
                else charStream.Write(',');
                FormatName(charStream, name.Name);
            }
        }
        /// <summary>
        /// 获取更新数据 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal override string GetUpdateStatement<T>(QueryBuilder<T> query, T value, MemberMap<T> memberMap)
        {
            CharStream charStream = GetCharStreamCache();
            try
            {
                charStream.SimpleWrite("update ");
                FormatName(charStream, query.TableWriter.TableName);
                charStream.SimpleWrite(" set ");
                query.TableWriter.UpdateValue(charStream, value, query.TableWriter, memberMap);
                wirteWhere(charStream, query);
                return charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
        }
        /// <summary>
        /// 获取删除数据 SQL 语句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal override string GetDeleteStatement(QueryBuilder query)
        {
            CharStream charStream = GetCharStreamCache();
            try
            {
                charStream.SimpleWrite("delete ");
                FormatName(charStream, query.TableWriter.TableName);
                wirteWhere(charStream, query);
                return charStream.ToString();
            }
            finally { FreeCharStreamCache(charStream); }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <typeparam name="T">持久化表格模型类型</typeparam>
        /// <param name="connectionPool"></param>
        /// <param name="tableWriter"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task Import<T>(ConnectionPool connectionPool, TableWriter<T> tableWriter, IEnumerable<T> values) where T : class
        {
            if (!object.ReferenceEquals(this, connectionPool.Creator)) throw new ArgumentException("数据库事务连接不匹配");
            using (DataTable dataTable = new DataTable("[" + tableWriter.TableName + "]"))
            {
                foreach(CustomColumnName name in tableWriter.Columns) dataTable.Columns.Add(new DataColumn(name.Name, name.Member.GetObjectType()));
                foreach (T value in values) dataTable.Rows.Add(tableWriter.ToArray(value));
                if (dataTable.Rows.Count != 0)
                {
                    SqlConnection connection = (SqlConnection)(connectionPool.GetConnection() ?? await CreateConnection());
                    try
                    {
                        using (SqlBulkCopy copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.UseInternalTransaction, null))
                        {
                            copy.BatchSize = dataTable.Rows.Count;
                            copy.DestinationTableName = dataTable.TableName;
                            foreach (DataColumn column in dataTable.Columns) copy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            await copy.WriteToServerAsync(dataTable);
                        }
                        await connectionPool.FreeConnection(connection);
                        connection = null;
                    }
                    finally
                    {
                        if (connection != null) await CloseConnectionAsync(connection);
                    }
                }
            }
        }

        /// <summary>
        /// SQL Server 版本正则
        /// </summary>
        private static readonly Regex versionRegex = new Regex(@"Microsoft SQL Server [0-9]{4}", RegexOptions.Compiled);
        /// <summary>
        /// 根据 SQL Server 版本创建数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<ConnectionCreator> Create(string connectionString)
        {
            string versionString;
#if DotNet45 || NetStandard2
            using (SqlConnection connection = new SqlConnection(connectionString))
#else
            await using (SqlConnection connection = new SqlConnection(connectionString))
#endif
            {
                await connection.OpenAsync();
#if DotNet45 || NetStandard2
                using (DbCommand command = ConnectionPool.CreateCommand(connection, "select @@version"))
#else
                await using (DbCommand command = ConnectionPool.CreateCommand(connection, "select @@version"))
#endif
                {
                    versionString = (string)await command.ExecuteScalarAsync();
                }
            }
            Match versionMatch = versionRegex.Match(versionString);
            if (versionMatch.Groups.Count > 0)
            {
                versionString = versionMatch.Groups[0].Value;
                int version;
                if (versionString.Length >= 4 && int.TryParse(versionString.Substring(versionString.Length - 4), out version))
                {
                    if (version >= 2000 && version < 2005) return new ConnectionCreator2000(connectionString);
                }
            }
            return new ConnectionCreator(connectionString);
        }
        /// <summary>
        /// 根据 SQL Server 版本创建数据库连接池
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="autoIdentityTableName"></param>
        /// <returns></returns>
        public static async Task<ConnectionPool> CreateConnectionPool(string connectionString, string autoIdentityTableName = null)
        {
            return await ConnectionPool.Create(await Create(connectionString), autoIdentityTableName);
        }
    }
}
