using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace AutoCSer.ORM.Excel
{
    /// <summary>
    /// Excel 连接信息
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform(AutoCSer.SupportedOSPlatformName.Windows)]
    public sealed class ConnectionInfo
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password;
        /// <summary>
        /// 数据接口属性，默认为 Ace12
        /// </summary>
        public ProviderEnum Provider = ProviderEnum.Ace12;
        /// <summary>
        /// 混合数据处理方式，默认为 WriteAndRead
        /// </summary>
        public IntermixedEnum Intermixed = IntermixedEnum.WriteAndRead;
        /// <summary>
        /// 默认为 true 表示第一行是列名
        /// </summary>
        public bool IsTitleColumn = true;
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            using (CharStream charStream = new CharStream(UnmanagedPool.Default))
            {
                charStream.SimpleWrite("Provider=");
                switch (Provider)
                {
                    case ProviderEnum.Jet4: charStream.SimpleWrite("Microsoft.Jet.OleDb.4.0"); break;
                    //case ProviderEnum.Ace16: charStream.SimpleWrite("Microsoft.ACE.OLEDB.16.0"); break;
                    default: charStream.SimpleWrite("Microsoft.ACE.OLEDB.12.0"); break;
                }
                charStream.SimpleWrite(";Data Source=");
                charStream.SimpleWrite(DataSource);
                if (!string.IsNullOrEmpty(Password))
                {
                    charStream.WriteNotNull(";Database Password=");
                    charStream.SimpleWrite(Password);
                }
                charStream.WriteNotNull(";Extended Properties='");
                switch (Provider)
                {
                    case ProviderEnum.Jet4: charStream.SimpleWrite("Excel 8.0"); break;
                    //case ProviderEnum.Ace16: charStream.SimpleWrite("Excel 16.0"); break;
                    default: charStream.SimpleWrite("Excel 12.0"); break;
                }
                charStream.WriteNotNull(IsTitleColumn ? ";HDR=YES;IMEX=" : ";HDR=NO;IMEX=");
                charStream.WriteString((byte)Intermixed);
                charStream.Write('\'');
                return charStream.ToString();
            }
        }

        /// <summary>
        /// 表格名称
        /// </summary>
        private const string schemaTableName = "Table_Name";
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetTableNames()
        {
#if DotNet45 || NetStandard2
            using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#else
            await using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#endif
            {
                await dbConnection.OpenAsync();
                using (DataTable table = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                {
                    DataRowCollection rows = table.Rows;
                    string[] names = new string[rows.Count];
                    int nameIndex = 0;
                    foreach (DataRow row in rows) names[nameIndex++] = row[schemaTableName].ToString();
                    return names;
                }
            }
        }
        /// <summary>
        /// 默认表格名称
        /// </summary>
        public const string DefaultTableName = "Sheet1$";
        /// <summary>
        /// 获取指定表格名称，如果表格不存在返回第一个表格名称
        /// </summary>
        /// <param name="tableName">指定表格名称</param>
        /// <returns></returns>
        public async Task<string> GetFirstTableName(string tableName = DefaultTableName)
        {
#if DotNet45 || NetStandard2
            using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#else
            await using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#endif
            {
                await dbConnection.OpenAsync();
                return getFirstTableName(dbConnection, tableName);
            }
        }
        /// <summary>
        /// 获取指定表格名称，如果表格不存在返回第一个表格名称
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static string getFirstTableName(OleDbConnection dbConnection, string tableName)
        {
            string firstTableName = null;
            using (DataTable table = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
            {
                foreach (DataRow row in table.Rows)
                {
                    string nextTableName = row[schemaTableName].ToString();
                    if (nextTableName == tableName) return tableName;
                    if (firstTableName == null) firstTableName = nextTableName;
                }
            }
            return firstTableName;
        }
        /// <summary>
        /// SQL 语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement"></param>
        /// <returns></returns>
        public async Task<LeftArray<T>> Query<T>(string statement) where T : class
        {
#if DotNet45 || NetStandard2
            using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#else
            await using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#endif
            {
                await dbConnection.OpenAsync();
                return await query<T>(dbConnection, statement);
            }
        }
        /// <summary>
        /// SQL 语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        private static async Task<LeftArray<T>> query<T>(OleDbConnection dbConnection, string statement) where T : class
        {
#if DotNet45 || NetStandard2
            using (OleDbCommand command = dbConnection.CreateCommand())
#else
            await using (OleDbCommand command = dbConnection.CreateCommand())
#endif
            {
                ConnectionPool.SetCommand(command, statement);
                return await ConnectionPool.Query<T>(command);
            }
        }
        /// <summary>
        /// 查询指定表格，如果表格不存在则查询第一个表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<LeftArray<T>> QueryTable<T>(string tableName = DefaultTableName) where T : class
        {
#if DotNet45 || NetStandard2
            using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#else
            await using (OleDbConnection dbConnection = new OleDbConnection(ToString()))
#endif
            {
                await dbConnection.OpenAsync();
                return await query<T>(dbConnection, $"select * from[{getFirstTableName(dbConnection, tableName)}]");
            }
        }
    }
}
