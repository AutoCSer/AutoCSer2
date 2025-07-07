using System;

namespace AutoCSer.Document.ReverseServer
{
    /// <summary>
    /// Reverse RPC server (initiating connection end)
    /// 反向 RPC 服务端（发起连接端）
    /// </summary>
    internal static class CommandReverseServer
    {
        /// <summary>
        /// Reverse RPC server singleton
        /// 反向 RPC 服务端单例
        /// </summary>
        internal static readonly AutoCSer.Net.CommandReverseClient CommandReverseClient = new AutoCSer.Net.CommandListenerBuilder(2)
            .Append<AutoCSer.CommandService.ITimestampVerifyReverseService<string>>(new AutoCSer.CommandService.TimestampVerifyReverseService<string>(AutoCSer.TestCase.Common.Config.TimestampVerifyString))
            .Append<ISymmetryService>(new SymmetryService())
            .CreateCommandListener(new AutoCSer.Net.CommandReverseClientConfig { Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document) });
    }
}
