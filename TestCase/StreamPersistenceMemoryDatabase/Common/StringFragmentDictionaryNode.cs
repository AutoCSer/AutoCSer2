using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试 256 基分片字典节点
    /// </summary>
    internal sealed class StringFragmentDictionaryNode : FragmentDictionaryNode<string, string>, IStringFragmentDictionaryNode
    {
    }
}
