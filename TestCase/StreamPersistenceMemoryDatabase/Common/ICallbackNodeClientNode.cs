using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 客户端节点接口扩展，用于服务端动态绑定新接口方法测试
    /// </summary>
    public partial interface ICallbackNodeClientNode
    {
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> BindNodeMethodTest(int value);
    }
}
