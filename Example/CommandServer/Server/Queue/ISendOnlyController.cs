using AutoCSer.Net;
using System;

namespace AutoCSer.Example.CommandServer.Server.Queue
{
    /// <summary>
    /// 服务端 同步队列线程调用 不返回数据（不应答客户端） 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallQueue queue, int parameter1, int parameter2);
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        CommandServerSendOnly SendOnly(CommandServerCallLowPriorityQueue queue, int parameter);
    }
    /// <summary>
    /// 服务端 同步队列线程调用 不返回数据（不应答客户端） 示例接口实例
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        CommandServerSendOnly ISendOnlyController.SendOnly(CommandServerSocket socket, CommandServerCallQueue queue, int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
            return null;
        }
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        CommandServerSendOnly ISendOnlyController.SendOnly(CommandServerCallLowPriorityQueue queue, int parameter)
        {
            Console.WriteLine(parameter);
            return null;
        }
    }
}
