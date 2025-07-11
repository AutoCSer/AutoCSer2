﻿using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerReadWriteQueueController), true)]
#endif
    public partial interface IClientReadWriteQueueController : IClientQueueController
    {
    }
}
