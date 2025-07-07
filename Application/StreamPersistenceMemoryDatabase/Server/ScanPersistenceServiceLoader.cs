using System;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Data loading in scan archive mode
    /// 扫描存档模式数据加载
    /// </summary>
    internal sealed class ScanPersistenceServiceLoader : ServiceLoader
    {
        /// <summary>
        /// Persistent file information
        /// 持久化文件信息
        /// </summary>
        private FileInfo persistenceFileInfo;
        /// <summary>
        /// The file name for log stream persistence
        /// 日志流持久化文件名称
        /// </summary>
        protected override string persistenceFileName { get { return persistenceFileInfo.FullName; } }
        /// <summary>
        /// Persistent callback exception location file information
        /// 持久化回调异常位置文件信息
        /// </summary>
        private FileInfo persistenceCallbackExceptionPositionFileInfo;
        /// <summary>
        /// The persistent callback exception location file name
        /// 持久化回调异常位置文件名称
        /// </summary>
        protected override string persistenceCallbackExceptionPositionFileName { get { return persistenceCallbackExceptionPositionFileInfo.FullName; } }
        /// <summary>
        /// Log stream persistence for in-memory database service data loading
        /// 日志流持久化内存数据库服务数据加载
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        internal ScanPersistenceServiceLoader(StreamPersistenceMemoryDatabaseService service) : base(service)
        {
            persistenceFileInfo = persistenceCallbackExceptionPositionFileInfo = service.PersistenceFileInfo;
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="persistenceFileInfo"></param>
        /// <param name="persistenceCallbackExceptionPositionFileInfo"></param>
        internal void Load(FileInfo persistenceFileInfo, FileInfo persistenceCallbackExceptionPositionFileInfo)
        {
            this.persistenceFileInfo = persistenceFileInfo;
            this.persistenceCallbackExceptionPositionFileInfo = persistenceCallbackExceptionPositionFileInfo;
            setBufferWait();
            Load();
        }
    }
}
