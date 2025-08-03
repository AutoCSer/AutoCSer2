using AutoCSer.Extensions.Threading;
using AutoCSer.Net;
using AutoCSer.Threading;
using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DatabaseBackup
{
    /// <summary>
    /// 数据库备份服务
    /// </summary>
    internal sealed class DatabaseBackupService : AutoCSer.CommandService.DeployTask.DatabaseBackupService
    {
        /// <summary>
        /// 数据库备份服务
        /// </summary>
        internal DatabaseBackupService()
        {
            DeleteFile().AutoCSerNotWait();
        }
        /// <summary>
        /// 删除历史文件
        /// </summary>
        /// <returns></returns>
        private async Task DeleteFile()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            ConfigFile configFile = ConfigFile.Default;
            TaskRunTimer taskRunTimer = new TaskRunTimer(60.0 * 60);
            do
            {
                await taskRunTimer.Delay();
                try
                {
                    foreach (Database database in configFile.DatabaseArray)
                    {
                        DirectoryInfo directory = new DirectoryInfo(Path.Combine(configFile.BackupPath, database.Name));
                        if (await AutoCSer.Common.DirectoryExists(directory))
                        {
                            foreach (FileInfo fileInfo in await AutoCSer.Common.DirectoryGetFiles(directory, "*.bak"))
                            {
                                if (fileInfo.CreationTimeUtc.AddHours(configFile.DeleteFileHours) <= SecondTimer.UtcNow)
                                {
                                    try
                                    {
                                        await AutoCSer.Common.TryDeleteFile(fileInfo.FullName);
                                    }
                                    catch (Exception exception)
                                    {
                                        ConsoleWriteQueue.WriteLine($"备份文件 {fileInfo.FullName} 删除失败 {exception.Message}", ConsoleColor.Red);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ConsoleWriteQueue.WriteLine(exception.Message, ConsoleColor.Red);
                    await AutoCSer.LogHelper.Exception(exception);
                }
            }
            while (true);
        }
        /// <summary>
        /// 获取可备份数据库名称集合
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public override string[] GetDatabase(CommandServerCallQueue queue)
        {
            return ConfigFile.Default.DatabaseArray.AutoCSerCollectionExtensions().GetArray(p => p.Name);
        }
        /// <summary>
        /// 创建数据库备份器
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        protected override AutoCSer.CommandService.DeployTask.DatabaseBackuper createDatabaseBackuper(CommandServerCallQueue queue, string database)
        {
            foreach (Database nextDatabase in ConfigFile.Default.DatabaseArray)
            {
                if (nextDatabase.Name == database) return new DatabaseBackuper(this, queue, nextDatabase);
            }
            return null;
        }
        /// <summary>
        /// 获取可备份数据库表格名称集合
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public override string[] GetTableName(CommandServerCallQueue queue, string database)
        {
            return null;
        }
    }
}
