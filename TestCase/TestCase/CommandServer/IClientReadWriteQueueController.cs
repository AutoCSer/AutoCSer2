using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerReadWriteQueueController), true)]
    public partial interface IClientReadWriteQueueController : IClientQueueController
    {
    }
}
