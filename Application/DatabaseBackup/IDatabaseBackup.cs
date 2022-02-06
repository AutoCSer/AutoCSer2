using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 数据库备份服务接口
    /// </summary>
    public interface IDatabaseBackup
    {
        /// <summary>
        /// 获取可备份数据库名称集合
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        string[] GetDatabase(CommandServerCallQueue queue);
        /// <summary>
        /// 备份数据库并返回文件名称
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database">数据库名称</param>
        /// <param name="callback">重写必须保证回调执行</param>
        void Backup(CommandServerCallQueue queue, string database, CommandServerCallback<string> callback);
        /// <summary>
        /// 下载备份文件
        /// </summary>
        /// <param name="backupFullName">备份文件名称</param>
        /// <param name="startIndex">读取文件起始位置</param>
        /// <param name="callback">下载文件数据回调委托</param>
        /// <returns></returns>
        [CommandServerMethod(KeepCallbackOutputCount = 2)]
        Task Download(string backupFullName, long startIndex, CommandServerKeepCallbackCount<DatabaseBackupDownloadBuffer> callback);
        /// <summary>
        /// 获取可备份数据库表格名称集合
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        string[] GetTableName(CommandServerCallQueue queue, string database);
    }
}
