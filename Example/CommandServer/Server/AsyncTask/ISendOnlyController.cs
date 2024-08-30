using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTask
{
    /// <summary>
    /// 服务端 async Task 调用 不返回数据（不应答客户端） 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        Task<CommandServerSendOnly> SendOnly(CommandServerSocket socket, int parameter1, int parameter2);
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        Task<CommandServerSendOnly> SendOnly(int parameter);
    }
    /// <summary>
    /// 服务端 async Task 调用 不返回数据（不应答客户端） 示例接口实例
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        Task<CommandServerSendOnly> ISendOnlyController.SendOnly(CommandServerSocket socket, int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
            return Task.FromResult((CommandServerSendOnly)null);
        }
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        Task<CommandServerSendOnly> ISendOnlyController.SendOnly(int parameter)
        {
            Console.WriteLine(parameter);
            return Task.FromResult((CommandServerSendOnly)null);
        }
    }
}
