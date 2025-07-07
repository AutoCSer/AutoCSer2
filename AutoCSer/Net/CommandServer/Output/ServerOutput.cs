using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端套接字输出信息
    /// </summary>
    internal abstract class ServerOutput : AutoCSer.Threading.Link<ServerOutput>
    {
        /// <summary>
        /// 输出流起始位置
        /// </summary>
        internal unsafe static int StreamStartIndex { get { return sizeof(CallbackIdentity) + sizeof(int); } }

        //	    29b		            1b		        1b		    1b
        //+4	CallbackIndex       JsonSerialize   SendData    Error		
        //+4	CallbackIdentity						                #数据头部8字节
        //[+4]  ErrorReturnType							                #错误回调类型 [当调用异常错误时]
        //[+4]  SendDataSize							                #发送数据字节长度4字节，负数表示数据被压缩 [当存在输入参数时存在]
        //[+4]  DataSize							                    #数据真实字节长度4字节 [当数据被压缩时存在]
        //[+n]  Data...								                    #参数数据4字节对齐

        /// <summary>
        /// 创建输出信息
        /// </summary>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="buildInfo">输出创建参数</param>
        /// <returns></returns>
#if NetStandard21
        internal abstract ServerOutput? Build(CommandServerSocket socket, ref ServerBuildInfo buildInfo);
#else
        internal abstract ServerOutput Build(CommandServerSocket socket, ref ServerBuildInfo buildInfo);
#endif
        /// <summary>
        /// 释放 TCP 服务端套接字输出信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal virtual ServerOutput? Free() { return LinkNext; }
#else
        internal virtual ServerOutput Free() { return LinkNext; }
#endif
        /// <summary>
        /// Cancel output
        /// 取消输出
        /// </summary>
        /// <param name="head"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void CancelLink(ServerOutput? head)
#else
        internal static void CancelLink(ServerOutput head)
#endif
        {
            while (head != null) head = head.Free();
        }
    }
    /// <summary>
    /// 返回值数据输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ServerOutput<T> : ServerOutput
        where T : struct
    {
        /// <summary>
        /// 会话标识
        /// </summary>
        protected CallbackIdentity callbackIdentity;
        /// <summary>
        /// 服务端输出信息
        /// </summary>
        protected ServerInterfaceMethod method;
        /// <summary>
        /// 输出参数
        /// </summary>
        protected T outputParameter;
        /// <summary>
        /// 返回值数据输出
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="outputParameter"></param>
        internal ServerOutput(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, ref T outputParameter)
        {
            this.callbackIdentity = callbackIdentity;
            this.method = method;
            this.outputParameter = outputParameter;
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
                        AutoCSer.Threading.LinkPool<ServerOutput<T>, ServerOutput>.Default.Push(this);
                    }
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
                AutoCSer.Threading.LinkPool<ServerOutput<T>, ServerOutput>.Default.Push(this);
                return next;
            }
            return LinkNext;
        }
        /// <summary>
        /// 设置输出参数
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, ref T outputParameter)
        {
            this.callbackIdentity = callbackIdentity;
            this.method = method;
            this.outputParameter = outputParameter;
        }
    }
}
