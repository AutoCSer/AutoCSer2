﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientCallbackTaskController : IClientCallbackController
    {
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientCallbackTaskController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            return ClientCallbackController.TestCase(client.ClientCallbackTaskController, clientSessionObject);
        }
    }
}