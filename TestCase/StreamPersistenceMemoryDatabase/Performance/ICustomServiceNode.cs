using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口）
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(CustomServiceNodeMethodEnum), IsAutoMethodIndex = false, IsClientCodeGeneratorOnlyDeclaringMethod = true)]
    public interface ICustomServiceNode : IServiceNode
    {
        /// <summary>
        /// 创建字典节点 FragmentDictionaryNode{int,Data.Address}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateAddressFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建字典节点 FragmentDictionaryNode{int,ServerBinary{Data.Address}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateServerBinaryAddressFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
    }
}
