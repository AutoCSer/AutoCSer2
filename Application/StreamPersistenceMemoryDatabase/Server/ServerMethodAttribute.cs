using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerMethodAttribute : Attribute
    {
        /// <summary>
        /// 自定义方法序号，用于客户端识别服务端 API 的路由，同一个节点接口中不允许重复，默认小于 0 表示采用自动匹配模式，自动匹配模式不能保证服务端修改升级以后旧的客户端调用路由能与新的服务端路由匹配。存在自定义需求时不要使用巨大的数据，建议从 0 开始，因为它会是某个数组的大小。
        /// </summary>
        public int MethodIndex = int.MinValue;
        /// <summary>
        /// 快照数据恢复方法执行顺序，默认为 0 表示非快照数据恢复方法，每一个快照接口类型仅允许定义一个 SnapshotMethodSort 不为 0 的 API 方法作为快照数据恢复的 API 方法
        /// </summary>
        public int SnapshotMethodSort;
        /// <summary>
        /// 默认为 true 表示调用需要持久化，如果调用不涉及数据变更操作则应该手动设置为 false 避免垃圾数据被持久化（只有在节点支持快照的场景下才有效，不支持快照的节点不支持持久化，即使设置为 true 也无效）
        /// </summary>
        public bool IsPersistence = true;
        /// <summary>
        /// 默认为 true 表示允许客户端调用，否则为服务端内存调用方法
        /// </summary>
        public bool IsClientCall = true;
        /// <summary>
        /// 默认为 false 表示服务端应答客户端请求，设置为 true 表示服务端仅接收请求不做应答操作
        /// </summary>
        public bool IsSendOnly;
        /// <summary>
        /// 默认为 false 表示持续响应 API 生成 EnumeratorCommand 客户端 API，设置为 true 则生成委托回调 KeepCallbackCommand 客户端 API
        /// </summary>
        public bool IsKeepCallbackCommand;
        /// <summary>
        /// 默认为 false 表示持久化回调异常时将节点设置为不可用状态，设置为 true 则在 Node.IsPersistenceCallbackChanged 为 false 时避免该问题，但是节点方法必须保证异常时还原恢复内存数据状态，必须关心 new 产生的内存不足异常，在修改数据以前应该将完成所有 new 操作
        /// </summary>
        public bool IsIgnorePersistenceCallbackException;

        /// <summary>
        /// 默认节点方法自定义属性
        /// </summary>
        internal static readonly ServerMethodAttribute Default = new ServerMethodAttribute();
    }
}
