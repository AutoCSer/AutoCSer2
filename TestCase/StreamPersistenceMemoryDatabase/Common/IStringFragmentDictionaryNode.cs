using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试 256 基分片字典 节点接口
    /// </summary>
    [ServerNode]
    public partial interface IStringFragmentDictionaryNode : IFragmentDictionaryNode<string, string>
    {
    }
}
