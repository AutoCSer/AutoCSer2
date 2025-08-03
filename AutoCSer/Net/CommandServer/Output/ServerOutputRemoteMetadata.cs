using AutoCSer.Memory;
using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据输出
    /// </summary>
    internal sealed class ServerOutputRemoteMetadata : ServerOutput
    {
        /// <summary>
        /// 远程元数据输出数据
        /// </summary>
        private RemoteMetadataOutputData outputData;
        /// <summary>
        /// 远程元数据输出
        /// </summary>
        /// <param name="metadata"></param>
        internal ServerOutputRemoteMetadata(ServerMetadata metadata)
        {
            outputData = new RemoteMetadataOutputData(metadata);
        }
        /// <summary>
        /// 远程元数据输出
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="formatDeserialize"></param>
        internal ServerOutputRemoteMetadata(ServerMetadata metadata, FormatDeserialize formatDeserialize)
        {
            outputData = new RemoteMetadataOutputData(metadata, formatDeserialize);
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
                socket.OutputSerializer.IndependentSerialize(ref outputData);
                if (!stream.IsResizeError)
                {
                    int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(CallbackIdentity) + sizeof(int));
                    byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                    *(CallbackIdentity*)dataFixed = new CallbackIdentity((uint)KeepCallbackCommand.RemoteMetadataIndex | (uint)CallbackFlagsEnum.SendData);
                    *(int*)(dataFixed + sizeof(CallbackIdentity)) = dataLength;
                    ++buildInfo.Count;
                    return LinkNext;
                }
            }
            stream.Data.Pointer.CurrentIndex = streamLength;
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 复制输出信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerOutputRemoteMetadata Clone()
        {
            ServerOutputRemoteMetadata output = (ServerOutputRemoteMetadata)MemberwiseClone();
            output.LinkNext = null;
            return output;
        }
    }
}
