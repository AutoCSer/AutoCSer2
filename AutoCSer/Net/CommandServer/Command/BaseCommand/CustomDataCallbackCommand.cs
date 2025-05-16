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
        /// 是否保持回调命令
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 自定义数据包回调命令
        /// </summary>
        /// <param name="socket"></param>
        internal CustomDataCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
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
