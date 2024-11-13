using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 数据库备份客户端文件下载器
    /// </summary>
    public class DatabaseBackupClientDownloader
    {
        /// <summary>
        /// 数据库备份客户端
        /// </summary>
        private readonly DatabaseBackupClient client;
        /// <summary>
        /// 数据库名称
        /// </summary>
        private readonly string database;
        /// <summary>
        /// 备份文件名称
        /// </summary>
        private readonly string backupFullName;
        /// <summary>
        /// 备份文件名称
        /// </summary>
        private readonly string backupFileName;
        /// <summary>
        /// 连续错误尝试次数
        /// </summary>
        private readonly int tryErrorCount;

        /// <summary>
        /// 数据库备份客户端文件下载器
        /// </summary>
        /// <param name="client">数据库备份客户端</param>
        /// <param name="database">数据库名称</param>
        /// <param name="backupFullName">备份文件名称</param>
        /// <param name="backupFileName">备份文件名称</param>
        /// <param name="tryErrorCount">连续错误尝试次数</param>
#if NetStandard21
        public DatabaseBackupClientDownloader(DatabaseBackupClient client, string database, string backupFullName, string? backupFileName = null, int tryErrorCount = 2)
#else
        public DatabaseBackupClientDownloader(DatabaseBackupClient client, string database, string backupFullName, string backupFileName = null, int tryErrorCount = 2)
#endif
        {
            this.client = client;
            this.database = database;
            this.backupFullName = backupFullName;
            this.backupFileName = backupFileName ?? new FileInfo(backupFullName).Name;
            this.tryErrorCount = tryErrorCount;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        internal async Task Download()
        {
            client.OnMessage($"开始下载数据库 {database} 备份文件 {backupFileName}");
            bool isCompleted = false;
            try
            {
                int tryErrorCount = this.tryErrorCount;
                await AutoCSer.Common.Config.TryCreateDirectory(new FileInfo(backupFileName).Directory.notNull());
                do
                {
                    int lastBufferSize = 0;
#if NetStandard21
                    await using (FileStream fileStream = await AutoCSer.Common.Config.CreateFileStream(backupFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 1 << 20, FileOptions.None))
#else
                    using (FileStream fileStream = await AutoCSer.Common.Config.CreateFileStream(backupFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 1 << 20, FileOptions.None))
#endif
                    {
                        await AutoCSer.Common.Config.Seek(fileStream, 0, SeekOrigin.End);

                        var downloadCommand = await client.Client.DatabaseBackupClient.Download(backupFullName, fileStream.Length);
                        if (downloadCommand != null)
                        {
                            while (await downloadCommand.MoveNext())
                            {
                                DatabaseBackupDownloadBuffer buffer = downloadCommand.Current;
                                if (buffer.Size == 0)
                                {
                                    isCompleted = true;
                                    client.OnMessage($"数据库 {database} 备份文件下载完毕 {fileStream.Length.toString()}");
                                    return;
                                }
                                await fileStream.WriteAsync(buffer.Buffer, 0, lastBufferSize = buffer.Size);
                            }
                        }
                    }
                    if (lastBufferSize == 0) --tryErrorCount;
                    else tryErrorCount = this.tryErrorCount;
                    if (tryErrorCount <= 0) await client.OnError($"数据库 {database} 备份文件下载失败");
                    else
                    {
                        await client.OnError($"数据库 {database} 备份文件下载失败，剩余重试次数 {tryErrorCount}");
                        await Task.Delay(1000);
                        if (await client.CommandClient.GetSocketAsync() == null)
                        {
                            await Task.Delay(1000);
                            await client.CommandClient.GetSocketAsync();
                        }
                    }
                }
                while (tryErrorCount > 0);
            }
            finally { await deleteFile(isCompleted); }
        }
        /// <summary>
        /// 删除历史文件
        /// </summary>
        /// <param name="isCompleted">当前文件是否下载成功</param>
        /// <returns></returns>
        protected virtual async Task deleteFile(bool isCompleted)
        {
            if (!isCompleted)
            {
                try
                {
                    await AutoCSer.Common.Config.TryDeleteFile(backupFileName);
                }
                catch (Exception exception)
                {
                    await client.OnError($"备份文件 {backupFileName} 删除失败 {exception.Message}");
                }
            }
            ;
            foreach (FileInfo FileInfo in (await AutoCSer.Common.Config.DirectoryGetFiles(new FileInfo(backupFileName).Directory.notNull(), "*.bak"))
                .OrderByDescending(p => p.CreationTime).Skip(2))
            {
                try
                {
                    await AutoCSer.Common.Config.TryDeleteFile(FileInfo.FullName);
                }
                catch (Exception exception)
                {
                    await client.OnError($"备份文件 {FileInfo.FullName} 删除失败 {exception.Message}");
                }
            }
        }
    }
}
