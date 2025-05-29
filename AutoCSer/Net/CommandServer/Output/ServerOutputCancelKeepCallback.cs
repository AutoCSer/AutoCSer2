using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 取消异步保持调用输出数据
    /// </summary>
    internal sealed class ServerOutputCancelKeepCallback : ServerOutput
    {
        ///// <summary>
        ///// 服务端输出信息
        ///// </summary>
        //private static readonly ServerInterfaceMethod method = new ServerInterfaceMethod();

        /// <summary>
        /// 取消异步保持调用数据
        /// </summary>
        private CancelKeepCallbackData cancelKeepCallbackData;
        /// <summary>
        /// 取消异步保持调用输出数据
        /// </summary>
        /// <param name="callbackIdentity">需要取消回调的会话标识</param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal ServerOutputCancelKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal ServerOutputCancelKeepCallback(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            cancelKeepCallbackData.Set(callbackIdentity, returnType, exception);
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
                SimpleSerialize.Serializer<CancelKeepCallbackData>.DefaultSerializer(socket.OutputSerializer.Stream, ref cancelKeepCallbackData);
                if (!stream.IsResizeError)
                {
                    int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(CallbackIdentity) + sizeof(int));
                    byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                    *(CallbackIdentity*)dataFixed = new CallbackIdentity((uint)KeepCallbackCommand.CancelKeepCallbackIndex | (uint)CallbackFlagsEnum.SendData);
                    *(int*)(dataFixed + sizeof(CallbackIdentity)) = dataLength;
                    ++buildInfo.Count;
                    socket.IsCancelKeepCallback = true;
                    return LinkNext;
                }
            }
            stream.Data.Pointer.CurrentIndex = streamLength;
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
