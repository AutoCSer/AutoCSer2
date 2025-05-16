using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令控制器数据输出
    /// </summary>
    internal sealed class ServerOutputController : ServerOutput
    {
        ///// <summary>
        ///// 服务端输出信息
        ///// </summary>
        //private static readonly ServerInterfaceMethod method = new ServerInterfaceMethod();

        /// <summary>
        /// 命令控制器查询输出数据
        /// </summary>
        private CommandControllerOutputData controllerOutputData;
        /// <summary>
        /// 命令控制器数据输出
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <param name="controller">命令控制器</param>
        internal ServerOutputController(int controllerIndex, CommandServerController controller)
        {
            controllerOutputData.Set(controllerIndex, controller);
        }
        /// <summary>
        /// 创建输出信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buildInfo"></param>
        /// <returns></returns>
#if NetStandard21
        internal override unsafe ServerOutput? Build(CommandServerSocket socket, ref ServerBuildInfo buildInfo)
#else
        internal override unsafe ServerOutput Build(CommandServerSocket socket, ref ServerBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = socket.OutputSerializer.Stream;
            int streamLength = stream.SetCanResizeGetCurrentIndex(buildInfo.Count == 0);
            if (stream.MoveSize(sizeof(CallbackIdentity) + sizeof(int)))
            {
                socket.OutputSerializer.IndependentSerialize(ref controllerOutputData);
                //SimpleSerialize.Serializer<CommandControllerOutputData>.DefaultSerializer(socket.OutputSerializer.Stream, ref controllerOutputData);
                if (!stream.IsResizeError)
                {
                    int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(CallbackIdentity) + sizeof(int));
                    byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                    *(CallbackIdentity*)dataFixed = new CallbackIdentity((uint)KeepCallbackCommand.ControllerIndex | (uint)CallbackFlagsEnum.SendData);
                    *(int*)(dataFixed + sizeof(CallbackIdentity)) = dataLength;
                    ++buildInfo.Count;
                    return LinkNext;
                }
            }
            stream.Data.Pointer.CurrentIndex = streamLength;
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
