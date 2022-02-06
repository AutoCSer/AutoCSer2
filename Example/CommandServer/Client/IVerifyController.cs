using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Client
{
    /// <summary>
    /// 服务认证 API 示例接口（客户端）
    /// </summary>
    public interface IVerifyController
    {
        /// <summary>
        /// 服务认证 API（客户端 await 等待）
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerVerifyState</returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.IVerifyController.Verify))]
        ReturnCommand<CommandServerVerifyState> VerifyAsync(int parameter1, int parameter2);
        /// <summary>
        /// 服务认证 API（客户端同步等待）
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerVerifyState</returns>
        CommandClientReturnValue<CommandServerVerifyState> Verify(int parameter1, int parameter2);
    }
}
