using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 无返回值数据输出
    /// </summary>
    internal class ServerOutputReturnType : ServerOutput
    {
        /// <summary>
        /// 会话标识
        /// </summary>
        internal CallbackIdentity CallbackIdentity;
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        protected readonly CommandClientReturnTypeEnum returnType;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        protected readonly string? ErrorMessage;
#else
        protected readonly string ErrorMessage;
#endif
        /// <summary>
        /// 无返回值数据输出
        /// </summary>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="returnType">会话标识</param>
        /// <param name="exception"></param>
#if NetStandard21
        internal ServerOutputReturnType(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal ServerOutputReturnType(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            this.CallbackIdentity = callbackIdentity;
            this.returnType = returnType;
            ErrorMessage = exception?.Message;
        }
        /// <summary>
        /// 无返回值数据输出
        /// </summary>
        /// <param name="callbackIdentity">Session callback identifier
        /// 会话回调标识</param>
        /// <param name="returnType">会话标识</param>
        internal ServerOutputReturnType(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType)
        {
            this.CallbackIdentity = callbackIdentity;
            this.returnType = returnType;
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
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                if ((buildInfo.SendBufferSize - stream.Data.Pointer.CurrentIndex) >= sizeof(CallbackIdentity))
                {
                    stream.Data.Pointer.Write(CallbackIdentity);
                    ++buildInfo.Count;
                    return LinkNext;
                }
            }
            else if (ErrorMessage != null)
            {
                if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.Data.Pointer.CurrentIndex) >= (((ErrorMessage.Length << 1) + (3 + (sizeof(int) * 3 + sizeof(CallbackIdentity)))) & (int.MaxValue - 3)))
                {
                    CallbackIdentity.Index |= (uint)(CallbackFlagsEnum.Error | CallbackFlagsEnum.SendData);
                    byte* write = stream.GetBeforeMove(sizeof(CallbackIdentity) + sizeof(int) * 2);
                    *(CallbackIdentity*)write = CallbackIdentity;
                    *(int*)(write + (sizeof(CallbackIdentity) + sizeof(int))) = (byte)returnType;
                    if (ErrorMessage.Length == 0) *(int*)(write + sizeof(CallbackIdentity)) = sizeof(int);
                    else
                    {
                        int startIndex = stream.Data.Pointer.CurrentIndex;
                        fixed (char* valueFixed = ErrorMessage) stream.Serialize(valueFixed, ErrorMessage.Length);
                        *(int*)(write + sizeof(CallbackIdentity)) = stream.Data.Pointer.CurrentIndex - startIndex + sizeof(int);
                    }
                    ++buildInfo.Count;
                    return LinkNext;
                }
            }
            else
            {
                if ((buildInfo.SendBufferSize - stream.Data.Pointer.CurrentIndex) >= sizeof(CallbackIdentity) + sizeof(int))
                {
                    CallbackIdentity.Index |= (uint)CallbackFlagsEnum.Error;
                    byte* write = stream.GetBeforeMove(sizeof(CallbackIdentity) + sizeof(int));
                    *(CallbackIdentity*)write = CallbackIdentity;
                    *(int*)(write + sizeof(CallbackIdentity)) = (byte)returnType;
                    ++buildInfo.Count;
                    return LinkNext;
                }
            }
            buildInfo.isFullSend = 1;
            return this;
        }
    }
}
