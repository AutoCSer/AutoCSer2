using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.NativeAOT.MemoryDatabaseLocalService
{
    /// <summary>
    /// Test the dictionary node
    /// 测试字典节点
    /// </summary>
    internal sealed class StringDictionaryNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.DictionaryNode<string, string>, IStringDictionaryNode
    {
        /// <summary>
        /// Test the dictionary node
        /// 测试字典节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        public StringDictionaryNode(int capacity = 0, AutoCSer.ReusableDictionaryGroupTypeEnum groupType = AutoCSer.ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType) { }

        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<IStringDictionaryNodeLocalClientNode> clientNodeCache = ServiceConfig.Client.CreateNode(client => client.GetOrCreateNode<IStringDictionaryNodeLocalClientNode>(nameof(IStringDictionaryNodeLocalClientNode), (index, nodeKey, nodeInfo) => client.ClientNode.CreateStringDictionaryNode(index, nodeKey, nodeInfo, 0, AutoCSer.ReusableDictionaryGroupTypeEnum.HashIndex)));
        /// <summary>
        /// Client-side testing
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static async Task<bool> TestCase()
        {
            var nodeResult = await clientNodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            IStringDictionaryNodeLocalClientNode node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Set("3A", "AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryGetValue("3A");
            if (!valueResult.IsSuccess || valueResult.Value.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.TryGetValue("3B");
            if (!valueResult.IsSuccess || valueResult.Value.IsValue)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Remove("3A");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
