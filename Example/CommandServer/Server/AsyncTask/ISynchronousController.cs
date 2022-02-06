using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTask
{
    /// <summary>
    /// 服务端 async Task 调用 同步返回数据 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task<int> SynchronousReturn(CommandServerSocket socket, int parameter1, int parameter2);
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task SynchronousCall(int parameter1, int parameter2);
    }
    /// <summary>
    /// 服务端 async Task 调用 同步返回数据 示例接口实例
    /// </summary>
    internal sealed class SynchronousController : ISynchronousController
    {
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        async Task<int> ISynchronousController.SynchronousReturn(CommandServerSocket socket, int parameter1, int parameter2)
        {
            await Task.Yield();
            return parameter1 + parameter2;
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        async Task ISynchronousController.SynchronousCall(int parameter1, int parameter2)
        {
            await Task.Yield();
            Console.WriteLine(parameter1 + parameter2);
        }
    }
}
