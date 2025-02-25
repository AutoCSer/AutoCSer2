﻿using AutoCSer.CommandService.DeployTask;
using AutoCSer.Net;
using System;
using System.IO;

namespace AutoCSer.TestCase.DatabaseBackupClient
{
    /// <summary>
    /// 数据库备份客户端
    /// </summary>
    internal sealed class DatabaseBackupClient : AutoCSer.CommandService.DeployTask.DatabaseBackupClient
    {
        /// <summary>
        /// 数据库备份客户端
        /// </summary>
        /// <param name="commandClient">命令客户端</param>
        /// <param name="client">数据库备份客户端套接字事件</param>
        public DatabaseBackupClient(CommandClient commandClient, IDatabaseBackupClientSocketEvent client) : base(commandClient, client) { }
        /// <summary>
        /// 数据库备份客户端文件下载器
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="backupFullName">备份文件名称</param>
        /// <returns></returns>
        protected override DatabaseBackupClientDownloader getDatabaseBackupClientDownloader(string database, string backupFullName)
        {
            return new DatabaseBackupClientDownloader(this, database, backupFullName, Path.Combine(ConfigFile.Default.BackupPath, database, new FileInfo(backupFullName).Name));
        }
        /// <summary>
        /// 获取数据库表格备份操作对象
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected override DatabaseBackupClientTable getDatabaseBackupClientTable(string database, string tableName)
        {
            return null;
        }
    }
}
