using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试 256 基分片哈希表节点
    /// </summary>
    internal sealed class StringFragmentHashSetNode : FragmentHashSetNode<string>, IStringFragmentHashSetNode
    {
    }
}
