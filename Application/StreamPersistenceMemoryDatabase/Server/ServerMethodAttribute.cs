using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Custom attributes of server-side node methods
    /// 服务端节点方法自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerMethodAttribute : Attribute
    {
        /// <summary>
        /// Custom method sequence numbers are used for the client to identify the routes of the server API. Repetition is not allowed in the same node interface. By default, a sequence number less than 0 indicates the use of the automatic matching mode. The automatic matching mode cannot guarantee that the old client call routes will match the new server routes after the server modifies and upgrades. When there are custom requirements, do not use huge data. It is recommended to start from 0 because it will be the size of a certain array.
        /// 自定义方法序号，用于客户端识别服务端 API 的路由，同一个节点接口中不允许重复，默认小于 0 表示采用自动匹配模式，自动匹配模式不能保证服务端修改升级以后旧的客户端调用路由能与新的服务端路由匹配。存在自定义需求时不要使用巨大的数据，建议从 0 开始，因为它会是某个数组的大小。
        /// </summary>
        public int MethodIndex = int.MinValue;
        /// <summary>
        /// The execution order of snapshot data recovery methods. By default, 0 indicates non-snapshot data recovery methods. Each snapshot interface type is only allowed to define one API method whose SnapshotMethodSort is not 0 as the API method for snapshot data recovery
        /// 快照数据恢复方法执行顺序，默认为 0 表示非快照数据恢复方法，每一个快照接口类型仅允许定义一个 SnapshotMethodSort 不为 0 的 API 方法作为快照数据恢复的 API 方法
        /// </summary>
        public int SnapshotMethodSort;
        /// <summary>
        /// By default, false indicates a non-persistent API call read operation queue; If set to true, it will force the call of the write operation queue (such as setting callbacks and other scenarios that only modify non-persistent memory data)
        /// 默认为 false 表示非持久化 API 调用读操作队列；设置为 true 则强制调用写操作队列（比如设置回调等仅修改非持久化内存数据的场景）
        /// </summary>
        public bool IsWriteQueue;
        /// <summary>
        /// By default, true indicates that the call needs to be persisted. If the call does not involve data change operations, it should be manually set to false to prevent junk data from being persisted (this is only effective in scenarios where nodes support snapshots; nodes that do not support snapshots do not support persistence, and even if set to true, it is invalid)
        /// 默认为 true 表示调用需要持久化，如果调用不涉及数据变更操作则应该手动设置为 false 避免垃圾数据被持久化（只有在节点支持快照的场景下才有效，不支持快照的节点不支持持久化，即使设置为 true 也无效）
        /// </summary>
        public bool IsPersistence = true;
        /// <summary>
        /// By default, true indicates that the method is allowed to be called by the client; otherwise, it is called internally by the server
        /// 默认为 true 表示允许客户端调用，否则为服务端内部调用方法
        /// </summary>
        public bool IsClientCall = true;
        /// <summary>
        /// By default, false indicates that the server responds to the client's request. Setting it to true means that the server only receives the request and does not perform a response operation
        /// 默认为 false 表示服务端应答客户端请求，设置为 true 表示服务端仅接收请求不做应答操作
        /// </summary>
        public bool IsSendOnly;
        /// <summary>
        /// By default, false indicates the generation of the await client API, and setting it to true generates the delegate callback client API
        /// 默认为 false 表示生成 await 客户端 API，设置为 true 则生成委托回调客户端 API
        /// </summary>
        public bool IsCallbackClient;
        /// <summary>
        /// Default to false said persistent callback sets the Node to unavailable when abnormal condition, is set to true, the Node. The IsPersistenceCallbackChanged to false when to avoid the problem, but the Node method must ensure that the abnormal data reduction to restore memory state, It is necessary to pay attention to the memory shortage exception caused by new. All new operations should be completed before modifying the data
        /// 默认为 false 表示持久化回调异常时将节点设置为不可用状态，设置为 true 则在 Node.IsPersistenceCallbackChanged 为 false 时避免该问题，但是节点方法必须保证异常时还原恢复内存数据状态，必须关心 new 产生的内存不足异常，在修改数据以前应该将完成所有 new 操作
        /// </summary>
        public bool IsIgnorePersistenceCallbackException;

        /// <summary>
        /// Default node method custom attribute
        /// 默认节点方法自定义属性
        /// </summary>
        internal static readonly ServerMethodAttribute Default = new ServerMethodAttribute();
    }
}
