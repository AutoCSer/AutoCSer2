using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 返回值数据输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ServerOutputTwoStage‌<T> : ServerOutput
    {
        /// <summary>
        /// 会话标识
        /// </summary>
        private CallbackIdentity callbackIdentity;
        /// <summary>
        /// 服务端输出信息
        /// </summary>
        private ServerInterfaceMethod method;
        /// <summary>
        /// 返回值
        /// </summary>
        private T returnValue;
        /// <summary>
        /// 返回值数据输出
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        internal ServerOutputTwoStage‌(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T returnValue)
        {
            this.callbackIdentity = callbackIdentity;
            this.method = method;
            this.returnValue = returnValue;
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
                ServerReturnValue<T> outputParameter = new ServerReturnValue<T>(returnValue);
                if (method.IsSimpleSerializeTwoStage‌ReturnValue)
                {
                    SimpleSerialize.Serializer<ServerReturnValue<T>>.DefaultSerializer(socket.OutputSerializer.Stream, ref outputParameter);
                    callbackIdentity.Index |= (uint)CallbackFlagsEnum.SendData | (uint)CallbackFlagsEnum.TwoStageCallback;
                }
                else
                {
                    socket.OutputSerializer.IndependentSerialize(ref outputParameter);
                    callbackIdentity.Index |= (uint)CallbackFlagsEnum.SendData | (uint)CallbackFlagsEnum.TwoStageCallback;
                }
                if (!stream.IsResizeError)
                {
                    int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(CallbackIdentity) + sizeof(int));
                    byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                    *(CallbackIdentity*)dataFixed = callbackIdentity;
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
