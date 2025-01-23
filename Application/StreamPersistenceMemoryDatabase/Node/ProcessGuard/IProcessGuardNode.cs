using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 进程守护节点接口（服务端需要以管理员身份运行，否则可能异常）
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsMethodParameterCreator = true)]
    public partial interface IProcessGuardNode
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ProcessGuardInfo value);
        /// <summary>
        /// 初始化添加待守护进程
        /// </summary>
        /// <param name="processInfo">进程信息</param>
        /// <returns>是否添加成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool GuardLoadPersistence(ProcessGuardInfo processInfo);
        /// <summary>
        /// 添加待守护进程
        /// </summary>
        /// <param name="processInfo">进程信息</param>
        /// <returns>是否添加成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Guard(ProcessGuardInfo processInfo);
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="processId">进程标识</param>
        /// <param name="startTime">进程启动时间</param>
        /// <param name="processName">进程名称</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Remove(int processId, DateTime startTime, string processName);
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="key">切换进程关键字</param>
        /// <param name="callback">切换进程回调</param>
        [ServerMethod(IsPersistence = false)]
        void Switch(string key, MethodCallback<bool> callback);
    }
}
