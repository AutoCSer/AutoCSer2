using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库备份
    /// </summary>
    public sealed class Backuper : CommandServerSocketSessionObjectService, ISlaveLoader, IDisposable
    {
        /// <summary>
        /// 主节点客户端
        /// </summary>
        private readonly IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient;
        /// <summary>
        /// 同步失败重试间隔
        /// </summary>
        private readonly TimeSpan delayTimeSpan;
        /// <summary>
        /// 日志流持久化内存数据库从节点服务数据加载
        /// </summary>
        private SlaveLoader loader;
        /// <summary>
        /// 是否已经启动数据加载
        /// </summary>
        private int isLoad;
        /// <summary>
        /// 是否备份客户端
        /// </summary>
        internal override bool IsBackup { get { return true; } }
        /// <summary>
        /// 日志流持久化内存数据库备份
        /// </summary>
        /// <param name="config">日志流持久化内存数据库服务端配置</param>
        /// <param name="masterClient">主节点客户端</param>
        internal Backuper(SlaveServiceConfig config, IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient) : base(config, false)
        {
            this.masterClient = masterClient;
            delayTimeSpan = config.DelayTimeSpan;

            DirectoryInfo directory = PersistenceFileInfo.Directory;
            if (!directory.Exists) directory.Create();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                loader?.Close();
            }
        }
        /// <summary>
        /// 开始加载数据
        /// </summary>
        /// <returns></returns>
        internal async Task Load()
        {
            if (Interlocked.CompareExchange(ref isLoad, 1, 0) == 0)
            {
                loader = new SlaveLoader(this, masterClient);
                await loader.Load();
            }
        }
        /// <summary>
        /// 关闭数据加载
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="isRetry"></param>
        internal override void CloseLoader(SlaveLoader loader, bool isRetry)
        {
            if (Interlocked.CompareExchange(ref this.loader, null, loader) == loader)
            {
                try
                {
                    loader.Close();
                }
                finally
                {
                    if (!IsDisposed)
                    {
                        if (isRetry) AutoCSer.Threading.CatchTask.AddIgnoreException(delayLoad());
                        else IsDisposed = true;
                    }
                }
            }
        }
        /// <summary>
        /// 重试加载数据
        /// </summary>
        /// <returns></returns>
        private async Task delayLoad()
        {
            if (!IsDisposed)
            {
                await Task.Delay(delayTimeSpan);
                if (!IsDisposed)
                {
                    loader = new SlaveLoader(this, masterClient);
                    await loader.Load();
                }
            }
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        void ISlaveLoader.GetPersistenceCallbackExceptionPositionFile(long position, ref SubArray<byte> buffer)
        {
            loader?.GetPersistenceCallbackExceptionPositionFile(position, ref buffer);
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        void ISlaveLoader.GetPersistenceFile(long position, ref SubArray<byte> buffer)
        {
            loader?.GetPersistenceFile(position, ref buffer);
        }
    }
}
