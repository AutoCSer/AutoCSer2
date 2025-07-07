using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.Reflection;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persistence in-memory database client interface
    /// 日志流持久化内存数据库客户端接口
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseClient
    {
        /// <summary>
        /// Get the server UTC time
        /// 获取服务端 UTC 时间
        /// </summary>
        /// <returns></returns>
        ReturnCommand<DateTime> GetUtcNow();
        /// <summary>
        /// Get the current write location of the persistent stream
        /// 获取持久化流已当前写入位置
        /// </summary>
        /// <returns></returns>
        ReturnCommand<long> GetPersistencePosition();
        /// <summary>
        /// Get the end position of the rebuild snapshot
        /// 获取重建快照结束位置
        /// </summary>
        /// <returns></returns>
        ReturnCommand<long> GetRebuildSnapshotPosition();
        /// <summary>
        /// Get node identity
        /// 获取节点标识
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="isCreate">Create a free node identity when the keyword does not exist
        /// 关键字不存在时创建空闲节点标识</param>
        /// <returns>When the keyword does not exist, return an free node identifier for creating the node
        /// 关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        ReturnCommand<NodeIndex> GetNodeIndex(string key, NodeInfo nodeInfo, bool isCreate);
        /// <summary>
        /// Gets the global keyword for all matching nodes
        /// 获取所有匹配节点的全局关键字
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <returns></returns>
        EnumeratorCommand<string> GetNodeKeys(NodeInfo nodeInfo);
        /// <summary>
        /// Gets the node index information for all matching nodes
        /// 获取所有匹配节点的节点索引信息
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <returns></returns>
        EnumeratorCommand<NodeIndex> GetNodeIndexs(NodeInfo nodeInfo);
        /// <summary>
        /// Gets the global keyword and node index information of all matching nodes
        /// 获取所有匹配节点的全局关键字与节点索引信息
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <returns></returns>
        EnumeratorCommand<BinarySerializeKeyValue<string, NodeIndex>> GetNodeKeyIndexs(NodeInfo nodeInfo);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        SendOnlyCommand SendOnly(RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> Call(NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand Call(NodeIndex index, int methodIndex, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(Call))]
        ReturnCommand<CallStateEnum> ClientSynchronousCall(NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(Call))]
        CallbackCommand ClientSynchronousCall(NodeIndex index, int methodIndex, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallOutput))]
        ReturnCommand<ResponseParameter> ClientSynchronousCallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallOutput))]
        CallbackCommand ClientSynchronousCallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> CallInput(RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallInput(RequestParameter parameter, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInput))]
        ReturnCommand<CallStateEnum> ClientSynchronousCallInput(RequestParameter parameter);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInput))]
        CallbackCommand ClientSynchronousCallInput(RequestParameter parameter, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallInputOutput(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallInputOutput(ResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputOutput))]
        ReturnCommand<ResponseParameter> ClientSynchronousCallInputOutput(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputOutput))]
        CallbackCommand ClientSynchronousCallInputOutput(ResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> KeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        KeepCallbackCommand KeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(KeepCallback))]
        EnumeratorCommand<KeepCallbackResponseParameter> ClientSynchronousKeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(KeepCallback))]
        KeepCallbackCommand ClientSynchronousKeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> InputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        KeepCallbackCommand InputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(InputKeepCallback))]
        EnumeratorCommand<KeepCallbackResponseParameter> ClientSynchronousInputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(InputKeepCallback))]
        KeepCallbackCommand ClientSynchronousInputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        SendOnlyCommand SendOnlyWrite(RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> CallWrite(NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallWrite(NodeIndex index, int methodIndex, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallWrite))]
        ReturnCommand<CallStateEnum> ClientSynchronousCallWrite(NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallWrite))]
        CallbackCommand ClientSynchronousCallWrite(NodeIndex index, int methodIndex, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallOutputWrite(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallOutputWrite(ResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallOutputWrite))]
        ReturnCommand<ResponseParameter> ClientSynchronousCallOutputWrite(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallOutputWrite))]
        CallbackCommand ClientSynchronousCallOutputWrite(ResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> CallInputWrite(RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallInputWrite(RequestParameter parameter, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputWrite))]
        ReturnCommand<CallStateEnum> ClientSynchronousCallInputWrite(RequestParameter parameter);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputWrite))]
        CallbackCommand ClientSynchronousCallInputWrite(RequestParameter parameter, Action<CommandClientReturnValue<CallStateEnum>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallInputOutputWrite(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        CallbackCommand CallInputOutputWrite(ResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputOutputWrite))]
        ReturnCommand<ResponseParameter> ClientSynchronousCallInputOutputWrite(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(CallInputOutputWrite))]
        CallbackCommand ClientSynchronousCallInputOutputWrite(ResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<ResponseParameter>> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> KeepCallbackWrite(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        KeepCallbackCommand KeepCallbackWrite(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(KeepCallbackWrite))]
        EnumeratorCommand<KeepCallbackResponseParameter> ClientSynchronousKeepCallbackWrite(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(KeepCallbackWrite))]
        KeepCallbackCommand ClientSynchronousKeepCallbackWrite(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> InputKeepCallbackWrite(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        KeepCallbackCommand InputKeepCallbackWrite(KeepCallbackResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(InputKeepCallbackWrite))]
        EnumeratorCommand<KeepCallbackResponseParameter> ClientSynchronousInputKeepCallbackWrite(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// Call the node method (client I/O thread synchronization callback)
        /// 调用节点方法（客户端 IO 线程同步回调）
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, MatchMethodName = nameof(InputKeepCallbackWrite))]
        KeepCallbackCommand ClientSynchronousInputKeepCallbackWrite(KeepCallbackResponseParameter returnValue, RequestParameter parameter, Action<CommandClientReturnValue<KeepCallbackResponseParameter>, KeepCallbackCommand> callback);
        /// <summary>
        /// Rebuild the persistent file (clear invalid data), and note that nodes that do not support snapshots will be discarded
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <returns></returns>
        ReturnCommand<RebuildResult> Rebuild();
        /// <summary>
        /// Fix the interface method error and force overwriting the original interface method call. Except for the first parameter being the operation node object, the method definition must be consistent
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="methodName">The name of the repair method must be a static method. The first parameter must be the interface type of the operation node, and the method number must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName);
        /// <summary>
        /// Bind a new method to dynamically add interface functionality. The initial state of the new method number must be free
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="methodName">The name of the repair method must be a static method. The first parameter must be the interface type of the operation node. The method number and other necessary configuration information must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName);
        /// <summary>
        /// Create a slave node
        /// 创建从节点
        /// </summary>
        /// <param name="isBackup">Is the backup client
        /// 是否备份客户端</param>
        /// <returns>Verify the timestamp from the node, and a negative number represents the CallStateEnum error status
        /// 从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        ReturnCommand<long> CreateSlave(bool isBackup);
        /// <summary>
        /// Remove the information from the node client
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <returns></returns>
        SendOnlyCommand RemoveSlave(long timestamp);
        /// <summary>
        /// Add the directory and file information of the repair method from the node
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="directory">Directory information of the repair method
        /// 修复方法目录信息</param>
        /// <param name="file">File information of the repair method
        /// 修复方法文件信息</param>
        /// <returns></returns>
        SendOnlyCommand AppendRepairNodeMethodDirectoryFile(long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file);
        /// <summary>
        /// Get the repair node method information from slave node
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback">Callback delegate for repair node method information
        /// 修复节点方法信息回调委托</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetRepairNodeMethodPosition(long timestamp, Action<CommandClientReturnValue<RepairNodeMethodPosition>, KeepCallbackCommand> callback);
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
        ReturnCommand<long> CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// Get the persistent file data
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback">Callback delegate to get persistent file data
        /// 获取持久化文件数据回调委托</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetPersistenceFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, Action<CommandClientReturnValue<PersistenceFileBuffer>, KeepCallbackCommand> callback);
        /// <summary>
        /// Get the location data of the persistent callback exception
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback"></param>
        KeepCallbackCommand GetPersistenceCallbackExceptionPosition(long timestamp, Action<CommandClientReturnValue<long>, KeepCallbackCommand> callback);
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
        ReturnCommand<long> CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition);
        /// <summary>
        /// Get the file data of the persistent callback exception location
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback">Callback delegate that gets persistent callback exception location file data
        /// 获取持久化回调异常位置文件数据回调委托</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetPersistenceCallbackExceptionPositionFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, Action<CommandClientReturnValue<PersistenceFileBuffer>, KeepCallbackCommand> callback);
    }
}
