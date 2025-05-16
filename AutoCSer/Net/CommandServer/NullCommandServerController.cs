using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 默认空命令服务控制器
    /// </summary>
    internal sealed class NullCommandServerController : CommandServerController
    {
        /// <summary>
        /// 默认空命令服务控制器
        /// </summary>
        /// <param name="server"></param>
        internal NullCommandServerController(CommandListener server) : base(server) { }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override CommandClientReturnTypeEnum DoCommand(CommandServerSocket socket, ref SubArray<byte> data)
        {
            throw new InvalidOperationException();
        }
    }
}
