using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Dictionary generic expansion custom node interface example
    /// 字典泛型展开自定义节点接口示例
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface IStringDictionaryNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<string, string>
    {
    }
}
