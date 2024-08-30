using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用状态
    /// </summary>
    public enum CallStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 持久化成功但是执行异常，该节点可能处于错误状态，如果是内存不足异常则需要重启服务重新加载，对于程序逻辑异常还需要修复该错误逻辑
        /// </summary>
        PersistenceCallbackException,
        /// <summary>
        /// 节点编号超出索引范围
        /// </summary>
        NodeIndexOutOfRange,
        /// <summary>
        /// 节点标识不匹配
        /// </summary>
        NodeIdentityNotMatch,
        /// <summary>
        /// 服务已释放资源
        /// </summary>
        Disposed,
        /// <summary>
        /// 持久化回调异常写入位置信息，服务端将忽略该请求
        /// </summary>
        IgnorePersistenceCallbackException,
        /// <summary>
        /// 反序列化参数操作没有找到套接字会话对象
        /// </summary>
        NotFoundSessionObject,
        /// <summary>
        /// 客户端节点生成失败
        /// </summary>
        NotFoundClientNodeCreator,
        /// <summary>
        /// 关键字匹配的节点正在创建中
        /// </summary>
        NodeCreating,
        /// <summary>
        /// 服务端节点生成失败，说明该接口类型不符合节点定义
        /// </summary>
        NotFoundNodeCreator,
        /// <summary>
        /// 节点类型不匹配
        /// </summary>
        NodeTypeNotMatch,
        /// <summary>
        /// 该调用仅支持主服务节点
        /// </summary>
        OnlyMaster,
        /// <summary>
        /// 关键字为 null
        /// </summary>
        NullKey,
        /// <summary>
        /// 没有找到调用方法，说明服务端与客户端定义不匹配
        /// </summary>
        NotFoundMethod,
        /// <summary>
        /// 方法编号超出索引范围
        /// </summary>
        MethodIndexOutOfRange,
        /// <summary>
        /// 调用方法类型不匹配，说明服务端与客户端定义不匹配
        /// </summary>
        CallTypeNotMatch,
        /// <summary>
        /// 调用方法不允许客户端调用
        /// </summary>
        NotAllowClientCall,
        /// <summary>
        /// 持久化序列化异常
        /// </summary>
        PersistenceSerializeException,
        /// <summary>
        /// 持久化写入异常
        /// </summary>
        PersistenceWriteException,
        /// <summary>
        /// 加载数据参数缓冲区大小不一致
        /// </summary>
        LoadParameterSizeError,
        /// <summary>
        /// 加载数据参数反序列化失败
        /// </summary>
        LoadParameterDeserializeError,
        /// <summary>
        /// 节点加载执行异常
        /// </summary>
        LoadException,
        /// <summary>
        /// 节点修复方法加载异常
        /// </summary>
        LoadRepairNodeMethodException,
        /// <summary>
        /// 日志流持久化文件重建中
        /// </summary>
        PersistenceRebuilding,
        /// <summary>
        /// 快照数据类型不匹配
        /// </summary>
        SnapshotTypeNotMatch,
        /// <summary>
        /// 调用无返回值，表示 ValueResult{T}.IsValue 为 false
        /// </summary>
        NoReturnValue,
        /// <summary>
        /// 非法输出参数
        /// </summary>
        IllegalInputParameter,
        /// <summary>
        /// 节点方法输入参数不允许使用 ref / out 修饰
        /// </summary>
        NodeMethodParameterIsByRef,
        /// <summary>
        /// 非应答方法不支持返回值
        /// </summary>
        SendOnlyNotSupportReturnType,
        /// <summary>
        /// 非应答方法必须存在输入参数
        /// </summary>
        SendOnlyMustInputParameter,
        /// <summary>
        /// 持久化检查方法返回值类型必须为 bool 或者 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}
        /// </summary>
        BeforePersistenceMethodReturnTypeError,
        /// <summary>
        /// 持久化检查方法调用类型不匹配
        /// </summary>
        BeforePersistenceMethodCallTypeError,
        /// <summary>
        /// 动态加载程序集中没有找到修复方法的定义类型
        /// </summary>
        NotFoundRepairMethodDeclaringType,
        /// <summary>
        /// 动态加载程序集中没有找到修复方法
        /// </summary>
        NotFoundRepairMethod,
        /// <summary>
        /// 修复方法必须为静态方法
        /// </summary>
        RepairMethodNotStatic,
        /// <summary>
        /// 修复方法不能是泛型方法定义
        /// </summary>
        RepairMethodIsGenericMethodDefinition,
        /// <summary>
        /// 修复方法返回值类型与原接口定义不一致
        /// </summary>
        RepairMethodReturnTypeNotMatch,
        /// <summary>
        /// 修复方法第一个输入参数必须是节点接口类型
        /// </summary>
        RepairMethodNotFoundNodeTypeParameter,
        /// <summary>
        /// 修复方法输入参数数量与原接口定义不一致（除了第一个输出参数必须是节点接口类型）
        /// </summary>
        RepairMethodParameterCountNotMatch,
        /// <summary>
        /// 修复方法输入参数类型与原接口定义不一致
        /// </summary>
        RepairMethodParameterTypeNotMatch,
        /// <summary>
        /// 绑定方法编号以被使用，要修改该方法请调用修复功能
        /// </summary>
        BindMethodIndexUsed,
        /// <summary>
        /// 绑定方法不支持持久化之前检查参数方法
        /// </summary>
        BindMethodNotSupportBeforePersistence,
        /// <summary>
        /// 从节点客户端创建时间戳不匹配
        /// </summary>
        SlaveTimestampNotMatch,
        /// <summary>
        /// 文件头部数据不匹配，说明服务端持久化文件被重建
        /// </summary>
        FileHeadNotMatch,
        /// <summary>
        /// 文件持久化位置不再范围内，说明服务端重建处理过
        /// </summary>
        FilePositionOutOfRange,
        /// <summary>
        /// 当前配置 StreamPersistenceMemoryDatabaseServiceConfig.CanCreateSlave 不允许创建从节点
        /// </summary>
        CanNotCreateSlave,
        /// <summary>
        /// 持久化位置不匹配
        /// </summary>
        PositionNotMatch,
        /// <summary>
        /// 当前状态不匹配
        /// </summary>
        StateNotMatch,
        /// <summary>
        /// 客户端调用失败
        /// </summary>
        CallFail,
    }
}
