using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 取消异步保持调用回调命令
    /// </summary>
    internal sealed class CancelKeepCallbackCommand : BaseCommand
    {
        /// <summary>
        /// 是否保持回调命令
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 取消异步保持调用回调命令
        /// </summary>
        /// <param name="socket"></param>
        internal CancelKeepCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        /// <returns></returns>
        internal unsafe override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            CancelKeepCallbackData cancelKeepCallbackData = default(CancelKeepCallbackData);
            fixed (byte* dataFixed = data.GetFixedBuffer())
            {
                byte* start = dataFixed + data.Start, end = start + data.Length;
                if (SimpleSerialize.Deserializer<CancelKeepCallbackData>.DefaultDeserializer(start, ref cancelKeepCallbackData, end) != end)
                {
                    Controller.Socket.Client.Log.ErrorIgnoreException($"{typeof(CancelKeepCallbackData).fullName()} {nameof(Method.DeserializeError)}");
                    return ClientReceiveErrorTypeEnum.Success;
                }
            }
            Socket.CommandPool.CancelCallback(ref cancelKeepCallbackData);
            return ClientReceiveErrorTypeEnum.Success;
        }
    }
}
