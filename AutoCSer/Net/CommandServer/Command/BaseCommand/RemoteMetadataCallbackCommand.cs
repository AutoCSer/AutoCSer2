using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据回调命令
    /// </summary>
    internal sealed class RemoteMetadataCallbackCommand : BaseCommand
    {
        /// <summary>
        /// Keep callback command returning true
        /// 保持回调命令返回 true
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 远程元数据回调命令
        /// </summary>
        /// <param name="socket"></param>
        internal RemoteMetadataCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal unsafe override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            RemoteMetadataOutputData remoteMetadataOutputData = default(RemoteMetadataOutputData);
            if (Socket.Deserialize(ref data, ref remoteMetadataOutputData, false))
            {
                Socket.RemoteMetadata.Callback(ref remoteMetadataOutputData);
                return ClientReceiveErrorTypeEnum.Success;
            }
            return ClientReceiveErrorTypeEnum.RemoteMetadataDataError;
        }
    }
}
