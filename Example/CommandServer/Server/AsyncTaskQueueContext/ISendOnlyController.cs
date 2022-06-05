using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTaskQueueContext
{
    /// <summary>
    /// 服务端 async Task 读写队列调用上下文 不返回数据（不应答客户端） 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        [CommandServerMethod(IsLowPriorityTaskQueue = true)]
        Task<CommandServerSendOnly> SendOnly(int parameter1, int parameter2);
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        Task<CommandServerSendOnly> SendOnly(int parameter);
    }
    /// <summary>
    /// 服务端 async Task 读写队列调用上下文 不返回数据（不应答客户端） 示例接口实例
    /// </summary>
    internal sealed class SendOnlyController : CommandServerTaskQueue<int>, ISendOnlyController
    {
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        public SendOnlyController(CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        async Task<CommandServerSendOnly> ISendOnlyController.SendOnly(int parameter1, int parameter2)
        {
            await Task.Yield();
            Console.WriteLine(parameter1 + parameter2);
            return null;
        }
        /// <summary>
        /// 不返回数据（不应答客户端）
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>必须是 async Task</returns>
        async Task<CommandServerSendOnly> ISendOnlyController.SendOnly(int parameter)
        {
            await Task.Yield();
            Console.WriteLine(parameter);
            return null;
        }
    }
}
