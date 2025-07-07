using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 合并处理命令
    /// </summary>
    internal sealed class MergeCallbackCommand : BaseCommand
    {
        /// <summary>
        /// Keep callback command returning true
        /// 保持回调命令返回 true
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 合并处理命令
        /// </summary>
        /// <param name="socket"></param>
        internal MergeCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            return Socket.MergeCallback(ref data);
        }
    }
}
