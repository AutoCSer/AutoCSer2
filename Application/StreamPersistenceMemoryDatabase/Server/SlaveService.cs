using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库从节点服务端
    /// </summary>
    internal class SlaveService : StreamPersistenceMemoryDatabaseService, ISlaveLoader
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
#if NetStandard21
        private SlaveLoader? loader;
#else
        private SlaveLoader loader;
#endif
        /// <summary>
        /// 是否已经启动数据加载
        /// </summary>
        private int isLoad;
        /// <summary>
        /// 日志流持久化内存数据库从节点服务端
        /// </summary>
        /// <param name="config">日志流持久化内存数据库服务端配置</param>
        /// <param name="createServiceNode">创建服务基础操作节点委托</param>
        /// <param name="masterClient">主节点客户端</param>
        internal SlaveService(SlaveServiceConfig config, Func<StreamPersistenceMemoryDatabaseService, ServerNode> createServiceNode, IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient) : base(config, createServiceNode, false)
        {
            this.masterClient = masterClient;
            delayTimeSpan = config.DelayTimeSpan;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                base.Dispose();
                loader?.Close();
            }
        }
        /// <summary>
        /// 获取节点标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="isCreate">关键字不存在时创建空闲节点标识</param>
        /// <returns>关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        public override NodeIndex GetNodeIndex(CommandServerSocket socket, CommandServerCallQueue queue, string key, NodeInfo nodeInfo, bool isCreate)
        {
            return new NodeIndex(CallStateEnum.OnlyMaster);
        }
        /// <summary>
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public override RebuildResult Rebuild(CommandServerSocket socket, CommandServerCallQueue queue)
        {
            return new RebuildResult(CallStateEnum.OnlyMaster);
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override void RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback)
        {
            callback.Callback(CallStateEnum.OnlyMaster);
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override void BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback)
        {
            callback.Callback(CallStateEnum.OnlyMaster);
        }
        /// <summary>
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="isBackup">是否备份客户端</param>
        /// <returns>从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        public override long CreateSlave(CommandServerSocket socket, CommandServerCallQueue queue, bool isBackup)
        {
            return -(long)(byte)CallStateEnum.OnlyMaster;
        }
        /// <summary>
        /// 开始加载数据
        /// </summary>
        /// <returns></returns>
        internal new Task Load()
        {
            if (Interlocked.CompareExchange(ref isLoad, 1, 0) == 0)
            {
                loader = new SlaveLoader(this, masterClient);
                return loader.Load();
            }
            return AutoCSer.Common.CompletedTask;
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
                        else Dispose();
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