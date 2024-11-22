using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作
    /// </summary>
    public class ServiceNode : IServiceNode
    {
        /// <summary>
        /// 创建服务基础操作节点标识
        /// </summary>
        internal static readonly NodeIndex ServiceNodeIndex = new NodeIndex(0, 1);

        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 服务基础操作
        /// </summary>
        /// <param name="service"></param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 删除节点持久化参数检查
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public virtual ValueResult<bool> RemoveNodeBeforePersistence(NodeIndex index)
        {
            if (index.Index == 0) return false;
            return service.RemoveNodeBeforePersistence(index);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public virtual bool RemoveNode(NodeIndex index)
        {
            return index.Index != 0 && service.RemoveNode(index);
        }
        /// <summary>
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createNode<T>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode) where T : class
        {
            try
            {
                if (!service.IsLoaded) service.LoadCreateNode(index, key);
                ServerNodeCreator nodeCreator = service.GetNodeCreator<T>();
                if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                NodeIndex nodeIndex = service.CheckCreateNodeIndex(index, key, ref nodeInfo);
                if (nodeIndex.Index < 0 || !nodeIndex.GetFree()) return nodeIndex;
                return new ServerNode<T>(service, nodeIndex, key, getNode()).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照的服务端节点 参数检查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ST"></typeparam>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="nodeIndex"></param>
        /// <returns>返回 true 表示直接返回无需继续操作</returns>
        protected virtual bool checkCreateNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, out NodeIndex nodeIndex)
        {
            if (!service.IsLoaded) service.LoadCreateNode(index, key);
            ServerNodeCreator nodeCreator = service.GetNodeCreator<T>();
            if (nodeCreator == null)
            {
                nodeIndex = new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                return true;
            }
            if (nodeCreator.SnapshotType != typeof(ST))
            {
                nodeIndex = new NodeIndex(CallStateEnum.SnapshotTypeNotMatch);
                return true;
            }
            nodeIndex = service.CheckCreateNodeIndex(index, key, ref nodeInfo);
            return nodeIndex.Index < 0 || !nodeIndex.GetFree();
        }
        /// <summary>
        /// 创建支持快照的服务端节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="NT">节点接口操作对象类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createNode<T, NT, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<NT> getNode)
            where T : class
            where NT : T, ISnapshot<ST>
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
            where T : class
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照克隆的服务端节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="NT">节点接口操作对象类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createSnapshotCloneNode<T, NT, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<NT> getNode)
            where T : class
            where NT : T, ISnapshot<ST>
            where ST : SnapshotCloneObject<ST>
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerSnapshotCloneNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照克隆的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createSnapshotCloneNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
            where T : class
            where ST : SnapshotCloneObject<ST>
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerSnapshotCloneNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建字典节点 FragmentHashStringDictionary256{HashString,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateFragmentHashStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IHashStringFragmentDictionaryNode<string>, HashStringFragmentDictionaryNode<string>, KeyValue<string, string>>(index, key, nodeInfo, () => new HashStringFragmentDictionaryNode<string>());
        }

        /// <summary>
        /// 创建服务基础操作节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ServerNode<T> CreateServiceNode<T>(StreamPersistenceMemoryDatabaseService service, T target)
             where T : class, IServiceNode
        {
            return new ServerNode<T>(service, ServiceNodeIndex, string.Empty, target);
        }
    }
}
