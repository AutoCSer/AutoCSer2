using AutoCSer.Extensions;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Delete the historical persistence file
    /// 删除历史持久化文件
    /// </summary>
    public class RemoveHistoryFile
    {
        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
#if NetStandard21
        private volatile StreamPersistenceMemoryDatabaseServiceBase? service;
#else
        private volatile StreamPersistenceMemoryDatabaseServiceBase service;
#endif
        /// <summary>
        /// The name of the persistent file
        /// 持久化文件名称
        /// </summary>
        private readonly string persistenceFileName;
        /// <summary>
        /// The directory of the persistent file
        /// 持久化文件目录
        /// </summary>
        private readonly DirectoryInfo directory;
        /// <summary>
        /// Switch the directory of the files that have been persistently rebuild
        /// 持久化重建文件切换目录
        /// </summary>
#if NetStandard21
        private readonly DirectoryInfo? switchDirectory;
#else
        private readonly DirectoryInfo switchDirectory;
#endif
        /// <summary>
        /// Delete the historical persistence file
        /// 删除历史持久化文件
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
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
        /// Delete the file
        /// 删除文件
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        internal async Task Remove(StreamPersistenceMemoryDatabaseServiceBase service)
        {
            DateTime removeTime = service.Config.GetRemoveHistoryFileTime();
            await remove(directory, removeTime);
            if (switchDirectory != null) await remove(switchDirectory, removeTime);
        }
        /// <summary>
        /// Delete the file
        /// 删除文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="removeTime"></param>
        /// <returns></returns>
        private async Task remove(DirectoryInfo directory, DateTime removeTime)
        {
            foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(directory, "*.bak"))
            {
                if (file.LastWriteTimeUtc < removeTime && file.Name.StartsWith(persistenceFileName))
                {
                    try
                    {
                        await AutoCSer.Common.DeleteFile(file);
                    }
                    catch (Exception exception)
                    {
                        await AutoCSer.LogHelper.Exception(exception, $"内存数据库历史文件 {file.FullName} 清理失败");
                    }
                }
            }
        }
        /// <summary>
        /// Delete files regularly
        /// 定时删除文件
        /// </summary>
        /// <param name="runTimer"></param>
        /// <returns></returns>
        public async Task Remove(AutoCSer.Threading.TaskRunTimer runTimer)
        {
            var service = this.service;
            if (service == null || service.IsDisposed) return;
            if (service.Set(this))
            {
                do
                {
                    service = this.service;
                    if (service == null || service.IsDisposed) return;
                    await runTimer.Delay();
                    await Remove(service);
                }
                while (true);
            }
        }
        /// <summary>
        /// Cancel the task
        /// 取消任务
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            service = null;
        }
    }
}
