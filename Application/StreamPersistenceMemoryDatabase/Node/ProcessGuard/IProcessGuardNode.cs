using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Process daemon node interface (The server needs to run as an administrator; otherwise, an exception may occur)
    /// 进程守护节点接口（服务端需要以管理员身份运行，否则可能异常）
    /// </summary>
    [ServerNode(IsMethodParameterCreator = true)]
    public partial interface IProcessGuardNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ProcessGuardInfo value);
        /// <summary>
        /// Initialize and add the daemon process to be added (Initialize and load the persistent data)
        /// 初始化添加待守护进程（初始化加载持久化数据）
        /// </summary>
        /// <param name="processInfo">Process information
        /// 进程信息</param>
        /// <returns>Add failed and return false
        /// 添加失败返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool GuardLoadPersistence(ProcessGuardInfo processInfo);
        /// <summary>
        /// Add the process to be daemon
        /// 添加待守护进程
        /// </summary>
        /// <param name="processInfo">Process information
        /// 进程信息</param>
        /// <returns>Add failed and return false
        /// 添加失败返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Guard(ProcessGuardInfo processInfo);
        /// <summary>
        /// Delete the daemon process
        /// 删除被守护进程
        /// </summary>
        /// <param name="processId">Process identity
        /// 进程标识</param>
        /// <param name="startTime">Process startup time
        /// 进程启动时间</param>
        /// <param name="processName">Process name
        /// 进程名称</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Remove(int processId, DateTime startTime, string processName);
        /// <summary>
        /// Switch processes
        /// 切换进程
        /// </summary>
        /// <param name="key">The key words of the switched process
        /// 切换进程关键字</param>
        /// <param name="callback">Switch process callback
        /// 切换进程回调</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        void Switch(string key, MethodCallback<bool> callback);
    }
}
