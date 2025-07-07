using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库服务从节点配置
    /// </summary>
    public sealed class SlaveServiceConfig : StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// 同步失败重试间隔毫秒数，默认为 100，最小值为 1
        /// </summary>
        public int DelayMilliseconds = 100;
        /// <summary>
        /// 同步失败重试间隔
        /// </summary>
        internal TimeSpan DelayTimeSpan { get { return new TimeSpan(0, 0, 0, 0, Math.Max(DelayMilliseconds, 1)); } }

        /// <summary>
        /// 日志流持久化内存数据库备份
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        public async Task<Backuper> Create(IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient, CommandClientSocket socket)
        {
            Backuper backuper = new Backuper(this, masterClient);
            socket.SessionObject = backuper;
            await backuper.Load();
            return backuper;
        }
        /// <summary>
        /// 日志流持久化内存数据库服务（从服务节点）
        /// </summary>
        /// <typeparam name="T">Node service interface type
        /// 节点服务接口类型</typeparam>
        /// <param name="createServiceNode"></param>
        /// <param name="masterClient"></param>
        /// <returns></returns>
        public async Task<StreamPersistenceMemoryDatabaseService> Create<T>(Func<StreamPersistenceMemoryDatabaseService, T> createServiceNode, IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient)
            where T : class, IServiceNode
        {
            SlaveService slaveService = new SlaveService(this, service => ServiceNode.CreateServiceNode(service, createServiceNode(service)), masterClient);
            await slaveService.Load();
            return slaveService;
        }
    }
}
