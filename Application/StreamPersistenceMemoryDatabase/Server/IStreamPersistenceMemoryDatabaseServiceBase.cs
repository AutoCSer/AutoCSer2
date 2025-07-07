using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence in-memory database service interface
    /// 日志流持久化内存数据库服务接口
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseServiceBase
    {
        /// <summary>
        /// Get the server UTC time
        /// 获取服务端 UTC 时间
        /// </summary>
        /// <returns></returns>
        DateTime GetUtcNow();
        /// <summary>
        /// Get the current write location of the persistent stream
        /// 获取持久化流已当前写入位置
        /// </summary>
        /// <returns></returns>
        long GetPersistencePosition();
        /// <summary>
        /// Get the end position of the rebuild snapshot
        /// 获取重建快照结束位置
        /// </summary>
        /// <returns>The end position of the rebuild snapshot
        /// 重建快照结束位置</returns>
        long GetRebuildSnapshotPosition();
        /// <summary>
        /// Gets the global keyword for all matching nodes
        /// 获取所有匹配节点的全局关键字
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetNodeKeys(NodeInfo nodeInfo, CommandServerKeepCallbackCount<string> callback);
        /// <summary>
        /// Gets the node index information for all matching nodes
        /// 获取所有匹配节点的节点索引信息
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetNodeIndexs(NodeInfo nodeInfo, CommandServerKeepCallbackCount<NodeIndex> callback);
        /// <summary>
        /// Gets the global keyword and node index information of all matching nodes
        /// 获取所有匹配节点的全局关键字与节点索引信息
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetNodeKeyIndexs(NodeInfo nodeInfo, CommandServerKeepCallbackCount<BinarySerializeKeyValue<string, NodeIndex>> callback);
        /// <summary>
        /// Fix the interface method error and force overwriting the original interface method call. Except for the first parameter being the operation node object, the method definition must be consistent
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="methodName">The name of the repair method must be a static method. The first parameter must be the interface type of the operation node, and the method number must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="callback"></param>
        void RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// Bind a new method to dynamically add interface functionality. The initial state of the new method number must be free
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="methodName">The name of the repair method must be a static method. The first parameter must be the interface type of the operation node. The method number and other necessary configuration information must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="callback"></param>
        void BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// Check whether the header of the persistent file matches
        /// 检查持久化文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <returns>The persistent stream has been written to the location and returns -1 in case of failure
        /// 持久化流已写入位置，失败返回 -1</returns>
        long CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// Check whether the header of the persistent callback exception location file matches
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <returns>The written location of the persistent callback exception location file. Return -1 in case of failure
        /// 持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        long CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition);
    }
}
