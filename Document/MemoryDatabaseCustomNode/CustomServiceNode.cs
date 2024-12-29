using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 自定义基础服务，用于添加自定义节点创建 API 方法
    /// </summary>
    public sealed class CustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// 自定义基础服务
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public CustomServiceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建计数器节点 ICounterNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ICounterNode>(index, key, nodeInfo, () => new CounterNode());
        }
        /// <summary>
        /// 创建字典计数器节点 IDictionaryCounterNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionaryCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum.Success)
            {
                MethodInfo method = typeof(CustomServiceNode).GetMethod(nameof(createDictionaryCounterNode), BindingFlags.Instance | BindingFlags.NonPublic).notNull();
                return (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex)method.MakeGenericMethod(type.notNull()).Invoke(this, new object[] { index, key, nodeInfo, capacity }).notNull();
            }
            return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex(state);
        }
        /// <summary>
        /// 创建字典计数器节点 IDictionaryCounterNode{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex createDictionaryCounterNode<T>(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity)
            where T : IEquatable<T>
        {
            return CreateSnapshotNode<IDictionaryCounterNode<T>>(index, key, nodeInfo, () => new DictionaryCounterNode<T>(capacity));
        }
        /// <summary>
        /// 创建支持快照克隆的字典计数器节点 IDictionarySnapshotCloneCounterNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionarySnapshotCloneCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum.Success)
            {
                MethodInfo method = typeof(CustomServiceNode).GetMethod(nameof(createDictionarySnapshotCloneCounterNode), BindingFlags.Instance | BindingFlags.NonPublic).notNull();
                return (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex)method.MakeGenericMethod(type.notNull()).Invoke(this, new object[] { index, key, nodeInfo, capacity }).notNull();
            }
            return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex(state);
        }
        /// <summary>
        /// 创建支持快照克隆的字典计数器节点 IDictionarySnapshotCloneCounterNode{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex createDictionarySnapshotCloneCounterNode<T>(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity)
            where T : IEquatable<T>
        {
            return CreateSnapshotNode<IDictionarySnapshotCloneCounterNode<T>>(index, key, nodeInfo, () => new DictionarySnapshotCloneCounterNode<T>(capacity));
        }
        /// <summary>
        /// 创建持久化前置检查示例节点 IBeforePersistenceNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateBeforePersistenceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IBeforePersistenceNode>(index, key, nodeInfo, () => new BeforePersistenceNode(capacity));
        }
    }
}
