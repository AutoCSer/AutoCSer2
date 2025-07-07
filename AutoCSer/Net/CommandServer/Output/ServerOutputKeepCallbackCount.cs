using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 返回值数据输出保持回调计数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ServerOutputKeepCallbackCount<T> : ServerOutput<T>
        where T : struct
    {
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        private CommandServerKeepCallbackCount keepCallbackCount;
        /// <summary>
        /// 返回值数据输出
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="outputParameter"></param>
        /// <param name="keepCallbackCount"></param>
        internal ServerOutputKeepCallbackCount(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T outputParameter, CommandServerKeepCallbackCount keepCallbackCount)
            : base(callbackIdentity, method, ref outputParameter)
        {
            this.keepCallbackCount = keepCallbackCount;
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
                if (method.IsSimpleSerializeParamter)
                {
                    SimpleSerialize.Serializer<T>.DefaultSerializer(socket.OutputSerializer.Stream, ref outputParameter);
                    callbackIdentity.Index |= (uint)CallbackFlagsEnum.SendData;
                }
                else
                {
                    socket.OutputSerializer.IndependentSerialize(ref outputParameter);
                    callbackIdentity.Index |= (uint)CallbackFlagsEnum.SendData;
                }
                if (!stream.IsResizeError)
                {
                    outputParameter = default(T);
                    var nextBuild = LinkNext;
                    int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(CallbackIdentity) + sizeof(int));
                    byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                    *(CallbackIdentity*)dataFixed = callbackIdentity;
                    *(int*)(dataFixed + sizeof(CallbackIdentity)) = dataLength;
                    ++buildInfo.Count;
                    if (method.IsOutputPool)
                    {
                        LinkNext = null;
                        AutoCSer.Threading.LinkPool<ServerOutputKeepCallbackCount<T>, ServerOutput>.Default.Push(this);
                    }
                    keepCallbackCount.FreeCount();
                    return nextBuild;
                }
            }
            stream.Data.Pointer.CurrentIndex = streamLength;
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 释放 TCP 服务端套接字输出信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override ServerOutput? Free()
#else
        internal override ServerOutput Free()
#endif
        {
            if (method.IsOutputPool)
            {
                var next = LinkNext;
                outputParameter = default(T);
                LinkNext = null;
                AutoCSer.Threading.LinkPool<ServerOutputKeepCallbackCount<T>, ServerOutput>.Default.Push(this);
                keepCallbackCount.FreeCount();
                return next;
            }
            keepCallbackCount.FreeCount();
            return LinkNext;
        }
        /// <summary>
        /// 设置输出参数
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="outputParameter"></param>
        /// <param name="keepCallbackCount"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, T outputParameter, CommandServerKeepCallbackCount keepCallbackCount)
        {
            base.Set(callbackIdentity, method, ref outputParameter);
            this.keepCallbackCount = keepCallbackCount;
        }
    }
}
