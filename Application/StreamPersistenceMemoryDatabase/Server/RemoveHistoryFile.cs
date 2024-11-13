using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 删除历史持久化文件
    /// </summary>
    public sealed class RemoveHistoryFile
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseServiceBase service;
        /// <summary>
        /// 持久化文件名称
        /// </summary>
        private readonly string persistenceFileName;
        /// <summary>
        /// 持久化文件目录
        /// </summary>
        private readonly DirectoryInfo directory;
        /// <summary>
        /// 重建持久化文件切换目录
        /// </summary>
#if NetStandard21
        private readonly DirectoryInfo? switchDirectory;
#else
        private readonly DirectoryInfo switchDirectory;
#endif
        /// <summary>
        /// 删除历史持久化文件
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public RemoveHistoryFile(StreamPersistenceMemoryDatabaseServiceBase service)
        {
            this.service = service;
            FileInfo persistenceFileInfo = service.PersistenceFileInfo;
            directory = service.PersistenceFileInfo.Directory.notNull();
            persistenceFileName = persistenceFileInfo.Name;
            if (!object.ReferenceEquals(service.PersistenceFileInfo, service.PersistenceSwitchFileInfo))
            {
                switchDirectory = service.PersistenceSwitchFileInfo.Directory;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        public async Task Remove()
        {
            DateTime removeTime = service.Config.GetRemoveHistoryFileTime();
            await remove(directory, removeTime);
            if (switchDirectory != null) await remove(switchDirectory, removeTime);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="removeTime"></param>
        /// <returns></returns>
        private async Task remove(DirectoryInfo directory, DateTime removeTime)
        {
            foreach (FileInfo file in await AutoCSer.Common.Config.DirectoryGetFiles(directory, "*.bak"))
            {
                if (file.LastWriteTimeUtc < removeTime && file.Name.StartsWith(persistenceFileName))
                {
                    try
                    {
                        await AutoCSer.Common.Config.DeleteFile(file);
                    }
                    catch (Exception exception)
                    {
                        await AutoCSer.LogHelper.Exception(exception, $"内存数据库历史文件 {file.FullName} 清理失败");
                    }
                }
            }
        }
        /// <summary>
        /// 定时删除文件
        /// </summary>
        /// <param name="runTimer"></param>
        /// <returns></returns>
        public async Task Remove(AutoCSer.Threading.TaskRunTimer runTimer)
        {
            while (!service.IsDisposed)
            {
                await runTimer.Delay();
                await Remove();
            }
        }
    }
}
