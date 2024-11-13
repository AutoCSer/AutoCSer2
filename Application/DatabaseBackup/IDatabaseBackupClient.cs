using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 数据库备份客户端接口
    /// </summary>
    public interface IDatabaseBackupClient
    {
        /// <summary>
        /// 获取可备份数据库名称集合
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand GetDatabase(Action<CommandClientReturnValue<string[]>> callback);
        /// <summary>
        /// 获取可备份数据库名称集合
        /// </summary>
        /// <returns></returns>
        ReturnCommand<string[]> GetDatabase();

        /// <summary>
        /// 备份数据库并返回文件名称
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="callback">备份文件名称，返回 null 表示没有找到数据库</param>
#if NetStandard21
        CallbackCommand Backup(string database, Action<CommandClientReturnValue<string?>> callback);
#else
        CallbackCommand Backup(string database, Action<CommandClientReturnValue<string>> callback);
#endif
        /// <summary>
        /// 备份数据库并返回文件名称
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns>备份文件名称，返回 null 表示没有找到数据库</returns>
#if NetStandard21
        ReturnCommand<string?> Backup(string database);
#else
        ReturnCommand<string> Backup(string database);
#endif

        /// <summary>
        /// 下载备份文件
        /// </summary>
        /// <param name="backupFullName">备份文件名称</param>
        /// <param name="startIndex">读取文件起始位置</param>
        /// <param name="callback">下载备份文件缓冲区</param>
        /// <returns></returns>
        KeepCallbackCommand Download(string backupFullName, long startIndex, Action<CommandClientReturnValue<DatabaseBackupDownloadBuffer>, KeepCallbackCommand> callback);
        /// <summary>
        /// 下载备份文件
        /// </summary>
        /// <param name="backupFullName">备份文件名称</param>
        /// <param name="startIndex">读取文件起始位置</param>
        /// <returns>下载备份文件缓冲区</returns>
        EnumeratorCommand<DatabaseBackupDownloadBuffer> Download(string backupFullName, long startIndex);

        /// <summary>
        /// 获取可备份数据库表格名称集合
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand GetTableName(string database, Action<CommandClientReturnValue<string[]>> callback);
        /// <summary>
        /// 获取可备份数据库表格名称集合
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        ReturnCommand<string[]> GetTableName(string database);
    }
}
