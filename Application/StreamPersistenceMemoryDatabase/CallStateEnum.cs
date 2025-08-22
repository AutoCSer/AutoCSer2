using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Call status
    /// 调用状态
    /// </summary>
    public enum CallStateEnum : byte
    {
        /// <summary>
        /// Unknown error or exception
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// The call was successful
        /// 调用成功
        /// </summary>
        Success,
        /// <summary>
        /// The persistence was successful but an execution exception occurred. This node may be in an error state. If it is due to insufficient memory, the service needs to be restarted and reloaded. For program logic exceptions, the erroneous logic also needs to be fixed
        /// 持久化成功但是执行异常，该节点可能处于错误状态，如果是内存不足异常则需要重启服务重新加载，对于程序逻辑异常还需要修复该错误逻辑
        /// </summary>
        PersistenceCallbackException,
        /// <summary>
        /// The node number exceeds the index range
        /// 节点编号超出索引范围
        /// </summary>
        NodeIndexOutOfRange,
        /// <summary>
        /// Node identity does not match
        /// 节点标识不匹配
        /// </summary>
        NodeIdentityNotMatch,
        /// <summary>
        /// The service has released resources
        /// 服务已释放资源
        /// </summary>
        Disposed,
        /// <summary>
        /// The persistent callback exception writes location information, and the server will ignore the request
        /// 持久化回调异常写入位置信息，服务端将忽略该请求
        /// </summary>
        IgnorePersistenceCallbackException,
        /// <summary>
        /// The deserialization parameter operation did not find the socket session object
        /// 反序列化参数操作没有找到套接字会话对象
        /// </summary>
        NotFoundSessionObject,
        /// <summary>
        /// The generation of the client node failed
        /// 客户端节点生成失败
        /// </summary>
        NotFoundClientNodeCreator,
        ///// <summary>
        ///// 关键字匹配的节点正在创建中
        ///// </summary>
        //NodeCreating,
        /// <summary>
        /// No nodes that match the keywords were found
        /// 没有找到匹配关键字的节点
        /// </summary>
        NotFoundNodeKey,
        /// <summary>
        /// The creation of server nodes only supports interface types, indicating that the invocation of generic type parameters is incorrect
        /// 创建服务端节点仅支持接口类型，说明调用泛型类型参数不正确
        /// </summary>
        OnlySupportInterface,
        /// <summary>
        /// The request parameters lack node information
        /// 请求参数缺少节点信息
        /// </summary>
        NullNodeInfo,
        /// <summary>
        /// The server node generation failed, indicating that the interface type does not conform to the node definition
        /// 服务端节点生成失败，说明该接口类型不符合节点定义
        /// </summary>
        NotFoundNodeCreator,
        /// <summary>
        /// The node type do not match
        /// 节点类型不匹配
        /// </summary>
        NodeTypeNotMatch,
        /// <summary>
        /// This call only supports the main service node
        /// 该调用仅支持主服务节点
        /// </summary>
        OnlyMaster,
        /// <summary>
        /// The key word is null
        /// 关键字为 null
        /// </summary>
        NullKey,
        /// <summary>
        /// The call method was not found, indicating that the definitions of the server and the client do not match
        /// 没有找到调用方法，说明服务端与客户端定义不匹配
        /// </summary>
        NotFoundMethod,
        /// <summary>
        /// The method number is beyond the index range
        /// 方法编号超出索引范围
        /// </summary>
        MethodIndexOutOfRange,
        /// <summary>
        /// If the types of the called methods do not match, it indicates that the definitions of the server and the client do not match
        /// 调用方法类型不匹配，说明服务端与客户端定义不匹配
        /// </summary>
        CallTypeNotMatch,
        /// <summary>
        /// The calling method is not allowed to be invoked by the client
        /// 调用方法不允许客户端调用
        /// </summary>
        NotAllowClientCall,
        /// <summary>
        /// Persistent serialization exception
        /// 持久化序列化异常
        /// </summary>
        PersistenceSerializeException,
        /// <summary>
        /// Persistent write exception
        /// 持久化写入异常
        /// </summary>
        PersistenceWriteException,
        /// <summary>
        /// The buffer sizes of the loaded data parameters are inconsistent
        /// 加载数据参数缓冲区大小不一致
        /// </summary>
        LoadParameterSizeError,
        /// <summary>
        /// The deserialization of the loaded data parameters failed
        /// 加载数据参数反序列化失败
        /// </summary>
        LoadParameterDeserializeError,
        /// <summary>
        /// Node loading execution is abnormal
        /// 节点加载执行异常
        /// </summary>
        LoadException,
        /// <summary>
        /// Node repair method loading exception
        /// 节点修复方法加载异常
        /// </summary>
        LoadRepairNodeMethodException,
        /// <summary>
        /// Reconstruction of log stream persistence files
        /// 日志流持久化文件重建中
        /// </summary>
        PersistenceRebuilding,
        /// <summary>
        /// The snapshot interface was not found
        /// 没有找到快照接口
        /// </summary>
        NotFoundSnapshotNode,
        /// <summary>
        /// No method matching the snapshot interface type was found
        /// 没有找到与快照接口类型匹配的方法
        /// </summary>
        NotFoundSnapshotMethod,
        /// <summary>
        /// The call has no return value, indicating that ValueResult{T}.IsValue is false
        /// 调用无返回值，表示 ValueResult{T}.IsValue 为 false
        /// </summary>
        NoReturnValue,
        /// <summary>
        /// Illegal output of parameters
        /// 非法输出参数
        /// </summary>
        IllegalInputParameter,
        /// <summary>
        /// The input parameters of node methods are not allowed to be modified by ref/out
        /// 节点方法输入参数不允许使用 ref / out 修饰
        /// </summary>
        NodeMethodParameterIsByRef,
        /// <summary>
        /// Unresponsive methods do not support return values
        /// 无响应方法不支持返回值
        /// </summary>
        SendOnlyNotSupportReturnType,
        /// <summary>
        /// Unresponsive methods must have input parameters
        /// 无响应方法必须存在输入参数
        /// </summary>
        SendOnlyMustInputParameter,
        /// <summary>
        /// Persistent check method return value type must be a bool or AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}
        /// 持久化检查方法返回值类型必须为 bool 或者 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}
        /// </summary>
        BeforePersistenceMethodReturnTypeError,
        /// <summary>
        /// The persistence check method call type does not match
        /// 持久化检查方法调用类型不匹配
        /// </summary>
        BeforePersistenceMethodCallTypeError,
        /// <summary>
        /// The definition type of the repair method was not found in the dynamic loader
        /// 动态加载程序集中没有找到修复方法的定义类型
        /// </summary>
        NotFoundRepairMethodDeclaringType,
        /// <summary>
        /// No repair method was found in the dynamic loader
        /// 动态加载程序集中没有找到修复方法
        /// </summary>
        NotFoundRepairMethod,
        /// <summary>
        /// The repair method must be a static method
        /// 修复方法必须为静态方法
        /// </summary>
        RepairMethodNotStatic,
        /// <summary>
        /// The repair method cannot be defined by a generic method
        /// 修复方法不能是泛型方法定义
        /// </summary>
        RepairMethodIsGenericMethodDefinition,
        /// <summary>
        /// The return value type of the repair method is inconsistent with the original interface definition
        /// 修复方法返回值类型与原接口定义不一致
        /// </summary>
        RepairMethodReturnTypeNotMatch,
        /// <summary>
        /// The first input parameter of the repair method must be the node interface type
        /// 修复方法第一个输入参数必须是节点接口类型
        /// </summary>
        RepairMethodNotFoundNodeTypeParameter,
        /// <summary>
        /// The number of input parameters in the repair method is inconsistent with the original interface definition (except that the first output parameter must be of the node interface type).
        /// 修复方法输入参数数量与原接口定义不一致（除了第一个输出参数必须是节点接口类型）
        /// </summary>
        RepairMethodParameterCountNotMatch,
        /// <summary>
        /// The input parameter types of the repair method are inconsistent with the original interface definitions
        /// 修复方法输入参数类型与原接口定义不一致
        /// </summary>
        RepairMethodParameterTypeNotMatch,
        /// <summary>
        /// The binding method number has been used. To modify this method, please call the repair function
        /// 绑定方法编号已被使用，要修改该方法请调用修复功能
        /// </summary>
        BindMethodIndexUsed,
        /// <summary>
        /// The binding method does not support checking parameter methods before persistence
        /// 绑定方法不支持持久化之前检查参数方法
        /// </summary>
        BindMethodNotSupportBeforePersistence,
        /// <summary>
        /// Create a timestamp mismatch from the node client
        /// 从节点客户端创建时间戳不匹配
        /// </summary>
        SlaveTimestampNotMatch,
        /// <summary>
        /// If the data in the file header does not match, it indicates that the server's persistent file has been rebuild
        /// 文件头部数据不匹配，说明服务端持久化文件被重建
        /// </summary>
        FileHeadNotMatch,
        /// <summary>
        /// The file persistence location is not within the range, indicating that the server has rebuild and processed it
        /// 文件持久化位置不在范围内，说明服务端重建处理过
        /// </summary>
        FilePositionOutOfRange,
        /// <summary>
        /// The current configuration  StreamPersistenceMemoryDatabaseServiceConfig.CanCreateSlave don't allow you to create a node
        /// 当前配置 StreamPersistenceMemoryDatabaseServiceConfig.CanCreateSlave 不允许创建从节点
        /// </summary>
        CanNotCreateSlave,
        /// <summary>
        /// The persistent location does not match
        /// 持久化位置不匹配
        /// </summary>
        PositionNotMatch,
        /// <summary>
        /// The current status does not match
        /// 当前状态不匹配
        /// </summary>
        StateNotMatch,
        /// <summary>
        /// The custom deserialization failed
        /// 自定义反序列化失败
        /// </summary>
        CustomDeserializeError,
        /// <summary>
        /// File data reading failed
        /// 文件读取数据失败
        /// </summary>
        ReadFileSizeError,
        /// <summary>
        /// The client call failed
        /// 客户端调用失败
        /// </summary>
        CallFail,
        /// <summary>
        /// Could not find the generic type, or generic type does not meet the AutoCSer.Common.Config.CheckRemoteType legitimacy check condition
        /// 没有找到泛型类型，或者泛型类型不满足 AutoCSer.Common.Config.CheckRemoteType 合法性检查条件
        /// </summary>
        NotFoundRemoteType,
        /// <summary>
        /// Generic types do not meet the requirements
        /// 泛型类型不满足要求
        /// </summary>
        RemoteTypeError,
        /// <summary>
        /// No data was returned
        /// 没有返回数据
        /// </summary>
        NullResponseParameter,
        /// <summary>
        /// Custom operation status error
        /// 自定义操作状态错误
        /// </summary>
        CustomStateError,
        /// <summary>
        /// Custom operation exception
        /// 自定义操作异常
        /// </summary>
        CustomException,
        /// <summary>
        /// Persistent operations are not supported
        /// 不支持持久化操作
        /// </summary>
        NotSupportPersistence,
        /// <summary>
        /// Unknown persistence type
        /// 未知的持久化类型
        /// </summary>
        UnknownPersistenceType,

        /// <summary>
        /// The client initialization loading has not been completed
        /// 客户端初始化加载未完成
        /// </summary>
        ClientLoadUnfinished,
        /// <summary>
        /// The client has released resources
        /// 客户端已释放资源
        /// </summary>
        ClientDisposed,

        /// <summary>
        /// Callback already (available for counting)
        /// 已回调（可用于计数）
        /// </summary>
        Callbacked,
    }
}
