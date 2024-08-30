using System;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// 创建数据库连接（适合2005以下版本）
    /// </summary>
    public sealed class ConnectionCreator2000 : ConnectionCreator
    {
        /// <summary>
        /// 创建数据库连接 MSSQL2000
        /// </summary>
        /// <param name="connectionString"></param>
        public ConnectionCreator2000(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 获取表格列备注信息 SQL 语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected override string GetSelectTableColumnRemarkStatement(string tableName)
        {
            return @"select top 1 cast(value as varchar(256))from sysproperties where id=syscolumns.id and smallid=syscolumns.colid";
        }
    }
}
