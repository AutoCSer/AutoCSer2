using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 数据库备份客户端
    /// </summary>
    public abstract class DatabaseBackupClient
    {
        /// <summary>
        /// 命令客户端
        /// </summary>
        public readonly CommandClient CommandClient;
        /// <summary>
        /// 数据库备份客户端套接字事件
        /// </summary>
        public readonly IDatabaseBackupClientSocketEvent Client;
        /// <summary>
        /// 数据库备份客户端
        /// </summary>
        /// <param name="commandClient">命令客户端</param>
        /// <param name="client">数据库备份客户端套接字事件</param>
        public DatabaseBackupClient(CommandClient commandClient, IDatabaseBackupClientSocketEvent client)
        {
            CommandClient = commandClient;
            Client = client;
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task OnError(string message)
        {
            Console.WriteLine($"{AutoCSer.Threading.SecondTimer.Now.toString()} {message}");
            await AutoCSer.LogHelper.Error(message);
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message"></param>
        public virtual void OnMessage(string message)
        {
            Console.WriteLine($"{AutoCSer.Threading.SecondTimer.Now.toString()} {message}");
        }
        /// <summary>
        /// 开始备份数据库
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            CommandClientReturnValue<string[]> databases = await Client.DatabaseBackupClient.GetDatabase();
            if (!databases.IsSuccess)
            {
                await OnError($"数据库名称获取失败 {databases.ReturnType} {databases.ErrorMessage}");
                return;
            }
            LeftArray<Task> downloadTasks = new LeftArray<Task>(databases.Value.Length);
            foreach (string database in databases.Value)
            {
                if (checkDatabase(database))
                {
                    OnMessage($"开始备份数据库 {database}");
                    var backupFullName = await Client.DatabaseBackupClient.Backup(database);
                    if (backupFullName.IsSuccess)
                    {
                        if (!string.IsNullOrEmpty(backupFullName.Value))
                        {
                            downloadTasks.Add(getDatabaseBackupClientDownloader(database, backupFullName.Value).Download());
                        }
                        else await OnError($"没有找到数据库 {database}");
                    }
                    else await OnError($"数据库 {database} 备份失败 {backupFullName.ReturnType} {backupFullName.ErrorMessage}");
                }
            }
            foreach (Task downloadTask in downloadTasks) await downloadTask;
        }
        /// <summary>
        /// 检查数据库是否需要备份
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        protected virtual bool checkDatabase(string database) { return true; }
        /// <summary>
        /// 数据库备份客户端文件下载器
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="backupFullName">备份文件名称</param>
        /// <returns></returns>
        protected virtual DatabaseBackupClientDownloader getDatabaseBackupClientDownloader(string database, string backupFullName)
        {
            return new DatabaseBackupClientDownloader(this, database, backupFullName);
        }

        /// <summary>
        /// 开始备份数据库表格
        /// </summary>
        /// <returns></returns>
        public async Task BackupTable()
        {
            CommandClientReturnValue<string[]> databases = await Client.DatabaseBackupClient.GetDatabase();
            if (!databases.IsSuccess)
            {
                await OnError($"数据库名称获取失败 {databases.ReturnType} {databases.ErrorMessage}");
                return;
            }
            foreach (string database in databases.Value)
            {
                if (checkDatabase(database))
                {
                    OnMessage($"开始备份数据库 {database}");
                    CommandClientReturnValue<string[]> tableNames = await Client.DatabaseBackupClient.GetTableName(database);
                    if (tableNames.IsSuccess)
                    {
                        if (tableNames.Value != null)
                        {
                            foreach (string tableName in tableNames.Value)
                            {
                                await getDatabaseBackupClientTable(database, tableName).Backup();
                            }
                        }
                        else await OnError($"数据库 {database} 没有找到可备份表格");
                    }
                    else await OnError($"数据库 {database} 获取备份表格失败 {tableNames.ReturnType} {tableNames.ErrorMessage}");
                }
            }
        }
        /// <summary>
        /// 获取数据库表格备份操作对象
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="tableName">数据库表格名称</param>
        /// <returns></returns>
        protected abstract DatabaseBackupClientTable getDatabaseBackupClientTable(string database, string tableName);
    }
}
