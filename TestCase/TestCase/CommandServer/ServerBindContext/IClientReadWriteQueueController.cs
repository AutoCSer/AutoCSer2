using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerReadWriteQueueController), true)]
    public partial interface IClientReadWriteQueueController : IClientQueueController
    {
    }
}
