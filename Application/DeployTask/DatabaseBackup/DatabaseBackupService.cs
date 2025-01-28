using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 数据库备份服务
    /// </summary>
    public abstract class DatabaseBackupService : IDatabaseBackupService
    {
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual Task OnException(Exception exception)
        {
            Console.WriteLine($"{AutoCSer.Threading.SecondTimer.Now.toString()} {exception.Message}");
            return AutoCSer.LogHelper.Exception(exception);
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
        /// 获取可备份数据库名称集合
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public abstract string[] GetDatabase(CommandServerCallQueue queue);
        /// <summary>
        /// 当前处理的数据库备份器
        /// </summary>
        protected readonly Dictionary<string, DatabaseBackuper> databaseBackupers = DictionaryCreator.CreateAny<string, DatabaseBackuper>();
        /// <summary>
        /// 备份数据库并返回文件名称
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database">数据库名称</param>
        /// <param name="callback">重写必须保证回调执行，返回空字符串表示没有找到数据库</param>
        public virtual void Backup(CommandServerCallQueue queue, string database, CommandServerCallback<string> callback)
        {
            var exception = default(Exception);
            var backuper = default(DatabaseBackuper);
            bool isCallback = true;
            try
            {
                if (databaseBackupers.TryGetValue(database, out backuper)) backuper.Callback(callback, ref isCallback);
                else
                {
                    backuper = createDatabaseBackuper(queue, database);
                    if (backuper != null)
                    {
                        databaseBackupers.Add(database, backuper);
                        backuper.Start(callback, ref isCallback);
                    }
                    else
                    {
                        callback.Callback(string.Empty);
                        isCallback = false;
                    }
                }
            }
            catch (Exception catchException)
            {
                AutoCSer.Threading.CatchTask.AddIgnoreException(OnException(exception = catchException));
            }
            finally
            {
                if (isCallback)
                {
                    callback.Callback(CommandClientReturnTypeEnum.ServerException, exception);
                    if (backuper != null && backuper.CallbackCount == 0) databaseBackupers.Remove(database);
                }
            }
        }
        /// <summary>
        /// 创建数据库备份器
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        protected abstract DatabaseBackuper createDatabaseBackuper(CommandServerCallQueue queue, string database);
        /// <summary>
        /// 移除数据库备份器
        /// </summary>
        /// <param name="database">数据库名称</param>
        public virtual void RemoveDatabaseBackuper(string database)
        {
            databaseBackupers.Remove(database);
        }
        /// <summary>
        /// 下载备份文件
        /// </summary>
        /// <param name="backupFullName">备份文件名称</param>
        /// <param name="startIndex">读取文件起始位置</param>
        /// <param name="callback">下载文件数据回调委托</param>
        /// <returns></returns>
        public virtual async Task Download(string backupFullName, long startIndex, CommandServerKeepCallbackCount<DatabaseBackupDownloadBuffer> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Unknown;
            var exception = default(Exception);
            try
            {
#if NetStandard21
                await using (FileStream fileStream = File.OpenRead(backupFullName))
#else
                using (FileStream fileStream = File.OpenRead(backupFullName))
#endif
                {
                    if (startIndex != fileStream.Length)
                    {
                        await AutoCSer.Common.Seek(fileStream, startIndex, SeekOrigin.Begin);
                        byte[][] buffers = new byte[][] { AutoCSer.Common.GetUninitializedArray<byte>(1 << 20), AutoCSer.Common.GetUninitializedArray<byte>(1 << 20), AutoCSer.Common.GetUninitializedArray<byte>(1 << 20) };
                        int bufferIndex = 0;
                        do
                        {
                            byte[] buffer = buffers[bufferIndex];
                            int readSize = await fileStream.ReadAsync(buffer, 0, buffer.Length);
                            if (readSize == 0) break;
                            if (!await callback.CallbackAsync(new DatabaseBackupDownloadBuffer { Buffer = buffer, Size = readSize })) return;
                            if (readSize != buffer.Length) break;
                            if (++bufferIndex == buffers.Length) bufferIndex = 0;
                        }
                        while (true);
                    }
                }
                await callback.CallbackAsync(default(DatabaseBackupDownloadBuffer));
                returnType = CommandClientReturnTypeEnum.Success;
            }
            catch (Exception catchException)
            {
                returnType = CommandClientReturnTypeEnum.ServerException;
                await OnException(exception = catchException);
            }
            finally
            {
                callback.CancelKeep(returnType, exception);
                if (returnType == CommandClientReturnTypeEnum.Success)
                {
                    await AutoCSer.Common.TryDeleteFile(backupFullName);
                    OnMessage($"数据库备份文件下载完成并删除 {backupFullName}");
                }
                else OnMessage($"数据库备份文件下载失败 {backupFullName}");
            }
        }
        /// <summary>
        /// 获取可备份数据库表格名称集合
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        public abstract string[] GetTableName(CommandServerCallQueue queue, string database);
    }
}
