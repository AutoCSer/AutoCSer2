using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence In-memory database service interface (supporting concurrent read operations)
    /// 日志流持久化内存数据库服务接口（支持并发读取操作）
    /// </summary>
    [AutoCSer.Net.CommandServer.ServerControllerInterface(typeof(IStreamPersistenceMemoryDatabaseServiceMethodEnum))]
    [AutoCSer.Net.CommandServerControllerInterface(IsCodeGeneratorMethodEnum = false, IsCodeGeneratorClientInterface = false)]
    public interface IReadWriteQueueService : IStreamPersistenceMemoryDatabaseServiceBase
    {
        /// <summary>
        /// Get node identity
        /// 获取节点标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="isCreate">Create a free node identity when the keyword does not exist
        /// 关键字不存在时创建空闲节点标识</param>
        /// <returns>When the keyword does not exist, return an free node identifier for creating the node
        /// 关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        NodeIndex GetNodeIndex(CommandServerSocket socket, CommandServerCallWriteQueue queue, string key, NodeInfo nodeInfo, bool isCreate);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        void Call(CommandServerSocket socket, CommandServerCallReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        void CallOutput(CommandServerSocket socket, CommandServerCallReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        void CallInput(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        void CallInputOutput(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void KeepCallback(CommandServerSocket socket, CommandServerCallReadQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void InputKeepCallback(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        void CallWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        void CallOutputWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        void CallInputWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        void CallInputOutputWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        CommandServerSendOnly SendOnlyWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void KeepCallbackWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void InputKeepCallbackWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// Rebuild the persistent file (clear invalid data), and note that nodes that do not support snapshots will be discarded
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        RebuildResult Rebuild(CommandServerSocket socket, CommandServerCallWriteQueue queue);
        /// <summary>
        /// Create a slave node
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="isBackup">Is the backup client
        /// 是否备份客户端</param>
        /// <returns>Verify the timestamp from the node, and a negative number represents the CallStateEnum error status
        /// 从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        long CreateSlave(CommandServerSocket socket, CommandServerCallWriteQueue queue, bool isBackup);
        /// <summary>
        /// Remove the information from the node client
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp from create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <returns></returns>
        CommandServerSendOnly RemoveSlave(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp);
        /// <summary>
        /// Add the directory and file information of the repair method from the slave node
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="directory">Directory information of the repair method
        /// 修复方法目录信息</param>
        /// <param name="file">File information of the repair method
        /// 修复方法文件信息</param>
        /// <returns></returns>
        CommandServerSendOnly AppendRepairNodeMethodDirectoryFile(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file);
        /// <summary>
        /// Get the repair node method information from slave node
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback">The callback delegate for get the method information of the repair node
        /// 获取修复节点方法信息回调委托</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetRepairNodeMethodPosition(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, CommandServerKeepCallback<RepairNodeMethodPosition> callback);
        /// <summary>
        /// Get the persistent file data
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPersistenceFile(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback);
        /// <summary>
        /// Get the location data of the persistent callback exception
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPersistenceCallbackExceptionPosition(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, CommandServerKeepCallback<long> callback);
        /// <summary>
        /// Get the file data of the persistent callback exception location
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPersistenceCallbackExceptionPositionFile(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback);
    }
}
