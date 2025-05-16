using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务命令控制器查询回调命令
    /// </summary>
    internal sealed class ControllerCallbackCommand : BaseCommand
    {
        /// <summary>
        /// 是否保持回调命令
        /// </summary>
        internal override bool IsKeepCallback { get { return true; } }
        /// <summary>
        /// 服务命令控制器查询回调命令
        /// </summary>
        /// <param name="socket"></param>
        internal ControllerCallbackCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        /// <returns></returns>
        internal unsafe override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            CommandControllerOutputData controllerOutputData = default(CommandControllerOutputData);
            if (Socket.Deserialize(ref data, ref controllerOutputData, false))
            {
                Socket.ControllerCallback(ref controllerOutputData);
                return ClientReceiveErrorTypeEnum.Success;
            }
            return ClientReceiveErrorTypeEnum.ControllerDataError;
            //fixed (byte* dataFixed = data.GetFixedBuffer())
            //{
            //    byte* start = dataFixed + data.Start, end = start + data.Length;
            //    if (SimpleSerialize.Deserializer<CommandControllerOutputData>.DefaultDeserializer(start, ref controllerOutputData, end) != end)
            //    {
            //        return ClientReceiveErrorTypeEnum.ControllerDataError;
            //    }
            //}
            //Socket.ControllerCallback(ref controllerOutputData);
            //return ClientReceiveErrorTypeEnum.Success;
        }
    }
}
