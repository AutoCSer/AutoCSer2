using System;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 扫描存档模式数据加载
    /// </summary>
    internal sealed class ScanPersistenceServiceLoader : ServiceLoader
    {
        /// <summary>
        /// 持久化文件信息
        /// </summary>
        private FileInfo persistenceFileInfo;
        /// <summary>
        /// 日志流持久化文件名称
        /// </summary>
        protected override string persistenceFileName { get { return persistenceFileInfo.FullName; } }
        /// <summary>
        /// 持久化回调异常位置文件信息
        /// </summary>
        private FileInfo persistenceCallbackExceptionPositionFileInfo;
        /// <summary>
        /// 持久化回调异常位置文件名称
        /// </summary>
        protected override string persistenceCallbackExceptionPositionFileName { get { return persistenceCallbackExceptionPositionFileInfo.FullName; } }
        /// <summary>
        /// 日志流持久化内存数据库服务端数据加载
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        internal ScanPersistenceServiceLoader(StreamPersistenceMemoryDatabaseService service) : base(service)
        {
            persistenceFileInfo = persistenceCallbackExceptionPositionFileInfo = service.PersistenceFileInfo;
        }
        /// <summary>
        /// 加载数据
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
