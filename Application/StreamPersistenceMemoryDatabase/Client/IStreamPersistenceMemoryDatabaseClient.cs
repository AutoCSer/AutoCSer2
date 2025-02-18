using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端接口
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseClient
    {
        /// <summary>
        /// 获取服务端 UTC 时间
        /// </summary>
        /// <returns></returns>
        ReturnCommand<DateTime> GetUtcNow();
        /// <summary>
        /// 获取持久化流已当前写入位置
        /// </summary>
        /// <returns></returns>
        ReturnCommand<long> GetPersistencePosition();
        /// <summary>
        /// 获取重建快照结束位置
        /// </summary>
        /// <returns></returns>
        ReturnCommand<long> GetRebuildSnapshotPosition();
        /// <summary>
        /// 获取节点标识
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="isCreate">关键字不存在时创建空闲节点标识</param>
        /// <returns>关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        ReturnCommand<NodeIndex> GetNodeIndex(string key, NodeInfo nodeInfo, bool isCreate);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        SendOnlyCommand SendOnly(RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> Call(NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand Call(NodeIndex index, int methodIndex, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(Call))]
        ReturnCommand<CallStateEnum> ClientSynchronousCall(NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(Call))]
        CallbackCommand ClientSynchronousCall(NodeIndex index, int methodIndex, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallOutput))]
        ReturnCommand<ResponseParameter> ClientSynchronousCallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallOutput))]
        CallbackCommand ClientSynchronousCallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> CallInput(RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallInput(RequestParameter parameter, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInput))]
        ReturnCommand<CallStateEnum> ClientSynchronousCallInput(RequestParameter parameter);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInput))]
        CallbackCommand ClientSynchronousCallInput(RequestParameter parameter, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallInputOutput(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallInputOutput(ResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputOutput))]
        ReturnCommand<ResponseParameter> ClientSynchronousCallInputOutput(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputOutput))]
        CallbackCommand ClientSynchronousCallInputOutput(ResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> KeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        KeepCallbackCommand KeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(KeepCallback))]
        EnumeratorCommand<KeepCallbackResponseParameter> ClientSynchronousKeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(KeepCallback))]
        KeepCallbackCommand ClientSynchronousKeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> InputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        KeepCallbackCommand InputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(InputKeepCallback))]
        EnumeratorCommand<KeepCallbackResponseParameter> ClientSynchronousInputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(InputKeepCallback))]
        KeepCallbackCommand ClientSynchronousInputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <returns></returns>
        ReturnCommand<RebuildResult> Rebuild();
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName);
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName);
        /// <summary>
        /// 创建从节点
        /// </summary>
        /// <param name="isBackup">是否备份客户端</param>
        /// <returns>从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        ReturnCommand<long> CreateSlave(bool isBackup);
        /// <summary>
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <returns></returns>
        SendOnlyCommand RemoveSlave(long timestamp);
        /// <summary>
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="directory">修复方法目录信息</param>
        /// <param name="file">修复方法文件信息</param>
        /// <returns></returns>
        SendOnlyCommand AppendRepairNodeMethodDirectoryFile(long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file);
        /// <summary>
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback">修复节点方法信息回调委托</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetRepairNodeMethodPosition(long timestamp, Action<CommandClientReturnValue<RepairNodeMethodPosition>, KeepCallbackCommand> callback);
        /// <summary>
        /// 检查持久化文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化流已写入位置，失败返回 -1</returns>
        ReturnCommand<long> CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback">获取持久化文件数据回调委托</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetPersistenceFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, Action<CommandClientReturnValue<PersistenceFileBuffer>, KeepCallbackCommand> callback);
        /// <summary>
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback"></param>
        KeepCallbackCommand GetPersistenceCallbackExceptionPosition(long timestamp, Action<CommandClientReturnValue<long>, KeepCallbackCommand> callback);
        /// <summary>
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        ReturnCommand<long> CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback">获取持久化回调异常位置文件数据回调委托</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetPersistenceCallbackExceptionPositionFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, Action<CommandClientReturnValue<PersistenceFileBuffer>, KeepCallbackCommand> callback);
    }
}
