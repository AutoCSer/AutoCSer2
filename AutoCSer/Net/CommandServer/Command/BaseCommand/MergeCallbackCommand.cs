using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 合并处理命令
    /// </summary>
    internal sealed class MergeCallbackCommand : BaseCommand
    {
        /// <summary>
        /// 是否保持回调命令
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 合并处理命令
        /// </summary>
        /// <param name="socket"></param>
        internal MergeCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            return Socket.MergeCallback(ref data);
        }
    }
}
