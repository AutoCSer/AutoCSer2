using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 直接获取返回值的客户端 API 封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ClientReturnValueController<T> where T : CommandClientSocketEvent
    {
        /// <summary>
        /// Command client socket event
        /// 命令客户端套接字事件
        /// </summary>
        protected readonly T client;
        /// <summary>
        /// Whether errors and exceptions are ignored
        /// 是否忽略异常与错误
        /// </summary>
        protected readonly bool isIgnoreError;
        /// <summary>
        /// 直接获取返回值的客户端 API 封装
        /// </summary>
        /// <param name="client">Command client socket event
        /// 命令客户端套接字事件</param>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略异常与错误</param>
        protected ClientReturnValueController(T client, bool isIgnoreError)
        {
            this.client = client;
            this.isIgnoreError = isIgnoreError;
            client.Client.GetSocketAsync().Catch();
        }
    }
    /// <summary>
    /// 代码生成模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TT"></typeparam>
    internal abstract class ClientReturnValueController<T, TT>
    {
        /// <summary>
        /// 代码生成模板
        /// </summary>
        protected readonly T client;
        /// <summary>
        /// 代码生成模板
        /// </summary>
        protected readonly bool isIgnoreError;
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isIgnoreError"></param>
        protected ClientReturnValueController(T client, bool isIgnoreError) 
        {
            this.client = client;
        }
    }
}
