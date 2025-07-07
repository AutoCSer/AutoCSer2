using System;

namespace AutoCSer.Document.CodeGenerator.MemoryDatabase
{
    /// <summary>
    /// An example of generate the API definition of the local client node interface
    /// 生成本地客户端节点接口 API 定义示例
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode(IsLocalClient = true, IsClient = false)]
    public partial interface ILocalNode
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
