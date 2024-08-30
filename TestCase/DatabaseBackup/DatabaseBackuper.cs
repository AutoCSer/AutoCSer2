using AutoCSer.Net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace AutoCSer.TestCase.DatabaseBackup
{
    /// <summary>
    /// 数据库备份器
    /// </summary>
    internal sealed class DatabaseBackuper : AutoCSer.CommandService.DatabaseBackuper
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private new readonly Database database;
        /// <summary>
        /// 备份超时
        /// </summary>
        protected override int commandTimeout { get { return 60 * ConfigFile.Default.CommandTimeoutMinutes; } }
        /// <summary>
        /// 数据库备份器
        /// </summary>
        /// <param name="databaseBackup"></param>
        /// <param name="queue"></param>
        /// <param name="database"></param>
        public DatabaseBackuper(AutoCSer.CommandService.DatabaseBackupService databaseBackup, CommandServerCallQueue queue, Database database)
            : base(databaseBackup, queue, database.Name
                  , Path.Combine(ConfigFile.Default.BackupPath, database.Name, $@"{database.Name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak")
                  //, Path.Combine(ConfigFile.Default.BackupPath, database.Name, $@"{database.Name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.7z")
                  )
        {
            this.database = database;
        }
        /// <summary>
        /// 备份数据库，失败返回异常
        /// </summary>
        protected override void backup()
        {
            using (SqlConnection dbConnection = new SqlConnection(database.ConnectionString))
            {
                dbConnection.Open();
                if (shrinkLogSize > 0)
                {
                    LeftArray<string> names = new LeftArray<string>(0);
                    using (SqlCommand command = new SqlCommand(getLogNameSqlServer(), dbConnection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (reader.Read()) names.Add(reader.GetString(0));
                        }
                    }
                    if (names.Count != 0)
                    {
                        using (SqlCommand command = new SqlCommand(getRecoverySimpleSqlServer(), dbConnection))
                        {
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                        foreach(string name in names)
                        {
                            using (SqlCommand command = new SqlCommand(getShrinkLogSqlServer(name), dbConnection))
                            {
                                command.CommandType = CommandType.Text;
                                command.ExecuteNonQuery();
                            }
                        }
                        using (SqlCommand command = new SqlCommand(getRecoveryFullSqlServer(), dbConnection))
                        {
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand(getBackupSqlServer(), dbConnection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = commandTimeout;
                    command.ExecuteNonQuery();
                }
            }
        }
        ///// <summary>
        ///// 压缩数据库备份文件，比如可以引用 SevenZipSharp 压缩成 7z 文件
        ///// </summary>
        //protected override void compression()
        //{
        //    SevenZip.SevenZipBase.SetLibraryPath("7z.dll");
        //    FileInfo backupFileInfo = new FileInfo(backupFullName);
        //    using (FileStream compressionFileStream = File.OpenWrite(compressionFullName))
        //    {
        //        SevenZip.SevenZipCompressor compressor = new SevenZip.SevenZipCompressor();
        //        compressor.CompressionLevel = SevenZip.CompressionLevel.Ultra;
        //        compressor.CompressFilesEncrypted(compressionFileStream, ConfigFile.Default.VerifyString, backupFullName);
        //    }
        //    backupFileInfo.Delete();
        //}
    }
}
