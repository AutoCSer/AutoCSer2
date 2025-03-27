using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.Reflection;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库服务端接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(ServiceMethodEnum), MethodIndexEnumTypeCodeGeneratorPath = "", IsAutoMethodIndex = false, IsCodeGeneratorClientInterface = false)]
    public interface IStreamPersistenceMemoryDatabaseService
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
        /// 获取节点标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="isCreate">关键字不存在时创建空闲节点标识</param>
        /// <returns>关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        NodeIndex GetNodeIndex(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, string key, NodeInfo nodeInfo, bool isCreate);
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
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        void Call(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        void CallOutput(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        void CallInput(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        void CallInputOutput(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void KeepCallback(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void InputKeepCallback(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        void CallWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        void CallOutputWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        void CallInputWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        void CallInputOutputWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        CommandServerSendOnly SendOnlyWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void KeepCallbackWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void InputKeepCallbackWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback);
        /// <summary>
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        RebuildResult Rebuild(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue);
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
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="isBackup">是否备份客户端</param>
        /// <returns>从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        long CreateSlave(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, bool isBackup);
        /// <summary>
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <returns></returns>
        CommandServerSendOnly RemoveSlave(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp);
        /// <summary>
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="directory">修复方法目录信息</param>
        /// <param name="file">修复方法文件信息</param>
        /// <returns></returns>
        CommandServerSendOnly AppendRepairNodeMethodDirectoryFile(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file);
        /// <summary>
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback">获取修复节点方法信息委托</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetRepairNodeMethodPosition(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, CommandServerKeepCallback<RepairNodeMethodPosition> callback);
        /// <summary>
        /// 检查持久化文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化流已写入位置，失败返回 -1</returns>
        long CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPersistenceFile(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback);
        /// <summary>
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPersistenceCallbackExceptionPosition(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, CommandServerKeepCallback<long> callback);
        /// <summary>
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        long CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPersistenceCallbackExceptionPositionFile(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback);
    }
}
