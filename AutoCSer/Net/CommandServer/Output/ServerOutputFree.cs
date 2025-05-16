using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 返回值数据输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ServerOutputFree<T> : ServerOutput<T>
        where T : struct
    {
        /// <summary>
        /// 错误释放处理
        /// </summary>
        private readonly Action onFree;
        /// <summary>
        /// 返回值数据输出
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="method"></param>
        /// <param name="outputParameter"></param>
        /// <param name="onFree"></param>
        internal ServerOutputFree(CallbackIdentity callbackIdentity, ServerInterfaceMethod method, ref T outputParameter, Action onFree) : base(callbackIdentity, method, ref outputParameter)
        {
            this.onFree = onFree;
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
            onFree();
            return base.Free();
        }
    }
}
