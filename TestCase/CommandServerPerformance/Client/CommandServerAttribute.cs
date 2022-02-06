using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令服务配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    internal sealed class CommandServerAttribute : AutoCSer.Net.CommandServerAttribute
    {
        /// <summary>
        /// 调用客户端验证函数
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override CommandClientReturnValue<CommandServerVerifyState> ClientVerifyMethod(CommandClientController controller)
        {
            return CommandServerVerifyState.Success;
        }
    }
}
