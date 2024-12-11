using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 服务基础操作自定义扩展（用于添加自定义节点创建接口）
    /// </summary>
    public class CustomServiceNode : ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// 服务基础操作自定义扩展
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public CustomServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建字典节点 FragmentDictionaryNode{int,Data.Address}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateAddressFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IFragmentDictionaryNode<int, Data.Address>, FragmentDictionaryNode<int, Data.Address>, KeyValue<int, Data.Address>>(index, key, nodeInfo, () => new FragmentDictionaryNode<int, Data.Address>());
        }
        /// <summary>
        /// 创建字典节点 FragmentDictionaryNode{int,ServerBinary{Data.Address}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateServerBinaryAddressFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IFragmentDictionaryNode<int, ServerBinary<Data.Address>>, FragmentDictionaryNode<int, ServerBinary<Data.Address>>, KeyValue<int, ServerBinary<Data.Address>>>(index, key, nodeInfo, () => new FragmentDictionaryNode<int, ServerBinary<Data.Address>>());
        }
        
    }
}
