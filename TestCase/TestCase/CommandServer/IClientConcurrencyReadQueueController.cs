using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerConcurrencyReadQueueController), true)]
#endif
    public partial interface IClientConcurrencyReadQueueController : IClientQueueController
    {
    }
}
