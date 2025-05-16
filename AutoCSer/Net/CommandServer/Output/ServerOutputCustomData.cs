using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 自定义数据输出
    /// </summary>
    internal sealed class ServerOutputCustomData : ServerOutput
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 自定义数据输出
        /// </summary>
        /// <param name="data">输出参数</param>
        internal ServerOutputCustomData(ref SubArray<byte> data)
        {
            this.data = data;
        }
        /// <summary>
        /// 自定义数据输出
        /// </summary>
        /// <param name="data">输出参数</param>
        internal ServerOutputCustomData(byte[] data)
        {
            this.data.Set(data, 0, data.Length);
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
            int dataSize = (data.Length + (sizeof(int) + 3)) & (int.MaxValue), prepLength = dataSize + (sizeof(CallbackIdentity) + sizeof(int));
            if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.Data.Pointer.CurrentIndex) >= prepLength
                || (stream.Data.Pointer.FreeSize >= prepLength && prepLength > buildInfo.SendBufferSize - StreamStartIndex))
            {
                byte* write = stream.GetCanResizeBeforeMove(prepLength);
                *(CallbackIdentity*)write = new CallbackIdentity((uint)KeepCallbackCommand.CustomDataIndex | (uint)CallbackFlagsEnum.SendData);
                *(int*)(write + sizeof(CallbackIdentity)) = dataSize;
                *(int*)(write + (sizeof(CallbackIdentity) + sizeof(int))) = data.Length;
                if (data.Length != 0)
                {
                    AutoCSer.Common.CopyTo(data.Array, data.Start, write + (sizeof(CallbackIdentity) + sizeof(int) * 2), data.Length);
                }
                ++buildInfo.Count;
                return LinkNext;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
