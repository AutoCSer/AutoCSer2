using System;
using AutoCSer.Extensions;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 自定义数据包回调命令
    /// </summary>
    internal sealed class CustomDataCallbackCommand : BaseCommand
    {
        /// <summary>
        /// Keep callback command returning true
        /// 保持回调命令返回 true
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 自定义数据包回调命令
        /// </summary>
        /// <param name="socket"></param>
        internal CustomDataCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal unsafe override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            fixed (byte* dataFixed = data.GetFixedBuffer())
            {
                int dataSize = *(int*)(dataFixed + data.Start);
                if ((uint)(data.Length - dataSize) < 8)
                {
                    if (dataSize == 0) data.SetEmpty();
                    else data.Set(data.Start + sizeof(int), dataSize);
                }
                else
                {
                    Socket.Client.Log.ErrorIgnoreException(nameof(ClientReceiveErrorTypeEnum.CustomDataError));
                    return ClientReceiveErrorTypeEnum.CustomDataError;
                }
            }
            return Socket.Client.Config.OnCustomData(Socket, ref data);
        }
    }
}
