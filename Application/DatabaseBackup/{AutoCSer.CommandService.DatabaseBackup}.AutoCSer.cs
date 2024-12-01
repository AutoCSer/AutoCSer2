//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 数据库备份服务接口 客户端接口
        /// </summary>
        public partial interface IDatabaseBackupServiceClientController
        {
            /// <summary>
            /// 备份数据库并返回文件名称
            /// </summary>
            /// <param name="database">数据库名称</param>
            /// <returns>重写必须保证回调执行，返回空字符串表示没有找到数据库</returns>
            AutoCSer.Net.ReturnCommand<string> Backup(string database);
            /// <summary>
            /// 下载备份文件
            /// </summary>
            /// <param name="backupFullName">备份文件名称</param>
            /// <param name="startIndex">读取文件起始位置</param>
            /// <returns>下载文件数据回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DatabaseBackupDownloadBuffer> Download(string backupFullName, long startIndex);
            /// <summary>
            /// 获取可备份数据库名称集合
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string[]> GetDatabase();
            /// <summary>
            /// 获取可备份数据库表格名称集合
            /// </summary>
            /// <param name="database">数据库名称</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string[]> GetTableName(string database);
        }
}
#endif