using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server
{
    /// <summary>
    /// 服务认证 API 示例接口
    /// </summary>
    public interface IVerifyController
    {
        /// <summary>
        /// 服务认证 API
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerVerifyState</returns>
        Task<CommandServerVerifyState> Verify(CommandServerSocket socket, int parameter1, int parameter2);
    }
    /// <summary>
    /// 服务认证 API 示例接口实例
    /// </summary>
    internal sealed class VerifyController : IVerifyController
    {
        /// <summary>
        /// 服务认证 API
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerVerifyState</returns>
        async Task<CommandServerVerifyState> IVerifyController.Verify(CommandServerSocket socket, int parameter1, int parameter2)
        {
            await Task.Yield();
            if (parameter1 == 1 && parameter2 == 2) return CommandServerVerifyState.Success;
            return CommandServerVerifyState.Fail;
        }
    }
}
