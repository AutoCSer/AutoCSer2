using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
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
    }
}
