using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库服务端接口
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseServiceBase
    {
        /// <summary>
        /// 获取服务端 UTC 时间
        /// </summary>
        /// <returns></returns>
        DateTime GetUtcNow();
        /// <summary>
        /// 获取持久化流已当前写入位置
        /// </summary>
        /// <returns></returns>
        long GetPersistencePosition();
        /// <summary>
        /// 获取重建快照结束位置
        /// </summary>
        /// <returns>重建快照结束位置</returns>
        long GetRebuildSnapshotPosition();
        /// <summary>
        /// 获取所有匹配节点的全局关键字
        /// </summary>
        /// <param name="nodeInfo">匹配服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetNodeKeys(NodeInfo nodeInfo, CommandServerKeepCallbackCount<string> callback);
        /// <summary>
        /// 获取所有匹配节点的节点索引信息
        /// </summary>
        /// <param name="nodeInfo">匹配服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetNodeIndexs(NodeInfo nodeInfo, CommandServerKeepCallbackCount<NodeIndex> callback);
        /// <summary>
        /// 获取所有匹配节点的全局关键字与节点索引信息
        /// </summary>
        /// <param name="nodeInfo">匹配服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetNodeKeyIndexs(NodeInfo nodeInfo, CommandServerKeepCallbackCount<BinarySerializeKeyValue<string, NodeIndex>> callback);
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="callback"></param>
        void RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="callback"></param>
        void BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 检查持久化文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化流已写入位置，失败返回 -1</returns>
        long CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        long CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition);
    }
}
