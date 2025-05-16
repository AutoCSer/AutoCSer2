using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 无返回值数据输出保持回调计数
    /// </summary>
    internal class ServerOutputReturnTypeKeepCallbackCount : ServerOutputReturnType
    {
        /// <summary>
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        private readonly CommandServerKeepCallbackCount keepCallbackCount;
        /// <summary>
        /// 无返回值数据输出保持回调计数
        /// </summary>
        /// <param name="callbackIdentity">会话标识</param>
        /// <param name="returnType">会话标识</param>
        /// <param name="keepCallbackCount">TCP 服务器端异步保持回调计数</param>
        internal ServerOutputReturnTypeKeepCallbackCount(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, CommandServerKeepCallbackCount keepCallbackCount)
            : base(callbackIdentity, returnType)
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
            var next = base.Build(socket, ref buildInfo);
            if(!object.ReferenceEquals(this, next)) keepCallbackCount.FreeCount();
            return next;
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
            keepCallbackCount.FreeCount();
            return LinkNext;
        }
    }
}
