using System;

namespace AutoCSer.Document.CodeGenerator.MemoryDatabase
{
    /// <summary>
    /// An example of generate the API definition of the client node interface
    /// 生成客户端节点接口 API 定义示例
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface IServerNode
    {
        /// <summary>
        /// Test API
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
}
