using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端接口转换封装
    /// </summary>
    public sealed class StreamPersistenceMemoryDatabaseTaskClient : IStreamPersistenceMemoryDatabaseClient
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        private readonly IStreamPersistenceMemoryDatabaseTaskClient client;
        /// <summary>
        /// 日志流持久化内存数据库客户端接口转换封装
        /// </summary>
        /// <param name="client">日志流持久化内存数据库客户端接口</param>
        public StreamPersistenceMemoryDatabaseTaskClient(IStreamPersistenceMemoryDatabaseTaskClient client)
        {
            this.client = client;
        }
        /// <summary>
        /// 创建会话对象，用于反序列化时获取服务信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand CreateSessionObject()
        {
            return client.CreateSessionObject();
        }
        /// <summary>
        /// 获取服务端 UTC 时间
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<DateTime> GetUtcNow()
        {
            return client.GetUtcNow();
        }
        /// <summary>
        /// 获取节点标识
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<NodeIndex> GetNodeIndex(string key, NodeInfo nodeInfo)
        {
            return client.GetNodeIndex(key, nodeInfo);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand SendOnly(RequestParameter parameter)
        {
            return client.SendOnly(parameter);
        }
        /// <summary>
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<RebuildResult> Rebuild()
        {
            return client.Rebuild();
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<CallStateEnum> RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName)
        {
            return client.RepairNodeMethod(index, rawAssembly, methodName);
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<CallStateEnum> BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName)
        {
            return client.BindNodeMethod(index, rawAssembly, methodName);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<CallStateEnum> Call(NodeIndex index, int methodIndex)
        {
            return client.Call(index, methodIndex);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<ResponseParameter> CallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex)
        {
            return client.CallOutput(returnValue, index, methodIndex);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<CallStateEnum> CallInput(RequestParameter parameter)
        {
            return client.CallInput(parameter);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<ResponseParameter> CallInputOutput(ResponseParameter returnValue, RequestParameter parameter)
        {
            return client.CallInputOutput(returnValue, parameter);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorCommand<KeepCallbackResponseParameter> KeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex)
        {
            return client.KeepCallback(returnValue, index, methodIndex);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorCommand<KeepCallbackResponseParameter> InputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter)
        {
            return client.InputKeepCallback(returnValue, parameter);
        }
        /// <summary>
        /// 获取持久化流已当前写入位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> GetPersistencePosition()
        {
            return client.GetPersistencePosition();
        }
        /// <summary>
        /// 创建备份
        /// </summary>
        /// <param name="isBackup">是否备份客户端</param>
        /// <returns>备份验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> CreateSlave(bool isBackup)
        {
            return client.CreateSlave(isBackup);
        }
        /// <summary>
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand RemoveSlave(long timestamp)
        {
            return client.RemoveSlave(timestamp);
        }
        /// <summary>
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="directory">修复方法目录信息</param>
        /// <param name="file">修复方法文件信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand AppendRepairNodeMethodDirectoryFile(long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file)
        {
            return client.AppendRepairNodeMethodDirectoryFile(timestamp, directory, file);
        }
        /// <summary>
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="timestamp">创建备份客户端信息时间戳</param>
        /// <param name="callback">修复节点方法信息回调委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetRepairNodeMethodPosition(long timestamp, Action<CommandClientReturnValue<RepairNodeMethodPosition>, KeepCallbackCommand> callback)
        {
            return client.GetRepairNodeMethodPosition(timestamp, callback);
        }

        /// <summary>
        /// 检查持久化文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化流已写入位置，失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition)
        {
            return client.CheckPersistenceFileHead(fileHeadVersion, rebuildPosition);
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback">获取持久化文件数据回调委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetPersistenceFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, Action<CommandClientReturnValue<PersistenceFileBuffer>, KeepCallbackCommand> callback)
        {
            return client.GetPersistenceFile(timestamp, fileHeadVersion, rebuildPosition, position, callback);
        }
        /// <summary>
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetPersistenceCallbackExceptionPosition(long timestamp, Action<CommandClientReturnValue<long>, KeepCallbackCommand> callback)
        {
            return client.GetPersistenceCallbackExceptionPosition(timestamp, callback);
        }
        /// <summary>
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition)
        {
            return client.CheckPersistenceCallbackExceptionPositionFileHead(fileHeadVersion, rebuildPosition);
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback">获取持久化回调异常位置文件数据回调委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetPersistenceCallbackExceptionPositionFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, Action<CommandClientReturnValue<PersistenceFileBuffer>, KeepCallbackCommand> callback)
        {
            return client.GetPersistenceCallbackExceptionPositionFile(timestamp, fileHeadVersion, rebuildPosition, position, callback);
        }
    }
}
